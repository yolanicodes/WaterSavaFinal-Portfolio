<%@ Page Title="Manage Properties"  Language="C#" MasterPageFile="~/AdminSite.Master"AutoEventWireup="true" CodeBehind="ManageProperties.aspx.cs" Inherits="WaterSavaFinal.ManageProperties" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Label ID="lblDebug" runat="server" ForeColor="Red"></asp:Label>
   
    <h3>My Properties</h3>
    <asp:GridView ID="listProperties" runat="server" AutoGenerateColumns="False"
        DataKeyNames="Property_ID"
        OnRowEditing="listProperties_RowEditing"
        OnRowDeleting="listProperties_RowDeleting"
            OnSelectedIndexChanged="listProperties_SelectedIndexChanged"
        OnRowUpdating="listProperties_RowUpdating">

       <Columns>
    <asp:BoundField DataField="Property_HouseNum" HeaderText="Property House Number" SortExpression="Property_HouseNum" />
    <asp:BoundField DataField="Property_StreetName" HeaderText="Street Name" SortExpression="Property_StreetName" />
    <asp:BoundField DataField="Property_Suburb" HeaderText="Suburb" SortExpression="Property_Suburb" />
    <asp:BoundField DataField="Property_City" HeaderText="City" SortExpression="Property_City" />
    <asp:TemplateField HeaderText="Province" SortExpression="Property_Province">
            <ItemTemplate>
                <asp:Label ID="lblProvince" runat="server" Text='<%# Bind("Property_Province") %>'></asp:Label>
            </ItemTemplate>
            <EditItemTemplate>
                <asp:DropDownList ID="ddlProvince" runat="server">
                    <asp:ListItem Text="Select Province" Value=""></asp:ListItem>
                    <asp:ListItem Text="Eastern Cape" Value="Eastern Cape"></asp:ListItem>
                    <asp:ListItem Text="Free State" Value="Free State"></asp:ListItem>
                    <asp:ListItem Text="Gauteng" Value="Gauteng"></asp:ListItem>
                    <asp:ListItem Text="KwaZulu-Natal" Value="KwaZulu-Natal"></asp:ListItem>
                    <asp:ListItem Text="Limpopo" Value="Limpopo"></asp:ListItem>
                    <asp:ListItem Text="Mpumalanga" Value="Mpumalanga"></asp:ListItem>
                    <asp:ListItem Text="Northern Cape" Value="Northern Cape"></asp:ListItem>
                    <asp:ListItem Text="North West" Value="North West"></asp:ListItem>
                    <asp:ListItem Text="Western Cape" Value="Western Cape"></asp:ListItem>
                </asp:DropDownList>
            </EditItemTemplate>
        </asp:TemplateField>

        <asp:BoundField DataField="Property_PostalCode" HeaderText="Postal Code" SortExpression="Property_PostalCode" />
       
    <asp:CommandField ShowSelectButton="True"/>
    <asp:CommandField ShowEditButton="true" />
    <asp:ButtonField Text="Delete" CommandName="Delete" />
</Columns>
    </asp:GridView>
    <br>
        <asp:Button ID="btnAddProperty" runat="server" Text="Add property" OnClick="btnAddProperty_Click" CssClass="btnSaveCons"/>

    <asp:Panel ID="PropertyPanel" runat="server" Visible="false">
         <hr />
         <h3>Add House Details</h3>
         <hr />
         
         <div class="input-group">
             <label for="houseNo">House Number:</label>
             <asp:Textbox runat="server" id="houseNo" type="text" name="houseNo" required="required" CssClass="textbox"/>
        </div>
          <div class="input-group">
             <label for="houseNo"> Street Name:</label>
             <asp:Textbox runat="server" id="houseStreet" type="text" name="houseStreet" required="required" CssClass="textbox"/>
        </div>
          <div class="input-group">
             <label for="houseSub">Suburb:</label>
             <asp:Textbox runat="server" id="houseSub" type="text" name="houseSub" required="required" CssClass="textbox"/>
        </div>
          <div class="input-group">
             <label for="houseCity">City:</label>
             <asp:Textbox runat="server" id="houseCity" type="text" name="houseCity" required="required" CssClass="textbox"/>
        </div>
          <div class="input-group">
     <label for="houseProv">Province:</label>
     <asp:DropDownList ID="houseProv" runat="server"  CssClass="activityDD">
         <asp:ListItem Enabled="False">Select Province</asp:ListItem>
         <asp:ListItem>Eastern Cape</asp:ListItem>
         <asp:ListItem>Western Cape</asp:ListItem>
         <asp:ListItem>KwaZulu Natal</asp:ListItem>
         <asp:ListItem>Northern Cape</asp:ListItem>
         <asp:ListItem>North West</asp:ListItem>
         <asp:ListItem>Gauteng</asp:ListItem>
         <asp:ListItem>Mpumalanga </asp:ListItem>
         <asp:ListItem>Limpopo</asp:ListItem>
         <asp:ListItem>Free State</asp:ListItem>
     </asp:DropDownList>
