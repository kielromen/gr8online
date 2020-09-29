﻿Public Class PurchaseOrder_Loadlist
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
        query = " SELECT   TransID, PO_NO , REPLACE(CAST(DatePO as DATE),' 12:00:00 AM','') as DatePO, VCECode, GrossAmount, Status " &
                " FROM     tblPO " &
                " WHERE PO_NO LIKE '%" & txtFilter.Text & "%' ORDER BY TransID DESC"
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
        Response.Write("<script>window.location='PurchaseOrder.aspx';</script>")
    End Sub
    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadList()
    End Sub
End Class