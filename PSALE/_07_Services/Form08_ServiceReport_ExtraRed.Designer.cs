namespace PSHOP._07_Services
{
    partial class Form08_ServiceReport_ExtraRed
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form08_ServiceReport_ExtraRed));
            Janus.Windows.GridEX.GridEXLayout gridEX_Extra_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX_Factors_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
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
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_Extra = new Janus.Windows.GridEX.GridEX();
            this.gridEX_Factors = new Janus.Windows.GridEX.GridEX();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.dataSet_Report = new PSHOP._06_Reports.DataSet_Report();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Extra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Factors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).BeginInit();
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
            this.bindingNavigator1.TabIndex = 2;
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
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Black;
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(110)))));
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2010;
            this.uiPanel0.Id = new System.Guid("154be953-7f99-42ff-92be-553da5aa623d");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("154be953-7f99-42ff-92be-553da5aa623d"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(260, 421), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("154be953-7f99-42ff-92be-553da5aa623d"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(531, 28);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(260, 421);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "اضافات و کسورات";
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.gridEX_Extra);
            this.uiPanel0Container.Location = new System.Drawing.Point(5, 23);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(254, 397);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // gridEX_Extra
            // 
            this.gridEX_Extra.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Extra.AlternatingColors = true;
            this.gridEX_Extra.BuiltInTextsData = resources.GetString("gridEX_Extra.BuiltInTextsData");
            this.gridEX_Extra.ColumnAutoResize = true;
            gridEX_Extra_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Extra_DesignTimeLayout.LayoutString");
            this.gridEX_Extra.DesignTimeLayout = gridEX_Extra_DesignTimeLayout;
            this.gridEX_Extra.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Extra.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Extra.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Extra.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Extra.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Extra.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_Extra.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Extra.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Extra.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.gridEX_Extra.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX_Extra.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX_Extra.GroupByBoxVisible = false;
            this.gridEX_Extra.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX_Extra.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Extra.Name = "gridEX_Extra";
            this.gridEX_Extra.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Extra.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Extra.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_Extra.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Extra.Size = new System.Drawing.Size(254, 397);
            this.gridEX_Extra.TabIndex = 9;
            this.gridEX_Extra.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Extra.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_Extra.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Extra.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Extra.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.gridEX_Extra.CurrentCellChanged += new System.EventHandler(this.gridEX_Extra_CurrentCellChanged);
            // 
            // gridEX_Factors
            // 
            this.gridEX_Factors.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Factors.AlternatingColors = true;
            this.gridEX_Factors.BuiltInTextsData = resources.GetString("gridEX_Factors.BuiltInTextsData");
            this.gridEX_Factors.DataSource = this.bindingSource1;
            gridEX_Factors_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Factors_DesignTimeLayout.LayoutString");
            this.gridEX_Factors.DesignTimeLayout = gridEX_Factors_DesignTimeLayout;
            this.gridEX_Factors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Factors.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Factors.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Factors.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Factors.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Factors.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_Factors.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Factors.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Factors.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.gridEX_Factors.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX_Factors.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX_Factors.GroupByBoxVisible = false;
            this.gridEX_Factors.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX_Factors.Location = new System.Drawing.Point(3, 28);
            this.gridEX_Factors.Name = "gridEX_Factors";
            this.gridEX_Factors.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Factors.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Factors.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_Factors.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Factors.Size = new System.Drawing.Size(528, 421);
            this.gridEX_Factors.TabIndex = 8;
            this.gridEX_Factors.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Factors.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Factors.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Factors.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_Factors.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Factors.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Factors.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.gridEX_Factors.RowDoubleClick += new Janus.Windows.GridEX.RowActionEventHandler(this.gridEX_Factors_RowDoubleClick);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "\"Excel files|*.xls;*.xlsx\"";
            this.saveFileDialog1.RestoreDirectory = true;
            this.saveFileDialog1.Title = "مسیر ذخیره سازی فایل";
            // 
            // gridEXExporter1
            // 
            this.gridEXExporter1.GridEX = this.gridEX_Factors;
            // 
            // dataSet_Report
            // 
            this.dataSet_Report.DataSetName = "DataSet_Report";
            this.dataSet_Report.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Form08_ServiceReport_ExtraRed
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.gridEX_Factors);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form08_ServiceReport_ExtraRed";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "اضافات و کسورات فاکتورهای خدمات";
            this.Activated += new System.EventHandler(this.Form08_ServiceReport_ExtraRed_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form08_ServiceReport_ExtraRed_FormClosing);
            this.Load += new System.EventHandler(this.Form08_ServiceReport_ExtraRed_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form08_ServiceReport_ExtraRed_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Extra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Factors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Report)).EndInit();
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
        private System.Windows.Forms.ToolStripButton bt_Print;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bt_Display;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.GridEX.GridEX gridEX_Extra;
        private Janus.Windows.GridEX.GridEX gridEX_Factors;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private _06_Reports.DataSet_Report dataSet_Report;
        private System.Windows.Forms.ToolStripButton bt_ExportToExcel;
    }
}