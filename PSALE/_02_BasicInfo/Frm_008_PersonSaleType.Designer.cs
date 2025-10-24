namespace PSHOP._02_BasicInfo
{
    partial class Frm_008_PersonSaleType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_008_PersonSaleType));
            Janus.Windows.GridEX.GridEXLayout mlt_ACC_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX_Goods_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bt_Refresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.bt_Apply = new System.Windows.Forms.ToolStripButton();
            this.mlt_ACC = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.gridEX_Goods = new Janus.Windows.GridEX.GridEX();
            this.table_045_PersonInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_05_Awards = new PSHOP._02_BasicInfo.DataSet_05_Awards();
            this.table_045_PersonInfoTableAdapter = new PSHOP._02_BasicInfo.DataSet_05_AwardsTableAdapters.Table_045_PersonInfoTableAdapter();
            this.tableAdapterManager = new PSHOP._02_BasicInfo.DataSet_05_AwardsTableAdapters.TableAdapterManager();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_ACC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_045_PersonInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_Awards)).BeginInit();
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
            this.bt_Refresh,
            this.toolStripSeparator2,
            this.toolStripDropDownButton1,
            this.toolStripSeparator1,
            this.bt_Save,
            this.toolStripSeparator3,
            this.toolStripLabel1,
            this.bt_Apply});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(714, 25);
            this.bindingNavigator1.TabIndex = 3;
            this.bindingNavigator1.TabStop = true;
            this.bindingNavigator1.Text = "bindingNavigator2";
            // 
            // bt_Refresh
            // 
            this.bt_Refresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_Refresh.Image = ((System.Drawing.Image)(resources.GetObject("bt_Refresh.Image")));
            this.bt_Refresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Refresh.Name = "bt_Refresh";
            this.bt_Refresh.Size = new System.Drawing.Size(23, 22);
            this.bt_Refresh.Text = "bt_Refresh";
            this.bt_Refresh.ToolTipText = "بازخوانی اطلاعات\r\nF5";
            this.bt_Refresh.Click += new System.EventHandler(this.bt_Refresh_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLabel1.Size = new System.Drawing.Size(249, 22);
            this.toolStripLabel1.Text = "نوع فروش :                                                               ";
            // 
            // bt_Apply
            // 
            this.bt_Apply.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.bt_Apply.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt_Apply.Image = ((System.Drawing.Image)(resources.GetObject("bt_Apply.Image")));
            this.bt_Apply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Apply.Name = "bt_Apply";
            this.bt_Apply.Size = new System.Drawing.Size(23, 22);
            this.bt_Apply.Text = "اعمال نوع فروش به اشخاص انتخاب شده";
            this.bt_Apply.Click += new System.EventHandler(this.bt_Apply_Click);
            // 
            // mlt_ACC
            // 
            this.mlt_ACC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            mlt_ACC_DesignTimeLayout.LayoutString = resources.GetString("mlt_ACC_DesignTimeLayout.LayoutString");
            this.mlt_ACC.DesignTimeLayout = mlt_ACC_DesignTimeLayout;
            this.mlt_ACC.DisplayMember = "column02";
            this.mlt_ACC.Location = new System.Drawing.Point(468, 0);
            this.mlt_ACC.Name = "mlt_ACC";
            this.mlt_ACC.SelectedIndex = -1;
            this.mlt_ACC.SelectedItem = null;
            this.mlt_ACC.SelectInDataSource = true;
            this.mlt_ACC.Size = new System.Drawing.Size(185, 21);
            this.mlt_ACC.TabIndex = 8;
            this.mlt_ACC.ValueMember = "columnid";
            this.mlt_ACC.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // gridEX_Goods
            // 
            this.gridEX_Goods.AlternatingColors = true;
            this.gridEX_Goods.BuiltInTextsData = resources.GetString("gridEX_Goods.BuiltInTextsData");
            this.gridEX_Goods.CardWidth = 751;
            this.gridEX_Goods.DataSource = this.table_045_PersonInfoBindingSource;
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
            this.gridEX_Goods.GroupByBoxVisible = false;
            this.gridEX_Goods.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.Gold;
            this.gridEX_Goods.GroupTotals = Janus.Windows.GridEX.GroupTotals.ExpandedGroup;
            this.gridEX_Goods.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX_Goods.Location = new System.Drawing.Point(0, 25);
            this.gridEX_Goods.Name = "gridEX_Goods";
            this.gridEX_Goods.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Goods.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Goods.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Goods.RecordNavigator = true;
            this.gridEX_Goods.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.SettingsKey = "Form34_9CompReport_Goods09";
            this.gridEX_Goods.Size = new System.Drawing.Size(714, 427);
            this.gridEX_Goods.TabIndex = 9;
            this.gridEX_Goods.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.TotalRowFormatStyle.BackColor = System.Drawing.Color.White;
            this.gridEX_Goods.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Goods.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Goods.UseCompatibleTextRendering = true;
            this.gridEX_Goods.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // table_045_PersonInfoBindingSource
            // 
            this.table_045_PersonInfoBindingSource.DataMember = "Table_045_PersonInfo";
            this.table_045_PersonInfoBindingSource.DataSource = this.dataSet_05_Awards;
            // 
            // dataSet_05_Awards
            // 
            this.dataSet_05_Awards.DataSetName = "DataSet_05_Awards";
            this.dataSet_05_Awards.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // table_045_PersonInfoTableAdapter
            // 
            this.table_045_PersonInfoTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_000_OrgInfoTableAdapter = null;
            this.tableAdapterManager.table_004_CommodityAndIngredientsTableAdapter = null;
            this.tableAdapterManager.Table_040_PersonGroupsTableAdapter = null;
            this.tableAdapterManager.Table_040_RialAwardsTableAdapter = null;
            this.tableAdapterManager.Table_045_PersonInfoTableAdapter = this.table_045_PersonInfoTableAdapter;
            this.tableAdapterManager.Table_045_SaleManChartTableAdapter = null;
            this.tableAdapterManager.Table_045_TotalQtyAwardsTableAdapter = null;
            this.tableAdapterManager.Table_050_VehicleAwardsTableAdapter = null;
            this.tableAdapterManager.Table_115_VehicleTypeTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PSHOP._02_BasicInfo.DataSet_05_AwardsTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "مسیر ذخیره سازی فایل";
            // 
            // Frm_008_PersonSaleType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 452);
            this.Controls.Add(this.gridEX_Goods);
            this.Controls.Add(this.mlt_ACC);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Frm_008_PersonSaleType";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ثبت نوع فروش اشخاص";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_008_PersonSaleType_FormClosing);
            this.Load += new System.EventHandler(this.Frm_008_PersonSaleType_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_008_PersonSaleType_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_ACC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_045_PersonInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_Awards)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton bt_Refresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bt_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton bt_Apply;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_ACC;
        private Janus.Windows.GridEX.GridEX gridEX_Goods;
        private DataSet_05_Awards dataSet_05_Awards;
        private System.Windows.Forms.BindingSource table_045_PersonInfoBindingSource;
        private DataSet_05_AwardsTableAdapters.Table_045_PersonInfoTableAdapter table_045_PersonInfoTableAdapter;
        private DataSet_05_AwardsTableAdapters.TableAdapterManager tableAdapterManager;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}