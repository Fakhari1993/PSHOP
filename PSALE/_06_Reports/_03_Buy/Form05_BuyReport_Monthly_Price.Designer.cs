namespace PSHOP._06_Reports._03_Buy
{
    partial class Form05_BuyReport_Monthly_Price
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form05_BuyReport_Monthly_Price));
            Janus.Windows.GridEX.GridEXLayout gridEX_Goods_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
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
            this.cmb_Print = new System.Windows.Forms.ToolStripDropDownButton();
            this.mnu_PrintTable = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_PrintChart = new System.Windows.Forms.ToolStripMenuItem();
            this.mnu_PrintBoth = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.gridEX_Goods = new Janus.Windows.GridEX.GridEX();
            this.dataSet_Report = new PSHOP._06_Reports.DataSet_Report();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).BeginInit();
            this.SuspendLayout();
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = global::PSHOP.Properties.Resources.me_bg;
            resources.ApplyResources(this.bindingNavigator1, "bindingNavigator1");
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
            this.cmb_Print});
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
            this.mnu_PrintTable,
            this.mnu_PrintChart,
            this.mnu_PrintBoth});
            this.cmb_Print.Image = global::PSHOP.Properties.Resources.Printer;
            resources.ApplyResources(this.cmb_Print, "cmb_Print");
            this.cmb_Print.Name = "cmb_Print";
            // 
            // mnu_PrintTable
            // 
            resources.ApplyResources(this.mnu_PrintTable, "mnu_PrintTable");
            this.mnu_PrintTable.Name = "mnu_PrintTable";
            this.mnu_PrintTable.Click += new System.EventHandler(this.mnu_PrintTable_Click);
            // 
            // mnu_PrintChart
            // 
            resources.ApplyResources(this.mnu_PrintChart, "mnu_PrintChart");
            this.mnu_PrintChart.Name = "mnu_PrintChart";
            this.mnu_PrintChart.Click += new System.EventHandler(this.mnu_PrintChart_Click);
            // 
            // mnu_PrintBoth
            // 
            resources.ApplyResources(this.mnu_PrintBoth, "mnu_PrintBoth");
            this.mnu_PrintBoth.Name = "mnu_PrintBoth";
            this.mnu_PrintBoth.Click += new System.EventHandler(this.mnu_PrintBoth_Click);
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
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Black;
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.DarkBlue;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2010;
            this.uiPanel0.Id = new System.Guid("c1e852ec-e270-40e7-b720-4371764bb6b2");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("c1e852ec-e270-40e7-b720-4371764bb6b2"), Janus.Windows.UI.Dock.PanelDockStyle.Bottom, new System.Drawing.Size(788, 355), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("c1e852ec-e270-40e7-b720-4371764bb6b2"), new System.Drawing.Point(21, 562), new System.Drawing.Size(654, 200), false);
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
            // gridEX_Goods
            // 
            this.gridEX_Goods.AllowColumnDrag = false;
            this.gridEX_Goods.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Goods.AlternatingColors = true;
            this.gridEX_Goods.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX_Goods.ColumnHeaders = Janus.Windows.GridEX.InheritableBoolean.False;
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
            this.gridEX_Goods.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEX_Goods.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.gridEX_Accounts_RowDoubleClick);
            // 
            // dataSet_Report
            // 
            this.dataSet_Report.DataSetName = "DataSet_Report";
            this.dataSet_Report.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Form05_BuyReport_Monthly_Price
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gridEX_Goods);
            this.Controls.Add(this.bindingNavigator1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form05_BuyReport_Monthly_Price";
            this.Activated += new System.EventHandler(this.Form04_SaleReport_NumericMonthly_Activated);
            this.Load += new System.EventHandler(this.Form04_SaleReport_NumericMonthly_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form04_SaleReport_NumericMonthly_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).EndInit();
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
        private System.Windows.Forms.ToolStripDropDownButton cmb_Print;
        private System.Windows.Forms.ToolStripMenuItem mnu_PrintTable;
        private System.Windows.Forms.ToolStripMenuItem mnu_PrintChart;
        private DataSet_Report dataSet_Report;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.ToolStripMenuItem mnu_PrintBoth;
    }
}