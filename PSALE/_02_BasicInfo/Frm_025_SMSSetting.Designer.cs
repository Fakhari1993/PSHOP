namespace PSHOP._02_BasicInfo
{
    partial class Frm_025_SMSSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_025_SMSSetting));
            Janus.Windows.GridEX.GridEXLayout gridEX_Line_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.Common.Layouts.JanusLayoutReference gridEX_Line_DesignTimeLayout_Reference_0 = new Janus.Windows.Common.Layouts.JanusLayoutReference("GridEXLayoutData.RootTable.Columns.Column0.ButtonImage");
            Janus.Windows.GridEX.GridEXLayout gridEX_SmsText_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bt_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel1Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_Line = new Janus.Windows.GridEX.GridEX();
            this.table_175_SMSBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_EtelaatPaye = new PSHOP._02_BasicInfo.DataSet_EtelaatPaye();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_SmsText = new Janus.Windows.GridEX.GridEX();
            this.table_220_SMSTextBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.table_175_SMSTableAdapter = new PSHOP._02_BasicInfo.DataSet_EtelaatPayeTableAdapters.Table_175_SMSTableAdapter();
            this.table_220_SMSTextTableAdapter = new PSHOP._02_BasicInfo.DataSet_EtelaatPayeTableAdapters.Table_220_SMSTextTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            this.uiPanel1Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Line)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_175_SMSBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_EtelaatPaye)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_SmsText)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_220_SMSTextBindingSource)).BeginInit();
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
            this.bt_Save,
            this.toolStripSeparator1});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(794, 25);
            this.bindingNavigator1.TabIndex = 5;
            this.bindingNavigator1.TabStop = true;
            this.bindingNavigator1.Text = "bindingNavigator2";
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
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanel0.Id = new System.Guid("02096c55-6062-4519-836d-2a6f1180ff56");
            this.uiPanel0.StaticGroup = true;
            this.uiPanel1.Id = new System.Guid("4afdde5c-968b-49a8-b79a-69d6940bc685");
            this.uiPanel0.Panels.Add(this.uiPanel1);
            this.uiPanel2.Id = new System.Guid("bad8e7fc-441e-46c1-8efa-0a30fb643118");
            this.uiPanel0.Panels.Add(this.uiPanel2);
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("02096c55-6062-4519-836d-2a6f1180ff56"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(788, 421), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("4afdde5c-968b-49a8-b79a-69d6940bc685"), new System.Guid("02096c55-6062-4519-836d-2a6f1180ff56"), 198, true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("bad8e7fc-441e-46c1-8efa-0a30fb643118"), new System.Guid("02096c55-6062-4519-836d-2a6f1180ff56"), 197, true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("02096c55-6062-4519-836d-2a6f1180ff56"), Janus.Windows.UI.Dock.PanelGroupStyle.HorizontalTiles, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("4afdde5c-968b-49a8-b79a-69d6940bc685"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("bad8e7fc-441e-46c1-8efa-0a30fb643118"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.Location = new System.Drawing.Point(3, 28);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(788, 421);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "Panel 0";
            // 
            // uiPanel1
            // 
            this.uiPanel1.InnerContainer = this.uiPanel1Container;
            this.uiPanel1.Location = new System.Drawing.Point(0, 0);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.Size = new System.Drawing.Size(788, 210);
            this.uiPanel1.TabIndex = 4;
            this.uiPanel1.Text = "لیست خطوط ارسال";
            this.uiPanel1.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel1Container
            // 
            this.uiPanel1Container.Controls.Add(this.gridEX_Line);
            this.uiPanel1Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel1Container.Name = "uiPanel1Container";
            this.uiPanel1Container.Size = new System.Drawing.Size(786, 186);
            this.uiPanel1Container.TabIndex = 0;
            // 
            // gridEX_Line
            // 
            this.gridEX_Line.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Line.AlternatingColors = true;
            this.gridEX_Line.BuiltInTextsData = resources.GetString("gridEX_Line.BuiltInTextsData");
            this.gridEX_Line.DataSource = this.table_175_SMSBindingSource;
            gridEX_Line_DesignTimeLayout_Reference_0.Instance = ((object)(resources.GetObject("gridEX_Line_DesignTimeLayout_Reference_0.Instance")));
            gridEX_Line_DesignTimeLayout.LayoutReferences.AddRange(new Janus.Windows.Common.Layouts.JanusLayoutReference[] {
            gridEX_Line_DesignTimeLayout_Reference_0});
            gridEX_Line_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Line_DesignTimeLayout.LayoutString");
            this.gridEX_Line.DesignTimeLayout = gridEX_Line_DesignTimeLayout;
            this.gridEX_Line.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Line.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Line.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Line.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridEX_Line.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridEX_Line.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Line.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Line.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX_Line.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX_Line.GroupByBoxVisible = false;
            this.gridEX_Line.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Line.Name = "gridEX_Line";
            this.gridEX_Line.NewRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_Line.NewRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridEX_Line.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Line.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Line.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Line.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_Line.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Line.Size = new System.Drawing.Size(786, 186);
            this.gridEX_Line.TabIndex = 9;
            this.gridEX_Line.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridEX_Line.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridEX_Line.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Line.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Line.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX_Line.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.gridEX_Line.Error += new Janus.Windows.GridEX.ErrorEventHandler(this.gridEX1_Error);
            this.gridEX_Line.ColumnButtonClick += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX1_ColumnButtonClick);
            // 
            // table_175_SMSBindingSource
            // 
            this.table_175_SMSBindingSource.DataMember = "Table_175_SMS";
            this.table_175_SMSBindingSource.DataSource = this.dataSet_EtelaatPaye;
            // 
            // dataSet_EtelaatPaye
            // 
            this.dataSet_EtelaatPaye.DataSetName = "DataSet_EtelaatPaye";
            this.dataSet_EtelaatPaye.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // uiPanel2
            // 
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(0, 214);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(788, 207);
            this.uiPanel2.TabIndex = 4;
            this.uiPanel2.Text = "متن پیامکهای ارسالی";
            this.uiPanel2.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.gridEX_SmsText);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 23);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(786, 183);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // gridEX_SmsText
            // 
            this.gridEX_SmsText.AlternatingColors = true;
            this.gridEX_SmsText.BuiltInTextsData = resources.GetString("gridEX_SmsText.BuiltInTextsData");
            this.gridEX_SmsText.DataSource = this.table_220_SMSTextBindingSource;
            gridEX_SmsText_DesignTimeLayout.LayoutString = resources.GetString("gridEX_SmsText_DesignTimeLayout.LayoutString");
            this.gridEX_SmsText.DesignTimeLayout = gridEX_SmsText_DesignTimeLayout;
            this.gridEX_SmsText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_SmsText.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_SmsText.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_SmsText.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridEX_SmsText.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridEX_SmsText.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_SmsText.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_SmsText.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX_SmsText.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX_SmsText.GroupByBoxVisible = false;
            this.gridEX_SmsText.Location = new System.Drawing.Point(0, 0);
            this.gridEX_SmsText.Name = "gridEX_SmsText";
            this.gridEX_SmsText.NewRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_SmsText.NewRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridEX_SmsText.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_SmsText.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_SmsText.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_SmsText.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_SmsText.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_SmsText.Size = new System.Drawing.Size(786, 183);
            this.gridEX_SmsText.TabIndex = 9;
            this.gridEX_SmsText.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.gridEX_SmsText.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.gridEX_SmsText.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_SmsText.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_SmsText.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX_SmsText.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.gridEX_SmsText.Error += new Janus.Windows.GridEX.ErrorEventHandler(this.gridEX1_Error);
            // 
            // table_220_SMSTextBindingSource
            // 
            this.table_220_SMSTextBindingSource.DataMember = "Table_220_SMSText";
            this.table_220_SMSTextBindingSource.DataSource = this.dataSet_EtelaatPaye;
            // 
            // table_175_SMSTableAdapter
            // 
            this.table_175_SMSTableAdapter.ClearBeforeFill = true;
            // 
            // table_220_SMSTextTableAdapter
            // 
            this.table_220_SMSTextTableAdapter.ClearBeforeFill = true;
            // 
            // Frm_025_SMSSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Frm_025_SMSSetting";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تنظیمات ارسال پیامکها";
            this.Load += new System.EventHandler(this.Frm_025_SMSSetting_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_025_SMSSetting_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            this.uiPanel1Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Line)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_175_SMSBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_EtelaatPaye)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_SmsText)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_220_SMSTextBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton bt_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanel0;
        private Janus.Windows.UI.Dock.UIPanel uiPanel1;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel1Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel2;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private Janus.Windows.GridEX.GridEX gridEX_Line;
        private Janus.Windows.GridEX.GridEX gridEX_SmsText;
        private DataSet_EtelaatPaye dataSet_EtelaatPaye;
        private System.Windows.Forms.BindingSource table_175_SMSBindingSource;
        private DataSet_EtelaatPayeTableAdapters.Table_175_SMSTableAdapter table_175_SMSTableAdapter;
        private DataSet_EtelaatPayeTableAdapters.Table_220_SMSTextTableAdapter table_220_SMSTextTableAdapter;
        private System.Windows.Forms.BindingSource table_220_SMSTextBindingSource;
    }
}