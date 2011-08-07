<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Maps.aspx.cs" Inherits="HSPI_JJLATITUDE.Web.Maps" EnableSessionState="True" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <asp:Literal ID="litHSHeader" runat="server"></asp:Literal>
    <style type="text/css">
      .style1
      {
        width: 97px;
      }
    </style>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?sensor=false"></script>
</head>
<body>
    <asp:Literal ID="litHSBody" runat="server"></asp:Literal>
    <form id="form1" runat="server">
    <div style="border-width: 0px; border-style: solid;">    
      <br />
      <asp:DataList ID="lstLocations" runat="server" BorderColor="#3366CC" 
        BorderWidth="1px" CellPadding="4" GridLines="Both" 
        RepeatColumns="3" RepeatDirection="Horizontal" style="margin-right: 0px" 
        BackColor="White" BorderStyle="None">
        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
        <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
        <ItemStyle BackColor="White" ForeColor="#003399" />
        <ItemTemplate>
          <div align="center">
            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
          </div>
          <br />
          <div id="map_canvas<%# Container.ItemIndex %>" style="width: 400px; height: 400px">
          </div>
          <br />
          <script type="text/javascript">
            var latlng = new google.maps.LatLng(<%# Eval("Lat") %>, <%# Eval("Lon") %>); //default view to Los Angeles area
            var myOptions = {
              zoom: 13,
              center: latlng,
              mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map<%# Container.ItemIndex %> = new google.maps.Map(document.getElementById("map_canvas<%# Container.ItemIndex %>"), myOptions);
            var marker<%# Container.ItemIndex %> = new google.maps.Marker({
              position: latlng,
              map: map<%# Container.ItemIndex %>
            });
            marker<%# Container.ItemIndex %>.setTitle("<%# Eval("Name") %>")
          </script>

          <table style="width:100%;">
            <tr>
              <td class="style1">
                <asp:Label ID="Label1" runat="server" Text="Email :"></asp:Label>
              </td>
              <td>
                <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
              </td>
            </tr>
            <tr>
              <td class="style1">
                <asp:Label ID="Label2" runat="server" Text="Latitude :"></asp:Label>
              </td>
              <td>
                <asp:Label ID="lblLat" runat="server" Text='<%# Eval("Lat") %>'></asp:Label>
              </td>
            </tr>
            <tr>
              <td class="style1">
                <asp:Label ID="Label3" runat="server" Text="Longitude :"></asp:Label>
              </td>
              <td>
                <asp:Label ID="lblLon" runat="server" Text='<%# Eval("Lon") %>'></asp:Label>
              </td>
            </tr>
            <tr>
              <td class="style1">
                <asp:Label ID="Label4" runat="server" Text="Accuracy :"></asp:Label>
              </td>
              <td>
                <asp:Label ID="lblAccuracy" runat="server" 
                  Text='<%# Eval("Accuracy","{0:n0} feet") %>'></asp:Label>
              </td>
            </tr>
            <tr>
              <td class="style1">
                <asp:Label ID="Label5" runat="server" Text="Time :"></asp:Label>
              </td>
              <td>
                <asp:Label ID="lblTime" runat="server" Text='<%# Eval("Time") %>'></asp:Label>
              </td>
            </tr>
          </table>
        </ItemTemplate>
        <SelectedItemStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
      </asp:DataList>
      <br />
      <asp:Button ID="btnAddAccount" runat="server" onclick="btnAddAccount_Click" 
        Text="Add Account" />
      <br />
      <br />
      <asp:TextBox ID="TextBox1" runat="server" Height="121px" TextMode="MultiLine" 
        Width="448px" Wrap="False" Visible="False"></asp:TextBox>
    </div>
    </form>
    <asp:Literal ID="litHSFooter" runat="server"></asp:Literal>
</body>
</html>
