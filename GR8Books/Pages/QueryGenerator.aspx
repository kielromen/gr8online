<%@ Page Title="Query Generator" EnableEventValidation = "false" MaintainScrollPositionOnPostback="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="QueryGenerator.aspx.vb" Inherits="GR8Books.QueryGenerator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".txtFromAccountTitle").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("QueryGenerator.aspx/ListAccountTitle") %>",
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
                    $(".hfFromAccountCode").val(i.item.id);
                },
                minLength: 1
            });

            $(".txtToAccountTitle").autocomplete({
                source: function (request, response) {
                    $.ajax({
                        url: "<%= ResolveUrl("QueryGenerator.aspx/ListAccountTitle") %>",
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
                    $(".hfToAccountCode").val(i.item.id);
                },
                minLength: 1
            });
        });
    </script>
    <div class="row row-cols-2">
        <div class="col-sm-3">
            <h6>Financial Period</h6>
            <hr />
            <div class="row">
                <asp:Label Text="Fiscal" runat="server" class="col-sm-3 col-form-label col-form-label-sm" />
                <div class="col-9">
                    <asp:DropDownList ID="ddlFiscal" runat="server" class="form-control form-control-sm" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="LoadPeriod"></asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <asp:Label Text="Year" runat="server" class="col-sm-3 col-form-label col-form-label-sm" />
                <div class="col-9">
                    <asp:TextBox ID="txtYear" runat="server" class="form-control form-control-sm" autocomplete="off" type="number" />
                </div>
            </div>
            <div class="row">
                <asp:Label Text="Month" runat="server" class="col-sm-3 col-form-label col-form-label-sm" />
                <div class="col-9">
                    <asp:DropDownList ID="ddlMonth" runat="server" class="form-control form-control-sm" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="LoadPeriod"></asp:DropDownList>
                </div>
            </div>
            <div class="row">
                <asp:Label Text="From" runat="server" class="col-sm-3 col-form-label col-form-label-sm" />
                <div class="col-9">
                    <input type="date" runat="server" id="dtpFrom" class="form-control form-control-sm">
                </div>
            </div>
            <div class="row">
                <asp:Label Text="To" runat="server" class="col-sm-3 col-form-label col-form-label-sm" />
                <div class="col-9">
                    <input type="date" runat="server" id="dtpTo" class="form-control form-control-sm">
                </div>
            </div>
            <hr />
            <h6>Book</h6>
            <hr />
            <div class="row">
                <div class="col">
                    <asp:CheckBox ID="rbBB" Text="BB" runat="server" GroupName="Book" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
                <div class="col">
                    <asp:CheckBox ID="rbCRB" Text="CRB" runat="server" GroupName="Book" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
                <div class="col">
                    <asp:CheckBox ID="rbCDB" Text="CDB" runat="server" GroupName="Book" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <asp:CheckBox ID="rbGJ" Text="GJ" runat="server" GroupName="Book" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
                <div class="col">
                    <asp:CheckBox ID="rbPB" Text="PB" runat="server" GroupName="Book" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
                <div class="col">
                    <asp:CheckBox ID="rbSB" Text="SB" runat="server" GroupName="Book" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <asp:CheckBox ID="rbPEC" Text="PEC" runat="server" GroupName="Book" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
                <div class="col"></div>
                <div class="col"></div>
            </div>
            <hr />
            <h6>Report Type</h6>
            <hr />
            <div class="row">
                <div class="col">
                    <asp:RadioButton ID="rbSummary" Text="Summary" runat="server" GroupName="ReportType" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
                <div class="col">
                    <asp:RadioButton ID="rbDetailed" Text="Detailed" runat="server" GroupName="ReportType" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
            </div>
            <hr />
            <h6>Account Group</h6>
            <hr />
            <div class="row">
                <asp:Label Text="From" runat="server" class="col-sm-3 col-form-label col-form-label-sm" />
                <div class="col-9">
                    <asp:TextBox ID="txtFromAccountTitle" runat="server" class="txtFromAccountTitle form-control form-control-sm" autocomplete="off" />
                </div>
            </div>
            <div class="row">
                <asp:Label Text="To" runat="server" class="col-sm-3 col-form-label col-form-label-sm" />
                <div class="col-9">
                    <asp:TextBox ID="txtToAccountTitle" runat="server" class="txtToAccountTitle form-control form-control-sm" autocomplete="off" />
                </div>
            </div>
            <hr />
            <h6>Sort By</h6>
            <hr />
            <div class="row">
                <div class="col">
                    <asp:RadioButton ID="rbTransDate" Text="Trans. Date" runat="server" GroupName="Sort" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
                <div class="col">
                    <asp:RadioButton ID="rbTransNo" Text="Trans. Number" runat="server" GroupName="Sort" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <asp:RadioButton ID="rbAccountCode" Text="Account Code" runat="server" GroupName="Sort" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
                <div class="col">
                    <asp:RadioButton ID="rbAccountTitle" Text="Account Title" runat="server" GroupName="Sort" class="col-xs-3 col-form-label col-form-label-sm" />
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <asp:TextBox ID="hfFromAccountCode" class="hfFromAccountCode" runat="server" />
                </div>
                <div class="col">
                    <asp:TextBox ID="hfToAccountCode" class="hfToAccountCode" runat="server" />
                </div>
            </div>
        </div>

        <div class="col-sm-9">
            <div class="row">
                <div class="col-4">
                    <asp:Button Text="Generate" ID="btnGenerate" class="btn btn-secondary btn-sm" runat="server" />
                    <asp:Button Text="Export to excel" ID="btnExport" class="btn btn-secondary btn-sm" runat="server" OnClick="btnExport_Click" />
                </div>
            </div>
            <div class="row">
                <div class="col"><br />
                    <div style="width: 100%; height:760px; overflow-y: scroll; overflow-x: scroll; overflow:scroll">
                        <asp:GridView ID="gvListSummary" runat="server" Width="100%" AutoGenerateColumns="false" ShowFooter="True" >
                            <AlternatingRowStyle BackColor="White" />
                            <Columns>
                                <asp:BoundField DataField="AccountCode" HeaderText="Account Code"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="110px" />
                                <asp:BoundField DataField="AccountTitle" HeaderText="Account Title"   ItemStyle-Width="200px" />
                                <asp:BoundField DataField="Debit" HeaderText="Debit" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px" />
                                <asp:BoundField DataField="Credit" HeaderText="Credit" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px"  />
                                <asp:BoundField DataField="AccountType" HeaderText="Account Type"  ItemStyle-Width="200px"/>
                            </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="white" />
                                <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                        <asp:GridView ID="gvListDetailed"  runat="server" AutoGenerateColumns="false"  ShowFooter="True"  Width="1600px"  OnSelectedIndexChanged = "OnSelectedIndexChanged"     OnRowDataBound="GridView_RowDataBound" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"  >
                            <Columns>
                                <asp:BoundField DataField="AppDate" HeaderText="Date" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="100px"/>
                                <asp:BoundField DataField="AccountCode" HeaderText="Account Code"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="120px"/>
                                <asp:BoundField DataField="AccountTitle" HeaderText="Account Title"  ItemStyle-Width="200px" />
                                <asp:BoundField DataField="Debit" HeaderText="Debit" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="150px" />
                                <asp:BoundField DataField="Credit" HeaderText="Credit" ItemStyle-HorizontalAlign="Right"   ItemStyle-Width="150px" />
                                <asp:BoundField DataField="RefType" HeaderText="RefType" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="70px" />
                                <asp:BoundField DataField="TransNo" HeaderText="Trans No" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="70px" />
                                <asp:BoundField DataField="VCECode" HeaderText="Code"  ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px"   />
                                <asp:BoundField DataField="VCEName" HeaderText="Name"  ItemStyle-Width="250px"/>
                                <asp:BoundField DataField="Particulars" HeaderText="Particulars"  ItemStyle-Width="200px"/>
                                <asp:BoundField DataField="Book" HeaderText="Book" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="RefTransID" HeaderText="Trans ID" ItemStyle-HorizontalAlign="Center"  ItemStyle-Width="80px"/>
                            </Columns>
                             <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <RowStyle BackColor="white" />
                                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
