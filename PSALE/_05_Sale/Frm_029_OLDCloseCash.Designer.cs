namespace PSHOP._05_Sale
{
    partial class Frm_029_OLDCloseCash
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label column04Label;
            System.Windows.Forms.Label column03Label;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_029_OLDCloseCash));
            Janus.Windows.GridEX.GridEXLayout gridEX_Header_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.closeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_Sale = new PSHOP._05_Sale.DataSet_Sale();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.cmb_User = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.bt_Search = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_ExportDoc = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.gridEX_Header = new Janus.Windows.GridEX.GridEX();
            this.table_010_SaleFactorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.closeTableAdapter = new PSHOP._05_Sale.DataSet_SaleTableAdapters.CloseTableAdapter();
            this.tableAdapterManager = new PSHOP._05_Sale.DataSet_SaleTableAdapters.TableAdapterManager();
            this.table_010_SaleFactorTableAdapter = new PSHOP._05_Sale.DataSet_SaleTableAdapters.Table_010_SaleFactorTableAdapter();
            this.ribbonBarMergeContainer1 = new DevComponents.DotNetBar.RibbonBarMergeContainer();
            this.ribbonBar1 = new DevComponents.DotNetBar.RibbonBar();
            this.gridEXFieldChooserControl1 = new Janus.Windows.GridEX.GridEXFieldChooserControl();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.microChartItem1 = new DevComponents.DotNetBar.MicroChartItem();
            column04Label = new System.Windows.Forms.Label();
            column03Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Header)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_010_SaleFactorBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            this.ribbonBarMergeContainer1.SuspendLayout();
            this.ribbonBar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // column04Label
            // 
            column04Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            column04Label.AutoSize = true;
            column04Label.BackColor = System.Drawing.Color.Transparent;
            column04Label.Location = new System.Drawing.Point(711, 50);
            column04Label.Name = "column04Label";
            column04Label.Size = new System.Drawing.Size(65, 13);
            column04Label.TabIndex = 57;
            column04Label.Text = "عملکرد انبار:";
            // 
            // column03Label
            // 
            column03Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            column03Label.AutoSize = true;
            column03Label.BackColor = System.Drawing.Color.Transparent;
            column03Label.Location = new System.Drawing.Point(748, 15);
            column03Label.Name = "column03Label";
            column03Label.Size = new System.Drawing.Size(30, 13);
            column03Label.TabIndex = 56;
            column03Label.Text = "انبار:";
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.LightSlateGray;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.VS2010;
            // 
            // closeBindingSource
            // 
            this.closeBindingSource.DataMember = "Close";
            this.closeBindingSource.DataSource = this.dataSet_Sale;
            // 
            // dataSet_Sale
            // 
            this.dataSet_Sale.DataSetName = "DataSet_Sale";
            this.dataSet_Sale.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = global::PSHOP.Properties.Resources.me_bg;
            this.bindingNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel2,
            this.cmb_User,
            this.toolStripLabel1,
            this.faDatePickerStrip1,
            this.bt_Search,
            this.toolStripSeparator2,
            this.bt_ExportDoc,
            this.toolStripSeparator1,
            this.toolStripButton1});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(714, 35);
            this.bindingNavigator1.TabIndex = 13;
            this.bindingNavigator1.Text = "bindingNavigator2";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.Font = new System.Drawing.Font("B Mitra", 14.25F);
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLabel2.Size = new System.Drawing.Size(53, 32);
            this.toolStripLabel2.Text = "صندوق:";
            // 
            // cmb_User
            // 
            this.cmb_User.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmb_User.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cmb_User.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_User.Font = new System.Drawing.Font("B Mitra", 14.25F);
            this.cmb_User.Items.AddRange(new object[] {
            "تمام فاکتورها",
            "فاکتورهای باطل شده",
            "فاکتورهای باطل نشده"});
            this.cmb_User.Name = "cmb_User";
            this.cmb_User.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmb_User.Size = new System.Drawing.Size(160, 35);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Font = new System.Drawing.Font("B Mitra", 14.25F);
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLabel1.Size = new System.Drawing.Size(54, 32);
            this.toolStripLabel1.Text = "تا تاریخ:";
            // 
            // faDatePickerStrip1
            // 
            this.faDatePickerStrip1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.faDatePickerStrip1.AutoSize = false;
            this.faDatePickerStrip1.BackColor = System.Drawing.SystemColors.Window;
            this.faDatePickerStrip1.Font = new System.Drawing.Font("B Mitra", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.faDatePickerStrip1.Name = "faDatePickerStrip1";
            this.faDatePickerStrip1.Size = new System.Drawing.Size(133, 32);
            // 
            // bt_Search
            // 
            this.bt_Search.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_Search.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_Search.Image = global::PSHOP.Properties.Resources.Search;
            this.bt_Search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Search.Name = "bt_Search";
            this.bt_Search.Size = new System.Drawing.Size(23, 32);
            this.bt_Search.Text = "مشاهده";
            this.bt_Search.ToolTipText = "Ctrl+F";
            this.bt_Search.Click += new System.EventHandler(this.bt_Search_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 35);
            // 
            // bt_ExportDoc
            // 
            this.bt_ExportDoc.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.bt_ExportDoc.Image = ((System.Drawing.Image)(resources.GetObject("bt_ExportDoc.Image")));
            this.bt_ExportDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_ExportDoc.Name = "bt_ExportDoc";
            this.bt_ExportDoc.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_ExportDoc.Size = new System.Drawing.Size(102, 32);
            this.bt_ExportDoc.Text = "بستن صندوق";
            this.bt_ExportDoc.ToolTipText = "Ctrl+S";
            this.bt_ExportDoc.Click += new System.EventHandler(this.bt_ExportDoc_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 35);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Font = new System.Drawing.Font("B Mitra", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripButton1.Size = new System.Drawing.Size(87, 32);
            this.toolStripButton1.Text = "کاردکس کالا";
            this.toolStripButton1.ToolTipText = " ";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // gridEX_Header
            // 
            this.gridEX_Header.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Header.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Header.AlternatingColors = true;
            this.gridEX_Header.BuiltInTextsData = resources.GetString("gridEX_Header.BuiltInTextsData");
            this.gridEX_Header.CardWidth = 751;
            this.gridEX_Header.ColumnSetNavigation = Janus.Windows.GridEX.ColumnSetNavigation.ColumnSet;
            this.gridEX_Header.DataSource = this.table_010_SaleFactorBindingSource;
            gridEX_Header_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Header_DesignTimeLayout.LayoutString");
            this.gridEX_Header.DesignTimeLayout = gridEX_Header_DesignTimeLayout;
            this.gridEX_Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Header.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Header.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Header.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Header.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Header.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Header.Font = new System.Drawing.Font("B Mitra", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.gridEX_Header.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX_Header.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX_Header.GroupByBoxVisible = false;
            this.gridEX_Header.GroupTotals = Janus.Windows.GridEX.GroupTotals.ExpandedGroup;
            this.gridEX_Header.Location = new System.Drawing.Point(3, 38);
            this.gridEX_Header.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.gridEX_Header.Name = "gridEX_Header";
            this.gridEX_Header.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Header.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Header.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Header.OfficeCustomColor = System.Drawing.Color.Black;
            this.gridEX_Header.RecordNavigator = true;
            this.gridEX_Header.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_Header.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Header.SaveSettings = true;
            this.gridEX_Header.SettingsKey = "StoregridColsecash7";
            this.gridEX_Header.Size = new System.Drawing.Size(708, 411);
            this.gridEX_Header.TabIndex = 14;
            this.gridEX_Header.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Header.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Header.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Header.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // table_010_SaleFactorBindingSource
            // 
            this.table_010_SaleFactorBindingSource.DataMember = "Table_010_SaleFactor";
            this.table_010_SaleFactorBindingSource.DataSource = this.dataSet_Sale;
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.TabPage;
            this.uiGroupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.uiGroupBox1.Controls.Add(column04Label);
            this.uiGroupBox1.Controls.Add(column03Label);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.None;
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiGroupBox1.OfficeCustomColor = System.Drawing.Color.SlateBlue;
            this.uiGroupBox1.Size = new System.Drawing.Size(788, 105);
            this.uiGroupBox1.TabIndex = 1;
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // closeTableAdapter
            // 
            this.closeTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Connection = null;
            this.tableAdapterManager.Rpt_ReturnFactor_Customer_FactorsTableAdapter = null;
            this.tableAdapterManager.Table_007_FactorBeforeTableAdapter = null;
            this.tableAdapterManager.Table_007_PwhrsDraftTableAdapter = null;
            this.tableAdapterManager.Table_008_Child_PwhrsDraftTableAdapter = null;
            this.tableAdapterManager.Table_008_Child1_FactorBeforeTableAdapter = null;
            this.tableAdapterManager.Table_009_Child2_FactorBeforeTableAdapter = null;
            this.tableAdapterManager.Table_010_SaleFactor_NoDoc_DraftTableAdapter = null;
            this.tableAdapterManager.Table_010_SaleFactor_ReportCustomer_Detail1TableAdapter = null;
            this.tableAdapterManager.Table_010_SaleFactorTableAdapter = null;
            this.tableAdapterManager.Table_011_Child1_SaleFactor_NoDoc_DraftTableAdapter = null;
            this.tableAdapterManager.Table_011_Child1_SaleFactorTableAdapter = null;
            this.tableAdapterManager.Table_011_PwhrsReceiptTableAdapter = null;
            this.tableAdapterManager.Table_012_Child_PwhrsReceiptTableAdapter = null;
            this.tableAdapterManager.Table_012_Child2_SaleFactor_NoDoc_DraftTableAdapter = null;
            this.tableAdapterManager.Table_012_Child2_SaleFactorTableAdapter = null;
            this.tableAdapterManager.Table_012_SaleFactorDeallerTableAdapter = null;
            this.tableAdapterManager.Table_012_SaleFactorSellerTableAdapter = null;
            this.tableAdapterManager.Table_018_MarjooiSaleTableAdapter = null;
            this.tableAdapterManager.Table_019_Child1_MarjooiSaleTableAdapter = null;
            this.tableAdapterManager.Table_020_Child2_MarjooiSale1TableAdapter = null;
            this.tableAdapterManager.Table_020_Child2_MarjooiSaleTableAdapter = null;
            this.tableAdapterManager.Table_032_GoodPriceTableAdapter = null;
            this.tableAdapterManager.Table_034_SaleFactor_Child3TableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PSHOP._05_Sale.DataSet_SaleTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // table_010_SaleFactorTableAdapter
            // 
            this.table_010_SaleFactorTableAdapter.ClearBeforeFill = true;
            // 
            // ribbonBarMergeContainer1
            // 
            this.ribbonBarMergeContainer1.AutoActivateTab = false;
            this.ribbonBarMergeContainer1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.ribbonBarMergeContainer1.Controls.Add(this.ribbonBar1);
            this.ribbonBarMergeContainer1.Location = new System.Drawing.Point(155, 10);
            this.ribbonBarMergeContainer1.Margin = new System.Windows.Forms.Padding(8);
            this.ribbonBarMergeContainer1.MergeRibbonGroupName = "SettingTab";
            this.ribbonBarMergeContainer1.Name = "ribbonBarMergeContainer1";
            this.ribbonBarMergeContainer1.RibbonTabColorTable = DevComponents.DotNetBar.eRibbonTabColor.Green;
            this.ribbonBarMergeContainer1.RibbonTabText = "تنظیمات";
            this.ribbonBarMergeContainer1.ShowFocusRectangle = true;
            this.ribbonBarMergeContainer1.Size = new System.Drawing.Size(122, 25);
            this.ribbonBarMergeContainer1.StretchLastRibbonBar = true;
            // 
            // 
            // 
            this.ribbonBarMergeContainer1.Style.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBarMergeContainer1.StyleMouseDown.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBarMergeContainer1.StyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonBarMergeContainer1.TabIndex = 27;
            this.ribbonBarMergeContainer1.Visible = false;
            // 
            // ribbonBar1
            // 
            this.ribbonBar1.AutoOverflowEnabled = true;
            // 
            // 
            // 
            this.ribbonBar1.BackgroundMouseOverStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.ribbonBar1.ContainerControlProcessDialogKey = true;
            this.ribbonBar1.Controls.Add(this.gridEXFieldChooserControl1);
            this.ribbonBar1.Dock = System.Windows.Forms.DockStyle.Right;
            this.ribbonBar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.buttonItem1,
            this.buttonItem2,
            this.microChartItem1});
            this.ribbonBar1.Location = new System.Drawing.Point(-49, 0);
            this.ribbonBar1.Margin = new System.Windows.Forms.Padding(8);
            this.ribbonBar1.Name = "ribbonBar1";
            this.ribbonBar1.ResizeItemsToFit = false;
            this.ribbonBar1.Size = new System.Drawing.Size(171, 25);
            this.ribbonBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonBar1.TabIndex = 0;
            this.ribbonBar1.Text = "انتخاب ستونها";
            // 
            // 
            // 
            this.ribbonBar1.TitleStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // 
            // 
            this.ribbonBar1.TitleStyleMouseOver.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            // 
            // gridEXFieldChooserControl1
            // 
            this.gridEXFieldChooserControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEXFieldChooserControl1.GridEX = this.gridEX_Header;
            this.gridEXFieldChooserControl1.Location = new System.Drawing.Point(0, 0);
            this.gridEXFieldChooserControl1.Margin = new System.Windows.Forms.Padding(8);
            this.gridEXFieldChooserControl1.Name = "gridEXFieldChooserControl1";
            this.gridEXFieldChooserControl1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEXFieldChooserControl1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEXFieldChooserControl1.Size = new System.Drawing.Size(171, 0);
            this.gridEXFieldChooserControl1.TabIndex = 2;
            this.gridEXFieldChooserControl1.Text = "gridEXFieldChooserControl1";
            // 
            // buttonItem1
            // 
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.SubItemsExpandWidth = 14;
            this.buttonItem1.Text = "buttonItem1";
            // 
            // buttonItem2
            // 
            this.buttonItem2.Name = "buttonItem2";
            this.buttonItem2.SubItemsExpandWidth = 14;
            this.buttonItem2.Text = "buttonItem2";
            // 
            // microChartItem1
            // 
            this.microChartItem1.Name = "microChartItem1";
            // 
            // Frm_029_OLDCloseCash
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(714, 452);
            this.Controls.Add(this.ribbonBarMergeContainer1);
            this.Controls.Add(this.gridEX_Header);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("B Mitra", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Frm_029_OLDCloseCash";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "بستن صندوق فروش";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_029_CloseCash_FormClosing);
            this.Load += new System.EventHandler(this.Frm_029_CloseCash_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_029_CloseCash_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.closeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.Configuration.IPersistComponentSettings)(this.gridEX_Header)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Header)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_010_SaleFactorBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            this.ribbonBarMergeContainer1.ResumeLayout(false);
            this.ribbonBar1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripComboBox cmb_User;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip1;
        private System.Windows.Forms.ToolStripButton bt_Search;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Janus.Windows.GridEX.GridEX gridEX_Header;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton bt_ExportDoc;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        //private DataSet_SaleTableAdapters.Table_010_CancleSaleFactorForClose1TableAdapter table_010_CancleSaleFactorForClose1TableAdapter;
        //private DataSet_SaleTableAdapters.Table_010_SaleFactorForCloseTableAdapter table_010_SaleFactorForCloseTableAdapter;
        private DataSet_Sale dataSet_Sale;
        private System.Windows.Forms.BindingSource closeBindingSource;
        private DataSet_SaleTableAdapters.CloseTableAdapter closeTableAdapter;
        private DataSet_SaleTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingSource table_010_SaleFactorBindingSource;
        private DataSet_SaleTableAdapters.Table_010_SaleFactorTableAdapter table_010_SaleFactorTableAdapter;
        private DevComponents.DotNetBar.RibbonBarMergeContainer ribbonBarMergeContainer1;
        private DevComponents.DotNetBar.RibbonBar ribbonBar1;
        private Janus.Windows.GridEX.GridEXFieldChooserControl gridEXFieldChooserControl1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.MicroChartItem microChartItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        //private DataSet_SaleTableAdapters.Table_010_CancleSaleFactorForClose1TableAdapter table_010_CancleSaleFactorForClose1TableAdapter;
      
    }
}