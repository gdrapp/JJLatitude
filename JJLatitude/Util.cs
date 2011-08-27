using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HSPI_JJLATITUDE
{
  public static class Util
  {
    public static DateTime Epoch2DateTime(string dateTime)
    {
      DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
      return epoch.AddMilliseconds(Convert.ToDouble(dateTime));
    }
  }
}