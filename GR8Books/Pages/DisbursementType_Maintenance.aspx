<%@ Page Title="Disbursement Type Maintenance" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="DisbursementType_Maintenance.aspx.vb" Inherits="GR8Books.DisbursementType_Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".txtAccountTitle").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("DisbursementType_Maintenance.aspx/ListAccountTitle") %>",
                        data: "{'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    id: item.split('--')[1],
                                    value: item.split('--')[0]
                                }
                            }))
                        },
                        error: function (response) {
                            alert(response.responseText);
                        },
                        failure: function (response) {
                            alert(response.responseText);
                        }
                    });
                },
                select: function (e, i) {
                    $(".txtAccountCode").val(i.item.id);
                },
                minLength: 1
            });

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

    <asp:Panel runat="server" ID="panelDisbursementType">
        <div class="row mb-2">
            <div class="col-sm-2 my-auto">
                <asp:Label Text="Description:" runat="server" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="txtDescription" class="txtDescription form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"  ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtDescription" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm-2 my-auto">
                <asp:Label Text="Account code:" runat="server" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="txtAccountCode" Class="txtAccountCode form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"  ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtAccountCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm-2 my-auto">
                <asp:Label Text="Account title:" runat="server" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="txtAccountTitle" Class="txtAccountTitle form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"  ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtAccountTitle" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm-2 my-auto">
                <asp:Label Text="Amount:" runat="server" />
            </div>
            <div class="col-sm">
                <asp:TextBox ID="txtAmount" runat="server" Type="number" class="form-control" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"  ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtAmount" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row mb-2">
            <div class="col-sm-2 my-auto">
                <asp:Label Text="Module:" runat="server" />
            </div>
            <div class="col-sm">
                <asp:DropDownList ID="ddlModule" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"  ID="RequiredFieldValidator6" runat="Server" ControlToValidate="ddlModule" InitialValue="--Select Module--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
        <div class="row justify-content-end mt-2">
            <div class="col-sm-2 my-auto mb-2">
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary btn-block" runat="server" ValidationGroup="g" />
            </div>
            <div class="col-sm-2 my-auto">
                <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary  btn-block" runat="server" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
