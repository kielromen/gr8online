<%@ Page Title="Subsidiary Ledger" EnableEventValidation = "false"  Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="LedgerDetails.aspx.vb" Inherits="GR8Books.LedgerDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

     <style type="text/css">
     .hidden
     {
         display:none;
     }
    </style>

      <asp:Panel runat="server">   

          <div class="row mb-2">
                <div class="col-5">
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="VCECode:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCode" runat="server" class="form-control " autocomplete="off" />
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="VCEName:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtName" runat="server" class="form-control " autocomplete="off" />
                        </div>
                    </div>
                  
                </div>

                <div class="col-5">
                  <div class="row mb-2">
                        <div class="col-3 my-auto">
                            <asp:Label Text="Account Title:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtAccount" runat="server" class="form-control " autocomplete="off" />
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-3 my-auto text-nowrap">
                            <asp:Label Text="Balance:" runat="server" />
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtBalance" runat="server" class="form-control text-right" autocomplete="off" />
                        </div>
                    </div>
                </div>
            </div>

          <asp:GridView ID="dgvList" runat="server" AutoGenerateColumns="false"  CellPadding="4" ForeColor="#333333" Width="100%" GridLines="none"  OnSelectedIndexChanged = "OnSelectedIndexChanged"     OnRowDataBound="GridView_RowDataBound"  ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">                       
          <Columns>
                <asp:BoundField DataField="No" HeaderText="No."/> 
                <asp:BoundField DataField="RefTransID" HeaderText="Trans ID" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/> 
                <asp:BoundField DataField="RefType" HeaderText="RefType " />
                <asp:BoundField DataField="TransNo" HeaderText="RefID" />
                <asp:BoundField DataField="AppDate" HeaderText="Date"/> 
                <asp:BoundField DataField="VCECode" HeaderText="VCECode" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden"/> 
                <asp:BoundField DataField="VCEName" HeaderText="VCEName" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Debit" HeaderText="Debit"  ItemStyle-HorizontalAlign="Right"  />
                <asp:BoundField DataField="Credit" HeaderText="Credit" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Balance" HeaderText="Balance" ItemStyle-HorizontalAlign="Right"/>
                <asp:BoundField DataField="RefNo" HeaderText="RefNo" />
                <asp:BoundField DataField="LoginName" HeaderText="LoginName" />
           </Columns>
             <EditRowStyle BackColor="#2461BF" />
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EFF3FB" />
                        <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#F5F7FB" />
                        <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                        <SortedDescendingCellStyle BackColor="#E9EBEF" />
                        <SortedDescendingHeaderStyle BackColor="#4870BE" />
           </asp:GridView>
      </asp:Panel>
</asp:Content>
