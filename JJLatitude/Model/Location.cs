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
    public Decimal Lat { get; set; }
    public Decimal Lon { get; set; }
    public int Accuracy { get; set; }
    public DateTime Time { get; set; }
    
    public Location() { }

    public Location(string email, Decimal lat, Decimal lon, int accuracy, DateTime time)
    {
      this.Email = email;
      this.Lat = lat;
      this.Lon = lon;
      this.Accuracy = accuracy;
      this.Time = time;
    }

    public string MapUrl
    {
      get
      {
        return string.Format("https://maps.googleapis.com/maps/api/staticmap?markers=|{0},{1}&size={2}x{3}&sensor=false",
          this.Lat, this.Lon, 320, 320);
      }
      private set { }
    }

  }
}