<%@ Page Title="Chart of Accounts"  MaintainScrollPositionOnPostback="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="ChartofAccount_Loadlist.aspx.vb" Inherits="GR8Books.ChartofAccount_Loadlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../Scripts/jquery-ui.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".gvCOA").sortable({
                items: 'tr:not(tr:first-child)',
                cursor: 'pointer',
                axis: 'y',
                dropOnEmpty: false,
                start: function (e, ui) {
                    ui.item.addClass("selected");
                },
                stop: function (e, ui) {
                    ui.item.removeClass("selected");
                    $('#<%=btnSort.ClientID%>').click();
                },
            });
        });
    </script>
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var ID = $(this).attr("title");
                var Actions = "View"
                window.open("ChartofAccounts.aspx?id=" + ID + '&Actions=' + Actions, "_self");
                return false;
            });

            $(".btnEdit").click(function () {
                var ID = $(this).attr("title");
                var Actions = "Edit"
                window.open("ChartofAccounts.aspx?id=" + ID + '&Actions=' + Actions, "_self");
                return false;
            });

            $(".btnAdd").click(function () {
                //window.location = "ChartofAccounts.aspx";
                //return false;
                var Actions = "Add"
                window.open("ChartofAccounts.aspx?Actions=" + Actions, "_self");
                return false;
            });


            $(".btnInactive").click(function () {
                if (confirm("Are you sure you want to remove this?")) {
                }
                else {
                    return false;
                }
            });
        });
    </script>

    <asp:Panel runat="server">
        <div class="row mb-3 justify-content-start">
            <div class="col-3">
                <asp:DropDownList ID="ddlAccountType" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Loadlist"></asp:DropDownList>
            </div>
            <div class="col-2">
                <asp:DropDownList ID="ddlFilter" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Loadlist"></asp:DropDownList>
            </div>
            <div class="col-2">
                <asp:Button ID="btnSort" class="btnSort btn btn-primary btn-block" Text="Update Order" runat="server" OnClick="UpdateSort" />
            </div>
        </div>

            <div class="row">
                <div class="col">
                    <asp:GridView ID="gvCOA" CssClass="gvCOA" runat="server" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Order" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                    <input type="hidden" name="Code" value='<%# Eval("AccountCode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="AccountCode" HeaderText="Code" ItemStyle-Width="160" />
                            <asp:BoundField DataField="AccountTitle" HeaderText="Description" ItemStyle-Width="300" />
                            <asp:BoundField DataField="AccountGroup" HeaderText="Account Group" ItemStyle-Width="200" />
                            <asp:BoundField DataField="AccountNature" HeaderText="Account Nature" ItemStyle-Width="160" />
                            <asp:BoundField DataField="withSubsidiary" HeaderText="With Subsidiary" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Class" HeaderText="Class" ItemStyle-Width="100" />
                            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="350">
                                <ItemTemplate>
                                    <asp:Button ID="btnView" Width="70px" class="btnView btn btn-secondary btn-sm" Text="View" runat="server" title='<%# Eval("AccountCode") %>' />
                                    <asp:Button ID="btnEdit" Width="70px" class="btnEdit btn btn-secondary btn-sm" Text="Edit" runat="server" title='<%# Eval("AccountCode") %>' />
                                    <asp:Button ID="btnInactive" Width="70px" class="btnInactive btn btn-secondary btn-sm" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("AccountCode") %>' />
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
            </div>
        
            <div class="row">
                <div class="col">
                    <asp:GridView ID="GridView1" CssClass="gvCOA" runat="server" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="Order" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                    <input type="hidden" name="Code" value='<%# Eval("AccountCode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="AccountCode" HeaderText="Code" ItemStyle-Width="160" />
                            <asp:BoundField DataField="AccountTitle" HeaderText="Description" ItemStyle-Width="300" />
                            <asp:BoundField DataField="AccountGroup" HeaderText="Account Group" ItemStyle-Width="200" />
                            <asp:BoundField DataField="AccountNature" HeaderText="Account Nature" ItemStyle-Width="160" />
                            <asp:BoundField DataField="withSubsidiary" HeaderText="With Subsidiary" ItemStyle-Width="150" />
                            <asp:BoundField DataField="Class" HeaderText="Class" ItemStyle-Width="100" />
                            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="350">
                                <ItemTemplate>
                                    <asp:Button ID="btnView" Width="70px" class="btnView btn btn-secondary btn-sm" Text="View" runat="server" title='<%# Eval("AccountCode") %>' />
                                    <asp:Button ID="btnEdit" Width="70px" class="btnEdit btn btn-secondary btn-sm" Text="Edit" runat="server" title='<%# Eval("AccountCode") %>' />
                                    <asp:Button ID="btnInactive" Width="70px" class="btnInactive btn btn-secondary btn-sm" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("AccountCode") %>' />
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
            </div>
        
        <div class="row mt-4 justify-content-end">
            <div class="col-2 ">
                <asp:Button ID="btnAdd" Text="Add" class="btnAdd btn btn-primary btn-block" runat="server" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
