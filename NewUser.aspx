<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Login.Master" AutoEventWireup="true" CodeBehind="NewUser.aspx.cs" Inherits="WaterSavaFinal.NewUser" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
<h3>Are you a Resident or Home Owner?</h3>
    <p>If you are a Resident, setup your username in the link sent by your landlord
    </p>
    <form runat="server">
 <asp:Button ID="BtnAdmin" runat="server" Text="New Admin" OnClick="BtnAdmin_Click" />
    </form>
   
</asp:Content>
