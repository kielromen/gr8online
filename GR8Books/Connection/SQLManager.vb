Imports System.Data.SqlClient

Public Class SQLManager
    ReadOnly SQLCon As SqlConnection
    ReadOnly SQLCon1 As SqlConnection
    ReadOnly SQLCon2 As SqlConnection

    Protected Property Transaction As SqlTransaction

    Public SQLDA As SqlDataAdapter
    Public SQLDS As DataSet
    Public SQLDT As DataTable

    Public SQLDR As SqlDataReader
    Public SQLDR1 As SqlDataReader
    Public SQLDR2 As SqlDataReader

    Public RecordCount As Integer
    Public SQLParams As New List(Of SqlParameter)

    Public Property Server As String = "122.52.201.210"
    'Public Property Server As String = "(local)"
    Public Property Database As String = "GR8Books"
    Public Property UserName As String = "sa"
    Public Property Password As String = "@dm1nEvo"
    'Public Property Password As String = "eVoSolution1"

    Public Function GetSQLConnectionString() As String
        Return "Server=" & Server & " ;Database=" & Database & " ;integrated security=sspi;Uid=" & UserName & " ;Pwd=" & Password & " ;Trusted_Connection=no;MultipleActiveResultSets=True;Max Pool Size=200;"
    End Function

    Public Sub New()
        SQLCon = New SqlConnection(GetSQLConnectionString)
        SQLCon1 = New SqlConnection(GetSQLConnectionString)
        SQLCon2 = New SqlConnection(GetSQLConnectionString)
    End Sub

    Public Sub BeginTransaction()
        SQLCon.Open()
        Transaction = SQLCon.BeginTransaction
    End Sub

    Public Sub Commit()
        Transaction.Commit()
        Transaction = Nothing
    End Sub
    Public Sub Rollback()
        Transaction.Rollback()
        Transaction = Nothing
    End Sub

    Public Sub GetDataTable(ByVal CommandText As String, Optional ByVal Server As Integer = 0, Optional ByVal With_Param As Boolean = False)
        Try

            If SQLCon.State = ConnectionState.Open Then
                SQLCon.Close()
            End If
            Using SQLCmd = New SqlCommand(CommandText, SQLCon)
                SQLCon.Open()
                For Each p As SqlParameter In SQLParams
                    SQLCmd.Parameters.Add(p)
                    SQLCmd.Parameters(p.ParameterName).Value = p.Value
                Next
                SQLDA = New SqlDataAdapter(SQLCmd)
                SQLDT = New DataTable
                RecordCount = SQLDA.Fill(SQLDT)
                If With_Param Then
                    FlushParams()
                End If
            End Using
        Catch ex As Exception

        Finally
            CloseCon(Server)
            FlushParams()
        End Try
    End Sub

    Public Sub GetQuery(ByVal CommandText As String, Optional ByVal Server As Integer = 0, Optional ByVal With_Param As Boolean = False)
        Try

            If SQLCon.State = ConnectionState.Open Then
                SQLCon.Close()
            End If
            Using SQLCmd = New SqlCommand(CommandText, SQLCon)
                SQLCon.Open()
                For Each p As SqlParameter In SQLParams
                    SQLCmd.Parameters.Add(p)
                    SQLCmd.Parameters(p.ParameterName).Value = p.Value
                Next
                SQLDA = New SqlDataAdapter(SQLCmd)
                SQLDS = New DataSet
                RecordCount = SQLDA.Fill(SQLDS)
                If With_Param Then
                    FlushParams()
                End If
            End Using
        Catch ex As Exception

        Finally
            CloseCon(Server)
            FlushParams()
        End Try
    End Sub

    Public Sub ReadQuery(ByVal CommandText As String, Optional ByVal Connection As Integer = 0)
        Try
            If Connection = 0 Then
                If SQLCon.State = ConnectionState.Open Then
                    SQLCon.Close()
                End If
                Using SQLCmd = New SqlCommand(CommandText, SQLCon)
                    SQLCmd.CommandType = GetCommandType(CommandText)
                    SQLCon.Open()
                    For Each p As SqlParameter In SQLParams
                        SQLCmd.Parameters.Add(p)
                        SQLCmd.Parameters(p.ParameterName).Value = p.Value
                    Next
                    SQLDR = SQLCmd.ExecuteReader
                End Using
            ElseIf Connection = 1 Then
                If SQLCon1.State = ConnectionState.Open Then
                    SQLCon1.Close()
                End If
                Using SQLCmd = New SqlCommand(CommandText, SQLCon1)
                    SQLCmd.CommandType = GetCommandType(CommandText)
                    SQLCon1.Open()
                    For Each p As SqlParameter In SQLParams
                        SQLCmd.Parameters.Add(p)
                        SQLCmd.Parameters(p.ParameterName).Value = p.Value
                    Next
                    SQLDR1 = SQLCmd.ExecuteReader
                End Using
            ElseIf Connection = 2 Then
                If SQLCon2.State = ConnectionState.Open Then
                    SQLCon2.Close()
                End If
                Using SQLCmd = New SqlCommand(CommandText, SQLCon2)
                    SQLCmd.CommandType = GetCommandType(CommandText)
                    SQLCon2.Open()
                    For Each p As SqlParameter In SQLParams
                        SQLCmd.Parameters.Add(p)
                        SQLCmd.Parameters(p.ParameterName).Value = p.Value
                    Next
                    SQLDR2 = SQLCmd.ExecuteReader
                End Using
            End If
        Catch ex As Exception
            WriteLine(ex.Message)
        Finally
            FlushParams()
        End Try
    End Sub

    Public Function ExecNonQuery(ByVal CommandText As String) As Integer
        Try
            Dim RecordChanged As Integer
            Using SQLCmd = New SqlCommand(CommandText, SQLCon1)
                If SQLCon1.State = 1 Then
                    SQLCon1.Close()
                End If
                SQLCon1.Open()
                For Each p As SqlParameter In SQLParams
                    SQLCmd.Parameters.Add(p)
                    SQLCmd.Parameters(p.ParameterName).Value = p.Value
                Next
                RecordChanged = SQLCmd.ExecuteNonQuery()
            End Using
            Return RecordChanged
        Catch ex As Exception
            WriteLine(ex.Message)
            Return -1
        Finally
            SQLCon1.Close()
            SQLParams.Clear
            FlushParams()
        End Try
    End Function

    Private Function GetCommandType(ByVal CommandText As String) As CommandType
        Dim Command As String = CommandText.Trim
        If Left(Command, 6) = "SELECT" Then
            Return CommandType.Text
        Else
            Return CommandType.StoredProcedure
        End If
    End Function

    Public Sub AddParam(ByVal Name As String, ByVal Value As Object, Optional ByVal DataType As SqlDbType = SqlDbType.NVarChar)
        Dim newParam As New SqlParameter With {.ParameterName = Name, .Value = Value, .SqlDbType = DataType}
        SQLParams.Add(newParam)
    End Sub

    Public Sub FlushParams()
        SQLParams.Clear()
    End Sub

    Public Sub CloseCon(Optional ByVal Server As Integer = 0)
        If Server = 0 Then
            If SQLCon.State = ConnectionState.Open Then
                SQLCon.Close()
            End If
            If SQLCon1.State = ConnectionState.Open Then
                SQLCon1.Close()
            End If
            If SQLCon2.State = ConnectionState.Open Then
                SQLCon2.Close()
            End If
        End If
    End Sub
End Class
