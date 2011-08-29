using System;
using Scheduler;
using System.Collections;
using System.IO;
using System.Text;
using Microsoft.Win32;
using HSPI_JJLATITUDE.Config;

namespace HSPI_JJLATITUDE
{
  public class Log
  {
    public hsapplication HomeSeerApp {get; set;}

    public enum LogLevel { Off, Fatal, Error, Warn, Info, Debug, All }

    public static LogLevel Level { get; set; }

    private static Hashtable classes = new Hashtable();

    private string logFile = Registry.LocalMachine.OpenSubKey("Software\\HomeSeer Technologies\\HomeSeer 2").GetValue("Installdir").ToString() + "\\" + App.PLUGIN_NAME + ".log";

    private string logClass;
    private static readonly object _syncObject = new object();

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
      UpdateLogLevel();
    }

    private void UpdateLogLevel()
    {
      try
      {
        Log.Level = (LogLevel)Enum.Parse(typeof(LogLevel), AppConfig.Read("Main", "LogLevel"));
      }
      catch (Exception)
      {
        Log.Level = LogLevel.All;
      }
    }

    private void WriteFile(string text)
    {
      if (AppConfig.Read("Main", "LogToFile").Equals("true", StringComparison.CurrentCultureIgnoreCase))
      {
        lock (_syncObject)
        {
          try
          {
            using (StreamWriter outfile = new StreamWriter(logFile, true))
            {
              outfile.WriteLine(String.Format("{0} {1} {2}", DateTime.Now.ToString("o"), logClass, text));
            }
          }
          catch (Exception)
          {

          }
        }
      }
    }

    private void WriteHomeSeer(string text)
    {
      if (HomeSeerApp != null && 
        (AppConfig.Read("Main", "LogToHomeSeer").Equals("true", StringComparison.CurrentCultureIgnoreCase)))
      {
        try
        {
          HomeSeerApp.WriteLog(App.PLUGIN_NAME, text);
        }
        catch (Exception)
        {

        }
      }
    }

    public void Fatal(string message)
    {
      UpdateLogLevel();
      if (Log.Level < LogLevel.Fatal) return;

      WriteFile(String.Format("{0}: {1}", "FATAL", message));
      WriteHomeSeer(String.Format("{0}", message));
    }

    public void Error(string message)
    {
      UpdateLogLevel();
      if (Log.Level < LogLevel.Error) return;

      WriteFile(String.Format("{0}: {1}", "ERROR", message));
      WriteHomeSeer(String.Format("{0}", message));
    }

    public void Warn(string message)
    {
      UpdateLogLevel();
      if (Log.Level < LogLevel.Warn) return;

      WriteFile(String.Format("{0}: {1}", "WARN", message));
      WriteHomeSeer(String.Format("{0}", message));
    }

    public void Info(string message)
    {
      UpdateLogLevel();
      if (Log.Level < LogLevel.Info) return;

      WriteFile(String.Format("{0}: {1}", "INFO", message));
      WriteHomeSeer(String.Format("{0}", message));
    }

    public void Debug(string message)
    {
      UpdateLogLevel();
      if (Log.Level < LogLevel.Debug) return;

      WriteFile(String.Format("{0}: {1}", "DEBUG", message));
      WriteHomeSeer(String.Format("{0}", message));
    }
  }
}