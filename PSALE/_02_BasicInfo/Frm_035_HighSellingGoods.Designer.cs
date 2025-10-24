namespace PSHOP._02_BasicInfo
{
    partial class Frm_035_HighSellingGoods
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_035_HighSellingGoods));
            Janus.Windows.GridEX.GridEXLayout gridEX_Header_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.bt_Search = new System.Windows.Forms.ToolStripButton();
            this.bt_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.gridEX_Header = new Janus.Windows.GridEX.GridEX();
            this.table_004_CommodityAndIngredientsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_EtelaatPaye = new PSHOP._02_BasicInfo.DataSet_EtelaatPaye();
            this.tableAdapterManager = new PSHOP._02_BasicInfo.DataSet_EtelaatPayeTableAdapters.TableAdapterManager();
            this.table_004_CommodityAndIngredientsTableAdapter = new PSHOP._02_BasicInfo.DataSet_EtelaatPayeTableAdapters.table_004_CommodityAndIngredientsTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Header)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_004_CommodityAndIngredientsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_EtelaatPaye)).BeginInit();
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
            this.toolStripSeparator8,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.faDatePickerStrip1,
            this.toolStripLabel2,
            this.faDatePickerStrip2,
            this.bt_Search,
            this.bt_Save,
            this.toolStripSeparator2});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(714, 25);
            this.bindingNavigator1.TabIndex = 9;
            this.bindingNavigator1.Text = "bindingNavigator2";
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            this.faDatePickerStrip1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePickerStrip1_KeyPress);
            this.faDatePickerStrip1.TextChanged += new System.EventHandler(this.faDatePickerStrip1_TextChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLabel2.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabel2.Text = "از تاریخ:";
            // 
            // faDatePickerStrip2
            // 
            this.faDatePickerStrip2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.faDatePickerStrip2.BackColor = System.Drawing.SystemColors.Window;
            this.faDatePickerStrip2.Name = "faDatePickerStrip2";
            this.faDatePickerStrip2.Size = new System.Drawing.Size(100, 22);
            this.faDatePickerStrip2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePickerStrip2_KeyPress);
            this.faDatePickerStrip2.TextChanged += new System.EventHandler(this.faDatePickerStrip2_TextChanged);
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
            this.bt_Search.ToolTipText = "Ctrl+F5";
            this.bt_Search.Click += new System.EventHandler(this.bt_Search_Click);
            // 
            // bt_Save
            // 
            this.bt_Save.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.bt_Save.Image = ((System.Drawing.Image)(resources.GetObject("bt_Save.Image")));
            this.bt_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Save.Name = "bt_Save";
            this.bt_Save.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_Save.Size = new System.Drawing.Size(53, 22);
            this.bt_Save.Text = "ذخیره";
            this.bt_Save.ToolTipText = "Ctrl+S";
            this.bt_Save.Click += new System.EventHandler(this.bt_Save_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // gridEX_Header
            // 
            this.gridEX_Header.AlternatingColors = true;
            this.gridEX_Header.BuiltInTextsData = resources.GetString("gridEX_Header.BuiltInTextsData");
            this.gridEX_Header.CardWidth = 751;
            this.gridEX_Header.ColumnSetNavigation = Janus.Windows.GridEX.ColumnSetNavigation.ColumnSet;
            this.gridEX_Header.DataSource = this.table_004_CommodityAndIngredientsBindingSource;
            gridEX_Header_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Header_DesignTimeLayout.LayoutString");
            this.gridEX_Header.DesignTimeLayout = gridEX_Header_DesignTimeLayout;
            this.gridEX_Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Header.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Header.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Header.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Header.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Header.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Header.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX_Header.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX_Header.GroupByBoxVisible = false;
            this.gridEX_Header.Location = new System.Drawing.Point(0, 25);
            this.gridEX_Header.Name = "gridEX_Header";
            this.gridEX_Header.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Header.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Header.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Header.OfficeCustomColor = System.Drawing.Color.Teal;
            this.gridEX_Header.RecordNavigator = true;
            this.gridEX_Header.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_Header.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Header.SettingsKey = "pregridEX_Header";
            this.gridEX_Header.Size = new System.Drawing.Size(714, 427);
            this.gridEX_Header.TabIndex = 10;
            this.gridEX_Header.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Header.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridEX_Header.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Header.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Header.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // table_004_CommodityAndIngredientsBindingSource
            // 
            this.table_004_CommodityAndIngredientsBindingSource.DataMember = "table_004_CommodityAndIngredients";
            this.table_004_CommodityAndIngredientsBindingSource.DataSource = this.dataSet_EtelaatPaye;
            // 
            // dataSet_EtelaatPaye
            // 
            this.dataSet_EtelaatPaye.DataSetName = "DataSet_EtelaatPaye";
            this.dataSet_EtelaatPaye.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_000_ClassTableAdapter = null;
            this.tableAdapterManager.Table_001_CustomerAdditionalInformationTableAdapter = null;
            this.tableAdapterManager.Table_002_SalesTypesTableAdapter = null;
            this.tableAdapterManager.Table_003_InformationProductCashTableAdapter = null;
            this.tableAdapterManager.table_004_CommodityAndIngredientsTableAdapter = this.table_004_CommodityAndIngredientsTableAdapter;
            this.tableAdapterManager.Table_005_OrderHeaderTableAdapter = null;
            this.tableAdapterManager.Table_024_Discount_BuyTableAdapter = null;
            this.tableAdapterManager.Table_024_DiscountTableAdapter = null;
            this.tableAdapterManager.Table_028_AwardTableAdapter = null;
            this.tableAdapterManager.Table_029_CustomerGroupGoodPricingTableAdapter = null;
            this.tableAdapterManager.Table_040_PersonGroupsTableAdapter = null;
            this.tableAdapterManager.Table_045_PersonInfoContactsTableAdapter = null;
            this.tableAdapterManager.Table_045_PersonInfoTableAdapter = null;
            this.tableAdapterManager.Table_045_PersonScopeTableAdapter = null;
            this.tableAdapterManager.Table_060_ProvinceInfoTableAdapter = null;
            this.tableAdapterManager.Table_065_CityInfoTableAdapter = null;
            this.tableAdapterManager.Table_105_SystemTransactionInfoTableAdapter = null;
            this.tableAdapterManager.Table_160_StatesTableAdapter = null;
            this.tableAdapterManager.Table_175_SMSTableAdapter = null;
            this.tableAdapterManager.Table_220_SMSTextTableAdapter = null;
            this.tableAdapterManager.Table_295_StoreInfoTableAdapter = null;
            this.tableAdapterManager.Table_296_StoreUsersTableAdapter = null;
            this.tableAdapterManager.Table_82_PriceStatementTableAdapter = null;
            this.tableAdapterManager.Table_83_PriceStatementChildTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PSHOP._02_BasicInfo.DataSet_EtelaatPayeTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // table_004_CommodityAndIngredientsTableAdapter
            // 
            this.table_004_CommodityAndIngredientsTableAdapter.ClearBeforeFill = true;
            // 
            // Frm_035_HighSellingGoods
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 452);
            this.Controls.Add(this.gridEX_Header);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Frm_035_HighSellingGoods";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تعیین کالای پر فروش";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_035_HighSellingGoods_FormClosing);
            this.Load += new System.EventHandler(this.Frm_035_HighSellingGoods_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_035_HighSellingGoods_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Header)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_004_CommodityAndIngredientsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_EtelaatPaye)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip2;
        private System.Windows.Forms.ToolStripButton bt_Search;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private Janus.Windows.GridEX.GridEX gridEX_Header;
        private DataSet_EtelaatPaye dataSet_EtelaatPaye;
        private System.Windows.Forms.ToolStripButton bt_Save;
        private System.Windows.Forms.BindingSource table_004_CommodityAndIngredientsBindingSource;
        private DataSet_EtelaatPayeTableAdapters.TableAdapterManager tableAdapterManager;
        private DataSet_EtelaatPayeTableAdapters.table_004_CommodityAndIngredientsTableAdapter table_004_CommodityAndIngredientsTableAdapter;
    }
}