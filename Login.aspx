<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Login.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WaterSavaFinal.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="FeaturedContent" runat="server">
    <main aria-labelledby="title">
        <div class="lgnContainer">
            <img class="imgLogin" src="Images/waterSava Logo.png" alt="Logo" />
            <h1>WaterSava</h1>

            <form id="lgnForm" runat="server">
                
                        <div class="user-buttons">
                <asp:RadioButtonList ID="userRoles" runat="server" AutoPostBack="true" OnSelectedIndexChanged="UserRoles_SelectedIndexChanged" RepeatDirection="Horizontal" CssClass="userRoleTabs">
                    <asp:ListItem Value="Admin">Admin</asp:ListItem>
                    <asp:ListItem Value="Resident">Resident</asp:ListItem>
                </asp:RadioButtonList>
            </div>
                <div>
                    <asp:Login ID="LoginID" runat="server" OnAuthenticate="Login_Authenticate">
            <LayoutTemplate>
                <div class="input-group">
                <asp:Label for="UserName" runat="server"  class="textbox-label">Username: </asp:Label>
                <asp:TextBox ID="UserName" runat="server" CssClass="textbox" />
                </div>
                 <div class="input-group">
                <label for="Password" class="textbox-label">Password: </label>
                <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="textbox" />
                </div>  
                <asp:Button ID="LoginButton" runat="server" Text="Log In" CssClass="login-button" CommandName="Login"/>
                <asp:CheckBox ID="RememberMe" runat="server" Text="Remember me" CssClass="remember-me-checkbox"/>
                </LayoutTemplate>
                    </asp:Login>

                </div>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Text="Hi, Please login" />
                <p><a class="links" id="forgotPassword" href="setupPassword.aspx">Forgot Password?</a></p>
                <p><a class="links" id="newUser" href="NewUser.aspx">Are you a new user?</a></p>
            </form>
        </div>
    </main>
</asp:Content>
