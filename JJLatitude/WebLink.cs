using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HSPI_JJLATITUDE
{
  [Serializable]
  public class WebLink : MarshalByRefObject
  {
    #region "Public Members - HomeSeer"
    //	Public members required by HomeSeer
    public string link;
    public string linktext;
    public string page_title;
    #endregion

    #region "Enums and Constants"
    #endregion

    public WebLink(string link, string linktext, string page_title)
    {
      this.link = link;
      this.linktext = linktext;
      this.page_title = page_title;
    }

    public string GenPage(ref string lnk)
    {
      StringBuilder response = new StringBuilder();
      response.Append("HTTP/1.1 301 Moved Permanently" + Environment.NewLine);
      response.Append("Location: " + this.link + Environment.NewLine);
      return response.ToString();
    }
  }
}
