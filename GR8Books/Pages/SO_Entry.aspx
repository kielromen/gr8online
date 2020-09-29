<%@ Page Title="Sales Order" Language="vb" AutoEventWireup="false" CodeBehind="SO_Entry.aspx.vb" Inherits="GR8Books.SO_Entry" MasterPageFile="~/Master/Dashboard.Master" %>

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
            $(".txtItemCode").prop("readonly", true);
            $(".txtItemCode,.txtItemDesc,.txtQTY,.txtUOM,.txtItemPrice,.txtTotalPrice").keydown(function (e) {
                if (e.keyCode == 13) {
                    e.preventDefault;
                    return false;
                }
            })

            $(".txtItemDesc").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("SO_Entry.aspx/ListAccountTitle") %>",
                        data: "{'prefix': '" + request.term + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            response($.map(data.d, function (item) {
                                return {
                                    id: item.split('--')[1],
                                    value: item.split('--')[0],
                                    uom: item.split('--')[2],
                                    price: item.split('--')[3]
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
                    var desc = $(this).attr('id');
                    var id = desc.replace("txtItemDesc", "txtItemCode");
                    $("#" + id).val(i.item.id);
                    var uom = desc.replace("txtItemDesc", "txtUOM");
                    $("#" + uom).val(i.item.uom);
                    var price = desc.replace("txtItemDesc", "txtItemPrice");
                    $("#" + price).val(i.item.price);
                },
                minLength: 1
            });


            $(".txtQty").keyup(function () {
                var id = $(this).attr('id');
                var qty = $(this).val();
                var price = $("#" + id.replace("txtQty", "txtItemPrice")).val();
                $("#" + id.replace("txtQty", "txtTotalPrice")).val(parseFloat(qty * price).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                totalAmount();
            });
            totalAmount();
            function totalAmount() {
                var Total = 0;
                var Price = 0;
                $(".txtTotalPrice").each(function () {
                    Total += parseFloat(this.value.replace(/,/g, ""));
                    if (this.value != "") {
                    }
                });
                $(".txtItemPrice").each(function () {
                    if (this.value != "") {
                        Price += parseFloat(this.value.replace(/,/g, ""));
                    }
                });
                $(".lblTotalPrice").text(parseFloat(Total).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                $(".lblItemPrice").text(parseFloat(Price).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
            };


            $("#<%=btnSearch.ClientID%>").click(function (e) {
                e.preventDefault();
                var Type = "SO";
                var Url = "SO_Entry.aspx";
                var myWidth = "700";
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
        });

    </script>

    <div>
        <asp:Panel ID="Panel1" runat="server">
            <div class="row mb-2">
                <div class="col">
                    <asp:Button Text="Search" ID="btnSearch" class="btn btn-primary" runat="server" />
                    <asp:Button Text="New" ID="btnNew" class="btn btn-primary" runat="server" />
                    <asp:Button Text="Edit" ID="btnEdit" class="btn btn-primary" runat="server" />
                    <asp:Button Text="Save" ID="btnSave" class="btn btn-primary" runat="server" ValidationGroup="g" />
                    <asp:Button Text="Cancel" ID="btnCancel" class="btn btn-primary" runat="server" />
                    <asp:Button Text="Close" ID="btnClose" class="btn btn-primary" runat="server" />
                </div>
            </div>
        </asp:Panel>
        <hr />
        <asp:Panel ID="panelConrols" runat="server">
            <div class="row mb-2">
                <div class="col-7">
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Customer Code:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCode" runat="server" class="form-control" />
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtCode" ErrorMessage="Field is required." ValidationGroup="g" ForeColor="#ff0000"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Name:" runat="server" />
                        </div>

                        <div class="col">
                            <asp:TextBox ID="txtName" runat="server" class="form-control" />
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Address:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtAddress" runat="server" class="form-control" />
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Amount:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtAmount" runat="server" class="txtAmount form-control" />
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Remarks:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtRemarks" runat="server" class="form-control" />
                        </div>
                    </div>
                </div>
                <div class="col">
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="SO No.:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtTrans_Num" runat="server" class="form-control" />
                            <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtCode" ErrorMessage="Field is required." ValidationGroup="g" ForeColor="#ff0000"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="SO Date:" runat="server" />
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
                            <input type="date" runat="server" id="dtpDoc_Date2" class="form-control">
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Salesman:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtSalesman" runat="server" ReadOnly="true" class="form-control" />
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="PO/Ref No.:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtRef_No" runat="server" ReadOnly="true" class="form-control" />
                        </div>
                    </div>
                </div>
            </div>
            <br />
        </asp:Panel>
        <asp:Panel ID="panelEntry" runat="server">
            <div class="row mb-2">
                <div class="col">
                    <hr />
                </div>
            </div>
            <asp:GridView ID="dgvEntry" runat="server" CellPadding="4" GridLines="None" Width="100%" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="True">
                <AlternatingRowStyle BackColor="WhiteSmoke" />
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ButtonType="Button" DeleteText="X" />
                    <asp:BoundField DataField="chNo" HeaderText="No." />
                    <asp:TemplateField HeaderText="Item Code">
                        <ItemTemplate>
                            <asp:TextBox ID="txtItemCode" Class="txtItemCode" runat="server" Width="110"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnAdd_Entry" runat="server" Text="Add Entry" OnClick="AddNewRow" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Item Name">
                        <ItemTemplate>
                            <asp:TextBox ID="txtItemDesc" Class="txtItemDesc" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="QTY">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQty" Class="txtQty" runat="server"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="UOM">
                        <ItemTemplate>
                            <asp:TextBox ID="txtUOM" class="txtUOM" runat="server" Width="110"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <div style="text-align: right;">
                                <span style="font-weight: bold;">Total: </span>
                            </div>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-HorizontalAlign="Center" HeaderText="Unit Price">
                        <ItemTemplate>
                            <asp:TextBox ID="txtItemPrice" Class="txtItemPrice" runat="server" Width="110" Text="0.00" Style="text-align: right"> </asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblItemPrice" runat="server" Class="lblItemPrice" Font-Bold="true" Text="0.00"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField FooterStyle-HorizontalAlign="Center" HeaderText="Total Price">
                        <ItemTemplate>
                            <asp:TextBox ID="txtTotalPrice" Class="txtTotalPrice" runat="server" Width="110" Text="0.00" Style="text-align: right"></asp:TextBox>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblTotalPrice" runat="server" Class="lblTotalPrice" Font-Bold="true" Text="0.00"></asp:Label>
                        </FooterTemplate>
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
    </div>
</asp:Content>
