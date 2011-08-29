using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Win32;

namespace HSPI_JJLATITUDE.Config
{
  public static class AppConfig
  {
    private static string iniFile = Registry.LocalMachine.OpenSubKey("Software\\HomeSeer Technologies\\HomeSeer 2").GetValue("Installdir").ToString() + "Config\\" + App.PLUGIN_NAME + ".ini";

    private static IniFile ini = new IniFile(AppConfig.iniFile);

    private static Dictionary<string, string> configCache = new Dictionary<string, string>();

    private const string SEP = "+";

    public static void Reload(string section, string key)
    {
      string value = ini.Read(section, key);
      configCache[section + SEP + key] = value;
    }

    public static void Write(string section, string key, string value)
    {
      configCache[section + SEP + key] = value;
      ini.Write(section, key, value);
    }

    public static string Read(string section, string key)
    {
      if (configCache.ContainsKey(section + SEP + key))
        return configCache[section + "+" + key];
      else
      {
        string value = ini.Read(section, key);
        configCache.Add(section + SEP + key, value);
        return value;
      }
    }

  /*  public static int LogLevel
    {
      get
      {
        int level = 0;
        string tmp = Read("Main", "LogLevel");
        if (tmp != null && (int.TryParse(tmp, out level) == true))
          return level;
        else
          return 0;
      }

      set
      {
        Write("Main", "LogLevel", value.ToString());
      }

    }*/
  }
}