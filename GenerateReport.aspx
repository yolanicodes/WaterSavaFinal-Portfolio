<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerateReport.aspx.cs" Inherits="WaterSavaFinal.GenerateReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="container">
   
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:Label ID="lblDebug" Text="" runat="server"></asp:Label>

        <h2>Generate Consumption Report</h2>

        <asp:Panel ID="filterPanel" runat="server">
            <h3>Select Date Range: </h3>
            <label for="startDate">Start Date:</label>
            <asp:TextBox ID="startDate" runat="server" CssClass="datebox"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="CalendarExtender1" TargetControlID="startDate" runat="server" Format="yyyy-MM-dd" ></ajaxToolkit:CalendarExtender>

            <label for="endDate">End Date:</label>
            <asp:TextBox ID="endDate" runat="server" CssClass="datebox"></asp:TextBox>
            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" TargetControlID="endDate" runat="server" Format="yyyy-MM-dd" ></ajaxToolkit:CalendarExtender>

            <h3>Filter by Activity</h3>
            <asp:DropDownList ID="activitiesDD" runat="server" CssClass="activityDD"></asp:DropDownList>

            <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" OnClick="btnGenerate_Click" CssClass="btnSaveCons" />
        </asp:Panel>

      <asp:Panel ID="summaryPanel" runat="server" CssClass="summary-panel" Visible="false">
            <asp:ListView ID="ListView1" runat="server"></asp:ListView>

            <div class="chart-container">
                <asp:Chart ID="consumptionChart" runat="server" Width="1000px" Height="850px" BackColor="Transparent">
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" BackColor="#DAE8ED">
                            <AxisX LabelStyle-Font="sans-serif, 12pt" LabelStyle-ForeColor="#003F67" />
                            <AxisY LabelStyle-Font="sans-serif, 12pt" LabelStyle-ForeColor="#003F67" />
                        </asp:ChartArea>
                    </ChartAreas>
                    <Series>
                        <asp:Series Name="ConsumptionSeries" ChartType="Column" Font="sans-serif, 14pt" LabelForeColor="#003F67" >
                             
                        </asp:Series>
                    </Series>
                </asp:Chart>
            </div>
        </asp:Panel>

       <asp:ListView ID="consumptionLV" runat="server" class="consumption-list">
    <ItemTemplate>
        <div class="consumptionItem">
            <span>Activity: <%# Eval("Activity_Description") %></span><br />
            <span>Date: <%# Eval("WCDate", "{0:MM/dd/yyyy}") %></span><br />
            <span>Amount Consumed: <%# Eval("Consumption") %> Liters</span>
        </div>
        <hr />
    </ItemTemplate>
    <LayoutTemplate>
        <div>
            <h3>Water Consumption Report</h3>
            <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
        </div>
    </LayoutTemplate>
</asp:ListView>


    </div>
</asp:Content>
