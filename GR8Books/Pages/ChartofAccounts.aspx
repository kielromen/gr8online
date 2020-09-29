<%@ Page Title="Chart of Accounts" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="ChartofAccounts.aspx.vb" Inherits="GR8Books.ChartofAccounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/jquery.mask.js"></script>
    <script type="text/javascript">                
        $(document).ready(function () {
            $('.txtCode').click(
                function () {
                    $(".txtCode").mask("0000000");
                });
            $('.txtCode').keypress(
                function () {
                    $(".txtCode").mask("0000000");
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
            $('#<%= ddlGroup.ClientID%>').change(function () {
                if ($('#<%=ddlGroup.ClientID%> :selected').text() == "Sub Account") {
                    document.getElementById("divWithSub").style.visibility = "visible";
                    return;
                }
                else {
                    document.getElementById("divWithSub").style.visibility = "hidden";
                    return;
                }
            });

        });

    </script>

    <script type="text/javascript">
        function RefreshParent() {
            if (window.opener != null && !window.opener.closed) {
                window.opener.location.reload();
            }
        }
        window.onbeforeunload = RefreshParent;
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.txtCode').keyup(
                function () {
                    var username = $(".txtCode").val();
                    var len = username.length;
                    $.ajax({
                        type: "POST",
                        url: "<%= ResolveUrl("ChartofAccounts.aspx/CheckAccountCode") %>",
                        data: '{AccountCode: "' + username + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var message = $("#message")
                            if (len < 7) {
                                message.css("color", "red");
                                message.html("Code must be 7 digits.");
                                $('.btnSave').prop('disabled', true);
                            }
                            else if (response.d) {
                                message.css("color", "green");
                                message.html("Code is available.");
                                $('.btnSave').prop('disabled', false);
                            }
                            else {
                                message.css("color", "red");
                                message.html("Code is not available.");
                                $('.btnSave').prop('disabled', true);
                            }
                        }
                    });
                });
        });
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $('.txtDescription').keyup(
                function () {
                    var username = $(".txtDescription").val();
                    $.ajax({
                        type: "POST",
                        url: "<%= ResolveUrl("ChartofAccounts.aspx/CheckDescription") %>",
                        data: '{Description: "' + username + '" }',
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                            var message = $("#message1");
                            if (response.d) {
                                message.css("color", "green");
                                message.html("Description is available.");
                            }
                            else {
                                message.css("color", "red");
                                message.html("Description is not available.");
                            }
                        }
                    });
                });
        });
    </script>

    <script type="text/javascript">
        $('body').on('keydown', 'input, select', function (e) {
            if (e.key === "Enter") {
                var self = $(this), form = self.parents('form:eq(0)'), focusable, next;
                focusable = form.find('input,a,select,button,textarea').filter(':visible');
                next = focusable.eq(focusable.index(this) + 1);
                if (next.length) {
                    next.focus();
                } else {
                    form.submit();
                }
                return false;
            }
        });
    </script>

    <div class="alert alert-success alert-dismissible fade show" role="alert" id="alertSave" runat="server">
        <strong>Record successfully saved!</strong>
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>
    <div class="alert alert-warning alert-dismissible fade show" role="alert" id="alertUpdate" runat="server">
        <strong>Record successfully updated!</strong>
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>


    <asp:Panel runat="server" ID="panelCOA">
        <form class="needs-validation" novalidate>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Type:" runat="server" />
            </div>
            <div class="col">
                <asp:DropDownList ID="ddlType" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control" OnSelectedIndexChanged="GetMaxOrderNo" AutoPostBack="true" aria-describedby="validationServer"></asp:DropDownList>
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"    Display="Dynamic" ID="RequiredFieldValidator5" runat="Server" ControlToValidate="ddlType" InitialValue="--Select Type--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>
            </form>
        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Group:" runat="server" />
            </div>
            <div class="col">
                <asp:DropDownList ID="ddlGroup" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"   Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="ddlGroup" InitialValue="--Select Group--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Code:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtCode" class="txtCode form-control" runat="server" placeholder="7 Digits" autocomplete="off" />
                <span id="message" class="message"></span>
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"   Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Description:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtDescription" class="txtDescription form-control" runat="server" autocomplete="off" />
                <span id="message1" class="message1"></span>
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"   Display="Dynamic" ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtDescription" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>

        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Report Alias:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtReportAlias" class="txtReportAlias form-control" runat="server" autocomplete="off"  />
            </div>
        </div>

        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label Text="Order No:" runat="server" />
            </div>
            <div class="col">
                <asp:TextBox ID="txtOrderNo" class="txtOrderNo form-control" runat="server" autocomplete="off" type="number" />
            </div>
        </div>

        <div class="row mb-2">
            <div class="col-2 my-auto">
                <asp:Label ID="lblNature" Text="Nature:" runat="server" />
            </div>
            <div class="col">
                <asp:DropDownList ID="ddlNature" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                <asp:RequiredFieldValidator Forecolor="Red" Font-size="Small"   Display="Dynamic" ID="RequiredFieldValidator4" runat="Server" ControlToValidate="ddlNature" InitialValue="--Select Account Nature--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
            </div>
        </div>


        <div class="row" id="divWithSub">
            <div class="col text-right my-auto">
                <asp:CheckBox Text="&nbsp;With Subsidiary Ledger" ID="chkWithSub" runat="server" class="chkWithSub" />
            </div>
        </div>


        <div class="row mt-4 justify-content-end">
            <div class="col-sm-2 mb-2">
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary btn-block" runat="server" ValidationGroup="g" />
            </div>
            <div class="col-sm-2">
                <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary btn-block" runat="server" />
            </div>
        </div>
    </asp:Panel>

</asp:Content>
