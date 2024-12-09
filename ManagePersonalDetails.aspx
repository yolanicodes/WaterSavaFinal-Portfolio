<%@ Page Title="" Language="C#" MasterPageFile="~/AdminSite.Master" AutoEventWireup="true" CodeBehind="ManagePersonalDetails.aspx.cs" Inherits="WaterSavaFinal.ManagePersonalDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <div class="container">
       
       <asp:Label ID="lblDebug" runat="server" CssClass="debug"></asp:Label>
        <div id="profile">
            <h2 id="h2ProfileUser" runat="server" >Initials - linked to db</h2>
            <h3 id="h3UserRole" runat="server">Admin/Resident - linked to User role</h3>
        </div>

        <div >
            <h3>First Name</h3>
            <asp:Label ID="lblFirstName" runat="server" CssClass="detailfields"></asp:Label>
        </div>
        <div>
            <h3>Last Name</h3>
            <asp:Label ID="lblLastName" runat="server" CssClass="detailfields"></asp:Label>
          
        </div>
        <div >
            <h3>Title</h3>
            <asp:Label ID="lblTitle" runat="server" CssClass="detailfields"></asp:Label>
        </div>  
        <div>
            <h3>Contact Number</h3>
            <asp:Label ID="lblContactNumber" runat="server" CssClass="detailfields"></asp:Label>
        </div>
        <div >
            <h3>Email Address</h3>
            <asp:Label ID="lblEmailAddress" runat="server" CssClass="detailfields"></asp:Label>
            
        </div>
        <div class="detailscontainer"><asp:Button ID="btnEdit" runat="server" type="button" title="Edit" OnClick="btnEdit_Click" Text="Edit" CssClass="cancel-button"/>
            <asp:Button ID="btnDelete" runat="server" Text="Delete Account" OnClick="btnDelete_Click" Cssclass="delete-button"/>
        </div>
            <asp:Panel ID="pnlEditDetails" runat="server">
            <h3>Edit Details</h3>
            <div class="detail-container">
                <h4>First Name</h4>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="detailfields"></asp:TextBox>
            </div>
            <div class="detail-container">
                <h4>Last Name</h4>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="detailfields"></asp:TextBox>
            </div>
            <div class="detail-container">
                <h4>Title</h4>
                <asp:TextBox ID="txtTitle" runat="server" CssClass="detailfields" ReadOnly="true"></asp:TextBox>
            </div>  
            <div class="detail-container">
                <h4>Contact Number</h4>
                <asp:TextBox ID="txtContactNumber" runat="server" CssClass="detailfields"></asp:TextBox>
            </div>
            <div class="detail-container">
                <h4>Email Address</h4>
                <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="detailfields" ReadOnly="true"></asp:TextBox>
            </div>

            <div class="detail-container">
                <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" CssClass="btnSaveCons" />
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_Click" CssClass="cancel-button"/>
            </div>
        </asp:Panel>
            <asp:Label ID="lblDebugging" runat="server" Text=""></asp:Label>

           
            </div>
    </main>
</asp:Content>
