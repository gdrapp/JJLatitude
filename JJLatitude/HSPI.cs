using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HSPI_JJLATITUDE.Config;
using Scheduler.Classes;
using Scheduler;

namespace HSPI_JJLATITUDE
{
  [Serializable]
  public class HSPI : MarshalByRefObject
  {
    #region "Public Members"
    public App AppInstance = App.GetInstance();
    public hsapplication HomeSeerApp { get; private set; }	// Interface to HomeSeer Application
    public clsHSPI HomeSeerPI { get; private set; }
    public string HouseCode { get; private set; }
    public Dictionary<string, string> Devices { get; private set; }
    #endregion

    #region "Private Members"
    private Log log = Log.GetInstance("HSPI_JJLATITUDE.HSPI");
    #endregion

    #region "Accessor Methods for Members"
    #endregion

    #region "Constructors"
    public HSPI()
    {
      Devices = new Dictionary<string, string>();
    }
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
      HomeSeerPI = (clsHSPI)frm;
      HomeSeerApp = (hsapplication)HomeSeerPI.GetHSIface();
      log.HomeSeerApp = HomeSeerApp;

      GetPluginDevices();

      //AppInstance.SetHomeSeerCallback((Scheduler.clsHSPI)frm);
      AppInstance.SetPluginCallback(this);
      AppInstance.Initialize();
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
        object linkConfig = new WebLink("/JJLatitude/Config.aspx", App.PLUGIN_NAME + " Config", App.PLUGIN_NAME + " Configuration");
        HomeSeerApp.RegisterConfigLink(ref linkConfig, App.PLUGIN_NAME);

        object linkApp = new WebLink("/JJLatitude/People.aspx", App.PLUGIN_NAME, App.PLUGIN_NAME);
        HomeSeerApp.RegisterLinkEx(ref linkApp, App.PLUGIN_NAME);
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
        AppInstance.CleanUp();
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

    #region "HomeSeer Access Methods"
    public void ReloadConfig(string section, string key)
    {
      AppConfig.Reload(section, key);
    }

    public void CreateDevice(string name, string iomisc)
    {
      if (!this.Devices.ContainsKey(iomisc))
      {
        string newDC = GetNextFreeDeviceCode(this.HouseCode);
        if (newDC != null)
        {
          log.Debug(String.Format("Creating device: {0}", name));
          DeviceClass deviceClass = HomeSeerApp.NewDeviceEx(name);
          deviceClass.@interface = App.PLUGIN_NAME;
          deviceClass.location = App.PLUGIN_NAME;
          deviceClass.misc = HomeSeer.MISC_STATUS_ONLY;
          deviceClass.iotype = HomeSeer.IOTYPE_INPUT;
          deviceClass.iomisc = iomisc;
          deviceClass.dev_type_string = App.PLUGIN_NAME + " Plug-in";
          deviceClass.hc = this.HouseCode;
          deviceClass.dc = newDC;

          // Add new device to list of devices owned by the plugin
          Devices.Add(deviceClass.iomisc, deviceClass.hc + deviceClass.dc);
        }
        else
        {
          log.Error("Could not create plugin device: device limit reached");
        }
      }
    }

    public string GetNextFreeDeviceCode(string houseCode)
    {
      const int MAX_DEVICE_CODE = 99;

      log.Debug("Getting unused plugin device code");

      if (houseCode == null || houseCode.Length != 1)
        return null;

      try
      {
        for (int i = 1; i <= MAX_DEVICE_CODE; i++)
        {
          if (HomeSeerApp.DeviceExists(houseCode + i.ToString()) == -1)
            return i.ToString();
        }
      }
      catch (Exception)
      {
        log.Warn("Error finding unused plugin device code");
      }
      return null;
    }

    // Load all devices that belong to the plugin
    private void GetPluginDevices()
    {
      log.Debug("Enumerating HomeSeer devices owned by plugin");
      try
      {
        clsDeviceEnumeration enumerator = (clsDeviceEnumeration)HomeSeerApp.GetDeviceEnumerator();
        DeviceClass deviceClass;

        while (!enumerator.Finished)
        {
          deviceClass = enumerator.GetNext();
          if (deviceClass.@interface.Equals(App.PLUGIN_NAME))
          {
            log.Debug(String.Format("Found device: DC={0}{1} NAME={2} IOMISC={3}", deviceClass.hc, deviceClass.dc, deviceClass.Name, deviceClass.iomisc));
            if (this.HouseCode == null)
            {
              this.HouseCode = deviceClass.hc;
              log.Debug("Got plugin housecode: " + this.HouseCode);
            }
            Devices.Add(deviceClass.iomisc, deviceClass.hc + deviceClass.dc);
          }
        }
        if (this.HouseCode == null)
        {
          this.HouseCode = ((char)HomeSeerPI.GetNextFreeIOCode()).ToString();
          log.Debug("Generating new housecode for plugin: " + this.HouseCode);
        }
      }
      catch (Exception)
      {

      }
    }

    public void UpdateDevice(string iomisc, string status, int value)
    {
      //Update the longitude device
      if (this.Devices.ContainsKey(iomisc))
      {
        string deviceCode = this.Devices[iomisc];
        if (HomeSeerApp.DeviceExists(deviceCode) != -1)
          lock (this)
          {
            HomeSeerApp.SetDeviceString(deviceCode, status, true);
            HomeSeerApp.SetDeviceValue(deviceCode, value);
          }
      }
    }

    public void UpdateDevice(string iomisc, string status)
    {
      UpdateDevice(iomisc, status, 0);
    }

    #endregion

  }
}
