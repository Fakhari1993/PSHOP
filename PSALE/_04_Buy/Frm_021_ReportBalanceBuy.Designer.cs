namespace PSHOP._04_Buy
{
    partial class Frm_021_ReportBalanceBuy
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_021_ReportBalanceBuy));
            Janus.Windows.GridEX.GridEXLayout gridEX_Goods_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDate1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.faDate2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.btn_Sarch = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.btn_Print = new System.Windows.Forms.ToolStripButton();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.gridEX_Goods = new Janus.Windows.GridEX.GridEX();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
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
            this.toolStripLabel1,
            this.faDate1,
            this.toolStripLabel2,
            this.faDate2,
            this.btn_Sarch,
            this.toolStripButton2,
            this.btn_Print});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(784, 25);
            this.bindingNavigator1.TabIndex = 8;
            this.bindingNavigator1.Text = "bindingNavigator2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLabel1.Size = new System.Drawing.Size(42, 22);
            this.toolStripLabel1.Text = "از تاریخ:";
            // 
            // faDate1
            // 
            this.faDate1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.faDate1.BackColor = System.Drawing.SystemColors.Window;
            this.faDate1.Name = "faDate1";
            this.faDate1.Size = new System.Drawing.Size(100, 22);
            this.faDate1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePickerStrip1_KeyPress);
            this.faDate1.TextChanged += new System.EventHandler(this.faDatePickerStrip1_TextChanged);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripLabel2.Size = new System.Drawing.Size(41, 22);
            this.toolStripLabel2.Text = "تا تاریخ:";
            // 
            // faDate2
            // 
            this.faDate2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.faDate2.BackColor = System.Drawing.SystemColors.Window;
            this.faDate2.Name = "faDate2";
            this.faDate2.Size = new System.Drawing.Size(100, 22);
            this.faDate2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePickerStrip2_KeyPress);
            this.faDate2.TextChanged += new System.EventHandler(this.faDatePickerStrip2_TextChanged);
            // 
            // btn_Sarch
            // 
            this.btn_Sarch.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.btn_Sarch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btn_Sarch.Image = ((System.Drawing.Image)(resources.GetObject("btn_Sarch.Image")));
            this.btn_Sarch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Sarch.Name = "btn_Sarch";
            this.btn_Sarch.Size = new System.Drawing.Size(23, 22);
            this.btn_Sarch.Text = "toolStripButton1";
            this.btn_Sarch.Click += new System.EventHandler(this.btn_Sarch_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(98, 22);
            this.toolStripButton2.Text = "ارسال به اکسل";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // btn_Print
            // 
            this.btn_Print.Image = ((System.Drawing.Image)(resources.GetObject("btn_Print.Image")));
            this.btn_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Print.Name = "btn_Print";
            this.btn_Print.Size = new System.Drawing.Size(46, 22);
            this.btn_Print.Text = "چاپ";
            this.btn_Print.Click += new System.EventHandler(this.btn_Print_Click);
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            // 
            // gridEX_Goods
            // 
            this.gridEX_Goods.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Goods.AlternatingColors = true;
            this.gridEX_Goods.BuiltInTextsData = resources.GetString("gridEX_Goods.BuiltInTextsData");
            this.gridEX_Goods.DataSource = this.bindingSource1;
            gridEX_Goods_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Goods_DesignTimeLayout.LayoutString");
            this.gridEX_Goods.DesignTimeLayout = gridEX_Goods_DesignTimeLayout;
            this.gridEX_Goods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Goods.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Goods.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Goods.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Goods.FilterRowFormatStyle.BackColor = System.Drawing.Color.Lavender;
            this.gridEX_Goods.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.LavenderBlush;
            this.gridEX_Goods.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Goods.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Goods.GroupByBoxVisible = false;
            this.gridEX_Goods.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX_Goods.Location = new System.Drawing.Point(3, 28);
            this.gridEX_Goods.Name = "gridEX_Goods";
            this.gridEX_Goods.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Goods.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Goods.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_Goods.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.SaveSettings = true;
            this.gridEX_Goods.SettingsKey = "Frm_009_AdditionalGoodsInfo53";
            this.gridEX_Goods.Size = new System.Drawing.Size(778, 411);
            this.gridEX_Goods.TabIndex = 13;
            this.gridEX_Goods.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Goods.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX_Goods.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // gridEXExporter1
            // 
            this.gridEXExporter1.IncludeCollapsedRows = false;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "مسیر ذخیره سازی فایل";
            // 
            // Frm_021_ReportBalanceBuy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 442);
            this.Controls.Add(this.gridEX_Goods);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_021_ReportBalanceBuy";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " سود و زیان بر اساس قیمت خرید";
            this.Load += new System.EventHandler(this.Frm_ReportBalanceBuy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.Configuration.IPersistComponentSettings)(this.gridEX_Goods)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDate1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDate2;
        private System.Windows.Forms.ToolStripButton btn_Sarch;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.GridEX.GridEX gridEX_Goods;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton btn_Print;
        private System.Windows.Forms.BindingSource bindingSource1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}