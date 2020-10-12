
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports System.Net
Public Class Reports
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                BindReport()
            End If
        End If
    End Sub

    Private Sub BindReport()
        Dim Type As String = Request.QueryString("ID")
        Dim file As String = ""
        Dim selectQuery As String = "SELECT * FROM [Main].dbo.tblCompany_Information WHERE Default_EmailAddress ='" & Session("EmailAddress") & "'"
        Dim dsHEADERrpt As dsHEADERrpt = GetData("HEADER", selectQuery)
        Select Case Type
            Case "CHKV"
                Dim query As String
                query = " SELECT * FROM View_CHKV_Printout WHERE TransID = @TransID " & vbCrLf &
                        " SELECT * FROM View_GL WHERE RefTransID = @RefTransID AND RefType = 'CHKV' "
                SQL.FlushParams()
                SQL.AddParam("@TransID", Session("TransID"))
                SQL.AddParam("@RefTransID", Session("TransID"))
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptCHKV.rpt"))

                Dim dsCHKVrpt As dsCHKVrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsCHKVrpt.Tables("Table"))
                crystalReport.Database.Tables(1).SetDataSource(dsCHKVrpt.Tables("Table1"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptCHKV.pdf"))
                file = Server.MapPath("~/Reports/rptCHKV.pdf")

                crystalReport.Close()
                crystalReport.Dispose()
            Case "CV"
                Dim query As String
                query = " SELECT * FROM View_CV_Printout WHERE TransID = @TransID " & vbCrLf &
                        " SELECT * FROM View_GL WHERE RefTransID = @RefTransID AND RefType = 'CV' "
                SQL.FlushParams()
                SQL.AddParam("@TransID", Session("TransID"))
                SQL.AddParam("@RefTransID", Session("TransID"))

                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptCV.rpt"))

                Dim dsCVrpt As dsCVrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsCVrpt.Tables("Table"))
                crystalReport.Database.Tables(1).SetDataSource(dsCVrpt.Tables("Table1"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptCV.pdf"))
                file = Server.MapPath("~/Reports/rptCV.pdf")

                crystalReport.Close()
                crystalReport.Dispose()
            Case "JV"
                Dim query As String
                query = " SELECT * FROM tblJV WHERE TransID = @TransID " & vbCrLf &
                        " SELECT * FROM View_GL WHERE RefTransID = @RefTransID AND RefType = 'JV' "
                SQL.FlushParams()
                SQL.AddParam("@TransID", Session("TransID"))
                SQL.AddParam("@RefTransID", Session("TransID"))

                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptJV.rpt"))

                Dim dsJVrpt As dsJVrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsJVrpt.Tables("Table"))
                crystalReport.Database.Tables(1).SetDataSource(dsJVrpt.Tables("Table1"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptJV.pdf"))
                file = Server.MapPath("~/Reports/rptJV.pdf")

                crystalReport.Close()
                crystalReport.Dispose()
            Case "APV"
                Dim query As String
                query = " SELECT * FROM View_APV_Printout WHERE TransID = @TransID " & vbCrLf &
                        " SELECT * FROM View_GL WHERE RefTransID = @RefTransID AND RefType = 'APV' "
                SQL.FlushParams()
                SQL.AddParam("@TransID", Session("TransID"))
                SQL.AddParam("@RefTransID", Session("TransID"))

                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptAPV.rpt"))

                Dim dsAPVrpt As dsAPVrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsAPVrpt.Tables("Table"))
                crystalReport.Database.Tables(1).SetDataSource(dsAPVrpt.Tables("Table1"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport

                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptAPV.pdf"))

                file = Server.MapPath("~/Reports/rptAPV.pdf")

                crystalReport.Close()
                crystalReport.Dispose()
            Case "CA"
                Dim query As String
                query = " SELECT * FROM View_CA_Printout WHERE TransID = @TransID "
                SQL.FlushParams()
                SQL.AddParam("@TransID", Session("TransID"))

                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptCA.rpt"))

                Dim dsCArpt As dsCArpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsCArpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptCA.pdf"))
                file = Server.MapPath("~/Reports/rptCA.pdf")

                crystalReport.Close()
                crystalReport.Dispose()
            Case "CASUM"
                Dim query As String
                query = " SELECT * FROM View_CA_Summary WHERE TransDate BETWEEN @DateFrom AND @DateTo"
                SQL.FlushParams()
                SQL.AddParam("@DateFrom", Session("@DateFrom"))
                SQL.AddParam("@DateTo", Session("@DateTo"))

                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptCASUM.rpt"))

                Dim dsCASUMrpt As dsCASUMrpt = GetData(Type, query)

                crystalReport.Database.Tables(0).SetDataSource(dsCASUMrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                crystalReport.SetParameterValue("@DateFrom", Session("@DateFrom"))
                crystalReport.SetParameterValue("@DateTo", Session("@DateTo"))
                crystalReport.SetParameterValue("@User", GetUserFullName(Session("EmailAddress")))
                CrystalReportViewer1.ReportSource = crystalReport

                If Session("@FileType") = "Excel" Then
                    crystalReport.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("~/Reports/rptCASUM.xls"))
                    Response.AppendHeader("Content-Disposition", "attachment; filename=rptCASUM.xls")
                    Response.TransmitFile(Server.MapPath("~/Reports/rptCASUM.xls"))
                    Response.End()
                Else
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptCASUM.pdf"))
                    file = Server.MapPath("~/Reports/rptCASUM.pdf")
                End If

                crystalReport.Close()
                crystalReport.Dispose()
            Case "CAUNLQ"
                Dim query As String
                query = " SELECT * FROM View_CA_Return"


                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptCAUNLQ.rpt"))

                Dim dsCAUNLQrpt As dsCAUNLQrpt = GetData(Type, query)

                crystalReport.Database.Tables(0).SetDataSource(dsCAUNLQrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                crystalReport.SetParameterValue("@User", GetUserFullName(Session("EmailAddress")))
                CrystalReportViewer1.ReportSource = crystalReport

                If Session("@FileType") = "Excel" Then
                    crystalReport.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("~/Reports/rptCAUNLQ.xls"))
                    Response.AppendHeader("Content-Disposition", "attachment; filename=rptCAUNLQ.xls")
                    Response.TransmitFile(Server.MapPath("~/Reports/rptCAUNLQ.xls"))
                    Response.End()
                Else
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptCAUNLQ.pdf"))
                    file = Server.MapPath("~/Reports/rptCAUNLQ.pdf")
                End If

                crystalReport.Close()
                crystalReport.Dispose()
            Case "PC"
                Dim query As String
                query = " SELECT * FROM View_PC_Printout WHERE TransID = @TransID "
                SQL.FlushParams()
                SQL.AddParam("@TransID", Session("TransID"))

                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptPC.rpt"))

                Dim dsPCrpt As dsPCrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsPCrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptPC.pdf"))
                file = Server.MapPath("~/Reports/rptPC.pdf")

                crystalReport.Close()
                crystalReport.Dispose()
            Case "TBDETAILED"
                Dim query As String
                query = " SELECT * FROM tblPrint_TB"
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptTB_Detailed.rpt"))

                Dim dsTBrpt As dsTBrpt = GetData(Type, query)

                crystalReport.Database.Tables(0).SetDataSource(dsTBrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                crystalReport.SetParameterValue("@DateFrom", Session("@DateFrom"))
                crystalReport.SetParameterValue("@DateTo", Session("@DateTo"))
                crystalReport.SetParameterValue("@User", GetUserFullName(Session("EmailAddress")))
                CrystalReportViewer1.ReportSource = crystalReport

                If Session("@FileType") = "Excel" Then
                    crystalReport.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("~/Reports/rptTB_Detailed.xls"))
                    Response.AppendHeader("Content-Disposition", "attachment; filename=rptTB_Detailed.xls")
                    Response.TransmitFile(Server.MapPath("~/Reports/rptTB_Detailed.xls"))
                    Response.End()
                Else
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptTB_Detailed.pdf"))
                    file = Server.MapPath("~/Reports/rptTB_Detailed.pdf")
                End If

                crystalReport.Close()
                crystalReport.Dispose()
            Case "TBSUMMARY"
                Dim query As String
                query = " SELECT * FROM tblPrint_TB"
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptTB_Summary.rpt"))

                Dim dsTBrpt As dsTBrpt = GetData(Type, query)

                crystalReport.Database.Tables(0).SetDataSource(dsTBrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                crystalReport.SetParameterValue("@DateFrom", Session("@DateFrom"))
                crystalReport.SetParameterValue("@DateTo", Session("@DateTo"))
                crystalReport.SetParameterValue("@User", GetUserFullName(Session("EmailAddress")))
                CrystalReportViewer1.ReportSource = crystalReport

                If Session("@FileType") = "Excel" Then
                    crystalReport.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("~/Reports/rptTB_Summary.xls"))
                    Response.AppendHeader("Content-Disposition", "attachment; filename=rptTB_Summary.xls")
                    Response.TransmitFile(Server.MapPath("~/Reports/rptTB_Summary.xls"))
                    Response.End()
                Else
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptTB_Summary.pdf"))
                    file = Server.MapPath("~/Reports/rptTB_Summary.pdf")
                End If

                crystalReport.Close()
                crystalReport.Dispose()
            Case "TBBEFOREPEC"
                Dim query As String
                query = " SELECT * FROM tblPrint_TB"
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptTB_BEFOREPEC.rpt"))

                Dim dsTBrpt As dsTBrpt = GetData(Type, query)

                crystalReport.Database.Tables(0).SetDataSource(dsTBrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                crystalReport.SetParameterValue("@DateFrom", Session("@DateFrom"))
                crystalReport.SetParameterValue("@DateTo", Session("@DateTo"))
                crystalReport.SetParameterValue("@User", GetUserFullName(Session("EmailAddress")))
                CrystalReportViewer1.ReportSource = crystalReport

                If Session("@FileType") = "Excel" Then
                    crystalReport.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("~/Reports/rptTB_BEFOREPEC.xls"))
                    Response.AppendHeader("Content-Disposition", "attachment; filename=rptTB_BEFOREPEC.xls")
                    Response.TransmitFile(Server.MapPath("~/Reports/rptTB_BEFOREPEC.xls"))
                    Response.End()
                Else
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptTB_BEFOREPEC.pdf"))
                    file = Server.MapPath("~/Reports/rptTB_BEFOREPEC.pdf")
                End If

                crystalReport.Close()
                crystalReport.Dispose()
            Case "TBAFTERPEC"
                Dim query As String
                query = " SELECT * FROM tblPrint_TB"
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptTB_AFTERPEC.rpt"))

                Dim dsTBrpt As dsTBrpt = GetData(Type, query)

                crystalReport.Database.Tables(0).SetDataSource(dsTBrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                crystalReport.SetParameterValue("@DateFrom", Session("@DateFrom"))
                crystalReport.SetParameterValue("@DateTo", Session("@DateTo"))
                crystalReport.SetParameterValue("@User", GetUserFullName(Session("EmailAddress")))
                CrystalReportViewer1.ReportSource = crystalReport

                If Session("@FileType") = "Excel" Then
                    crystalReport.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("~/Reports/rptTB_AFTERPEC.xls"))
                    Response.AppendHeader("Content-Disposition", "attachment; filename=rptTB_AFTERPEC.xls")
                    Response.TransmitFile(Server.MapPath("~/Reports/rptTB_AFTERPEC.xls"))
                    Response.End()
                Else
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptTB_AFTERPEC.pdf"))
                    file = Server.MapPath("~/Reports/rptTB_AFTERPEC.pdf")
                End If

                crystalReport.Close()
                crystalReport.Dispose()
            Case "GENLGRD"
                Dim query As String
                query = " SELECT * FROM View_GL INNER JOIN tblPrint_TB ON View_GL.AccntCode = tblPrint_TB.Code " &
                        " WHERE  AppDate <= @AppDate AND Status <> 'Cancelled' "
                SQL.FlushParams()
                SQL.AddParam("@AppDate", Session("@DateTo"))

                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptGENLGRD.rpt"))

                Dim dsGENLGRrpt As dsGENLGRrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsGENLGRrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                crystalReport.SetParameterValue("@DateFrom", Session("@DateFrom"))
                crystalReport.SetParameterValue("@DateTo", Session("@DateTo"))
                crystalReport.SetParameterValue("@ReportType", Session("@ReportType"))
                crystalReport.SetParameterValue("@User", GetUserFullName(Session("EmailAddress")))
                CrystalReportViewer1.ReportSource = crystalReport

                If Session("@FileType") = "Excel" Then
                    crystalReport.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("~/Reports/rptGENLGRD.xls"))
                    Response.AppendHeader("Content-Disposition", "attachment; filename=rptGENLGRD.xls")
                    Response.TransmitFile(Server.MapPath("~/Reports/rptGENLGRD.xls"))
                    Response.End()
                Else
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptGENLGRD.pdf"))
                    file = Server.MapPath("~/Reports/rptGENLGRD.pdf")
                End If

                crystalReport.Close()
                crystalReport.Dispose()
            Case "GENLGRS"
                Dim query As String
                query = " SELECT * FROM View_GL INNER JOIN tblPrint_TB ON View_GL.AccntCode = tblPrint_TB.Code " &
                        " WHERE  AppDate <= @AppDate AND Status <> 'Cancelled' "
                SQL.FlushParams()
                SQL.AddParam("@AppDate", Session("@DateTo"))

                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptGENLGRS.rpt"))

                Dim dsGENLGRrpt As dsGENLGRrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsGENLGRrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                crystalReport.SetParameterValue("@DateFrom", Session("@DateFrom"))
                crystalReport.SetParameterValue("@DateTo", Session("@DateTo"))
                crystalReport.SetParameterValue("@ReportType", Session("@ReportType"))
                crystalReport.SetParameterValue("@User", GetUserFullName(Session("EmailAddress")))
                CrystalReportViewer1.ReportSource = crystalReport

                If Session("@FileType") = "Excel" Then
                    crystalReport.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("~/Reports/rptGENLGRS.xls"))
                    Response.AppendHeader("Content-Disposition", "attachment; filename=rptGENLGRS.xls")
                    Response.TransmitFile(Server.MapPath("~/Reports/rptGENLGRS.xls"))
                    Response.End()
                Else
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptGENLGRS.pdf"))
                    file = Server.MapPath("~/Reports/rptGENLGRS.pdf")
                End If

                crystalReport.Close()
                crystalReport.Dispose()
            Case "BOASUM"
                Dim query As String
                query = " SELECT * FROM tblPrint_TB "
                SQL.FlushParams()
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptBOASUM.rpt"))

                Dim dsBOASUMrpt As dsBOASUMrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsBOASUMrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                crystalReport.SetParameterValue("@DateFrom", Session("@DateFrom"))
                crystalReport.SetParameterValue("@DateTo", Session("@DateTo"))
                crystalReport.SetParameterValue("@Book", Session("@Book"))
                crystalReport.SetParameterValue("@Header", Session("@Header"))
                crystalReport.SetParameterValue("@User", GetUserFullName(Session("EmailAddress")))
                CrystalReportViewer1.ReportSource = crystalReport

                If Session("@FileType") = "Excel" Then
                    crystalReport.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("~/Reports/rptBOASUM.xls"))
                    Response.AppendHeader("Content-Disposition", "attachment; filename=rptBOASUM.xls")
                    Response.TransmitFile(Server.MapPath("~/Reports/rptBOASUM.xls"))
                    Response.End()
                Else
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptBOASUM.pdf"))
                    file = Server.MapPath("~/Reports/rptBOASUM.pdf")
                End If

                    crystalReport.Close()
                crystalReport.Dispose()
            Case "BOADET"
                Dim query As String
                query = " SELECT * FROM VIEW_GL WHERE Status <> @Status AND Appdate Between @DateFrom AND @DateTo AND Book = @Book AND RefType IN (SELECT RefType FROM tblPrint_BOA)"
                SQL.FlushParams()
                SQL.AddParam("@Status", "Cancelled")
                SQL.AddParam("@DateFrom", Session("@DateFrom"))
                SQL.AddParam("@DateTo", Session("@DateTo"))
                SQL.AddParam("@Book", Session("@Book"))

                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptBOADET.rpt"))

                Dim dsBOADETrpt As dsBOADETrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsBOADETrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                crystalReport.SetParameterValue("@DateFrom", Session("@DateFrom"))
                crystalReport.SetParameterValue("@DateTo", Session("@DateTo"))
                crystalReport.SetParameterValue("@Header", Session("@Header"))
                crystalReport.SetParameterValue("@User", GetUserFullName(Session("EmailAddress")))
                CrystalReportViewer1.ReportSource = crystalReport

                If Session("@FileType") = "Excel" Then
                    crystalReport.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("~/Reports/rptBOADET.xls"))
                    Response.AppendHeader("Content-Disposition", "attachment; filename=rptBOADET.xls")
                    Response.TransmitFile(Server.MapPath("~/Reports/rptBOADET.xls"))
                    Response.End()
                Else
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptBOADET.pdf"))
                    file = Server.MapPath("~/Reports/rptBOADET.pdf")
                End If

                crystalReport.Close()
                crystalReport.Dispose()
            Case "SBLL"
                Dim query As String
                query = " SELECT * FROM VIEW_GL WHERE Status <> @Status AND Appdate Between @DateFrom AND @DateTo AND Book = @Book AND RefType IN (SELECT RefType FROM tblPrint_BOA)"
                SQL.FlushParams()
                SQL.FlushParams()
                SQL.AddParam("@Status", "Cancelled")
                SQL.AddParam("@DateFrom", Session("@DateFrom"))
                SQL.AddParam("@DateTo", Session("@DateTo"))
                SQL.AddParam("@Book", Session("@Book"))

                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptSBLL.rpt"))

                Dim dsBOALLrpt As dsBOALLrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsBOALLrpt.Tables("Table"))
                'crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                crystalReport.SetParameterValue("@DateFrom", Session("@DateFrom"))
                crystalReport.SetParameterValue("@DateTo", Session("@DateTo"))
                'crystalReport.SetParameterValue("@User", GetUserFullName(Session("EmailAddress")))
                CrystalReportViewer1.ReportSource = crystalReport

                If Session("@FileType") = "Excel" Then
                    crystalReport.ExportToDisk(ExportFormatType.ExcelRecord, Server.MapPath("~/Reports/rptSBLL.xls"))
                    Response.AppendHeader("Content-Disposition", "attachment; filename=rptSBLL.xls")
                    Response.TransmitFile(Server.MapPath("~/Reports/rptSBLL.xls"))
                    Response.End()
                Else
                    crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptSBLL.pdf"))
                    file = Server.MapPath("~/Reports/rptSBLL.pdf")
                End If

                crystalReport.Close()
                crystalReport.Dispose()
        End Select

        If file <> "" Then
            Dim web As New WebClient
            Dim fileBuffer As Byte() = web.DownloadData(file)
            If (Not IsNothing(fileBuffer)) Then
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-length", fileBuffer.Length.ToString())
                Response.BinaryWrite(fileBuffer)
            End If
        End If
    End Sub

    Private Function GetData(ByVal Type As String, ByVal Query As String)
        SQL.GetQuery(Query)
        Select Case Type
            Case "HEADER"
                Using dsHEADERrpt As New dsHEADERrpt
                    SQL.SQLDA.Fill(dsHEADERrpt)
                    Return dsHEADERrpt
                End Using
            Case "CHKV"
                Using dsCHKVrpt As New dsCHKVrpt
                    SQL.SQLDA.Fill(dsCHKVrpt)
                    Return dsCHKVrpt
                End Using
            Case "CV"
                Using dsCVrpt As New dsCVrpt
                    SQL.SQLDA.Fill(dsCVrpt)
                    Return dsCVrpt
                End Using
            Case "JV"
                Using dsJVrpt As New dsJVrpt
                    SQL.SQLDA.Fill(dsJVrpt)
                    Return dsJVrpt
                End Using
            Case "APV"
                Using dsAPVrpt As New dsAPVrpt
                    SQL.SQLDA.Fill(dsAPVrpt)
                    Return dsAPVrpt
                End Using
            Case "CA"
                Using dsCArpt As New dsCArpt
                    SQL.SQLDA.Fill(dsCArpt)
                    Return dsCArpt
                End Using
            Case "CASUM"
                Using dsCASUMrpt As New dsCASUMrpt
                    SQL.SQLDA.Fill(dsCASUMrpt)
                    Return dsCASUMrpt
                End Using
            Case "CAUNLQ"
                Using dsCAUNLQrpt As New dsCAUNLQrpt
                    SQL.SQLDA.Fill(dsCAUNLQrpt)
                    Return dsCAUNLQrpt
                End Using
            Case "PC"
                Using dsPCrpt As New dsPCrpt
                    SQL.SQLDA.Fill(dsPCrpt)
                    Return dsPCrpt
                End Using
            Case "TBDETAILED"
                Using dsTBrpt As New dsTBrpt
                    SQL.SQLDA.Fill(dsTBrpt)
                    Return dsTBrpt
                End Using
            Case "TBSUMMARY"
                Using dsTBrpt As New dsTBrpt
                    SQL.SQLDA.Fill(dsTBrpt)
                    Return dsTBrpt
                End Using
            Case "TBAFTERPEC"
                Using dsTBrpt As New dsTBrpt
                    SQL.SQLDA.Fill(dsTBrpt)
                    Return dsTBrpt
                End Using
            Case "TBBEFOREPEC"
                Using dsTBrpt As New dsTBrpt
                    SQL.SQLDA.Fill(dsTBrpt)
                    Return dsTBrpt
                End Using
            Case "GENLGRD"
                Using dsGENLGRrpt As New dsGENLGRrpt
                    SQL.SQLDA.Fill(dsGENLGRrpt)
                    Return dsGENLGRrpt
                End Using
            Case "GENLGRS"
                Using dsGENLGRrpt As New dsGENLGRrpt
                    SQL.SQLDA.Fill(dsGENLGRrpt)
                    Return dsGENLGRrpt
                End Using
            Case "BOASUM"
                Using dsBOASUMrpt As New dsBOASUMrpt
                    SQL.SQLDA.Fill(dsBOASUMrpt)
                    Return dsBOASUMrpt
                End Using
            Case "BOADET"
                Using dsBOADETrpt As New dsBOADETrpt
                    SQL.SQLDA.Fill(dsBOADETrpt)
                    Return dsBOADETrpt
                End Using
            Case "SBLL"
                Using dsBOALLrpt As New dsBOALLrpt
                    SQL.SQLDA.Fill(dsBOALLrpt)
                    Return dsBOALLrpt
                End Using
        End Select
    End Function

End Class