<%@ Page Title="Collector Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Collector_Loadlist.aspx.vb" Inherits="GR8Books.Collector_Loadlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var ID = $(this).attr("title");
                var Actions = "View"
                window.open("Collector_Management.aspx?id=" + ID + '&Actions=' + Actions, "View", "width=550,height=550");
                return false;
            });

            $(".btnEdit").click(function () {
                var ID = $(this).attr("title");
                var Actions = "Edit"
                window.open("Collector_Management.aspx?id=" + ID + '&Actions=' + Actions, "Edit", "width=550,height=550");
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "Collector_Management.aspx";
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
        <div class="row">
            <div class="col">
                <asp:GridView ID="gvCollector" runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                        <asp:BoundField DataField="Collector_Name" HeaderText="Collector Name" />
                        <asp:BoundField DataField="Area" HeaderText="Area" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
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
            </div>
        </div>
        <div class="row mt-4 justify-content-end">
            <div class="col-3 ">
                <asp:Button ID="btnAdd" class="btnAdd btn btn-primary btn-block" Text="Add" runat="server" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
