namespace PSHOP._06_Reports
{
    partial class Frm_Rpt_SanadTypeSale
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
            Janus.Windows.GridEX.GridEXLayout gridEX_Doc_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_Rpt_SanadTypeSale));
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bt_Print = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip1 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.faDatePickerStrip2 = new FarsiLibrary.Win.Controls.FADatePickerStrip();
            this.bt_Search = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.gridEX_Doc = new Janus.Windows.GridEX.GridEX();
            this.filterEditor1 = new Janus.Windows.FilterEditor.FilterEditor();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Doc)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
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
            this.bindingNavigator1.Size = new System.Drawing.Size(784, 25);
            this.bindingNavigator1.TabIndex = 11;
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
            this.faDatePickerStrip2.TextChanged += new System.EventHandler(this.faDatePickerStrip2_TextChanged);
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
            // gridEX_Doc
            // 
            this.gridEX_Doc.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Doc.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Doc.AlternatingColors = true;
            this.gridEX_Doc.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX_Doc.DataSource = this.bindingSource1;
            this.gridEX_Doc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Doc.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Doc.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Doc.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Doc.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX_Doc.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Doc.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Doc.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Doc.GroupByBoxVisible = false;
            gridEX_Doc_Layout_0.DataSource = this.bindingSource1;
            gridEX_Doc_Layout_0.IsCurrentLayout = true;
            gridEX_Doc_Layout_0.Key = "PERP";
            gridEX_Doc_Layout_0.LayoutString = resources.GetString("gridEX_Doc_Layout_0.LayoutString");
            this.gridEX_Doc.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX_Doc_Layout_0});
            this.gridEX_Doc.Location = new System.Drawing.Point(3, 72);
            this.gridEX_Doc.Name = "gridEX_Doc";
            this.gridEX_Doc.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Doc.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Doc.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Doc.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Doc.RecordNavigator = true;
            this.gridEX_Doc.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_Doc.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Doc.SaveSettings = true;
            this.gridEX_Doc.SettingsKey = "Tab1_49";
            this.gridEX_Doc.Size = new System.Drawing.Size(778, 367);
            this.gridEX_Doc.TabIndex = 22;
            this.gridEX_Doc.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Doc.TotalRowFormatStyle.BackColor = System.Drawing.Color.Lavender;
            this.gridEX_Doc.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Horizontal;
            this.gridEX_Doc.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Doc.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // filterEditor1
            // 
            this.filterEditor1.AllowFilterByFieldValue = true;
            this.filterEditor1.AutoApply = true;
            this.filterEditor1.AutomaticHeightResize = true;
            this.filterEditor1.BackColor = System.Drawing.Color.MintCream;
            this.filterEditor1.BuiltInTextsData = resources.GetString("filterEditor1.BuiltInTextsData");
            this.filterEditor1.ColorScheme = "FilterEditorColorScheme";
            this.filterEditor1.Dock = System.Windows.Forms.DockStyle.Top;
            this.filterEditor1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.filterEditor1.InnerAreaStyle = Janus.Windows.UI.Dock.PanelInnerAreaStyle.UseFormatStyle;
            this.filterEditor1.Location = new System.Drawing.Point(0, 25);
            this.filterEditor1.MinSize = new System.Drawing.Size(16, 44);
            this.filterEditor1.Name = "filterEditor1";
            this.filterEditor1.OfficeColorScheme = Janus.Windows.Common.OfficeColorScheme.Custom;
            this.filterEditor1.OfficeCustomColor = System.Drawing.Color.SkyBlue;
            this.filterEditor1.ScrollMode = Janus.Windows.UI.Dock.ScrollMode.Both;
            this.filterEditor1.ScrollStep = 15;
            this.filterEditor1.Size = new System.Drawing.Size(784, 44);
            this.filterEditor1.SortFieldList = false;
            this.filterEditor1.SourceControl = this.gridEX_Doc;
            this.filterEditor1.VisualStyle = Janus.Windows.Common.VisualStyle.Office2007;
            this.filterEditor1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.filterEditor1_KeyPress);
            // 
            // Frm_Rpt_SanadTypeSale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 442);
            this.Controls.Add(this.gridEX_Doc);
            this.Controls.Add(this.filterEditor1);
            this.Controls.Add(this.bindingNavigator1);
            this.Name = "Frm_Rpt_SanadTypeSale";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "گزارش مرور اسناد روزانه";
            this.Load += new System.EventHandler(this.Frm_Rpt_SanadTypeSale_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.Configuration.IPersistComponentSettings)(this.gridEX_Doc)).LoadComponentSettings();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Doc)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton bt_Print;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private FarsiLibrary.Win.Controls.FADatePickerStrip faDatePickerStrip2;
        private System.Windows.Forms.ToolStripButton bt_Search;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private Janus.Windows.GridEX.GridEX gridEX_Doc;
        private Janus.Windows.FilterEditor.FilterEditor filterEditor1;
        private System.Windows.Forms.BindingSource bindingSource1;
    }
}