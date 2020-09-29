Public Class Employee_Loadlist
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            If Session("SessionExists") = False Then
                Response.Redirect("Login.aspx")
            Else
                Loadlist()
            End If
        End If
    End Sub

    Public Sub Loadlist()
        Dim query As String
        query = " SELECT Employee_Code, CONCAT(ISNULL(Last_Name, ''), ', ', ISNULL(First_Name, ''), ' ', ISNULL(Middle_Name, ''), ' ', ISNULL(Suffix_Name, '')) AS Employee_Name, CellphoneNo, Status FROM tblEmployee_Master " & vbCrLf &
                " WHERE Status = @Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.GetQuery(query)
        gvEmployee.DataSource = SQL.SQLDS
        gvEmployee.DataBind()
    End Sub

    Private Sub gvEmployee_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gvEmployee.PageIndexChanging
        gvEmployee.PageIndex = e.NewPageIndex
        Loadlist()
    End Sub

    Private Sub gvEmployee_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvEmployee.RowCommand
        If (e.CommandName = "btnInactive") Then
            Dim query As String
            query = "UPDATE tblEmployee_Master SET Status = @Status WHERE Employee_Code = @Employee_Code"
            SQL.FlushParams()
            SQL.AddParam("@Employee_Code", e.CommandArgument)
            SQL.AddParam("@Status", "Inactive")
            SQL.ExecNonQuery(query)
            Response.Write("<script>alert('Removed successfully');window.location='Employee_Loadlist.aspx';</script>")
        End If
    End Sub
End Class