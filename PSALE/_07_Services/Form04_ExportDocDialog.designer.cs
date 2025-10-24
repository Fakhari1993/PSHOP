namespace PSHOP._07_Services
{
    partial class Form04_ExportDocDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form04_ExportDocDialog));
            Janus.Windows.GridEX.GridEXLayout gridEX1_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.bt_ExportDoc = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.bt_ViewDocs = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.faDatePicker1 = new Janus.Windows.GridEX.EditControls.MaskedEditBox();
            this.txt_Cover = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txt_LastNum = new Janus.Windows.GridEX.EditControls.EditBox();
            this.rdb_New = new Janus.Windows.EditControls.UIRadioButton();
            this.txt_To = new Janus.Windows.GridEX.EditControls.EditBox();
            this.rdb_last = new Janus.Windows.EditControls.UIRadioButton();
            this.rdb_TO = new Janus.Windows.EditControls.UIRadioButton();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
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
            this.bt_ExportDoc,
            this.toolStripSeparator1,
            this.bt_ViewDocs,
            this.toolStripSeparator2});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.Size = new System.Drawing.Size(714, 25);
            this.bindingNavigator1.TabIndex = 7;
            this.bindingNavigator1.Text = "bindingNavigator1";
            // 
            // bt_ExportDoc
            // 
            this.bt_ExportDoc.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_ExportDoc.Image = ((System.Drawing.Image)(resources.GetObject("bt_ExportDoc.Image")));
            this.bt_ExportDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_ExportDoc.Name = "bt_ExportDoc";
            this.bt_ExportDoc.Size = new System.Drawing.Size(75, 22);
            this.bt_ExportDoc.Text = "صدور سند";
            this.bt_ExportDoc.ToolTipText = "Ctrl+S";
            this.bt_ExportDoc.Click += new System.EventHandler(this.bt_ExportDoc_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // bt_ViewDocs
            // 
            this.bt_ViewDocs.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bt_ViewDocs.Image = ((System.Drawing.Image)(resources.GetObject("bt_ViewDocs.Image")));
            this.bt_ViewDocs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.bt_ViewDocs.Name = "bt_ViewDocs";
            this.bt_ViewDocs.Size = new System.Drawing.Size(96, 22);
            this.bt_ViewDocs.Text = "مشاهده اسناد";
            this.bt_ViewDocs.ToolTipText = "Ctrl+W";
            this.bt_ViewDocs.Click += new System.EventHandler(this.bt_ViewDocs_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.TabPage;
            this.uiGroupBox1.Controls.Add(this.faDatePicker1);
            this.uiGroupBox1.Controls.Add(this.txt_Cover);
            this.uiGroupBox1.Controls.Add(this.label7);
            this.uiGroupBox1.Controls.Add(this.label6);
            this.uiGroupBox1.Controls.Add(this.txt_LastNum);
            this.uiGroupBox1.Controls.Add(this.rdb_New);
            this.uiGroupBox1.Controls.Add(this.txt_To);
            this.uiGroupBox1.Controls.Add(this.rdb_last);
            this.uiGroupBox1.Controls.Add(this.rdb_TO);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.uiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.None;
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 391);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiGroupBox1.OfficeCustomColor = System.Drawing.Color.CornflowerBlue;
            this.uiGroupBox1.Size = new System.Drawing.Size(714, 61);
            this.uiGroupBox1.TabIndex = 8;
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker1.Location = new System.Drawing.Point(7, 5);
            this.faDatePicker1.Mask = "0000/00/00";
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.faDatePicker1.Size = new System.Drawing.Size(104, 21);
            this.faDatePicker1.TabIndex = 54;
            this.faDatePicker1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // txt_Cover
            // 
            this.txt_Cover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Cover.Location = new System.Drawing.Point(7, 32);
            this.txt_Cover.Name = "txt_Cover";
            this.txt_Cover.Size = new System.Drawing.Size(630, 21);
            this.txt_Cover.TabIndex = 6;
            this.txt_Cover.Text = "فاکتور خدمات";
            this.txt_Cover.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(639, 36);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "روکش سند:";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(117, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 28;
            this.label6.Text = "تاریخ سند:";
            // 
            // txt_LastNum
            // 
            this.txt_LastNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_LastNum.Enabled = false;
            this.txt_LastNum.Location = new System.Drawing.Point(456, 5);
            this.txt_LastNum.Name = "txt_LastNum";
            this.txt_LastNum.Size = new System.Drawing.Size(44, 21);
            this.txt_LastNum.TabIndex = 2;
            this.txt_LastNum.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // rdb_New
            // 
            this.rdb_New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_New.AutoSize = true;
            this.rdb_New.BackColor = System.Drawing.Color.Transparent;
            this.rdb_New.Checked = true;
            this.rdb_New.Location = new System.Drawing.Point(635, 6);
            this.rdb_New.Name = "rdb_New";
            this.rdb_New.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Black;
            this.rdb_New.Size = new System.Drawing.Size(67, 18);
            this.rdb_New.TabIndex = 0;
            this.rdb_New.TabStop = true;
            this.rdb_New.Text = "سند جدید";
            this.rdb_New.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rdb_New.CheckedChanged += new System.EventHandler(this.rdb_New_CheckedChanged);
            // 
            // txt_To
            // 
            this.txt_To.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_To.Location = new System.Drawing.Point(260, 5);
            this.txt_To.Name = "txt_To";
            this.txt_To.Size = new System.Drawing.Size(49, 21);
            this.txt_To.TabIndex = 4;
            this.txt_To.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.txt_To.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_To_KeyPress);
            this.txt_To.Leave += new System.EventHandler(this.txt_To_Leave);
            // 
            // rdb_last
            // 
            this.rdb_last.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_last.AutoSize = true;
            this.rdb_last.BackColor = System.Drawing.Color.Transparent;
            this.rdb_last.Location = new System.Drawing.Point(497, 6);
            this.rdb_last.Name = "rdb_last";
            this.rdb_last.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Black;
            this.rdb_last.Size = new System.Drawing.Size(116, 18);
            this.rdb_last.TabIndex = 1;
            this.rdb_last.Text = "اضافه به آخرین سند:";
            this.rdb_last.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rdb_last.CheckedChanged += new System.EventHandler(this.rdb_last_CheckedChanged);
            // 
            // rdb_TO
            // 
            this.rdb_TO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_TO.AutoSize = true;
            this.rdb_TO.BackColor = System.Drawing.Color.Transparent;
            this.rdb_TO.Location = new System.Drawing.Point(309, 6);
            this.rdb_TO.Name = "rdb_TO";
            this.rdb_TO.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Black;
            this.rdb_TO.Size = new System.Drawing.Size(120, 18);
            this.rdb_TO.TabIndex = 3;
            this.rdb_TO.Text = "اضافه به سند شماره:";
            this.rdb_TO.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.rdb_TO.CheckedChanged += new System.EventHandler(this.rdb_TO_CheckedChanged);
            // 
            // gridEX1
            // 
            this.gridEX1.AlternatingColors = true;
            this.gridEX1.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.FilterRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX1.FocusCellFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.gridEX1.FocusCellFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Diagonal;
            this.gridEX1.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
            this.gridEX1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.GroupByBoxVisible = false;
            gridEX1_Layout_0.IsCurrentLayout = true;
            gridEX1_Layout_0.Key = "PERP";
            gridEX1_Layout_0.LayoutString = resources.GetString("gridEX1_Layout_0.LayoutString");
            this.gridEX1.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX1_Layout_0});
            this.gridEX1.Location = new System.Drawing.Point(0, 25);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX1.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX1.OfficeCustomColor = System.Drawing.Color.Teal;
            this.gridEX1.RecordNavigator = true;
            this.gridEX1.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.SettingsKey = "Form01_AccDocuments1";
            this.gridEX1.Size = new System.Drawing.Size(714, 366);
            this.gridEX1.TabIndex = 9;
            this.gridEX1.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.Moccasin;
            this.gridEX1.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // Form04_ExportDocDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 452);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.uiGroupBox1);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form04_ExportDocDialog";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "صدور سند حسابداری فاکتور خدمات";
            this.Load += new System.EventHandler(this.Form04_ExportDocDialog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form04_ExportDocDialog_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton bt_ExportDoc;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton bt_ViewDocs;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.GridEX.EditControls.MaskedEditBox faDatePicker1;
        private Janus.Windows.GridEX.EditControls.EditBox txt_Cover;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private Janus.Windows.GridEX.EditControls.EditBox txt_LastNum;
        private Janus.Windows.EditControls.UIRadioButton rdb_New;
        private Janus.Windows.GridEX.EditControls.EditBox txt_To;
        private Janus.Windows.EditControls.UIRadioButton rdb_last;
        private Janus.Windows.EditControls.UIRadioButton rdb_TO;
        private Janus.Windows.GridEX.GridEX gridEX1;
    }
}