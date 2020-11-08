<%@ Page Title="Bank Reconciliation"  MaintainScrollPositionOnPostback="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="BankRecon.aspx.vb" Inherits="GR8Books.BankRecon" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <script src="../Scripts/jquery.mask.js"></script> 
        <script src="../Scripts/jquery.formatCurrency-1.4.0.js"></script>

    <style type="text/css">
     .hidden
     {
         display:none;
     }
    </style>
    <script type = "text/javascript">
        function Check_Click(objRef)
        {
            //Get the Row based on checkbox
            var row = objRef.parentNode.parentNode;
            if(objRef.checked)
            {
                //If checked change color to Aqua
                row.style.backgroundColor = "gray";
            }
            else
            {   
                   row.style.backgroundColor = "white";
            }
            //Get the reference of GridView
            var GridView = row.parentNode;
            //Get all input elements in Gridview
            var inputList = GridView.getElementsByTagName("input");
            for (var i=0;i<inputList.length;i++)
            {
                //The First element is the Header Checkbox
                var headerCheckBox = inputList[0];
                //Based on all or none checkboxes
                //are checked check/uncheck Header Checkbox
                var checked = true;
                if(inputList[i].type == "checkbox" && inputList[i] != headerCheckBox)
                {
                    if(!inputList[i].checked)
                    {
                        checked = false;
                        break;
                    }
                }
            }
            headerCheckBox.checked = checked;
            }
    </script>

    <script type = "text/javascript">
        function checkAll(objRef) {
            var GridView = objRef.parentNode.parentNode.parentNode;
            var inputList = GridView.getElementsByTagName("input");
            for (var i = 0; i < inputList.length; i++) {
                //Get the Cell To find out ColumnIndex
                var row = inputList[i].parentNode.parentNode;
                if (inputList[i].type == "checkbox" && objRef != inputList[i]) {
                    if (objRef.checked) {
                        //If the header checkbox is checked
                        //check all checkboxes
                        //and highlight all rows
                        row.style.backgroundColor = "gray";
                        inputList[i].checked = true;
                    }
                    else {
                        row.style.backgroundColor = "white";
                        inputList[i].checked = false;
                    }
                }
            }
        }
    </script> 


    <script type = "text/javascript">
        function MouseEvents(objRef, evt) {
            var checkbox = objRef.getElementsByTagName("input")[0];
            if (evt.type == "mouseover") {
                //objRef.style.backgroundColor = "gray";
            }
            else {
                if (checkbox.checked) {
                    //objRef.style.backgroundColor = "gray";
                }
                else if (evt.type == "mouseout") {
                      objRef.style.backgroundColor = "white";
               }
            }
        }
    </script>

    <script type = "text/javascript">
        $(document).ready(function () {
            function LoadAdjustedBankBalance () {
                var DIT = 0;
                var CIB = 0;
                var OC = 0;
                var ADJ = 0;
                CIB = parseFloat($("#<%=txtBankBalance.ClientID%>").val().replace(/,/g, ""));
                DIT = parseFloat($("#<%=txtDIT.ClientID%>").val().replace(/,/g, ""));
                OC = parseFloat($("#<%=txtOC.ClientID%>").val().replace(/,/g, ""));
                if (isNaN(CIB)) {
                    CIB = 0;
                }
                if (isNaN(DIT)) {
                    DIT = 0;
                }
                if (isNaN(OC)) {
                    OC = 0;
                }
                if (isNaN(ADJ)) {
                    ADJ = 0;
                }
                ADJ = CIB + DIT - OC;
                $("#<%=txtAdjustedBankBalance.ClientID%>").val(parseFloat(ADJ).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
            }
            function ComputeVariance() {
                var BankBal = 0;
                var BookBal = 0;
                var Variance = 0;
                BankBal = parseFloat($("#<%=txtAdjustedBookBalance.ClientID%>").val().replace(/,/g, ""));;
                BookBal = parseFloat($("#<%=txtAdjustedBankBalance.ClientID%>").val().replace(/,/g, ""));;

                if (isNaN(BankBal)) {
                    ADBankBalJ = 0;
                }
                if (isNaN(BookBal)) {
                    BookBal = 0;
                }
                if (isNaN(Variance)) {
                    Variance = 0;
                }
                Variance = (BookBal - BankBal);
                $("#<%=txtVariance.ClientID%>").val(parseFloat(Variance).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString());
              }
            $(".txtBankBalance").keyup(function () {
                LoadAdjustedBankBalance();
                ComputeVariance();
            });

            var id = "test";
            $("#<%=btnSearch.ClientID%>").click(function (e) {
                e.preventDefault();
                var Type = "BR";
                var Url = "BankRecon.aspx";
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

       <script type="text/javascript">  
           $(document).ready(function () {
               $('.txtBankBalance').focusout(function () {
                   $('.txtBankBalance').formatCurrency();
                   $('.txtBankBalance').toNumber().formatCurrency('.txtBankBalance');
               });
           });
       </script>
        <div class="row mb-2">
            <div class="col-12">
                <asp:Button Text="Search" ID="btnSearch" runat="server" class="btn btn-primary" />
                <asp:Button Text="New" ID="btnNew" AutoPostBack="False" runat="server" class="btn btn-primary" />
                <asp:Button Text="Edit" ID="btnEdit" runat="server" class="btn btn-primary" />
                <asp:Button Text="Save" ID="btnSave" class="btnSave btn btn-primary" runat="server" ValidationGroup="g" />
                <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary" runat="server" />
                <asp:Button Text="Close" ID="btnClose" runat="server" class="btn btn-primary" />
                <asp:Button Text="Prev" ID="btnPrev" runat="server" class="btn btn-primary" />
                <asp:Button Text="Next" ID="btnNext" runat="server" class="btn btn-primary" />
                <asp:Button Text="Preview" ID="btnPreview" runat="server" class="btn btn-primary" />
            </div>
        </div>

        <hr />  
        <asp:Panel id="panelBR" runat="server">
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Bank Reconciliation Details" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
         </div>
            <div class="row mb-2">
             <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="BR No:" runat="server"  />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Status:" runat="server" />
                    </div>
                </div>
            </div>
        </div>
        <div class="row mb-2">
             <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox ID="txtTrans_Num" runat="server" class="form-control" autocomplete="off"/>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox ID="txtStatus" runat="server" class="form-control" autocomplete="off"/>
                    </div>
                </div>
            </div>
          </div>
          <%-- Select Bank  --%>
          <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Select Bank" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
         </div>

          <div class="row mb-2">
             <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Bank:" runat="server"  />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Account Code:" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Account Title:" runat="server" />
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-2">
             <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:DropDownList ID="ddlBank" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="RefreshData"></asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="ddlBank" InitialValue="--Select Bank--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>

                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox ID="txtAccountCode" runat="server" class="form-control" autocomplete="off"/>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox ID="txtAccountTitle" runat="server" class="form-control" autocomplete="off"/>
                    </div>
                </div>
            </div>
        </div>
        <%-- Enter the following from your statement  --%>
       <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Enter the following from your statement" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>

        <div class="row mb-2">
             <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Book Balance:" runat="server"  />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Bank Balance:" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Ending Date:" runat="server" />
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-2">
             <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox ID="txtBookBalance" runat="server" class="form-control text-right" autocomplete="off"/>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox ID="txtBankBalance" runat="server" class="txtBankBalance form-control text-right" autocomplete="off" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox ID="dtpDoc_Date" runat="server" onkeypress="return false;" onpaste="return false" TextMode="Date" AutoPostBack="True"  class="form-control dtpDoc_Date" ></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="row mb-2">
             <div class="col-sm-4">
               
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Outstanding Check:" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
            
            </div>
        </div>

        <div class="row mb-2">
             <div class="col-sm-4">
               
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox ID="txtOC" runat="server" class="form-control text-right" autocomplete="off"/>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
            
            </div>
        </div>

       <div class="row mb-2">
             <div class="col-sm-4">
               
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Deposit in Transit:" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
            
            </div>
        </div>

        <div class="row mb-2">
             <div class="col-sm-4">
               
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox id ="txtDIT" runat="server" class="form-control text-right" autocomplete="off"/>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
            
            </div>
        </div>

        <div class="row mb-2">
             <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Adjusted Book Balance:" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Adjusted Bank Balance:" runat="server" />
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col-7 my-auto">
                        <asp:Label Text="Variance:" runat="server" />
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-2">
             <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox ID="txtAdjustedBookBalance" runat="server" class="form-control text-right" autocomplete="off"/>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox ID="txtAdjustedBankBalance"  runat="server" class="form-control text-right" autocomplete="off"/>
                    </div>
                </div>
            </div>
            <div class="col-sm-4">
                <div class="row">
                    <div class="col">
                        <asp:TextBox  ID="txtVariance"  runat="server" class="form-control text-right" autocomplete="off"/>
                    </div>
                </div>
            </div>
        </div>
        <%--Reconcile Checking  --%>
        <div class="row mt-4">
            <div class="col">
                <h6>
                    <asp:Label Text="Reconcile Checking" runat="server" Style="color: #808080" /></h6>
                <hr />
            </div>
        </div>
        <nav>
          <div class="nav nav-tabs" id="nav-tab" role="tablist">
            <a runat="server" class="nav-item nav-link active" id="navDITtab" data-toggle="tab" href="#MainContent_navDIT" role="tab" aria-controls="navDIT" aria-selected="true">Deposit in Transit</a>
            <a runat="server" class="nav-item nav-link" id="navOCtab" data-toggle="tab" href="#MainContent_navOC" role="tab" aria-controls="navOC" aria-selected="false">Oustandng Checks</a>
            <a runat="server" class="nav-item nav-link" id="navCLEAREDtab" data-toggle="tab" href="#MainContent_navCLEARED" role="tab" aria-controls="navCLEARED" aria-selected="false">Cleared Deposits & Outsanding Checks</a>
          </div>
        </nav>
        <div class="tab-content ml-3 mr-3" id="nav-tabContent">
          <div runat="server" class="tab-pane fade show active" id="navDIT" role="tabpanel" aria-labelledby="DIT-home-tab">
              <asp:Panel ID="panelDIT" runat="server">
                 <div class="row mb-2 mt-2">
                    <div class="col-1">
                        <asp:Button Text="Clear" ID="btnClearDIT" runat="server" class="btn btn-primary" />
                    </div>
                 </div>
                <div class="row mb-2">
                    <div style="width: 100%; overflow-y: scroll;">
                        <asp:GridView ID="gvDIT" runat="server" Width="100%" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"  GridLines="none"   >
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" Checked="false" runat="server" onclick = "checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBox" Checked="false" runat="server" onclick = "Check_Click(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ID" HeaderText="ID"  HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                                <asp:BoundField DataField="AppDate" HeaderText="AppDate"/>
                                <asp:BoundField DataField="VCEName" HeaderText="VCEName"/>
                                <asp:BoundField DataField="RefTransID" HeaderText="RefTransID"  HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                                <asp:BoundField DataField="RefType" HeaderText="RefType"/>
                                <asp:BoundField DataField="TransNo" HeaderText="TransNo"/>
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks"/>
                                <asp:BoundField DataField="Amount" HeaderText="Amount"  HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/>
                            </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                    </div>
                </div>
             </asp:Panel>
          </div>
          <div runat="server"  class="tab-pane fade" id="navOC" role="tabpanel" aria-labelledby="nav-OC-tab">
             <asp:Panel ID="panelOC" runat="server">
                 <div class="row mb-2 mt-2">
                    <div class="col-1">
                        <asp:Button Text="Clear" ID="btnClearOC" runat="server" class="btn btn-primary" />
                    </div>
                 </div>
                <div class="row mb-2">
                    <div style="width: 100%; overflow-y: scroll;">
                        <asp:GridView ID="gvOC" runat="server" Width="100%" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"  GridLines="none"   >
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" Checked="false" runat="server" onclick = "checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBox" Checked="false" runat="server" onclick = "Check_Click(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ID" HeaderText="ID"  HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                                <asp:BoundField DataField="AppDate" HeaderText="AppDate"/>
                                <asp:BoundField DataField="VCEName" HeaderText="VCEName"/>
                                <asp:BoundField DataField="RefTransID" HeaderText="RefTransID"  HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                                <asp:BoundField DataField="RefType" HeaderText="RefType"/>
                                <asp:BoundField DataField="TransNo" HeaderText="TransNo"/>
                                <asp:BoundField DataField="CheckNo" HeaderText="CheckNo"/>
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks"/>
                                <asp:BoundField DataField="Amount" HeaderText="Amount"  HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/>
                            </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                    </div>
                </div>
             </asp:Panel>
          </div>
          <div runat="server" class="tab-pane fade" id="navCLEARED" role="tabpanel" aria-labelledby="nav-CLEARED-tab">
               <asp:Panel ID="panelCleared" runat="server">
                 <div class="row mb-2 mt-2">
                    <div class="col-1">
                        <asp:Button Text="Unclear" ID="btnUnclear" runat="server" class="btn btn-primary" />
                    </div>
                 </div>
                <div class="row mb-2">
                    <div style="width: 100%; overflow-y: scroll;">
                        <asp:GridView ID="gvCleared" runat="server" Width="100%" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"  GridLines="none"   >
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" Checked="false" runat="server" onclick = "checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBox" Checked="false" runat="server" onclick = "Check_Click(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ID" HeaderText="ID"  HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                                <asp:BoundField DataField="AppDate" HeaderText="AppDate"/>
                                <asp:BoundField DataField="VCEName" HeaderText="VCEName"/>
                                <asp:BoundField DataField="RefTransID" HeaderText="RefTransID"  HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                                <asp:BoundField DataField="RefType" HeaderText="RefType"/>
                                <asp:BoundField DataField="TransNo" HeaderText="TransNo"/>
                                <asp:BoundField DataField="CheckNo" HeaderText="CheckNo"/>
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks"/>
                                <asp:BoundField DataField="Amount" HeaderText="Amount"  HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"/>
                            </Columns>
                                <EditRowStyle BackColor="#2461BF" />
                                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#D1DDF1" ForeColor="#333333" />
                                <SortedAscendingCellStyle BackColor="#F5F7FB" />
                                <SortedAscendingHeaderStyle BackColor="#6D95E1" />
                                <SortedDescendingCellStyle BackColor="#E9EBEF" />
                                <SortedDescendingHeaderStyle BackColor="#4870BE" />
                        </asp:GridView>
                    </div>
                </div>
             </asp:Panel>
          </div>
        </div>
    </asp:Panel>
</asp:Content>
