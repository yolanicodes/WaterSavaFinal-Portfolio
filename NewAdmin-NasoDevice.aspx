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
     <div id="FnameField">
         <label for="firstName">First Name:</label>
         <asp:Textbox runat="server" id="firstName" type="text" name="firstName" ValidationGroup="AdminValidation" required="required"/>
         <asp:RequiredFieldValidator ID="FNameValidator" ControlToValidate="firstName" runat="server" ErrorMessage="*Required!" ForeColor="Red" ValidationGroup="AdminValidation"/>
     </div>
     <div id="LnameField">
         <label for="LastName">Last Name:</label>
         <asp:Textbox runat="server" id="LastName" type="text" name="firstName" ValidationGroup="AdminValidation" required="required"/>
         <asp:RequiredFieldValidator ID="LNameValidator" ControlToValidate="LastName" runat="server" ErrorMessage="*Required!" ForeColor="Red" ValidationGroup="AdminValidation"/>
     </div>
     <div id="emailField">
         <label for="Email">Email:</label>
         <asp:Textbox runat="server" id="Email" type="email" name="Email" ValidationGroup="AdminValidation" required="required"/>
         <asp:RequiredFieldValidator ID="emailValidator" ControlToValidate="Email" runat="server" ErrorMessage="*Required!" ForeColor="Red" ValidationGroup="AdminValidation"/>
     </div>
     <div id="ContactField">
         <label for="Contact">Contact:</label>
         <asp:Textbox runat="server" id="Contact" type="text" name="Contact" ValidationGroup="AdminValidation" required="required"/>
         <asp:RequiredFieldValidator ID="ContactValidator" ControlToValidate="Contact" runat="server" ErrorMessage="*Required!" ForeColor="Red" ValidationGroup="AdminValidation"/>
     </div>
    <div id="TitleField">
        <label for="titleButtons">Title:</label>
        <div id="titleButtons">
            &nbsp;<asp:RadioButton runat="server" ID="rbtnMr" GroupName="Title" Text="Mr" />
            &nbsp;<asp:RadioButton runat="server" ID="rbtnMrs" GroupName="Title" Text="Mrs"  />
            &nbsp;<asp:RadioButton runat="server" ID="rbtnMs" GroupName="Title" Text="Ms" />
            &nbsp;<asp:RadioButton runat="server" ID="rbtnMiss" GroupName="Title" Text="Miss" />
        </div>
    </div>
           <div id="CreatePWField">
            <label for="CreatePassword">Create Password:</label>
            <asp:TextBox runat="server" ID="CreatePassword" TextMode="Password" ValidationGroup="AdminValidation" required="required"/>
               <asp:RequiredFieldValidator ID="CreatePasswordValidator" ControlToValidate="CreatePassword" runat="server" ErrorMessage="*Password is required!" ForeColor="Red" ValidationGroup="AdminValidation" />
     </div>
        <div id="ConfirmPWField">
            <label for="ConfirmPassword">Confirm Password:</label>
            <asp:TextBox runat="server" ID="ConfirmPassword" TextMode="Password" required="required" />
            <asp:RequiredFieldValidator ID="ConfirmPasswordValidator" ControlToValidate="ConfirmPassword" runat="server" ErrorMessage="*Password confirmation is required!" ForeColor="Red" ValidationGroup="AdminValidation" />
            <asp:CompareValidator ID="passwordValidator" runat="server"  ControlToValidate="ConfirmPassword"  ControlToCompare="CreatePassword" ErrorMessage="Passwords do not match." ForeColor="Red" ValidationGroup="AdminValidation"/>
        </div>
         <div class="actionButtons">
             <asp:Button ID="Clear" runat="server" Text="Cancel" OnClick="Clear_Click" />
             <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="Save_Admin_Details" ValidationGroup="AdminValidation" CausesValidation="true" />

         <asp:Label ID="lblMessgae" runat="server"></asp:Label>

         </div>
         <asp:HiddenField ID="SuccessfulHiddenMessage" runat="server"  />
</asp:Panel>
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
            <asp:Button ID="btnSave_Property" runat="server" Text="Continue" OnClick="Save_Property_Details"/>
        </div>
    </asp:Panel>
<asp:Panel ID="ResidentPanel" runat="server" Visible="false">
         <hr />
         <h3>Add Resident</h3> <%--you must be able to add multiple residents in this page. Form must refresh--%>
         <hr />
          <div class="FnameField">
             <label for="firstName">First Name:</label>
             <asp:Textbox runat="server" id="Textbox1" type="text" name="firstName" required="required"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="firstName" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="LnameField">
             <label for="LastName">Last Name:</label>
             <asp:Textbox runat="server" id="Textbox2" type="text" name="firstName" required="required"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="LastName" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="emailField">
             <label for="Email">Email:</label>
             <asp:Textbox runat="server" id="Textbox3" type="email" name="Email" required="required"/>
             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="Email" runat="server" ErrorMessage="*Required!" ForeColor="Red"/>
         </div>
         <div class="ContactField">
             <label for="Contact">Contact:</label>
             <asp:Textbox runat="server" id="Textbox4" type="text" name="Contact" required="required"/>
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
   <div class="actionButtons">
             <asp:Button ID="AddResident" runat="server" Text="Add Resident" OnClick="Save_Resident_Info" />
             <asp:Button ID="DoneButton" runat="server" Text="Done" OnClick="SaveFinal_Resident_Info" CausesValidation="false" />
         <asp:Label ID="Label1" runat="server"></asp:Label>
         </div>
 

    <script type="text/javascript">
        var hiddenMessageClientID = '<%= SuccessfulHiddenMessage.ClientID %>'
    </script>
  </asp:Panel>
     </form>

 </main>

</asp:Content>

