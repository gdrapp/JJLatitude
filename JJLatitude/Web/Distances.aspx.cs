using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Scheduler;

namespace HSPI_JJLATITUDE.Web
{
  public partial class Distances : System.Web.UI.Page
  {
    private hsapplication homeSeerApp;
    private HSPI plugin;
    private Log log;

    protected void Page_Load(object sender, EventArgs e)
    {
      try
      {
        homeSeerApp = (hsapplication)Context.Items["Content"];

        // Used for debugging in VS
        if (homeSeerApp == null)
          homeSeerApp = Global.homeSeerApp;

        if (homeSeerApp == null)
          throw new Exception("Error loading HomeSeer application object");

        log = Log.GetInstance("HSPI_JJLATITUDE.Web.Distances", homeSeerApp);

        plugin = (HSPI)homeSeerApp.Plugin(App.PLUGIN_NAME);

        if (plugin == null)
          throw new Exception("Error getting a reference to the plug-in.  Is it loaded and enabled?");
      }
      catch (Exception ex)
      {
        Response.Write(ex.Message + ex.StackTrace);
      }
      log.Debug("Loading Distances web page");

      // Inject HomeSeer HTML
      litHSHeader.Text = HomeSeer.GetHeadContent(homeSeerApp);
      litHSBody.Text = HomeSeer.GetBodyContent(homeSeerApp);
      litHSFooter.Text = HomeSeer.GetFooterContent(homeSeerApp);
      
      dsPeople.DataFile = Db.DbPath;
      dsPlaces.DataFile = Db.DbPath;
      dsDistances.DataFile = Db.DbPath;

      if (!IsPostBack)
      {

      }

    }

    protected void btnLink_Click(object sender, EventArgs e)
    {
      string name = String.Format("Distance - {0} & {1}", lstPeople.SelectedItem.Text, lstPlaces.SelectedItem.Text);
      string iomisc = String.Format("DIST:{0},{1}", lstPeople.SelectedValue, lstPlaces.SelectedValue);
      //plugin.AppInstance.CreateDevice(name, iomisc);
      plugin.CreateDevice(name, iomisc);
      dsDistances.Insert();
    }
  }
}