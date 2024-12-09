<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Login.Master" AutoEventWireup="true" CodeBehind="NewAdmin.aspx.cs" Inherits="WaterSavaFinal.NewAdmin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">

     <main>
     <img src="Images/waterSava Logo.png" />
     <h2 class="pageTitle">WaterSava</h2>
     <form runat="server">
<asp:Panel ID="AdminPanel" runat="server" Visible="true">
     <h3>Add Admin Details</h3>
     <hr />
     <div class="input-group">
         <label for="firstName">First Name:</label>
         <asp:Textbox runat="server" id="firstName" type="text" name="firstName" ValidationGroup="AdminValidation" required="required" CssClass="textbox"/>
         <asp:RequiredFieldValidator ID="FNameValidator" ControlToValidate="firstName" runat="server" ErrorMessage="*Required!" ForeColor="Red" ValidationGroup="AdminValidation"/>
     </div>
     <div class="input-group">
         <label for="LastName">Last Name:</label>
         <asp:Textbox CssClass="textbox" runat="server" id="LastName" type="text" name="firstName" ValidationGroup="AdminValidation" required="required"/>
         <asp:RequiredFieldValidator ID="LNameValidator" ControlToValidate="LastName" runat="server" ErrorMessage="*Required!" ForeColor="Red" ValidationGroup="AdminValidation"/>
     </div>
     <div class="input-group">
         <label for="Email">Email:</label>
         <asp:Textbox CssClass="textbox" runat="server" id="Email" type="email" name="Email" ValidationGroup="AdminValidation" required="required"/>
         <asp:RequiredFieldValidator ID="emailValidator" ControlToValidate="Email" runat="server" ErrorMessage="*Required!" ForeColor="Red" ValidationGroup="AdminValidation"/>
     </div>
     <div class="input-group">
         <label for="Contact">Contact:</label>
         <asp:Textbox CssClass="textbox" runat="server" id="Contact" type="text" name="Contact" ValidationGroup="AdminValidation" required="required"/>
         <asp:RequiredFieldValidator ID="ContactValidator" ControlToValidate="Contact" runat="server" ErrorMessage="*Required!" ForeColor="Red" ValidationGroup="AdminValidation"/>
     </div>
    <div class="input-group">
        <label for="titleButtons">Title:</label>
        <div id="titleButtons">
            &nbsp;<asp:RadioButton runat="server" ID="rbtnMr" GroupName="Title" Text="Mr" />
            &nbsp;<asp:RadioButton runat="server" ID="rbtnMrs" GroupName="Title" Text="Mrs"  />
            &nbsp;<asp:RadioButton runat="server" ID="rbtnMs" GroupName="Title" Text="Ms" />
            &nbsp;<asp:RadioButton runat="server" ID="rbtnMiss" GroupName="Title" Text="Miss" />
        </div>
    </div>
           <div class="input-group">
            <label for="CreatePassword">Create Password:</label>
            <asp:TextBox CssClass="textbox" runat="server" ID="CreatePassword" TextMode="Password" ValidationGroup="AdminValidation" required="required"/>
               <asp:RequiredFieldValidator ID="CreatePasswordValidator" ControlToValidate="CreatePassword" runat="server" ErrorMessage="*Password is required!" ForeColor="Red" ValidationGroup="AdminValidation" />
     </div>
        <div class="input-group">
            <label for="ConfirmPassword">Confirm Password:</label>
            <asp:TextBox CssClass="textbox" runat="server" ID="ConfirmPassword" TextMode="Password" required="required" />
            <asp:RequiredFieldValidator ID="ConfirmPasswordValidator" ControlToValidate="ConfirmPassword" runat="server" ErrorMessage="*Password confirmation is required!" ForeColor="Red" ValidationGroup="AdminValidation" />
            <asp:CompareValidator ID="passwordValidator" runat="server"  ControlToValidate="ConfirmPassword"  ControlToCompare="CreatePassword" ErrorMessage="Passwords do not match." ForeColor="Red" ValidationGroup="AdminValidation"/>
        </div>
         <div class="actionButtons">
             <asp:Button ID="Clear" runat="server" Text="Cancel" OnClick="Clear_Click" CssClass="cancel-button" />
             <asp:Button ID="btnContinue" runat="server" CssClass="btnSaveCons" Text="Continue" OnClick="Save_Admin_Details" ValidationGroup="AdminValidation" CausesValidation="true" />

         <asp:Label ID="lblMessgae" runat="server"></asp:Label>

         </div>
         <asp:HiddenField ID="SuccessfulHiddenMessage" runat="server"  />
