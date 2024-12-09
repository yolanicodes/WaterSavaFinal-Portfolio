<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Login.Master" AutoEventWireup="true" CodeBehind="setupPassword.aspx.cs" Inherits="WaterSavaFinal.setupPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <form id="form1" runat="server">
        <h3>Setup Password</h3>
        <hr />
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        <br />
        <div class="input-group">
        <asp:Label ID="Email" runat="server" Text="Email Address: "></asp:Label>
        <asp:TextBox ID="txtEmail" CssClass="textbox" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
            ErrorMessage="Email address is required." ForeColor="Red"></asp:RequiredFieldValidator></div>
        <br />
         <div class="input-group">
        <asp:Label ID="Label2" runat="server" Text="Create Password:"></asp:Label>
        <asp:TextBox CssClass="textbox" ID="txtCreatePassword" runat="server" TextMode="Password"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvCreatePassword" runat="server" ControlToValidate="txtCreatePassword"
            ErrorMessage="Password is required." ForeColor="Red"></asp:RequiredFieldValidator></div>
        <br />
         <div class="input-group">
        <asp:Label ID="Label3" runat="server" Text="Confirm Password:"></asp:Label>
        <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password" CssClass="textbox"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
            ErrorMessage="Please confirm your password." ForeColor="Red"></asp:RequiredFieldValidator>
        <asp:CompareValidator ID="cvPasswordMatch" runat="server" ControlToCompare="txtCreatePassword"
            ControlToValidate="txtConfirmPassword" ErrorMessage="Passwords do not match." ForeColor="Red"></asp:CompareValidator></div>
        <br />
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btnSaveCons" OnClick="btnSave_Click" />
    </form>
</asp:Content>
