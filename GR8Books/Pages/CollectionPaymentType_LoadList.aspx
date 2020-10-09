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
                 if (confirm("Are you sure you want to remove this?")) {
                 }
                 else {
                     return false;
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
        <div class="row">
            <div class="col">
                 <div class="row mt-4 justify-content-end">
                    <div class="col-2 ">
                     <asp:Button ID="btnExport" class="btnExport btn btn-success btn-block" Text="Export" runat="server" />
                     </div>
                 </div>
                 <br />
                <asp:GridView ID="gvPaymentType" runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/>
                        <asp:BoundField DataField="PaymentType" HeaderText="Payment Type" />
                        <asp:BoundField DataField="WithBank" HeaderText="WithBank" />
                        <asp:BoundField DataField="AccountCode" HeaderText="Account Code" />
                        <asp:BoundField DataField="AccountTitle" HeaderText="Account Title" />
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
            </div>
        </div>
         <div class="row mt-4 justify-content-end">
           <div class="col-2 ">
              <asp:Button ID="btnAdd" class="btnAdd btn btn-primary btn-block" Text="Add" runat="server" />
           </div>
       </div>
    </asp:Panel>
</asp:Content>
