namespace PSHOP._06_Reports._02_Sale
{
    partial class Form18_MarginReport_GoodCustomerFactor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form18_MarginReport_GoodCustomerFactor));
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.bt_ExportToExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_Display = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
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
            this.bt_ExportToExcel.Size = new System.Drawing.Size(23, 22);
            this.bt_ExportToExcel.ToolTipText = "Excel ارسال به";
            this.bt_ExportToExcel.Click += new System.EventHandler(this.bt_ExportToExcel_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // gridEX1
            // 
            this.gridEX1.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.BuiltInTextsData = resources.GetString("gridEX1.BuiltInTextsData");
            gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX1.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX1.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridEX1.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridEX1.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX1.GroupRowFormatStyle.BackColor = System.Drawing.Color.Lavender;
            this.gridEX1.GroupRowFormatStyle.BackColorGradient = System.Drawing.Color.LightCyan;
            this.gridEX1.GroupRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.GroupRowFormatStyle.Font = new System.Drawing.Font("B Yekan", 9F);
            this.gridEX1.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridEX1.GroupTotalRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridEX1.GroupTotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.GroupTotalRowFormatStyle.Font = new System.Drawing.Font("B Yekan", 9F);
            this.gridEX1.GroupTotalRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX1.GroupTotals = Janus.Windows.GridEX.GroupTotals.Always;
            this.gridEX1.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX1.Location = new System.Drawing.Point(0, 25);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.SaveSettings = true;
            this.gridEX1.SettingsKey = "Form18_MarginReport_GoodCustomerFactor1";
            this.gridEX1.Size = new System.Drawing.Size(794, 427);
            this.gridEX1.TabIndex = 2;
            this.gridEX1.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridEX1.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridEX1.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.TreeCollapseGlyph = ((System.Drawing.Image)(resources.GetObject("gridEX1.TreeCollapseGlyph")));
            this.gridEX1.TreeExpandGlyph = ((System.Drawing.Image)(resources.GetObject("gridEX1.TreeExpandGlyph")));
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.gridEX1.FilterApplied += new System.EventHandler(this.gridEX1_FilterApplied);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "مسیر ذخیره سازی فایل";
            // 
            // Form18_MarginReport_GoodCustomerFactor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form18_MarginReport_GoodCustomerFactor";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "گزارش سود ناویژه فاکتور /کالا /مشتری";
            this.Activated += new System.EventHandler(this.Form18_MarginReport_GoodCustomerFactor_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form18_MarginReport_GoodCustomerFactor_FormClosing);
            this.Load += new System.EventHandler(this.Form18_MarginReport_GoodCustomerFactor_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form18_MarginReport_GoodCustomerFactor_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.Configuration.IPersistComponentSettings)(this.gridEX1)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip2;
        private System.Windows.Forms.ToolStripButton bt_ExportToExcel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton bt_Display;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}