</div>
        
          <div class="input-group">
             <label for="houseCode">Postal code:</label>
             <asp:Textbox runat="server" id="houseCode" type="text" name="houseCode" required="required" CssClass="textbox"/>
        </div>
             
         <div class="actionButtons">
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="cancel-button" />
            <asp:Button ID="btnSave_Property" runat="server" Text="Continue" OnClick="btnSave_Property_Click" CssClass="btnSaveCons"/>
        </div>
    </asp:Panel>
    <asp:Panel ID="ResidentPanel" runat="server" Visible="false">
         <hr />
         <h3>Add Resident</h3> 
         <hr />
          <div class="input-group">
             <label for="firstName">First Name:</label>
             <asp:Textbox runat="server" id="firstName" type="text" name="firstName" required="required" CssClass="textbox"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="firstName" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="input-group">
             <label for="LastName">Last Name:</label>
             <asp:Textbox runat="server" id="LastName" type="text" name="lastName" required="required" CssClass="textbox"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="LastName" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="input-group">
             <label for="Email">Email:</label>
             <asp:Textbox runat="server" id="Email" type="email" name="Email" required="required" CssClass="textbox"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="Email" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="input-group">
             <label for="Contact">Contact:</label>
             <asp:Textbox runat="server" id="Contact" type="text" name="Contact" required="required" CssClass="textbox"/>
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
          <asp:Button ID="saveRes" runat="server" Text="Done" OnClick="SaveFinal_Resident_Info" CssClass="btnSaveCons" />
   

    <script type="text/javascript">

</script>
  </asp:Panel>
    <asp:Panel ID="activityPanel" runat="server" Visible="false">
    <h3>Add New Activity</h3>
<div class="input-group">
    <asp:Label ID="lblActivityDescription" runat="server" Text="Activity Description:"></asp:Label>
    <asp:TextBox ID="txtActivityDescription" runat="server" CssClass="textbox"></asp:TextBox><br />
</div>
        <div class="input-group">
    <asp:Label ID="lblCapacity" runat="server" Text="Capacity:"></asp:Label>
    <asp:TextBox ID="txtCapacity" runat="server" CssClass="textbox"></asp:TextBox><br /></div>

    <asp:Label ID="lblUnit" runat="server" Text="Unit:"></asp:Label>
    <asp:DropDownList ID="ddlUnit" runat="server">
        <asp:ListItem Text="Liters" Value="Liters"></asp:ListItem>
        <asp:ListItem Text="Milliliters" Value="Milliliters"></asp:ListItem>
    </asp:DropDownList><br />


    <asp:Button ID="btnSaveActivity" runat="server" Text="Save Activity" OnClick="btnSaveActivity_Click" CssClass="btnSaveCons" /><br />

    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
</asp:Panel>
    <asp:Panel ID="propertyDetailsPanel" runat="server" Visible="false">
    
   <!-- List of Residents -->
<h3>Residents</h3>
<asp:GridView ID="gvResidents" runat="server" AutoGenerateColumns="False" EmptyDataText="No residents found."
    OnRowDeleting="gvResidents_RowDeleting">
    <Columns>
         <asp:BoundField DataField="User_ID" HeaderText="User ID" SortExpression="User_ID" Visible="False" />
        <asp:BoundField DataField="User_FName" HeaderText="First Name" />
        <asp:BoundField DataField="User_LName" HeaderText="Last Name" />
         <asp:BoundField DataField="User_Email" HeaderText="Email" />
        <asp:CommandField ShowDeleteButton="True" />
    </Columns>
</asp:GridView>
        <br />
<asp:Button ID="btnAddResident" runat="server" Text="Add Resident" OnClick="btnAddResident_Click" CssClass="btnSaveCons" />


 <!-- List of Activities -->
<h3>Activities</h3>
<asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" EmptyDataText="No activities found."
    OnRowEditing="gvActivities_RowEditing"
    OnRowDeleting="gvActivities_RowDeleting"
    OnRowUpdating="gvActivities_RowUpdating">
    <Columns>
        <asp:BoundField DataField="Activity_Description" HeaderText="Activity" />
        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
    </Columns>
</asp:GridView> <br>
<asp:Button ID="btnAddActivity" runat="server" CssClass="btnSaveCons" Text="Add activity" OnClick="btnAddActivity_Click" />
        <br />
</asp:Panel>


</asp:Content>
