<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Config.aspx.cs" Inherits="HSPI_JJLATITUDE.Web.Config" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <asp:Literal ID="litHSHeader" runat="server"></asp:Literal>
    <title></title>
    <style type="text/css">
      tr.odd {
        background-color: #FFFFFF;
      }
      tr.even 
      {
        background-color: #DFDFDF;
      }
      tr.subhead
      {
        text-align: center;
        background-color: #B6C8ED;
        font-weight: bold;
      }
    </style>
</head>
<body>

    <form id="form1" runat="server">
    <asp:Literal ID="litHSBody" runat="server"></asp:Literal>
    <br />
    
    <!--#include file="PluginMenu.inc"-->
    <br /> <br />
    <div style="width: 75%">
    
      <table style="width: 100%;">
        <tr>
          <td colspan="2" 
            style="background-color: #003399; color: #FFFFFF; font-weight: bold; font-size: small;">
            JJLatitude - Config</td>
        </tr>
        <tr class="subhead">
          <td colspan="2">
            Logging Options</td>
        </tr>
        <tr class="odd">
          <td>
            Logging Level</td>
          <td>
            <asp:DropDownList ID="lstLogLevel" runat="server" AutoPostBack="True" 
              onselectedindexchanged="lstLogLevel_SelectedIndexChanged">
              <asp:ListItem Value="0">Off</asp:ListItem>
              <asp:ListItem Value="1">Fatal</asp:ListItem>
              <asp:ListItem Value="2">Error</asp:ListItem>
              <asp:ListItem Value="3">Warn</asp:ListItem>
              <asp:ListItem Value="4">Info</asp:ListItem>
              <asp:ListItem Value="5">Debug</asp:ListItem>
              <asp:ListItem Value="6">All</asp:ListItem>
            </asp:DropDownList>
          </td>
        </tr>
        <tr class="even">
          <td>
            Log to HomeSeer</td>
          <td>
            <asp:CheckBox ID="chkLogHomeSeer" runat="server" AutoPostBack="True" 
              oncheckedchanged="chkLogHomeSeer_CheckedChanged" />
          </td>
        </tr>
        <tr class="odd">
          <td>
            Log to File</td>
          <td>
            <asp:CheckBox ID="chkLogFile" runat="server" AutoPostBack="True" 
              oncheckedchanged="chkLogFile_CheckedChanged" />
          </td>
        </tr>
        <tr class="subhead">
          <td colspan="2">
            Google Latitude Options</td>
        </tr>
        <tr class="odd">
          <td>
           Update Frequency</td>
          <td>
            <asp:DropDownList ID="lstUpdateFreq" runat="server" AutoPostBack="True" 
              onselectedindexchanged="lstUpdateFreq_SelectedIndexChanged">
              <asp:ListItem Value="60">1 minute</asp:ListItem>
              <asp:ListItem Value="300">5 minutes</asp:ListItem>
              <asp:ListItem Value="600">10 minutes</asp:ListItem>
              <asp:ListItem Value="1800">30 minutes</asp:ListItem>
              <asp:ListItem Value="3600">1 hour</asp:ListItem>
            </asp:DropDownList>
          </td>
        </tr>
        <tr class="even">
          <td>
            &nbsp;</td>
          <td>
            &nbsp;</td>
        </tr>
      </table>
    
    </div>
    </form>
    <asp:Literal ID="litHSFooter" runat="server"></asp:Literal>
</body>
</html>
