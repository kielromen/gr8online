<%@ Page Title="Credit Memo" Language="vb" AutoEventWireup="false" CodeBehind="CreditMemo_Maintenance.aspx.vb" Inherits="GR8Books.CreditMemo_Maintenance" MasterPageFile="~/Master/Dashboard.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#<%=txtCreditTitle.ClientID%>").autocomplete({
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
                    $("#<%=txtCreditCode.ClientID%>").val(i.item.id);
                },
                minLength: 1
            });


            $("#<%=txtCustomerName.ClientID%>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("CreditMemo_Maintenance.aspx/ListVCE") %>",
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
                    $("#<%=txtCustomerCode.ClientID%>").val(i.item.id);
                },
                minLength: 1
            });

            $("#<%=txtChargeToName.ClientID%>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("CreditMemo_Maintenance.aspx/ListVCE") %>",
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
                    $("#<%=txtCreditMemoTo.ClientID%>").val(i.item.id);
                },
                minLength: 1
            });

            $("#<%=btnSave.ClientID%>").click(function () {
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
    <div class="row mb-2">
        <div class="col-2 my-auto">
            Credit Memo Type:         
        </div>
        <div class="col">
            <asp:TextBox ID="txtCreditType" runat="server" Class="form-control"></asp:TextBox>
        </div>
        <div class="col-1 my-auto">
            CM No.:
        </div>
        <div class="col">
            <asp:TextBox ID="txtCMno" Class="txtCMno form-control" runat="server" Placeholder="DM No." />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtCMno" ErrorMessage="Field is required." ValidationGroup="g" Display="Dynamic"></asp:RequiredFieldValidator>
        </div>
        <div class="col-1 my-auto">
            Date:
        </div>
        <div class="col">
            <input type="date" runat="server" id="dtpDoc_Date" class="form-control">
        </div>
    </div>
    <div class="row mb-2">
        <div class="col-2 my-auto">
            <asp:Label ID="lblCustomerCode" runat="server" Text="Customer Code:"></asp:Label>
        </div>
        <div class="col">
            <asp:TextBox ID="txtCustomerCode" runat="server" Class="form-control"></asp:TextBox>
        </div>
        <div class="col-2 my-auto">
            <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name"></asp:Label>
        </div>
        <div class="col">
            <div class="input-group">
                <asp:TextBox ID="txtCustomerName" runat="server" Class="form-control"></asp:TextBox>
                <div class="input-group-append">
                    <asp:Button Text="Add New" ID="btnAddNewCustomer" runat="server" class="btn btn-primary" />
                </div>
            </div>
        </div>
    </div>

    <div class="row mb-2">
        <div class="col-2 my-auto">
            <asp:Label ID="lblCreditMemoTo" runat="server" Text="Credit Memo To:"></asp:Label>
        </div>
        <div class="col">
            <asp:TextBox ID="txtCreditMemoTo" runat="server" Class="form-control"></asp:TextBox>
        </div>
        <div class="col-2 my-auto">
            <asp:Label ID="lblChargeToName" runat="server" Text="Charge To Name:"></asp:Label>
        </div>
        <div class="col">
            <asp:TextBox ID="txtChargeToName" runat="server" Class="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="row mb-2">
        <div class="col-2 my-auto">
            <asp:Label ID="lblCreditAccnt" runat="server" Text="Credit Account:"></asp:Label>
        </div>
        <div class="col">
            <asp:TextBox ID="txtCreditCode" runat="server" Class="form-control"></asp:TextBox>
        </div>
        <div class="col-2 my-auto">
            <asp:Label ID="lblCreditAccntTitle" runat="server" Text="Credit Account Title:"></asp:Label>
        </div>
        <div class="col">
            <asp:TextBox ID="txtCreditTitle" runat="server" Class="form-control"></asp:TextBox>
        </div>
    </div>
    <div class="row ">
        <div class="col-2 my-auto">
            <asp:Label ID="lblAmount" runat="server" Text="Amount:" Type="number"></asp:Label>
        </div>
        <div class="col">
            <asp:TextBox ID="TxtAmount" Class="txtAmoun form-control" runat="server" Placeholder="0.00" />
        </div>
        <div class="col-2 my-auto">
            <asp:Label ID="lblRemarks" runat="server" Text="Remarks:" Type="number"></asp:Label>
        </div>
        <div class="col">
            <asp:TextBox ID="txtRemarks" Class="txtRemarks form-control" runat="server" />
        </div>
    </div>

    <div class="row justify-content-end mt-4">
        <div class="col-sm-2 mb-2">
            <asp:Button Text="Save" ID="btnSave" runat="server" ValidationGroup="g" class="btnSave btn btn-primary btn-block" />
        </div>
        <div class="col-sm-2">
            <asp:Button Text="Cancel" ID="btnCancel" runat="server" class="btnCancel btn btn-primary btn-block" />
        </div>
    </div>
</asp:Content>
