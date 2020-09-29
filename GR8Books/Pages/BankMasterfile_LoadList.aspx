<%@ Page Title="Bank Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="BankMasterfile_LoadList.aspx.vb" Inherits="GR8Books.BankMasterfile_LoadList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var ID = $(this).attr("title");
                var Actions = "View"
                window.open("BankMasterfile_Management.aspx?id=" + ID + '&Actions=' + Actions, "_self");
                return false;
            });

            $(".btnEdit").click(function () {
                var ID = $(this).attr("title");
                var Actions = "Edit"
                window.open("BankMasterfile_Management.aspx?id=" + ID + '&Actions=' + Actions, "_self");
                return false;
            });

            $(".btnAdd").click(function () {
                var ID = $(this).attr("title");
                var Actions = "Add"
                window.open("BankMasterfile_Management.aspx?id=" + ID + '&Actions=' + Actions, "_self");
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
    <asp:Panel runat="server" ID="panelBank">
        <asp:GridView ID="dgvBankList" runat="server" AutoGenerateColumns="False" AllowPaging="True" CellPadding="4" ForeColor="#333333" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="ID No." />
                <asp:BoundField DataField="Type" HeaderText="Type" />
                <asp:BoundField DataField="Bank" HeaderText="Bank" />
                <asp:BoundField DataField="Branch" HeaderText="Branch" />
                <asp:BoundField DataField="AccountNumber" HeaderText="AccountNumber" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnView" class="btnView" Text="View" runat="server" title='<%# Eval("ID") %>' />
                        <asp:Button ID="btnEdit" class="btnEdit" Text="Edit" runat="server" title='<%# Eval("ID") %>' />
                        <asp:Button ID="btnInactive" class="btnInactive" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("ID") %>' />
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
        <div class="row justify-content-end mt-4">
            <div class="col-sm-2">
                <asp:Button ID="btnAdd" Text="Add" runat="server" class="btnAdd btn btn-primary btn-block" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
