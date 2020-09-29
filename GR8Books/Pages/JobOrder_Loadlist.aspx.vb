Public Class JobOrder_Loadlist
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
        Dim query As String
        query = " SELECT   TransID, TransNo , REPLACE(CAST(TransDate as DATE),' 12:00:00 AM','') as TransDate, tblJO.VCECode as VCECode, TotalAmount, Remarks, tblJO.Status as Status " &
                " FROM     tblJO LEFT JOIN View_VCEMMaster " &
                " ON	   tblJO.VCECode = View_VCEMMaster.Code   " &
                " WHERE TransNo LIKE '%" & txtFilter.Text & "%' OR Remarks LIKE '%" & txtFilter.Text & "%' ORDER BY TransID DESC"
        SQL.FlushParams()
        SQL.GetQuery(query)
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
        txtFilter.Text = ""
        Session("ID") = TransID
        Response.Write("<script>window.location='JobOrder.aspx';</script>")
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadList()
    End Sub
End Class