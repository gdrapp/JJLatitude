using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Scheduler;
using System.Collections;
using System.IO;

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
      return Log.GetInstance(name, null);
    }

    public static Log GetInstance(string name, hsapplication homeSeerApp)
    {
      if (classes.ContainsKey(name)) return (Log)classes[name];

      Log log = new Log(name, homeSeerApp);
      classes.Add(name, log);
      return log;
    }

    private string logClass;
    private static readonly object _syncObject = new object();

    private Log(string logClass) 
    {
      Init(logClass);
    }

    private Log(string logClass, hsapplication homeSeerApp)
    {
      if (homeSeerApp != null)
        this.HomeSeerApp = homeSeerApp;

      Init(logClass);
    }

    private void Init(string logClass)
    {
      this.logClass = logClass;
      Log.Level = LogLevel.All;
    }

    private void WriteFile(string text)
    {
      if (HomeSeerApp != null)
      {
        lock (_syncObject)
        {
          try
          {
            using (StreamWriter outfile = new StreamWriter(HomeSeerApp.GetAppPath() + "\\" + App.PLUGIN_NAME + ".log", true))
            {
              outfile.WriteLine(logClass + " - " + text);
            }
          }
          catch (Exception ex)
          {

          }
        }
      }
    }

    private void WriteHomeSeer(string text)
    {
      if (HomeSeerApp != null)
      {
        try
        {
          HomeSeerApp.WriteLog(App.PLUGIN_NAME, text);
        }
        catch (Exception ex)
        {

        }
      }
    }

    public void Fatal(string message)
    {
      if (Log.Level < LogLevel.Fatal) return;

      WriteFile(String.Format("{0}: {1}", "FATAL", message));
      WriteHomeSeer(String.Format("{0}", message));
    }

    public void Error(string message)
    {
      if (Log.Level < LogLevel.Error) return;

      WriteFile(String.Format("{0}: {1}", "ERROR", message));
      WriteHomeSeer(String.Format("{0}", message));
    }

    public void Warn(string message)
    {
      if (Log.Level < LogLevel.Warn) return;

      WriteFile(String.Format("{0}: {1}", "WARN", message));
      WriteHomeSeer(String.Format("{0}", message));
    }

    public void Info(string message)
    {
      if (Log.Level < LogLevel.Info) return;

      WriteFile(String.Format("{0}: {1}", "INFO", message));
      WriteHomeSeer(String.Format("{0}", message));
    }

    public void Debug(string message)
    {
      if (Log.Level < LogLevel.Debug) return;

      WriteFile(String.Format("{0}: {1}", "DEBUG", message));
      WriteHomeSeer(String.Format("{0}", message));
    }
  }
}