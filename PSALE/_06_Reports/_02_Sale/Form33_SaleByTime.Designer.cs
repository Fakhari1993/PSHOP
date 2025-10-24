namespace PSHOP._06_Reports._02_Sale
{
    partial class Form33_SaleByTime
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
            Janus.Windows.GridEX.GridEXLayout gridEX2_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form33_SaleByTime));
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.bt_Print = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnu_ExportToExcel_Header = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Search = new System.Windows.Forms.ToolStripButton();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.gridEXPrintDocument1 = new Janus.Windows.GridEX.GridEXPrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX2 = new Janus.Windows.GridEX.GridEX();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
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
            this.toolStripLabel1,
            this.faDatePickerStrip1,
            this.toolStripSeparator1,
            this.toolStripLabel3,
            this.faDatePickerStrip2,
            this.bt_Print,
            this.toolStripSeparator2,
            this.mnu_ExportToExcel_Header,
            this.toolStripSeparator3,
            this.bt_Search});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(714, 25);
            this.bindingNavigator1.TabIndex = 15;
            this.bindingNavigator1.Text = "bindingNavigator2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLabel1.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabel1.Text = "از تاریخ:";
            // 
            // faDatePickerStrip1
            // 
            this.faDatePickerStrip1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.faDatePickerStrip1.BackColor = System.Drawing.SystemColors.Window;
            this.faDatePickerStrip1.Name = "faDatePickerStrip1";
            this.faDatePickerStrip1.Size = new System.Drawing.Size(100, 22);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLabel3.Size = new System.Drawing.Size(41, 22);
            this.toolStripLabel3.Text = "تا تاریخ:";
            // 
            // faDatePickerStrip2
            // 
            this.faDatePickerStrip2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.faDatePickerStrip2.BackColor = System.Drawing.SystemColors.Window;
            this.faDatePickerStrip2.Name = "faDatePickerStrip2";
            this.faDatePickerStrip2.Size = new System.Drawing.Size(100, 22);
            // 
            // bt_Print
            // 
            this.bt_Print.Image = global::PSHOP.Properties.Resources.Printer;
            this.bt_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Print.Name = "bt_Print";
            this.bt_Print.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_Print.Size = new System.Drawing.Size(46, 22);
            this.bt_Print.Text = "چاپ";
            this.bt_Print.Click += new System.EventHandler(this.bt_Print_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // mnu_ExportToExcel_Header
            // 
            this.mnu_ExportToExcel_Header.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnu_ExportToExcel_Header.Image = global::PSHOP.Properties.Resources.Excel;
            this.mnu_ExportToExcel_Header.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnu_ExportToExcel_Header.Name = "mnu_ExportToExcel_Header";
            this.mnu_ExportToExcel_Header.Size = new System.Drawing.Size(23, 22);
            this.mnu_ExportToExcel_Header.Text = "ارسال به Excel";
            this.mnu_ExportToExcel_Header.Click += new System.EventHandler(this.mnu_ExportToExcel_Header_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_Search
            // 
            this.bt_Search.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_Search.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_Search.Image = global::PSHOP.Properties.Resources.Search;
            this.bt_Search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Search.Name = "bt_Search";
            this.bt_Search.Size = new System.Drawing.Size(23, 22);
            this.bt_Search.Text = "مشاهده";
            this.bt_Search.Click += new System.EventHandler(this.bt_Search_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.gridEXPrintDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // gridEXPrintDocument1
            // 
            this.gridEXPrintDocument1.FitColumns = Janus.Windows.GridEX.FitColumnsMode.SizingColumns;
            this.gridEXPrintDocument1.GridEX = this.gridEX2;
            this.gridEXPrintDocument1.PageHeaderCenter = " ";
            this.gridEXPrintDocument1.PageHeaderFormatStyle.Font = new System.Drawing.Font("B Titr", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.gridEXPrintDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // pageSetupDialog1
            // 
            this.pageSetupDialog1.Document = this.gridEXPrintDocument1;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Excel files|*.xls;*.xlsx";
            // 
            // gridEXExporter1
            // 
            this.gridEXExporter1.GridEX = this.gridEX2;
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.DefaultPanelSettings.AutoHideButtonVisible = false;
            this.uiPanelManager1.DefaultPanelSettings.CloseButtonVisible = false;
            this.uiPanelManager1.DockingStyleMode = Janus.Windows.UI.Dock.DockingStyleMode.Standard;
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.Black;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.VS2010;
            this.uiPanel1.Id = new System.Guid("d220d43c-1a68-46fc-9246-f4a73117fa11");
            this.uiPanelManager1.Panels.Add(this.uiPanel1);
            this.uiPanel0.Id = new System.Guid("7f3ac644-990e-4084-8ecf-9358fd4ff7b3");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("7f3ac644-990e-4084-8ecf-9358fd4ff7b3"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(686, 421), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("d220d43c-1a68-46fc-9246-f4a73117fa11"), Janus.Windows.UI.Dock.PanelDockStyle.Left, new System.Drawing.Size(501, 421), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("7f3ac644-990e-4084-8ecf-9358fd4ff7b3"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("d220d43c-1a68-46fc-9246-f4a73117fa11"), new System.Drawing.Point(146, 502), new System.Drawing.Size(200, 200), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(25, 28);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(686, 421);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "Panel 0";
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.gridEX2);
            this.uiPanel0Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(684, 419);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // gridEX2
            // 
            this.gridEX2.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX2.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.AlternatingColors = true;
            this.gridEX2.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX2.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX2.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX2.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX2.FilterRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX2.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX2.GroupByBoxVisible = false;
            gridEX2_Layout_0.IsCurrentLayout = true;
            gridEX2_Layout_0.Key = "PERP";
            gridEX2_Layout_0.LayoutString = resources.GetString("gridEX2_Layout_0.LayoutString");
            this.gridEX2.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX2_Layout_0});
            this.gridEX2.Location = new System.Drawing.Point(0, 0);
            this.gridEX2.Name = "gridEX2";
            this.gridEX2.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX2.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX2.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX2.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX2.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX2.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.Size = new System.Drawing.Size(684, 419);
            this.gridEX2.TabIndex = 19;
            this.gridEX2.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.TotalRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX2.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridEX2.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX2.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiPanel1
            // 
            this.uiPanel1.AutoHide = true;
            this.uiPanel1.FloatingLocation = new System.Drawing.Point(146, 502);
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(3, 3);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(501, 421);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "نمودار";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.chart1);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 21);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(495, 399);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Fill;
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(495, 399);
            this.chart1.TabIndex = 1;
            this.chart1.Text = "chart1";
            // 
            // Form33_SaleByTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 452);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form33_SaleByTime";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "گزارش زمانی فروش";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form33_SaleByTime_FormClosing);
            this.Load += new System.EventHandler(this.Form33_SaleByTime_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip2;
        private System.Windows.Forms.ToolStripButton bt_Print;
        private System.Windows.Forms.ToolStripButton mnu_ExportToExcel_Header;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton bt_Search;
        private System.Windows.Forms.PrintDialog printDialog1;
        private Janus.Windows.GridEX.GridEXPrintDocument gridEXPrintDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.GridEX.GridEX gridEX2;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
    }
}