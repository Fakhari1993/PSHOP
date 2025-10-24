namespace PSHOP._06_Reports._02_Sale
{
    partial class Form11_DiscountReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form11_DiscountReport));
            Janus.Windows.GridEX.GridEXLayout gridEX_SaleFactor_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.bt_ExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Print = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Display = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_SaleFactor = new Janus.Windows.GridEX.GridEX();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.bindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_Report = new PSHOP._06_Reports.DataSet_Report();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_SaleFactor)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
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
            this.bt_Print,
            this.toolStripSeparator1,
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
            this.bindingNavigator1.TabIndex = 1;
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
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_Print
            // 
            this.bt_Print.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_Print.Image = ((System.Drawing.Image)(resources.GetObject("bt_Print.Image")));
            this.bt_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Print.Name = "bt_Print";
            this.bt_Print.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bt_Print.Size = new System.Drawing.Size(46, 22);
            this.bt_Print.Text = "چاپ";
            this.bt_Print.ToolTipText = "Ctrl+P";
            this.bt_Print.Click += new System.EventHandler(this.bt_Print_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_Display
            // 
            this.bt_Display.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_Display.Image = ((System.Drawing.Image)(resources.GetObject("bt_Display.Image")));
            this.bt_Display.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Display.Name = "bt_Display";
            this.bt_Display.Size = new System.Drawing.Size(23, 22);
            this.bt_Display.Text = "toolStripButton1";
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
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(110)))));
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2010;
            this.uiPanel0.Id = new System.Guid("e03d2298-ad35-4844-8f6f-1cb4a1afceb8");
            this.uiPanel0.StaticGroup = true;
            this.uiPanel2.Id = new System.Guid("b9b5db81-bc0d-47f7-8a71-263f96f724e3");
            this.uiPanel0.Panels.Add(this.uiPanel2);
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("e03d2298-ad35-4844-8f6f-1cb4a1afceb8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(788, 421), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("b9b5db81-bc0d-47f7-8a71-263f96f724e3"), new System.Guid("e03d2298-ad35-4844-8f6f-1cb4a1afceb8"), 399, true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("e03d2298-ad35-4844-8f6f-1cb4a1afceb8"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("b9b5db81-bc0d-47f7-8a71-263f96f724e3"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CaptionHeight = 0;
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.Location = new System.Drawing.Point(3, 28);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(788, 421);
            this.uiPanel0.TabIndex = 4;
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(0, -1);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(788, 422);
            this.uiPanel2.TabIndex = 4;
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.gridEX_SaleFactor);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(786, 398);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // gridEX_SaleFactor
            // 
            this.gridEX_SaleFactor.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_SaleFactor.AlternatingColors = true;
            this.gridEX_SaleFactor.BuiltInTextsData = resources.GetString("gridEX_SaleFactor.BuiltInTextsData");
            this.gridEX_SaleFactor.DataSource = this.bindingSource1;
            gridEX_SaleFactor_DesignTimeLayout.LayoutString = resources.GetString("gridEX_SaleFactor_DesignTimeLayout.LayoutString");
            this.gridEX_SaleFactor.DesignTimeLayout = gridEX_SaleFactor_DesignTimeLayout;
            this.gridEX_SaleFactor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_SaleFactor.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_SaleFactor.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_SaleFactor.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_SaleFactor.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_SaleFactor.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_SaleFactor.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_SaleFactor.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_SaleFactor.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_SaleFactor.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX_SaleFactor.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX_SaleFactor.GroupByBoxVisible = false;
            this.gridEX_SaleFactor.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX_SaleFactor.Location = new System.Drawing.Point(0, 0);
            this.gridEX_SaleFactor.Name = "gridEX_SaleFactor";
            this.gridEX_SaleFactor.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_SaleFactor.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_SaleFactor.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_SaleFactor.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_SaleFactor.Size = new System.Drawing.Size(786, 398);
            this.gridEX_SaleFactor.TabIndex = 9;
            this.gridEX_SaleFactor.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_SaleFactor.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_SaleFactor.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_SaleFactor.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_SaleFactor.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_SaleFactor.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_SaleFactor.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.gridEX_SaleFactor.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.gridEX_SaleFactor_RowDoubleClick);
            // 
            // gridEXExporter1
            // 
            this.gridEXExporter1.GridEX = this.gridEX_SaleFactor;
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
            // uiPanel1
            // 
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 22);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(788, 399);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "Panel 1";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(788, 399);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // Form11_DiscountReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form11_DiscountReport";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "گزارش تخفیفات فاکتورهای فروش";
            this.Activated += new System.EventHandler(this.Form10_ExtraReduction_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form10_ExtraReduction_FormClosing);
            this.Load += new System.EventHandler(this.Form10_ExtraReduction_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form10_ExtraReduction_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_SaleFactor)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bt_Display;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.BindingSource bindingSource2;
        private DataSet_Report dataSet_Report;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel2;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private Janus.Windows.GridEX.GridEX gridEX_SaleFactor;
        private System.Windows.Forms.ToolStripButton bt_ExportToExcel;
        private System.Windows.Forms.ToolStripButton bt_Print;
    }
}