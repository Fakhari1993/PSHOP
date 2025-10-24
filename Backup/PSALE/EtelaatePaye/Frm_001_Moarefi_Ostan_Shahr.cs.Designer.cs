namespace PSALE.EtelaatePaye
{
    partial class Frm_001_Moarefi_Ostan_Shahr
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_001_Moarefi_Ostan_Shahr));
            Janus.Windows.GridEX.GridEXLayout gridEX2_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.table_060_ProvinceInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_EtelaatPaye = new PSALE.EtelaatePaye.DataSet_EtelaatPaye();
            this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
            this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
            this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
            this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gridEX2 = new Janus.Windows.GridEX.GridEX();
            this.table_065_CityInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.table_060_ProvinceInfoTableAdapter = new PSALE.EtelaatePaye.DataSet_EtelaatPayeTableAdapters.Table_060_ProvinceInfoTableAdapter();
            this.tableAdapterManager = new PSALE.EtelaatePaye.DataSet_EtelaatPayeTableAdapters.TableAdapterManager();
            this.table_065_CityInfoTableAdapter = new PSALE.EtelaatePaye.DataSet_EtelaatPayeTableAdapters.Table_065_CityInfoTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table_060_ProvinceInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_EtelaatPaye)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_065_CityInfoBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            this.SuspendLayout();
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = global::PSALE.Properties.Resources.me_bg;
            this.bindingNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bindingNavigator1.BindingSource = this.table_060_ProvinceInfoBindingSource;
            this.bindingNavigator1.CountItem = this.bindingNavigatorCountItem;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.bt_Save,
            this.toolStripSeparator1});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = this.bindingNavigatorPositionItem;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(704, 25);
            this.bindingNavigator1.TabIndex = 6;
            // 
            // table_060_ProvinceInfoBindingSource
            // 
            this.table_060_ProvinceInfoBindingSource.DataMember = "Table_060_ProvinceInfo";
            this.table_060_ProvinceInfoBindingSource.DataSource = this.dataSet_EtelaatPaye;
            // 
            // dataSet_EtelaatPaye
            // 
            this.dataSet_EtelaatPaye.DataSetName = "DataSet_EtelaatPaye";
            this.dataSet_EtelaatPaye.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(36, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            this.bindingNavigatorMoveFirstItem.Click += new System.EventHandler(this.bindingNavigatorMoveFirstItem_Click);
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            this.bindingNavigatorMovePreviousItem.Click += new System.EventHandler(this.bindingNavigatorMovePreviousItem_Click);
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
            // 
            // bindingNavigatorSeparator1
            // 
            this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
            this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorMoveNextItem
            // 
            this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
            this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
            this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            this.bindingNavigatorMoveNextItem.Click += new System.EventHandler(this.bindingNavigatorMoveNextItem_Click);
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            this.bindingNavigatorMoveLastItem.Click += new System.EventHandler(this.bindingNavigatorMoveLastItem_Click);
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.gridEX2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.gridEX1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 25);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(704, 417);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // gridEX2
            // 
            this.gridEX2.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.AlternatingColors = true;
            this.gridEX2.ColumnAutoResize = true;
            this.gridEX2.DataSource = this.table_065_CityInfoBindingSource;
            gridEX2_DesignTimeLayout.LayoutString = resources.GetString("gridEX2_DesignTimeLayout.LayoutString");
            this.gridEX2.DesignTimeLayout = gridEX2_DesignTimeLayout;
            this.gridEX2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX2.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX2.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX2.GroupByBoxVisible = false;
            this.gridEX2.Location = new System.Drawing.Point(3, 3);
            this.gridEX2.Name = "gridEX2";
            this.gridEX2.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX2.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX2.Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme.Custom;
            this.gridEX2.Office2007CustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX2.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.Size = new System.Drawing.Size(346, 411);
            this.gridEX2.TabIndex = 13;
            this.gridEX2.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX2.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX2.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEX2.Error += new Janus.Windows.GridEX.ErrorEventHandler(this.gridEX2_Error);
            this.gridEX2.RecordsDeleted += new System.EventHandler(this.gridEX2_RecordsDeleted);
            this.gridEX2.Enter += new System.EventHandler(this.gridEX2_Enter);
            this.gridEX2.DeletingRecord += new Janus.Windows.GridEX.RowActionCancelEventHandler(this.gridEX2_DeletingRecord);
            // 
            // table_065_CityInfoBindingSource
            // 
            this.table_065_CityInfoBindingSource.DataMember = "FK_Table_065_CityInfo_Table_060_ProvinceInfo";
            this.table_065_CityInfoBindingSource.DataSource = this.table_060_ProvinceInfoBindingSource;
            // 
            // gridEX1
            // 
            this.gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.ColumnAutoResize = true;
            this.gridEX1.DataSource = this.table_060_ProvinceInfoBindingSource;
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.GroupByBoxVisible = false;
            this.gridEX1.Location = new System.Drawing.Point(355, 3);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX1.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.Office2007ColorScheme = Janus.Windows.GridEX.Office2007ColorScheme.Custom;
            this.gridEX1.Office2007CustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.Size = new System.Drawing.Size(346, 411);
            this.gridEX1.TabIndex = 12;
            this.gridEX1.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEX1.Error += new Janus.Windows.GridEX.ErrorEventHandler(this.gridEX1_Error);
            this.gridEX1.RecordsDeleted += new System.EventHandler(this.gridEX1_RecordsDeleted);
            this.gridEX1.DeletingRecord += new Janus.Windows.GridEX.RowActionCancelEventHandler(this.gridEX1_DeletingRecord);
            // 
            // table_060_ProvinceInfoTableAdapter
            // 
            this.table_060_ProvinceInfoTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_001_CustomerAdditionalInformationTableAdapter = null;
            this.tableAdapterManager.Table_002_SalesTypesTableAdapter = null;
            this.tableAdapterManager.Table_003_InformationProductCashTableAdapter = null;
            this.tableAdapterManager.table_004_CommodityAndIngredientsTableAdapter = null;
            this.tableAdapterManager.Table_004_ProductAdditionalInformationTableAdapter = null;
            this.tableAdapterManager.Table_005_OrderHeaderTableAdapter = null;
            this.tableAdapterManager.Table_040_PersonGroupsForEditTableAdapter = null;
            this.tableAdapterManager.Table_040_PersonGroupsTableAdapter = null;
            this.tableAdapterManager.Table_045_PersonInfoForEditTableAdapter = null;
            this.tableAdapterManager.Table_045_PersonScopeForEditTableAdapter = null;
            this.tableAdapterManager.Table_045_PersonScopeTableAdapter = null;
            this.tableAdapterManager.Table_060_ProvinceInfoTableAdapter = this.table_060_ProvinceInfoTableAdapter;
            this.tableAdapterManager.Table_065_CityInfoTableAdapter = this.table_065_CityInfoTableAdapter;
            this.tableAdapterManager.UpdateOrder = PSALE.EtelaatePaye.DataSet_EtelaatPayeTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // table_065_CityInfoTableAdapter
            // 
            this.table_065_CityInfoTableAdapter.ClearBeforeFill = true;
            // 
            // Frm_001_Moarefi_Ostan_Shahr
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 442);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_001_Moarefi_Ostan_Shahr";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "معرفی استان  و شهر";
            this.Load += new System.EventHandler(this.Frm_001_Moarefi_Ostan_Shahr_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table_060_ProvinceInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_EtelaatPaye)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_065_CityInfoBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
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
        private System.Windows.Forms.ToolStripButton bt_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Janus.Windows.GridEX.GridEX gridEX2;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private DataSet_EtelaatPaye dataSet_EtelaatPaye;
        private System.Windows.Forms.BindingSource table_060_ProvinceInfoBindingSource;
        private PSALE.EtelaatePaye.DataSet_EtelaatPayeTableAdapters.Table_060_ProvinceInfoTableAdapter table_060_ProvinceInfoTableAdapter;
        private PSALE.EtelaatePaye.DataSet_EtelaatPayeTableAdapters.TableAdapterManager tableAdapterManager;
        private PSALE.EtelaatePaye.DataSet_EtelaatPayeTableAdapters.Table_065_CityInfoTableAdapter table_065_CityInfoTableAdapter;
        private System.Windows.Forms.BindingSource table_065_CityInfoBindingSource;
    }
}