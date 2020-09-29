Public Class ResponsibilityCenter_Loadlist
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
        query = " SELECT   ID, CC_Code , CC_Name, PC_Code, PC_Name, IC_Code, IC_Name " &
                " FROM     tblRespon_Center " &
                " WHERE ID LIKE '%" & txtFilter.Text & "%' OR CC_Name LIKE '%" & txtFilter.Text & "%' ORDER BY ID DESC"
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
        Response.Write("<script>window.location='Responsibility_Center.aspx';</script>")
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadList()
    End Sub
End Class