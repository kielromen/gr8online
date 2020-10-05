<%@ Page Title="Cash Advance" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="CashAdvance.aspx.vb" Inherits="GR8Books.CashAdvance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script src="../Scripts/jquery.formatCurrency-1.4.0.js"></script>


    <script type="text/javascript">  
        $(document).ready(function () {
            $('.txtAmount').focusout(function () {
                $('.txtAmount').formatCurrency();
                $('.txtAmount').toNumber().formatCurrency('.txtAmount');
            });
        });
    </script>

    <script type='text/javascript'>
        $(document).ready(function () {
            $(".txtAccntTitle_Entry").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("CashAdvance.aspx/ListAccountTitle")%>",
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
                    var id = $(this).attr('id');
                    id = id.replace("txtAccntTitle", "txtAccntCode");
                    $("#" + id).val(i.item.id);
                },
                minLength: 1
            });
            $(".txtName_Entry").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("CashAdvance.aspx/ListVCE")%>",
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
                    var id = $(this).attr('id');
                    id = id.replace("txtName", "txtCode");
                    $("#" + id).val(i.item.id);
                },
                minLength: 1
            });

            $("#<%=txtName.ClientID%>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("CashAdvance.aspx/ListVCE")%>",
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
                    $("#<%=txtCode.ClientID%>").val(i.item.id);
                },
                minLength: 1
            });

            $("#<%=txtAccntName.ClientID%>").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("CashAdvance.aspx/ListAccountTitle")%>",
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
                    $("#<%=txtAccntCode.ClientID%>").val(i.item.id);
                },
                minLength: 1
            });

            $("#<%=btnSave.ClientID%>").click(function (e) {
                if (Page_IsValid) {
                    if (confirm("Are you sure you want to save?")) {

                    }
                    else {
                        return false;
                    }
                }
            });

            var id = "test";
            $("#<%=btnSearch.ClientID%>").click(function (e) {
                e.preventDefault();
                var Type = "CA";
                var Url = "CashAdvance.aspx";
                var myWidth = "900";
                var myHeight = "550";
                var left = (screen.width - myWidth) / 2;
                var top = (screen.height - myHeight) / 4;
                var win = window.open("LoadTransaction.aspx?id=" + Type + '&Url=' + Url, "Load Transaction", 'dialog=yes,resizable=no, width=' + myWidth + ', height=' + myHeight + ', top=' + top + ', left=' + left);
                var timer = setInterval(function () {
                    if (win.closed) {
                        clearInterval(timer);
                    }
                });
            });

            $(".btnCancel").click(function () {
                if (confirm("Are you sure you want to cancel this transaction?")) {

                }
                else {
                    return false;
                }
            });

            $("#<%=btnPreview.ClientID%>").click(function (e) {
                e.preventDefault();
                var Type = "CA";
                var win = window.open("Reports.aspx?id=" + Type, "_blank");
            });
        });



    </script>


    <asp:Panel ID="Panel1" runat="server">
        <div class="row mt-4 justify-content-start">
            <div class="col">
                <asp:Button Text="Search" ID="btnSearch" runat="server" class="btn btn-primary" />
                <asp:Button Text="New" ID="btnNew" AutoPostBack="False" runat="server" class="btn btn-primary" />
                <asp:Button Text="Edit" ID="btnEdit" runat="server" class="btn btn-primary" />
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary" runat="server" ValidationGroup="g" />
                <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary" runat="server" />
                <asp:Button Text="Close" ID="btnClose" runat="server" class="btn btn-primary" />
                <asp:Button Text="Prev" ID="btnPrev" runat="server" class="btn btn-primary" />
                <asp:Button Text="Next" ID="btnNext" runat="server" class="btn btn-primary" />
                <asp:Button Text="Preview" ID="btnPreview" runat="server" class="btn btn-primary" />
            </div>
        </div>
        <hr />

        <asp:Panel ID="panelConrols" runat="server">
            <div class="row row-cols-2">
                <div class="col-6">
                    <%-- VCE --%>
                    <div class="row mb-2">
                        <div class="col-4 my-auto text-nowrap">
                            <asp:Label Text="VCECode:" runat="server" class="col-sm-3 col-form-label" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCode" runat="server" class="form-control" autocomplete="off" />
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" D="RequiredFieldValidator4" runat="Server" ControlToValidate="txtCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto text-nowrap">
                            <asp:Label Text="VCEName:" runat="server" class="col-sm-3 col-form-label" />
                        </div>
                        <div class="col">
                            <div class="input-group">
                                <asp:TextBox ID="txtName" runat="server" class="form-control" autocomplete="off" />
                                <div class="input-group-append">
                                    <asp:Button Text="Add New" ID="btnAddNewVCE" runat="server" class="btn btn-primary" />
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <%-- Account --%>
                    <div class="row mb-2">
                        <div class="col-4 my-auto text-nowrap">
                            <asp:Label Text="Account Code:" runat="server" class="col-sm-3 col-form-label" />
                        </div>
                        <div class="col">

                            <asp:TextBox ID="txtAccntCode" runat="server" class="form-control" autocomplete="off" />

                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator8" runat="Server" ControlToValidate="txtAccntCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto text-nowrap">
                            <asp:Label Text="Account Title:" runat="server" class="col-sm-3 col-form-label" />
                        </div>
                        <div class="col">
                            <div class="input-group">
                                <asp:TextBox ID="txtAccntName" runat="server" class="form-control" autocomplete="off" />
                                <div class="input-group-append">
                                    <asp:Button Text="Add New" ID="btnAddNewAccount" runat="server" class="btn btn-primary" />
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator9" runat="Server" ControlToValidate="txtAccntName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <%-- Cost Center --%>
                    <div class="row mb-2">
                        <div class="col-4 my-auto text-nowrap">
                            <asp:Label Text="Cost Center:" runat="server" class="col-sm-3 col-form-label" autocomplete="off" />
                        </div>
                        <div class="col">
                            <asp:DropDownList runat="server" ID="ddlCostCenter" class="ddlCostCenter form-control" AppendDataBoundItems="true" EnableViewState="true">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="row mb-2">
                        <div class="col-4 my-auto text-nowrap">
                            <asp:Label Text="Amount:" runat="server" class="col-sm-3 col-form-label" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtAmount" runat="server" class="txtAmount form-control text-right" autocomplete="off" />
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtAmount" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto text-nowrap">
                            <asp:Label Text="Remarks:" runat="server" class="col-sm-3 col-form-label" autocomplete="off" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtRemarks" runat="server" class="form-control" autocomplete="off" TextMode="MultiLine" />
                        </div>
                    </div>
                </div>

                <div class="col-6">
                    <div class="row mb-2">
                        <div class="col-4 my-auto text-nowrap">
                            <asp:Label Text="Trans No.:" runat="server" class="col-sm-3 col-form-label" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtTrans_Num" runat="server" class="form-control" autocomplete="off" />
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" D="RequiredFieldValidator4" runat="Server" ControlToValidate="txtTrans_Num" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto text-nowrap">
                            <asp:Label Text="Document Date:" runat="server" class="col-sm-3 col-form-label" />
                        </div>
                        <div class="col">
                            <input type="date" runat="server" id="dtpDoc_Date" class="form-control">
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto text-nowrap">
                            <asp:Label Text="Status:" runat="server" class="col-sm-3 col-form-label" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtStatus" runat="server" class="form-control" autocomplete="off" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

    </asp:Panel>
</asp:Content>
