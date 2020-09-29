<%@ Page Title="Dashboard" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Dashboard.aspx.vb" Inherits="GR8Books.Dashboard1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Panel runat="server">
        <div class="alert alert-success alert-dismissible fade show" role="alert" id="alertWelcome" runat="server">
            Welcome!
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
        </div>
        <asp:Label ID="lblUserCompany" Text="" runat="server" />
    </asp:Panel>
</asp:Content>
