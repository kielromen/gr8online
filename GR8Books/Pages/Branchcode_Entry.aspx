<%@ Page Title="Branch Code" Language="vb" AutoEventWireup="false" CodeBehind="Branchcode_Entry.aspx.vb" Inherits="GR8Books.Branchcode_Entry" MasterPageFile="~/Master/Dashboard.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var ID = $(this).attr("title");
                var Actions = "View"
                window.open("Branchcode_Maintenance.aspx?Branchcode=" + ID + '&Actions=' + Actions, "View", "width=700,height=550");
                return false;
            });

            $(".btnEdit").click(function () {
                var ID = $(this).attr("title");
                var Actions = "Edit"
                window.open("Branchcode_Maintenance.aspx?Branchcode=" + ID + '&Actions=' + Actions, "Edit", "width=700,height=550");
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "Branchcode_Maintenance.aspx";
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
    <asp:GridView ID="gvVendor" runat="server" AutoGenerateColumns="False" AllowPaging="True" CellPadding="4" GridLines="None" Width="100%" ForeColor="#333333">
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:BoundField DataField="Branchcode" HeaderText="Branch code" />
            <asp:BoundField DataField="Description" HeaderText="Description" />
            <asp:BoundField DataField="Status" HeaderText="Status" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:Button ID="btnView" class="btnView" runat="server" Text="View" title='<%# Eval("Branchcode")%>' />
                    <asp:Button ID="btnEdit" class="btnEdit" runat="server" Text="Edit" title='<%# Eval("Branchcode")%>' />
                    <asp:Button ID="btnInactive" class="btnInactive" runat="server" CommandArgument='<%# Bind("Branchcode")%>' CommandName="btnInactive" CssClass="btnInactive" Text="Inactive" />
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
    <div class="row mt-4">
        <div class="col text-right">
    <asp:Button Text="Add Branch" class="btnAdd btn btn-primary" ID="btnAdd" runat="server" />
        </div>
    </div>

</asp:Content>
