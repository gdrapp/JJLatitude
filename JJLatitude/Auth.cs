using Matlus.FederatedIdentity;
using System.Net;
using System.IO;
using System.Configuration;

namespace HSPI_JJLATITUDE
{
  public static class Auth
  {
    private static readonly string consumerKey = ConfigurationManager.AppSettings["consumerKey"];
    private static readonly string consumerSecret = ConfigurationManager.AppSettings["consumerSecret"];

    private static string tokenSecret = null;

    public static AccessToken HandleAuthorizeTokenResponse(string token, string verifier)
    {      
      string accessTokenEndpoint = "https://www.google.com/accounts/OAuthGetAccessToken";
      var oAuthConsumer = new OAuthConsumer();
      return oAuthConsumer.GetOAuthAccessToken(accessTokenEndpoint, null, consumerKey, consumerSecret, token, verifier, tokenSecret);
    }

    public static RequestToken MakeRequestForToken(string callback)
    {
      string requestTokenEndpoint = "https://www.google.com/accounts/OAuthGetRequestToken?scope=https://www.googleapis.com/auth/latitude https://www.googleapis.com/auth/userinfo.email&xoauth_displayname=JJLatitude";
      string requestTokenCallback = callback;
      
      var oAuthConsumer = new OAuthConsumer();
      var requestToken = oAuthConsumer.GetOAuthRequestToken(requestTokenEndpoint, null, consumerKey, consumerSecret, requestTokenCallback);
      
      return requestToken;
    }

    public static void SetTokenSecret(string tokenSecret)
    {
      Auth.tokenSecret = tokenSecret;
    }

    public static string MakeApiCall(string url, string token, string tokenSecret)
    {
      var oAuthUtils = new OAuthUtils();
      var authorizationHeader = oAuthUtils.GetUserInfoAuthorizationHeader(url, null, consumerKey, consumerSecret, token, tokenSecret, SignatureMethod.HMACSHA1, "GET");

      var request = WebRequest.Create(url);
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