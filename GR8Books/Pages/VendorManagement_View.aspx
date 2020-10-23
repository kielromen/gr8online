<%@ Page Title="Vendor Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="VendorManagement_View.aspx.vb" Inherits="GR8Books.VendorManagement_View" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/xlsx.core.min.js"></script>
    <script src="../Scripts/xls.core.min.js"></script>
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var Code = $(this).attr("title");
                var Actions = "View"
                window.open("VendorManagement.aspx?id=" + Code + '&Actions=' + Actions, "View", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnEdit").click(function () {
                var Code = $(this).attr("title");
                var Actions = "Edit"
                window.open("VendorManagement.aspx?id=" + Code + '&Actions=' + Actions, "Edit", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "VendorManagement.aspx";
                return false;
            });

            $(".btnInactive").click(function () {
                if (confirm("Are you sure you want to remove this?")) {
                }
                else {
                    return false;
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
                        url: "<%= ResolveUrl("VendorManagement_View.aspx/SaveVCE") %>",
                        data: "{'Vendor_Code':'" + row.Vendor_Code + "', 'Classification':'" + row.Classification + "', 'First_Name':'" + row.First_Name + "', 'Last_Name':'" + row.Last_Name + "', 'Middle_Name':'" + row.Middle_Name + "', 'Suffix_Name':'" + row.Suffix_Name + "', 'Vendor_Name':'" + row.Vendor_Name + "', 'TIN_No':'" + row.TIN_No + "', 'BranchCode':'" + row.BranchCode + "', 'Address_Lot_Unit':'" + row.Address_Lot_Unit + "', 'Address_Blk_Bldg':'" + row.Address_Blk_Bldg + "', 'Address_Street':'" + row.Address_Street + "', 'Address_Subd':'" + row.Address_Subd + "', 'Address_Brgy':'" + row.Address_Brgy + "', 'Address_Town_City':'" + row.Address_Town_City + "', 'Address_Province':'" + row.Address_Province + "', 'Address_Region':'" + row.Address_Region + "', 'Address_ZipCode':'" + row.Address_ZipCode + "', 'Contact_Person':'" + row.Contact_Person + "', 'Contact_Position':'" + row.Contact_Position + "', 'Contact_Telephone':'" + row.Contact_Telephone + "', 'Contact_Cellphone':'" + row.Contact_Cellphone + "', 'Contact_Fax':'" + row.Contact_Fax + "', 'Contact_Email':'" + row.Contact_Email + "', 'Contact_Website':'" + row.Contact_Website + "', 'Terms':'" + row.Terms + "', 'CutOff':'" + row.CutOff + "', 'VAT_Type':'" + row.VAT_Type + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var bool = data.d;
                            var row$ = $('<tr style="white-space:nowrap;"/>');  
                            row$.append($('<td/>').html(row.Vendor_Code));
                            row$.append($('<td/>').html(row.Classification));
                            row$.append($('<td/>').html(row.First_Name));
                            row$.append($('<td/>').html(row.Last_Name));
                            row$.append($('<td/>').html(row.Middle_Name));
                            row$.append($('<td/>').html(row.Suffix_Name));
                            row$.append($('<td/>').html(row.Vendor_Name));
                            row$.append($('<td/>').html(row.TIN_No));
                            row$.append($('<td/>').html(row.BranchCode));
                            row$.append($('<td/>').html(row.Address_Lot_Unit));
                            row$.append($('<td/>').html(row.Address_Blk_Bldg));
                            row$.append($('<td/>').html(row.Address_Street));
                            row$.append($('<td/>').html(row.Address_Subd));
                            row$.append($('<td/>').html(row.Address_Brgy));
                            row$.append($('<td/>').html(row.Address_Town_City));
                            row$.append($('<td/>').html(row.Address_Province));
                            row$.append($('<td/>').html(row.Address_Region));
                            row$.append($('<td/>').html(row.Address_ZipCode));
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
        });
    </script>
    
    <style>
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
<%--     <div class="alert alert-success alert-dismissible fade show" role="alert" id="alertSave" runat="server">
        <strong>Record successfully saved!</strong>
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>
    <div class="alert alert-warning alert-dismissible fade show" role="alert" id="Div1" runat="server">
        <strong>Record successfully updated!</strong>
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>--%>
    <asp:Panel runat="server">
        <div class="row mt-4 justify-content-end">
            <div class="col-2 ">
                <asp:Button ID="btnExport" class="btnExport btn btn-success btn-block" Text="Export" runat="server" />
            </div>
        </div>
        <br />
        <asp:GridView ID="gvVendor" runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="Vendor_Code" HeaderText="Vendor Code" />
                <asp:BoundField DataField="Vendor_Name" HeaderText="Vendor Name" />
                <asp:BoundField DataField="Contact_Person" HeaderText="Contact Person" />
                <asp:BoundField DataField="Contact_Cellphone" HeaderText="Contact Cellphone" />
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:Button ID="btnView" class="btnView" Text="View" runat="server" title='<%# Eval("Vendor_Code")%>' />
                        <asp:Button ID="btnEdit" class="btnEdit" Text="Edit" runat="server" title='<%# Eval("Vendor_Code")%>' />
                        <asp:Button ID="btnInactive" CssClass="btnInactive" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("Vendor_Code")%>' />
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
        <div class="row mt-4 justify-content-end">
            <div class="col-2 ">
                <asp:Button ID="btnAdd" class="btnAdd btn btn-primary btn-block" Text="Add" runat="server" />
                <asp:Button ID="btnUpload" class="btnUpload btn btn-primary btn-block" Text="Upload" runat="server" data-toggle="modal" data-target="#modalCOA" data-whatever="@mdo" />
            </div>

        </div>
        
        <%-- Upload Vendor Modal --%>
        <div class="modal fade " id="modalCOA" tabindex="-1" role="dialog" aria-labelledby="modalLabelCOA" aria-hidden="true">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalLabelCOA">Upload Chart of Account</h5>
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
                                            <asp:BoundField DataField="Vendor_Code" HeaderText = "Vendor Code" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Classification" HeaderText = "Classification" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="First_Name" HeaderText = "First Name" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Last_Name" HeaderText = "Last Name" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Middle_Name" HeaderText = "Middle Name" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Suffix_Name" HeaderText = "Suffix Name" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Vendor_Name" HeaderText = "Vendor Name" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="TIN_No" HeaderText = "TIN No" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="BranchCode" HeaderText = "BranchCode" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Address_Lot_Unit" HeaderText = "Address Lot Unit" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Address_Blk_Bldg" HeaderText = "Address Blk Bldg" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Address_Street" HeaderText = "Address Street" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Address_Subd" HeaderText = "Address Subd" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Address_Brgy" HeaderText = "Address Brgy" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Address_Town_City" HeaderText = "Address Town City" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Address_Province" HeaderText = "Address Province" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Address_Region" HeaderText = "Address Region" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Address_ZipCode" HeaderText = "Address ZipCode" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Person" HeaderText = "Contact Person" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Position" HeaderText = "Contact Position" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Telephone" HeaderText = "Contact Telephone" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Cellphone" HeaderText = "Contact Cellphone" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Fax" HeaderText = "Contact Fax" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Email" HeaderText = "Contact Email" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Contact_Website" HeaderText = "Contact Website" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="Terms" HeaderText = "Terms" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="CutOff" HeaderText = "CutOff" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="VAT_Type" HeaderText = "VAT Type" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
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
                        <asp:Button ID="btnUploadSave" Text="Save" class="btnUploadSave btn btn-primary btn-block" runat="server" />
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

</asp:Content>
