
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
                query = " SELECT * FROM View_CHKV_Printout WHERE TransID = '" & Session("TransID") & "' " & vbCrLf &
                        " SELECT * FROM View_GL WHERE RefTransID = '" & Session("TransID") & "' AND RefType = 'CHKV' "
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptCHKV.rpt"))

                Dim dsCHKVrpt As dsCHKVrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsCHKVrpt.Tables("Table"))
                crystalReport.Database.Tables(1).SetDataSource(dsCHKVrpt.Tables("Table1"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptCHKV.pdf"))
                file = Server.MapPath("~/Reports/rptCHKV.pdf")
            Case "CV"
                Dim query As String
                query = " SELECT * FROM View_CV_Printout WHERE TransID = '" & Session("TransID") & "' " & vbCrLf &
                        " SELECT * FROM View_GL WHERE RefTransID = '" & Session("TransID") & "' AND RefType = 'CV' "
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptCV.rpt"))

                Dim dsCVrpt As dsCVrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsCVrpt.Tables("Table"))
                crystalReport.Database.Tables(1).SetDataSource(dsCVrpt.Tables("Table1"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptCV.pdf"))
                file = Server.MapPath("~/Reports/rptCV.pdf")

            Case "JV"
                Dim query As String
                query = " SELECT * FROM tblJV WHERE TransID = '" & Session("TransID") & "' " & vbCrLf &
                        " SELECT * FROM View_GL WHERE RefTransID = '" & Session("TransID") & "' AND RefType = 'JV' "
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptJV.rpt"))

                Dim dsJVrpt As dsJVrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsJVrpt.Tables("Table"))
                crystalReport.Database.Tables(1).SetDataSource(dsJVrpt.Tables("Table1"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptJV.pdf"))
                file = Server.MapPath("~/Reports/rptJV.pdf")

            Case "APV"
                Dim query As String
                query = " SELECT * FROM View_APV_Printout WHERE TransID = '" & Session("TransID") & "' " & vbCrLf &
                        " SELECT * FROM View_GL WHERE RefTransID = '" & Session("TransID") & "' AND RefType = 'APV' "
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptAPV.rpt"))

                Dim dsAPVrpt As dsAPVrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsAPVrpt.Tables("Table"))
                crystalReport.Database.Tables(1).SetDataSource(dsAPVrpt.Tables("Table1"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptAPV.pdf"))
                file = Server.MapPath("~/Reports/rptAPV.pdf")
            Case "CA"
                Dim query As String
                query = " SELECT * FROM View_CA_Printout WHERE TransID = '" & Session("TransID") & "' "
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptCA.rpt"))

                Dim dsCArpt As dsCArpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsCArpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptCA.pdf"))
                file = Server.MapPath("~/Reports/rptCA.pdf")
            Case "PC"
                Dim query As String
                query = " SELECT * FROM View_PC_Printout WHERE TransID = '" & Session("TransID") & "' "
                Dim crystalReport As New ReportDocument()
                crystalReport.Load(Server.MapPath("~/Reports/rptPC.rpt"))

                Dim dsPCrpt As dsPCrpt = GetData(Type, query)
                crystalReport.Database.Tables(0).SetDataSource(dsPCrpt.Tables("Table"))
                crystalReport.Subreports(0).SetDataSource(dsHEADERrpt.Tables("Table"))
                CrystalReportViewer1.ReportSource = crystalReport
                crystalReport.ExportToDisk(ExportFormatType.PortableDocFormat, Server.MapPath("~/Reports/rptPC.pdf"))
                file = Server.MapPath("~/Reports/rptPC.pdf")
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
        SQL.FlushParams()
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
            Case "PC"
                Using dsPCrpt As New dsPCrpt
                    SQL.SQLDA.Fill(dsPCrpt)
                    Return dsPCrpt
                End Using
        End Select
    End Function

End Class