<%@ Page Title="Transacton Uploader" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Transaction_Uploader.aspx.vb" Inherits="GR8Books.Transaction_Uploader" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
       <script type="text/javascript">  
        $(document).ready(function () {
            $('.txtAmount').focusout(function () {
                $('.txtAmount').formatCurrency();
                $('.txtAmount').toNumber().formatCurrency('.txtAmount');
            });
        });
    </script>

    <script type='text/javascript'>
        $(document).ready(function () {
            var id = "test";
            $("#<%=btnSearch.ClientID%>").click(function (e) {
                e.preventDefault();
                var Type = "TU";
                var Url = "Transaction_Uploader.aspx";
                var myWidth = "900";
                var myHeight = "550";
                var left = (screen.width - myWidth) / 2;
                var top = (screen.height - myHeight) / 4;
                var win = window.open("LoadTransaction.aspx?id=" + Type + '&Url=' + Url, "Load Transaction", 'dialog=yes,resizable=no, width=' + myWidth + ', height=' + myHeight + ', top=' + top + ', left=' + left);
                var timer = setInterval(function () {
                    if (win.closed) {
                        clearInterval(timer);
                    }
                });
            });


        });
    </script>
    
    <asp:Panel runat="server">
        <div class="row">
            <div class="col">
                 <div class="row mb-2">
                    <div class="col-12">
                        <asp:Button Text="Search" ID="btnSearch" runat="server" class="btn btn-primary" />
                        <asp:Button Text="Upload" ID="btnUpload" AutoPostBack="False" runat="server" class="btn btn-primary" />
                        <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary" runat="server" />
                        <asp:Button ID="btnExport" class="btnExport btn btn-success" Text="Export" runat="server" />
                    </div>
                </div>
                 <br />
                 <div style="width: 100%; height:760px; overflow-y: scroll;overflow:scroll">
                <asp:GridView ID="gvUploader" runat="server" AutoGenerateColumns="false"  Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
                    <AlternatingRowStyle BackColor="White" />
                    <Columns>
                        <asp:BoundField DataField="AppDate" HeaderText="Date"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                        <asp:BoundField DataField="RefType" HeaderText="Doc Type"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="TransNo" HeaderText="Doc No"   HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>
                        <asp:BoundField DataField="AccntCode" HeaderText="Account Code"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>
                        <asp:BoundField DataField="AccntTitle" HeaderText="Account Title"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>
                        <asp:BoundField DataField="VCECode" HeaderText="VCECode"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>
                        <asp:BoundField DataField="VCEName" HeaderText="VCEName"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                        <asp:BoundField DataField="Debit" HeaderText="Debit"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"  ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Credit" HeaderText="Credit"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"   ItemStyle-HorizontalAlign="Right" />
                        <asp:BoundField DataField="Particulars" HeaderText="Particulars"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>
                        <asp:BoundField DataField="RefNo" HeaderText="RefNo"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>
                        <asp:BoundField DataField="CostCenter" HeaderText="Cost Center"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>
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
        </div>
    </asp:Panel>
</asp:Content>
