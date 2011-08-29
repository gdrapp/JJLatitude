<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Places.aspx.cs" Inherits="HSPI_JJLATITUDE.Places" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <asp:Literal ID="litHSHeader" runat="server"></asp:Literal>
  <title></title>
  <script type="text/javascript" src="http://maps.google.com/maps/api/js?v=3.4&sensor=true"></script>
  <script type="text/javascript">
    var geocoder;
    var map;
    var marker;

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

    addLoadEvent(function () {
      var latlng = new google.maps.LatLng(40.051193, -98.783275);
      var myOptions = {
        zoom: 4,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
      };
      map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
      geocoder = new google.maps.Geocoder();

      marker = new google.maps.Marker({
        position: latlng,
        map: map,
        draggable: true
      });

      // Try HTML5 geolocation
      if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(
          function (position) {
            var myLatLng = new google.maps.LatLng(position.coords.latitude,
                                             position.coords.longitude);


            map.setCenter(myLatLng);
          },
          function () { }
        );
      }

      google.maps.event.addListener(marker, 'position_changed', function () {
        document.getElementById("<%= txtLat.ClientID %>").value = marker.position.lat();
        document.getElementById("<%= txtLon.ClientID %>").value = marker.position.lng();
        geocoder.geocode({ 'latLng': marker.position }, function (results, status) {
          if (status == google.maps.GeocoderStatus.OK) {
            if (results[0]) {
              document.getElementById("<%= txtAddress.ClientID %>").value = results[0].formatted_address;
            }
          }
        });

      });

      google.maps.event.addListener(map, 'click', function (event) {
        marker.setPosition(event.latLng);
      });
    });
    function addressSearch() {
      var address = document.getElementById("<%= txtAddress.ClientID %>").value
      geocoder.geocode({ 'address': address }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
          if (results[0]) {
            map.setCenter(results[0].geometry.location);
            map.setZoom(13);
            marker.setPosition(results[0].geometry.location);
          }
        }
      });
    }
  </script>
  <style type="text/css">
    div.label
    {
      font-weight: bold;
      margin-bottom: 5px;
    }
    div.textbox
    {
      margin-bottom: 5px;
    }
  </style>
</head>
<body>
  <asp:Literal ID="litHSBody" runat="server"></asp:Literal>
  <br />
  <!--#include file="PluginMenu.inc"-->
  <form id="Form1" runat="server">
  <div style="margin-top:15px">
    <div id="map_canvas" style="width: 700px; height: 400px; float: left; margin-right: 10px;">
    </div>
    <div style="float: left; border: 0px solid black; padding: 10px;">
      <div class="label">
        Address:</div>
      <div class="textbox">
        <asp:TextBox ID="txtAddress" runat="server" Width="286px" TextMode="MultiLine"></asp:TextBox></div>
      <div>
        <input type="button" name="search" value="Address Search" onclick="addressSearch()" />
      </div>
      <div class="label" style="margin-top: 115px">
        Latitude:</div>
      <div class="textbox">
        <asp:TextBox ID="txtLat" runat="server"></asp:TextBox></div>
      <div class="label">
        Longitude:</div>
      <div class="textbox">
        <asp:TextBox ID="txtLon" runat="server"></asp:TextBox></div>
      <div class="label">
        Name:</div>
      <div class="textbox">
        <asp:TextBox ID="txtName" runat="server" EnableViewState="False" Width="286px"></asp:TextBox>
      </div>
      <div>
        <asp:Button ID="btnSave" runat="server" EnableViewState="False" Text="Save Location"
          OnClick="btnSave_Click" />
      </div>
    </div>
  </div>
  <div style="clear: both;">
    <asp:AccessDataSource ID="DB" runat="server" 
      DeleteCommand="DELETE FROM Places WHERE id = @id" 
      SelectCommand="SELECT * FROM Places">
      <DeleteParameters>
        <asp:ControlParameter ControlID="GridView1" Name="id" 
          PropertyName="SelectedValue" />
      </DeleteParameters>
    </asp:AccessDataSource>
  </div>
  <div style="margin-top:15px">
    <asp:GridView ID="gridPlaces" runat="server" AutoGenerateColumns="False" AutoGenerateDeleteButton="True"
      DataSourceID="DB" EnableModelValidation="True" DataKeyNames="id" 
      Width="700px">
      <Columns>
        <asp:BoundField DataField="Name" HeaderText="Name" />
        <asp:BoundField DataField="Lat" HeaderText="Latitude" />
        <asp:BoundField DataField="Lon" HeaderText="Longitude" />
      </Columns>
    </asp:GridView>
  </div>
  </form>
  <asp:Literal ID="litHSFooter" runat="server"></asp:Literal>
</body>
</html>
