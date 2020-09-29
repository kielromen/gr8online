<%@ Page Title="Employee Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Employee_Loadlist.aspx.vb" Inherits="GR8Books.Employee_Loadlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var ID = $(this).attr("title");
                var Actions = "View"
                window.open("Employee_Management.aspx?id=" + ID + '&Actions=' + Actions, "View", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnEdit").click(function () {
                var ID = $(this).attr("title");
                var Actions = "Edit"
                window.open("Employee_Management.aspx?id=" + ID + '&Actions=' + Actions, "Edit", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "Employee_Management.aspx";
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
    <div>
        <asp:Panel runat="server">
            <asp:GridView ID="gvEmployee" runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Employee_Code" HeaderText="Employee Code" />
                    <asp:BoundField DataField="Employee_Name" HeaderText="Employee_Name" />
                    <asp:BoundField DataField="CellphoneNo" HeaderText="CellphoneNo" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnView" class="btnView" Text="View" runat="server" title='<%# Eval("Employee_Code") %>' />
                            <asp:Button ID="btnEdit" class="btnEdit" Text="Edit" runat="server" title='<%# Eval("Employee_Code") %>' />
                            <asp:Button ID="btnInactive" class="btnInactive" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("Employee_Code") %>' />
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
                    <asp:Button ID="btnAdd" class="btnAdd btn btn-primary btn-block" Text="Add" runat="server" />
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
