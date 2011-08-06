using System.IO;
using System.Net;
using System;
using System.Text;

namespace Matlus.FederatedIdentity
{
  public class OAuthConsumer
  {
    #region Private methods

    private string NormalizeUrl(string url)
    {
      int questionIndex = url.IndexOf('?');
      if (questionIndex == -1)
        return url;

      var parameters = url.Substring(questionIndex + 1);
      var result = new StringBuilder();
      result.Append(url.Substring(0, questionIndex + 1));

      bool hasQueryParameters = false;
      if (!String.IsNullOrEmpty(parameters))
      {
        string[] parts = parameters.Split('&');
        hasQueryParameters = parts.Length > 0;
        foreach (var part in parts)
        {
          var nameValue = part.Split('=');
          result.Append(nameValue[0] + "=");
          if (nameValue.Length == 2)
            result.Append(OAuthUtils.UrlEncode(nameValue[1]));
          result.Append("&");
        }
        if (hasQueryParameters)
          result = result.Remove(result.Length - 1, 1);
      }
      return result.ToString();
    }

    private T MakeRequest<T>(string endPoint, AuthorizeHeader authorizationHeader) where T : TokenBase, new()
    {
      var normalizedEndpoint = NormalizeUrl(endPoint);
      var request = WebRequest.Create(normalizedEndpoint);
      request.Headers.Add("Authorization", authorizationHeader.ToString());
      request.Method = "POST";
      try
      {
        var requestStream = request.GetRequestStream();
        var response = request.GetResponse();
        using (var responseStream = response.GetResponseStream())
        {
          var reader = new StreamReader(responseStream);
          var responseText = reader.ReadToEnd();
          reader.Close();
          var instanceOfT = new T();
          instanceOfT.Init(responseText);
          return instanceOfT;
        }
      }
      catch (WebException e)
      {
        using (var resp = e.Response)
        {
          using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
          {
            var errorMessage = sr.ReadToEnd();
            throw new OAuthProtocolException(errorMessage, e);            
          }
        }        
      }
    }

    #endregion Private methods

    /// <summary>
    /// Step 1 of the oAuth protocol process
    /// Make a request with the provider for a <see cref="RequestToken"/>
    /// </summary>
    /// <param name="requestTokenEndpoint">The url (as per the provider) to use for making a requet for a token</param>
    /// <param name="realm">Typically the url of Your "application" or website</param>
    /// <param name="consumerKey">The Consumer Key given to you by the provider</param>
    /// <param name="consumerSecret">The Consumer Secret given to you by the provider</param>
    /// <param name="callback">The url you'd like the provider to call you back on</param>
    /// <param name="signatureMethod">defaults to HMAC-SHA1 - the only signature method currently supported</param>
    /// <returns>An instance of a <see cref="RequestToken" /> class</returns>
    public RequestToken GetOAuthRequestToken(string requestTokenEndpoint, string realm, string consumerKey, string consumerSecret, string callback, SignatureMethod signatureMethod = SignatureMethod.HMACSHA1)
    {
      var oAuthUtils = new OAuthUtils();
      var authorizationHeader = oAuthUtils.GetRequestTokenAuthorizationHeader(requestTokenEndpoint, realm, consumerKey, consumerSecret, callback, signatureMethod);
      return MakeRequest<RequestToken>(requestTokenEndpoint, authorizationHeader);
    }

    /// <summary>
    /// Step 3 of the oAuth protocol process
    /// Make a request on the provider to Exchange a <see cref="RequesToken"/> for an <see cref="AccessToken"/>
    /// (Step 2 is a simple redirect and so there is no method for it in this class)
    /// </summary>
    /// <param name="accessTokenEndpoint">The url (as per the provider) to use for making a requet to Exchange a request token for an access token</param>
    /// <param name="realm">Typically the url of Your "application" or website</param>
    /// <param name="consumerKey">The Consumer Key given to you by the provider</param>
    /// <param name="consumerSecret">The Consumer Secret given to you by the provider</param>
    /// <param name="token">The token you got at the end of Step 1 or Step 2</param>
    /// <param name="verifier">The verifier you got at the end of step 2</param>
    /// <param name="tokenSecret">The tokenSecret you got at the end of step 1</param>
    /// <param name="signatureMethod">defaults to HMAC-SHA1 - the only signature method currently supported</param>
    /// <returns>An instance of a <see cref="AccessToken"/> class</returns>
    public AccessToken GetOAuthAccessToken(string accessTokenEndpoint, string realm, string consumerKey, string consumerSecret, string token, string verifier, string tokenSecret, SignatureMethod signatureMethod = SignatureMethod.HMACSHA1)
    {
      var oAuthUtils = new OAuthUtils();
      var authorizationHeader = oAuthUtils.GetAccessTokenAuthorizationHeader(accessTokenEndpoint, realm, consumerKey, consumerSecret, token, verifier, tokenSecret, signatureMethod);
      return MakeRequest<AccessToken>(accessTokenEndpoint, authorizationHeader);
    }


    public string GetUserInfo(string userInfoEndpoint, string realm, string consumerKey, string consumerSecret, string token, string tokenSecret, SignatureMethod signatureMethod = SignatureMethod.HMACSHA1)
    {
      var oAuthUtils = new OAuthUtils();
      var authorizationHeader = oAuthUtils.GetUserInfoAuthorizationHeader(userInfoEndpoint, realm, consumerKey, consumerSecret, token, tokenSecret, signatureMethod, "GET");

      var request = WebRequest.Create(userInfoEndpoint);
      request.Headers.Add("Authorization", authorizationHeader.ToString());
      request.Method = "GET";
      try
      {
        var response = request.GetResponse();
        using (var responseStream = response.GetResponseStream())
        {
          var reader = new StreamReader(responseStream);
          var responseText = reader.ReadToEnd();
          reader.Close();
          return responseText;
        }
      }
      catch (WebException e)
      {
        using (var resp = e.Response)
        {
          using (StreamReader sr = new StreamReader(resp.GetResponseStream()))
          {
            var errorMessage = sr.ReadToEnd();
            throw new OAuthProtocolException(errorMessage, e);
          }
        }
      }
    }
  }
}
