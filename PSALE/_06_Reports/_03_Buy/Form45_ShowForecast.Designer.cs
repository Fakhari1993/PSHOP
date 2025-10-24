namespace PSHOP._06_Reports._03_Buy
{
    partial class Form45_ShowForecast
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form45_ShowForecast));
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX_Factors_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.gridEXPrintDocument1 = new Janus.Windows.GridEX.GridEXPrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.mnu_ExportToExcel_Header = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Print = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.btn_Forecast = new System.Windows.Forms.ToolStripButton();
            this.printPreviewDialog2 = new System.Windows.Forms.PrintPreviewDialog();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.filterEditor1 = new Janus.Windows.FilterEditor.FilterEditor();
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_Factors = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Factors)).BeginInit();
            this.SuspendLayout();
            // 
            // pageSetupDialog1
            // 
            this.pageSetupDialog1.Document = this.gridEXPrintDocument1;
            // 
            // gridEXPrintDocument1
            // 
            this.gridEXPrintDocument1.FitColumns = Janus.Windows.GridEX.FitColumnsMode.SizingColumns;
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
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "مسیر ذخیره سازی فایل";
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.gridEXPrintDocument1;
            this.printDialog1.UseEXDialog = true;
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
            this.mnu_ExportToExcel_Header,
            this.toolStripSeparator2,
            this.bt_Print,
            this.toolStripSeparator1,
            this.toolStripButton1,
            this.toolStripLabel1,
            this.faDatePickerStrip1,
            this.toolStripLabel2,
            this.faDatePickerStrip2,
            this.btn_Forecast});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(704, 25);
            this.bindingNavigator1.TabIndex = 14;
            this.bindingNavigator1.Text = "bindingNavigator2";
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_Print
            // 
            this.bt_Print.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Print.Image = ((System.Drawing.Image)(resources.GetObject("bt_Print.Image")));
            this.bt_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Print.Name = "bt_Print";
            this.bt_Print.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_Print.Size = new System.Drawing.Size(46, 22);
            this.bt_Print.Text = "چاپ";
            this.bt_Print.Click += new System.EventHandler(this.bt_Print_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripButton1.Size = new System.Drawing.Size(96, 22);
            this.toolStripButton1.Text = "درخواست خرید";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
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
            this.faDatePickerStrip1.Size = new System.Drawing.Size(120, 22);
            this.faDatePickerStrip1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePickerStrip1_KeyPress);
            this.faDatePickerStrip1.TextChanged += new System.EventHandler(this.faDatePickerStrip1_TextChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLabel2.Size = new System.Drawing.Size(38, 22);
            this.toolStripLabel2.Text = "تاتاریخ:";
            // 
            // faDatePickerStrip2
            // 
            this.faDatePickerStrip2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.faDatePickerStrip2.BackColor = System.Drawing.SystemColors.Window;
            this.faDatePickerStrip2.Name = "faDatePickerStrip2";
            this.faDatePickerStrip2.Size = new System.Drawing.Size(120, 22);
            this.faDatePickerStrip2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePickerStrip2_KeyPress);
            this.faDatePickerStrip2.TextChanged += new System.EventHandler(this.faDatePickerStrip2_TextChanged);
            // 
            // btn_Forecast
            // 
            this.btn_Forecast.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_Forecast.Image = ((System.Drawing.Image)(resources.GetObject("btn_Forecast.Image")));
            this.btn_Forecast.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Forecast.Name = "btn_Forecast";
            this.btn_Forecast.Size = new System.Drawing.Size(73, 22);
            this.btn_Forecast.Text = "پیش بینی";
            this.btn_Forecast.Click += new System.EventHandler(this.btn_Forecast_Click);
            // 
            // printPreviewDialog2
            // 
            this.printPreviewDialog2.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog2.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog2.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog2.Document = this.gridEXPrintDocument1;
            this.printPreviewDialog2.Enabled = true;
            this.printPreviewDialog2.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog2.Icon")));
            this.printPreviewDialog2.Name = "printPreviewDialog1";
            this.printPreviewDialog2.Visible = false;
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.VS2010;
            this.uiPanel1.Id = new System.Guid("2c345b71-47c9-460f-8fd5-a29537a181c7");
            this.uiPanelManager1.Panels.Add(this.uiPanel1);
            this.uiPanel0.Id = new System.Guid("6c92de07-5309-4dcf-9d82-1a8f0e6ce42c");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("6c92de07-5309-4dcf-9d82-1a8f0e6ce42c"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(676, 411), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("2c345b71-47c9-460f-8fd5-a29537a181c7"), Janus.Windows.UI.Dock.PanelDockStyle.Left, new System.Drawing.Size(349, 411), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("6c92de07-5309-4dcf-9d82-1a8f0e6ce42c"), new System.Drawing.Point(552, 397), new System.Drawing.Size(200, 200), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("2c345b71-47c9-460f-8fd5-a29537a181c7"), new System.Drawing.Point(17, 374), new System.Drawing.Size(200, 200), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel1
            // 
            this.uiPanel1.AutoHide = true;
            this.uiPanel1.AutoHideButtonVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanel1.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.FloatingLocation = new System.Drawing.Point(17, 374);
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(3, 3);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(349, 411);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "جزئیات";
            this.uiPanel1.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            this.uiPanel1.Click += new System.EventHandler(this.uiPanel1_Click);
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.gridEX1);
            this.uiPanel1Container.Controls.Add(this.filterEditor1);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 21);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(343, 389);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // gridEX1
            // 
            this.gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX1.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.BuiltInTextsData = resources.GetString("gridEX1.BuiltInTextsData");
            this.gridEX1.CardWidth = 751;
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX1.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX1.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX1.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX1.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.GroupByBoxVisible = false;
            this.gridEX1.GroupRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX1.GroupRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX1.GroupRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.GroupRowFormatStyle.Font = new System.Drawing.Font("B Traffic", 9F, System.Drawing.FontStyle.Bold);
            this.gridEX1.GroupRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.gridEX1.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.UseRowStyle;
            this.gridEX1.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX1.GroupTotalRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX1.GroupTotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.GroupTotals = Janus.Windows.GridEX.GroupTotals.ExpandedGroup;
            this.gridEX1.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX1.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX1.Location = new System.Drawing.Point(0, 45);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX1.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX1.RecordNavigator = true;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.SettingsKey = "Form10_CompReport_Factor02";
            this.gridEX1.Size = new System.Drawing.Size(343, 344);
            this.gridEX1.TabIndex = 19;
            this.gridEX1.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.White;
            this.gridEX1.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX1.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // filterEditor1
            // 
            this.filterEditor1.AllowFilterByFieldValue = true;
            this.filterEditor1.AllowNegateRootCondition = true;
            this.filterEditor1.AutoApply = true;
            this.filterEditor1.AutomaticHeightResize = true;
            this.filterEditor1.BackColor = System.Drawing.Color.Transparent;
            this.filterEditor1.BuiltInTextsData = resources.GetString("filterEditor1.BuiltInTextsData");
            this.filterEditor1.ColorScheme = "FilterEditorColorScheme";
            this.filterEditor1.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterEditor1.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.UseFormatStyle;
            this.filterEditor1.Location = new System.Drawing.Point(0, 0);
            this.filterEditor1.MinSize = new System.Drawing.Size(16, 45);
            this.filterEditor1.Name = "filterEditor1";
            this.filterEditor1.OfficeColorScheme = Janus.Windows.Common.OfficeColorScheme.Custom;
            this.filterEditor1.OfficeCustomColor = System.Drawing.Color.SkyBlue;
            this.filterEditor1.ScrollMode = Janus.Windows.UI.Dock.ScrollMode.Both;
            this.filterEditor1.ScrollStep = 15;
            this.filterEditor1.Size = new System.Drawing.Size(343, 45);
            this.filterEditor1.SourceControl = this.gridEX1;
            this.filterEditor1.VisualStyle = Janus.Windows.Common.VisualStyle.Office2007;
            // 
            // uiPanel0
            // 
            this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.FloatingLocation = new System.Drawing.Point(552, 397);
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(25, 28);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(676, 411);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "Panel 0";
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.gridEX_Factors);
            this.uiPanel0Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(674, 409);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // gridEX_Factors
            // 
            this.gridEX_Factors.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Factors.AlternatingColors = true;
            this.gridEX_Factors.BuiltInTextsData = resources.GetString("gridEX_Factors.BuiltInTextsData");
            this.gridEX_Factors.CardWidth = 751;
            gridEX_Factors_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Factors_DesignTimeLayout.LayoutString");
            this.gridEX_Factors.DesignTimeLayout = gridEX_Factors_DesignTimeLayout;
            this.gridEX_Factors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Factors.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Factors.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Factors.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Factors.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Factors.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_Factors.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Factors.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Factors.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Factors.GroupByBoxVisible = false;
            this.gridEX_Factors.GroupRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Factors.GroupRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_Factors.GroupRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Factors.GroupRowFormatStyle.Font = new System.Drawing.Font("B Traffic", 9F, System.Drawing.FontStyle.Bold);
            this.gridEX_Factors.GroupRowFormatStyle.FontBold = Janus.Windows.GridEX.TriState.True;
            this.gridEX_Factors.GroupRowVisualStyle = Janus.Windows.GridEX.GroupRowVisualStyle.UseRowStyle;
            this.gridEX_Factors.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Factors.GroupTotalRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_Factors.GroupTotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Factors.GroupTotals = Janus.Windows.GridEX.GroupTotals.ExpandedGroup;
            this.gridEX_Factors.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX_Factors.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX_Factors.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Factors.Name = "gridEX_Factors";
            this.gridEX_Factors.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Factors.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Factors.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Factors.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Factors.RecordNavigator = true;
            this.gridEX_Factors.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Factors.SettingsKey = "Form10_CompReport_Factor02";
            this.gridEX_Factors.Size = new System.Drawing.Size(674, 409);
            this.gridEX_Factors.TabIndex = 17;
            this.gridEX_Factors.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Factors.TotalRowFormatStyle.BackColor = System.Drawing.Color.White;
            this.gridEX_Factors.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Factors.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Factors.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Factors.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.gridEX_Factors.SelectionChanged += new System.EventHandler(this.gridEX_Factors_SelectionChanged);
            // 
            // Form45_ShowForecast
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 442);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form45_ShowForecast";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "پیش بینی خرید کالاها";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form44_ShowForecast_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Factors)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton mnu_ExportToExcel_Header;
        private System.Windows.Forms.ToolStripButton bt_Print;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private Janus.Windows.GridEX.GridEXPrintDocument gridEXPrintDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PrintDialog printDialog1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog2;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.GridEX.GridEX gridEX_Factors;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private Janus.Windows.FilterEditor.FilterEditor filterEditor1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton btn_Forecast;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip2;
    }
}