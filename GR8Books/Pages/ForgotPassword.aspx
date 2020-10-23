<%@ Page Title="Forgot Password" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="ForgotPassword.aspx.vb" Inherits="GR8Books.ForgotPassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />

    <div class="row justify-content-center mt-5 mb-5">
        <div class="col-md-4 border">
            <div class="text-center">
                <h3><i class="fa fa-lock fa-4x"></i></h3>
                <h2 class="text-center">Forgot Password?</h2>
                <p>You can reset your password here.</p>
                <div class="form-group">
                    <asp:TextBox ID="txtEmail" runat="server" Placeholder="Email address" class="form-control" />
                </div>
                <div class="form-group">
                    <asp:Button Text="Reset Password" ID="btnSubmit" runat="server" class="btn btn-lg btn-primary btn-block" />
                </div>
            </div>
        </div>
    </div>
</asp:Content>
