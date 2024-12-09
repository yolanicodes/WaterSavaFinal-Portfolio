<%@ Page Title="" Language="C#" MasterPageFile="~/AdminSite.Master" AutoEventWireup="true" CodeBehind="indexAdmin.aspx.cs" Inherits="WaterSavaFinal.indexAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h1>Welcome to WaterSava</h1>
<hr /> 

<div class="filter-container">
       <%-- <label for="propertyDropdown">Select Property:</label>
        <asp:DropDownList ID="propertyDropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="propertyDropdown_SelectedIndexChanged">
            <asp:ListItem Text="ALL Properties" Value="ALL" />
            
        </asp:DropDownList>--%>
    <asp:Label runat="server" ID="lblMessage">Visit Manage Propeties to manage properties activities and residents. Visit Track properties to analyse property consumption</asp:Label>
    </div>

    <%--<div class ="chart-container">
        <asp:Chart ID="Chart1" runat="server">
            <Series>
                <asp:Series Name="Series1"></asp:Series>
            </Series>
            <ChartAreas>
                <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
            </ChartAreas>
        </asp:Chart>

    </div>--%>
</asp:Content>
