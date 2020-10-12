<%@ Page Title="Transaction Reports" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Reports_TransactionReports.aspx.vb" Inherits="GR8Books.Reports_TransactionReports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
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
    <div class="row row-cols-2">
        <div class="col-6">
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Report:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlReports" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="SelectReport"></asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator3" runat="Server" ControlToValidate="ddlReports" InitialValue="--Select Report--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator2" runat="Server" ControlToValidate="ddlReports" InitialValue="--Looseleaf--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator4" runat="Server" ControlToValidate="ddlReports" InitialValue="--Journals--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Report Type:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlReportType" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control"></asp:DropDownList>
                        <asp:RequiredFieldValidator ForeColor="Red" Font-Size="Small" Display="Dynamic" ID="RequiredFieldValidator1" runat="Server" ControlToValidate="ddlReportType" InitialValue="--Select Report Type--" ErrorMessage="Field is required." ValidationGroup="g"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Period Type:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlPeriodType" runat="server" class="form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="LoadPeriod" ></asp:DropDownList>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Year:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="txtYear" runat="server" class="form-control" Autopostback="true" autocomplete="off" type="number" OnTextChanged="LoadPeriod" />
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Month:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:DropDownList ID="ddlMonth" runat="server" class="form-control" AppendDataBoundItems="true" AutoPostBack="true" EnableViewState="true" OnSelectedIndexChanged="LoadPeriod"></asp:DropDownList>
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Date From:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="dtpFromDate" TextMode="Date" Autopostback="true" class="dtpFromDate form-control" runat="server" OnTextChanged="LoadPeriod"  />
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-3 my-auto">
                        <asp:Label Text="Date To:" runat="server" />
                    </div>
                    <div class="col">
                        <asp:TextBox ID="dtpToDate" TextMode="Date"  Autopostback="true" class="dtpFromDate form-control" runat="server" OnTextChanged="LoadPeriod" />
                    </div>
                </div>
                <div class="row mb-2">
                    <div class="col-6">
                         <asp:Button ID="btnPreview" Text="Preview" class="btnPreview btn btn-primary btnAdd btn-block" runat="server"  ValidationGroup="g"/>
                    </div>
                    <div class="col-6">
                         <asp:Button ID="btnExport" Text="Export to Excel" class="btnExport btn btn-success btnAdd btn-block" runat="server"  ValidationGroup="g"/>
                    </div>
                </div>
            </div>  

         <div class="col-5">
                <asp:Panel ID="panelFilter" runat="server">
                <div class="row mb-2">
                    <div style="width: 100%; height:320px; overflow-y: scroll;">
                        <asp:GridView ID="gvFilter" runat="server" Width="100%" CellPadding="4" ForeColor="#333333" AutoGenerateColumns="false"  GridLines="none"   >
                            <Columns>

                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chkAll" Checked="true" runat="server" onclick = "checkAll(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkBox" Checked="true" runat="server" onclick = "Check_Click(this)" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ID" HeaderText="ID"  HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                                <asp:BoundField DataField="Filter" HeaderText="Filter"/>
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

</asp:Content>
