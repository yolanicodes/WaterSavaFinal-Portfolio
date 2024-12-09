<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ResidentHouseTrack.aspx.cs" Inherits="WaterSavaFinal.ResidentHouseTracking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

    <h2 class="title">Water Consumption By Residents</h2>
    <asp:Label ID="lblDebug" Text="" runat="server"></asp:Label>
    <asp:GridView ID="consumptionGridView" runat="server" AutoGenerateColumns="false" CssClass="gridview-style">
        <Columns>
            <asp:BoundField DataField="Resident_Username" HeaderText="Resident Name" SortExpression="Resident_Username" />
            <asp:BoundField DataField="TotalConsumption" HeaderText="Total Consumption (Liters)" SortExpression="TotalConsumption" />
        </Columns>
    </asp:GridView>
</asp:Content>
