Public Class LoadTransaction
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                LoadList()
            End If
        End If
    End Sub

    Public Sub LoadList()
        Dim Type As String = Request.QueryString("ID")
        Dim query As String
        Dim filter As String = txtFilter.Text
        Select Case Type
            Case "SJ"
                query = " SELECT   TransID, SJ_No AS TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, Name, TotalAmount, Remarks, tblSJ.Status  " &
                        " FROM     tblSJ LEFT JOIN View_VCEMMaster " &
                        " ON	   tblSJ.VCECode = View_VCEMMaster.Code   " &
                        " WHERE SJ_No LIKE '%" & filter & "%' OR Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%'  OR tblSJ.Status LIKE '%" & filter & "%' ORDER BY TransID DESC"
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "PJ"
                query = " SELECT   TransID, PJ_No AS TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, Name, TotalAmount, Remarks, tblPJ.Status  " &
                        " FROM     tblPJ LEFT JOIN View_VCEMMaster " &
                        " ON	   tblPJ.VCECode = View_VCEMMaster.Code   " &
                        " WHERE PJ_No LIKE '%" & filter & "%' OR Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%'  OR tblPJ.Status LIKE '%" & filter & "%' ORDER BY TransID DESC"
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "JV"
                query = " SELECT   TransID, JV_No AS TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, '' AS Name, '0.00' AS TotalAmount, Remarks, tblJV.Status  " &
                        " FROM     tblJV  " &
                        " WHERE JV_No LIKE '%" & filter & "%' OR Remarks LIKE '%" & filter & "%' OR tblJV.Status LIKE '%" & filter & "%'  ORDER BY TransID ASC"
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "CV"
                query = " SELECT   TransID, CV_No AS TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, Name, TotalAmount, Remarks, tblDV_Cash.Status  " &
                        " FROM     tblDV_Cash LEFT JOIN View_VCEMMaster " &
                        " ON	   tblDV_Cash.VCECode = View_VCEMMaster.Code   " &
                        " WHERE CV_No LIKE '%" & filter & "%' OR Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%' OR tblDV_Cash.Status LIKE '%" & filter & "%'  ORDER BY TransID DESC"
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "CHKV"
                query = " SELECT   TransID, CV_No AS TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, Name, TotalAmount, Remarks, tblDV_Check.Status  " &
                        " FROM     tblDV_Check LEFT JOIN View_VCEMMaster " &
                        " ON	   tblDV_Check.VCECode = View_VCEMMaster.Code   " &
                        " WHERE CV_No LIKE '%" & filter & "%' OR Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%' OR tblDV_Check.Status LIKE '%" & filter & "%'  ORDER BY TransID DESC"
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "CA"
                query = " SELECT  tblCA.TransID, CA_No AS TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, Name, Amount AS TotalAmount, Remarks,  " & vbCrLf &
                        " CASE WHEN View_CA_Balance.TransID IS NOT NULL THEN 'Active'  " & vbCrLf &
                        " 	      WHEN tblCA.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	    ELSE tblCA.Status END AS Status  " & vbCrLf &
                        " FROM     tblCA LEFT JOIN View_VCEMMaster " & vbCrLf &
                        " ON	   tblCA.VCECode = View_VCEMMaster.Code   " & vbCrLf &
                        " LEFT JOIN (SELECT TransID FROM View_CA_Balance) AS View_CA_Balance  " & vbCrLf &
                        " ON	   tblCA.TransID = View_CA_Balance.TransID   " & vbCrLf &
                        " WHERE CA_No LIKE '%" & filter & "%' OR Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%' OR " & vbCrLf &
                        " CASE WHEN View_CA_Balance.TransID IS NOT NULL THEN 'Active'  " & vbCrLf &
                        " 	      WHEN tblCA.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	    ELSE tblCA.Status END LIKE '%" & filter & "%'   " & vbCrLf &
                        " ORDER BY tblCA.TransID DESC"
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "OR"
                query = " SELECT   TransID, OR_No AS TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, Name, Amount AS TotalAmount,  Remarks, tblCollection_OR.Status  " &
                        " FROM     tblCollection_OR LEFT JOIN View_VCEMMaster " &
                        " ON	   tblCollection_OR.VCECode = View_VCEMMaster.Code   " &
                        " WHERE OR_No LIKE '%" & filter & "%' OR Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%' OR tblCollection_OR.Status LIKE '%" & filter & "%'  ORDER BY TransID DESC"
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "CR"
                query = " SELECT   TransID, CR_No AS TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, Name, Amount AS TotalAmount, Remarks, tblCollection_CR.Status  " &
                        " FROM     tblCollection_CR LEFT JOIN View_VCEMMaster " &
                        " ON	   tblCollection_CR.VCECode = View_VCEMMaster.Code   " &
                        " WHERE CR_No LIKE '%" & filter & "%' OR Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%' OR tblCollection_CR.Status LIKE '%" & filter & "%'  ORDER BY TransID DESC"
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "PR"
                query = " SELECT   TransID, PR_No AS TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, Name, Amount AS TotalAmount, Remarks, tblCollection_PR.Status  " &
                        " FROM     tblCollection_PR LEFT JOIN View_VCEMMaster " &
                        " ON	   tblCollection_PR.VCECode = View_VCEMMaster.Code   " &
                        " WHERE PR_No LIKE '%" & filter & "%' OR Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%' OR tblCollection_PR.Status LIKE '%" & filter & "%'  ORDER BY TransID DESC"
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "AR"
                query = " SELECT   TransID, AR_No AS TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, Name, Amount AS TotalAmount, Remarks, tblCollection_AR.Status  " &
                        " FROM     tblCollection_AR LEFT JOIN View_VCEMMaster " &
                        " ON	   tblCollection_AR.VCECode = View_VCEMMaster.Code   " &
                        " WHERE AR_No LIKE '%" & filter & "%' OR Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%' OR tblCollection_AR.Status LIKE '%" & filter & "%'  ORDER BY TransID DESC"
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "APV"
                query = " SELECT   tblAPV.TransID, APV_No AS TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, Name, Amount AS TotalAmount, Remarks," & vbCrLf &
                        " CASE WHEN View_APV_Balance.TransID IS NOT NULL THEN 'Active'  " & vbCrLf &
                        " 	      WHEN tblAPV.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	    ELSE tblAPV.Status END AS Status  " & vbCrLf &
                        " FROM     tblAPV LEFT JOIN View_VCEMMaster " & vbCrLf &
                        " ON	   tblAPV.VCECode = View_VCEMMaster.Code   " & vbCrLf &
                        " LEFT JOIN (SELECT TransID FROM View_APV_Balance) AS View_APV_Balance  " & vbCrLf &
                        " ON	   tblAPV.TransID = View_APV_Balance.TransID   " & vbCrLf &
                        " WHERE APV_No LIKE '%" & filter & "%' OR Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%' OR " & vbCrLf &
                        " CASE WHEN View_APV_Balance.TransID IS NOT NULL THEN 'Active'  " & vbCrLf &
                        " 	      WHEN tblAPV.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	    ELSE tblAPV.Status END LIKE '%" & filter & "%'   " & vbCrLf &
                        " ORDER BY tblAPV.TransID DESC"
                SQL.FlushParams()
                SQL.GetQuery(query)
        End Select
        dgvList.DataSource = SQL.SQLDS
        dgvList.DataBind()
    End Sub

    Protected Sub GridView_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes("onclick") = Page.ClientScript.GetPostBackClientHyperlink(dgvList, "Select$" & e.Row.RowIndex)
            e.Row.Attributes("onmouseover") = "this.style.backgroundColor='gray';"
                e.Row.Attributes("onmouseout") = "this.style.backgroundColor='white';"
        End If
    End Sub

    Protected Sub OnSelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim TransID As String = dgvList.SelectedRow.Cells(0).Text
        Dim url As String = Request.QueryString("Url")
        txtFilter.Text = ""
        Session("ID") = TransID
        Response.Write("<script>window.opener.location = '" & url & "';window.close();</script>")
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadList()
        txtFilter.Focus()
    End Sub
End Class