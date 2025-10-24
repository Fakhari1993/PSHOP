namespace PSHOP._02_BasicInfo
{
    partial class Frm_033_PriceStatementList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_033_PriceStatementList));
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Refresh = new System.Windows.Forms.ToolStripButton();
            this.cmb_Print = new System.Windows.Forms.ToolStripButton();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.dataSet_EtelaatPaye = new PSHOP._02_BasicInfo.DataSet_EtelaatPaye();
            this.dataTable1BindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataTable1TableAdapter = new PSHOP._02_BasicInfo.DataSet_EtelaatPayeTableAdapters.DataTable1TableAdapter();
            this.tableAdapterManager = new PSHOP._02_BasicInfo.DataSet_EtelaatPayeTableAdapters.TableAdapterManager();
            this.ribbonBarMergeContainer1 = new DevComponents.DotNetBar.RibbonBarMergeContainer();
            this.ribbonBar1 = new DevComponents.DotNetBar.RibbonBar();
            this.gridEXFieldChooserControl1 = new Janus.Windows.GridEX.GridEXFieldChooserControl();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.buttonItem2 = new DevComponents.DotNetBar.ButtonItem();
            this.microChartItem1 = new DevComponents.DotNetBar.MicroChartItem();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.gridEXPrintDocument1 = new Janus.Windows.GridEX.GridEXPrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.gridEXExporter2 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_EtelaatPaye)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1BindingSource)).BeginInit();
            this.ribbonBarMergeContainer1.SuspendLayout();
            this.ribbonBar1.SuspendLayout();
            this.SuspendLayout();
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = global::PSHOP.Properties.Resources.me_bg;
            this.bindingNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator3,
            this.toolStripDropDownButton1,
            this.bindingNavigatorSeparator5,
            this.bt_Refresh,
            this.cmb_Print});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(704, 25);
            this.bindingNavigator1.TabIndex = 6;
            this.bindingNavigator1.TabStop = true;
            this.bindingNavigator1.Text = "bindingNavigator2";
            this.bindingNavigator1.RefreshItems += new System.EventHandler(this.bindingNavigator1_RefreshItems);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripDropDownButton1.Image = global::PSHOP.Properties.Resources.Excel;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(95, 22);
            this.toolStripDropDownButton1.Text = "ارسال به Excel";
            this.toolStripDropDownButton1.Click += new System.EventHandler(this.toolStripDropDownButton1_Click);
            // 
            // bindingNavigatorSeparator5
            // 
            this.bindingNavigatorSeparator5.Name = "bindingNavigatorSeparator5";
            this.bindingNavigatorSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_Refresh
            // 
            this.bt_Refresh.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_Refresh.Image = ((System.Drawing.Image)(resources.GetObject("bt_Refresh.Image")));
            this.bt_Refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Refresh.Name = "bt_Refresh";
            this.bt_Refresh.Size = new System.Drawing.Size(23, 22);
            this.bt_Refresh.Text = "bt_Refresh";
            this.bt_Refresh.ToolTipText = "بازخوانی اطلاعات\r\nF5";
            this.bt_Refresh.Click += new System.EventHandler(this.bt_Refresh_Click);
            // 
            // cmb_Print
            // 
            this.cmb_Print.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmb_Print.Image = global::PSHOP.Properties.Resources.Printer;
            this.cmb_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.cmb_Print.Name = "cmb_Print";
            this.cmb_Print.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cmb_Print.Size = new System.Drawing.Size(49, 22);
            this.cmb_Print.Text = "چاپ";
            this.cmb_Print.ToolTipText = "Ctrl+P";
            this.cmb_Print.Click += new System.EventHandler(this.cmb_Print_Click);
            // 
            // gridEX1
            // 
            this.gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX1.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.BuiltInTextsData = resources.GetString("gridEX1.BuiltInTextsData");
            this.gridEX1.DataSource = this.dataTable1BindingSource;
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX1.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX1.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX1.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX1.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX1.GroupByBoxVisible = false;
            this.gridEX1.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX1.Location = new System.Drawing.Point(0, 25);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.SaveSettings = true;
            this.gridEX1.SettingsKey = "PriceStatementListgrid";
            this.gridEX1.Size = new System.Drawing.Size(704, 417);
            this.gridEX1.TabIndex = 7;
            this.gridEX1.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.gridEX1.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX1.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // dataSet_EtelaatPaye
            // 
            this.dataSet_EtelaatPaye.DataSetName = "DataSet_EtelaatPaye";
            this.dataSet_EtelaatPaye.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dataTable1BindingSource
            // 
            this.dataTable1BindingSource.DataMember = "DataTable1";
            this.dataTable1BindingSource.DataSource = this.dataSet_EtelaatPaye;
            // 
            // dataTable1TableAdapter
            // 
            this.dataTable1TableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Connection = null;
            this.tableAdapterManager.Table_000_ClassTableAdapter = null;
            this.tableAdapterManager.Table_001_CustomerAdditionalInformationTableAdapter = null;
            this.tableAdapterManager.Table_002_SalesTypesTableAdapter = null;
            this.tableAdapterManager.Table_003_InformationProductCashTableAdapter = null;
            this.tableAdapterManager.table_004_CommodityAndIngredientsTableAdapter = null;
            this.tableAdapterManager.Table_005_OrderHeaderTableAdapter = null;
            this.tableAdapterManager.Table_024_Discount_BuyTableAdapter = null;
            this.tableAdapterManager.Table_024_DiscountTableAdapter = null;
            this.tableAdapterManager.Table_028_AwardTableAdapter = null;
            this.tableAdapterManager.Table_029_CustomerGroupGoodPricingTableAdapter = null;
            this.tableAdapterManager.Table_040_PersonGroupsTableAdapter = null;
            this.tableAdapterManager.Table_045_PersonInfoTableAdapter = null;
            this.tableAdapterManager.Table_045_PersonScopeTableAdapter = null;
            this.tableAdapterManager.Table_060_ProvinceInfoTableAdapter = null;
            this.tableAdapterManager.Table_065_CityInfoTableAdapter = null;
            this.tableAdapterManager.Table_105_SystemTransactionInfoTableAdapter = null;
            this.tableAdapterManager.Table_160_StatesTableAdapter = null;
            this.tableAdapterManager.Table_175_SMSTableAdapter = null;
            this.tableAdapterManager.Table_220_SMSTextTableAdapter = null;
            this.tableAdapterManager.Table_82_PriceStatementTableAdapter = null;
            this.tableAdapterManager.Table_83_PriceStatementChildTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PSHOP._02_BasicInfo.DataSet_EtelaatPayeTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // ribbonBarMergeContainer1
            // 
            this.ribbonBarMergeContainer1.AutoActivateTab = false;
            this.ribbonBarMergeContainer1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.ribbonBarMergeContainer1.Controls.Add(this.ribbonBar1);
            this.ribbonBarMergeContainer1.Location = new System.Drawing.Point(354, 4);
            this.ribbonBarMergeContainer1.MergeRibbonGroupName = "SettingTab";
            this.ribbonBarMergeContainer1.Name = "ribbonBarMergeContainer1";
            this.ribbonBarMergeContainer1.RibbonTabColorTable = DevComponents.DotNetBar.eRibbonTabColor.Green;
            this.ribbonBarMergeContainer1.RibbonTabText = "تنظیمات";
            this.ribbonBarMergeContainer1.ShowFocusRectangle = true;
            this.ribbonBarMergeContainer1.Size = new System.Drawing.Size(16, 21);
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
            this.ribbonBarMergeContainer1.TabIndex = 28;
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
            this.ribbonBar1.Location = new System.Drawing.Point(-245, 0);
            this.ribbonBar1.Name = "ribbonBar1";
            this.ribbonBar1.ResizeItemsToFit = false;
            this.ribbonBar1.Size = new System.Drawing.Size(261, 21);
            this.ribbonBar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.ribbonBar1.TabIndex = 0;
            this.ribbonBar1.Text = "تظیمات";
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
            this.gridEXFieldChooserControl1.Size = new System.Drawing.Size(261, 5);
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
            // saveFileDialog2
            // 
            this.saveFileDialog2.DefaultExt = "xls";
            this.saveFileDialog2.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog2.RestoreDirectory = true;
            this.saveFileDialog2.Title = "مسیر ذخیره سازی فایل";
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.gridEXPrintDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // Frm_033_PriceStatementList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 442);
            this.Controls.Add(this.ribbonBarMergeContainer1);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Frm_033_PriceStatementList";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "مشاهده اعلامیه قیمت";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_033_PriceStatementList_FormClosing);
            this.Load += new System.EventHandler(this.Frm_033_PriceStatementList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.Configuration.IPersistComponentSettings)(this.gridEX1)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_EtelaatPaye)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataTable1BindingSource)).EndInit();
            this.ribbonBarMergeContainer1.ResumeLayout(false);
            this.ribbonBar1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator5;
        private System.Windows.Forms.ToolStripButton bt_Refresh;
        private System.Windows.Forms.ToolStripButton cmb_Print;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private DataSet_EtelaatPaye dataSet_EtelaatPaye;
        private System.Windows.Forms.BindingSource dataTable1BindingSource;
        private DataSet_EtelaatPayeTableAdapters.DataTable1TableAdapter dataTable1TableAdapter;
        private DataSet_EtelaatPayeTableAdapters.TableAdapterManager tableAdapterManager;
        private DevComponents.DotNetBar.RibbonBarMergeContainer ribbonBarMergeContainer1;
        private DevComponents.DotNetBar.RibbonBar ribbonBar1;
        private Janus.Windows.GridEX.GridEXFieldChooserControl gridEXFieldChooserControl1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.ButtonItem buttonItem2;
        private DevComponents.DotNetBar.MicroChartItem microChartItem1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private Janus.Windows.GridEX.GridEXPrintDocument gridEXPrintDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.PrintDialog printDialog1;
    }
}