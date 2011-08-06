<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Config.aspx.cs" Inherits="HSPI_JJLATITUDE.Web.Config" EnableSessionState="True" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <asp:Literal ID="litHSHeader" runat="server"></asp:Literal>
    <style type="text/css">
      .style1
      {
        width: 97px;
      }
    </style>
</head>
<body>
    <asp:Literal ID="litHSBody" runat="server"></asp:Literal>
    <form id="form1" runat="server">
    <div style="border-width: 1px; border-style: solid;">    
      <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        Visible="False">
        <Columns>
          <asp:BoundField DataField="Email" HeaderText="Email" />
          <asp:BoundField DataField="Lat" HeaderText="Latitude" />
          <asp:BoundField DataField="Lon" HeaderText="Longitude" />
          <asp:BoundField DataField="Accuracy" HeaderText="Accuracy" />
          <asp:BoundField DataField="Time" HeaderText="Time" />
        </Columns>
      </asp:GridView>
      <br />
      <asp:DataList ID="DataList1" runat="server" BorderColor="#3366CC" 
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
          <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("MapUrl") %>' />
          <br />

          <script src="http://maps.googleapis.com/maps/api/js?sensor=false" type="text/javascript"/>
           
          <script type="text/javascript">
            var latlng = new google.maps.LatLng(34.03, -118.14); //default view to Los Angeles area
            var myOptions = {
              zoom: 13,
              center: latlng,
              mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);
          </script>
          <div ID="map_canvas" style="width: 400px; height: 400px">
          </div>
          <br />
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
