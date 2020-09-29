<%@ Page Title="Collector Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Collector_Management.aspx.vb" Inherits="GR8Books.Collector_Management" %>

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
    <asp:Panel runat="server" ID="panelCollector">
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Collector Name:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtCollectorName" class="txtCollectorName form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtCollectorName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row">
            <div class="col-2 my-auto">
                <asp:Label Text="Area:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtArea" class="txtArea form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtArea" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
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
