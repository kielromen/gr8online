<%@ Page Title="Purchase Order" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="PurchaseOrder.aspx.vb" Inherits="GR8Books.PurchaseOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type='text/javascript'>
        $(document).ready(function () {
            $(".txtAccntCode_Entry").prop("readonly", true);
            $(".txtCode_Entry").prop("readonly", true);
            $(".txtCode_Entry, .txtName_Entry, .txtDescription_Entry, .txtUnitPrice_Entry, .txtUOM_Entry, .txtQTY_Entry, .txtUnitCost_Entry, .txtVATable_Entry, .txtWHS_Entry, .txtAccntCode_Entry, .txtAccntTitle_Entry").keydown(function (e) {
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
                        url: "<%= ResolveUrl("PurchaseOrder.aspx/ListAccountTitle")%>",
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
                        url: "<%= ResolveUrl("PurchaseOrder.aspx/ListItem")%>",
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
                        url: "<%= ResolveUrl("PurchaseOrder.aspx/ListVCE")%>",
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
                window.location = "PurchaseOrder_Loadlist.aspx";
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
                <asp:Button Text="Search" ID="btnSearch" class="btnSearch btn btn-primary" runat="server" />
                <asp:Button Text="New" ID="btnNew" AutoPostBack="False" runat="server" class="btn btn-primary" />
                <asp:Button Text="Edit" ID="btnEdit" runat="server" class="btn btn-primary" />
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary" runat="server" ValidationGroup="g" />
                <asp:Button Text="Cancel" ID="btnCancel" runat="server" class="btn btn-primary" />
                <asp:Button Text="Close" ID="btnClose" runat="server" class="btn btn-primary" />
                <asp:Button Text="Prev" ID="btnPrev" runat="server" class="btn btn-primary" />
                <asp:Button Text="Next" ID="btnNext" runat="server" class="btn btn-primary" />
            </div>
        </div>
        <hr />

        <asp:Panel ID="panelConrols" runat="server">
            <div class="row mb-2">
                <div class="col-sm-8">
                    <div class="row mb-2">
                        <div class="col-2 my-auto">
                            <asp:Label Text="VCE Code:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCode" runat="server" class="form-control" autocomplete="off" />
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-2 my-auto">
                            <asp:Label Text="VCE Name:" runat="server" />
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
                        <div class="col-2 my-auto">
                            <asp:Label Text="Gross Amount:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtGrossAmount" runat="server" class="form-control text-right" type="number" autocomplete="off" />
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtGrossAmount" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-2 my-auto">
                            <asp:Label Text="Discount:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtDiscount" runat="server" class="form-control text-right" type="number" autocomplete="off" />
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtDiscount" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-2 my-auto">
                            <asp:Label Text="VAT Amount:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtVATAmount" runat="server" class="form-control text-right" type="number" autocomplete="off" />
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator5" runat="Server" ControlToValidate="txtVATAmount" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-2 my-auto">
                            <asp:Label Text="Net Amount:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtNetAmount" runat="server" class="form-control text-right" type="number" autocomplete="off" />
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator6" runat="Server" ControlToValidate="txtNetAmount" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-2 my-auto">
                            <asp:Label Text="Address:" runat="server" autocomplete="off" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtAddress" runat="server" class="form-control" autocomplete="off" />
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-2 my-auto">
                            <asp:Label Text="Contact No:" runat="server" autocomplete="off" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtContactNo" runat="server" class="form-control" autocomplete="off" />
                        </div>
                    </div>
                </div>

                <div class="col-sm">
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Ref No.:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtRef_No" runat="server" class="form-control" autocomplete="off" />
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Trans No.:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtTrans_Num" runat="server" class="form-control" autocomplete="off" />
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Trans Date:" runat="server" />
                        </div>
                        <div class="col">
                            <input type="date" runat="server" id="dtpDoc_Date" class="form-control">
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Delivery Date:" runat="server" />
                        </div>
                        <div class="col">
                            <input type="date" runat="server" id="dtpDelivery_Date" class="form-control">
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>
        <%-- Entry --%>
        <asp:Panel ID="panelEntry" runat="server">
            <div class="row mb-2">
                <div class="col">
                    <br />
                    <h5>Entries</h5>
                    <hr />
                </div>
            </div>
            <div style="overflow-y: scroll;">
                <asp:GridView ID="dgvEntry" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="True">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ButtonType="Button" DeleteText="X" />
                        <asp:BoundField DataField="chNo" HeaderText="No." />
                        <asp:TemplateField HeaderText="Item Code">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCode_Entry" class="txtCode_Entry" runat="server" Width="110"></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnAdd_Entry" runat="server" Text="Add Entry" OnClick="AddNewrow" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Item Name">
                            <ItemTemplate>
                                <asp:TextBox ID="txtName_Entry" Class="txtName_Entry" runat="server" autocomplete="off"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDescription_Entry" Class="txtDescription_Entry" runat="server" autocomplete="off"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="UnitPrice">
                            <ItemTemplate>
                                <asp:TextBox ID="txtUnitPrice_Entry" onkeydown="return((event.keyCode==110 || event.keyCode==190 || event.keyCode==188 || event.keyCode==32 || !(event.keyCode>=65)) && !((event.keyCode==110 || event.keyCode==190) && this.value.split('.').length==2));" Class="txtUnitPrice_Entry" runat="server" Width="110" Text="0.00" Style="text-align: right" autocomplete="off"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="UOM">
                            <ItemTemplate>
                                <asp:TextBox ID="txtUOM_Entry" Class="txtUOM_Entry" runat="server" autocomplete="off"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="QTY">
                            <ItemTemplate>
                                <asp:TextBox ID="txtQTY_Entry" onkeydown="return((event.keyCode==110 || event.keyCode==190 || event.keyCode==188 || event.keyCode==32 || !(event.keyCode>=65)) && !((event.keyCode==110 || event.keyCode==190) && this.value.split('.').length==2));" Class="txtQTY_Entry" runat="server" Width="110" Text="0.00" Style="text-align: right" autocomplete="off"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <%--<asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="UnitCost">
                    <ItemTemplate>
                        <asp:TextBox ID="txtUnitCost_Entry"  onkeydown = "return((event.keyCode==110 || event.keyCode==190 || event.keyCode==188 || event.keyCode==32 || !(event.keyCode>=65)) && !((event.keyCode==110 || event.keyCode==190) && this.value.split('.').length==2));" Class="txtUnitCost_Entry" runat="server" Width="110" Text="0.00" Style="text-align: right" autocomplete="off" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="GrossAmount">
                    <ItemTemplate>
                        <asp:TextBox ID="txtGrossAmount_Entry"  onkeydown = "return((event.keyCode==110 || event.keyCode==190 || event.keyCode==188 || event.keyCode==32 || !(event.keyCode>=65)) && !((event.keyCode==110 || event.keyCode==190) && this.value.split('.').length==2));" Class="txtGrossAmount_Entry" runat="server" Width="110" Text="0.00" Style="text-align: right" autocomplete="off" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="VATAmount">
                    <ItemTemplate>
                        <asp:TextBox ID="txtVATAmount_Entry"  onkeydown = "return((event.keyCode==110 || event.keyCode==190 || event.keyCode==188 || event.keyCode==32 || !(event.keyCode>=65)) && !((event.keyCode==110 || event.keyCode==190) && this.value.split('.').length==2));" Class="txtVATAmount_Entry" runat="server" Width="110" Text="0.00" Style="text-align: right" autocomplete="off" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="VATInc">
                    <ItemTemplate>
                        <asp:checkbox ID="txtVATInc_Entry"  Class="txtVATInc_Entry" runat="server" Width="110" autocomplete="off" ></asp:checkbox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="DiscountRate">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDiscountRate_Entry"  onkeydown = "return((event.keyCode==110 || event.keyCode==190 || event.keyCode==188 || event.keyCode==32 || !(event.keyCode>=65)) && !((event.keyCode==110 || event.keyCode==190) && this.value.split('.').length==2));" Class="txtDiscountRate_Entry" runat="server" Width="110" Text="0.00" Style="text-align: right" autocomplete="off" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="Discount">
                    <ItemTemplate>
                        <asp:TextBox ID="txtDiscount_Entry"  onkeydown = "return((event.keyCode==110 || event.keyCode==190 || event.keyCode==188 || event.keyCode==32 || !(event.keyCode>=65)) && !((event.keyCode==110 || event.keyCode==190) && this.value.split('.').length==2));" Class="txtDiscount_Entry" runat="server" Width="110" Text="0.00" Style="text-align: right" autocomplete="off" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="NetAmount">
                    <ItemTemplate>
                        <asp:TextBox ID="txtNetAmount_Entry"  onkeydown = "return((event.keyCode==110 || event.keyCode==190 || event.keyCode==188 || event.keyCode==32 || !(event.keyCode>=65)) && !((event.keyCode==110 || event.keyCode==190) && this.value.split('.').length==2));" Class="txtNetAmount_Entry" runat="server" Width="110" Text="0.00" Style="text-align: right" autocomplete="off" ></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="VATable">
                            <ItemTemplate>
                                <asp:CheckBox ID="txtVATable_Entry" Class="txtVATable_Entry" runat="server" Width="110" Style="text-align: right"></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="WHS">
                            <ItemTemplate>
                                <asp:TextBox ID="txtWHS_Entry" Class="txtWHS_Entry" runat="server" autocomplete="off"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account Code">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAccntCode_Entry" Class="txtAccntCode_Entry" runat="server" Width="110" required="true" autocomplete="off"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account Title">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAccntTitle_Entry" Class="txtAccntTitle_Entry" runat="server" autocomplete="off"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>



                        <%--                <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="Credit">
                    <ItemTemplate>
                        <asp:TextBox  ID="txtCredit_Entry"  onkeydown = "return((event.keyCode==110 || event.keyCode==190 || event.keyCode==188 || event.keyCode==32 || !(event.keyCode>=65)) && !((event.keyCode==110 || event.keyCode==190) && this.value.split('.').length==2));" Class="txtCredit_Entry" runat="server" Width="110" Text="0.00" Style="text-align: right" autocomplete="off"></asp:TextBox>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lblTotalCredit_Amount" runat="server" Class="lblTotalCredit_Amount" Font-Bold="true" Text="0.00"></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>--%>
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
            </div>
        </asp:Panel>
    </asp:Panel>

</asp:Content>
