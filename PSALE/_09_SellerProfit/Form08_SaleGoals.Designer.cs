namespace PSHOP._09_SellerProfit
{
    partial class Form08_SaleGoals
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form08_SaleGoals));
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.table_77_SaleGoalBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet1 = new PSHOP._09_SellerProfit.DataSet1();
            this.table_77_SaleGoalTableAdapter = new PSHOP._09_SellerProfit.DataSet1TableAdapters.Table_77_SaleGoalTableAdapter();
            this.tableAdapterManager = new PSHOP._09_SellerProfit.DataSet1TableAdapters.TableAdapterManager();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_77_SaleGoalBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
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
            this.toolStripDropDownButton1,
            this.toolStripSeparator1,
            this.bt_Save,
            this.toolStripSeparator5});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(714, 25);
            this.bindingNavigator1.TabIndex = 5;
            this.bindingNavigator1.TabStop = true;
            this.bindingNavigator1.Text = "bindingNavigator2";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
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
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_Save
            // 
            this.bt_Save.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
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
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // gridEX1
            // 
            this.gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.DataSource = this.table_77_SaleGoalBindingSource;
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.GroupByBoxVisible = false;
            this.gridEX1.Location = new System.Drawing.Point(0, 25);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.NewRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX1.NewRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX1.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.Size = new System.Drawing.Size(714, 427);
            this.gridEX1.TabIndex = 20;
            this.gridEX1.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.gridEX1.DeletingRecord += new Janus.Windows.GridEX.RowActionCancelEventHandler(this.gridEX1_DeletingRecord);
            this.gridEX1.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX1_CellValueChanged);
            this.gridEX1.CellUpdated += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX1_CellUpdated);
            this.gridEX1.Error += new Janus.Windows.GridEX.ErrorEventHandler(this.gridEX1_Error);
            this.gridEX1.AddingRecord += new System.ComponentModel.CancelEventHandler(this.gridEX1_AddingRecord);
            this.gridEX1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.gridEX1_KeyDown);
            // 
            // table_77_SaleGoalBindingSource
            // 
            this.table_77_SaleGoalBindingSource.DataMember = "Table_77_SaleGoal";
            this.table_77_SaleGoalBindingSource.DataSource = this.dataSet1;
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "DataSet1";
            this.dataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // table_77_SaleGoalTableAdapter
            // 
            this.table_77_SaleGoalTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_70_GoodPriceTableAdapter = null;
            this.tableAdapterManager.Table_71_HolidayRatioTableAdapter = null;
            this.tableAdapterManager.Table_72_TimeRatioTableAdapter = null;
            this.tableAdapterManager.Table_73_CustomerGroupRatioTableAdapter = null;
            this.tableAdapterManager.Table_74_SaleManEvaluationTableAdapter = null;
            this.tableAdapterManager.Table_75_SaleTypeTableAdapter = null;
            this.tableAdapterManager.Table_76_AssignGoodTypeTableAdapter = null;
            this.tableAdapterManager.Table_77_SaleGoalTableAdapter = this.table_77_SaleGoalTableAdapter;
            this.tableAdapterManager.Table_78_DiscountRatioTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PSHOP._09_SellerProfit.DataSet1TableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "مسیر ذخیره سازی فایل";
            // 
            // Form08_SaleGoals
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 452);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form08_SaleGoals";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تعریف اهداف فروش";
            this.Load += new System.EventHandler(this.Form08_SaleGoals_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form08_SaleGoals_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_77_SaleGoalBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bt_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private DataSet1 dataSet1;
        private System.Windows.Forms.BindingSource table_77_SaleGoalBindingSource;
        private DataSet1TableAdapters.Table_77_SaleGoalTableAdapter table_77_SaleGoalTableAdapter;
        private DataSet1TableAdapters.TableAdapterManager tableAdapterManager;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}