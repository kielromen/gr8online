<%@ Page Title="Cash Disbursement" Language="vb" AutoEventWireup="false" CodeBehind="CashDisbursement_Entry.aspx.vb" Inherits="GR8Books.CashDisbursment_Entry" MasterPageFile="~/Master/Dashboard.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>


    <script type='text/javascript'>
        $(document).ready(function () {
            $(".txtAccntCode_Entry").prop("readonly", true);
            $(".txtCode_Entry").prop("readonly", true);
            $(".txtAccntCode_Entry,.txtAccntTitle_Entry,.txtParticulars_Entry,.txtDebit_Entry,.txtCredit_Entry,.txtCode_Entry,.txtName_Entry,.txtRefID_Entry").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault;
                    return false;
                }
            })
            $(".txtDebit_Entry,.txtCredit_Entry").focus(function () {
                $(this).select();
            });

            $(".txtAccntTitle_Entry").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("CashDisbursement_Entry.aspx/ListAccountTitle") %>",
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
                        url: "<%= ResolveUrl("CashDisbursement_Entry.aspx/ListVCE") %>",
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
                        url: "<%= ResolveUrl("CashDisbursement_Entry.aspx/ListVCE") %>",
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
        });
    </script>


    <asp:Panel ID="Panel1" runat="server">
        <div class="row mt-4 justify-content-start">
            <div class="col-2">
                <asp:Button Text="Search" ID="btnSearch" runat="server" class="btn btn-primary btn-block" />
            </div>
            <div class="col-2">
                <asp:Button Text="New" ID="btnNew" runat="server" class="btn btn-primary btn-block" />
            </div>
            <div class="col-2">
                <asp:Button Text="Edit" ID="btnEdit" runat="server" class="btn btn-primary btn-block" />
            </div>
            <div class="col-2">
                <asp:Button Text="Save" ID="btnSave" runat="server" ValidationGroup="g" class="btn btn-primary btn-block" />
            </div>
            <div class="col-2">
                <asp:Button Text="Cancel" ID="btnCancel" runat="server" class="btn btn-primary btn-block" />
            </div>
        </div>

        <hr />

        <div class="row mt-5 mb-5">
            <div class="col-5">
                <div class="row">
                    <div class="col-3">
                        <asp:Label Text="Code:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtCode" runat="server" class="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-3">
                        <asp:Label Text="Name:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtName" runat="server" class="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-3">
                        <asp:Label Text="Amount:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtAmount" runat="server" class="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row">
                    <div class="col-3">
                        <asp:Label Text="Payment Type:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlType" AutoPostBack="True" runat="server" class="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>
            <div class="col-3">
                <div class="row mb-4">
                    <div class="col-4">
                        <asp:Label Text="Bank:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlBank_List" AutoPostBack="True" runat="server" class="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row mb-4">
                    <div class="col-4">
                        <asp:Label Text="Name:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBank_CheckName" runat="server" class="form-control" />
                    </div>
                </div>
                <div class="row mb-4">
                    <div class="col-4">
                        <asp:Label Text="Check Date:" runat="server" />
                    </div>
                    <div class="col">
                        <input type="date" runat="server" id="dtpBank_Date" class="form-control">
                    </div>
                </div>
                <div class="row mb-4">
                    <div class="col-4">
                        <asp:Label Text="Check No:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBank_CheckNo" runat="server" class="form-control" />
                    </div>
                </div>
                <div class="row mb-4">
                    <div class="col-4">
                        <asp:Label Text="Check amount:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBank_CheckAmount" runat="server" class="form-control" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-4">
                        <asp:Label Text="Check Status:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtBank_CheckStatus" runat="server" class="form-control" />
                    </div>
                </div>
            </div>
            <div class="col">
                <div class="row mb-4">
                    <div class="col-5">
                        <asp:Label Text="CV No.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtTrans_Num" runat="server" ReadOnly="true" class="form-control" />
                    </div>
                </div>
                <div class="row mb-4">
                    <div class="col-5">
                        <asp:Label Text="Document Date:" runat="server" />
                    </div>
                    <div class="col">
                        <input type="date" runat="server" id="dtpDoc_Date" class="form-control">
                    </div>
                </div>
                <div class="row mb-4">
                    <div class="col-5">
                        <asp:Label Text="Ref Type:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtRef_Type" runat="server" ReadOnly="true" class="form-control" />
                    </div>
                </div>
                <div class="row">
                    <div class="col-5">
                        <asp:Label Text="Ref No.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtRef_No" runat="server" ReadOnly="true" class="form-control" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col">
                <h5>Entries</h5>
                <hr />
            </div>
        </div>
        <asp:GridView ID="dgvEntry" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="True">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:CommandField ShowDeleteButton="True" ButtonType="Button" DeleteText="X" />
                <asp:BoundField DataField="chNo" HeaderText="No." />
                <asp:TemplateField HeaderText="Account Code">
                    <ItemTemplate>
                        <asp:TextBox ID="txtAccntCode_Entry" Class="txtAccntCode_Entry" runat="server" Width="110"></asp:TextBox>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button ID="btnAdd_Entry" runat="server" Text="Add Entry" OnClick="AddNewRow" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Account Title">
                    <ItemTemplate>
                        <asp:TextBox ID="txtAccntTitle_Entry" Class="txtAccntTitle_Entry" runat="server"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Particulars">
                    <ItemTemplate>
                        <asp:TextBox ID="txtParticulars_Entry" Class="txtParticulars_Entry" runat="server"></asp:TextBox>
                    </ItemTemplate>
                    <FooterTemplate>
                        <div style="text-align: right;">
                            <span style="font-weight: bold;">Total: </span>
                        </div>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="Debit">
                    <ItemTemplate>
                        <asp:TextBox OnTextChanged="TotalDBCR" ID="txtDebit_Entry" Class="txtDebit_Entry" runat="server" Width="110" Text="0.00" Style="text-align: right"></asp:TextBox>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalDebit_Amount" Class="lblTotalDebit_Amount" runat="server" Font-Bold="true"> </asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="Credit">
                    <ItemTemplate>
                        <asp:TextBox ID="txtCredit_Entry" Class="txtCredit_Entry" runat="server" Width="110" Text="0.00" Style="text-align: right"></asp:TextBox>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalCredit_Amount" runat="server" Class="lblTotalCredit_Amount" Font-Bold="true"></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Code">
                    <ItemTemplate>
                        <asp:TextBox ID="txtCode_Entry" class="txtCode_Entry" runat="server" Width="110"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Name">
                    <ItemTemplate>
                        <asp:TextBox ID="txtName_Entry" Class="txtName_Entry" runat="server"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Reference ID">
                    <ItemTemplate>
                        <asp:TextBox ID="txtRefID_Entry" Class="txtRefID_Entry" runat="server" Width="100%"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EditRowStyle BackColor="#2461BF" />
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#EFF3FB" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F5F7FB" />
            <SortedAscendingHeaderStyle BackColor="#6D95E1" />
            <SortedDescendingCellStyle BackColor="#E9EBEF" />
            <SortedDescendingHeaderStyle BackColor="#4870BE" />
        </asp:GridView>
    </asp:Panel>

</asp:Content>
