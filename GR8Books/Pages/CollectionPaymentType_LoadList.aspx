<%@ Page Title="Collection Payment Type" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="CollectionPaymentType_LoadList.aspx.vb" Inherits="GR8Books.CollectionPaymentType_LoadList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <script type='text/javascript'>
         $(document).ready(function () {
             $(".btnView").click(function () {
                 var ID = $(this).attr("title");
                 var Actions = "View"
                 window.open("CollectionPaymentType_Maintenance.aspx?id=" + ID + '&Actions=' + Actions, "View", "width=550,height=550");
                 return false;
             });

             $(".btnEdit").click(function () {
                 var ID = $(this).attr("title");
                 var Actions = "Edit"
                 window.open("CollectionPaymentType_Maintenance.aspx?id=" + ID + '&Actions=' + Actions, "Edit", "width=550,height=550");
                 return false;
             });

             $(".btnAdd").click(function () {
                 window.location = "CollectionPaymentType_Maintenance.aspx";
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
                      <asp:TextBox ID="txtFilter" runat="server" class="txtFilter form-control" autocomplete="off" placeholder="Payment Type"  />
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
                         <asp:Button ID="btnExport" Width="100px" class="btnExport btn btn-success" Text="Export" runat="server" />
                     </div>
                 </div>
            </div>
                <asp:GridView ID="gvPaymentType"   PageSize="13" runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                        <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" />
                        <asp:BoundField DataField="WithBank" HeaderText="WithBank" />
                        <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                        <asp:BoundField DataField="DateModified" HeaderText="DateModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                        <asp:BoundField DataField="WhoCreated" HeaderText="WhoCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                        <asp:BoundField DataField="WhoModified" HeaderText="WhoModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
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
    </asp:Panel>
</asp:Content>
