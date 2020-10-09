<%@ Page Title="Responsibility Center" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Responsibility_Center.aspx.vb" Inherits="GR8Books.Responsibility_Center" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

     <script type="text/javascript">
         $(document).ready(function () {
             $(".btnSave").click(function () {
                 if (Page_IsValid) {
                     if (confirm("Are you sure you want to save?")) {

                     }
                     else {
                         return false;
                     }
                 }
             });
         });
     </script>
    <asp:Panel runat="server" ID="panelRecCenter">
          <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Type:" runat="server" />
            </div>
            <div class="col">
                <asp:DropDownList ID="ddlType" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator6" runat="Server" ControlToValidate="ddlType" InitialValue="--Select Type--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-2 my-auto">
                <asp:Label Text="Res. Description:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtCostCenter" class="txtCostCenter form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtCostCenter" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mt-4 justify-content-end">
            <div class="col-sm-2 ">
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary btn-block" runat="server" ValidationGroup="g" />
            </div>
            <div class="col-sm-2 ">
                <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary btn-block" runat="server" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
