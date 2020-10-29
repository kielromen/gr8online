<%@ Page Title="Employee Management" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Employee_Loadlist.aspx.vb" Inherits="GR8Books.Employee_Loadlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/xlsx.core.min.js"></script>
    <script src="../Scripts/xls.core.min.js"></script>
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var ID = $(this).attr("title");
                var Actions = "View"
                window.open("Employee_Management.aspx?id=" + ID + '&Actions=' + Actions, "View", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnEdit").click(function () {
                var ID = $(this).attr("title");
                var Actions = "Edit"
                window.open("Employee_Management.aspx?id=" + ID + '&Actions=' + Actions, "Edit", "width=" + screen.availWidth + ",height=" + screen.availHeight);
                return false;
            });

            $(".btnAdd").click(function () {
                window.location = "Employee_Management.aspx";
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
                        url: "<%= ResolveUrl("Employee_LoadList.aspx/SaveVCE") %>",
                        data: "{'Employee_Code':'" + row.Employee_Code + "', 'Suffix_Name':'" + row.Suffix_Name + "', 'First_Name':'" + row.First_Name + "', 'Middle_Name':'" + row.Middle_Name + "', 'Last_Name':'" + row.Last_Name + "', 'Employee_Name':'" + row.Employee_Name + "', 'Department':'" + row.Department + "', 'Section':'" + row.Section + "', 'Unit':'" + row.Unit + "', 'Address_Lot_Unit':'" + row.Address_Lot_Unit + "', 'Address_Blk_Bldg':'" + row.Address_Blk_Bldg + "', 'Address_Street':'" + row.Address_Street + "', 'Address_Subd':'" + row.Address_Subd + "', 'Address_Brgy':'" + row.Address_Brgy + "', 'Address_Town_City':'" + row.Address_Town_City + "', 'Address_Province':'" + row.Address_Province + "', 'Address_Region':'" + row.Address_Region + "', 'Address_ZipCode':'" + row.Address_ZipCode + "', 'EmailAddress':'" + row.EmailAddress + "', 'CellphoneNo':'" + row.CellphoneNo + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var bool = data.d;
                            var row$ = $('<tr style="white-space:nowrap;"/>');  
                            row$.append($('<td/>').html(row.Employee_Code));
                            row$.append($('<td/>').html(row.Suffix_Name));
                            row$.append($('<td/>').html(row.First_Name));
                            row$.append($('<td/>').html(row.Middle_Name));
                            row$.append($('<td/>').html(row.Last_Name));
                            row$.append($('<td/>').html(row.Employee_Name));
                            row$.append($('<td/>').html(row.Department));
                            row$.append($('<td/>').html(row.Section));
                            row$.append($('<td/>').html(row.Unit));
                            row$.append($('<td/>').html(row.Address_Lot_Unit));
                            row$.append($('<td/>').html(row.Address_Blk_Bldg));
                            row$.append($('<td/>').html(row.Address_Street));
                            row$.append($('<td/>').html(row.Address_Subd));
                            row$.append($('<td/>').html(row.Address_Brgy));
                            row$.append($('<td/>').html(row.Address_Town_City));
                            row$.append($('<td/>').html(row.Address_Province));
                            row$.append($('<td/>').html(row.Address_Region));
                            row$.append($('<td/>').html(row.Address_ZipCode));
                            row$.append($('<td/>').html(row.EmailAddress));
                            row$.append($('<td/>').html(row.CellphoneNo));
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

    <div>
        <asp:Panel runat="server">
             <div class="row mt-4 justify-content-end">
                 <div class="col-2 ">
                    <asp:Button ID="btnExport" class="btnExport btn btn-success btn-block" Text="Export" runat="server" />
                </div>
            </div>
             <br />
            <asp:GridView ID="gvEmployee" runat="server" AutoGenerateColumns="false" AllowPaging="True" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
                <AlternatingRowStyle BackColor="White" />
                <Columns>
                    <asp:BoundField DataField="Employee_Code" HeaderText="Employee Code" />
                    <asp:BoundField DataField="Employee_Name" HeaderText="Employee_Name" />
                    <asp:BoundField DataField="Section" HeaderText="Section" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Unit" HeaderText="Unit" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Address_Lot_Unit" HeaderText="Address_Lot_Unit" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Address_Blk_Bldg" HeaderText="Address_Blk_Bldg" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Address_Street" HeaderText="Address_Street" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Address_Subd" HeaderText="Address_Subd" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Address_Brgy" HeaderText="Address_Brgy" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />                
                    <asp:BoundField DataField="Address_Town_City" HeaderText="Address_Town_City" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Address_Province" HeaderText="Address_Province" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Address_Region" HeaderText="Address_Region" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="Address_ZipCode" HeaderText="Address_ZipCode" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="EmailAddress" HeaderText="EmailAddress" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="CellphoneNo" HeaderText="CellphoneNo" />
                    <asp:BoundField DataField="Status" HeaderText="Status" />
                    <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="DateModified" HeaderText="DateModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="WhoCreated" HeaderText="WhoCreated" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />
                    <asp:BoundField DataField="WhoModified" HeaderText="WhoModified" HeaderStyle-CssClass="hidden" ItemStyle-CssClass="hidden" />

                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:Button ID="btnView" class="btnView" Text="View" runat="server" title='<%# Eval("Employee_Code") %>' />
                            <asp:Button ID="btnEdit" class="btnEdit" Text="Edit" runat="server" title='<%# Eval("Employee_Code") %>' />
                            <asp:Button ID="btnInactive" class="btnInactive" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("Employee_Code") %>' />
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
            <div class="row justify-content-end mt-4">
                <div class="col-sm-2">
                    <asp:Button ID="btnAdd" class="btnAdd btn btn-primary btn-block" Text="Add" runat="server" />
                </div>
                <div class="col-2 ">
                    <asp:Button ID="btnUpload" class="btnUpload btn btn-primary btn-block" Text="Upload" runat="server" data-toggle="modal" data-target="#modalCOA" data-whatever="@mdo" />
                </div>
            </div>
            
            <%-- Upload Employee Modal --%>
            <div class="modal fade " id="modalCOA" tabindex="-1" role="dialog" aria-labelledby="modalLabelCOA" aria-hidden="true">
                <div class="modal-dialog modal-xl">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h5 class="modal-title" id="modalLabelCOA">Upload Employee</h5>
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
                                                <asp:BoundField DataField="Employee_Code" HeaderText = "Employee Code" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Suffix_Name" HeaderText = "Suffix Name" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="First_Name" HeaderText = "First Name" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Middle_Name" HeaderText = "Middle Name" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Last_Name" HeaderText = "Last Name" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Employee_Name" HeaderText = "Employee Name" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Department" HeaderText = "Department" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Section" HeaderText = "Section" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Unit" HeaderText = "Unit" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Address_Lot_Unit" HeaderText = "Address Lot Unit" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Address_Blk_Bldg" HeaderText = "Address Blk Bldg" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Address_Street" HeaderText = "Address Street" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Address_Subd" HeaderText = "Address Subd" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Address_Brgy" HeaderText = "Address Brgy" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Address_Town_City" HeaderText = "Address Town City" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Address_Province" HeaderText = "Address Province" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Address_Region" HeaderText = "Address Region" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="Address_ZipCode" HeaderText = "Address ZipCode" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="EmailAddress" HeaderText = "EmailAddress" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                                <asp:BoundField DataField="CellphoneNo" HeaderText = "CellphoneNo" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
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
    </div>
</asp:Content>
