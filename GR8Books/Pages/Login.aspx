<%@ Page Title="Login Page" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Login.aspx.vb" Inherits="GR8Books.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="text-center mt-5" style="width: 400px; margin: auto;">
        <div class="card">
            <div class="card-header">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/GR8_LOGO.png" Height="40" Style="vertical-align: top;" class="mr-2"></asp:Image><h2 style="display: inline;">GR8BOOKS</h2>
            </div>
            <div class="card-body">
                <h3 class="card-title">Sign In</h3>
                <hr />
                <asp:Panel ID="Panel1" runat="server" DefaultButton="BtnLogin">
                    <div class="form-group">
                        <asp:TextBox ID="txtEmail" runat="server" class="form-control" placeholder="Username" />
                    </div>
                    <div class="form-group">
                        <asp:TextBox ID="txtPassword" TextMode="password" runat="server" class="form-control" placeholder="Password" />
                    </div>
                    <asp:CheckBox ID="btnRemember" runat="server" Text="&nbsp; Remember me" />
                    <asp:Button ID="btnLogin" runat="server" Text="SIGN IN" class="btn btn-dark btn-block" />
                    <div class="a">
                        <div class="font-italic text-muted"><sub>By clicking Sign in, you agree to the Terms and have read and acknowledge our Privacy Statement.</sub></div>
                    </div>
                    <hr />
                    <button class="btn btn-light shadow"><i class="fa fa-google mr-2"></i>Sign in with Google</button>
                    <hr />
                    <small>
                        <asp:LinkButton ID="lnkForgot" runat="server">Forgot password?</asp:LinkButton>
                        <asp:LinkButton ID="lnkRegister" runat="server" Style="padding-left: 4em">Not a member yet?</asp:LinkButton>
                    </small>
                </asp:Panel>
            </div>
        </div>
    </div>
</asp:Content>
