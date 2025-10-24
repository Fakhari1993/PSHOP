namespace PSHOP._05_Sale
{
    partial class Frm_027_PreFactorToSaleFactor
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
            Janus.Windows.GridEX.GridEXLayout gridEX1_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_027_PreFactorToSaleFactor));
            Janus.Windows.GridEX.GridEXLayout gridEX_List_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX_Extra_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.table_010_SaleFactorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_Sale = new PSHOP._05_Sale.DataSet_Sale();
            this.superTabStrip1 = new DevComponents.DotNetBar.SuperTabStrip();
            this.superTabItem1 = new DevComponents.DotNetBar.SuperTabItem();
            this.bt_No = new DevComponents.DotNetBar.ButtonItem();
            this.bt_Yes = new DevComponents.DotNetBar.ButtonItem();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.table_010_SaleFactorTableAdapter = new PSHOP._05_Sale.DataSet_SaleTableAdapters.Table_010_SaleFactorTableAdapter();
            this.gridEX_List = new Janus.Windows.GridEX.GridEX();
            this.table_011_Child1_SaleFactorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.table_011_Child1_SaleFactorTableAdapter = new PSHOP._05_Sale.DataSet_SaleTableAdapters.Table_011_Child1_SaleFactorTableAdapter();
            this.gridEX_Extra = new Janus.Windows.GridEX.GridEX();
            this.table_012_Child2_SaleFactorBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.table_012_Child2_SaleFactorTableAdapter = new PSHOP._05_Sale.DataSet_SaleTableAdapters.Table_012_Child2_SaleFactorTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.table_010_SaleFactorBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.superTabStrip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_List)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_011_Child1_SaleFactorBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Extra)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_012_Child2_SaleFactorBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // table_010_SaleFactorBindingSource
            // 
            this.table_010_SaleFactorBindingSource.DataMember = "Table_010_SaleFactor";
            this.table_010_SaleFactorBindingSource.DataSource = this.dataSet_Sale;
            // 
            // dataSet_Sale
            // 
            this.dataSet_Sale.DataSetName = "DataSet_Sale";
            this.dataSet_Sale.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // superTabStrip1
            // 
            this.superTabStrip1.AutoSelectAttachedControl = false;
            this.superTabStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.superTabStrip1.BackgroundImage = global::PSHOP.Properties.Resources.me_bg;
            // 
            // 
            // 
            this.superTabStrip1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.superTabStrip1.ContainerControlProcessDialogKey = true;
            // 
            // 
            // 
            // 
            // 
            // 
            this.superTabStrip1.ControlBox.CloseBox.Name = "";
            // 
            // 
            // 
            this.superTabStrip1.ControlBox.MenuBox.Name = "";
            this.superTabStrip1.ControlBox.Name = "";
            this.superTabStrip1.ControlBox.SubItems.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabStrip1.ControlBox.MenuBox,
            this.superTabStrip1.ControlBox.CloseBox});
            this.superTabStrip1.ControlBox.Visible = false;
            this.superTabStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.superTabStrip1.Location = new System.Drawing.Point(0, 0);
            this.superTabStrip1.Name = "superTabStrip1";
            this.superTabStrip1.ReorderTabsEnabled = true;
            this.superTabStrip1.SelectedTabFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.superTabStrip1.SelectedTabIndex = 0;
            this.superTabStrip1.Size = new System.Drawing.Size(516, 24);
            this.superTabStrip1.TabCloseButtonHot = null;
            this.superTabStrip1.TabFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.superTabStrip1.TabIndex = 1;
            this.superTabStrip1.TabLayoutType = DevComponents.DotNetBar.eSuperTabLayoutType.MultiLine;
            this.superTabStrip1.Tabs.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.superTabItem1,
            this.bt_No,
            this.bt_Yes});
            this.superTabStrip1.TabStyle = DevComponents.DotNetBar.eSuperTabStyle.OneNote2007;
            this.superTabStrip1.Text = "superTabStrip1";
            // 
            // superTabItem1
            // 
            this.superTabItem1.GlobalItem = false;
            this.superTabItem1.Name = "superTabItem1";
            this.superTabItem1.PredefinedColor = DevComponents.DotNetBar.eTabItemColor.Teal;
            this.superTabItem1.Text = "اطلاعات فاکتور فروش";
            // 
            // bt_No
            // 
            this.bt_No.BeginGroup = true;
            this.bt_No.ColorTable = DevComponents.DotNetBar.eButtonColor.MagentaWithBackground;
            this.bt_No.Name = "bt_No";
            this.bt_No.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlQ);
            this.bt_No.Text = "لغو Ctrl+Q";
            this.bt_No.Click += new System.EventHandler(this.bt_No_Click);
            // 
            // bt_Yes
            // 
            this.bt_Yes.BeginGroup = true;
            this.bt_Yes.ColorTable = DevComponents.DotNetBar.eButtonColor.MagentaWithBackground;
            this.bt_Yes.Name = "bt_Yes";
            this.bt_Yes.Shortcuts.Add(DevComponents.DotNetBar.eShortcut.CtrlS);
            this.bt_Yes.Text = "صدور فاکتور فروش Ctrl+S";
            this.bt_Yes.Click += new System.EventHandler(this.bt_Yes_Click);
            // 
            // gridEX1
            // 
            this.gridEX1.AcceptsEscape = false;
            this.gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AllowColumnDrag = false;
            this.gridEX1.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX1.BackColor = System.Drawing.Color.DarkTurquoise;
            this.gridEX1.CardBorders = false;
            this.gridEX1.CardColumnHeaderFormatStyle.BackColor = System.Drawing.Color.Transparent;
            this.gridEX1.CardColumnHeaderFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX1.CardHeaders = false;
            this.gridEX1.CardInnerSpacing = 7;
            this.gridEX1.CardSpacing = 3;
            this.gridEX1.CardViewGridlines = Janus.Windows.GridEX.CardViewGridlines.FieldsOnly;
            this.gridEX1.CardWidth = 508;
            this.gridEX1.CenterSingleCard = false;
            this.gridEX1.ColumnAutoResize = true;
            this.gridEX1.DataSource = this.table_010_SaleFactorBindingSource;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.ExpandableCards = false;
            this.gridEX1.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX1.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
            this.gridEX1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.GridLineColor = System.Drawing.Color.Blue;
            this.gridEX1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX1.GroupByBoxVisible = false;
            this.gridEX1.HeaderFormatStyle.BackColor = System.Drawing.Color.Transparent;
            this.gridEX1.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            gridEX1_Layout_0.DataSource = this.table_010_SaleFactorBindingSource;
            gridEX1_Layout_0.IsCurrentLayout = true;
            gridEX1_Layout_0.Key = "PERP";
            gridEX1_Layout_0.LayoutString = resources.GetString("gridEX1_Layout_0.LayoutString");
            this.gridEX1.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX1_Layout_0});
            this.gridEX1.Location = new System.Drawing.Point(0, 24);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.NewRowEnterKeyBehavior = Janus.Windows.GridEX.NewRowEnterKeyBehavior.AddRowAndMoveToAddedRow;
            this.gridEX1.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX1.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.NewRowPosition = Janus.Windows.GridEX.NewRowPosition.BottomRow;
            this.gridEX1.OfficeCustomColor = System.Drawing.Color.Black;
            this.gridEX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridEX1.RowFormatStyle.BackColor = System.Drawing.Color.Transparent;
            this.gridEX1.RowHeaderFormatStyle.BackColor = System.Drawing.Color.SkyBlue;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.Size = new System.Drawing.Size(516, 278);
            this.gridEX1.TabIndex = 18;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX1.View = Janus.Windows.GridEX.View.SingleCard;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.gridEX1.CellValueChanged += new Janus.Windows.GridEX.ColumnActionEventHandler(this.gridEX1_CellValueChanged);
            this.gridEX1.UpdatingCell += new Janus.Windows.GridEX.UpdatingCellEventHandler(this.gridEX1_UpdatingCell);
            this.gridEX1.Error += new Janus.Windows.GridEX.ErrorEventHandler(this.gridEX1_Error);
            this.gridEX1.CurrentCellChanged += new System.EventHandler(this.gridEX1_CurrentCellChanged);
            // 
            // table_010_SaleFactorTableAdapter
            // 
            this.table_010_SaleFactorTableAdapter.ClearBeforeFill = true;
            // 
            // gridEX_List
            // 
            this.gridEX_List.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_List.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_List.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_List.AlternatingColors = true;
            this.gridEX_List.CardWidth = 751;
            this.gridEX_List.ColumnSetNavigation = Janus.Windows.GridEX.ColumnSetNavigation.ColumnSet;
            this.gridEX_List.DataSource = this.table_011_Child1_SaleFactorBindingSource;
            gridEX_List_DesignTimeLayout.LayoutString = resources.GetString("gridEX_List_DesignTimeLayout.LayoutString");
            this.gridEX_List.DesignTimeLayout = gridEX_List_DesignTimeLayout;
            this.gridEX_List.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_List.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_List.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_List.GroupByBoxVisible = false;
            this.gridEX_List.Location = new System.Drawing.Point(0, 0);
            this.gridEX_List.Name = "gridEX_List";
            this.gridEX_List.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_List.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_List.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_List.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_List.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_List.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_List.SaveSettings = true;
            this.gridEX_List.SettingsKey = "Form_SaleFactor40";
            this.gridEX_List.Size = new System.Drawing.Size(516, 302);
            this.gridEX_List.TabIndex = 19;
            this.gridEX_List.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_List.TotalRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_List.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_List.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_List.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX_List.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // table_011_Child1_SaleFactorBindingSource
            // 
            this.table_011_Child1_SaleFactorBindingSource.DataMember = "Table_011_Child1_SaleFactor";
            this.table_011_Child1_SaleFactorBindingSource.DataSource = this.dataSet_Sale;
            // 
            // table_011_Child1_SaleFactorTableAdapter
            // 
            this.table_011_Child1_SaleFactorTableAdapter.ClearBeforeFill = true;
            // 
            // gridEX_Extra
            // 
            this.gridEX_Extra.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Extra.AllowDelete = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Extra.AlternatingColors = true;
            this.gridEX_Extra.CardWidth = 751;
            this.gridEX_Extra.ColumnSetNavigation = Janus.Windows.GridEX.ColumnSetNavigation.ColumnSet;
            this.gridEX_Extra.DataSource = this.table_012_Child2_SaleFactorBindingSource;
            gridEX_Extra_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Extra_DesignTimeLayout.LayoutString");
            this.gridEX_Extra.DesignTimeLayout = gridEX_Extra_DesignTimeLayout;
            this.gridEX_Extra.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Extra.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Extra.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Extra.GroupByBoxVisible = false;
            this.gridEX_Extra.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Extra.Name = "gridEX_Extra";
            this.gridEX_Extra.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Extra.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Extra.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Extra.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Extra.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_Extra.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Extra.Size = new System.Drawing.Size(516, 302);
            this.gridEX_Extra.TabIndex = 20;
            this.gridEX_Extra.TableHeaderFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX_Extra.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Extra.TotalRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Extra.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Extra.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Extra.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX_Extra.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // table_012_Child2_SaleFactorBindingSource
            // 
            this.table_012_Child2_SaleFactorBindingSource.DataMember = "Table_012_Child2_SaleFactor";
            this.table_012_Child2_SaleFactorBindingSource.DataSource = this.dataSet_Sale;
            // 
            // table_012_Child2_SaleFactorTableAdapter
            // 
            this.table_012_Child2_SaleFactorTableAdapter.ClearBeforeFill = true;
            // 
            // Frm_027_PreFactorToSaleFactor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 302);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.superTabStrip1);
            this.Controls.Add(this.gridEX_List);
            this.Controls.Add(this.gridEX_Extra);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Frm_027_PreFactorToSaleFactor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Frm_027_PreFactorToSaleFactor";
            this.Load += new System.EventHandler(this.Frm_027_PreFactorToSaleFactor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.table_010_SaleFactorBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.superTabStrip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            ((System.Configuration.IPersistComponentSettings)(this.gridEX_List)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_List)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_011_Child1_SaleFactorBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Extra)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_012_Child2_SaleFactorBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.SuperTabStrip superTabStrip1;
        private DevComponents.DotNetBar.SuperTabItem superTabItem1;
        private DevComponents.DotNetBar.ButtonItem bt_Yes;
        private DevComponents.DotNetBar.ButtonItem bt_No;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private DataSet_Sale dataSet_Sale;
        private System.Windows.Forms.BindingSource table_010_SaleFactorBindingSource;
        private DataSet_SaleTableAdapters.Table_010_SaleFactorTableAdapter table_010_SaleFactorTableAdapter;
        private Janus.Windows.GridEX.GridEX gridEX_List;
        private System.Windows.Forms.BindingSource table_011_Child1_SaleFactorBindingSource;
        private DataSet_SaleTableAdapters.Table_011_Child1_SaleFactorTableAdapter table_011_Child1_SaleFactorTableAdapter;
        private Janus.Windows.GridEX.GridEX gridEX_Extra;
        private System.Windows.Forms.BindingSource table_012_Child2_SaleFactorBindingSource;
        private DataSet_SaleTableAdapters.Table_012_Child2_SaleFactorTableAdapter table_012_Child2_SaleFactorTableAdapter;
    }
}