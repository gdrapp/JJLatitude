<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AuthorizeToken.aspx.cs" Inherits="HSPI_JJLATITUDE.Web.AuthorizeToken" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
      <asp:Panel ID="Panel1" runat="server">
        <asp:HiddenField ID="txtToken" runat="server" />
        <asp:HiddenField ID="txtVerifier" runat="server" />
        <br />
        <table style="width: 27%;">
          <tr>
            <td>
              <asp:Label ID="Label1" runat="server" Text="Name :"></asp:Label>
            </td>
            <td>
              <asp:TextBox ID="txtName" runat="server" Width="193px"></asp:TextBox>
            </td>
          </tr>
          <tr>
            <td>
              &nbsp;</td>
            <td>
              &nbsp;</td>
          </tr>
          <tr>
            <td align="center" colspan="2">
              <asp:Button ID="btnSubmit" runat="server" Text="Submit" />
            </td>
          </tr>
        </table>
      </asp:Panel>
    
    </div>
    </form>
</body>
</html>
