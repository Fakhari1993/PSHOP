namespace PSHOP._06_Reports._02_Sale
{
    partial class Form23_VarianceSaleAndACC
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form23_VarianceSaleAndACC));
            Janus.Windows.GridEX.GridEXLayout gridEX_Headers_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX_Variance_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.bt_ExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Display = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel3 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel3Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_Headers = new Janus.Windows.GridEX.GridEX();
            this.uiPanel4 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel4Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_Variance = new Janus.Windows.GridEX.GridEX();
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).BeginInit();
            this.uiPanel3.SuspendLayout();
            this.uiPanel3Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Headers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel4)).BeginInit();
            this.uiPanel4.SuspendLayout();
            this.uiPanel4Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Variance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("bindingNavigator1.BackgroundImage")));
            this.bindingNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.faDatePickerStrip1,
            this.toolStripLabel2,
            this.faDatePickerStrip2,
            this.bt_ExportToExcel,
            this.toolStripSeparator2,
            this.bt_Display,
            this.toolStripSeparator3});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(794, 25);
            this.bindingNavigator1.TabIndex = 2;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabel1.Text = "از تاریخ:";
            // 
            // faDatePickerStrip1
            // 
            this.faDatePickerStrip1.BackColor = System.Drawing.SystemColors.Window;
            this.faDatePickerStrip1.Name = "faDatePickerStrip1";
            this.faDatePickerStrip1.Size = new System.Drawing.Size(120, 22);
            this.faDatePickerStrip1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePickerStrip1_KeyPress);
            this.faDatePickerStrip1.TextChanged += new System.EventHandler(this.faDatePickerStrip1_TextChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(41, 22);
            this.toolStripLabel2.Text = "تا تاریخ:";
            // 
            // faDatePickerStrip2
            // 
            this.faDatePickerStrip2.BackColor = System.Drawing.SystemColors.Window;
            this.faDatePickerStrip2.Name = "faDatePickerStrip2";
            this.faDatePickerStrip2.Size = new System.Drawing.Size(120, 22);
            this.faDatePickerStrip2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePickerStrip2_KeyPress);
            this.faDatePickerStrip2.TextChanged += new System.EventHandler(this.faDatePickerStrip1_TextChanged);
            // 
            // bt_ExportToExcel
            // 
            this.bt_ExportToExcel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_ExportToExcel.Image = ((System.Drawing.Image)(resources.GetObject("bt_ExportToExcel.Image")));
            this.bt_ExportToExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_ExportToExcel.Name = "bt_ExportToExcel";
            this.bt_ExportToExcel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bt_ExportToExcel.Size = new System.Drawing.Size(23, 22);
            this.bt_ExportToExcel.ToolTipText = "Excel ارسال به";
            this.bt_ExportToExcel.Click += new System.EventHandler(this.bt_ExportToExcel_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_Display
            // 
            this.bt_Display.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_Display.Image = ((System.Drawing.Image)(resources.GetObject("bt_Display.Image")));
            this.bt_Display.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Display.Name = "bt_Display";
            this.bt_Display.Size = new System.Drawing.Size(23, 22);
            this.bt_Display.Text = "مشاهده";
            this.bt_Display.ToolTipText = "Ctrl+D";
            this.bt_Display.Click += new System.EventHandler(this.bt_Display_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(64)))), ((int)(((byte)(120)))));
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2010;
            this.uiPanel1.Id = new System.Guid("17bcff73-2506-4c0a-bf0d-b884c004d68e");
            this.uiPanel1.StaticGroup = true;
            this.uiPanel4.Id = new System.Guid("88f0707f-02a4-4ac5-b816-0fd5810a10b8");
            this.uiPanel1.Panels.Add(this.uiPanel4);
            this.uiPanel3.Id = new System.Guid("4c1e8672-b380-45d2-ab93-1e1de529c2c6");
            this.uiPanel1.Panels.Add(this.uiPanel3);
            this.uiPanelManager1.Panels.Add(this.uiPanel1);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("17bcff73-2506-4c0a-bf0d-b884c004d68e"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(788, 421), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("88f0707f-02a4-4ac5-b816-0fd5810a10b8"), new System.Guid("17bcff73-2506-4c0a-bf0d-b884c004d68e"), 199, true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("4c1e8672-b380-45d2-ab93-1e1de529c2c6"), new System.Guid("17bcff73-2506-4c0a-bf0d-b884c004d68e"), 199, true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("4934217e-8579-434b-9dd1-ca914b2e6294"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(724, 399), new System.Drawing.Size(200, 200), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("17bcff73-2506-4c0a-bf0d-b884c004d68e"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("4c1e8672-b380-45d2-ab93-1e1de529c2c6"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("88f0707f-02a4-4ac5-b816-0fd5810a10b8"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel1
            // 
            this.uiPanel1.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.Location = new System.Drawing.Point(3, 28);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(788, 421);
            this.uiPanel1.TabAlignment = Janus.Windows.UI.Dock.TabAlignment.Top;
            this.uiPanel1.TabIndex = 4;
            // 
            // uiPanel3
            // 
            this.uiPanel3.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel3.InnerContainer = this.uiPanel3Container;
            this.uiPanel3.Location = new System.Drawing.Point(0, 213);
            this.uiPanel3.Name = "uiPanel3";
            this.uiPanel3.Size = new System.Drawing.Size(788, 208);
            this.uiPanel3.TabIndex = 4;
            this.uiPanel3.Text = "انتخاب سرفصلهای مربوطه";
            this.uiPanel3.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel3Container
            // 
            this.uiPanel3Container.Controls.Add(this.gridEX_Headers);
            this.uiPanel3Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel3Container.Name = "uiPanel3Container";
            this.uiPanel3Container.Size = new System.Drawing.Size(786, 184);
            this.uiPanel3Container.TabIndex = 0;
            // 
            // gridEX_Headers
            // 
            this.gridEX_Headers.AlternatingColors = true;
            this.gridEX_Headers.AutomaticSort = false;
            this.gridEX_Headers.BuiltInTextsData = resources.GetString("gridEX_Headers.BuiltInTextsData");
            this.gridEX_Headers.CardWidth = 751;
            gridEX_Headers_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Headers_DesignTimeLayout.LayoutString");
            this.gridEX_Headers.DesignTimeLayout = gridEX_Headers_DesignTimeLayout;
            this.gridEX_Headers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Headers.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Headers.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Headers.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridEX_Headers.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX_Headers.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Headers.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Headers.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Headers.GroupByBoxVisible = false;
            this.gridEX_Headers.GroupTotals = Janus.Windows.GridEX.GroupTotals.ExpandedGroup;
            this.gridEX_Headers.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX_Headers.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Headers.Name = "gridEX_Headers";
            this.gridEX_Headers.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Headers.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Headers.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Silver;
            this.gridEX_Headers.OfficeCustomColor = System.Drawing.Color.Black;
            this.gridEX_Headers.RecordNavigator = true;
            this.gridEX_Headers.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Headers.SettingsKey = "Form14_CustomerGoodsReport4";
            this.gridEX_Headers.Size = new System.Drawing.Size(786, 184);
            this.gridEX_Headers.TabIndex = 9;
            this.gridEX_Headers.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Headers.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Headers.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEX_Headers.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX_Headers_CellValueChanged);
            this.gridEX_Headers.CancelingCellEdit += new Janus.Windows.GridEX.ColumnActionCancelEventHandler(this.gridEX_Headers_CancelingCellEdit);
            this.gridEX_Headers.CellUpdated += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX_Headers_CellUpdated);
            // 
            // uiPanel4
            // 
            this.uiPanel4.InnerContainer = this.uiPanel4Container;
            this.uiPanel4.Location = new System.Drawing.Point(0, 0);
            this.uiPanel4.Name = "uiPanel4";
            this.uiPanel4.Size = new System.Drawing.Size(788, 209);
            this.uiPanel4.TabIndex = 4;
            this.uiPanel4.Text = "صورت مغایرت";
            this.uiPanel4.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel4Container
            // 
            this.uiPanel4Container.Controls.Add(this.gridEX_Variance);
            this.uiPanel4Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel4Container.Name = "uiPanel4Container";
            this.uiPanel4Container.Size = new System.Drawing.Size(786, 185);
            this.uiPanel4Container.TabIndex = 0;
            // 
            // gridEX_Variance
            // 
            this.gridEX_Variance.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Variance.AlternatingColors = true;
            this.gridEX_Variance.BuiltInTextsData = resources.GetString("gridEX_Variance.BuiltInTextsData");
            this.gridEX_Variance.CardWidth = 751;
            gridEX_Variance_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Variance_DesignTimeLayout.LayoutString");
            this.gridEX_Variance.DesignTimeLayout = gridEX_Variance_DesignTimeLayout;
            this.gridEX_Variance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Variance.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Variance.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Variance.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridEX_Variance.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX_Variance.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Variance.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Variance.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Variance.GroupByBoxVisible = false;
            this.gridEX_Variance.GroupTotals = Janus.Windows.GridEX.GroupTotals.ExpandedGroup;
            this.gridEX_Variance.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX_Variance.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Variance.Name = "gridEX_Variance";
            this.gridEX_Variance.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Variance.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Variance.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Silver;
            this.gridEX_Variance.OfficeCustomColor = System.Drawing.Color.Black;
            this.gridEX_Variance.RecordNavigator = true;
            this.gridEX_Variance.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Variance.SettingsKey = "Form14_CustomerGoodsReport4";
            this.gridEX_Variance.Size = new System.Drawing.Size(786, 185);
            this.gridEX_Variance.TabIndex = 10;
            this.gridEX_Variance.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Variance.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Variance.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiPanel0
            // 
            this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(4, 0);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(312, 210);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "انتخاب حسابهای فروش";
            this.uiPanel0.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Location = new System.Drawing.Point(0, 0);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(312, 210);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(4, 214);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(312, 207);
            this.uiPanel2.TabIndex = 4;
            this.uiPanel2.Text = "انتخاب حسابهای تخفیفات";
            this.uiPanel2.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Location = new System.Drawing.Point(0, 0);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(312, 207);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "مسیر ذخیره سازی فایل";
            // 
            // Form23_VarianceSaleAndACC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form23_VarianceSaleAndACC";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "مغایرت حسابداری و فروش";
            this.Load += new System.EventHandler(this.Form23_VarianceSaleAndACC_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form23_VarianceSaleAndACC_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel3)).EndInit();
            this.uiPanel3.ResumeLayout(false);
            this.uiPanel3Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Headers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel4)).EndInit();
            this.uiPanel4.ResumeLayout(false);
            this.uiPanel4Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Variance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton bt_Display;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel2;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private System.Windows.Forms.ToolStripButton bt_ExportToExcel;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanel1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel3;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel3Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel4;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel4Container;
        private Janus.Windows.GridEX.GridEX gridEX_Headers;
        private Janus.Windows.GridEX.GridEX gridEX_Variance;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}