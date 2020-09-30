<%@ Page Title="Cost Center" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Settings.Master" CodeBehind="Settings_CostCenter.aspx.vb" Inherits="GR8Books.Settings_CostCenter" %>
<asp:Content ID="Content1" ContentPlaceHolderID="settings" runat="server">
    
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>


    <div class="alert alert-success alert-dismissible fade show" role="alert" id="alertSave" runat="server">
            <strong>Successfully saved!</strong>
            <button type="button" class="close" data-dismiss="alert" aria-label="Close">
                <span aria-hidden="true">&times;</span>
            </button>
    </div>
    <div class="row mb-2">
            <div class="col-12">
                <asp:Button Text="New" ID="btnNew" AutoPostBack="False" runat="server" class="btn btn-primary" />
                <asp:Button Text="Edit" ID="btnEdit" runat="server" class="btn btn-primary" />
                <asp:Button  Text="Save"  ID="btnSave" class="btnSave btn btn-primary" runat="server" />
                <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary" runat="server" />
                <asp:Button Text="Close" ID="btnClose" runat="server" class="btn btn-primary" />
            </div>
     </div>
     <hr />
    <asp:Panel ID="panelConrols" runat="server">
    <div class="row mb-2">
           <div class="col-6">
                <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Cost ID:" class="col-form-label" runat="server" />
                        </div>
                        <div class="col-6">
                            <asp:TextBox ID="txtCostID" runat="server"  class="form-control"  autocomplete="off" />
                         </div>
                 </div>
                 <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Cost Center:" class="col-form-label" runat="server" />
                        </div>
                        <div class="col-6">
                             <asp:TextBox ID="txtCostCenter" runat="server"  class="form-control"  autocomplete="off" />
                        </div>
                 </div>
           </div>
    </div>
    </asp:Panel>
</asp:Content>
