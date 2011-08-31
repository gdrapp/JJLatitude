﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Distances.aspx.cs" Inherits="HSPI_JJLATITUDE.Web.Distances" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <asp:Literal ID="litHSHeader" runat="server"></asp:Literal>
  <title></title>
</head>
<body>
  <asp:Literal ID="litHSBody" runat="server"></asp:Literal>
  <br />
  <!--#include file="PluginMenu.inc"-->
  <form id="form1" runat="server">
  <div>
    <div style="float: left; margin-top: 15px;">
      <asp:ListBox ID="lstPeople" runat="server" DataSourceID="dsPeople" DataTextField="name"
        DataValueField="id" Height="112px" Width="350px"></asp:ListBox>
    </div>
    <div style="float: left; padding-left: 10px; margin-top: 15px;">
      <asp:ListBox ID="lstPlaces" runat="server" DataSourceID="dsPlaces" DataTextField="name"
        DataValueField="id" Width="350px" Height="112px"></asp:ListBox>
    </div>
    <div style="clear: both;" />
    <div style="margin-top: 15px;">
      <asp:Button ID="btnLink" runat="server" Text="Link" OnClick="btnLink_Click" />
    </div>
    <div style="margin-top: 15px;">
      <asp:GridView ID="gridDistances" runat="server" AutoGenerateColumns="False" DataSourceID="dsDistances"
        EnableModelValidation="True" Width="435px" AutoGenerateDeleteButton="True">
        <Columns>
          <asp:BoundField DataField="id" HeaderText="id" InsertVisible="False" SortExpression="id" />
          <asp:BoundField DataField="Person" HeaderText="Person" SortExpression="Person" />
          <asp:BoundField DataField="Place" HeaderText="Place" SortExpression="Place" />
        </Columns>
      </asp:GridView>
    </div>
  </div>
  <asp:AccessDataSource ID="dsPeople" runat="server" DataFile="C:\Program Files\HomeSeer HSPRO\Data\JJLatitude\jjlatitude.mdb"
    SelectCommand="SELECT id, name FROM People">
  </asp:AccessDataSource>
  <asp:AccessDataSource ID="dsPlaces" runat="server" DataFile="C:\Program Files\HomeSeer HSPRO\Data\JJLatitude\jjlatitude.mdb"
    SelectCommand="SELECT [id], [name] FROM [Places]"></asp:AccessDataSource>
  <asp:AccessDataSource ID="dsDistances" runat="server" DataFile="C:\Program Files\HomeSeer HSPRO\Data\JJLatitude\jjlatitude.mdb"
    InsertCommand="INSERT INTO Distances (peopleId, placeId) VALUES (@People, @Place)"
    
      SelectCommand="SELECT Distances.id, People.name as Person, Places.name AS Place FROM ((Distances INNER JOIN People ON Distances.peopleId = People.id) INNER JOIN Places ON Distances.placeId = Places.id)" 
      DeleteCommand="DELETE FROM Places WHERE id = @id">
    <DeleteParameters>
      <asp:ControlParameter ControlID="gridDistances" Name="id" 
        PropertyName="SelectedValue" />
    </DeleteParameters>
    <InsertParameters>
      <asp:ControlParameter ControlID="lstPeople" Name="People" PropertyName="SelectedValue" />
      <asp:ControlParameter ControlID="lstPlaces" Name="Place" PropertyName="SelectedValue" />
    </InsertParameters>
  </asp:AccessDataSource>
  <br />
  </form>
  <asp:Literal ID="litHSFooter" runat="server"></asp:Literal>
</body>
</html>
