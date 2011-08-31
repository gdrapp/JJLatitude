using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scheduler;
using System.Threading;
using HSPI_JJLATITUDE.Config;

namespace HSPI_JJLATITUDE
{
  public class LatitudeThread
  {
    private App app = App.GetInstance();
    private Log log;

    public LatitudeThread() 
    {
      log = Log.GetInstance("HSPI_JJLATITUDE.LatitudeThread", app.Plugin.HomeSeerApp);
    }

    public void UpdaterThread()
    {
      log.Debug("Entering update thread loop");
      while (true)
      {
        try
        {
          log.Debug("Getting update locations");
          var locations = Latitude.UpdateLocations();
          foreach (var location in locations)
          {
            log.Debug(String.Format("Updating plugin devices for [{0}]", location.Name));

            // Update the latitude device
            log.Debug("Updating latitude device");
            app.Plugin.UpdateDevice("LAT:" + location.TokenID.ToString(), location.Lat.ToString());

            //Update the longitude device
            log.Debug("Updating longitude plugin device");
            app.Plugin.UpdateDevice("LON:" + location.TokenID.ToString(), location.Lon.ToString());
            
            // Update the accuracy device
            log.Debug("Updating accuracy plugin device");
            app.Plugin.UpdateDevice("ACC:" + location.TokenID.ToString(), String.Format("{0} feet", location.Accuracy.ToString()), location.Accuracy);
            
            // Update the static map device
            log.Debug("Updating map plugin device");
            app.Plugin.UpdateDevice("MAP:" + location.TokenID.ToString(), String.Format("<img src='{0}' />", location.MapUrl));

            // Update the timestamp device
            log.Debug("Updating timestamp plugin device");
            app.Plugin.UpdateDevice("TIME:" + location.TokenID.ToString(), location.Time.ToString());

            // Update the nearest address device
            log.Debug("Updating nearest address plugin device");
            app.Plugin.UpdateDevice("ADDRESS:" + location.TokenID.ToString(), location.Address);
          }

          try
          {
            string updateFreq = AppConfig.Read("Main", "UpdateFrequency");
            int sleepSecs = Convert.ToInt32(updateFreq);
            log.Debug(String.Format("Sleeping for {0} seconds", sleepSecs));
            Thread.Sleep(sleepSecs * 1000);
          }
          catch (ThreadAbortException)
          {
            log.Debug("Shutting down location update thread");
          }          
          catch (Exception)
          {
            // If we error out reading the value from the config above, default to 300 seconds
            log.Debug(String.Format("Error reading update frequency, sleeping for 300 seconds"));
            Thread.Sleep(300 * 1000);
          }
        }
        catch (Exception)
        {

        }
      }
    }
  }
}