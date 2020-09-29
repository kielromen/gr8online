<%@ Page Title="Debit Memo" Language="vb" AutoEventWireup="false" CodeBehind="DebitMemo_Entry.aspx.vb" Inherits="GR8Books.DebitMemo_Entry" MasterPageFile="~/Master/Dashboard.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var ID = $(this).attr("title");
                var Actions = "View"
                window.open("DebitMemo_Maintenance.aspx?TransID=" + ID + '&Actions=' + Actions, "View", "width=700,height=550");
                return false;
            });

            $(".btnEdit").click(function () {
                var ID = $(this).attr("title");
                var Actions = "Edit"
                window.open("DebitMemo_Maintenance.aspx?TransID=" + ID + '&Actions=' + Actions, "Edit", "width=700,height=550");
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "DebitMemo_Maintenance.aspx";
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
    <asp:GridView ID="gvVendor" runat="server" AutoGenerateColumns="False" AllowPaging="True" CellPadding="4" GridLines="None" Width="100%">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="TransID" HeaderText="ID" />
            <asp:BoundField DataField="DM_Type" HeaderText="DM Types" />
            <asp:BoundField DataField="DebitAccount" HeaderText="Account Code" />
            <asp:BoundField DataField="AccountTitle" HeaderText="Account Title" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:Button ID="btnView" class="btnView" runat="server" Text="View" title='<%# Eval("TransID")%>' />
                    <asp:Button ID="btnEdit" class="btnEdit" runat="server" Text="Edit" title='<%# Eval("TransID")%>' />
                    <asp:Button ID="btnInactive" class="btnInactive" runat="server" CommandArgument='<%# Bind("TransID")%>' CommandName="btnInactive" CssClass="btnInactive" Text="Inactive" />
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
        <div class="col-2">
            <asp:Button Text="Add New" class="btnAdd btn btn-primary btn-block" ID="btnAdd" runat="server" />
        </div>
    </div>
</asp:Content>
