using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Scheduler;

namespace HSPI_JJLATITUDE
{
  public static class HomeSeer
  {
    #region "HomeSeer - Constants"

    //  Plug-in capabilities
    public const int CA_X10 = 1;
    public const int CA_IR = 2;
    public const int CA_IO = 4;
    public const int CA_THERM = 16;

    //	InterfaceStatus returns
    public const short IS_ERR_NONE = 0;
    public const short IS_ERR_SEND = 1;
    public const short IS_ERR_INIT = 2;

    //	Access Levels
    public const int AL_FREE = 1;
    public const int AL_LICENSED = 2;

    //  Device MISC bit settings
    public const int MISC_PRESET_DIM = 1;		// supports preset dim if set
    public const int MISC_EXT_DIM = 2;      // extended dim command
    public const int MISC_SMART_LINC = 4;      // smart linc switch
    public const int MISC_NO_LOG = 8;      // no logging to event log for this device
    public const int MISC_STATUS_ONLY = 0x10;   // device cannot be controlled
    public const int MISC_HIDDEN = 0x20;   // device is hidden from views
    public const int MISC_THERM = 0x40;   // device is a thermostat. Copied from dev attr
    public const int MISC_INCLUDE_PF = 0x80;   // if set, device's state is restored if power fail enabled
    public const int MISC_SHOW_VALUES = 0x100;  // set=display value options in win gui and web status
    public const int MISC_AUTO_VC = 0x200;  // set=create a voice command for this device
    public const int MISC_VC_CONFIRM = 0x400;  // set=confirm voice command
    public const int MISC_COMPOSE = 0x800;  // compose protocol
    public const int MISC_ZWAVE = 0x1000; // zwave device
    public const int MISC_DIRECT_DIM = 0x2000; // Device supports direct dimming.
    // for compatibility with 1.7, the following 2 bits are 0
    // by default which disables SetDeviceStatus notify
    // and SetDeviceValue notify

    // if set, SetDeviceStatus calls plugin SetIO
    // (default is 0 or not to notify)
    public const int MISC_SETSTATUS_NOTIFY = 0x4000;
    // if set, SetDeviceValue calls plugin SetIO 
    // (default is 0 or to not notify)
    public const int MISC_SETVALUE_NOTIFY = 0x8000; 

    // if set, the device will not appear in the device status
    // change trigger list or the device conditions list.
    public const int MISC_NO_STATUS_TRIG = 0x20000;

    // Device IOTYPE property bit settings
    public const int IOTYPE_INPUT = 0;
    public const int IOTYPE_OUTPUT = 1;
    public const int IOTYPE_ANALOG_INPUT = 2;
    public const int IOTYPE_VARIABLE = 3;
    public const int IOTYPE_CONTROL = 4;		// device is a control device, no type display in device 

    #endregion

    #region "Web Related Methods"
    public static string GetHeadContent(hsapplication homeSeerApp)
    {
      string header = "";

      try
      {
        header = homeSeerApp.GetPageHeader(App.PLUGIN_NAME, "", "", false, false, true, false, false);
      }
      catch (Exception ex)
      {

      }
      return header;
    }

    public static string GetBodyContent(hsapplication homeSeerApp)
    {
      string body = "";

      try
      {
        body = homeSeerApp.GetPageHeader(App.PLUGIN_NAME, "", "", false, false, false, true, false);
      }
      catch (Exception ex)
      {

      }
      return body;
    }

    public static string GetFooterContent(hsapplication homeSeerApp)
    {
      string footer = "";

      try
      {
        footer = homeSeerApp.GetPageFooter(false);
      }
      catch (Exception ex)
      {

      }
      footer = footer.Replace("</body>", "");
      footer = footer.Replace("</html>", "");
      return footer;
    }

    #endregion
  }
}
