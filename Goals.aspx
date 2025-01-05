<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Goals.aspx.cs" Inherits="WaterSavaFinal.Goals" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

        <h3>My Goals</h3>
        <hr />
       <div>
    <asp:Label ID="lblCurrentDate" runat="server" CssClass="current-date"></asp:Label>
</div>
        <h4>Overall Consumption</h4>
<div>
    <asp:Label ID="lblRemainingConsumption" runat="server" CssClass="remaining-consumption"></asp:Label>
</div>
<div>
    <asp:Label ID="Label1" runat="server" CssClass="remaining-consumption"></asp:Label>
</div>

    <asp:Label ID="lblNoGoalsMessage" runat="server" Text="No goals set yet!" CssClass="alert alert-info" Visible="false"></asp:Label>

    <asp:Label ID="lblNoConsumptionMessage" runat="server" Text="No consumption data entered for today." CssClass="alert alert-info" Visible="false"></asp:Label>

   
      <asp:GridView ID="GridView1" runat="server" GridLines="None" CellPadding="10" CellSpacing="5">
    <Columns>
        <asp:BoundField DataField="Goal" HeaderText="Goal" />
        <asp:BoundField DataField="Used" HeaderText="Used" />
        <asp:BoundField DataField="Remaining" HeaderText="Remaining" />
    </Columns>
</asp:GridView>

       <asp:GridView ID="ActivityGridView" runat="server" GridLines="None" CellPadding="10" CellSpacing="5" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="Activity" HeaderText="Activities (Goal Value)" />
        <asp:BoundField DataField="WaterConsumption" HeaderText="Used" />
        <asp:TemplateField HeaderText="%">
            <ItemTemplate>
                <%# Eval("Percentage", "{0:F2}") %> %
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>


    <!-- Buttons to show Add Goal and Edit Goals panels -->
    <asp:Button ID="btnShowAddGoal" runat="server" Text="Add a New Goal" OnClick="btnShowAddGoal_Click" CssClass="btnSaveCons" />
    <asp:Button ID="btnShowEditGoals" runat="server" Text="Edit Goals" OnClick="btnShowEditGoals_Click" CssClass="btnSaveCons" />

  
        <!-- GridView to display and manage goals -->
     <asp:GridView ID="grdGoals" runat="server" GridLines="None" CellPadding="10" CellSpacing="5" AutoGenerateColumns="False" DataKeyNames="Goal_ID"
    OnRowEditing="grdGoals_RowEditing" OnRowCancelingEdit="grdGoals_RowCancelingEdit"
    OnRowUpdating="grdGoals_RowUpdating" OnRowDeleting="grdGoals_RowDeleting">
    <Columns>
        <asp:TemplateField HeaderText="Goal Description">
            <ItemTemplate>
                <asp:Label ID="lblGoalDescription" runat="server" Text='<%# Bind("Goal_Description") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtGoalDescription" runat="server" Text='<%# Bind("Goal_Description") %>'></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Goal Value (Litres)">
            <ItemTemplate>
                <asp:Label ID="lblGoalValue" runat="server" Text='<%# Bind("Goal_Value") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtGoalValue" runat="server" Text='<%# Bind("Goal_Value") %>' TextMode="Number"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:TemplateField HeaderText="Goal Date">
            <ItemTemplate>
                <asp:Label ID="lblGoalDate" runat="server" Text='<%# Bind("Goal_Date", "{0:yyyy-MM-dd}") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:TextBox ID="txtGoalDate" runat="server" Text='<%# Bind("Goal_Date", "{0:yyyy-MM-dd}") %>' TextMode="Date"></asp:TextBox>
            </EditItemTemplate>
        </asp:TemplateField>

       <asp:TemplateField HeaderText="Activity">
    <ItemTemplate>
        <asp:Label ID="lblGoalActivity" runat="server" Text='<%# Bind("Goal_Activity") %>'></asp:Label>
    </ItemTemplate>
</asp:TemplateField>


        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
    </Columns>
</asp:GridView>
        <!-- Panel to add new goals -->
        <asp:Panel ID="PanelAddGoal" runat="server" ViewStateMode="Enabled">
            <div>
                <h4>Add your goal</h4>

                <!-- Dropdown for selecting goal type -->
                <asp:Label ID="lblGoalType" runat="server" Text="Select Goal Type: "></asp:Label>
                <asp:DropDownList ID="ddlGoalType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlGoalType_SelectedIndexChanged">
                    <asp:ListItem Value="" Text="Select Goal Type" />
                    <asp:ListItem Value="Activity" Text="Set Goal for Activity" />
                    <asp:ListItem Value="OverallConsumption" Text="Set Goal for Overall Consumption" />
                </asp:DropDownList>

                <!-- Activity Goal Fields -->
                <asp:Label ID="lblGoalDescription" runat="server" Text="Enter Goal Description: " Visible="true"></asp:Label>
                <asp:TextBox ID="txtDescription" runat="server" ToolTip="Enter goal description here" Visible="true"></asp:TextBox>

                <asp:DropDownList ID="activities" runat="server" Visible="False" CssClass="form-control">
                    <asp:ListItem Text="Select Activity" Value="" /> 
                </asp:DropDownList>

                <asp:TextBox ID="txtCapacity" runat="server" TextMode="Number" Visible="true" placeholder="Goal Quantity (in litres)"></asp:TextBox>
                <asp:TextBox ID="txtDate" runat="server" TextMode="Date" Visible="true"></asp:TextBox>

                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
            </div>

            <asp:Button ID="btnAddGoals" runat="server" Text="Add" OnClick="btnAddGoals_Click"/>
        </asp:Panel>

        <asp:SqlDataSource ID="SqlDataSourceActivities" runat="server" 
            ConnectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\kwaWaterSava.accdb" 
            ProviderName="System.Data.OleDb" 
            SelectCommand="SELECT DISTINCT [Activity_Description] FROM [Activity]"></asp:SqlDataSource>
   
</asp:Content>
