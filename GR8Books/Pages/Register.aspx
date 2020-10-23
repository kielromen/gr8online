<%@ Page Title="Register" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Register.aspx.vb" Inherits="GR8Books.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />

    <asp:Panel runat="server">
        <div class="row justify-content-center mt-5 mb-5">
            <div class="col-md-6">
                <div class="card">
                    <header class="card-header">
                        <a href="Login.aspx" class="float-right btn btn-outline-primary mt-1">Log in</a>
                        <h4 class="card-title mt-2">Register</h4>
                    </header>
                    <article class="card-body">
                        <div class="form-row">
                            <div class="col form-group">
                                <label>First name </label>
                                <asp:TextBox ID="txtFirstname" runat="server" class="form-control" />
                            </div>
                            <div class="col form-group">
                                <label>Last name</label>
                                <asp:TextBox ID="txtLastName" runat="server" class="form-control" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label>Company name</label>
                            <asp:TextBox ID="txtCompanyName" runat="server" TextMode="MultiLine" class="form-control" />
                        </div>
                        <div class="form-group">
                            <label>Email address</label>
                            <asp:TextBox ID="txtEmail" runat="server" class="form-control" />
                            <small class="form-text text-muted">We'll never share your email with anyone else.</small>
                        </div>
                        <div class="form-group">
                            <label>Contact number</label>
                            <asp:TextBox ID="txtContactNumber" runat="server" class="form-control" />
                        </div>
                        <div class="form-group">
                            <label>Password</label>
                            <asp:TextBox ID="txtPassword" runat="server" Placeholder="Must be 8-20 characters" TextMode="Password" class="form-control" />
                        </div>
                        <div class="form-group">
                            <label>Confirm password</label>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" Placeholder="Repeat password here" TextMode="Password" class="form-control" />
                        </div>
                        <div class="form-group">
                            <asp:Button Text="Register" ID="btnRegister" runat="server" class="btn btn-primary btn-block" />
                            <asp:Label ID="Label1" runat="server" Text="" />
                        </div>
                        <small class="text-muted">By clicking the 'Register' button, you confirm that you accept our
                            <br>
                            Terms of use and Privacy Policy.</small>
                    </article>
                    <div class="border-top card-body text-center">Have an account? <a href="Login.aspx">Log In</a></div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
