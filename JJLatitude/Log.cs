using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scheduler;
using System.Collections;

namespace HSPI_JJLATITUDE
{
  public class Log
  {
    public hsapplication HomeSeerApp {get; set;}

    public enum LogLevel { Off, Fatal, Error, Warn, Info, Debug, All }

    public static LogLevel Level { get; set; }

    private static Hashtable classes = new Hashtable();

    public static Log GetInstance(string name)
    {
      if (classes.ContainsKey(name)) return (Log)classes[name];

      Log log = new Log(name);
      classes.Add(name, log);
      return log;
    }

    private string logClass;
    private static readonly object _syncObject = new object();

    private Log(string logClass) 
    {
      this.logClass = logClass;
      Log.Level = LogLevel.All;
    }

    private static void WriteFile(string text)
    {
      lock(_syncObject) {

      }
    }

    public void Fatal(string message)
    {
      if (Log.Level < LogLevel.Fatal) return;

      if (HomeSeerApp != null)
        HomeSeerApp.WriteLog(App.PLUGIN_NAME, String.Format("{0}: {1}", "FATAL", message));
    }

    public void Error(string message)
    {
      if (Log.Level < LogLevel.Error) return;

      if (HomeSeerApp != null)
        HomeSeerApp.WriteLog(App.PLUGIN_NAME, String.Format("{0}: {1}", "ERROR", message));
    }

    public void Warn(string message)
    {
      if (Log.Level < LogLevel.Warn) return;

      if (HomeSeerApp != null)
        HomeSeerApp.WriteLog(App.PLUGIN_NAME, String.Format("{0}: {1}", "WARN", message));
    }

    public void Info(string message)
    {
      if (Log.Level < LogLevel.Info) return;

      if (HomeSeerApp != null)
        HomeSeerApp.WriteLog(App.PLUGIN_NAME, String.Format("{0}: {1}", "INFO", message));
    }

    public void Debug(string message)
    {
      if (Log.Level < LogLevel.Debug) return;

      if (HomeSeerApp != null)
        HomeSeerApp.WriteLog(App.PLUGIN_NAME, String.Format("{0}: {1}", "DEBUG", message));
    }
  }
}