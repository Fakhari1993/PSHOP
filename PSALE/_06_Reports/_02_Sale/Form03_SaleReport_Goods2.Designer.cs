namespace PSHOP._06_Reports._02_Sale
{
    partial class Form03_SaleReport_Goods2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form03_SaleReport_Goods2));
            Janus.Windows.GridEX.GridEXLayout gridEX_Goods_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.rpt_SaleFactor_FactorsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rpt_SaleFactor_CustomerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rpt_SaleFactor_GoodBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_Sale = new PSHOP._05_Sale.DataSet_Sale();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnu_ExportToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.cmb_Cancel = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.bt_Search = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.dataSet_Report = new PSHOP._06_Reports.DataSet_Report();
            this.rpt_SaleFactor_GoodTableAdapter = new PSHOP._05_Sale.DataSet_SaleTableAdapters.Rpt_SaleFactor_GoodTableAdapter();
            this.rpt_SaleFactor_CustomerTableAdapter = new PSHOP._05_Sale.DataSet_SaleTableAdapters.Rpt_SaleFactor_CustomerTableAdapter();
            this.rpt_SaleFactor_FactorsTableAdapter = new PSHOP._05_Sale.DataSet_SaleTableAdapters.Rpt_SaleFactor_FactorsTableAdapter();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_Goods = new Janus.Windows.GridEX.GridEX();
            this.contextMenuStrip3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_SaleFactor_FactorsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_SaleFactor_CustomerBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_SaleFactor_GoodBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale)).BeginInit();
            this.contextMenuStrip2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem2});
            this.contextMenuStrip3.Name = "contextMenuStrip1";
            this.contextMenuStrip3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.contextMenuStrip3.Size = new System.Drawing.Size(143, 26);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem2.Image")));
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(142, 22);
            this.toolStripMenuItem2.Text = "ارسال به Excel";
            // 
            // rpt_SaleFactor_FactorsBindingSource
            // 
            this.rpt_SaleFactor_FactorsBindingSource.DataMember = "FK_Rpt_SaleFactor_Customer_Rpt_SaleFactor_Factors";
            this.rpt_SaleFactor_FactorsBindingSource.DataSource = this.rpt_SaleFactor_CustomerBindingSource;
            // 
            // rpt_SaleFactor_CustomerBindingSource
            // 
            this.rpt_SaleFactor_CustomerBindingSource.DataMember = "FK_Rpt_SaleFactor_Good_Rpt_SaleFactor_Customer";
            this.rpt_SaleFactor_CustomerBindingSource.DataSource = this.rpt_SaleFactor_GoodBindingSource;
            // 
            // rpt_SaleFactor_GoodBindingSource
            // 
            this.rpt_SaleFactor_GoodBindingSource.DataMember = "Rpt_SaleFactor_Good";
            this.rpt_SaleFactor_GoodBindingSource.DataSource = this.dataSet_Sale;
            // 
            // dataSet_Sale
            // 
            this.dataSet_Sale.DataSetName = "DataSet_Sale";
            this.dataSet_Sale.EnforceConstraints = false;
            this.dataSet_Sale.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.contextMenuStrip2.Size = new System.Drawing.Size(143, 26);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripMenuItem1.Image")));
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(142, 22);
            this.toolStripMenuItem1.Text = "ارسال به Excel";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnu_ExportToExcel});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.contextMenuStrip1.Size = new System.Drawing.Size(143, 26);
            // 
            // mnu_ExportToExcel
            // 
            this.mnu_ExportToExcel.Image = ((System.Drawing.Image)(resources.GetObject("mnu_ExportToExcel.Image")));
            this.mnu_ExportToExcel.Name = "mnu_ExportToExcel";
            this.mnu_ExportToExcel.Size = new System.Drawing.Size(142, 22);
            this.mnu_ExportToExcel.Text = "ارسال به Excel";
            this.mnu_ExportToExcel.Click += new System.EventHandler(this.mnu_ExportToExcel_Click);
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
            this.toolStripSeparator8,
            this.cmb_Cancel,
            this.toolStripLabel1,
            this.faDatePickerStrip1,
            this.toolStripLabel2,
            this.faDatePickerStrip2,
            this.bt_Search,
            this.toolStripSeparator1});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(794, 25);
            this.bindingNavigator1.TabIndex = 12;
            this.bindingNavigator1.Text = "bindingNavigator2";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // cmb_Cancel
            // 
            this.cmb_Cancel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmb_Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.cmb_Cancel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Cancel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_Cancel.Items.AddRange(new object[] {
            "تمام فاکتورها",
            "فاکتورهای باطل شده",
            "فاکتورهای باطل نشده"});
            this.cmb_Cancel.Name = "cmb_Cancel";
            this.cmb_Cancel.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmb_Cancel.Size = new System.Drawing.Size(100, 25);
            this.cmb_Cancel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmb_Cancel_KeyPress);
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
            this.faDatePickerStrip1.Size = new System.Drawing.Size(90, 22);
            this.faDatePickerStrip1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePickerStrip1_KeyPress);
            this.faDatePickerStrip1.TextChanged += new System.EventHandler(this.faDatePickerStrip1_TextChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLabel2.Size = new System.Drawing.Size(41, 22);
            this.toolStripLabel2.Text = "تا تاریخ:";
            // 
            // faDatePickerStrip2
            // 
            this.faDatePickerStrip2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.faDatePickerStrip2.BackColor = System.Drawing.SystemColors.Window;
            this.faDatePickerStrip2.Name = "faDatePickerStrip2";
            this.faDatePickerStrip2.Size = new System.Drawing.Size(90, 22);
            this.faDatePickerStrip2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePickerStrip2_KeyPress);
            this.faDatePickerStrip2.TextChanged += new System.EventHandler(this.faDatePickerStrip1_TextChanged);
            // 
            // bt_Search
            // 
            this.bt_Search.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_Search.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_Search.Image = ((System.Drawing.Image)(resources.GetObject("bt_Search.Image")));
            this.bt_Search.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Search.Name = "bt_Search";
            this.bt_Search.Size = new System.Drawing.Size(23, 22);
            this.bt_Search.Text = "مشاهده";
            this.bt_Search.ToolTipText = "Ctrl+D";
            this.bt_Search.Click += new System.EventHandler(this.bt_Search_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "مسیر ذخیره سازی فایل";
            // 
            // dataSet_Report
            // 
            this.dataSet_Report.DataSetName = "DataSet_Report";
            this.dataSet_Report.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // rpt_SaleFactor_GoodTableAdapter
            // 
            this.rpt_SaleFactor_GoodTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_SaleFactor_CustomerTableAdapter
            // 
            this.rpt_SaleFactor_CustomerTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_SaleFactor_FactorsTableAdapter
            // 
            this.rpt_SaleFactor_FactorsTableAdapter.ClearBeforeFill = true;
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Black;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanel0.Id = new System.Guid("7294bf21-914d-44bf-badc-ea65dd67db1c");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("7294bf21-914d-44bf-badc-ea65dd67db1c"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(788, 421), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("7294bf21-914d-44bf-badc-ea65dd67db1c"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("c621cb25-c513-4cd4-9281-643a920f1c40"), Janus.Windows.UI.Dock.PanelGroupStyle.VerticalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("007391bb-e69a-4f8a-a5c9-d46570f508c2"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("ede36a8c-2304-460c-b3bb-b05d456536fd"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.AllowPanelDrop = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.AutoHideButtonVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(3, 28);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(788, 421);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "فهرست کالا";
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.gridEX_Goods);
            this.uiPanel0Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(786, 397);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // gridEX_Goods
            // 
            this.gridEX_Goods.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Goods.AlternatingColors = true;
            this.gridEX_Goods.BuiltInTextsData = resources.GetString("gridEX_Goods.BuiltInTextsData");
            this.gridEX_Goods.CardWidth = 751;
            this.gridEX_Goods.ContextMenuStrip = this.contextMenuStrip1;
            gridEX_Goods_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Goods_DesignTimeLayout.LayoutString");
            this.gridEX_Goods.DesignTimeLayout = gridEX_Goods_DesignTimeLayout;
            this.gridEX_Goods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Goods.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Goods.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Goods.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Goods.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Goods.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_Goods.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Goods.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Goods.GroupTotals = Janus.Windows.GridEX.GroupTotals.Always;
            this.gridEX_Goods.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX_Goods.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Goods.Name = "gridEX_Goods";
            this.gridEX_Goods.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Goods.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Goods.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Goods.RecordNavigator = true;
            this.gridEX_Goods.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.Size = new System.Drawing.Size(786, 397);
            this.gridEX_Goods.TabIndex = 5;
            this.gridEX_Goods.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.TotalRowFormatStyle.BackColor = System.Drawing.Color.White;
            this.gridEX_Goods.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Goods.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Goods.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // Form03_SaleReport_Goods2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form03_SaleReport_Goods2";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "آمار فروش بر اساس کالاها";
            this.Activated += new System.EventHandler(this.Form03_SaleReport_Goods_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form03_SaleReport_Goods_FormClosing);
            this.Load += new System.EventHandler(this.Form03_SaleReport_Goods_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form03_SaleReport_Goods_KeyDown);
            this.contextMenuStrip3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.rpt_SaleFactor_FactorsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_SaleFactor_CustomerBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_SaleFactor_GoodBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale)).EndInit();
            this.contextMenuStrip2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripComboBox cmb_Cancel;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip2;
        private System.Windows.Forms.ToolStripButton bt_Search;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem mnu_ExportToExcel;
        private DataSet_Report dataSet_Report;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private _05_Sale.DataSet_Sale dataSet_Sale;
        private System.Windows.Forms.BindingSource rpt_SaleFactor_GoodBindingSource;
        private _05_Sale.DataSet_SaleTableAdapters.Rpt_SaleFactor_GoodTableAdapter rpt_SaleFactor_GoodTableAdapter;
        private System.Windows.Forms.BindingSource rpt_SaleFactor_CustomerBindingSource;
        private _05_Sale.DataSet_SaleTableAdapters.Rpt_SaleFactor_CustomerTableAdapter rpt_SaleFactor_CustomerTableAdapter;
        private System.Windows.Forms.BindingSource rpt_SaleFactor_FactorsBindingSource;
        private _05_Sale.DataSet_SaleTableAdapters.Rpt_SaleFactor_FactorsTableAdapter rpt_SaleFactor_FactorsTableAdapter;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.GridEX.GridEX gridEX_Goods;
    }
}