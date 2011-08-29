<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="People.aspx.cs" Inherits="HSPI_JJLATITUDE.Web.People" EnableSessionState="False" EnableViewState="false" %>

<html>
<head runat="server">
    <asp:Literal ID="litHSHeader" runat="server"></asp:Literal>
    <title></title>
    <style type="text/css">
      .style1
      {
        width: 97px;
      }
    </style>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?v=3.4&sensor=false"></script>
    <script type="text/javascript">
      var geocoder;
      function initialize() {
        geocoder = new google.maps.Geocoder();
      }
      function addLoadEvent(func) {
        var oldonload = window.onload;
        if (typeof window.onload != 'function') {
          window.onload = func;
        } else {
          window.onload = function () {
            if (oldonload) {
              oldonload();
            }
            func();
          }
        }
      }
    </script>
</head>
<body onload="initialize();">
    <asp:Literal ID="litHSBody" runat="server"></asp:Literal>
    <br />

    <!--#include file="PluginMenu.inc"-->

    <form id="form1" runat="server" enableviewstate="False">
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
          <div style="text-align:center">
            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>' 
              BackColor="#003399" Font-Bold="True" ForeColor="White" Width="100%"></asp:Label>
          </div>
          <br />
          <div id="map_canvas<%# Container.ItemIndex %>" style="width: 400px; height: 400px">
          </div>
          <br />
          <script type="text/javascript">
          addLoadEvent(function() {
            var latlng = new google.maps.LatLng(<%# Eval("Lat") %>, <%# Eval("Lon") %>);
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

            /*var infowindow<%# Container.ItemIndex %> = new google.maps.InfoWindow();

            geocoder.geocode({'latLng': latlng}, function(results, status) {
              if (status == google.maps.GeocoderStatus.OK) {
                if (results[1]) {
                  infowindow<%# Container.ItemIndex %>.setContent(results[0].formatted_address);
                  infowindow<%# Container.ItemIndex %>.open(map<%# Container.ItemIndex %>, marker<%# Container.ItemIndex %>);
                }
              }
            });*/
          });
          </script>

          <table style="width:100%;">
            <!--<tr>
              <td class="style1">
                <asp:Label ID="Label1" runat="server" Text="Email :" Font-Bold="True"></asp:Label>
              </td>
              <td>
                <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
              </td>
            </tr>-->
            <tr>
              <td class="style1">
                <asp:Label ID="Label6" runat="server" Text="Address :" Font-Bold="True"></asp:Label>
              </td>
              <td>
                <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
              </td>
            </tr>
            <tr style="background-color: #DFDFDF">
              <td class="style1">
                <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Latitude :"></asp:Label>
              </td>
              <td>
                <asp:Label ID="lblLat" runat="server" Text='<%# Eval("Lat") %>'></asp:Label>
              </td>
            </tr>
            <tr style="background-color: #FFFFFF">
              <td class="style1">
                <asp:Label ID="Label3" runat="server" Font-Bold="True" Text="Longitude :"></asp:Label>
              </td>
              <td>
                <asp:Label ID="lblLon" runat="server" Text='<%# Eval("Lon") %>'></asp:Label>
              </td>
            </tr>
            <tr style="background-color: #DFDFDF">
              <td class="style1">
                <asp:Label ID="Label4" runat="server" Text="Accuracy :" Font-Bold="True"></asp:Label>
              </td>
              <td>
                <asp:Label ID="lblAccuracy" runat="server" 
                  Text='<%# Eval("Accuracy","{0:n0} feet") %>'></asp:Label>
              </td>
            </tr>
            <tr style="background-color: #FFFFFF">
              <td class="style1">
                <asp:Label ID="Label5" runat="server" Text="Time :" Font-Bold="True"></asp:Label>
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
        Text="Add Account" EnableViewState="False" />
      <br />
      <br />
    </div>
    </form>
    <asp:Literal ID="litHSFooter" runat="server"></asp:Literal>
</body>
</html>
