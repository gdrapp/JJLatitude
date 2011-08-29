using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;

namespace HSPI_JJLATITUDE
{
  public static class Geocode
  {
    private static string BASE_URL = "https://maps.googleapis.com/maps/api/geocode/json?sensor=false&";

    public static string ToAddress(Decimal lat, Decimal lon)
    {
      ArrayList result = MakeApiCall(String.Format("{0}latlng={1},{2}", BASE_URL, lat, lon));
      
      if (result != null && result[0] != null &&
        ((Dictionary<string,object>)result[0]).ContainsKey("formatted_address"))
        return ((Dictionary<string,object>)result[0])["formatted_address"].ToString();
      else
        return "";
    }

    private static ArrayList MakeApiCall(string url)
    {
      var request = WebRequest.Create(url);
      request.Method = "GET";
      try
      {
        var response = request.GetResponse();
        using (var responseStream = response.GetResponseStream())
        {
          var reader = new StreamReader(responseStream);
          var responseText = reader.ReadToEnd();
          reader.Close();

          JavaScriptSerializer deserializer = new JavaScriptSerializer();
          Dictionary<string,object> responseDict = deserializer.Deserialize<Dictionary<string,object>>(responseText);

          if (responseDict.ContainsKey("results"))
            return (ArrayList)responseDict["results"];
          else
            return null;
        }
      }
      catch (WebException)
      {
        return null;
      }
    }
  }
}