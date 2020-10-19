<%@ Page Title="Collection Payment Type" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="CollectionPaymentType_Maintenance.aspx.vb" Inherits="GR8Books.CollectionPaymentType_Maintenance" %>
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
    <asp:Panel runat="server" ID="panelPaymentType">
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Payment Type" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtPaymentType" class="txtPaymentType form-control" runat="server" AutoComplete="off" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtPaymentType" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="With Bank:" runat="server" />
            </div>
            <div class="col">
                <asp:DropDownList ID="ddlWithBank" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator  InitialValue="--Select Options--"  Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator4" runat="Server" ControlToValidate="ddlWithBank" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
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