</asp:Panel>
<asp:Panel ID="PropertyPanel" runat="server" Visible="false">
         <hr />
         <h3>Add House Details</h3>
         <hr />
         
         <div class="input-group">
             <label for="houseNo">House Number:</label>
             <asp:Textbox runat="server" id="houseNo" type="text" CssClass="textbox" name="houseNo" required="required"/>
        </div>
          <div class="input-group">
             <label for="houseNo"> Street Name:</label>
             <asp:Textbox runat="server" id="houseStreet" CssClass="textbox" type="text" name="houseStreet" required="required"/>
        </div>
          <div class="input-group">
             <label for="houseSub">Suburb:</label>
             <asp:Textbox CssClass="textbox" runat="server" id="houseSub" type="text" name="houseSub" required="required"/>
        </div>
          <div class="input-group">
             <label for="houseCity">City:</label>
             <asp:Textbox CssClass="textbox" runat="server" id="houseCity" type="text" name="houseCity" required="required"/>
        </div>
          <div class="input-group">
             <label for="houseProv">Province:</label>
             <asp:DropDownList ID="houseProv" runat="server" CssClass="activityDD">
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
             <asp:Textbox CssClass="textbox" runat="server" id="houseCode" type="text" name="houseCode" required="required"/>
        </div>
             
         <div class="actionButtons">
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="cancel-button" />
            <asp:Button ID="btnSave_Property" runat="server" Text="Continue" OnClick="Save_Property_Details" CssClass="btnSaveCons"/>
        </div>
    </asp:Panel>
<asp:Panel ID="ResidentPanel" runat="server" Visible="false">
         <hr />
         <h3>Add Resident</h3> <%--you must be able to add multiple residents in this page. Form must refresh--%>
         <hr />
          <div class="input-group">
             <label for="firstName">First Name:</label>
             <asp:Textbox runat="server" id="Textbox1" type="text" CssClass="textbox" name="firstName" required="required"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="firstName" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="input-group">
             <label for="LastName">Last Name:</label>
             <asp:Textbox CssClass="textbox" runat="server" id="Textbox2" type="text" name="firstName" required="required"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="LastName" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="input-group">
             <label for="Email">Email:</label>
             <asp:Textbox CssClass="textbox" runat="server" id="Textbox3" type="email" name="Email" required="required"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="Email" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="input-group">
             <label for="Contact">Contact:</label>
             <asp:Textbox CssClass="textbox" runat="server" id="Textbox4" type="text" name="Contact" required="required"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="Contact" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
        <div class="TitleField">
            <label for="titleButtons">Title:</label>
            <div class="titleButtons">
                &nbsp;<asp:RadioButton runat="server" ID="RadioButton1" GroupName="Title" Text="Mr" />
                &nbsp;<asp:RadioButton runat="server" ID="RadioButton2" GroupName="Title" Text="Mrs"  />
                &nbsp;<asp:RadioButton runat="server" ID="RadioButton3" GroupName="Title" Text="Ms" />
                &nbsp;<asp:RadioButton runat="server" ID="RadioButton4" GroupName="Title" Text="Miss" />
            </div>
        </div>
   
           <asp:Button ID="saveRes" runat="server" Text="Done" OnClick="SaveFinal_Resident_Info"  CssClass="btnSaveCons"/>
   </asp:Panel>
<asp:GridView ID="gvAdmins" runat="server" AutoGenerateColumns="False" Visible="false">
    <Columns>
        <asp:BoundField DataField="User_ID" HeaderText="User ID" />
        <asp:BoundField DataField="User_LName" HeaderText="Last Name" />
        <asp:BoundField DataField="User_FName" HeaderText="First Name" />
        <asp:BoundField DataField="User_Email" HeaderText="Email" />
        <asp:BoundField DataField="User_ContactNo" HeaderText="Contact Number" />
    </Columns>
</asp:GridView>
<!-- Property GridView -->
<asp:GridView ID="gvProperties" runat="server" AutoGenerateColumns="False" Visible="false">
    <Columns>
        <asp:BoundField DataField="Property_ID" HeaderText="Property ID" />
        <asp:BoundField DataField="Property_HouseNum" HeaderText="House Number" />
        <asp:BoundField DataField="Property_StreetName" HeaderText="Street Name" />
        <asp:BoundField DataField="Property_Suburb" HeaderText="Suburb" />
        <asp:BoundField DataField="Property_City" HeaderText="City" />
        <asp:BoundField DataField="Property_Province" HeaderText="Province" />
        <asp:BoundField DataField="Property_PostalCode" HeaderText="Postal Code" />
    </Columns>
</asp:GridView>
    <asp:GridView ID="gvResidents" runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="User_ID" HeaderText="User ID" ReadOnly="True" />
        <asp:BoundField DataField="User_FName" HeaderText="First Name" />
        <asp:BoundField DataField="User_LName" HeaderText="Last Name" />
        <asp:BoundField DataField="User_Email" HeaderText="Email Address" />
        <asp:BoundField DataField="User_ContactNo" HeaderText="Contact Number" />
 
    </Columns>
</asp:GridView>

    <script type="text/javascript">
        var hiddenMessageClientID = '<%= SuccessfulHiddenMessage.ClientID %>'
    </script>
 
     </form>

 </main>

</asp:Content>

