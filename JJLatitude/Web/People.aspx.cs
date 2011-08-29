using System;
using System.Data.OleDb;
using Scheduler;
using System.Data;
using Matlus.FederatedIdentity;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using HSPI_JJLATITUDE.Model;
using System.Configuration;
using System.Web.UI.WebControls;
using Microsoft.Win32;

namespace HSPI_JJLATITUDE.Web
{
  public partial class People : System.Web.UI.Page
  {
    private hsapplication homeSeerApp;
    private HSPI plugin;
    private Log log;

    protected void Page_Load(object sender, EventArgs e)
    {
      if (!IsPostBack)
      {
        try
        {
          homeSeerApp = (hsapplication)Context.Items["Content"];

          // Used for debugging in VS
          if (homeSeerApp == null)
            homeSeerApp = Global.homeSeerApp;

          if (homeSeerApp == null)
            throw new Exception("Error loading HomeSeer application object");

          log = Log.GetInstance("HSPI_JJLATITUDE.Web.People", homeSeerApp);

          plugin = (HSPI)homeSeerApp.Plugin(App.PLUGIN_NAME);

          if (plugin == null)
            throw new Exception("Error getting a reference to the plug-in.  Is it loaded and enabled?");
        }
        catch (Exception ex)
        {
          Response.Write(ex.Message + ex.StackTrace);
        }
        log.Debug("Loading People page");

        lstLocations.DataSource = Latitude.UpdateLocations();
        lstLocations.DataBind();

        // Inject HomeSeer HTML
        litHSHeader.Text = HomeSeer.GetHeadContent(homeSeerApp);
        litHSBody.Text = HomeSeer.GetBodyContent(homeSeerApp);
        litHSFooter.Text = HomeSeer.GetFooterContent(homeSeerApp);
      }  // (!IsPostBack)
    }

    protected void btnAddAccount_Click(object sender, EventArgs e)
    {
      string[] arrUri = Request.Url.ToString().Split('/');
      arrUri[arrUri.Length - 1] = null;
      string uri = String.Join("/", arrUri);

      string callback = uri + "AuthorizeToken.aspx";
      var requestToken = Auth.MakeRequestForToken(callback);
      Session["RequestToken"] = requestToken;
      // Step 2: Make a the call to authorize the request token
      string authorizeTokenUrl = "https://www.google.com/latitude/apps/OAuthAuthorizeToken";
      Response.Redirect(authorizeTokenUrl + "?oauth_token=" + requestToken.Token + "&location=all&granularity=best&domain=" + ConfigurationManager.AppSettings["consumerKey"]);
    }
  }
}