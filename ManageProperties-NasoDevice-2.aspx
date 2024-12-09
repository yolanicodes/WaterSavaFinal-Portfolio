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
        OnRowUpdating="listProperties_RowUpdating"
            OnSelectedIndexChanged="listProperties_SelectedIndexChanged">

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
            <asp:DropDownList ID="ddlProvince" runat="server" required="required">
                 <asp:ListItem Value="" Text="Select Province" />
                 <asp:ListItem Value="Eastern Cape" Text="Eastern Cape" />
                 <asp:ListItem Value="Western Cape" Text="Western Cape" />
                 <asp:ListItem Value="Gauteng" Text="Gauteng" />
                 <asp:ListItem Value="North West" Text="North West" />
                 <asp:ListItem Value="Northern Cape" Text="Northern Cape" />
                 <asp:ListItem Value="Mpumalanga" Text="Mpumalanga" />
                 <asp:ListItem Value="Free State" Text="Free State" />
                 <asp:ListItem Value="Limpopo" Text="Limpopo" />
                 <asp:ListItem Value="Kwa-Zulu Natal" Text="Kwa-Zulu Natal" />
             </asp:DropDownList>
        </div>
          <div class="houseCode">
             <label for="houseCode">Postal code:</label>
             <asp:Textbox runat="server" id="houseCode" type="text" name="houseCode" required="required"/>
        </div>
             <asp:CheckBox ID="chkInUse" runat="server" Text="In Use" />
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
             <asp:Textbox runat="server" id="LastName" type="text" name="lastName" required="required"/>
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
        <asp:GridView ID="gvResidents" runat="server" AutoGenerateColumns="False" EmptyDataText="No residents found.">
            <Columns>
                <asp:BoundField DataField="User_FName" HeaderText="First Name" />
                <asp:BoundField DataField="User_LName" HeaderText="Last Name" />
            </Columns>
        </asp:GridView>
      
        <br />

        <asp:Button ID="btnAddResident" runat="server" Text="Add Resident" OnClick="btnAddResident_Click" />

    <!-- List of Activities -->
    <h3>Activities</h3>
        <asp:GridView ID="gvActivities" runat="server" AutoGenerateColumns="False" EmptyDataText="No activities found." >
            <Columns>
                <asp:BoundField DataField="Activity_Description" HeaderText="Activity" />
            </Columns>
        </asp:GridView>
        
        <asp:Button ID="btnAddActivity" runat="server" Text="Add activity" OnClick="btnAddActivity_Click"/>
       
        <br />
</asp:Panel>


</asp:Content>
