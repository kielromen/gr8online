<%@ Page Title="Official Receipt" MaintainScrollPositionOnPostback="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="OfficialReceipt.aspx.vb" Inherits="GR8Books.OfficialReceipt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script src="../Scripts/jquery.formatCurrency-1.4.0.js"></script>

    <script src="../Scripts/umd/popper.min.js"></script>
    <script src="../Scripts/bootstrap.min.js"></script>

    <script type="text/javascript">  
        $(document).ready(function () {
            $('.txtAmount').focusout(function () {
                $('.txtAmount').formatCurrency();
                $('.txtAmount').toNumber().formatCurrency('.txtAmount');
            });
        });

        $(".btnCompute").click(function (e) {
            totalDBCR();
            if ($(".lblTotalDebit_Amount").text() == $(".lblTotalCredit_Amount").text()) {
                e.preventDefault();
            }
        });
    </script>


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
                        url: "<%= ResolveUrl("OfficialReceipt.aspx/ListAccountTitle") %>",
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
                        url: "<%= ResolveUrl("OfficialReceipt.aspx/ListVCE") %>",
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
                        url: "<%= ResolveUrl("OfficialReceipt.aspx/ListVCE") %>",
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

            var id = "test";
            $("#<%=btnSearch.ClientID%>").click(function (e) {
                e.preventDefault();
                var Type = "OR";
                var Url = "OfficialReceipt.aspx";
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

            $(".btnCompute").click(function (e) {
                totalDBCR();
                if ($(".lblTotalDebit_Amount").text() == $(".lblTotalCredit_Amount").text()) {
                    e.preventDefault();
                }
            });

            $(".btnCancel").click(function () {
                if (confirm("Are you sure you want to cancel this transaction?")) {

                }
                else {
                    return false;
                }
            });

            $("#<%=btnCopyFromCASHR.ClientID%>").click(function (e) {
                e.preventDefault();
                var Type = "CASHR";
                var Url = "OfficialReceipt.aspx";
                var Name = $(".txtName").val();
                var myWidth = "900";
                var myHeight = "550";
                var left = (screen.width - myWidth) / 2;
                var top = (screen.height - myHeight) / 4;
                var win = window.open("CopyFrom.aspx?id=" + Type + '&Url=' + Url + '&Name=' + Name, "Load Transaction", 'dialog=yes,resizable=no, width=' + myWidth + ', height=' + myHeight + ', top=' + top + ', left=' + left);
                var timer = setInterval(function () {
                    if (win.closed) {
                        clearInterval(timer);
                    }
                });
            });


            $("#<%=btnCopyFromSJ.ClientID%>").click(function (e) {
                e.preventDefault();
                var Type = "SJ";
                var Url = "OfficialReceipt.aspx";
                var Name = $(".txtName").val();
                var myWidth = "900";
                var myHeight = "550";
                var left = (screen.width - myWidth) / 2;
                var top = (screen.height - myHeight) / 4;
                var win = window.open("CopyFrom.aspx?id=" + Type + '&Url=' + Url + '&Name=' + Name, "Load Transaction", 'dialog=yes,resizable=no, width=' + myWidth + ', height=' + myHeight + ', top=' + top + ', left=' + left);
                var timer = setInterval(function () {
                    if (win.closed) {
                        clearInterval(timer);
                    }
                });
            });

            //-------------NEW

            $("#<%=btnTax.ClientID%>").click(function (e) {
                e.preventDefault();
                var id = $(this).attr("id");
                var amount = parseFloat($("#<%=txtAmount.ClientID%>").val().replace(/,/g, ""));
                $("select#<%=ddlTaxType.ClientID%>")[0].selectedIndex = 0;
                $("select#<%=ddlETaxType.ClientID%>")[0].selectedIndex = 0;
                $("#<%=txtTAmount.ClientID%>").val(parseFloat(amount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                $("#<%=txtTTotalAmount.ClientID%>").val(parseFloat(amount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                $("#<%=txtTNetAmount.ClientID%>").val("0.00");
                $("#<%=txtTTaxAmount.ClientID%>").val("0.00");
                $("#<%=txtTPercent.ClientID%>").val("0.00%");
                $("#<%=txtETaxAmount.ClientID%>").val("0.00");
                $("#<%=txtEPercent.ClientID%>").val("0.00%");
            });

            $(".btnTax_Entry").click(function (e) {
                e.preventDefault();
                var id = $(this).attr("id");
                $("#<%=txtRow.ClientID%>").val(parseFloat(id.split("_")[id.split("_").length - 1]) + 1);
                var amount = parseFloat($("#" + id.replace("btnTax_Entry", "txtDebit_Entry")).val().replace(/,/g, "")) + parseFloat($("#" + id.replace("btnTax_Entry", "txtCredit_Entry")).val().replace(/,/g, ""));
                $("select#<%=ddlTaxType.ClientID%>")[0].selectedIndex = 0;
                $("select#<%=ddlETaxType.ClientID%>")[0].selectedIndex = 0;
                ////$("#<%=txtTAmount.ClientID%>").val("0.00");
                $("#<%=txtTAmount.ClientID%>").val(parseFloat(amount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                ////$("#<%=txtTNetAmount.ClientID%>").val(parseFloat(amount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                $("#<%=txtTTaxAmount.ClientID%>").val("0.00");
                $("#<%=txtTPercent.ClientID%>").val("0.00%");
                $("#<%=txtETaxAmount.ClientID%>").val("0.00");
                $("#<%=txtEPercent.ClientID%>").val("0.00%");
                $("#<%=txtTTotalAmount.ClientID%>").val("0.00");
            });
            //-------------VAT--------------------
            $("#<%=ddlTaxType.ClientID%>").change(function () {
                ComputeTax();
            });

            //-------------EWT--------------------
            $("#<%=ddlETaxType.ClientID%>").change(function () {
                ComputeTax();
            });

            function ComputeTax() {
                var taxcode = $("#<%=ddlTaxType.ClientID%>").val();
                var ewtcode = $("#<%=ddlETaxType.ClientID%>").val();
                console.log(taxcode + " " + ewtcode);
                $.ajax({
                    url: "<%= ResolveUrl("AccountsPayableVoucher.aspx/LoadTaxPercent") %>",
                    data: "{'TaxCode':'" + taxcode + "', 'EWTCode':'" + ewtcode + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {

                        var taxpercent = data.d.split("|")[0];
                        var ewtpercent = data.d.split("|")[1];
                        $("#<%=txtTPercent.ClientID%>").val(taxpercent);
                        $("#<%=txtEPercent.ClientID%>").val(ewtpercent);

                        //TAX COMPUTATION
                        taxpercent = parseFloat(taxpercent.replace("%", "")) / 100;
                        var amount = parseFloat($("#<%=txtTAmount.ClientID%>").val().replace(/,/g, ""));
                        if (amount > 0) {
                            amount = amount / (1 + taxpercent);
                            $("#<%=txtTNetAmount.ClientID%>").val(parseFloat(amount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        } else {
                            $("#<%=txtTAmount.ClientID%>").val(parseFloat($("#<%=txtTNetAmount.ClientID%>").val()).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString())
                        }
                        var taxamount = parseFloat($("#<%=txtTNetAmount.ClientID%>").val().replace(/,/g, "")) * taxpercent;
                        if (isNaN(taxamount)) {
                            taxamount = 0;
                        }
                        $("#<%=txtTTaxAmount.ClientID%>").val(parseFloat(taxamount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        $("#<%=txtTTotalAmount.ClientID%>").val(parseFloat(amount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());

                        //EWT COMPUTATION
                        ewtpercent = parseFloat(ewtpercent.replace("%", "")) / 100;
                        var amount = parseFloat($("#<%=txtTAmount.ClientID%>").val().replace(/,/g, ""));
                        if (amount > 0 && parseFloat($("#<%=txtTNetAmount.ClientID%>").val().replace(/,/g, "")) == 0) {
                            //amount = amount / (1 + percent);
                            $("#<%=txtTNetAmount.ClientID%>").val(parseFloat(amount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        } 
                        var taxamount = parseFloat($("#<%=txtTNetAmount.ClientID%>").val().replace(/,/g, "")) * ewtpercent;
                        if (isNaN(taxamount)) {
                            taxamount = 0;
                        }
                        $("#<%=txtETaxAmount.ClientID%>").val(parseFloat(taxamount).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                        var cash = amount - taxamount;
                        $("#<%=txtTTotalAmount.ClientID%>").val(parseFloat(cash).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
                    }
                });
            }

            function ComputeEWT() {

            }
        });
    </script>
    <asp:Panel ID="panel1" runat="server">

        <div class="row mt-4 justify-content-start">
            <div class="col">
                <asp:Button Text="Search" ID="btnSearch" class="btnSearch btn btn-primary" runat="server" />
                <asp:Button Text="New" ID="btnNew" AutoPostBack="False" runat="server" class="btn btn-primary" />
                <asp:Button Text="Edit" ID="btnEdit" runat="server" class="btn btn-primary" />
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary" runat="server" ValidationGroup="g" />
                <asp:Button Text="Cancel" ID="btnCancel" runat="server" class="btnCancel btn btn-primary" />
                <asp:Button Text="Close" ID="btnClose" runat="server" class="btn btn-primary" />
                <div class="btn-group">
                <button type="button"  runat="server" id="btnCopyFrom" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    Copy From
                  </button>
                  <div class="dropdown-menu">
                    <asp:Button Text="Cash Advance Liquidation" ID="btnCopyFromCASHR" runat="server" class="dropdown-item" />
                    <asp:Button Text="Sales Journal" ID="btnCopyFromSJ" runat="server" class="dropdown-item" />
                  </div>
                </div>
                <asp:Button Text="Prev" ID="btnPrev" runat="server" class="btn btn-primary" />
                <asp:Button Text="Next" ID="btnNext" runat="server" class="btn btn-primary" />
            </div>
        </div>


        <hr />

        <asp:Panel ID="panelConrols" runat="server">
            <div class="row mt mb-5">
                <div class="col-5 my-auto">
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Payment Type:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:DropDownList ID="ddlType" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" runat="server" class="form-control" OnSelectedIndexChanged="WithBank">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator9" runat="Server" ControlToValidate="ddlType" InitialValue="--Select Payment Type--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="VCECode:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCode" runat="server" class="form-control" AutoComplete="off" />
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="txtCode" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="VCEName:" runat="server" />
                        </div>
                        <div class="col">
                            <div class="input-group">
                                <asp:TextBox ID="txtName" runat="server" class="txtName form-control " autocomplete="off" />
                                <div class="input-group-append">
                                    <asp:Button Text="Add New" ID="btnAddNewCustomer" runat="server" class="btn btn-primary" />
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="txtName" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Amount:" runat="server" />
                        </div>
                        <div class="col">
                            <div class="input-group">
                                <asp:TextBox ID="txtAmount" runat="server" class="txtAmount form-control   text-right" autocomplete="off" />
                                <div class="input-group-append">
                                    <asp:Button Text="%" ID="btnTax" runat="server" class="btn btn-primary" data-toggle="modal" data-target="#modalTax" data-whatever="@mdo" />
                                </div>
                            </div>
                            <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator3" runat="Server" ControlToValidate="txtAmount" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Remarks :" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtRemarks" runat="server" class="form-control" AutoComplete="off" />
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Collection Type:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:DropDownList runat="server" ID="ddlCollectionType" class="ddlCollectionType form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="ddlCollectionType_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                    </div>

                </div>
                <div class="col">
                     
                     <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Deposit To:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:DropDownList ID="ddlBank" AutoPostBack="True" runat="server"  AppendDataBoundItems="true" EnableViewState="true"   class="form-control"  OnSelectedIndexChanged="BankDetails">
                            </asp:DropDownList>
                             <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator4" runat="Server" ControlToValidate="ddlBank" InitialValue="--Select Bank--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <asp:Panel ID="panelBank" runat="server">
                        <div id="div_Bank">
                            <div class="row mb-2">
                                <div class="col-4 my-auto">
                                    <asp:Label Text="Bank:" runat="server" />
                                </div>

                                <div class="col">
                                    <asp:TextBox ID="txtBank_Name" runat="server" class="form-control" AutoComplete="off" />
                                </div>
                            </div>


                            <div class="row mb-2">
                                <div class="col-4 my-auto">
                                    <asp:Label Text="Check No:" runat="server" />
                                </div>

                                <div class="col">
                                    <asp:TextBox ID="txtBank_CheckNo" runat="server" class="form-control" AutoComplete="off" />
                                </div>
                            </div>

                            <div class="row mb-2">
                                <div class="col-4 my-auto">
                                    <asp:Label Text="Check Date:" runat="server" />
                                </div>

                                <div class="col">
                                    <input type="date" runat="server" id="dtpBank_Date" class="form-control">
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
                <div class="col">
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="OR No.:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtTrans_Num" runat="server" class="form-control" AutoComplete="off" />
                           <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator5" runat="Server" ControlToValidate="txtTrans_Num" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Document Date:" runat="server" />
                        </div>
                        <div class="col">
                            <input type="date" runat="server" id="dtpDoc_Date" class="form-control">
                        </div>
                    </div>
                    <%--<div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Ref Type:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtRef_Type" runat="server" ReadOnly="true" class="form-control" AutoComplete="off" />
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Ref No.:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtRef_No" runat="server" ReadOnly="true" class="form-control" AutoComplete="off" />
                        </div>
                    </div>--%>
                    <div class="row mb-2">
                        <div class="col-4 my-auto">
                            <asp:Label Text="Status:" runat="server" />
                        </div>
                        <div class="col">
                             <asp:TextBox ID="txtStatus" runat="server" class="form-control " autocomplete="off" />
                        </div>
                    </div>
                </div>
            </div>
        </asp:Panel>

        <asp:Panel ID="panelEntry" runat="server">
            <div class="row mb-2">
                <div class="col">
                    <h5>Entries</h5>
                    <hr />
                </div>
            </div>

            <div style="overflow-y: scroll;">
            <asp:GridView ID="dgvEntry" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" Width="100%" AutoGenerateColumns="false" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="True">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:CommandField ShowDeleteButton="True" ButtonType="Button" DeleteText="X" ControlStyle-CssClass="btn btn-danger" />
                        <asp:BoundField DataField="chNo" HeaderText="No." />
                        <asp:TemplateField HeaderText="Account Code"  HeaderStyle-Wrap="false" >
                            <ItemTemplate>
                                <asp:TextBox ID="txtAccntCode_Entry" Class="txtAccntCode_Entry form-control" runat="server" Width="100px" AutoComplete="off"></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnAdd_Entry" runat="server" class="btn btn-light btn-sm" Text="Add Entry" OnClick="AddNewRow" AutoPostBack="False" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Account Title"  HeaderStyle-Wrap="false" >
                            <ItemTemplate>
                                <asp:TextBox ID="txtAccntTitle_Entry" Class="txtAccntTitle_Entry form-control" runat="server" AutoComplete="off" Width="150px"></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnCompute" runat="server" class="btnCompute btn btn-light btn-sm" Text="Auto Entry" OnClick="ComputeRow" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="Debit">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDebit_Entry" onkeydown="return((event.keyCode==96 ||event.keyCode==97 ||event.keyCode==98 ||event.keyCode==99 ||event.keyCode==100 ||event.keyCode==101 ||event.keyCode==102 ||event.keyCode==103 ||event.keyCode==104 ||event.keyCode==105 ||event.keyCode==106 ||event.keyCode==110 || event.keyCode==190 || event.keyCode==188 || event.keyCode==32 || !(event.keyCode>=65)) && !((event.keyCode==110 || event.keyCode==190) && this.value.split('.').length==2));" Class="txtDebit_Entry form-control" runat="server" Width="110" Text="0.00" Style="text-align: right" AutoComplete="off"></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotalDebit_Amount" Class="lblTotalDebit_Amount" runat="server" Font-Bold="true" Text="0.00" AutoComplete="off"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField FooterStyle-HorizontalAlign="Right" HeaderText="Credit">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCredit_Entry" onkeydown="return((event.keyCode==96 ||event.keyCode==97 ||event.keyCode==98 ||event.keyCode==99 ||event.keyCode==100 ||event.keyCode==101 ||event.keyCode==102 ||event.keyCode==103 ||event.keyCode==104 ||event.keyCode==105 ||event.keyCode==106 ||event.keyCode==110 || event.keyCode==190 || event.keyCode==188 || event.keyCode==32 || !(event.keyCode>=65)) && !((event.keyCode==110 || event.keyCode==190) && this.value.split('.').length==2));" Class="txtCredit_Entry form-control" runat="server" Width="110" Text="0.00" Style="text-align: right" AutoComplete="off"></asp:TextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotalCredit_Amount" runat="server" Class="lblTotalCredit_Amount" Font-Bold="true" Text="0.00" AutoComplete="off"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tax">
                            <ItemTemplate>
                                <asp:Button ID="btnTax_Entry" Class="btnTax_Entry  btn btn-primary" runat="server" Text="%" data-toggle="modal" data-target="#modalTax" data-whatever="@mdo"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Particulars">
                            <ItemTemplate>
                                <asp:TextBox ID="txtParticulars_Entry" Class="txtParticulars_Entry form-control" runat="server" AutoComplete="off" Width="150px"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Code">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCode_Entry" class="txtCode_Entry form-control " runat="server" Width="100px" AutoComplete="off"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:TextBox ID="txtName_Entry" Class="txtName_Entry form-control" runat="server" Width="150px" AutoComplete="off"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Res. Center">
                            <ItemTemplate>
                                <asp:DropDownList runat="server" ID="ddlCostCenter" class="ddlCostCenter form-control" AppendDataBoundItems="true" Width="120px"  AutoPostBack="false" EnableViewState="true">
                                </asp:DropDownList>
                              </ItemTemplate>
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Ref. ID">
                             <ItemTemplate>
                                <asp:TextBox ID="txtRefID_Entry" Class="txtRefID_Entry form-control" runat="server" AutoComplete="off"  Width="100px" ></asp:TextBox>
                             </ItemTemplate>
                         </asp:TemplateField>

                          <asp:TemplateField HeaderText="VAT Type">
                             <ItemTemplate>
                                <asp:TextBox ID="txtVATType" Class="txtVATType form-control" runat="server" AutoComplete="off"  Width="100px" ></asp:TextBox>
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
            </div>

        </asp:Panel>
        
          <%-- Tax --%>
        <div class="modal fade" id="modalTax" tabindex="-1" role="dialog" aria-labelledby="modalLabelTax" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalLabelTax">Tax</h5>
                        <asp:TextBox ID ="txtRow" runat="server"></asp:TextBox>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-3 text-nowrap">
                                    <asp:Label Text="Gross Amount :" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtTAmount" class="form-control text-right" runat="server" AutoComplete="off"></asp:TextBox>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-3 text-nowrap">
                                    <asp:Label Text="Net Amount :" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtTNetAmount" class="form-control text-right" runat="server" AutoComplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <%---------------------VAT---------------------%>
                            <div class="row">
                                <div class="col-3 text-nowrap">
                                    <asp:Label Text="VAT Code :" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:DropDownList runat="server" ID="ddlTaxType" class="ddlTaxType form-control" AppendDataBoundItems="true" AutoPostBack="false" EnableViewState="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-3 text-nowrap">
                                    <asp:Label Text="Percent :" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtTPercent" class="form-control text-right" runat="server" AutoComplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-3 text-nowrap">
                                    <asp:Label Text="VAT Amount :" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtTTaxAmount" class="form-control text-right" runat="server" AutoComplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <%-------------------EWT---------------------%>
                            <div class="row">
                                <div class="col-3 text-nowrap">
                                    <asp:Label Text="EWT Code :" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:DropDownList runat="server" ID="ddlETaxType" class="ddlETaxType form-control" AppendDataBoundItems="true" AutoPostBack="false" EnableViewState="true">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-3 text-nowrap">
                                    <asp:Label Text="Percent :" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtEPercent" class="form-control text-right" runat="server" AutoComplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-3 text-nowrap">
                                    <asp:Label Text="EWT Amount :" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtETaxAmount" class="form-control text-right" runat="server" AutoComplete="off"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-3 text-nowrap">
                                    <asp:Label Text="Amount :" runat="server" />
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtTTotalAmount" class="form-control text-right" runat="server" AutoComplete="off"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button Text="Save" ID="btnSaveTax" class="btnSaveTax btn btn-primary btn-block" runat="server" AutoPostBack="true" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
