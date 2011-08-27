using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scheduler;
using System.Threading;

namespace HSPI_JJLATITUDE
{
  public class LatitudeThread
  {
    App app = App.GetInstance();
/*    private hsapplication homeSeerApp;
    private Dictionary<string, string> pluginDevices;

    public LatitudeThread(hsapplication homeSeerApp, Dictionary<string, string> pluginDevices)
    {
      this.homeSeerApp = homeSeerApp;
      this.pluginDevices = pluginDevices;
    }*/
    public LatitudeThread() { }

    public void UpdaterThread()
    {
      while (true)
      {
        try
        {
          var locations = Latitude.UpdateLocations();
          foreach (var location in locations)
          {
            // Update the latitude device
            app.UpdateDevice("LAT:" + location.TokenID.ToString(), location.Lat.ToString());

            //Update the longitude device
            app.UpdateDevice("LON:" + location.TokenID.ToString(), location.Lon.ToString());
            app.UpdateDevice("ACC:" + location.TokenID.ToString(), String.Format("{0} feet", location.Accuracy.ToString()), location.Accuracy);
            
            // Update the static map device
            string mapUrl = "<img src='http://maps.googleapis.com/maps/api/staticmap?sensor=false&size=300x300&zoom=12&markers=color:blue|{0},{1}' />";
            app.UpdateDevice("MAP:" + location.TokenID.ToString(), String.Format(mapUrl, location.Lat, location.Lon));

            // Update the timestamp device
            app.UpdateDevice("TIME:" + location.TokenID.ToString(), location.Time.ToString());
          }
          Thread.Sleep(60000);
        }
        catch (ThreadAbortException ex)
        {

        }
        catch (Exception ex)
        {

        }
      }
    }
  }
}