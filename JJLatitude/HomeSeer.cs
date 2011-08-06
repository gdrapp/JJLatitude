using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    #endregion
  }
}
