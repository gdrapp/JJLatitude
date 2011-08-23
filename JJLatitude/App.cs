using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler;
using Scheduler.Classes;
using Microsoft.Win32;

namespace HSPI_JJLATITUDE
{
  public class App
  {
    #region "Enums and Constants"
    public const string PLUGIN_NAME = "JJLatitude";
    #endregion

    #region "Members"
    static private App appInstance = null;
    static private readonly object objLock = new object();

    private clsHSPI homeSeerPI = null;	// Interface to HomeSeer HSPI
    public hsapplication HomeSeerApp { get; private set; }	// Interface to HomeSeer Application

    private string houseCode = null;
    private Dictionary<string, string> devices = new Dictionary<string, string>();
    private List<Dictionary<string, string>> authTokens;

    private Log log = Log.GetInstance("HSPI_JJLATITUDE.App");

    #endregion

    #region "Accessor Methods for Members"
    //	Accessor method to get one and only instance of this Singleton object
    static public App GetInstance()
    {
      if (appInstance == null)
        lock (objLock)
        {
          if (appInstance == null)
            appInstance = new App();
        }

      return appInstance;
    }
    #endregion

    #region "HomeSeer access methods"
    public void SetHomeSeerCallback(clsHSPI hspi)
    {
      try
      {
        homeSeerPI = hspi;
        HomeSeerApp = (hsapplication)homeSeerPI.GetHSIface();
        log.HomeSeerApp = HomeSeerApp;
      }
      catch (Exception ex)
      {
      }
    }

    private string GetNextFreeDeviceCode(string houseCode)
    {
      const int MAX_DEVICE_CODE = 99;

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
      catch (Exception ex)
      {

      }
      return null;
    }

    #endregion

    #region "Initialization and Cleanup"
    public void Initialize()
    {
      //	Now that we have the HomeSeer object, get our devices
      GetHomeSeerDevices();
      LoadAuthTokens();
      CreateDevices();
    }

    // Load all devices that belong to the plugin
    private void GetHomeSeerDevices()
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
            this.houseCode = deviceClass.hc;
            log.Debug("Got plugin housecode: " + this.houseCode);
            this.devices.Add(deviceClass.iomisc, deviceClass.hc + deviceClass.dc);
          }
        }
        if (this.houseCode == null)
        {
          this.houseCode = ((char)homeSeerPI.GetNextFreeIOCode()).ToString();
          log.Debug("Generating new housecode for plugin: " + this.houseCode);
        }
      }
      catch (Exception ex)
      {

      }
    }

    private void LoadAuthTokens()
    {
      try
      {
        log.Debug("Loading authorization tokens");
        String dbLocation = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("HomeSeer Technologies").OpenSubKey("HomeSeer 2").GetValue("Installdir").ToString();
        dbLocation += "Data" + "\\" + App.PLUGIN_NAME + "\\" + App.PLUGIN_NAME + ".mdb";
        Db.Init(dbLocation);
        authTokens = Db.GetAccessTokens();
        //log.Debug("Loaded auth tokens : " + String.Join(":", authTokens.Select(x => String.Format("{0}", x.Value)).ToArray()));
      }
      catch (Exception ex)
      {

      }
    }

    private void CreateDevices()
    {
      log.Debug("Creating missing plugin devices");
      log.Debug("Existing devices:" + String.Join(":", devices.Select(x => String.Format("{0}={1}", x.Key, x.Value)).ToArray()));
      foreach (var token in authTokens)
      {
        try
        {
          if (!devices.ContainsKey("LAT:" + token["id"]))
          {
            string newDC = GetNextFreeDeviceCode(this.houseCode);
            if (newDC != null)
            {
              log.Debug(String.Format("Creating device: Latitude - {0}", token["name"]));
              DeviceClass latDevice = HomeSeerApp.NewDeviceEx(String.Format("Latitude - {0}", token["name"]));
              latDevice.@interface = App.PLUGIN_NAME;
              latDevice.misc = HomeSeer.MISC_STATUS_ONLY;
              latDevice.iotype = HomeSeer.IOTYPE_INPUT;
              latDevice.iomisc = "LAT:" + token["id"];
              latDevice.dev_type_string = App.PLUGIN_NAME + " Plug-in";
              latDevice.hc = this.houseCode;
              latDevice.dc = newDC;
            }
            else
            {
              log.Error("Could not create plugin device: device limit reached");
            }
          }

          if (!devices.ContainsKey("LON:" + token["id"]))
          {
            string newDC = GetNextFreeDeviceCode(this.houseCode);
            if (newDC != null)
            {
              log.Debug(String.Format("Creating device: Longitude - {0}", token["name"]));
              DeviceClass lonDevice = HomeSeerApp.NewDeviceEx(String.Format("Longitude - {0}", token["name"]));
              lonDevice.@interface = App.PLUGIN_NAME;
              lonDevice.misc = HomeSeer.MISC_STATUS_ONLY;
              lonDevice.iotype = HomeSeer.IOTYPE_INPUT;
              lonDevice.iomisc = "LON:" + token["id"];
              lonDevice.dev_type_string = App.PLUGIN_NAME + " Plug-in";
              lonDevice.hc = this.houseCode;
              lonDevice.dc = newDC;
            }
            else
            {
              log.Error("Could not create plugin device: device limit reached");
            }
          }
        }
        catch (Exception ex)
        {

        }
      }
    }

    public void CleanUp()
    {
      HomeSeerApp = null;
      homeSeerPI = null;
    }
    #endregion

  }
}
