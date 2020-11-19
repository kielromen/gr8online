<%@ Page Title="Chart of Accounts"  MaintainScrollPositionOnPostback="true" Language="vb" AutoEventWireup="false" MasterPageFile="~/Master/Dashboard.Master" CodeBehind="ChartofAccount_Loadlist.aspx.vb" Inherits="GR8Books.ChartofAccount_Loadlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <script src="../Scripts/jquery-ui.js"></script>
    <script src="../Scripts/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".gvCOA").sortable({
                items: 'tr:not(tr:first-child)',
                cursor: 'pointer',
                axis: 'y',
                dropOnEmpty: false,
                start: function (e, ui) {
                    ui.item.addClass("selected");
                },
                stop: function (e, ui) {
                    ui.item.removeClass("selected");
                    $('#<%=btnSort.ClientID%>').click();
                },
            });
        });
    </script>
    <script src="../Scripts/xlsx.core.min.js"></script>
    <script src="../Scripts/xls.core.min.js"></script>
    <script type='text/javascript'>
        $(document).ready(function () {
            $(".btnView").click(function () {
                var ID = $(this).attr("title");
                var Actions = "View"
                window.open("ChartofAccounts.aspx?id=" + ID + '&Actions=' + Actions, "_self");
                return false;
            });

            $(".btnEdit").click(function () {
                var ID = $(this).attr("title");
                var Actions = "Edit"
                window.open("ChartofAccounts.aspx?id=" + ID + '&Actions=' + Actions, "_self");
                return false;
            });

            $(".btnAdd").click(function () {
                //window.location = "ChartofAccounts.aspx";
                //return false;
                var Actions = "Add"
                window.open("ChartofAccounts.aspx?Actions=" + Actions, "_self");
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

            $("#COA_Upload").change(function (e) {
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
                                    BindTable(exceljson, '#COA_Upload');  
                                    cnt++;  
                                }  
                            });
                        } 
                        reader.readAsArrayBuffer($("#COA_Upload")[0].files[0]);  
                    }
                }
            });

            $("#alertClose").click(function () {
                $("#<%=alertUpdate.ClientID%>").hide();
            });
            $("#<%=btnUpload.ClientID%>").click(function (e) {
                e.preventDefault();
            });
            
            function BindTable(jsondata, tableid) {/*Function used to convert the JSON array to Html Table*/  
                var columns = BindTableHeader(jsondata, tableid); /*Gets all the column headings of Excel*/  
                var rowCount = 0;
                $.each(jsondata, function (i, row) {
                    $.ajax({
                        url: "<%= ResolveUrl("ChartofAccount_Loadlist.aspx/SaveCOA") %>",
                        data: "{'AccountCode': '" + row.AccountCode + "','AccountTitle': '" + row.AccountTitle + "','AccountType': '" + row.AccountType + "','AccountGroup': '" + row.AccountGroup + "','AccountNature': '" + row.AccountNature + "','ReportAlias': '" + row.ReportAlias + "','AccountClass': '" + row.Class + "','withSubsidiary': '" + row.withSubsidiary + "','OrderNo': '" + row.OrderNo + "'}",
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json; charset=utf-8",
                        success: function (data) {
                            var bool = data.d;
                            var row$ = $('<tr/>');  
                            row$.append($('<td/>').html(row.AccountCode));
                            row$.append($('<td/>').html(row.AccountType));
                            row$.append($('<td/>').html(row.AccountTitle));
                            row$.append($('<td/>').html(row.AccountGroup));
                            row$.append($('<td/>').html(row.AccountNature));
                            row$.append($('<td/>').html(row.ReportAlias));
                            row$.append($('<td/>').html(row.Class));
                            row$.append($('<td/>').html(row.withSubsidiary));
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
    <asp:Panel runat="server">
           <div class="row mb-3 mr-2 justify-content-end">
                <div class="col">
                    <asp:DropDownList ID="ddlAccountType" runat="server" EnableViewState="true" AppendDataBoundItems="true" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="Loadlist"></asp:DropDownList>
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
            <div class="row">
                <div class="col" id="divCOA">
                    <asp:GridView ID="gvCOA" CssClass="gvCOA" runat="server" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True" EmptyDataText="No Records Found">
                        <AlternatingRowStyle BackColor="White" />
                        <Columns>
                            <asp:TemplateField HeaderText="No" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <%# Container.DataItemIndex + 1 %>
                                    <input type="hidden" name="Code" value='<%# Eval("AccountCode") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="AccountCode" HeaderText="Code"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"  />
                            <asp:BoundField DataField="AccountTitle" HeaderText="Description"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="AccountGroup" HeaderText="Account Group"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="AccountNature" HeaderText="Account Nature"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"  />
                            <asp:BoundField DataField="withSubsidiary" HeaderText="With Subsidiary"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                            <asp:BoundField DataField="Class" HeaderText="Class" ItemStyle-Width="100" />
                            <asp:TemplateField HeaderText="Actions" ItemStyle-Width="350">
                                <ItemTemplate>
                                    <asp:Button  ID="btnView" class="btnView" Text="View" runat="server" title='<%# Eval("AccountCode") %>' />
                                    <asp:Button ID="btnEdit" class="btnEdit" Text="Edit" runat="server" title='<%# Eval("AccountCode") %>' />
                                    <asp:Button ID="btnInactive" class="btnInactive" Text="Inactive" runat="server" CommandName="btnInactive" CommandArgument='<%# Bind("AccountCode") %>' />
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
                     <div class="col">
                         <asp:Button ID="btnSort"  Display="Dynamic" class="btnSort btn btn-primary btn-block" Text="Update Order" runat="server" OnClick="UpdateSort" />
                     </div>
                </div>
            </div>

        
        <%-- Upload COA Modal --%>
        <div class="modal fade " id="modalCOA" tabindex="-1" role="dialog" aria-labelledby="modalLabelCOA" aria-hidden="true">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="modalLabelCOA">Upload Chart of Accounts</h5>
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
                                    <input type="file" id="COA_Upload" class="form-control" accept=".xlsm">
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
                                <div style="overflow:scroll;">
                                                                <asp:GridView ID="gvUpload" CssClass="gvUpload" runat="server" AutoGenerateColumns="false" CellPadding="4" ForeColor="#333333" Width="100%" GridLines="None" ShowHeaderWhenEmpty="True">
                                    <AlternatingRowStyle BackColor="White" />
                                    <Columns>
                                        <asp:BoundField DataField="AccountCode" HeaderText="Code"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                        <asp:BoundField DataField="AccountType" HeaderText="Type"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                        <asp:BoundField DataField="AccountTitle" HeaderText="Description"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>
                                        <asp:BoundField DataField="AccountGroup" HeaderText="Account Group"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                        <asp:BoundField DataField="AccountNature" HeaderText="Account Nature"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                        <asp:BoundField DataField="ReportAlias" HeaderText="Report Alias"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false"/>
                                        <asp:BoundField DataField="Class" HeaderText="Class"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
                                        <asp:BoundField DataField="withSubsidiary" HeaderText="With Subsidiary"  HeaderStyle-Wrap="false" ItemStyle-Wrap="false" />
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
                                <br />
                            </div>
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
