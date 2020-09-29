Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Public Class ItemMasterfile_Maintenance
    Inherits System.Web.UI.Page
    Dim TransAuto As Boolean
    Dim ModuleID As String = "Item"
    Dim ColumnPK As String = "ItemCode"
    Dim DBTable As String = "tblItem_Master"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        TransAuto = GetTransSetup(ModuleID)
        If Not IsPostBack Then
            If Session("SessionExists") <> True Then
                Response.Redirect("Login.aspx")
            Else
                Initialize()
            End If
        End If
    End Sub

    <WebMethod()>
    Public Shared Function ListAccountTitle(prefix As String) As String()
        Dim AccountTitle As New List(Of String)()
        Dim query As String
        query = "SELECT AccountTitle, AccountCode FROM tblCOA " & vbCrLf &
                "WHERE Class = 'Posting' AND AccountTitle LIKE  '%' + @AccountTitle + '%' AND Status = 'Active'"
        SQL.FlushParams()
        SQL.AddParam("@AccountTitle", prefix)
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            AccountTitle.Add(String.Format("{0}--{1}", SQL.SQLDR("AccountTitle"), SQL.SQLDR("AccountCode")))
        End While
        Return AccountTitle.ToArray()
    End Function

    <WebMethod()>
    Public Shared Function ListName(prefix As String) As String()
        Dim Name As New List(Of String)()
        Dim query As String
        query = "SELECT Code, Name FROM View_VCEMMaster " & vbCrLf &
                "WHERE Status = @Status AND Name LIKE '%' + @Name + '%'"
        SQL.FlushParams()
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@Name", prefix)
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read()
            Name.Add(String.Format("{0}-{1}", SQL.SQLDR("Name"), SQL.SQLDR("Code")))
        End While
        Return Name.ToArray()
    End Function


    Public Sub Initialize()
        txtItemCode.Attributes.Add("readonly", "readonly")
        txtAccountCode_CostOfSales.Attributes.Add("readonly", "readonly")
        txtAccountCode_Inv.Attributes.Add("readonly", "readonly")
        txtAccountCode_Sales.Attributes.Add("readonly", "readonly")
        txtCode.Attributes.Add("readonly", "readonly")

        txtItemCode.Text = ""
        txtItemName.Text = ""
        txtBarcode.Text = ""
        txtPrice.Text = ""
        txtCost.Text = ""
        txtItemUOM_QTY.Text = ""
        txtLocation.Text = ""
        txtBin.Text = ""
        txtLotNo.Text = ""

        txtCode.Text = ""
        txtName.Text = ""

        txtAccountCode_CostOfSales.Text = ""
        txtAccountTitle_CostOfSales.Text = ""
        txtAccountTitle_Inv.Text = ""
        txtAccountCode_Inv.Text = ""
        txtAccountCode_Sales.Text = ""
        txtAccountTitle_Sales.Text = ""

        ddlItemType.Items.Clear()
        ddlItemType.Items.Add("--Select Item Type--")
        ddlItemType.DataSource = LoadItemType().ToArray
        ddlItemType.DataBind()

        ddlItemCategory.Items.Clear()
        ddlItemCategory.Items.Add("--Select Item Category--")
        ddlItemCategory.DataSource = LoadItemCategory().ToArray
        ddlItemCategory.DataBind()

        ddlItemUOM.Items.Clear()
        ddlItemUOM.Items.Add("--Select Item UOM--")
        ddlItemUOM.DataSource = LoadItemUOM().ToArray
        ddlItemUOM.DataBind()

        ddlWarehouse.Items.Clear()
        ddlWarehouse.Items.Add("--Select Warehoouse--")
        ddlWarehouse.DataSource = LoadWarehouse().ToArray
        ddlWarehouse.DataBind()
    End Sub

    Public Sub EnableControl(ByVal Value As Boolean)
        panelItem.Enabled = Not Value
        fuItemPhoto.Enabled = Not Value
        btnAddItemCategory.Visible = Not Value
        btnAddItemUOM.Visible = Not Value
        tbnAddItemType.Visible = Not Value
        btnAddWarehouse.Visible = Not Value
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Page.Validate()
        If (Page.IsValid) Then
            If btnSave.Text = "Save" Then
                Save()
                Response.Write("<script>alert('Successfully Saved.');window.location='ItemMasterfile_Loadlist.aspx';</script>")
            ElseIf btnSave.Text = "Update" Then
                Update()
                Response.Write("<script>alert('Successfully Updated.');</script>")
                Response.Write("<script>opener.location.reload();</script>")
                Response.Write("<script>window.close();</script>")
            End If
        End If
    End Sub

    Public Sub Save()
        txtItemCode.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
        Dim query As String
        Dim bytes As Byte()
        Using br As BinaryReader = New BinaryReader(fuItemPhoto.PostedFile.InputStream)
            bytes = br.ReadBytes(fuItemPhoto.PostedFile.ContentLength)
        End Using
        query = " INSERT INTO tblItem_Master " &
                " (ItemCode, ItemName, Barcode, ItemType, ItemCategory, ItemUOM, ItemUOM_QTY, ItemPrice, ItemCost, ItemPhoto, Warehouse, Location, Bin, LotNo, Default_Supplier, Inv_AccountCode, Sales_AccountCode, CostOfSales_AccountCode, 
                         Status, DateCreated, WhoCreated)" &
                " VALUES " &
                " (@ItemCode, @ItemName, @Barcode, @ItemType, @ItemCategory, @ItemUOM, @ItemUOM_QTY, @ItemPrice, " &
                "  @ItemCost, @ItemPhoto, @Warehouse, @Location, @Bin, @LotNo, @Default_Supplier, @Inv_AccountCode, @Sales_AccountCode, @CostOfSales_AccountCode, 
                         @Status, @DateCreated, @WhoCreated)"
        SQL.FlushParams()
        SQL.AddParam("@ItemCode", txtItemCode.Text)
        SQL.AddParam("@ItemName", txtItemName.Text)
        SQL.AddParam("@Barcode", txtBarcode.Text)
        SQL.AddParam("@ItemType", ddlItemType.SelectedValue)
        SQL.AddParam("@ItemCategory", ddlItemCategory.SelectedValue)
        SQL.AddParam("@ItemUOM", ddlItemUOM.SelectedValue)
        SQL.AddParam("@ItemUOM_QTY", txtItemUOM_QTY.Text)
        SQL.AddParam("@ItemPrice", CDec(txtPrice.Text).ToString("N2"))
        SQL.AddParam("@ItemCost", CDec(txtCost.Text).ToString("N2"))
        SQL.AddParam("@ItemPhoto", bytes, SqlDbType.VarBinary)
        SQL.AddParam("@Warehouse", ddlWarehouse.SelectedValue)
        SQL.AddParam("@Location", txtLocation.Text)
        SQL.AddParam("@Bin", txtBin.Text)
        SQL.AddParam("@LotNo", txtLotNo.Text)
        SQL.AddParam("@Default_Supplier", txtCode.Text)
        SQL.AddParam("@Inv_AccountCode", txtAccountCode_Inv.Text)
        SQL.AddParam("@Sales_AccountCode", txtAccountCode_Sales.Text)
        SQL.AddParam("@CostOfSales_AccountCode", txtAccountCode_CostOfSales.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.AddParam("@WhoCreated", Session("EmailAddress"))
        SQL.ExecNonQuery(query)
    End Sub

    Public Sub Update()
        Dim ItemCode As String = Request.QueryString("Id")
        Dim query As String
        query = " UPDATE tblItem_Master " &
                " SET ItemName = @ItemName, Barcode = @Barcode, ItemType = @ItemType, ItemCategory = @ItemCategory, " &
                " ItemUOM = @ItemUOM, ItemUOM_QTY = @ItemUOM_QTY, ItemPrice = @ItemPrice, ItemCost = @ItemCost, " &
                " Warehouse = @Warehouse, Location = @Location, Bin = @Bin, LotNo = @LotNo, Default_Supplier = @Default_Supplier, Inv_AccountCode = @Inv_AccountCode, " &
                " Sales_AccountCode = @Sales_AccountCode, CostOfSales_AccountCode = @CostOfSales_AccountCode, 
                  Status = @Status, DateModified = @DateModified, WhoModified = @WhoModified " &
                " WHERE ItemCode = @ItemCode"
        SQL.FlushParams()
        SQL.AddParam("@ItemCode", txtItemCode.Text)
        SQL.AddParam("@ItemName", txtItemName.Text)
        SQL.AddParam("@Barcode", txtBarcode.Text)
        SQL.AddParam("@ItemType", ddlItemType.SelectedValue)
        SQL.AddParam("@ItemCategory", ddlItemCategory.SelectedValue)
        SQL.AddParam("@ItemUOM", ddlItemUOM.SelectedValue)
        SQL.AddParam("@ItemUOM_QTY", txtItemUOM_QTY.Text)
        SQL.AddParam("@ItemPrice", CDec(txtPrice.Text).ToString("N2"))
        SQL.AddParam("@ItemCost", CDec(txtCost.Text).ToString("N2"))
        SQL.AddParam("@Warehouse", ddlWarehouse.SelectedValue)
        SQL.AddParam("@Location", txtLocation.Text)
        SQL.AddParam("@Bin", txtBin.Text)
        SQL.AddParam("@LotNo", txtLotNo.Text)
        SQL.AddParam("@Default_Supplier", txtCode.Text)
        SQL.AddParam("@Inv_AccountCode", txtAccountCode_Inv.Text)
        SQL.AddParam("@Sales_AccountCode", txtAccountCode_Sales.Text)
        SQL.AddParam("@CostOfSales_AccountCode", txtAccountCode_CostOfSales.Text)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateModified", Now.Date)
        SQL.AddParam("@WhoModified", Session("EmailAddress"))
        SQL.ExecNonQuery(query)


        If fuItemPhoto.HasFile = True Then
            Dim bytes As Byte()
            Using br As BinaryReader = New BinaryReader(fuItemPhoto.PostedFile.InputStream)
                bytes = br.ReadBytes(fuItemPhoto.PostedFile.ContentLength)
            End Using
            query = " UPDATE tblItem_Master SET ItemPhoto = @ItemPhoto " &
                    " WHERE ItemCode = @ItemCode"
            SQL.FlushParams()
            SQL.AddParam("@ItemCode", ItemCode)
            SQL.AddParam("@ItemPhoto", bytes, SqlDbType.VarBinary)
            SQL.ExecNonQuery(query)
        End If
    End Sub

    Public Sub View()
        Dim ItemCode As String = Request.QueryString("Id")
        Dim query As String
        query = "  SELECT    * FROM View_Item " &
                "  WHERE View_Item.Status = @Status And ItemCode = @ItemCode "
        SQL.FlushParams()
        SQL.AddParam("@ItemCode", ItemCode)
        SQL.AddParam("@Status", "Active")
        SQL.ReadQuery(query)
        If SQL.SQLDR.Read Then
            txtItemCode.Text = SQL.SQLDR("ItemCode").ToString
            txtItemName.Text = SQL.SQLDR("ItemName").ToString
            txtBarcode.Text = SQL.SQLDR("Barcode").ToString
            txtPrice.Text = CDec(SQL.SQLDR("ItemPrice")).ToString("N2")
            txtCost.Text = CDec(SQL.SQLDR("ItemCost")).ToString("N2")
            ddlItemType.SelectedValue = SQL.SQLDR("ItemType").ToString
            ddlItemCategory.SelectedValue = SQL.SQLDR("ItemCategory").ToString
            ddlItemUOM.SelectedValue = SQL.SQLDR("ItemUOM").ToString
            txtItemUOM_QTY.Text = SQL.SQLDR("ItemUOM_QTY").ToString
            ddlWarehouse.SelectedValue = SQL.SQLDR("Warehouse").ToString
            txtLocation.Text = SQL.SQLDR("Location").ToString
            txtBin.Text = SQL.SQLDR("Bin").ToString
            txtLotNo.Text = SQL.SQLDR("LotNo").ToString
            txtCode.Text = SQL.SQLDR("Default_Supplier").ToString
            txtName.Text = SQL.SQLDR("Name").ToString
            txtAccountCode_Inv.Text = SQL.SQLDR("Inv_AccountCode").ToString
            txtAccountTitle_Inv.Text = SQL.SQLDR("Inv_AccountTitle").ToString
            txtAccountCode_Sales.Text = SQL.SQLDR("Sales_AccountCode").ToString
            txtAccountTitle_Sales.Text = SQL.SQLDR("Sales_AccountTitle").ToString
            txtAccountCode_CostOfSales.Text = SQL.SQLDR("CostOfSales_AccountCode").ToString
            txtAccountTitle_CostOfSales.Text = SQL.SQLDR("CostOfSales_AccountTitle").ToString
        End If
    End Sub

    Private Sub ItemMasterfile_Maintenance_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        If Not IsPostBack Then
            Dim ItemCode As String = Request.QueryString("Id")
            Dim Actions As String = Request.QueryString("Actions")
            If Actions = "Edit" Then
                Initialize()
                EnableControl(False)
                View()
                btnSave.Visible = True
                btnCancel.Visible = True
                btnSave.Text = "Update"
            ElseIf Actions = "View" Then
                Initialize()
                EnableControl(True)
                View()
                btnSave.Visible = False
                btnCancel.Visible = False
            Else
                Initialize()
                EnableControl(False)
                btnSave.Visible = True
                btnCancel.Visible = True
                txtItemCode.Text = GenerateTransNum(TransAuto, ModuleID, ColumnPK, DBTable)
            End If
        End If
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Dim Actions As String = Request.QueryString("Actions")
        If Actions = "Edit" Then
            Response.Write("<script>window.close();</script>")
        Else
            Response.Write("<script>window.location='ItemMasterfile_Loadlist.aspx';</script>")
        End If
    End Sub


    Public Function LoadWarehouse() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT Warehouse " &
                       " FROM   tblItem_Warehouse  "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("Warehouse").ToString)
        End While
        Return list
    End Function

    Public Function LoadItemType() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT ItemType " &
                       " FROM   tblItem_Type  "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("ItemType").ToString)
        End While
        Return list
    End Function


    Public Function LoadItemCategory() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT ItemCategory " &
                       " FROM   tblItem_Category  "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("ItemCategory").ToString)
        End While
        Return list
    End Function

    Public Function LoadItemUOM() As List(Of String)
        Dim list As New List(Of String)
        Dim query As String
        query = " SELECT ItemUOM " &
                       " FROM   tblItem_UOM  "
        SQL.FlushParams()
        SQL.ReadQuery(query)
        While SQL.SQLDR.Read
            list.Add(SQL.SQLDR("ItemUOM").ToString)
        End While
        Return list
    End Function

    <WebMethod>
    Public Shared Function SaveItemType(ItemType As ItemType) As String
        Dim query As String
        query = " INSERT INTO tblItem_Type " &
                    " (ItemType,Status,DateCreated)" &
                    " VALUES " &
                    " (@ItemType,@Status,@DateCreated)"
        SQL.FlushParams()
        SQL.AddParam("@ItemType", ItemType.ItemType)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.ExecNonQuery(query)
        Return "Saved Successfully"
    End Function

    Public Class ItemType
        Public Property ItemType() As String
            Get
                Return m_ItemType
            End Get
            Set
                m_ItemType = Value
            End Set
        End Property
        Private m_ItemType As String
    End Class

    <WebMethod>
    Public Shared Function SaveItemCategory(ItemCategory As ItemCategory) As String
        Dim query As String
        query = " INSERT INTO tblItem_Category " &
                    " (ItemCategory,Status,DateCreated)" &
                    " VALUES " &
                    " (@ItemCategory,@Status,@DateCreated)"
        SQL.FlushParams()
        SQL.AddParam("@ItemCategory", ItemCategory.ItemCategory)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.ExecNonQuery(query)
        Return "Saved Successfully"
    End Function

    Public Class ItemCategory
        Public Property ItemCategory() As String
            Get
                Return m_ItemCategory
            End Get
            Set
                m_ItemCategory = Value
            End Set
        End Property
        Private m_ItemCategory As String
    End Class

    <WebMethod>
    Public Shared Function SaveItemUOM(ItemUOM As ItemUOM) As String
        Dim query As String
        query = " INSERT INTO tblItem_UOM " &
                    " (ItemUOM,Status,DateCreated)" &
                    " VALUES " &
                    " (@ItemUOM,@Status,@DateCreated)"
        SQL.FlushParams()
        SQL.AddParam("@ItemUOM", ItemUOM.ItemUOM)
        SQL.AddParam("@Status", "Active")
        SQL.AddParam("@DateCreated", Now.Date)
        SQL.ExecNonQuery(query)
        Return "Saved Successfully"
    End Function

    Public Class ItemUOM
        Public Property ItemUOM() As String
            Get
                Return m_ItemUOM
            End Get
            Set
                m_ItemUOM = Value
            End Set
        End Property
        Private m_ItemUOM As String
    End Class



    <WebMethod>
    Public Shared Function SaveItemWarehouse(ItemWarehouse As ItemWarehouse) As String
        Dim query As String
        query = " INSERT INTO tblItem_Warehouse " &
                    " (Warehouse,Status,DateCreated)" &
                    " VALUES " &
                    " (@Warehouse,@Status,@DateCreated)"
            SQL.FlushParams()
            SQL.AddParam("@Warehouse", ItemWarehouse.ItemWarehouse)
            SQL.AddParam("@Status", "Active")
            SQL.AddParam("@DateCreated", Now.Date)
        SQL.ExecNonQuery(query)
        Return "Saved Successfully"
    End Function

    Public Class ItemWarehouse
        Public Property ItemWarehouse() As String
            Get
                Return m_ItemWarehouse
            End Get
            Set
                m_ItemWarehouse = Value
            End Set
        End Property
        Private m_ItemWarehouse As String
    End Class


    Public Sub ItemUOM_Load()
        txtItem_UOM.Text = ""
        ddlItemUOM.Items.Clear()
        ddlItemUOM.Items.Add("--Select Item UOM--")
        ddlItemUOM.DataSource = LoadItemUOM().ToArray
        ddlItemUOM.DataBind()
    End Sub

    Public Sub ItemType_Load()
        txtItemType.Text = ""
        ddlItemType.Items.Clear()
        ddlItemType.Items.Add("--Select Item Type--")
        ddlItemType.DataSource = LoadItemType().ToArray
        ddlItemType.DataBind()
    End Sub

    Public Sub ItemCategory_Load()
        txtItemCategory.Text = ""
        ddlItemCategory.Items.Clear()
        ddlItemCategory.Items.Add("--Select Item Category--")
        ddlItemCategory.DataSource = LoadItemCategory().ToArray
        ddlItemCategory.DataBind()
    End Sub

    Public Sub ItemWarehouse_Load()
        txtItem_Warehouse.Text = ""
        ddlWarehouse.Items.Clear()
        ddlWarehouse.Items.Add("--Select Warehoouse--")
        ddlWarehouse.DataSource = LoadWarehouse().ToArray
        ddlWarehouse.DataBind()
    End Sub
End Class