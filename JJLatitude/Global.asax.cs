using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Collections;

namespace HSPI_JJLATITUDE
{
  public class Global : System.Web.HttpApplication
  {
    public static Scheduler.hsapplication homeSeerApp;

    protected void Application_Start(object sender, EventArgs e)
    {
      // Code that runs on application startup
      try
      {
        IDictionary props = new Hashtable();
        Belikov.GenuineChannels.GenuineTcp.GenuineTcpChannel channel = new Belikov.GenuineChannels.GenuineTcp.GenuineTcpChannel(props, null, null);

        System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(channel, false);
        homeSeerApp = (Scheduler.hsapplication)Activator.GetObject(typeof(Scheduler.hsapplication), "gtcp://localhost:8737/hs_server.rem");
      }
      catch { }
    }

    protected void Session_Start(object sender, EventArgs e)
    {

    }

    protected void Application_BeginRequest(object sender, EventArgs e)
    {

    }

    protected void Application_AuthenticateRequest(object sender, EventArgs e)
    {

    }

    protected void Application_Error(object sender, EventArgs e)
    {

    }

    protected void Session_End(object sender, EventArgs e)
    {

    }

    protected void Application_End(object sender, EventArgs e)
    {

    }
  }
}