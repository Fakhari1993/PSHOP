namespace PSHOP._06_Reports._02_Sale
{
    partial class Form46_GoodProfit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form46_GoodProfit));
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bt_ExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Print = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Display = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.ribbonBarMergeContainer1 = new DevComponents.DotNetBar.RibbonBarMergeContainer();
            this.ribbonBar1 = new DevComponents.DotNetBar.RibbonBar();
            this.gridEXFieldChooserControl1 = new Janus.Windows.GridEX.GridEXFieldChooserControl();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.microChartItem1 = new DevComponents.DotNetBar.MicroChartItem();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.gridEXPrintDocument1 = new Janus.Windows.GridEX.GridEXPrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.tableAdapterManager = new PSHOP._06_Reports.DataSet_ReportTableAdapters.TableAdapterManager();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            this.ribbonBarMergeContainer1.SuspendLayout();
            this.ribbonBar1.SuspendLayout();
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
            this.bt_ExportToExcel,
            this.toolStripSeparator2,
            this.bt_Print,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.faDatePickerStrip1,
            this.toolStripLabel2,
            this.faDatePickerStrip2,
            this.bt_Display,
            this.toolStripSeparator3});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(714, 25);
            this.bindingNavigator1.TabIndex = 2;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bt_ExportToExcel
            // 
            this.bt_ExportToExcel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_ExportToExcel.Image = ((System.Drawing.Image)(resources.GetObject("bt_ExportToExcel.Image")));
            this.bt_ExportToExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_ExportToExcel.Name = "bt_ExportToExcel";
            this.bt_ExportToExcel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bt_ExportToExcel.Size = new System.Drawing.Size(23, 22);
            this.bt_ExportToExcel.ToolTipText = "Ctrl+E";
            this.bt_ExportToExcel.Click += new System.EventHandler(this.bt_ExportToExcel_Click);
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
            this.bt_Display.ToolTipText = "Ctrl+F5";
            this.bt_Display.Click += new System.EventHandler(this.bt_Display_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // gridEX1
            // 
            this.gridEX1.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.BuiltInTextsData = resources.GetString("gridEX1.BuiltInTextsData");
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
            this.gridEX1.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX1.Location = new System.Drawing.Point(0, 25);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX1.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.SaveSettings = true;
            this.gridEX1.SettingsKey = "goodproftgridx";
            this.gridEX1.Size = new System.Drawing.Size(714, 427);
            this.gridEX1.TabIndex = 9;
            this.gridEX1.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX1.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX1.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // ribbonBarMergeContainer1
            // 
            this.ribbonBarMergeContainer1.AutoActivateTab = false;
            this.ribbonBarMergeContainer1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.ribbonBarMergeContainer1.Controls.Add(this.ribbonBar1);
            this.ribbonBarMergeContainer1.Location = new System.Drawing.Point(23, 0);
            this.ribbonBarMergeContainer1.MergeRibbonGroupName = "SettingTab";
            this.ribbonBarMergeContainer1.Name = "ribbonBarMergeContainer1";
            this.ribbonBarMergeContainer1.RibbonTabColorTable = DevComponents.DotNetBar.eRibbonTabColor.Green;
            this.ribbonBarMergeContainer1.RibbonTabText = "تنظیمات";
            this.ribbonBarMergeContainer1.ShowFocusRectangle = true;
            this.ribbonBarMergeContainer1.Size = new System.Drawing.Size(298, 22);
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
            this.ribbonBarMergeContainer1.TabIndex = 68;
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
            this.ribbonBar1.Location = new System.Drawing.Point(37, 0);
            this.ribbonBar1.Name = "ribbonBar1";
            this.ribbonBar1.ResizeItemsToFit = false;
            this.ribbonBar1.Size = new System.Drawing.Size(261, 22);
            this.ribbonBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonBar1.TabIndex = 0;
            this.ribbonBar1.Text = "انتخاب ستونهای لیست کالاها";
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
            this.gridEXFieldChooserControl1.GridEX = this.gridEX1;
            this.gridEXFieldChooserControl1.Location = new System.Drawing.Point(0, 0);
            this.gridEXFieldChooserControl1.Name = "gridEXFieldChooserControl1";
            this.gridEXFieldChooserControl1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEXFieldChooserControl1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEXFieldChooserControl1.Size = new System.Drawing.Size(261, 6);
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
            // printDialog1
            // 
            this.printDialog1.Document = this.gridEXPrintDocument1;
            this.printDialog1.UseEXDialog = true;
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
            // pageSetupDialog1
            // 
            this.pageSetupDialog1.Document = this.gridEXPrintDocument1;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Connection = null;
            this.tableAdapterManager.Table_034_SaleFactor_Child3TableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PSHOP._06_Reports.DataSet_ReportTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
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
            this.faDatePickerStrip2.TextChanged += new System.EventHandler(this.faDatePickerStrip2_TextChanged);
            // 
            // Form46_GoodProfit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 452);
            this.Controls.Add(this.ribbonBarMergeContainer1);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form46_GoodProfit";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "گزارش سود براساس فاکتورها";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiffrenceReport_FormClosing);
            this.Load += new System.EventHandler(this.DiffrenceReport_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form46_GoodProfit_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.Configuration.IPersistComponentSettings)(this.gridEX1)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ribbonBarMergeContainer1.ResumeLayout(false);
            this.ribbonBar1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bt_Display;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private DevComponents.DotNetBar.RibbonBarMergeContainer ribbonBarMergeContainer1;
        private DevComponents.DotNetBar.RibbonBar ribbonBar1;
        private Janus.Windows.GridEX.GridEXFieldChooserControl gridEXFieldChooserControl1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.MicroChartItem microChartItem1;
        private _06_Reports.DataSet_ReportTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.ToolStripButton bt_ExportToExcel;
        private System.Windows.Forms.ToolStripButton bt_Print;
        private System.Windows.Forms.PrintDialog printDialog1;
        private Janus.Windows.GridEX.GridEXPrintDocument gridEXPrintDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip2;
    }
}