<%@ Page Title="" Language="C#" MasterPageFile="~/AdminSite.Master" AutoEventWireup="true" CodeBehind="ManageProperties.aspx.cs" Inherits="WaterSavaFinal.ManageProperties" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblDebug" runat="server" ForeColor="Red"></asp:Label>
   
    <h3>My Properties</h3>
    <asp:GridView ID="listProperties" runat="server" AutoGenerateColumns="False"
        DataKeyNames="Property_ID"
        OnRowEditing="listProperties_RowEditing"
        OnRowDeleting="listProperties_RowDeleting"
            OnSelectedIndexChanged="listProperties_SelectedIndexChanged">

        <Columns>
           
            <asp:BoundField DataField="Property_HouseNum" HeaderText="Property House Number" SortExpression="Property_HouseNum" />
            <asp:BoundField DataField="Property_StreetName" HeaderText="Street Name" SortExpression="Property_StreetName" />
            <asp:BoundField DataField="Property_Suburb" HeaderText="Suburb" SortExpression="Property_Suburb" />
            <asp:BoundField DataField="Property_City" HeaderText="City" SortExpression="Property_City" />
             <asp:CommandField ShowSelectButton="True"/>
            <asp:CommandField ShowEditButton="true" />
            <asp:ButtonField Text="Delete" CommandName="Delete" />

        </Columns>
    </asp:GridView>
        <asp:Button ID="btnAddProperty" runat="server" Text="Add property" OnClick="btnAddProperty_Click"/>

    <asp:Panel ID="PropertyPanel" runat="server" Visible="false">
         <hr />
         <h3>Add House Details</h3>
         <hr />
         
         <div class="houseNo">
             <label for="houseNo">House Number:</label>
             <asp:Textbox runat="server" id="houseNo" type="text" name="houseNo" required="required"/>
        </div>
          <div class="houseStreet">
             <label for="houseNo"> Street Name:</label>
             <asp:Textbox runat="server" id="houseStreet" type="text" name="houseStreet" required="required"/>
        </div>
          <div class="houseNSub">
             <label for="houseSub">Suburb:</label>
             <asp:Textbox runat="server" id="houseSub" type="text" name="houseSub" required="required"/>
        </div>
          <div class="houseCity">
             <label for="houseCity">City:</label>
             <asp:Textbox runat="server" id="houseCity" type="text" name="houseCity" required="required"/>
        </div>
          <div class="houseProv">
             <label for="houseProv">Province:</label>
             <asp:Textbox runat="server" id="houseProv" type="text" name="houseProv" required="required"/>
        </div>
          <div class="houseCode">
             <label for="houseCode">Postal code:</label>
             <asp:Textbox runat="server" id="houseCode" type="text" name="houseCode" required="required"/>
        </div>
             
         <div class="actionButtons">
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
            <asp:Button ID="btnSave_Property" runat="server" Text="Continue" OnClick="btnSave_Property_Click"/>
        </div>
    </asp:Panel>
    <asp:Panel ID="ResidentPanel" runat="server" Visible="false">
         <hr />
         <h3>Add Resident</h3> <%--you must be able to add multiple residents in this page. Form must refresh--%>
         <hr />
          <div class="FnameField">
             <label for="firstName">First Name:</label>
             <asp:Textbox runat="server" id="firstName" type="text" name="firstName" required="required"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="firstName" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="LnameField">
             <label for="LastName">Last Name:</label>
             <asp:Textbox runat="server" id="LastName" type="text" name="firstName" required="required"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="LastName" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="emailField">
             <label for="Email">Email:</label>
             <asp:Textbox runat="server" id="Email" type="email" name="Email" required="required"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="Email" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="ContactField">
             <label for="Contact">Contact:</label>
             <asp:Textbox runat="server" id="Contact" type="text" name="Contact" required="required"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="Contact" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
        <div class="TitleField">
            <label for="titleButtons">Title:</label>
            <div class="titleButtons">
                &nbsp;<asp:RadioButton runat="server" ID="rbtnMr" GroupName="Title" Text="Mr" />
                &nbsp;<asp:RadioButton runat="server" ID="rbtnMrs" GroupName="Title" Text="Mrs"  />
                &nbsp;<asp:RadioButton runat="server" ID="rbtnMs" GroupName="Title" Text="Ms" />
                &nbsp;<asp:RadioButton runat="server" ID="rbtnMiss" GroupName="Title" Text="Miss" />
            </div>
        </div>
    <asp:Button ID="btnSaveResident" runat="server" Text="Save Resident" OnClientClick="Save_Resident_Info;" />
           <asp:Button ID="saveRes" runat="server" Text="Done" OnClick="SaveFinal_Resident_Info" /> <%--after saving, display options to either redirect to the  my properties page or add another resident--%>
   

    <script type="text/javascript">

</script>
  </asp:Panel>
    <asp:Panel ID="activityPanel" runat="server" Visible="false">
    <h3>Add New Activity</h3>

    <asp:Label ID="lblActivityDescription" runat="server" Text="Activity Description:"></asp:Label>
    <asp:TextBox ID="txtActivityDescription" runat="server"></asp:TextBox><br />

    <asp:Label ID="lblCapacity" runat="server" Text="Capacity:"></asp:Label>
    <asp:TextBox ID="txtCapacity" runat="server"></asp:TextBox><br />

    <asp:Label ID="lblUnit" runat="server" Text="Unit:"></asp:Label>
    <asp:DropDownList ID="ddlUnit" runat="server">
        <asp:ListItem Text="Liters" Value="Liters"></asp:ListItem>
        <asp:ListItem Text="Milliliters" Value="Milliliters"></asp:ListItem>
    </asp:DropDownList><br />

    <asp:Button ID="btnSaveActivity" runat="server" Text="Save Activity" OnClick="btnSaveActivity_Click" /><br />

    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Panel>
    <asp:Panel ID="propertyDetailsPanel" runat="server" Visible="false">
    
    <!-- List of Residents -->
    <h3>Residents</h3>
        <asp:GridView ID="gvResidents" runat="server" AutoGenerateColumns="False" EmptyDataText="No residents found." DataSourceID="SqlDataSource1"></asp:GridView>
        <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\kwaWaterSava.accdb;Persist Security Info=True" ProviderName="System.Data.OleDb" SelectCommand="SELECT u.Resident_Username FROM (Resident r INNER JOIN [User] u ON r.Resident_Username = u.User_Email) WHERE (r.Property_ID = [@PropertyID])">
            <SelectParameters>
        <asp:Parameter Name="PropertyID" />
    </SelectParameters>
        </asp:SqlDataSource>
        <br />

        <asp:Button ID="btnAddResident" runat="server" Text="Button" />

    <!-- List of Activities -->
    <h3>Activities</h3>
        <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" EmptyDataText="No activities found." DataSourceID="SqlDataSource2"></asp:GridView>
        <asp:SqlDataSource runat="server" ID="SqlDataSource2" ConnectionString="Provider=Microsoft.ACE.OLEDB.12.0;Data Source=|DataDirectory|\kwaWaterSava.accdb;Persist Security Info=True">
                    <SelectParameters>
    <asp:Parameter Name="PropertyID"  />
</SelectParameters>
        </asp:SqlDataSource>
        <asp:Button ID="btnAddActivity" runat="server" Text="Button" />
       
        <br />
</asp:Panel>


</asp:Content>
