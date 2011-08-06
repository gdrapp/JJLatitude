using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Matlus.FederatedIdentity;

namespace HSPI_JJLATITUDE.Web
{
  public partial class AuthorizeToken : System.Web.UI.Page
  {
    protected void Page_Load(object sender, EventArgs e)
    {
      if (IsPostBack)
      {
        string token = txtToken.Value;
        string verifier = txtVerifier.Value;

        Auth.SetTokenSecret(GetRequestToken().TokenSecret);
        var accessToken = Auth.HandleAuthorizeTokenResponse(token, verifier);
        var emailAddress = GetGoogleEmailAddress(accessToken);

        Db.PersistToken(txtName.Text, emailAddress, accessToken.Token, accessToken.TokenSecret);

        Response.Redirect("Config.aspx");
      }
      else
      {
        txtToken.Value = Request.QueryString["oauth_token"];
        txtVerifier.Value = Request.QueryString["oauth_verifier"];
      }
    }

    private RequestToken GetRequestToken()
    {
      var requestToken = (RequestToken)Session["RequestToken"];
      Session.Remove("RequestToken");
      return requestToken;
    }

    private string GetGoogleEmailAddress(AccessToken accessToken)
    {
      var userInfo = Auth.MakeApiCall("https://www.googleapis.com/userinfo/email", accessToken.Token, accessToken.TokenSecret);

      if (userInfo.IndexOf('&') != -1)
        userInfo = userInfo.Split('&')[0];

      if (userInfo.IndexOf('=') != -1)
        userInfo = userInfo.Split('=')[1];
      else
        userInfo = "";

      return userInfo;
    }
  }
}