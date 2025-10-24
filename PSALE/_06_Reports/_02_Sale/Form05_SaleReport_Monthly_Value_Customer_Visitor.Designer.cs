namespace PSHOP._06_Reports._02_Sale
{
    partial class Form05_SaleReport_Monthly_Value_Customer_Visitor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form05_SaleReport_Monthly_Value_Customer_Visitor));
            Janus.Windows.GridEX.GridEXLayout gridEX_Visitors_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX_Goods_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.rpt_Sale_MonthlyNumeric_VisitorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_Report = new PSHOP._06_Reports.DataSet_Report();
            this.rpt_Sale_Monthly_Value_Visitor_CustomerBindingSource = new System.Windows.Forms.BindingSource(this.components);
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
            this.cmb_Print = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_Visitors = new Janus.Windows.GridEX.GridEX();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.gridEX_Goods = new Janus.Windows.GridEX.GridEX();
            this.rpt_Sale_MonthlyNumeric_VisitorTableAdapter = new PSHOP._06_Reports.DataSet_ReportTableAdapters.Rpt_Sale_MonthlyNumeric_VisitorTableAdapter();
            this.rpt_Sale_Monthly_Value_Visitor_CustomerTableAdapter = new PSHOP._06_Reports.DataSet_ReportTableAdapters.Rpt_Sale_Monthly_Value_Visitor_CustomerTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_Sale_MonthlyNumeric_VisitorBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_Sale_Monthly_Value_Visitor_CustomerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Visitors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).BeginInit();
            this.SuspendLayout();
            // 
            // rpt_Sale_MonthlyNumeric_VisitorBindingSource
            // 
            this.rpt_Sale_MonthlyNumeric_VisitorBindingSource.DataMember = "Rpt_Sale_MonthlyNumeric_Visitor";
            this.rpt_Sale_MonthlyNumeric_VisitorBindingSource.DataSource = this.dataSet_Report;
            // 
            // dataSet_Report
            // 
            this.dataSet_Report.DataSetName = "DataSet_Report";
            this.dataSet_Report.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // rpt_Sale_Monthly_Value_Visitor_CustomerBindingSource
            // 
            this.rpt_Sale_Monthly_Value_Visitor_CustomerBindingSource.DataMember = "FK_Rpt_Sale_MonthlyNumeric_Visitor_Rpt_Sale_Monthly_Value_Visitor_Customer";
            this.rpt_Sale_Monthly_Value_Visitor_CustomerBindingSource.DataSource = this.rpt_Sale_MonthlyNumeric_VisitorBindingSource;
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = global::PSHOP.Properties.Resources.me_bg;
            this.bindingNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bindingNavigator1.BindingSource = this.rpt_Sale_Monthly_Value_Visitor_CustomerBindingSource;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("Tahoma", 8.25F);
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
            this.toolStripSeparator1});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.bindingNavigator1.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.bindingNavigator1.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.bindingNavigator1.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(794, 25);
            this.bindingNavigator1.TabIndex = 1;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(36, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // cmb_ExportToExcel
            // 
            this.cmb_ExportToExcel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmb_ExportToExcel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_Excel_Table,
            this.mnu_Send_Chart});
            this.cmb_ExportToExcel.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.cmb_ExportToExcel.Image = ((System.Drawing.Image)(resources.GetObject("cmb_ExportToExcel.Image")));
            this.cmb_ExportToExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmb_ExportToExcel.Name = "cmb_ExportToExcel";
            this.cmb_ExportToExcel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmb_ExportToExcel.Size = new System.Drawing.Size(104, 22);
            this.cmb_ExportToExcel.Text = "ارسال به Excel";
            this.cmb_ExportToExcel.ToolTipText = "Ctrl+E";
            // 
            // mnu_Excel_Table
            // 
            this.mnu_Excel_Table.Name = "mnu_Excel_Table";
            this.mnu_Excel_Table.Size = new System.Drawing.Size(172, 22);
            this.mnu_Excel_Table.Text = "ارسال جدول به Excel";
            this.mnu_Excel_Table.Click += new System.EventHandler(this.mnu_Excel_Table_Click);
            // 
            // mnu_Send_Chart
            // 
            this.mnu_Send_Chart.Name = "mnu_Send_Chart";
            this.mnu_Send_Chart.Size = new System.Drawing.Size(172, 22);
            this.mnu_Send_Chart.Text = "ارسال نمودار به Excel";
            this.mnu_Send_Chart.Click += new System.EventHandler(this.mnu_Send_Chart_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // cmb_Print
            // 
            this.cmb_Print.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmb_Print.Image = global::PSHOP.Properties.Resources.Printer;
            this.cmb_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmb_Print.Name = "cmb_Print";
            this.cmb_Print.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmb_Print.Size = new System.Drawing.Size(46, 22);
            this.cmb_Print.Text = "چاپ";
            this.cmb_Print.Click += new System.EventHandler(this.mnu_PrintAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.AllowAutoHideAnimation = false;
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.Navy;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2010;
            this.uiPanel0.Id = new System.Guid("ac44a3c1-7a44-456e-a7d0-5a5b27f4aefb");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            this.uiPanel1.Id = new System.Guid("adcd5daa-0f83-403b-b5b7-a7d168e4fbc2");
            this.uiPanelManager1.Panels.Add(this.uiPanel1);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("ac44a3c1-7a44-456e-a7d0-5a5b27f4aefb"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(242, 397), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("adcd5daa-0f83-403b-b5b7-a7d168e4fbc2"), Janus.Windows.UI.Dock.PanelDockStyle.Bottom, new System.Drawing.Size(493, 160), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("ac44a3c1-7a44-456e-a7d0-5a5b27f4aefb"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("adcd5daa-0f83-403b-b5b7-a7d168e4fbc2"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(549, 28);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(242, 397);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "مسئولین فروش";
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.gridEX_Visitors);
            this.uiPanel0Container.Location = new System.Drawing.Point(5, 23);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(236, 373);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // gridEX_Visitors
            // 
            this.gridEX_Visitors.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Visitors.AlternatingColors = true;
            this.gridEX_Visitors.DataSource = this.rpt_Sale_MonthlyNumeric_VisitorBindingSource;
            this.gridEX_Visitors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Visitors.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Visitors.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Visitors.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Visitors.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX_Visitors.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX_Visitors.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Visitors.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Visitors.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Visitors.GroupByBoxVisible = false;
            gridEX_Visitors_Layout_0.DataSource = this.rpt_Sale_MonthlyNumeric_VisitorBindingSource;
            gridEX_Visitors_Layout_0.IsCurrentLayout = true;
            gridEX_Visitors_Layout_0.Key = "PERP";
            gridEX_Visitors_Layout_0.LayoutString = resources.GetString("gridEX_Visitors_Layout_0.LayoutString");
            this.gridEX_Visitors.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX_Visitors_Layout_0});
            this.gridEX_Visitors.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Visitors.Name = "gridEX_Visitors";
            this.gridEX_Visitors.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Visitors.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Visitors.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Visitors.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Visitors.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Visitors.Size = new System.Drawing.Size(236, 373);
            this.gridEX_Visitors.TabIndex = 21;
            this.gridEX_Visitors.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiPanel1
            // 
            this.uiPanel1.AutoHide = true;
            this.uiPanel1.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(3, 279);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(493, 160);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "جهت نمایش نمودار کالا، روی سطر مربوط دوبار کلیک کنید";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.crystalReportViewer1);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 27);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(491, 132);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.DisplayStatusBar = false;
            this.crystalReportViewer1.DisplayToolbar = false;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.EnableDrillDown = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.SelectionFormula = "";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowGotoPageButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowPageNavigateButtons = false;
            this.crystalReportViewer1.ShowParameterPanelButton = false;
            this.crystalReportViewer1.ShowRefreshButton = false;
            this.crystalReportViewer1.ShowTextSearchButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(491, 132);
            this.crystalReportViewer1.TabIndex = 6;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crystalReportViewer1.ViewTimeSelectionFormula = "";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "مسیر ذخیره سازی فایل";
            // 
            // gridEX_Goods
            // 
            this.gridEX_Goods.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Goods.AlternatingColors = true;
            this.gridEX_Goods.DataSource = this.rpt_Sale_Monthly_Value_Visitor_CustomerBindingSource;
            this.gridEX_Goods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Goods.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Goods.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Goods.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Goods.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX_Goods.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX_Goods.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Goods.GroupByBoxVisible = false;
            gridEX_Goods_Layout_0.DataSource = this.rpt_Sale_Monthly_Value_Visitor_CustomerBindingSource;
            gridEX_Goods_Layout_0.IsCurrentLayout = true;
            gridEX_Goods_Layout_0.Key = "PERP";
            gridEX_Goods_Layout_0.LayoutString = resources.GetString("gridEX_Goods_Layout_0.LayoutString");
            this.gridEX_Goods.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX_Goods_Layout_0});
            this.gridEX_Goods.Location = new System.Drawing.Point(3, 28);
            this.gridEX_Goods.Name = "gridEX_Goods";
            this.gridEX_Goods.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Goods.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Goods.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Goods.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.Size = new System.Drawing.Size(546, 397);
            this.gridEX_Goods.TabIndex = 21;
            this.gridEX_Goods.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Goods.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEX_Goods.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.gridEX_Goods_RowDoubleClick);
            // 
            // rpt_Sale_MonthlyNumeric_VisitorTableAdapter
            // 
            this.rpt_Sale_MonthlyNumeric_VisitorTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_Sale_Monthly_Value_Visitor_CustomerTableAdapter
            // 
            this.rpt_Sale_Monthly_Value_Visitor_CustomerTableAdapter.ClearBeforeFill = true;
            // 
            // Form05_SaleReport_Monthly_Value_Customer_Visitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.gridEX_Goods);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form05_SaleReport_Monthly_Value_Customer_Visitor";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "آمار ماهیانه فروش ریالی مشتریان بر اساس مسئولین فروش";
            this.Load += new System.EventHandler(this.Form04_SaleReport_NumericMonthly_Visitor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form04_SaleReport_NumericMonthly_Goods_Visitor_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.rpt_Sale_MonthlyNumeric_VisitorBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_Sale_Monthly_Value_Visitor_CustomerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Visitors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripDropDownButton cmb_ExportToExcel;
        private System.Windows.Forms.ToolStripMenuItem mnu_Excel_Table;
        private System.Windows.Forms.ToolStripMenuItem mnu_Send_Chart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private DataSet_Report dataSet_Report;
        private Janus.Windows.GridEX.GridEX gridEX_Visitors;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.ToolStripButton cmb_Print;
        private Janus.Windows.GridEX.GridEX gridEX_Goods;
        private System.Windows.Forms.BindingSource rpt_Sale_MonthlyNumeric_VisitorBindingSource;
        private DataSet_ReportTableAdapters.Rpt_Sale_MonthlyNumeric_VisitorTableAdapter rpt_Sale_MonthlyNumeric_VisitorTableAdapter;
        private System.Windows.Forms.BindingSource rpt_Sale_Monthly_Value_Visitor_CustomerBindingSource;
        private DataSet_ReportTableAdapters.Rpt_Sale_Monthly_Value_Visitor_CustomerTableAdapter rpt_Sale_Monthly_Value_Visitor_CustomerTableAdapter;
    }
}