using System;
using System.Data.OleDb;
using Scheduler;
using System.Data;
using Matlus.FederatedIdentity;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using HSPI_JJLATITUDE.Model;
using System.Configuration;

namespace HSPI_JJLATITUDE.Web
{
  public partial class Config : System.Web.UI.Page
  {
    hsapplication homeSeerApp;
    HSPI plugin;

    protected void Page_Load(object sender, EventArgs e)
    {
      Db.Init(Server.MapPath("/App_data/jjlatitude.mdb"));

      var tokens = Db.GetAccessTokens();

      var locations = new List<Location>();
      foreach (var token in tokens)
      {
        try
        {
          string latitudeApiKey = ConfigurationManager.AppSettings["latitudeApiKey"];
          var loc = Auth.MakeApiCall("https://www.googleapis.com/latitude/v1/currentLocation?granularity=best&key=" + latitudeApiKey, token["token"], token["secret"]);
          JavaScriptSerializer deserializer = new JavaScriptSerializer();
          Dictionary<string, object> dsLoc = deserializer.Deserialize<Dictionary<string, object>>(loc);
          dsLoc = (Dictionary<string, object>)dsLoc["data"];
          Location location = new Location();
          location.Name = token["name"];
          location.Email = token["email"];
          location.Lat = (Decimal)dsLoc["latitude"];
          location.Lon = (Decimal)dsLoc["longitude"];
          location.Time = Epoch2DateTime((string)(dsLoc["timestampMs"]));
          location.Accuracy = Convert.ToInt32(dsLoc["accuracy"]);
          locations.Add(location);
          TextBox1.Text += loc;
        }
        catch (OAuthProtocolException ex)
        {

        }
      }

      GridView1.DataSource = locations;
      GridView1.DataBind();

      DataList1.DataSource = locations;
      DataList1.DataBind();

      /*try
      {
        homeSeerApp = (hsapplication)Context.Items["Content"];

        // Used for debugging in VS
        if (homeSeerApp == null)
          homeSeerApp = Global.homeSeerApp;

        if (homeSeerApp == null)
          throw new Exception("Error loading HomeSeer application object");

        plugin = (HSPI)homeSeerApp.Plugin(App.PLUGIN_NAME);

        if (plugin == null)
          throw new Exception("Error getting a reference to the plug-in.  Is it loaded and enabled?");
      }
      catch (Exception ex)
      {
        Response.Write(ex.Message + ex.StackTrace);
      }

      // Inject HomeSeer HTML
      litHSHeader.Text = GetHeadContent();
      litHSBody.Text = GetBodyContent();
      litHSFooter.Text = GetFooterContent();

      if (!IsPostBack)
      {
      }*/
    }

    private string GetHeadContent()
    {
      string header = "";

      try
      {
        header = homeSeerApp.GetPageHeader(App.PLUGIN_NAME, "", "", false, false, true, false, false);
      }
      catch
      {

      }
      return header;
    }

    private string GetBodyContent()
    {
      string header = "";

      try
      {
        header = homeSeerApp.GetPageHeader(App.PLUGIN_NAME, "", "", false, false, false, true, false);
      }
      catch
      {

      }
      return header;
    }

    private string GetFooterContent()
    {
      string footer = "";

      try
      {
        footer = homeSeerApp.GetPageFooter(false);
      }
      catch
      {

      }
      return footer;
    }

    protected void btnAddAccount_Click(object sender, EventArgs e)
    {
      string[] arrUri = Request.Url.ToString().Split('/');
      arrUri[arrUri.Length-1] = null;
      string uri = String.Join("/",arrUri);

      string callback = uri + "AuthorizeToken.aspx";
      var requestToken = Auth.MakeRequestForToken(callback);
      Session["RequestToken"] = requestToken;
      // Step 2: Make a the call to authorize the request token
      string authorizeTokenUrl = "https://www.google.com/latitude/apps/OAuthAuthorizeToken";
      Response.Redirect(authorizeTokenUrl + "?oauth_token=" + requestToken.Token + "&location=all&granularity=best&domain=" + ConfigurationManager.AppSettings["consumerKey"]);
    }

    private DateTime Epoch2DateTime(string dateTime)
    {
      DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      return epoch.AddMilliseconds(Convert.ToDouble(dateTime));
    }

  }
}