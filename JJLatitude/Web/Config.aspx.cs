using System;
using Scheduler;
using HSPI_JJLATITUDE.Config;
using System.Web.UI.WebControls;

namespace HSPI_JJLATITUDE.Web
{
  public partial class Config : System.Web.UI.Page
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

          plugin = (HSPI)homeSeerApp.Plugin(App.PLUGIN_NAME);

          if (plugin == null)
            throw new Exception("Error getting a reference to the plug-in.  Is it loaded and enabled?");
        }
        catch (Exception ex)
        {
          Response.Write(ex.Message + ex.StackTrace);
        }
        log = Log.GetInstance("HSPI_JJLATITUDE.Web.Config", homeSeerApp);

      if (!IsPostBack)
        {

        log.Debug("Loading Config page");

        // Inject HomeSeer HTML
        litHSHeader.Text = HomeSeer.GetHeadContent(homeSeerApp);
        litHSBody.Text = HomeSeer.GetBodyContent(homeSeerApp);
        litHSFooter.Text = HomeSeer.GetFooterContent(homeSeerApp);

        try
        {
          lstLogLevel.SelectedValue = AppConfig.Read("Main", "LogLevel");
        }
        catch (Exception) 
        {
          log.Error("Error reading config value: LogLevel");
        }

        try
        {
          lstUpdateFreq.SelectedValue = AppConfig.Read("Main", "UpdateFrequency");
        }
        catch (Exception)
        {
          log.Error("Error reading config value: UpdateFrequency");
        }


        try
        {
          chkLogFile.Checked = Convert.ToBoolean(AppConfig.Read("Main", "LogToFile"));
        }
        catch (Exception)
        {
          log.Error("Error reading config value: LogToFile");
        }

        
        try
        {
          chkLogHomeSeer.Checked = Convert.ToBoolean(AppConfig.Read("Main", "LogToHomeSeer"));
        }
        catch (Exception)
        {
          log.Error("Error reading config value: LogToHomeSeer");
        }


      }  // (!IsPostBack)
    }

    protected void lstLogLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
      AppConfig.Write("Main", "LogLevel", ((DropDownList)sender).SelectedValue);
      plugin.ReloadConfig("Main", "LogLevel");
    }

    protected void lstUpdateFreq_SelectedIndexChanged(object sender, EventArgs e)
    {
      AppConfig.Write("Main", "UpdateFrequency", ((DropDownList)sender).SelectedValue);
      plugin.ReloadConfig("Main", "UpdateFrequency");
    }

    protected void chkLogFile_CheckedChanged(object sender, EventArgs e)
    {
      AppConfig.Write("Main", "LogToFile", ((CheckBox)sender).Checked.ToString());
      plugin.ReloadConfig("Main", "LogToFile");
    }

    protected void chkLogHomeSeer_CheckedChanged(object sender, EventArgs e)
    {
      AppConfig.Write("Main", "LogToHomeSeer", ((CheckBox)sender).Checked.ToString());
      plugin.ReloadConfig("Main", "LogToHomeSeer");
    }

  }
}