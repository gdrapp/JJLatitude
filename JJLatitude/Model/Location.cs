using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSPI_JJLATITUDE.Model
{
  public class Location
  {
    public int TokenID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int Accuracy { get; set; }
    public DateTime Time { get; set; }
    public string Address { get; set; }

    public Decimal Lat { get; private set; }
    public Decimal Lon { get; private set; }

    public Location() { }

    public Location(string email, Decimal lat, Decimal lon, int accuracy, DateTime time)
    {
      this.Email = email;
      this.Lat = lat;
      this.Lon = lon;
      this.Accuracy = accuracy;
      this.Time = time;
    }

    public void LatLng(Decimal lat, Decimal lon)
    {
      if (lat != Lat || lon != Lon)
      {
        // If lat or lon changes, reverse geocode the address
        this.Address = Geocode.ToAddress(lat, lon);
      }
      Lat = lat;
      Lon = lon;
    }

    public string MapUrl
    {
      get
      {
        return string.Format("https://maps.googleapis.com/maps/api/staticmap?sensor=false&size={0}x{1}&zoom={2}&markers=color:red|{3},{4}",
          300, 300, 12, this.Lat, this.Lon);
      }
      private set { }
    }

  }
}