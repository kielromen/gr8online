<%@ Page Title="Customer Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="CustomerManagement_View.aspx.vb" Inherits="GR8Books.CustomerManagement_View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <script src="../Scripts/xlsx.core.min.js"></script>
    <script src="../Scripts/xls.core.min.js"></script>
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var Code = $(this).attr("title");
                var Actions = "View"
                window.open("CustomerManagement.aspx?id=" + Code + '&Actions=' + Actions, "View", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnEdit").click(function () {
                var Code = $(this).attr("title");
                var Actions = "Edit"
                window.open("CustomerManagement.aspx?id=" + Code + '&Actions=' + Actions, "Edit", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "CustomerManagement.aspx";
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
            
            $("#<%=btnUpload.ClientID%>").click(function (e) {
                e.preventDefault();
            });

            $("#File_Upload").change(function (e) {
                var fileExtension = "xlsm"
                var arr = $(this).val().split(".");
                if (arr[$(this).val().split(".").length - 1] != fileExtension) {
                    $("#<%=alertUpdate.ClientID%>").show(0).delay(5000).hide(0);
                    $(this).val("");
                } else {
                    if (confirm("Are you sure you want to upload this file?")) {
                        var reader = new FileReader();  
                        reader.onload = function (e) {  
                            var data = e.target.result;  
                            /*Converts the excel data in to object*/ 
                            var workbook = XLSX.read(data, { type: 'binary' });  
                            /*Gets all the sheetnames of excel in to a variable*/  
                            var sheet_name_list = workbook.SheetNames;  
  
                            var cnt = 0; /*This is used for restricting the script to consider only first sheet of excel*/  
                            sheet_name_list.forEach(function (y) { /*Iterate through all sheets*/  
                                /*Convert the cell value to Json*/
                                var exceljson = XLSX.utils.sheet_to_json(workbook.Sheets[y]);  
                                if (exceljson.length > 0 && cnt == 0) {  
                                    $(".files").attr("title", "Uploading");
                                    $(".files").css({ "background-image": "url('../Images/waiting.png')" });
                                    BindTable(exceljson, '#File_Upload');  
                                    cnt++;  
                                }  
                            });
                        } 
                        reader.readAsArrayBuffer($("#File_Upload")[0].files[0]);  
                    }
                }
            });
            
            function BindTable(jsondata, tableid) {/*Function used to convert the JSON array to Html Table*/  
                var columns = BindTableHeader(jsondata, tableid); /*Gets all the column headings of Excel*/  
                var rowCount = 0;
                $.each(jsondata, function (i, row) {
                    $.ajax({
                        url: "<%= ResolveUrl("CustomerManagement_View.aspx/SaveVCE") %>",
                        data: "{'Customer_Code':'" + row.Customer_Code + "', 'Classification':'" + row.Classification + "', 'First_Name':'" + row.First_Name + "', 'Last_Name':'" + row.Last_Name + "', 'Middle_Name':'" + row.Middle_Name + "', 'Suffix_Name':'" + row.Suffix_Name + "', 'Customer_Name':'" + row.Customer_Name + "', 'TIN_No':'" + row.TIN_No + "', 'BranchCode':'" + row.BranchCode + "', 'Billing_Lot_Unit':'" + row.Billing_Lot_Unit + "', 'Billing_Blk_Bldg':'" + row.Billing_Blk_Bldg + "', 'Billing_Street':'" + row.Billing_Street + "', 'Billing_Subd':'" + row.Billing_Subd + "', 'Billing_Brgy':'" + row.Billing_Brgy + "', 'Billing_Town_City':'" + row.Billing_Town_City + "', 'Billing_Province':'" + row.Billing_Province + "', 'Billing_Region':'" + row.Billing_Region + "', 'Billing_ZipCode':'" + row.Billing_ZipCode + "', 'Delivery_Lot_Unit':'" + row.Delivery_Lot_Unit + "', 'Delivery_Blk_Bldg':'" + row.Delivery_Blk_Bldg + "', 'Delivery_Street':'" + row.Delivery_Street + "', 'Delivery_Subd':'" + row.Delivery_Subd + "', 'Delivery_Brgy':'" + row.Delivery_Brgy + "', 'Delivery_Town_City':'" + row.Delivery_Town_City + "', 'Delivery_Province':'" + row.Delivery_Province + "', 'Delivery_Region':'" + row.Delivery_Region + "', 'Delivery_ZipCode':'" + row.Delivery_ZipCode + "', 'SameAddress':'" + row.SameAddress + "', 'Contact_Person':'" + row.Contact_Person + "', 'Contact_Position':'" + row.Contact_Position + "', 'Contact_Telephone':'" + row.Contact_Telephone + "', 'Contact_Cellphone':'" + row.Contact_Cellphone + "', 'Contact_Fax':'" + row.Contact_Fax + "', 'Contact_Email':'" + row.Contact_Email + "', 'Contact_Website':'" + row.Contact_Website + "', 'Terms':'" + row.Terms + "', 'CutOff':'" + row.CutOff + "', 'VAT_Type':'" + row.VAT_Type + "', 'AccountNo':'" + row.AccountNo + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var bool = data.d;
                            var row$ = $('<tr style="white-space:nowrap;"/>');  
                            row$.append($('<td/>').html(row.Customer_Code));
                            row$.append($('<td/>').html(row.Classification));
                            row$.append($('<td/>').html(row.First_Name));
                            row$.append($('<td/>').html(row.Last_Name));
                            row$.append($('<td/>').html(row.Middle_Name));
                            row$.append($('<td/>').html(row.Suffix_Name));
                            row$.append($('<td/>').html(row.Customer_Name));
                            row$.append($('<td/>').html(row.TIN_No));
                            row$.append($('<td/>').html(row.BranchCode));
                            row$.append($('<td/>').html(row.Billing_Lot_Unit));
                            row$.append($('<td/>').html(row.Billing_Blk_Bldg));
                            row$.append($('<td/>').html(row.Billing_Street));
                            row$.append($('<td/>').html(row.Billing_Subd));
                            row$.append($('<td/>').html(row.Billing_Brgy));
                            row$.append($('<td/>').html(row.Billing_Town_City));
                            row$.append($('<td/>').html(row.Billing_Province));
                            row$.append($('<td/>').html(row.Billing_Region));
                            row$.append($('<td/>').html(row.Billing_ZipCode));
                            row$.append($('<td/>').html(row.Delivery_Lot_Unit));
                            row$.append($('<td/>').html(row.Delivery_Blk_Bldg));
                            row$.append($('<td/>').html(row.Delivery_Street));
                            row$.append($('<td/>').html(row.Delivery_Subd));
                            row$.append($('<td/>').html(row.Delivery_Brgy));
                            row$.append($('<td/>').html(row.Delivery_Town_City));
                            row$.append($('<td/>').html(row.Delivery_Province));
                            row$.append($('<td/>').html(row.Delivery_Region));
                            row$.append($('<td/>').html(row.Delivery_ZipCode));
                            row$.append($('<td/>').html(row.SameAddress));
                            row$.append($('<td/>').html(row.Contact_Person));
                            row$.append($('<td/>').html(row.Contact_Position));
                            row$.append($('<td/>').html(row.Contact_Telephone));
                            row$.append($('<td/>').html(row.Contact_Cellphone));
                            row$.append($('<td/>').html(row.Contact_Fax));
                            row$.append($('<td/>').html(row.Contact_Email));
                            row$.append($('<td/>').html(row.Contact_Website));
                            row$.append($('<td/>').html(row.Terms));
                            row$.append($('<td/>').html(row.CutOff));
                            row$.append($('<td/>').html(row.VAT_Type));
                            row$.append($('<td/>').html(row.AccountNo));
                            if (bool == "Exist") {
                                $("#<%=gvUpload.ClientID%>").append(row$.css({"background-color":"red","color":"white"}));  
                            } else {
                                $("#<%=gvUpload.ClientID%>").append(row$);  
                            }
                            rowCount++;
                            var percent =  parseFloat((rowCount/(jsondata.length)) * 100).toFixed(0);
                            $("#divUploadProgress").css({ "width": percent + "%" }).html(percent + "%");
                            if (rowCount == jsondata.length) {
                                $(".files").attr("title", "Upload Complete");
                            }
   
                        }

                    });
                    
                });
            }
            function BindTableHeader(jsondata, tableid) {/*Function used to get all column names from JSON and bind the html table header*/  
                var columnSet = [];  
                var headerTr$ = $('<tr/>');  
                for (var i = 0; i < jsondata.length; i++) {  
                    var rowHash = jsondata[i];  
                    for (var key in rowHash) {  
                        if (rowHash.hasOwnProperty(key)) {  
                            if ($.inArray(key, columnSet) == -1) {/*Adding each unique column names to a variable array*/  
                                columnSet.push(key);  
                                headerTr$.append($('<th/>').html(key));  
                            }  
                        }  
                    }  
                }  
                $(tableid).append(headerTr$);  
                return columnSet;  
            }
            $('#modalCOA').on('hidden.bs.modal', function () {
                 $(".btnUploadSave").click();
            })
        });
    </script>
    <style type="text/css">
     .hidden
     {
         display:none;
     }
     .files input {
            outline: 2px dashed #92b0b3;
            outline-offset: -10px;
            -webkit-transition: outline-offset .15s ease-in-out, background-color .15s linear;
            transition: outline-offset .15s ease-in-out, background-color .15s linear;
            padding: 120px 0px 85px 35%;
            text-align: center !important;
            margin: 0;
            width: 100% !important;
    }
    .files input:focus{     outline: 2px dashed #92b0b3;  outline-offset: -10px;
        -webkit-transition: outline-offset .15s ease-in-out, background-color .15s linear;
        transition: outline-offset .15s ease-in-out, background-color .15s linear; border:1px solid #92b0b3;
        }
    .files{ position:relative}
    .files:after {  pointer-events: none;
        position: absolute;
        top: 60px;
        left: 0;
        width: 50px;
        right: 0;
        height: 56px;
        content: "";
        background-image:url("../Images/upload.png");
        display: block;
        margin: 0 auto;
        background-size: 100%;
        background-repeat: no-repeat;
    }
    .color input{ background-color:#f1f1f1;}
    .files:before {
        position: absolute;
        bottom: 10px;
        left: 0;  pointer-events: none;
        width: 100%;
        right: 0;
        height: 40px;
        content: attr(title);
        display: block;
        margin: 0 auto;
        color: #507CD1;
        font-weight: 600;
        text-transform: capitalize;
        text-align: center;
    }
    .divUpload {
        display:none;
    }
    </style>

    <asp:Panel runat="server">
         <div class="row mb-3 mr-2 justify-content-end">
                <div class="col">
                 <div class="input-group">
                      <asp:TextBox ID="txtFilter" runat="server" class="txtFilter form-control" autocomplete="off" placeholder="Customer Name"  />
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
        <asp:GridView ID="gvCustomer"  PageSize="13" runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Customer_Code" HeaderText="Customer Code" />
                <asp:BoundField DataField="Classification" HeaderText="Classification" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Customer_Name" HeaderText="Customer Name" />
                <asp:BoundField DataField="First_Name" HeaderText="First_Name" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Last_Name" HeaderText="Last_Name" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Middle_Name" HeaderText="Middle_Name" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Suffix_Name" HeaderText="Suffix_Name" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="TIN_No" HeaderText="TIN_No" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                 <asp:BoundField DataField="BranchCode" HeaderText="BranchCode" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Lot_Unit" HeaderText="Billing_Lot_Unit" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Blk_Bldg" HeaderText="Billing_Blk_Bldg" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Street" HeaderText="Billing_Street" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Subd" HeaderText="Billing_Subd" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Brgy" HeaderText="Billing_Brgy" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />                
                <asp:BoundField DataField="Billing_Town_City" HeaderText="Billing_Town_City" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Province" HeaderText="Billing_Province" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_Region" HeaderText="Billing_Region" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Billing_ZipCode" HeaderText="Billing_ZipCode" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />                
                <asp:BoundField DataField="Delivery_Lot_Unit" HeaderText="Delivery_Lot_Unit" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Blk_Bldg" HeaderText="Delivery_Blk_Bldg" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Street" HeaderText="Delivery_Street" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Subd" HeaderText="Delivery_Subd" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Brgy" HeaderText="Delivery_Brgy" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />                
                <asp:BoundField DataField="Delivery_Town_City" HeaderText="Delivery_Town_City" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Province" HeaderText="Delivery_Province" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_Region" HeaderText="Delivery_Region" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Delivery_ZipCode" HeaderText="Delivery_ZipCode" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Contact_Person" HeaderText="Contact Person" />
                <asp:BoundField DataField="Contact_Position" HeaderText="Contact_Position" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Contact_Telephone" HeaderText="Contact_Telephone" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Contact_Cellphone" HeaderText="Contact Cellphone" />
                <asp:BoundField DataField="Contact_Fax" HeaderText="Contact_Fax" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Contact_Email" HeaderText="Contact_Email" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Contact_Website" HeaderText="Contact_Website" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Terms" HeaderText="Terms" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="CutOff" HeaderText="CutOff" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="VAT_Type" HeaderText="VAT_Type" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="AccountNo" HeaderText="AccountNo" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="DateModified" HeaderText="DateModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="WhoCreated" HeaderText="WhoCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                <asp:BoundField DataField="WhoModified" HeaderText="WhoModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />




                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnView" class="btnView" Text="View" runat="server" title='<%# Eval("Customer_Code")%>' />
                        <asp:Button ID="btnEdit" class="btnEdit" Text="Edit" runat="server" title='<%# Eval("Customer_Code")%>' />
                        <asp:Button ID="btnInactive" CssClass="btnInactive" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("Customer_Code")%>' />
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

        
        <%-- Upload Customer Modal --%>
        <div class="modal fade " id="modalCOA" tabindex="-1" role="dialog" aria-labelledby="modalLabelCOA" aria-hidden="true">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalLabelCOA">Upload Customer</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body">
                        <div class="alert alert-danger alert-dismissible fade show" style="display:none;" role="alert" id="alertUpdate" runat="server">
                            <strong>Invalid File!</strong>
                            <button type="button" class="close" id="alertClose" aria-label="Close">
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <div class="row">
                            <div class="col">
                                <div class="form-group files" title=" Drag and drop file here ">
                                    <input type="file" id="File_Upload" class="form-control" accept=".xlsm">
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <div class="progress">
                                    <div class="progress-bar" id="divUploadProgress" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width:0%">
                                        0%
                                    </div>
                                </div>
                            </div>
                            <br />
                        </div>
                        <div class="row">
                            <div class="col">
                                <div style="overflow-y:scroll;">
                                    <asp:GridView ID="gvUpload" CssClass="gvUpload" runat="server" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True">
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="Customer_Code" HeaderText = "CustomerCode" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Classification" HeaderText = "Classification" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="First_Name" HeaderText = "FirstName" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Last_Name" HeaderText = "LastName" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Middle_Name" HeaderText = "MiddleName" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Suffix_Name" HeaderText = "SuffixName" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Customer_Name" HeaderText = "CustomerName" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="TIN_No" HeaderText = "TINNo" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="BranchCode" HeaderText = "BranchCode" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Billing_Lot_Unit" HeaderText = "BillingLotUnit" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Billing_Blk_Bldg" HeaderText = "BillingBlkBldg" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Billing_Street" HeaderText = "BillingStreet" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Billing_Subd" HeaderText = "BillingSubd" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Billing_Brgy" HeaderText = "BillingBrgy" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Billing_Town_City" HeaderText = "BillingTownCity" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Billing_Province" HeaderText = "BillingProvince" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Billing_Region" HeaderText = "BillingRegion" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Billing_ZipCode" HeaderText = "BillingZipCode" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Delivery_Lot_Unit" HeaderText = "DeliveryLotUnit" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Delivery_Blk_Bldg" HeaderText = "DeliveryBlkBldg" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Delivery_Street" HeaderText = "DeliveryStreet" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Delivery_Subd" HeaderText = "DeliverySubd" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Delivery_Brgy" HeaderText = "DeliveryBrgy" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Delivery_Town_City" HeaderText = "DeliveryTownCity" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Delivery_Province" HeaderText = "DeliveryProvince" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Delivery_Region" HeaderText = "DeliveryRegion" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Delivery_ZipCode" HeaderText = "DeliveryZipCode" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="SameAddress" HeaderText = "SameAddress" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Person" HeaderText = "ContactPerson" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Position" HeaderText = "ContactPosition" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Telephone" HeaderText = "ContactTelephone" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Cellphone" HeaderText = "ContactCellphone" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Fax" HeaderText = "ContactFax" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Email" HeaderText = "ContactEmail" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Website" HeaderText = "ContactWebsite" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Terms" HeaderText = "Terms" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="CutOff" HeaderText = "CutOff" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="VAT_Type" HeaderText = "VATType" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="AccountNo" HeaderText = "AccountNo" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
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
                                <br />
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnUploadSave" Text="Close" class="btnUploadSave btn btn-primary btn-block" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
