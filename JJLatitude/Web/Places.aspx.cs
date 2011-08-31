using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Scheduler;

namespace HSPI_JJLATITUDE
{
  public partial class Places : System.Web.UI.Page
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

          log = Log.GetInstance("HSPI_JJLATITUDE.Web.Places", homeSeerApp);

          plugin = (HSPI)homeSeerApp.Plugin(App.PLUGIN_NAME);

          if (plugin == null)
            throw new Exception("Error getting a reference to the plug-in.  Is it loaded and enabled?");
        }
        catch (Exception ex)
        {
          Response.Write(ex.Message + ex.StackTrace);
        }
        log.Debug("Loading Places web page");

        // Inject HomeSeer HTML
        litHSHeader.Text = HomeSeer.GetHeadContent(homeSeerApp);
        litHSBody.Text = HomeSeer.GetBodyContent(homeSeerApp);
        litHSFooter.Text = HomeSeer.GetFooterContent(homeSeerApp);

        txtName.Text = "";
      }  // (!IsPostBack)

      dsPlaces.DataFile = Db.DbPath;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
      //Db.PersistPlace(txtName.Text, txtLat.Text, txtLon.Text);
      dsPlaces.Insert();
    }
  }
}