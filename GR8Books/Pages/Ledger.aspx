﻿<%@ Page Title="Subsidiary Ledger" EnableEventValidation = "false"  Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Ledger.aspx.vb" Inherits="GR8Books.Ledger" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

      <asp:Panel runat="server">   
          <div class="row">
                <div class="col">
                    <div class="row">                      
                         <div class="col-sm-8"> 
                             <div class="input-group mb-3">
                                <asp:TextBox ID="txtFilter" runat="server" class="txtFilter form-control" autocomplete="off" placeholder="VCECode/VCEName"/>
                                    <div class="input-group-append">
                                    <asp:Button Text="Search" ID="btnSearch"  class="btnSearch btn btn-secondary" runat="server"  />
                                </div>
                            </div>
                        </div>
                       
                    </div>
                </div>
          </div>

          <asp:GridView ID="dgvList" runat="server" AutoGenerateColumns="false"  CellPadding="4" ForeColor="#333333" Width="100%" GridLines="none"  OnSelectedIndexChanged = "OnSelectedIndexChanged"     OnRowDataBound="GridView_RowDataBound"  ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">                       
          <Columns>
                <asp:BoundField DataField="VCECode" HeaderText="VCECode"/> 
                <asp:BoundField DataField="VCEName" HeaderText="VCEName" />
                <asp:BoundField DataField="AccntCode" HeaderText="Account Code" />
                <asp:BoundField DataField="AccntTitle" HeaderText="Account Title" /> 
                <asp:BoundField DataField="Debit" HeaderText="Debitt" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField DataField="Credit" HeaderText="Credit" ItemStyle-HorizontalAlign="Right"/>
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
