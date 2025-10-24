namespace PSHOP._07_Services
{
    partial class Form09_CustomerBill
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form09_CustomerBill));
            Janus.Windows.GridEX.GridEXLayout gridEX_Person_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX_List_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.rpt_CustomerBill_HeaderBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_Sale4 = new PSHOP._05_Sale.DataSet_Sale4();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bt_Print = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.bt_Search = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_Person = new Janus.Windows.GridEX.GridEX();
            this.uiPanel1 = new Janus.Windows.UI.Dock.UIPanelGroup();
            this.uiPanel2 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel2Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_List = new Janus.Windows.GridEX.GridEX();
            this.rpt_CustomerBill_ChildBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rpt_CustomerBill_HeaderTableAdapter = new PSHOP._05_Sale.DataSet_Sale4TableAdapters.Rpt_CustomerBill_HeaderTableAdapter();
            this.rpt_CustomerBill_ChildTableAdapter = new PSHOP._05_Sale.DataSet_Sale4TableAdapters.Rpt_CustomerBill_ChildTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_CustomerBill_HeaderBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Person)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).BeginInit();
            this.uiPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).BeginInit();
            this.uiPanel2.SuspendLayout();
            this.uiPanel2Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_List)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_CustomerBill_ChildBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // rpt_CustomerBill_HeaderBindingSource
            // 
            this.rpt_CustomerBill_HeaderBindingSource.DataMember = "Rpt_CustomerBill_Header";
            this.rpt_CustomerBill_HeaderBindingSource.DataSource = this.dataSet_Sale4;
            // 
            // dataSet_Sale4
            // 
            this.dataSet_Sale4.DataSetName = "DataSet_Sale4";
            this.dataSet_Sale4.EnforceConstraints = false;
            this.dataSet_Sale4.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.bt_Print,
            this.toolStripSeparator8,
            this.toolStripLabel1,
            this.faDatePickerStrip1,
            this.toolStripLabel2,
            this.faDatePickerStrip2,
            this.bt_Search,
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
            this.bindingNavigator1.TabIndex = 12;
            this.bindingNavigator1.Text = "bindingNavigator2";
            // 
            // bt_Print
            // 
            this.bt_Print.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_Print.Image = ((System.Drawing.Image)(resources.GetObject("bt_Print.Image")));
            this.bt_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_Print.Name = "bt_Print";
            this.bt_Print.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.bt_Print.Size = new System.Drawing.Size(46, 22);
            this.bt_Print.Text = "چاپ";
            this.bt_Print.ToolTipText = "Ctrl+P";
            this.bt_Print.Click += new System.EventHandler(this.bt_Print_Click);
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
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.VS2010;
            this.uiPanel0.Id = new System.Guid("57bb7c8a-17cc-4c56-a58f-3f067e65b0c2");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            this.uiPanel1.Id = new System.Guid("4eab18b7-ee90-4ee6-9017-ad39491e5cb0");
            this.uiPanel1.StaticGroup = true;
            this.uiPanel2.Id = new System.Guid("f8468885-0bc9-429e-8368-1572f37fdec0");
            this.uiPanel1.Panels.Add(this.uiPanel2);
            this.uiPanelManager1.Panels.Add(this.uiPanel1);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("57bb7c8a-17cc-4c56-a58f-3f067e65b0c2"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(254, 421), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("4eab18b7-ee90-4ee6-9017-ad39491e5cb0"), Janus.Windows.UI.Dock.PanelGroupStyle.Tab, Janus.Windows.UI.Dock.PanelDockStyle.Fill, true, new System.Drawing.Size(534, 421), true);
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("f8468885-0bc9-429e-8368-1572f37fdec0"), new System.Guid("4eab18b7-ee90-4ee6-9017-ad39491e5cb0"), -1, true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("57bb7c8a-17cc-4c56-a58f-3f067e65b0c2"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("4eab18b7-ee90-4ee6-9017-ad39491e5cb0"), Janus.Windows.UI.Dock.PanelGroupStyle.Tab, true, new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("f8468885-0bc9-429e-8368-1572f37fdec0"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("1d7b0b79-8841-4db5-aaa1-e159bf4fcf12"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(537, 28);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(254, 421);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "مشتریان";
            this.uiPanel0.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.gridEX_Person);
            this.uiPanel0Container.Location = new System.Drawing.Point(5, 21);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(248, 399);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // gridEX_Person
            // 
            this.gridEX_Person.AllowColumnDrag = false;
            this.gridEX_Person.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Person.AlternatingColors = true;
            this.gridEX_Person.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX_Person.ColumnAutoResize = true;
            this.gridEX_Person.DataSource = this.rpt_CustomerBill_HeaderBindingSource;
            this.gridEX_Person.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Person.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Person.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Person.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridEX_Person.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Person.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Person.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Person.GroupByBoxVisible = false;
            this.gridEX_Person.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            gridEX_Person_Layout_0.DataSource = this.rpt_CustomerBill_HeaderBindingSource;
            gridEX_Person_Layout_0.IsCurrentLayout = true;
            gridEX_Person_Layout_0.Key = "PERP";
            gridEX_Person_Layout_0.LayoutString = resources.GetString("gridEX_Person_Layout_0.LayoutString");
            this.gridEX_Person.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX_Person_Layout_0});
            this.gridEX_Person.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Person.Name = "gridEX_Person";
            this.gridEX_Person.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Person.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Person.RecordNavigator = true;
            this.gridEX_Person.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Person.Size = new System.Drawing.Size(248, 399);
            this.gridEX_Person.TabIndex = 9;
            this.gridEX_Person.TotalRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX_Person.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Person.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiPanel1
            // 
            this.uiPanel1.CaptionDisplayMode = Janus.Windows.UI.Dock.PanelCaptionDisplayMode.ImageAndText;
            this.uiPanel1.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel1.GroupStyle = Janus.Windows.UI.Dock.PanelGroupStyle.Tab;
            this.uiPanel1.Image = ((System.Drawing.Image)(resources.GetObject("uiPanel1.Image")));
            this.uiPanel1.Location = new System.Drawing.Point(3, 28);
            this.uiPanel1.Name = "uiPanel1";
            this.uiPanel1.SelectedPanel = this.uiPanel2;
            this.uiPanel1.Size = new System.Drawing.Size(534, 421);
            this.uiPanel1.TabIndex = 4;
            // 
            // uiPanel2
            // 
            this.uiPanel2.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel2.Icon = ((System.Drawing.Icon)(resources.GetObject("uiPanel2.Icon")));
            this.uiPanel2.InnerContainer = this.uiPanel2Container;
            this.uiPanel2.Location = new System.Drawing.Point(0, 0);
            this.uiPanel2.Name = "uiPanel2";
            this.uiPanel2.Size = new System.Drawing.Size(534, 421);
            this.uiPanel2.TabIndex = 4;
            this.uiPanel2.Text = "لیست خدمات";
            // 
            // uiPanel2Container
            // 
            this.uiPanel2Container.Controls.Add(this.gridEX_List);
            this.uiPanel2Container.Location = new System.Drawing.Point(1, 21);
            this.uiPanel2Container.Name = "uiPanel2Container";
            this.uiPanel2Container.Size = new System.Drawing.Size(532, 399);
            this.uiPanel2Container.TabIndex = 0;
            // 
            // gridEX_List
            // 
            this.gridEX_List.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_List.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_List.AlternatingColors = true;
            this.gridEX_List.BuiltInTextsData = resources.GetString("gridEX_List.BuiltInTextsData");
            this.gridEX_List.CardWidth = 751;
            this.gridEX_List.ColumnSetNavigation = Janus.Windows.GridEX.ColumnSetNavigation.ColumnSet;
            this.gridEX_List.DataSource = this.rpt_CustomerBill_ChildBindingSource;
            gridEX_List_DesignTimeLayout.LayoutString = resources.GetString("gridEX_List_DesignTimeLayout.LayoutString");
            this.gridEX_List.DesignTimeLayout = gridEX_List_DesignTimeLayout;
            this.gridEX_List.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_List.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_List.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_List.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_List.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.gridEX_List.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_List.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_List.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_List.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_List.GroupByBoxVisible = false;
            this.gridEX_List.Location = new System.Drawing.Point(0, 0);
            this.gridEX_List.Name = "gridEX_List";
            this.gridEX_List.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_List.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_List.RecordNavigator = true;
            this.gridEX_List.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_List.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_List.SettingsKey = "Form_002_Salefactor1";
            this.gridEX_List.Size = new System.Drawing.Size(532, 399);
            this.gridEX_List.TabIndex = 3;
            this.gridEX_List.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_List.TotalRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_List.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_List.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_List.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX_List.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // rpt_CustomerBill_ChildBindingSource
            // 
            this.rpt_CustomerBill_ChildBindingSource.DataMember = "FK_Rpt_CustomerBill_Header_Rpt_CustomerBill_Child";
            this.rpt_CustomerBill_ChildBindingSource.DataSource = this.rpt_CustomerBill_HeaderBindingSource;
            // 
            // rpt_CustomerBill_HeaderTableAdapter
            // 
            this.rpt_CustomerBill_HeaderTableAdapter.ClearBeforeFill = true;
            // 
            // rpt_CustomerBill_ChildTableAdapter
            // 
            this.rpt_CustomerBill_ChildTableAdapter.ClearBeforeFill = true;
            // 
            // Form09_CustomerBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.uiPanel1);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form09_CustomerBill";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "صورتحساب فروش مشتریان";
            this.Load += new System.EventHandler(this.Form17_CustomerBill_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form17_CustomerBill_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.rpt_CustomerBill_HeaderBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Person)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel1)).EndInit();
            this.uiPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel2)).EndInit();
            this.uiPanel2.ResumeLayout(false);
            this.uiPanel2Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_List)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rpt_CustomerBill_ChildBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton bt_Print;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip2;
        private System.Windows.Forms.ToolStripButton bt_Search;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanelGroup uiPanel1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel2;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel2Container;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.GridEX.GridEX gridEX_Person;
        private Janus.Windows.GridEX.GridEX gridEX_List;
        private _05_Sale.DataSet_Sale4 dataSet_Sale4;
        private System.Windows.Forms.BindingSource rpt_CustomerBill_HeaderBindingSource;
        private _05_Sale.DataSet_Sale4TableAdapters.Rpt_CustomerBill_HeaderTableAdapter rpt_CustomerBill_HeaderTableAdapter;
        private System.Windows.Forms.BindingSource rpt_CustomerBill_ChildBindingSource;
        private _05_Sale.DataSet_Sale4TableAdapters.Rpt_CustomerBill_ChildTableAdapter rpt_CustomerBill_ChildTableAdapter;
    }
}