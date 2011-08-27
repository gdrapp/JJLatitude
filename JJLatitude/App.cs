using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler;
using Scheduler.Classes;
using Microsoft.Win32;
using System.Threading;

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
    private Dictionary<string, string> pluginDevices = new Dictionary<string, string>();
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

      //InitDB();

      //	Now that we have the HomeSeer object, get our devices
      GetHomeSeerDevices();
      LoadAuthTokens();
      CreateDevices();
      StartLocationFinderThread();
    }
/*
    private void InitDB()
    {
      String dbLocation = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("HomeSeer Technologies").OpenSubKey("HomeSeer 2").GetValue("Installdir").ToString();
      dbLocation += "Data" + "\\" + App.PLUGIN_NAME + "\\" + App.PLUGIN_NAME + ".mdb";

      log.Debug("Initializing database connection");
      Db.Init(dbLocation);
    }
*/
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
            this.pluginDevices.Add(deviceClass.iomisc, deviceClass.hc + deviceClass.dc);
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
        //String dbLocation = Registry.LocalMachine.OpenSubKey("Software").OpenSubKey("HomeSeer Technologies").OpenSubKey("HomeSeer 2").GetValue("Installdir").ToString();
        //dbLocation += "Data" + "\\" + App.PLUGIN_NAME + "\\" + App.PLUGIN_NAME + ".mdb";
        //Db.Init(dbLocation);
        accessTokens = Db.GetAccessTokens();
        //log.Debug("Loaded auth tokens : " + String.Join(":", authTokens.Select(x => String.Format("{0}", x.Value)).ToArray()));
      }
      catch (Exception ex)
      {

      }
    }

    private void CreateDevices()
    {
      log.Debug("Creating missing plugin devices");
      log.Debug("Existing devices:" + String.Join(" ", pluginDevices.Select(x => String.Format("{0}={1}", x.Key, x.Value)).ToArray()));
      foreach (var token in accessTokens)
      {
        try
        {
          CreateDevice(String.Format("Latitude - {0}", token["name"]), "LAT:" + token["id"]);
          CreateDevice(String.Format("Longitude - {0}", token["name"]), "LON:" + token["id"]);
          CreateDevice(String.Format("Accuracy - {0}", token["name"]), "ACC:" + token["id"]);
          CreateDevice(String.Format("Map - {0}", token["name"]), "MAP:" + token["id"]);
          CreateDevice(String.Format("Last Update - {0}", token["name"]), "TIME:" + token["id"]);
        }
        catch (Exception ex)
        {

        }
      }
    }

    private void CreateDevice(string name, string iomisc)
    {
      if (!this.pluginDevices.ContainsKey(iomisc))
      {
        string newDC = GetNextFreeDeviceCode(this.houseCode);
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
          deviceClass.hc = this.houseCode;
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

    private void StartLocationFinderThread()
    {
      log.Info("Spawning Google Latitude update thread");

      try
      {
        //LatitudeThread latThread = new LatitudeThread(this.HomeSeerApp, this.pluginDevices);
        LatitudeThread latThread = new LatitudeThread();
        locationThread = new Thread(new ThreadStart(latThread.UpdaterThread));
        locationThread.Start();
      }
      catch (Exception ex)
      {

      }
    }

    public void CleanUp()
    {
      log.Info("Shutting down");

      locationThread.Abort();

      HomeSeerApp = null;
      homeSeerPI = null;
    }
    #endregion

    #region "Misc. methods"

    public void UpdateDevice(string iomisc, string status, int value)
    {
      //Update the longitude device
      if (pluginDevices.ContainsKey(iomisc))
      {
        string deviceCode = pluginDevices[iomisc];
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
