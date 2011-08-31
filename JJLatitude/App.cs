using System;
using System.Collections.Generic;
using System.Linq;
using Scheduler;
using Scheduler.Classes;
using System.Threading;

namespace HSPI_JJLATITUDE
{
  [Serializable]
  public class App
  {
    #region "Enums and Constants"
    public const string PLUGIN_NAME = "JJLatitude";
    #endregion

    #region "Members"
    static private App appInstance = null;
    static private readonly object objLock = new object();

    //private clsHSPI homeSeerPI = null;	// Interface to HomeSeer HSPI
    //public hsapplication HomeSeerApp { get; private set; }	// Interface to HomeSeer Application
    public HSPI Plugin { get; private set; }
    //public string HouseCode {get; private set;}

    //private Dictionary<string, string> pluginDevices = new Dictionary<string, string>();
    private List<Dictionary<string, string>> accessTokens;
    private Thread locationThread;

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
   /* public void SetHomeSeerCallback(clsHSPI hspi)
    {
      try
      {
        homeSeerPI = hspi;
        HomeSeerApp = (hsapplication)homeSeerPI.GetHSIface();
        log.HomeSeerApp = HomeSeerApp;
        log.Debug("HomeSeer callback set");
      }
      catch (Exception)
      {
      }
    }*/
    public void SetPluginCallback(HSPI plugin)
    {
      try
      {
        this.Plugin = plugin;
      }
      catch (Exception)
      {

      }

    }
    /*
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
    */
    #endregion

    #region "Initialization and Cleanup"
    public void Initialize()
    {
      //	Now that we have the HomeSeer object, get our devices
      // GetHomeSeerDevices();

      // Load Google OAuth tokens from DB
      LoadAuthTokens();

      // Create plugin devices based on the tokens we just loaded
      CreateDevices();

      // Start the thread to update location devices we just created/loaded
      StartLocationFinderThread();
    }
    /*
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
            this.HouseCode = deviceClass.hc;
            log.Debug("Got plugin housecode: " + this.HouseCode);
            this.pluginDevices.Add(deviceClass.iomisc, deviceClass.hc + deviceClass.dc);
          }
        }
        if (this.HouseCode == null)
        {
          this.HouseCode = ((char)homeSeerPI.GetNextFreeIOCode()).ToString();
          log.Debug("Generating new housecode for plugin: " + this.HouseCode);
        }
      }
      catch (Exception)
      {

      }
    }
    */
    private void LoadAuthTokens()
    {
      try
      {
        log.Debug("Loading authorization tokens");
        accessTokens = Db.GetAccessTokens();
      }
      catch (Exception)
      {

      }
    }

    private void CreateDevices()
    {
      log.Debug("Creating missing plugin devices");
      log.Debug("Existing devices:" + String.Join(" ", Plugin.Devices.Select(x => String.Format("{0}={1}", x.Key, x.Value)).ToArray()));
      foreach (var token in accessTokens)
      {
        try
        {
          Plugin.CreateDevice(String.Format("Latitude - {0}", token["name"]), "LAT:" + token["id"]);
          Plugin.CreateDevice(String.Format("Longitude - {0}", token["name"]), "LON:" + token["id"]);
          Plugin.CreateDevice(String.Format("Accuracy - {0}", token["name"]), "ACC:" + token["id"]);
          Plugin.CreateDevice(String.Format("Map - {0}", token["name"]), "MAP:" + token["id"]);
          Plugin.CreateDevice(String.Format("Last Update - {0}", token["name"]), "TIME:" + token["id"]);
          Plugin.CreateDevice(String.Format("Nearest Address - {0}", token["name"]), "ADDRESS:" + token["id"]);
        }
        catch (Exception)
        {

        }
      }
    }
    /*
    public void CreateDevice(string name, string iomisc)
    {
      if (!this.pluginDevices.ContainsKey(iomisc))
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
          this.pluginDevices.Add(deviceClass.iomisc, deviceClass.hc + deviceClass.dc);
        }
        else
        {
          log.Error("Could not create plugin device: device limit reached");
        }
      }

    }
    */
    private void StartLocationFinderThread()
    {
      log.Info("Spawning Google Latitude update thread");

      try
      {
        LatitudeThread latThread = new LatitudeThread();
        locationThread = new Thread(new ThreadStart(latThread.UpdaterThread));
        locationThread.Start();
      }
      catch (Exception)
      {

      }
    }

    public void CleanUp()
    {
      log.Info("Shutting down");

      locationThread.Abort();

      //HomeSeerApp = null;
      //homeSeerPI = null;
    }
    #endregion

    #region "Misc. methods"
    #endregion
  }
}
