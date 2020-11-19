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
                if ($(".btnInactive").val() == "Inactive") {
                    if (confirm("Are you sure you want to remove this?")) {
                    }
                    else {
                        return false;
                    }
                }
                else {
                    if (confirm("Are you sure you want to put back this?")) {
                    }
                    else {
                        return false;
                    }
                }
            });

        });
    </script>
        <style type="text/css">
     .hidden
     {
         display:none;
     }
    </style>

    <asp:Panel runat="server">
        <div class="row mb-3 mr-2 justify-content-end">
                <div class="col">
                 <div class="input-group">
                      <asp:TextBox ID="txtFilter" runat="server" class="txtFilter form-control" autocomplete="off" placeholder="Item Name"  />
                      <div class="input-group-append">
                          <asp:Button Text="Search" ID="btnSearch"  class="btnSearch btn btn-secondary" runat="server" OnClick="loadlist"  />
                      </div>
                 </div>
                 </div>
                 <div class="col">
                    <asp:DropDownList ID="ddlFilter" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Loadlist"></asp:DropDownList>
                 </div>
                 <div class="btn-toolbar" role="toolbar" aria-label="Toolbar with button groups">
                     <div class="btn-group" role="group" aria-label="First group">
                        <asp:Button ID="btnAdd" Width="100px" Text="Add" class="btnAdd btn btn-primary btn-block" runat="server" />
                        <asp:Button ID="btnUpload" Width="100px" class="btnUpload btn btn-primary" Text="Upload" runat="server" data-toggle="modal" data-target="#modalCOA" data-whatever="@mdo" />
                        <asp:Button ID="btnDownload" Width="100px" class="btnDownload btn btn-success" Text="Download" runat="server" />
                        <asp:Button ID="btnExport" Width="100px" class="btnExport btn btn-success" Text="Export" runat="server" />
                     </div>
                 </div>
            </div>
        <asp:GridView ID="dgvItemList" runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found"> 
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
                <asp:BoundField DataField="ItemType" HeaderText="ItemType" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="ItemCategory" HeaderText="ItemCategory" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="ItemUOM" HeaderText="ItemUOM" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="ItemUOM_QTY" HeaderText="ItemUOM_QTY" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="ItemPrice" HeaderText="Item Price" />
                <asp:BoundField DataField="ItemCost" HeaderText="ItemCost" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Warehouse" HeaderText="Warehouse" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Location" HeaderText="Location" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Bin" HeaderText="Bin" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="LotNo" HeaderText="LotNo" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Default_Supplier" HeaderText="Default_Supplier" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="WhoCreated" HeaderText="WhoCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="DateModified" HeaderText="DateModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="WhoModified" HeaderText="WhoModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
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
    </asp:Panel>
</asp:Content>
