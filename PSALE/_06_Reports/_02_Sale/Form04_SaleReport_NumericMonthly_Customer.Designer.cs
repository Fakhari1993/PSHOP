namespace PSHOP._06_Reports._02_Sale
{
    partial class Form04_SaleReport_NumericMonthly_Customer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form04_SaleReport_NumericMonthly_Customer));
            Janus.Windows.GridEX.GridEXLayout gridEX1_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX_Goods_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.rpt_Sale_MonthlyNumeric_CustomerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_Report = new PSHOP._06_Reports.DataSet_Report();
            this.rpt_Sale_MonthlyNumeric_Customer_DetailBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmb_ExportToExcel = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnu_Excel_Table = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_Send_Chart = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.cmb_Print = new System.Windows.Forms.ToolStripSplitButton();
            this.mnu_PrintThis = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_PrintAll = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.gridEX_Goods = new Janus.Windows.GridEX.GridEX();
            this.chk_Kol = new System.Windows.Forms.CheckBox();
            this.chk_Box = new System.Windows.Forms.CheckBox();
            this.chk_Pack = new System.Windows.Forms.CheckBox();
            this.chk_Joz = new System.Windows.Forms.CheckBox();
            this.rpt_Sale_MonthlyNumeric_CustomerTableAdapter = new PSHOP._06_Reports.DataSet_ReportTableAdapters.Rpt_Sale_MonthlyNumeric_CustomerTableAdapter();
            this.rpt_Sale_MonthlyNumeric_Customer_DetailTableAdapter = new PSHOP._06_Reports.DataSet_ReportTableAdapters.Rpt_Sale_MonthlyNumeric_Customer_DetailTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_Sale_MonthlyNumeric_CustomerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_Sale_MonthlyNumeric_Customer_DetailBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).BeginInit();
            this.SuspendLayout();
            // 
            // rpt_Sale_MonthlyNumeric_CustomerBindingSource
            // 
            this.rpt_Sale_MonthlyNumeric_CustomerBindingSource.DataMember = "Rpt_Sale_MonthlyNumeric_Customer";
            this.rpt_Sale_MonthlyNumeric_CustomerBindingSource.DataSource = this.dataSet_Report;
            // 
            // dataSet_Report
            // 
            this.dataSet_Report.DataSetName = "DataSet_Report";
            this.dataSet_Report.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // rpt_Sale_MonthlyNumeric_Customer_DetailBindingSource
            // 
            this.rpt_Sale_MonthlyNumeric_Customer_DetailBindingSource.DataMember = "FK_Rpt_Sale_MonthlyNumeric_Customer_Rpt_Sale_MonthlyNumeric_Customer_Detail";
            this.rpt_Sale_MonthlyNumeric_Customer_DetailBindingSource.DataSource = this.rpt_Sale_MonthlyNumeric_CustomerBindingSource;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = global::PSHOP.Properties.Resources.me_bg;
            resources.ApplyResources(this.bindingNavigator1, "bindingNavigator1");
            this.bindingNavigator1.BindingSource = this.rpt_Sale_MonthlyNumeric_Customer_DetailBindingSource;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.cmb_ExportToExcel,
            this.toolStripSeparator8,
            this.cmb_Print,
            this.toolStripSeparator1,
            this.toolStripLabel1});
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            resources.ApplyResources(this.bindingNavigatorCountItem, "bindingNavigatorCountItem");
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveFirstItem, "bindingNavigatorMoveFirstItem");
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMovePreviousItem, "bindingNavigatorMovePreviousItem");
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            resources.ApplyResources(this.bindingNavigatorSeparator, "bindingNavigatorSeparator");
            // 
            // bindingNavigatorPositionItem
            // 
            resources.ApplyResources(this.bindingNavigatorPositionItem, "bindingNavigatorPositionItem");
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            resources.ApplyResources(this.bindingNavigatorSeparator1, "bindingNavigatorSeparator1");
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveNextItem, "bindingNavigatorMoveNextItem");
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            resources.ApplyResources(this.bindingNavigatorMoveLastItem, "bindingNavigatorMoveLastItem");
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            resources.ApplyResources(this.bindingNavigatorSeparator2, "bindingNavigatorSeparator2");
            // 
            // cmb_ExportToExcel
            // 
            this.cmb_ExportToExcel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmb_ExportToExcel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_Excel_Table,
            this.mnu_Send_Chart});
            resources.ApplyResources(this.cmb_ExportToExcel, "cmb_ExportToExcel");
            this.cmb_ExportToExcel.Name = "cmb_ExportToExcel";
            // 
            // mnu_Excel_Table
            // 
            this.mnu_Excel_Table.Name = "mnu_Excel_Table";
            resources.ApplyResources(this.mnu_Excel_Table, "mnu_Excel_Table");
            this.mnu_Excel_Table.Click += new System.EventHandler(this.mnu_Excel_Table_Click);
            // 
            // mnu_Send_Chart
            // 
            this.mnu_Send_Chart.Name = "mnu_Send_Chart";
            resources.ApplyResources(this.mnu_Send_Chart, "mnu_Send_Chart");
            this.mnu_Send_Chart.Click += new System.EventHandler(this.mnu_Send_Chart_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            // 
            // cmb_Print
            // 
            this.cmb_Print.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmb_Print.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_PrintThis,
            this.mnu_PrintAll});
            this.cmb_Print.Image = global::PSHOP.Properties.Resources.Printer;
            resources.ApplyResources(this.cmb_Print, "cmb_Print");
            this.cmb_Print.Name = "cmb_Print";
            // 
            // mnu_PrintThis
            // 
            this.mnu_PrintThis.Name = "mnu_PrintThis";
            resources.ApplyResources(this.mnu_PrintThis, "mnu_PrintThis");
            this.mnu_PrintThis.Click += new System.EventHandler(this.mnu_PrintTable_Click);
            // 
            // mnu_PrintAll
            // 
            this.mnu_PrintAll.Name = "mnu_PrintAll";
            resources.ApplyResources(this.mnu_PrintAll, "mnu_PrintAll");
            this.mnu_PrintAll.Click += new System.EventHandler(this.mnu_PrintAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Name = "toolStripLabel1";
            resources.ApplyResources(this.toolStripLabel1, "toolStripLabel1");
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            resources.ApplyResources(this.saveFileDialog1, "saveFileDialog1");
            this.saveFileDialog1.RestoreDirectory = true;
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.AllowAutoHideAnimation = false;
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.LightSlateGray;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2010;
            this.uiPanel0.Id = new System.Guid("c1e852ec-e270-40e7-b720-4371764bb6b2");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            this.uiPanel1.Id = new System.Guid("0060f9c5-9ca2-45a0-8194-b1fc0edcb165");
            this.uiPanelManager1.Panels.Add(this.uiPanel1);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("c1e852ec-e270-40e7-b720-4371764bb6b2"), Janus.Windows.UI.Dock.PanelDockStyle.Bottom, new System.Drawing.Size(788, 355), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("0060f9c5-9ca2-45a0-8194-b1fc0edcb165"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(257, 397), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("c1e852ec-e270-40e7-b720-4371764bb6b2"), new System.Drawing.Point(21, 562), new System.Drawing.Size(654, 200), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("0060f9c5-9ca2-45a0-8194-b1fc0edcb165"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            resources.ApplyResources(this.uiPanel0, "uiPanel0");
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.FloatingLocation = new System.Drawing.Point(21, 562);
            this.uiPanel0.FloatingSize = new System.Drawing.Size(654, 200);
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Center;
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.crystalReportViewer1);
            resources.ApplyResources(this.uiPanel0Container, "uiPanel0Container");
            this.uiPanel0Container.Name = "uiPanel0Container";
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.DisplayStatusBar = false;
            resources.ApplyResources(this.crystalReportViewer1, "crystalReportViewer1");
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // uiPanel1
            // 
            this.uiPanel1.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            resources.ApplyResources(this.uiPanel1, "uiPanel1");
            this.uiPanel1.Name = "uiPanel1";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.gridEX1);
            resources.ApplyResources(this.uiPanel1Container, "uiPanel1Container");
            this.uiPanel1Container.Name = "uiPanel1Container";
            // 
            // gridEX1
            // 
            this.gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.DataSource = this.rpt_Sale_MonthlyNumeric_CustomerBindingSource;
            resources.ApplyResources(this.gridEX1, "gridEX1");
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX1.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX1.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX1.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX1.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX1.GroupByBoxVisible = false;
            gridEX1_Layout_0.DataSource = this.rpt_Sale_MonthlyNumeric_CustomerBindingSource;
            gridEX1_Layout_0.IsCurrentLayout = true;
            gridEX1_Layout_0.Key = "PERP";
            resources.ApplyResources(gridEX1_Layout_0, "gridEX1_Layout_0");
            this.gridEX1.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX1_Layout_0});
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX1.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // gridEX_Goods
            // 
            this.gridEX_Goods.AllowColumnDrag = false;
            this.gridEX_Goods.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Goods.AlternatingColors = true;
            this.gridEX_Goods.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX_Goods.ColumnHeaders = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Goods.DataSource = this.rpt_Sale_MonthlyNumeric_Customer_DetailBindingSource;
            resources.ApplyResources(this.gridEX_Goods, "gridEX_Goods");
            this.gridEX_Goods.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Goods.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Goods.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Goods.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX_Goods.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(220)))), ((int)(((byte)(253)))));
            this.gridEX_Goods.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Goods.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX_Goods.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX_Goods.GroupByBoxVisible = false;
            this.gridEX_Goods.GroupIndent = 9;
            this.gridEX_Goods.HideColumnsWhenGrouped = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX_Goods.Hierarchical = true;
            this.gridEX_Goods.Indent = 13;
            gridEX_Goods_Layout_0.DataSource = this.rpt_Sale_MonthlyNumeric_Customer_DetailBindingSource;
            gridEX_Goods_Layout_0.IsCurrentLayout = true;
            gridEX_Goods_Layout_0.Key = "PERP";
            resources.ApplyResources(gridEX_Goods_Layout_0, "gridEX_Goods_Layout_0");
            this.gridEX_Goods.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX_Goods_Layout_0});
            this.gridEX_Goods.Name = "gridEX_Goods";
            this.gridEX_Goods.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Goods.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Goods.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Goods.RecordNavigator = true;
            this.gridEX_Goods.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.TotalRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX_Goods.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Goods.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.gridEX_Goods.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.gridEX_Accounts_RowDoubleClick);
            this.gridEX_Goods.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.gridEX_Goods_MouseDoubleClick);
            // 
            // chk_Kol
            // 
            resources.ApplyResources(this.chk_Kol, "chk_Kol");
            this.chk_Kol.BackColor = System.Drawing.Color.Transparent;
            this.chk_Kol.BackgroundImage = global::PSHOP.Properties.Resources.me_bg;
            this.chk_Kol.Name = "chk_Kol";
            this.chk_Kol.UseVisualStyleBackColor = false;
            this.chk_Kol.CheckedChanged += new System.EventHandler(this.chk_Kol_CheckedChanged);
            // 
            // chk_Box
            // 
            resources.ApplyResources(this.chk_Box, "chk_Box");
            this.chk_Box.BackColor = System.Drawing.Color.Transparent;
            this.chk_Box.BackgroundImage = global::PSHOP.Properties.Resources.me_bg;
            this.chk_Box.Name = "chk_Box";
            this.chk_Box.UseVisualStyleBackColor = false;
            this.chk_Box.CheckedChanged += new System.EventHandler(this.chk_Kol_CheckedChanged);
            // 
            // chk_Pack
            // 
            resources.ApplyResources(this.chk_Pack, "chk_Pack");
            this.chk_Pack.BackColor = System.Drawing.Color.Transparent;
            this.chk_Pack.BackgroundImage = global::PSHOP.Properties.Resources.me_bg;
            this.chk_Pack.Name = "chk_Pack";
            this.chk_Pack.UseVisualStyleBackColor = false;
            this.chk_Pack.CheckedChanged += new System.EventHandler(this.chk_Kol_CheckedChanged);
            // 
            // chk_Joz
            // 
            resources.ApplyResources(this.chk_Joz, "chk_Joz");
            this.chk_Joz.BackColor = System.Drawing.Color.Transparent;
            this.chk_Joz.BackgroundImage = global::PSHOP.Properties.Resources.me_bg;
            this.chk_Joz.Name = "chk_Joz";
            this.chk_Joz.UseVisualStyleBackColor = false;
            this.chk_Joz.CheckedChanged += new System.EventHandler(this.chk_Kol_CheckedChanged);
            // 
            // rpt_Sale_MonthlyNumeric_CustomerTableAdapter
            // 
            this.rpt_Sale_MonthlyNumeric_CustomerTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_Sale_MonthlyNumeric_Customer_DetailTableAdapter
            // 
            this.rpt_Sale_MonthlyNumeric_Customer_DetailTableAdapter.ClearBeforeFill = true;
            // 
            // Form04_SaleReport_NumericMonthly_Customer
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chk_Joz);
            this.Controls.Add(this.chk_Pack);
            this.Controls.Add(this.chk_Box);
            this.Controls.Add(this.chk_Kol);
            this.Controls.Add(this.gridEX_Goods);
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.bindingNavigator1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form04_SaleReport_NumericMonthly_Customer";
            this.Activated += new System.EventHandler(this.Form04_SaleReport_NumericMonthly_Activated);
            this.Load += new System.EventHandler(this.Form04_SaleReport_NumericMonthly_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form04_SaleReport_NumericMonthly_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.rpt_Sale_MonthlyNumeric_CustomerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_Sale_MonthlyNumeric_Customer_DetailBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
        private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
        private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.GridEX.GridEX gridEX_Goods;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private System.Windows.Forms.ToolStripDropDownButton cmb_ExportToExcel;
        private System.Windows.Forms.ToolStripMenuItem mnu_Excel_Table;
        private System.Windows.Forms.ToolStripMenuItem mnu_Send_Chart;
        private DataSet_Report dataSet_Report;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.CheckBox chk_Joz;
        private System.Windows.Forms.CheckBox chk_Pack;
        private System.Windows.Forms.CheckBox chk_Box;
        private System.Windows.Forms.CheckBox chk_Kol;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private System.Windows.Forms.BindingSource rpt_Sale_MonthlyNumeric_CustomerBindingSource;
        private DataSet_ReportTableAdapters.Rpt_Sale_MonthlyNumeric_CustomerTableAdapter rpt_Sale_MonthlyNumeric_CustomerTableAdapter;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private System.Windows.Forms.BindingSource rpt_Sale_MonthlyNumeric_Customer_DetailBindingSource;
        private DataSet_ReportTableAdapters.Rpt_Sale_MonthlyNumeric_Customer_DetailTableAdapter rpt_Sale_MonthlyNumeric_Customer_DetailTableAdapter;
        private System.Windows.Forms.ToolStripSplitButton cmb_Print;
        private System.Windows.Forms.ToolStripMenuItem mnu_PrintThis;
        private System.Windows.Forms.ToolStripMenuItem mnu_PrintAll;
    }
}