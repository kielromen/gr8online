Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Public Class CashDisbursment_Entry
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Dim dt As New DataTable
            dt.Columns.Add(New DataColumn("chNo"))
            dt.Columns.Add(New DataColumn("AccntCode"))
            dt.Columns.Add(New DataColumn("AccntTitle"))
            dt.Columns.Add(New DataColumn("Particulars"))
            dt.Columns.Add(New DataColumn("Debit"))
            dt.Columns.Add(New DataColumn("Credit"))
            dt.Columns.Add(New DataColumn("Code"))
            dt.Columns.Add(New DataColumn("Name"))
            dt.Columns.Add(New DataColumn("RefID"))
            Dim dr As DataRow = dt.NewRow
            dr("chNo") = 1
            dr("AccntCode") = Nothing
            dr("AccntTitle") = Nothing
            dr("Particulars") = Nothing
            dr("Debit") = "0.00"
            dr("Credit") = "0.00"
            dr("Code") = Nothing
            dr("Name") = Nothing
            dr("RefID") = Nothing
            dt.Rows.Add(dr)

            ViewState("EntryTable") = dt

            dgvEntry.DataSource = dt
            dgvEntry.DataBind()
            TotalDBCR()
        End If
    End Sub

    Protected Sub AddNewRow(sender As Object, e As EventArgs)
        Dim rowIndex As Integer = 0
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            Dim dr As DataRow = Nothing
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtParticulars_Entry")
                    Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtDebit_Entry")
                    Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtCredit_Entry")
                    Dim txtCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
                    Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
                    Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtRefID_Entry")
                    dr = dt.NewRow
                    dr("chNo") = i + 2
                    dr("Debit") = "0.00"
                    dr("Credit") = "0.00"

                    dt.Rows(i)("AccntCode") = txtAccntCode.Text
                    dt.Rows(i)("AccntTitle") = txtAccntTitle.Text
                    dt.Rows(i)("Particulars") = txtParticulars.Text
                    dt.Rows(i)("Debit") = txtDebit.Text
                    dt.Rows(i)("Credit") = txtCredit.Text
                    dt.Rows(i)("Code") = txtCode.Text
                    dt.Rows(i)("Name") = txtName.Text
                    dt.Rows(i)("RefID") = txtRefID.Text


                    rowIndex = i
                Next
                dt.Rows.Add(dr)
                ViewState("EntryTable") = dt

                dgvEntry.DataSource = dt
                dgvEntry.DataBind()

            End If
            SetDataTable()
        End If
    End Sub

    Private Sub SetDataTable()
        Dim rowIndex As Integer = 0
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtParticulars_Entry")
                    Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtDebit_Entry")
                    Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtCredit_Entry")
                    Dim txtCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
                    Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
                    Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtRefID_Entry")

                    dgvEntry.Rows(i).Cells(1).Text = i + 1
                    txtAccntCode.Text = dt.Rows(i)("AccntCode").ToString
                    txtAccntTitle.Text = dt.Rows(i)("AccntTitle").ToString
                    txtParticulars.Text = dt.Rows(i)("Particulars").ToString
                    txtDebit.Text = dt.Rows(i)("Debit").ToString
                    txtCredit.Text = dt.Rows(i)("Credit").ToString
                    txtCode.Text = dt.Rows(i)("Code").ToString
                    txtName.Text = dt.Rows(i)("Name").ToString
                    txtRefID.Text = dt.Rows(i)("RefID").ToString

                   
                Next
                TotalDBCR()
            End If
        End If
    End Sub




    Private Sub dgvEntry_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles dgvEntry.RowDeleting
        Dim rowIndex As Integer = e.RowIndex
        If Not IsNothing(ViewState("EntryTable")) Then
            Dim dt As DataTable = ViewState("EntryTable")
            If dt.Rows.Count > 0 Then
                For i As Integer = 0 To dt.Rows.Count - 1
                    Dim txtAccntCode As TextBox = dgvEntry.Rows(i).Cells(2).FindControl("txtAccntCode_Entry")
                    Dim txtAccntTitle As TextBox = dgvEntry.Rows(i).Cells(3).FindControl("txtAccntTitle_Entry")
                    Dim txtParticulars As TextBox = dgvEntry.Rows(i).Cells(4).FindControl("txtParticulars_Entry")
                    Dim txtDebit As TextBox = dgvEntry.Rows(i).Cells(5).FindControl("txtDebit_Entry")
                    Dim txtCredit As TextBox = dgvEntry.Rows(i).Cells(6).FindControl("txtCredit_Entry")
                    Dim txtCode As TextBox = dgvEntry.Rows(i).Cells(7).FindControl("txtCode_Entry")
                    Dim txtName As TextBox = dgvEntry.Rows(i).Cells(8).FindControl("txtName_Entry")
                    Dim txtRefID As TextBox = dgvEntry.Rows(i).Cells(9).FindControl("txtRefID_Entry")

                    dt.Rows(i)("AccntCode") = txtAccntCode.Text
                    dt.Rows(i)("AccntTitle") = txtAccntTitle.Text
                    dt.Rows(i)("Particulars") = txtParticulars.Text
                    dt.Rows(i)("Debit") = txtDebit.Text
                    dt.Rows(i)("Credit") = txtCredit.Text
                    dt.Rows(i)("Code") = txtCode.Text
                    dt.Rows(i)("Name") = txtName.Text
                    dt.Rows(i)("RefID") = txtRefID.Text

                Next

                dt.Rows.RemoveAt(rowIndex)
                ViewState("EntryTable") = dt

                dgvEntry.DataSource = dt
                dgvEntry.DataBind()

            End If
            SetDataTable()
        End If
    End Sub

    <WebMethod()>
    Public Shared Function ListAccountTitle(prefix As String) As String()
        Dim AccountTitle As New List(Of String)()
        Dim query As String
        query = "SELECT AccountTitle, AccountCode FROM tblCOA " & vbCrLf &
                "WHERE Class = 'Posting' AND AccountTitle LIKE '%' + @AccountTitle + '%'"
        SQL.FlushParams()
        SQL.AddParam("@AccountTitle", prefix)
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            AccountTitle.Add(String.Format("{0}--{1}", SQL.SQLDR("AccountTitle"), SQL.SQLDR("AccountCode")))
        End While
        Return AccountTitle.ToArray()
    End Function

    <WebMethod()>
    Public Shared Function ListVCE(prefix As String) As String()
        Dim strName As New List(Of String)()
        Dim query As String
        query = "SELECT Employee_Code, Name FROM ViewtblEmployee_Master " & vbCrLf &
                "WHERE Status = 'Active' AND Name LIKE '%' + @Name + '%'"
        SQL.FlushParams()
        SQL.AddParam("@Name", prefix)
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            strName.Add(String.Format("{0}--{1}", SQL.SQLDR("Name"), SQL.SQLDR("Employee_Code")))
        End While
        Return strName.ToArray()
    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

    End Sub

    Protected Sub TotalDBCR()
        Try
            'debit compute & print in textbox
            Dim debitamount As Double = "0.00"
            Dim creditamount As Double = "0.00"
            For i As Integer = 0 To dgvEntry.Rows.Count - 1
                Dim txtTotalDebit_Amount As TextBox = DirectCast(dgvEntry.Rows(i).FindControl("txtDebit_Entry"), TextBox)
                Dim txtTotalCredit_Amount As TextBox = DirectCast(dgvEntry.Rows(i).FindControl("txtCredit_Entry"), TextBox)

                debitamount = debitamount + Double.Parse(txtTotalDebit_Amount.Text)
                dgvEntry.FooterRow.Cells(5).Text = debitamount.ToString("N2")
                dgvEntry.FooterRow.Cells(5).Font.Bold = True

                creditamount = creditamount + Double.Parse(txtTotalCredit_Amount.Text)
                dgvEntry.FooterRow.Cells(6).Text = creditamount.ToString("N2")
                dgvEntry.FooterRow.Cells(6).Font.Bold = True
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    
    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click

        ClearText()
        ' Toolstrip Buttons
        btnSearch.Enabled = False
        btnNew.Enabled = False
        btnEdit.Enabled = False
        btnSave.Enabled = True
        btnCancel.Enabled = False
        'tsbDelete.Enabled = False
        'tsbClose.Enabled = True
        'tsbPrevious.Enabled = False
        'tsbNext.Enabled = False
        'tsbExit.Enabled = False
        'tsbPrint.Enabled = False
        'tsbCopy.Enabled = True
        'tsbOption.Enabled = False
        'txtStatus.Text = "Open"


    End Sub

    Private Sub ClearText()
        txtCode.Text = ""
        txtName.Text = ""
        'txtAmount.Text = ""
        'txtRef_No.Text = ""
        'ddlType.SelectedIndex = -1
       
        dgvEntry.DataSource = Nothing
        dgvEntry.DataBind()

        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("chNo"))
        dt.Columns.Add(New DataColumn("AccntCode"))
        dt.Columns.Add(New DataColumn("AccntTitle"))
        dt.Columns.Add(New DataColumn("Particulars"))
        dt.Columns.Add(New DataColumn("Debit"))
        dt.Columns.Add(New DataColumn("Credit"))
        dt.Columns.Add(New DataColumn("Code"))
        dt.Columns.Add(New DataColumn("Name"))
        dt.Columns.Add(New DataColumn("RefID"))
        Dim dr As DataRow = dt.NewRow
        dr("chNo") = 1
        dr("AccntCode") = Nothing
        dr("AccntTitle") = Nothing
        dr("Particulars") = Nothing
        dr("Debit") = "0.00"
        dr("Credit") = "0.00"
        dr("Code") = Nothing
        dr("Name") = Nothing
        dr("RefID") = Nothing
        dt.Rows.Add(dr)

        ViewState("EntryTable") = dt

        dgvEntry.DataSource = dt
        dgvEntry.DataBind()
        TotalDBCR()

    End Sub
End Class