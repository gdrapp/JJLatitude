using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Script.Serialization;
using Matlus.FederatedIdentity;
using Scheduler;
using HSPI_JJLATITUDE.Model;
using System.Threading;

namespace HSPI_JJLATITUDE
{
  public static class Latitude
  {
    public const string latitudeApiKey = "AIzaSyCFtCJF1D5eT2JiV2OfyXsE9RHuG6iVKIM";

    public static List<Location> UpdateLocations()
    {
      var locations = new List<Location>();
      var accessTokens = Db.GetAccessTokens();

      foreach (var token in accessTokens)
      {
        try
        {
          var loc = Auth.MakeApiCall("https://www.googleapis.com/latitude/v1/currentLocation?granularity=best&key=" + latitudeApiKey, token["token"], token["secret"]);
          JavaScriptSerializer deserializer = new JavaScriptSerializer();
          Dictionary<string, object> dsLoc = deserializer.Deserialize<Dictionary<string, object>>(loc);
          dsLoc = (Dictionary<string, object>)dsLoc["data"];

          Location location = new Location();
          location.TokenID = Convert.ToInt32(token["id"]);
          location.Name = token["name"];
          location.Email = token["email"];
          location.Lat = (Decimal)dsLoc["latitude"];
          location.Lon = (Decimal)dsLoc["longitude"];
          location.Time = DateTime.SpecifyKind(Util.Epoch2DateTime((string)(dsLoc["timestampMs"])),DateTimeKind.Utc).ToLocalTime();
          location.Accuracy = Convert.ToInt32(dsLoc["accuracy"]);
          locations.Add(location);
        }
        catch (OAuthProtocolException ex)
        {
        
        }
        catch (Exception ex)
        {

        }

      }
      return locations;
    }


    #region "Private Members"
 
    #endregion
  }
}