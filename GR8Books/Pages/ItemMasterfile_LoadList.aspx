<%@ Page Title="Item Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="ItemMasterfile_LoadList.aspx.vb" Inherits="GR8Books.ItemMasterfile_LoadList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var ID = $(this).attr("title");
                var Actions = "View"
                window.open("ItemMasterfile_Maintenance.aspx?id=" + ID + '&Actions=' + Actions, "View", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "ItemMasterfile_Maintenance.aspx";
                return false;
            });

            $(".btnEdit").click(function () {
                var ID = $(this).attr("title");
                var Actions = "Edit"
                window.open("ItemMasterfile_Maintenance.aspx?id=" + ID + '&Actions=' + Actions, "Edit", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "ItemMasterfile_Maintenance.aspx";
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
        <asp:GridView ID="dgvItemList" runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" OnRowDataBound="OnRowDataBound" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"> 
             <AlternatingRowStyle BackColor="White" />
            <Columns>
                 <asp:TemplateField HeaderText="Photo">
                    <ItemTemplate>
                        <asp:Image ID="ItemPhoto" Height="100px" width="100px" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ItemCode" HeaderText="Item Code" />
                <asp:BoundField DataField="ItemName" HeaderText="Item Name" />
                <asp:BoundField DataField="BarCode" HeaderText="BarCode" />
                <asp:BoundField DataField="ItemPrice" HeaderText="Item Price" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnView" class="btnView" Text="View" runat="server" title='<%# Eval("ItemCode")%>' />
                        <asp:Button ID="btnEdit" class="btnEdit" Text="Edit" runat="server" title='<%# Eval("ItemCode")%>' />
                        <asp:Button ID="btnInactive" class="btnInactive" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("ItemCode")%>' />
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
                <asp:Button ID="btnAdd" class="btnAdd btn btn-primary btn-block" Text="Add" runat="server" />
            </div>
        </div>
    </asp:Panel>
</asp:Content>
