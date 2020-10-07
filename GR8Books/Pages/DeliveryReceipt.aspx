<%@ Page Title="Delivery Receipt" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="DeliveryReceipt.aspx.vb" Inherits="GR8Books.DeliveryReceipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type='text/javascript'>
        $(document).ready(function () {
            $(".txtAccntCode_Entry").prop("readonly", true);
            $(".txtCode_Entry").prop("readonly", true);
            $(".txtAccntCode_Entry,.txtAccntTitle_Entry,.txtUOM_Entry,.txtQTY_Entry,.txtUnitPrice_Entry,.txtDeliveryAmount_Entry,.txtGrossAmount_Entry,.txtNetAmount_Entry,.txtWHSE_Entry,.txtReference_Entry").keydown(function (e) {
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
                        url: "<%= ResolveUrl("DeliveryReceipt.aspx/ItemList")%>",
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
                        url: "<%= ResolveUrl("DeliveryReceipt.aspx/ListVCE")%>",
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
                        url: "<%= ResolveUrl("DeliveryReceipt.aspx/ListVCE")%>",
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

            $(".txtDebit_Entry,.txtCredit_Entry").keyup(function (e) {
                totalDBCR();
            });
            totalDBCR();
            function totalDBCR() {
                var totaldebit = 0;
                var totalcredit = 0;
                $(".txtDebit_Entry").each(function () {
                    if (this.value != "") {
                        totaldebit += parseFloat(this.value.replace(/,/g, ""));
                    }
                });
                $(".txtCredit_Entry").each(function () {
                    if (this.value != "") {
                        totalcredit += parseFloat(this.value.replace(/,/g, ""));
                    }
                });
                $(".lblTotalDebit_Amount").text(parseFloat(totaldebit).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                $(".lblTotalCredit_Amount").text(parseFloat(totalcredit).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
            };
            $("#<%=btnSave.ClientID%>").click(function (e) {
                totalDBCR();
                var totaldebit = $(".lblTotalDebit_Amount").text();
                var totalcredit = $(".lblTotalCredit_Amount").text();
                if (totaldebit != totalcredit) {
                    e.preventDefault
                    alert("Total Debit Credit No Match");
                    return false;
                }
                else {

                    if (Page_IsValid) {
                        if (confirm("Are you sure you want to save?")) {

                        }
                        else {
                            return false;
                        }
                    }
                }
            });


            $(".btnSearch").click(function () {
                window.location = "DeliveryReceipt_Loadlist.aspx";
                return false;
            });

        });
    </script>

    <script type="text/javascript">
        function Comma(Num) {
            Num += '';
            Num = Num.replace(/,/g, '');
            x = Num.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d)((\d)(\d{2}?)+)$/;
            while (rgx.test(x1))
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            return x1 + x2;

        }
    </script>
    <asp:Panel ID="Panel1" runat="server">
        <div class="row mt-4 justify-content-start">
            <div class="col">
                <asp:Button Text="Search" ID="btnSearch" CssClass="" runat="server" class="btnSearch btn btn-primary" />
                <asp:Button Text="New" ID="btnNew" AutoPostBack="False" runat="server" class="btn btn-primary" />
                <asp:Button Text="Edit" ID="btnEdit" runat="server" class="btn btn-primary" />
                <asp:Button Text="Save" ID="btnSave" runat="server" ValidationGroup="g" class=" btnSave btn btn-primary" />
                <asp:Button Text="Cancel" ID="btnCancel" runat="server" class="btn btn-primary" />
                <asp:Button Text="Close" ID="btnClose" runat="server" class="btn btn-primary" />
                <asp:Button Text="Prev" ID="btnPrev" runat="server" class="btn btn-primary" />
                <asp:Button Text="Next" ID="btnNext" runat="server" class="btn btn-primary" />
            </div>
        </div>
        <hr />

        <asp:Panel ID="panelConrols" runat="server">
            <div class="row mb-2">
                <div class="col">
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Code:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCode" runat="server" class="form-control" autocomplete="off" />
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Name:" runat="server" />
                        </div>
                        <div class="col">
                            <div class="input-group">
                                <asp:TextBox ID="txtName" runat="server" class="form-control" autocomplete="off" />
                                <div class="input-group-append">
                                    <asp:Button Text="Add New" ID="btnAddNewCustomer" runat="server" class="btn btn-primary" />
                                </div>
                            </div>
                        <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Plate No.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtPlateNo" runat="server" class="form-control text-right" autocomplete="off" />
                        <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtPlateNo" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Driver Name:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtDriverName" runat="server" class="form-control text-right" autocomplete="off" />
                        <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtDriverName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Remarks:" runat="server" autocomplete="off" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtRemarks" runat="server" class="form-control" autocomplete="off" />
                    </div>
                </div>
            </div>


            <div class="col">
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Ref.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtRef_No" runat="server" class="form-control" autocomplete="off" />
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Trans No.:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtTrans_Num" runat="server" class="form-control" autocomplete="off" />
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Document Date:" runat="server" />
                    </div>
                    <div class="col">
                        <input type="date" runat="server" id="dtpDoc_Date" class="form-control">
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Delivery Date:" runat="server" />
                    </div>
                    <div class="col">
                        <input type="date" runat="server" id="dtpDel_Date" class="form-control">
                    </div>
                </div>
            </div>
            </div>

        </asp:Panel>
        <%-- Entry --%>
        <asp:Panel ID="panelEntry" runat="server">
            <div class="row mb-2">
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
                    <asp:TemplateField HeaderText="Item Code">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAccntCode_Entry" Class="txtAccntCode_Entry" runat="server" Width="110" required="true" autocomplete="off"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnAdd_Entry" runat="server" Text="Add Entry" OnClick="AddNewRow" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item Title">
                        <ItemTemplate>
                            <asp:TextBox ID="txtAccntTitle_Entry" Class="txtAccntTitle_Entry" runat="server" autocomplete="off"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UOM">
                        <ItemTemplate>
                            <asp:TextBox ID="txtUOM_Entry" Class="txtUOM_Entry" runat="server" autocomplete="off"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="QTY">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQTY_Entry" Class="txtQTY_Entry" runat="server" Width="100%" autocomplete="off"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit Price">
                        <ItemTemplate>
                            <asp:TextBox ID="txtUnitPrice_Entry" Class="txtUnitPrice_Entry" runat="server" Width="100%" autocomplete="off"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Delivery Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDeliveryAmount_Entry" Class="txtDeliveryAmount_Entry" runat="server" Width="100%" autocomplete="off"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Gross Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txtGrossAmount_Entry" Class="txtGrossAmount_Entry" runat="server" Width="100%" autocomplete="off"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Net Amount">
                        <ItemTemplate>
                            <asp:TextBox ID="txtNetAmount_Entry" Class="txtNetAmount_Entry" runat="server" Width="100%" autocomplete="off"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Warehouse ">
                        <ItemTemplate>
                            <asp:TextBox ID="txtWHSE_Entry" Class="txtWHSE_Entry" runat="server" Width="100%" autocomplete="off"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ref. ">
                        <ItemTemplate>
                            <asp:TextBox ID="txtReference_Entry" Class="txtReference_Entry" runat="server" Width="100%" autocomplete="off"></asp:TextBox>
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
    </asp:Panel>

</asp:Content>
