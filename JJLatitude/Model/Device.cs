using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSPI_JJLATITUDE.Model
{
  [Serializable]
  public class Device
  {
    public string Hc { get; set; }
    public string Dc { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public string Location2 { get; set; }
    public string DeviceString { get; set; }
    public string DeviceValue { get; set; }
    public string DeviceStatus { get; set; }

    // Constructors
    public Device() { }

    public Device(string hc, String dc, string name, string loc, string loc2)
    {
      this.Hc = hc;
      this.Dc = dc;
      this.Name = name;
      this.Location = loc;
      this.Location2 = loc2;
    }

    // Overrides
    public override string ToString()
    {
      return String.Format("{1} {0} {2}", Location, Location2, Name);
    }

    // Properties
    public string Id
    {
      get { return Hc + Dc; }
    }

    public string FullName
    {
      get { return this.ToString(); }
    }
  }
}