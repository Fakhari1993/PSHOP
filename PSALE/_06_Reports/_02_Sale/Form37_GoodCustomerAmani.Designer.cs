namespace PSHOP._06_Reports._02_Sale
{
    partial class Form37_GoodCustomerAmani
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form37_GoodCustomerAmani));
            Janus.Windows.GridEX.GridEXLayout gridEX_Goods_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_Goods = new Janus.Windows.GridEX.GridEX();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.mnu_ExportToExcel_Header = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.bt_Search = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Print = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2010;
            this.uiPanel1.Id = new System.Guid("bfe06f5f-9cd6-4068-a08e-61f62e29c737");
            this.uiPanelManager1.Panels.Add(this.uiPanel1);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("bfe06f5f-9cd6-4068-a08e-61f62e29c737"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(698, 411), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("7f1d55f3-ebdb-40ff-aa01-a33c627ac0e9"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("bfe06f5f-9cd6-4068-a08e-61f62e29c737"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel1
            // 
            this.uiPanel1.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(3, 28);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(698, 411);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "Panel 1";
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.gridEX_Goods);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(696, 409);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // gridEX_Goods
            // 
            this.gridEX_Goods.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Goods.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.AlternatingColors = true;
            this.gridEX_Goods.BuiltInTextsData = resources.GetString("gridEX_Goods.BuiltInTextsData");
            this.gridEX_Goods.CardWidth = 751;
            gridEX_Goods_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Goods_DesignTimeLayout.LayoutString");
            this.gridEX_Goods.DesignTimeLayout = gridEX_Goods_DesignTimeLayout;
            this.gridEX_Goods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Goods.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Goods.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Goods.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Goods.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridEX_Goods.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_Goods.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Goods.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Goods.GroupTotals = Janus.Windows.GridEX.GroupTotals.ExpandedGroup;
            this.gridEX_Goods.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.gridEX_Goods.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Goods.Name = "gridEX_Goods";
            this.gridEX_Goods.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Goods.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Goods.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Goods.RecordNavigator = true;
            this.gridEX_Goods.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.SettingsKey = "gridEX_Goods";
            this.gridEX_Goods.Size = new System.Drawing.Size(696, 409);
            this.gridEX_Goods.TabIndex = 18;
            this.gridEX_Goods.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Goods.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridEX_Goods.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Goods.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Goods.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
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
            this.mnu_ExportToExcel_Header,
            this.toolStripSeparator8,
            this.toolStripLabel1,
            this.faDatePickerStrip1,
            this.toolStripLabel2,
            this.faDatePickerStrip2,
            this.bt_Search,
            this.toolStripSeparator1,
            this.bt_Print,
            this.toolStripSeparator2});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(704, 25);
            this.bindingNavigator1.TabIndex = 13;
            this.bindingNavigator1.Text = "bindingNavigator2";
            // 
            // mnu_ExportToExcel_Header
            // 
            this.mnu_ExportToExcel_Header.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.mnu_ExportToExcel_Header.Image = global::PSHOP.Properties.Resources.Excel;
            this.mnu_ExportToExcel_Header.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.mnu_ExportToExcel_Header.Name = "mnu_ExportToExcel_Header";
            this.mnu_ExportToExcel_Header.Size = new System.Drawing.Size(23, 22);
            this.mnu_ExportToExcel_Header.Text = "ارسال به Excel";
            this.mnu_ExportToExcel_Header.Click += new System.EventHandler(this.mnu_ExportToExcel_Header_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
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
            this.toolStripLabel2.Size = new System.Drawing.Size(41, 22);
            this.toolStripLabel2.Text = "تا تاریخ:";
            // 
            // faDatePickerStrip2
            // 
            this.faDatePickerStrip2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.faDatePickerStrip2.BackColor = System.Drawing.SystemColors.Window;
            this.faDatePickerStrip2.Name = "faDatePickerStrip2";
            this.faDatePickerStrip2.Size = new System.Drawing.Size(100, 22);
            this.faDatePickerStrip2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePickerStrip2_KeyPress);
            this.faDatePickerStrip2.TextChanged += new System.EventHandler(this.faDatePickerStrip1_TextChanged);
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
            this.bt_Search.ToolTipText = "Ctrl+D";
            this.bt_Search.Click += new System.EventHandler(this.bt_Search_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_Print
            // 
            this.bt_Print.Image = global::PSHOP.Properties.Resources.Printer;
            this.bt_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Print.Name = "bt_Print";
            this.bt_Print.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_Print.Size = new System.Drawing.Size(46, 22);
            this.bt_Print.Text = "چاپ";
            this.bt_Print.Visible = false;
            this.bt_Print.Click += new System.EventHandler(this.bt_Print_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "مسیر ذخیره سازی فایل";
            // 
            // Form37_GoodCustomerAmani
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 442);
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form37_GoodCustomerAmani";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "گزارش فروش امانی";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form19_Visitors_FormClosing);
            this.Load += new System.EventHandler(this.Form36_GoodReportByVisitors_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form19_Visitors_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Goods)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton mnu_ExportToExcel_Header;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip2;
        private System.Windows.Forms.ToolStripButton bt_Search;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bt_Print;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private Janus.Windows.GridEX.GridEX gridEX_Goods;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}