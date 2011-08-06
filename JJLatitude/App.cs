using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler;

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
      //	Assign locally 
      homeSeerPI = hspi;
      HomeSeerApp = (hsapplication)homeSeerPI.GetHSIface();
    }
    #endregion

    #region "Initialization and Cleanup"
    public void Initialize()
    {
      //	Now that we have the HomeSeer object, open up the configuration
      //InitConfiguration();
    }

    public void CleanUp()
    {
      HomeSeerApp = null;
      homeSeerPI = null;
    }
    #endregion

  }
}
