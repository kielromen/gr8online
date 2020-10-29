
Imports System.Net.Mail
Module ModuleGen
    Public UserID, UserName, Password, Name, UserLevel, database As String
    Public SQL As New SQLManager
    Public activityStatus As Boolean
    Public server As String
    Public Default_Email As String
    Public Subject, BodyHeader, BodyFooter As String

    Public Sub SetDatabase()
        SQL = New SQLManager
        activityStatus = True
    End Sub

    Public Function GenerateID(ByVal DbTable As String, Optional Database As String = "Main.") As String
        Dim query As String
        query = " SELECT ISNULL(MAX( " & GetPkColumn(DbTable, Database) & "),0) + 1 AS ID FROM  " & Database & "dbo." & DbTable & " "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then

        End If
        Return SQL.SQLDR("ID").ToString
    End Function

    Private Function GetPkColumn(Table As String, Optional Database As String = "Main") As String
        Dim query As String
        query = "SELECT COLUMN_NAME FROM " & Database & "INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE WHERE TABLE_NAME ='" & Table & "' "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
        End If
        Return SQL.SQLDR("COLUMN_NAME").ToString
    End Function


    Public Function LoadtblDefault_VATType() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT VAT_Type " &
                       " FROM   [Main].dbo.tblDefault_VAT_Type  "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("VAT_Type").ToString)
        End While
        Return list
    End Function

    Public Function LoadtblAddress_Region() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT  regDesc " &
                       " FROM   [Main].dbo.tblAddress_Region  "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("regDesc"))
        End While
        Return list
    End Function

    Public Function LoadtblAddress_Province(Optional ByVal Region As String = "") As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT provDesc " &
                " FROM   [Main].dbo.tblAddress_Province " &
                " INNER JOIN  [Main].dbo.tblAddress_Region ON " &
                " [Main].dbo.tblAddress_Province.regCode =  [Main].dbo.tblAddress_Region.regCode " &
                " WHERE regDesc = @regDesc"
        SQL.FlushParams()
        SQL.AddParam("@regDesc", Region)
        SQL.ReadQuery(query, 2)
        While SQL.SQLDR2.Read
            list.Add(SQL.SQLDR2("provDesc"))
        End While
        Return list
    End Function

    Public Function LoadtblAddress_CityMunicipality(Optional ByVal Province As String = "") As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT citymunDesc " &
                " FROM   [Main].dbo.tblAddress_CityMunicipality " &
                " INNER JOIN  [Main].dbo.tblAddress_Province ON " &
                " [Main].dbo.tblAddress_Province.provCode =  [Main].dbo.tblAddress_CityMunicipality.provCode " &
                " WHERE provDesc = @provDesc"
        SQL.FlushParams()
        SQL.AddParam("@provDesc", Province)
        SQL.ReadQuery(query, 2)
        While SQL.SQLDR2.Read
            list.Add(SQL.SQLDR2("citymunDesc"))
        End While
        Return list
    End Function

    Public Function LoadtblAddress_Brgy(Optional ByVal CityMunicipality As String = "") As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT brgyDesc " &
                " FROM   [Main].dbo.tblAddress_Brgy " &
                " INNER JOIN  [Main].dbo.tblAddress_CityMunicipality ON  " &
                " [Main].dbo.tblAddress_Brgy.citymunCode =  [Main].dbo.tblAddress_CityMunicipality.citymunCode " &
                " WHERE citymunDesc = @citymunDesc"
        SQL.FlushParams()
        SQL.AddParam("@citymunDesc", CityMunicipality)
        SQL.ReadQuery(query, 2)
        While SQL.SQLDR2.Read
            list.Add(SQL.SQLDR2("brgyDesc"))
        End While
        Return list
    End Function

    Public Function LoadtblDefault_Industry() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT Industry " &
                       " FROM   [Main].dbo.tblDefault_Industry  "
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("Industry").ToString)
        End While
        Return list
    End Function

    Public Function LoadtblDefault_GeneralType() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT GeneralType " &
                       " FROM   [Main].dbo.tblDefault_General_Type  "
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("GeneralType").ToString)
        End While
        Return list
    End Function


    Public Function LoadtblDefault_Reports(ByVal Type As String) As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT Report_Name " &
                       " FROM   [Main].dbo.tblDefault_Reports WHERE Type = @Type AND Status = @Status ORDER BY OrderNo"
        SQL.FlushParams()
        SQL.AddParam("@Type", Type)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("Report_Name").ToString)
        End While
        Return list
    End Function

    Public Function LoadtblDefault_RDO() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT RDO_Code, RDO_Description " &
                       " FROM   [Main].dbo.tblDefault_RDO  "
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("RDO_Code").ToString & "-" & SQL.SQLDR("RDO_Description").ToString)
        End While
        Return list
    End Function


    Public Function GenerateRandomPassword(ByRef iLength As Integer) As String
        Dim rdm As New Random()
        Dim allowChrs() As Char = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()
        Dim sResult As String = ""

        For i As Integer = 0 To iLength - 1
            sResult += allowChrs(rdm.Next(0, allowChrs.Length))
        Next

        Dim b As Boolean = False
        While b = False
            Dim query As String = "SELECT * FROM [Main].dbo.tblCompany_User WHERE Password COLLATE Latin1_General_CS_AS = @Password"
            SQL.FlushParams()
            SQL.AddParam("@Password", sResult)
            SQL.ReadQuery(query)
            If SQL.SQLDR.Read Then
                For i As Integer = 0 To iLength - 1
                    sResult += allowChrs(rdm.Next(0, allowChrs.Length))
                Next
                b = False
            Else
                b = True
            End If
        End While
        Return sResult
    End Function

    Public Sub LoadCompany()
        Dim query As String
        query = "SELECT * FROM [Main].dbo.tblDefault_Company"
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Default_Email = SQL.SQLDR("Default_EmailAddress").ToString
        End If
    End Sub

    Public Sub EmailReply(ByVal Type As String)
        Dim query As String
        query = " SELECT Subject, Header, Footer" &
                " FROM [Main].dbo.tblDefault_Email_Reply " &
                " WHERE Type = @Type"
        SQL.FlushParams()
        SQL.AddParam("@Type", Type)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Subject = SQL.SQLDR("Subject").ToString
            BodyHeader = SQL.SQLDR("Header").ToString
            BodyFooter = SQL.SQLDR("Footer").ToString
        End If
    End Sub

    Public Sub SendEmail(ByVal Default_EmailAddress As String, ByVal EmailAddress As String, ByVal Subject As String, ByVal Body As String)
        Dim smtpClient As SmtpClient = New SmtpClient("smtp.gmail.com", 25)
        smtpClient.Credentials = New System.Net.NetworkCredential(Default_EmailAddress, "mikzmikzmikz")
        smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
        smtpClient.EnableSsl = True
        Dim mailMessage As MailMessage = New MailMessage(Default_EmailAddress, EmailAddress)
        mailMessage.Subject = Subject
        mailMessage.Body = Body
        Try
            smtpClient.Send(mailMessage)
        Catch ex As Exception
            ex.ToString()
        End Try
    End Sub

    Public Sub LoadTransaction(accntCode As String, vcecode As String, gridview As GridView, Optional dateFrom As String = "", Optional dateTo As String = "")
        Dim query As String = "SELECT RefType, TransNo, AppDate, Debit, Credit, Balance " &
                              "FROM view_Ledger WHERE AccntCode = '" & accntCode & "' AND VCECode = '" & vcecode & "' "
        If dateFrom <> "" And dateTo <> "" Then
            query += " AND AppDate BETWEEN '" & dateFrom & "' AND '" & dateTo & "'"
        End If
        SQL.ReadQuery(query)
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn(5) {New DataColumn("RefType"), New DataColumn("TransNo"), New DataColumn("AppDate"), New DataColumn("Debit"), New DataColumn("Credit"), New DataColumn("Balance")})
        While SQL.SQLDR.Read
            Dim reftype, transno As String
            Dim appdate As Date
            Dim dr, cr, bal As String
            reftype = SQL.SQLDR("RefType")
            transno = SQL.SQLDR("TransNo")
            appdate = CDate(SQL.SQLDR("AppDate"))
            dr = CDec(SQL.SQLDR("Debit")).ToString("N2")
            cr = CDec(SQL.SQLDR("Credit")).ToString("N2")
            bal = CDec(SQL.SQLDR("Balance")).ToString("N2")
            dt.Rows.Add(reftype, transno, appdate, dr, cr, bal)
        End While
        gridview.DataSource = dt
        gridview.DataBind()
    End Sub

    Public Sub LoadLast60days(accntcode As String, vcecode As String, gridview As GridView)
        Dim query As String
        Dim LatestTxnDate As String = ""
        query = "SELECT MAX(AppDate) AS LatestDate " &
                "FROM view_Ledger WHERE AccntCode = '" & accntcode & "' AND VCECode = '" & vcecode & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            LatestTxnDate = IIf(IsDBNull(SQL.SQLDR("LatestDate")), Now.Date, SQL.SQLDR("LatestDate"))
        End If
        query = "SELECT * FROM view_ledger WHERE AccntCode = '" & accntcode & "' AND VCECode = '" & vcecode & "' " & " and  AppDate >= DATEADD(DAY, -60, '" & LatestTxnDate & "') "
        SQL.ReadQuery(query)
        Dim dt As New DataTable()
        dt.Columns.AddRange(New DataColumn(5) {New DataColumn("RefType"), New DataColumn("TransNo"), New DataColumn("AppDate"), New DataColumn("Debit"), New DataColumn("Credit"), New DataColumn("Balance")})
        While SQL.SQLDR.Read
            Dim reftype, transno As String
            Dim appdate As Date
            Dim dr, cr, bal As String
            reftype = SQL.SQLDR("RefType")
            transno = SQL.SQLDR("TransNo")
            appdate = CDate(SQL.SQLDR("AppDate"))
            dr = CDec(SQL.SQLDR("Debit")).ToString("N2")
            cr = CDec(SQL.SQLDR("Credit")).ToString("N2")
            bal = CDec(SQL.SQLDR("Balance")).ToString("N2")
            dt.Rows.Add(reftype, transno, appdate, dr, cr, bal)
        End While
        gridview.DataSource = dt
        gridview.DataBind()
    End Sub

    Public Function MemberIDExist(memno As String)
        Dim bool As Boolean
        Dim query As String = "SELECT TOP 1 Member_ID FROM tblMember_Master WHERE Member_ID = '" & memno & "'"
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            bool = True
        Else
            bool = False
        End If
        Return bool
    End Function

    Public Function GetTransSetup(ByVal Type As String) As Boolean
        Dim query As String
        query = " SELECT AutoSeries FROM tblTransactionSetup WHERE TransType ='" & Type & "' "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("AutoSeries")
        Else
            Return False
        End If
    End Function

    Public Function GenerateTransID(ColID As String, Table As String) As String
        Dim TransID As String = ""
        ' GENERATE TRANS ID 
        Dim query As String
        query = " SELECT    ISNULL(MAX(" & ColID & ")+ 1,1) AS TransID  " &
                " FROM      " & Table & "  "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            TransID = SQL.SQLDR("TransID")
        Else
            TransID = 0
        End If
        Return TransID
    End Function

    Public Function GenerateTransNum(ByRef Auto As Boolean, ModuleID As String, ColumnPK As String, Table As String) As String
        Dim TransNum As String = ""
        If Auto = True Then
            ' GENERATE TRANS ID 
            Dim query As String
            Do
                query = " SELECT	GlobalSeries, ISNULL(BranchCode,'All') AS BranchCode, ISNULL(BusinessCode,'All') AS BusinessCode,  " &
                                    " 		    ISNULL(TransGroup,0) AS TransGroup, ISNULL(Prefix,'') AS Prefix, ISNULL(Digits,6) AS Digits, " &
                                    "           ISNULL(StartRecord,1) AS StartRecord, LEN(ISNULL(Prefix,'')) + ISNULL(Digits,6) AS ID_Length " &
                                    " FROM	    tblTransactionSetup LEFT JOIN tblTransactionDetails " &
                                    " ON		tblTransactionSetup.TransType = tblTransactionDetails.TransType " &
                                    " WHERE	    tblTransactionSetup.TransType ='" & ModuleID & "' "
                SQL.FlushParams()
                SQL.ReadQuery(query)
                If SQL.SQLDR.Read Then
                    If SQL.SQLDR("GlobalSeries") = True Then
                        If SQL.SQLDR("BranchCode") = "All" AndAlso SQL.SQLDR("BusinessCode") = "All" Then
                            Dim digits As Integer = SQL.SQLDR("Digits")
                            Dim prefix As String = SQL.SQLDR("Prefix")
                            Dim ID_Length As String = SQL.SQLDR("ID_Length")
                            Dim startrecord As Integer = SQL.SQLDR("StartRecord")
                            query = " SELECT    ISNULL(MAX(SUBSTRING(" & ColumnPK & "," & prefix.Length + 1 & "," & digits & "))+ 1,1) AS TransID  " &
                                    " FROM      " & Table & "  " &
                                    " WHERE     " & ColumnPK & " LIKE '" & prefix & "%' AND LEN(" & ColumnPK & ") = '" & ID_Length & "'  AND TransAuto = 1 "
                            SQL.FlushParams()
                            SQL.ReadQuery(query)
                            If SQL.SQLDR.Read Then
                                If SQL.SQLDR("TransID") = 1 Then
                                    TransNum = startrecord
                                Else
                                    TransNum = SQL.SQLDR("TransID")
                                End If
                                For i As Integer = 1 To digits
                                    TransNum = "0" & TransNum
                                Next
                                TransNum = prefix & Strings.Right(TransNum, digits)
                                ' CHECK IF GENERATED TRANSNUM ALREADY EXIST
                                query = " SELECT    " & ColumnPK & " AS TransID  " &
                                        " FROM      " & Table & "  " &
                                        " WHERE     " & ColumnPK & " = '" & TransNum & "' "
                                SQL.FlushParams()
                                SQL.ReadQuery(query)
                                If SQL.SQLDR.Read Then
                                    Dim updateSQL As String
                                    updateSQL = " UPDATE  " & Table & "  SET TransAuto = 1 WHERE " & ColumnPK & " = '" & TransNum & "' "
                                    SQL.FlushParams()
                                    SQL.ExecNonQuery(updateSQL)
                                    TransNum = -1
                                End If
                            End If
                        End If
                    End If
                End If
                If TransNum <> "-1" Then Exit Do
            Loop

            Return TransNum
        Else
            Return ""
        End If
    End Function

    Public Function LoadJE(ByVal Ref_Type As String, Ref_TransID As String) As String
        Dim query As String
        query = " SELECT JE_No FROM tblJE_Header WHERE RefType='" & Ref_Type & "' AND RefTransID ='" & Ref_TransID & "' "
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read AndAlso Not IsDBNull(SQL.SQLDR("JE_No")) Then
            Return SQL.SQLDR("JE_No")
        Else
            Return 0
        End If
    End Function

    Public Function GetVCEName(ByVal Code As String) As String
        Dim query As String
        query = " SELECT Name FROM View_VCEMMaster WHERE Code = @Code"
        SQL.FlushParams()
        SQL.AddParam("@Code", Code)
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            Return SQL.SQLDR("Name").ToString
        Else
            Return ""
        End If
    End Function

    Public Function GetUserFullName(ByVal emailAddress As String) As String
        Dim query As String
        query = "  SELECT EmailAddress, CONCAT(LastName, ', ', FirstName, ' ', MiddleName) AS FullName 
                   FROM [main].dbo.tblCompany_User WHERE EmailAddress = @EmailAddress"
        SQL.FlushParams()
        SQL.AddParam("@EmailAddress", emailAddress)
        SQL.ReadQuery(query, 2)
        If SQL.SQLDR2.Read Then
            Return SQL.SQLDR2("FullName").ToString
        Else
            Return ""
        End If
    End Function
    'new
    Public Function LoadCostCenter() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT  CostCenter " &
                " FROM    tblResponsibility_Center WHERE Status = @Status "
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query, 2)
        While SQL.SQLDR2.Read
            list.Add(SQL.SQLDR2("CostCenter").ToString)
        End While
        Return list
    End Function

    Public Function LoadDisbursementType() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT  Description " &
                " FROM    tblDisbursement_Type " &
                " WHERE   Status =@Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("Description").ToString)
        End While
        Return list
    End Function

    Public Function LoadCollectionType() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT  Description " &
                " FROM    tblCollection_Type " &
                " WHERE   Status =@Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("Description").ToString)
        End While
        Return list
    End Function


    Public Function LoadBank() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT  Bank " &
                " FROM    tblBank " &
                " WHERE   Status =@Status"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("Bank").ToString)
        End While
        Return list
    End Function

    Public Function LoadCollectionPaymentType() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = "SELECT PaymentType FROM tblCollection_PaymentType WHERE Status ='Active' ORDER BY ID  "
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("PaymentType").ToString)
        End While
        Return list
    End Function


    Public Function LoadDisbursementPaymentType() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = "SELECT PaymentType FROM tblDV_PaymentType WHERE Status ='Active' ORDER BY ID "
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("PaymentType").ToString)
        End While
        Return list
    End Function


    Public Function GetCostCenterID(ByVal CostCenter As String) As String
        Dim query As String
        Dim CostID As String = ""
        query = " SELECT  CostID, CostCenter " &
                " FROM    tblResponsibility_Center WHERE CostCenter = @CostCenter "
        SQL.FlushParams()
        SQL.AddParam("@CostCenter", CostCenter)
        SQL.ReadQuery(query, 2)
        If SQL.SQLDR2.Read() Then
            CostID = SQL.SQLDR2("CostID").ToString
        End If
        Return CostID
    End Function

    Public Function GetCostCenter(ByVal CostID As String) As String
        Dim query As String
        Dim CostCenter As String = ""
        query = " SELECT  CostID, CostCenter " &
                " FROM    tblResponsibility_Center WHERE CostID = @CostID"
        SQL.FlushParams()
        SQL.AddParam("@CostID", CostID)
        SQL.ReadQuery(query, 2)
        If SQL.SQLDR2.Read() Then
            CostCenter = SQL.SQLDR2("CostCenter").ToString
        End If
        Return CostCenter
    End Function

    Public Function GetAccountCode(ByVal AccountTitle As String) As String
        Dim query As String
        Dim AccountCode As String = ""
        query = " SELECT  AccountCode, AccountTitle" &
                " FROM    tblCOA WHERE AccountTitle = @AccountTitle"
        SQL.FlushParams()
        SQL.AddParam("@AccountTitle", AccountTitle)
        SQL.ReadQuery(query, 2)
        If SQL.SQLDR2.Read() Then
            AccountCode = SQL.SQLDR2("AccountCode").ToString
        End If
        Return AccountCode
    End Function


    Public Function GetAccountTitle(ByVal AccountCode As String) As String
        Dim query As String
        Dim AccountTitle As String = ""
        query = " SELECT  AccountCode, AccountTitle" &
                " FROM    tblCOA WHERE AccountCode = @AccountCode"
        SQL.FlushParams()
        SQL.AddParam("@AccountCode", AccountCode)
        SQL.ReadQuery(query, 2)
        If SQL.SQLDR2.Read() Then
            AccountTitle = SQL.SQLDR2("AccountTitle").ToString
        End If
        Return AccountTitle
    End Function

    Public Function GetSLBalance(ByVal VCECode As String, ByVal AccountCode As String) As String
        Dim query As String
        Dim Balance As Decimal = 0
        query = " SELECT Top 1 CONVERT(VARCHAR,CONVERT(MONEY,Balance),1) AS Balance FROM View_ledger " &
                " WHERE AccntCode = @AccntCode AND VCECode = @VCECode Order BY No desc"
        SQL.FlushParams()
        SQL.AddParam("@AccntCode", AccountCode)
        SQL.AddParam("@VCECode", VCECode)
        SQL.ReadQuery(query, 2)
        If SQL.SQLDR2.Read() Then
            Balance = SQL.SQLDR2("Balance").ToString
        End If
        Return Balance
    End Function

    Public Function LoadDefaultModuleAccount() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT  ModuleID, Description " &
                " FROM    [Main].dbo.tblDefault_ModuleAccount WHERE Status = @Status "
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query, 2)
        While SQL.SQLDR2.Read
            list.Add(SQL.SQLDR2("Description").ToString)
        End While
        Return list
    End Function

    Public Function GetModuleID(ByVal Description As String) As String
        Dim query As String
        Dim AccountCode As String = ""
        query = " SELECT  ModuleID, Description" &
                " FROM    [Main].dbo.tblDefault_ModuleAccount WHERE Description = @Description"
        SQL.FlushParams()
        SQL.AddParam("@Description", Description)
        SQL.ReadQuery(query, 2)
        If SQL.SQLDR2.Read() Then
            AccountCode = SQL.SQLDR2("ModuleID").ToString
        End If
        Return AccountCode
    End Function

    Public Function LoadDefaultAccount(ByVal ModuleID As String, ByVal Description As String) As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT tblDefaultAccount.AccountCode, tblCOA.AccountTitle FROM tblDefaultAccount " &
                " INNER JOIN tblCOA ON tblDefaultAccount.AccountCode = tblCOA.AccountCode " &
                " WHERE ModuleID = @ModuleID AND Description = @Description "
        SQL.FlushParams()
        SQL.AddParam("@ModuleID", ModuleID)
        SQL.AddParam("@Description", Description)
        SQL.ReadQuery(query, 2)
        While SQL.SQLDR2.Read
            list.Add(SQL.SQLDR2("AccountTitle").ToString)
        End While
        Return list
    End Function

    Public Function GetCompanyCode(ByVal EmailAddress As String) As String
        Dim query As String
        Dim CompanyCode As String = ""
        query = " SELECT Company_Code " &
                       " FROM   [Main].dbo.tblCompany_User WHERE EmailAddress = @EmailAddress "
        SQL.FlushParams()
        SQL.AddParam("@EmailAddress", EmailAddress)
        SQL.ReadQuery(query, 2)
        If SQL.SQLDR2.Read Then
            CompanyCode = SQL.SQLDR2("Company_Code").ToString
        End If
        Return CompanyCode
    End Function

End Module


