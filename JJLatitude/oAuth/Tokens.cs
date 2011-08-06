using System.Collections.Specialized;
using System.Web;

namespace Matlus.FederatedIdentity
{
  public class TokenBase
  {
    public string Token { get; protected set; }
    public string TokenSecret { get; protected set; }
    public NameValueCollection AddtionalProperties { get; private set; }

    public TokenBase()
    {
      AddtionalProperties = new NameValueCollection();
    }

    public void Init(string tokenResponse)
    {
      string[] parts = tokenResponse.Split('&');
      foreach (var part in parts)
      {
        var nameValue = part.Split('=');
        if (nameValue.Length == 2)
        {
          if (nameValue[0] == "oauth_token")
            Token = HttpUtility.UrlDecode(nameValue[1]);
          else if (nameValue[0] == "oauth_token_secret")
            TokenSecret = HttpUtility.UrlDecode(nameValue[1]);
          else
            AddtionalProperties.Add(nameValue[0], HttpUtility.UrlDecode(nameValue[1]));
        }
      }
    }
  }

  public class RequestToken : TokenBase
  {
    public bool CallbackConfirmed { get; set; }
  }

  public class AccessToken : TokenBase
  {
    public AccessToken()
    {
    }
  }
}