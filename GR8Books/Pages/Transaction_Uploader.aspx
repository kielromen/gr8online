<%@ Page Title="Transacton Uploader" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="Transaction_Uploader.aspx.vb" Inherits="GR8Books.Transaction_Uploader" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="../Scripts/xlsx.core.min.js"></script>
    <script src="../Scripts/xls.core.min.js"></script>
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
            
            $("#<%=btnUpload.ClientID%>").click(function (e) {
                e.preventDefault();
            });

            $("#<%=File_Upload.ClientID%>").change(function (e) {
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
                                    $(".files").attr("title", "Reading");
                                    $(".files").css({ "background-image": "url('../Images/waiting.png')" });
                                    BindTable(exceljson, '#<%=File_Upload.ClientID%>');  
                                    cnt++;  
                                }  
                            });
                        } 
                        reader.readAsArrayBuffer($("#<%=File_Upload.ClientID%>")[0].files[0]);  
                    }
                }
            });
            
            function BindTable(jsondata, tableid) {/*Function used to convert the JSON array to Html Table*/  
                var columns = BindTableHeader(jsondata, tableid); /*Gets all the column headings of Excel*/  
                var rowCount = 0;
                $.each(jsondata, function (i, row) {
                    var d = new Date(row.Date);
                    var day = d.getDate();
                    var month = d.getMonth() + 1;
                    var year = d.getFullYear();
                    if (day < 10) {
                        day = "0" + day;
                    }
                    if (month < 10) {
                        month = "0" + month;
                    }
                    var date = year + "/" + month + "/" + day;
                    var bg = "white";
                    if ((rowCount % 2) == 0) {
                        bg = "#EFF3FB"
                    }
                    var row$ = $('<tr style="white-space:nowrap;background-color:' + bg + '"/>');  
                    row$.append($('<td/>').html(date));
                    row$.append($('<td/>').html(row.Doc_Type));
                    row$.append($('<td/>').html(row.Doc_No));
                    row$.append($('<td/>').html(row.AccntCode));
                    row$.append($('<td/>').html(row.AccountTitle));
                    row$.append($('<td/>').html(row.VCECode));
                    row$.append($('<td/>').html(row.VCEname));
                    row$.append($('<td/>').html(row.Debit));
                    row$.append($('<td/>').html(row.Credit));
                    row$.append($('<td/>').html(row.Particulars));
                    row$.append($('<td/>').html(row.RefNo));
                    row$.append($('<td/>').html(row.CostCenter));
                    row$.append($('<td/>').html(row.Book));
                    console.log(row$);
                    $("#<%=gvUpload.ClientID%>").append(row$);  
                    rowCount++;
                    var percent =  parseFloat((rowCount/(jsondata.length)) * 100).toFixed(0);
                    $("#divUploadProgress").css({ "width": percent + "%" }).html(percent + "%");
                    if (rowCount == jsondata.length) {
                        $(".files").attr("title", "Read Complete");
                    }
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

            $("#ToTop").click(function () {
                $("#modalCOA").animate({ scrollTop: 0}, "fast");
            });
            $("#ToBottom").click(function () {
                $("#modalCOA").animate({ scrollTop: $(".modal-dialog").height() }, "fast");
            }); 


            $("#ToTop1").click(function () {
                $("#uploader").animate({ scrollTop: 0 }, "fast");
            });
            $("#ToBottom1").click(function () {
                var _gridheight = $('.gvUploader').height();
                $("#uploader").animate({ scrollTop: _gridheight }, "fast");
            }); 
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
        #scroll {
            position:fixed;
            bottom:40px;
            right:40px;
        }

        #scroll1 {
            position:fixed;
            bottom:40px;
            right:40px;
        }
    </style>
    <asp:Panel runat="server">
        <div class="row">
            <div class="col">
                 <div class="row mb-2">
                    <div class="col-12">
                        <asp:Button Text="Search" ID="btnSearch" runat="server" class="btn btn-primary" />
                <asp:Button ID="btnUpload" class="btnUpload btn btn-primary" Text="Upload" runat="server" data-toggle="modal" data-target="#modalCOA" data-whatever="@mdo" />
                        <asp:Button Text="Cancel" ID="btnCancel" class="btnCancel btn btn-primary" runat="server" />
                        <asp:Button ID="btnExport" class="btnExport btn btn-success" Text="Export" runat="server" />
                    </div>
                </div>
                 <br />
                 <div id="uploader" style="width: 100%; height: 500px;overflow-y: scroll;overflow:scroll">
                <asp:GridView Class="gvUploader" ID="gvUploader" runat="server" AutoGenerateColumns="false"  Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found" ShowFooter="true">
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
                <div id="scroll1">
                <div class="row p-1">
                    <a id="ToTop1" class="btn btn-light btn-lg back-to-top"><i class="fa fa-arrow-up"></i></a>
                </div>
                <div class="row p-1">
                    <a id="ToBottom1" class="btn btn-light btn-lg back-to-top"><i class="fa fa-arrow-down"></i></a>
                </div>
            </div>
             </div>
            </div>
        </div>
        <%-- Upload Transaction Modal --%>
        <div class="modal fade " id="modalCOA" tabindex="-1" role="dialog" aria-labelledby="modalLabelCOA" aria-hidden="true">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalLabelCOA">Upload Transaction</h5>
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
                                    <input type="file" id="File_Upload" class="form-control" accept=".xlsm" runat="server">
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
                                <div style="overflow-x:scroll;overflow-y:hidden;">
                                    <asp:GridView ID="gvUpload" CssClass="gvUpload" runat="server" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" >
                                        <AlternatingRowStyle BackColor="White" />
                                        <Columns>
                                            <asp:BoundField DataField="UDate" HeaderText = "Date" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="UDoc_Type" HeaderText = "Doc Type" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="UDoc_No" HeaderText = "Doc No" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="UAccntCode" HeaderText = "AccntCode" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="UAccountTitle" HeaderText = "AccountTitle" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="UVCECode" HeaderText = "VCECode" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="UVCEname" HeaderText = "VCEname" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="UDebit" HeaderText = "Debit" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="UCredit" HeaderText = "Credit" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="UParticulars" HeaderText = "Particulars" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="URefNo" HeaderText = "RefNo" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="UCostCenter" HeaderText = "CostCenter" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                            <asp:BoundField DataField="UBook" HeaderText = "Book" HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
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
                        <asp:Button ID="btnUploadSave" Text="Save" class="btnUploadSave btn btn-primary" runat="server" />
                    </div>
                </div>
            </div>
            <div id="scroll">
                <div class="row p-1">
                    <a id="ToTop" class="btn btn-light btn-lg back-to-top"><i class="fa fa-arrow-up"></i></a>
                </div>
                <div class="row p-1">
                    <a id="ToBottom" class="btn btn-light btn-lg back-to-top"><i class="fa fa-arrow-down"></i></a>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>
