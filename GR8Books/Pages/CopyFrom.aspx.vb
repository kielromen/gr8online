Public Class CopyFrom
    Inherits System.Web.UI.Page
    Public RefID As New Dictionary(Of Integer, Integer)
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
            Case "APV"
                query = "  SELECT   tblAPV.TransID, tblAPV.APV_No AS TransNo, REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, " & vbCrLf &
                        " 		    Name, tblAPV.Remarks, '' AS Reference, SUM(ISNULL(View_APV_Balance.Amount,0)) AS TotalAmount,  " & vbCrLf &
                        "           CASE WHEN View_APV_Balance.TransID IS NOT NULL THEN  'Active'    " & vbCrLf &
                        "                WHEN tblAPV.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 		    ELSE tblAPV.Status END AS Status  " & vbCrLf &
                        "  FROM tblAPV LEFT JOIN   " & vbCrLf &
                        "  View_VCEMMaster ON tblAPV .VCECode = View_VCEMMaster.Code   " & vbCrLf &
                        "  LEFT JOIN   " & vbCrLf &
                        "  View_APV_Balance ON tblAPV.TransID = View_APV_Balance.TransID   " & vbCrLf &
                        "  WHERE (tblAPV.APV_No LIKE '%" & filter & "%' OR tblAPV.Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%') " & vbCrLf &
                        "  AND CASE WHEN View_APV_Balance.TransID IS NOT NULL THEN  'Active'  " & vbCrLf &
                        " 	      WHEN tblAPV.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	 ELSE tblAPV.Status END = 'Active'  " & vbCrLf &
                        "  GROUP BY tblAPV.TransID, tblAPV.APV_No, TransDate, Name, tblAPV.Remarks,  " & vbCrLf &
                        " 	   CASE WHEN View_APV_Balance.TransID IS NOT NULL THEN  'Active'  " & vbCrLf &
                        " 	        WHEN tblAPV.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	   ELSE tblAPV.Status END  " & vbCrLf &
                        "  ORDER BY TransID DESC "
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "CA"
                query = "  SELECT   tblCA.TransID, tblCA.CA_No AS TransNo, REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, " & vbCrLf &
                        " 		    Name, tblCA.Remarks, '' AS Reference, SUM(ISNULL(View_CA_Balance.Amount,0)) AS TotalAmount,  " & vbCrLf &
                        "           CASE WHEN View_CA_Balance.TransID IS NOT NULL THEN  'Active'    " & vbCrLf &
                        "                WHEN tblCA.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 		    ELSE tblCA.Status END AS Status  " & vbCrLf &
                        "  FROM tblCA LEFT JOIN   " & vbCrLf &
                        "  View_VCEMMaster ON tblCA .VCECode = View_VCEMMaster.Code   " & vbCrLf &
                        "  LEFT JOIN   " & vbCrLf &
                        "  View_CA_Balance ON tblCA.TransID = View_CA_Balance.TransID   " & vbCrLf &
                        "  WHERE (tblCA.CA_No LIKE '%" & filter & "%' OR tblCA.Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%') " & vbCrLf &
                        "  AND CASE WHEN View_CA_Balance.TransID IS NOT NULL THEN  'Active'  " & vbCrLf &
                        " 	      WHEN tblCA.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	 ELSE tblCA.Status END = 'Active'  " & vbCrLf &
                        "  GROUP BY tblCA.TransID, tblCA.CA_No, TransDate, Name, tblCA.Remarks,  " & vbCrLf &
                        " 	   CASE WHEN View_CA_Balance.TransID IS NOT NULL THEN  'Active'  " & vbCrLf &
                        " 	        WHEN tblCA.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	   ELSE tblCA.Status END  " & vbCrLf &
                        "  ORDER BY TransID DESC "
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "PC"
                query = "  SELECT   tblPC.TransID, tblPC.PC_No AS TransNo, REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, " & vbCrLf &
                        " 		    Name, tblPC.Remarks, '' AS Reference, SUM(ISNULL(View_PC_Balance.Amount,0)) AS TotalAmount,  " & vbCrLf &
                        "           CASE WHEN View_PC_Balance.TransID IS NOT NULL THEN  'Active'    " & vbCrLf &
                        "                WHEN tblPC.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 		    ELSE tblPC.Status END AS Status  " & vbCrLf &
                        "  FROM tblPC LEFT JOIN   " & vbCrLf &
                        "  View_VCEMMaster ON tblPC .VCECode = View_VCEMMaster.Code   " & vbCrLf &
                        "  LEFT JOIN   " & vbCrLf &
                        "  View_PC_Balance ON tblPC.TransID = View_PC_Balance.TransID   " & vbCrLf &
                        "  WHERE (tblPC.PC_No LIKE '%" & filter & "%' OR tblPC.Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%') " & vbCrLf &
                        "  AND CASE WHEN View_PC_Balance.TransID IS NOT NULL THEN  'Active'  " & vbCrLf &
                        " 	      WHEN tblPC.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	 ELSE tblPC.Status END = 'Active'  " & vbCrLf &
                        "  GROUP BY tblPC.TransID, tblPC.PC_No, TransDate, Name, tblPC.Remarks,  " & vbCrLf &
                        " 	   CASE WHEN View_PC_Balance.TransID IS NOT NULL THEN  'Active'  " & vbCrLf &
                        " 	        WHEN tblPC.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	   ELSE tblPC.Status END  " & vbCrLf &
                        "  ORDER BY TransID DESC "
                SQL.FlushParams()
                SQL.GetQuery(query)
            Case "CASHR"
                query = "  SELECT   tblCA.TransID, tblCA.CA_No AS TransNo, REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, " & vbCrLf &
                        " 		    Name, tblCA.Remarks, '' AS Reference, SUM(ISNULL(View_CA_Return.Amount,0)) AS TotalAmount,  " & vbCrLf &
                        "           CASE WHEN View_CA_Return.TransID IS NOT NULL THEN  'Active'    " & vbCrLf &
                        "                WHEN tblCA.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 		    ELSE tblCA.Status END AS Status  " & vbCrLf &
                        "  FROM tblCA LEFT JOIN   " & vbCrLf &
                        "  View_VCEMMaster ON tblCA .VCECode = View_VCEMMaster.Code   " & vbCrLf &
                        "  LEFT JOIN   " & vbCrLf &
                        "  View_CA_Return ON tblCA.TransID = View_CA_Return.TransID   " & vbCrLf &
                        "  WHERE (tblCA.CA_No LIKE '%" & filter & "%' OR tblCA.Remarks LIKE '%" & filter & "%' OR Name LIKE '%" & filter & "%') " & vbCrLf &
                        "  AND CASE WHEN View_CA_Return.TransID IS NOT NULL THEN  'Active'  " & vbCrLf &
                        " 	      WHEN tblCA.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	 ELSE tblCA.Status END = 'Active'  " & vbCrLf &
                        "  GROUP BY tblCA.TransID, tblCA.CA_No, TransDate, Name, tblCA.Remarks,  " & vbCrLf &
                        " 	   CASE WHEN View_CA_Return.TransID IS NOT NULL THEN  'Active'  " & vbCrLf &
                        " 	        WHEN tblCA.Status ='Active' THEN 'Closed'  " & vbCrLf &
                        " 	   ELSE tblCA.Status END  " & vbCrLf &
                        "  ORDER BY TransID DESC "
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


    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadList()
        txtFilter.Focus()
    End Sub

    Private Sub bntChoose_Click(sender As Object, e As EventArgs) Handles bntChoose.Click
        For Each row As GridViewRow In dgvList.Rows
            If TryCast(row.FindControl("chkCheck"), CheckBox).Checked Then
                Dim id As Integer = Integer.Parse(row.Cells(1).Text)
                Session("CopyID") = id.ToString
                RefID.Add(id, id)
            End If
        Next
        Dim Type As String = Request.QueryString("ID")
        Dim url As String = Request.QueryString("Url")
        txtFilter.Text = ""
        Session("Type") = Type
        Session("CopyFromID") = RefID
        Response.Write("<script>window.opener.location = '" & url & "';window.close();</script>")
    End Sub
End Class