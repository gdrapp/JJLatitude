using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HSPI_JJLATITUDE
{
  [Serializable]
  public class HSPI : MarshalByRefObject
  {
    #region "Members"
    public App appInstance = App.GetInstance();
    #endregion

    #region "Accessor Methods for Members"
    #endregion

    #region "Constructors"
    #endregion

    /*
        *	This section is for the shared procedures that are common
	    *	to all types of interfaces
	    */
    #region	"Common Plug-In Interface Procedures"
    public string Name()
    {
      //	Short name for this plugin
      return App.PLUGIN_NAME;
    }

    public int Capabilities()
    {
      //	OR the capabilities together to let HS know what this plug-in
      //	is capable of doing
      return HomeSeer.CA_IO;
    }

    public int AccessLevel()
    {
      //	Return the type of licensing that the plug-in uses
      return HomeSeer.AL_FREE;
    }

    public bool HSCOMPort()
    {
      //	TRUE if plug-in uses COM ports and you want to use HS pages 
      //	for configuration, FALSE otherwise
      return false;
    }

    public bool SupportsHS2()
    {
      return true;
    }

    public bool SupportsConfigDevice2()
    {
      return true;
    }

    /* HomeSeer will call this function to register a callback object
     * If events are needed (for things like IR matching, X10 events, etc.), call back
     * to HomeSeer using the passed in object.
     */
    public void RegisterCallback(ref object frm)
    {
      //	Set the global information for the HS interfaces here, now that we have them
      appInstance.SetHomeSeerCallback((Scheduler.clsHSPI)frm);
      appInstance.Initialize();
    }

    public short InterfaceStatus()
    {
      //	TODO ... get the real interface status (if I have any)
      return HomeSeer.IS_ERR_NONE;
    }
    #endregion

    /*
		 *	This section is for the procedures that are necessary for an I/O
		 *	device.  This is known as an Other type in the capabilities method.
		 */
    #region "  I/O - Other Plug-In Related Procedures  "
    public string InitIO(long port)
    {
      try
      {
        //	Register the config link here
        object linkConfig = new WebLink("/JJLatitude/Config.aspx", "JJLatitude Config", "JJLatitude Configuration");
        appInstance.HomeSeerApp.RegisterConfigLink(ref linkConfig, App.PLUGIN_NAME);
        object linkApp = new WebLink("/JJLatitude/Maps.aspx", App.PLUGIN_NAME, App.PLUGIN_NAME);
        appInstance.HomeSeerApp.RegisterLinkEx(ref linkApp, App.PLUGIN_NAME);

        //	Tell the app to go find its device
        //hspiApp.EnableHomeSeerAccess();
      }
      catch (Exception ex)
      {
        return "JJLatitude::InitIO: Failed to init: Exception: " + ex.ToString();
      }

      return "";
    }

    public void ShutdownIO()
    {
      try
      {
        //	Tell the app we're shutting down
        appInstance.CleanUp();
      }
      catch
      {
      }
    }

    public void ConfigIO()
    {
    }

    public void SetIOEx(Object dv, string housecode, string devicecode, short command, short brightness, short data1, short data2)
    {
    }
    #endregion
  }
}
