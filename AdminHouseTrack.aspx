<%@ Page Title="" Language="C#" MasterPageFile="AdminSite.Master" AutoEventWireup="true" CodeBehind="AdminHouseTrack.aspx.cs" Inherits="WaterSavaFinal.HouseTrack" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:Label ID="lblDebug" Text="" runat="server"></asp:Label>

        <h2>Generate Consumption Report</h2>

        <asp:Panel ID="filterPanel" runat="server">
            <h3>Select Date Range: </h3>
            <div class="input-group">
            <label for="startDate">Start Date:</label>
            <asp:TextBox ID="startDate" runat="server" CssClass="textbox"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" TargetControlID="startDate" runat="server" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>

            <label for="endDate">End Date:</label>
            <asp:TextBox ID="endDate" runat="server" CssClass="textbox"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" TargetControlID="endDate" runat="server" Format="yyyy-MM-dd"></ajaxToolkit:CalendarExtender>
                 </div>
            
            <h3>Filter by House</h3>
            <asp:DropDownList ID="housesDD" runat="server" CssClass="activityDD"></asp:DropDownList>

            <h3>Filter by Activity</h3>
            <asp:DropDownList ID="activitiesDD" runat="server" CssClass="activityDD"></asp:DropDownList>

             <h3>Filter by Residents</h3>
            <asp:DropDownList ID="residentsDD" runat="server" CssClass="activityDD"></asp:DropDownList>

            <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" OnClick="btnGenerate_Click" CssClass="btnSaveCons"/>
        </asp:Panel>

        <asp:Panel ID="summaryPanel" runat="server" CssClass="summary-panel" Visible="false">
            <asp:ListView ID="ListView1" runat="server"></asp:ListView>

            <div class="chart-container">
                <asp:Chart ID="consumptionChart" runat="server" Width="600px" Height="400px">
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                    </ChartAreas>
                    <Series>
                        <asp:Series Name="ConsumptionSeries" ChartType="Column"></asp:Series>
                    </Series>
                </asp:Chart>
            </div>
        </asp:Panel>
   
</asp:Content>
