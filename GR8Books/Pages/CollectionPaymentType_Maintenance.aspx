<%@ Page Title="Collection Payment Type" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="CollectionPaymentType_Maintenance.aspx.vb" Inherits="GR8Books.CollectionPaymentType_Maintenance" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

 <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".txtAccountTitle").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("CollectionPaymentType_Maintenance.aspx/ListAccountTitle") %>",
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
    <asp:Panel runat="server" ID="panelPaymentType">
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="PaymentType" runat="server" />
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
       <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Account Code:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtAccountCode" class="txtAccountCode form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtAccountCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
       <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Account Title:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtAccountTitle" class="txtAccountTitle form-control" runat="server" />
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small" Display="Dynamic"   ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtAccountTitle" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
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
