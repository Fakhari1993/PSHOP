namespace PSHOP._04_Buy.Reports
{
    partial class Form_ReturnBuyFactorPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_ReturnBuyFactorPrint));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.bt_Display = new DevComponents.DotNetBar.ButtonX();
            this.label2 = new System.Windows.Forms.Label();
            this.faDatePicker2 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.rdb_Date = new System.Windows.Forms.RadioButton();
            this.rdb_CurrentNumber = new System.Windows.Forms.RadioButton();
            this.txt_To = new Janus.Windows.GridEX.EditControls.EditBox();
            this.rdb_FromNumber = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_From = new Janus.Windows.GridEX.EditControls.EditBox();
            this.crystalReportViewer6 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel5 = new System.Windows.Forms.Panel();
            this.crystalReportViewer5 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.crystalReportViewer4 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.bt_First = new DevComponents.DotNetBar.ButtonItem();
            this.bt_Second = new DevComponents.DotNetBar.ButtonItem();
            this.bt_Last = new DevComponents.DotNetBar.ButtonItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.chk_Logo = new System.Windows.Forms.CheckBox();
            this.panelEx1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.chk_Logo);
            this.panelEx1.Controls.Add(this.bt_Display);
            this.panelEx1.Controls.Add(this.label2);
            this.panelEx1.Controls.Add(this.faDatePicker2);
            this.panelEx1.Controls.Add(this.faDatePicker1);
            this.panelEx1.Controls.Add(this.rdb_Date);
            this.panelEx1.Controls.Add(this.rdb_CurrentNumber);
            this.panelEx1.Controls.Add(this.txt_To);
            this.panelEx1.Controls.Add(this.rdb_FromNumber);
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Controls.Add(this.txt_From);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(794, 56);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 4;
            // 
            // bt_Display
            // 
            this.bt_Display.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bt_Display.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bt_Display.Location = new System.Drawing.Point(9, 7);
            this.bt_Display.Name = "bt_Display";
            this.bt_Display.Size = new System.Drawing.Size(84, 21);
            this.bt_Display.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bt_Display.TabIndex = 9;
            this.bt_Display.Text = "مشاهده";
            this.bt_Display.Click += new System.EventHandler(this.bt_Display_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(213, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "تا تاریخ:";
            // 
            // faDatePicker2
            // 
            this.faDatePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker2.Location = new System.Drawing.Point(112, 7);
            this.faDatePicker2.Name = "faDatePicker2";
            this.faDatePicker2.Size = new System.Drawing.Size(95, 20);
            this.faDatePicker2.TabIndex = 8;
            this.faDatePicker2.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker2.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker2_KeyPress);
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker1.Location = new System.Drawing.Point(260, 7);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(95, 20);
            this.faDatePicker1.TabIndex = 6;
            this.faDatePicker1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker1.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker1_KeyPress);
            // 
            // rdb_Date
            // 
            this.rdb_Date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_Date.AutoSize = true;
            this.rdb_Date.BackColor = System.Drawing.Color.Transparent;
            this.rdb_Date.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_Date.Location = new System.Drawing.Point(364, 9);
            this.rdb_Date.Name = "rdb_Date";
            this.rdb_Date.Size = new System.Drawing.Size(60, 17);
            this.rdb_Date.TabIndex = 5;
            this.rdb_Date.TabStop = true;
            this.rdb_Date.Text = "از تاریخ:";
            this.rdb_Date.UseVisualStyleBackColor = false;
            // 
            // rdb_CurrentNumber
            // 
            this.rdb_CurrentNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_CurrentNumber.AutoSize = true;
            this.rdb_CurrentNumber.BackColor = System.Drawing.Color.Transparent;
            this.rdb_CurrentNumber.Checked = true;
            this.rdb_CurrentNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_CurrentNumber.Location = new System.Drawing.Point(706, 9);
            this.rdb_CurrentNumber.Name = "rdb_CurrentNumber";
            this.rdb_CurrentNumber.Size = new System.Drawing.Size(82, 17);
            this.rdb_CurrentNumber.TabIndex = 0;
            this.rdb_CurrentNumber.TabStop = true;
            this.rdb_CurrentNumber.Text = "شماره جاری";
            this.rdb_CurrentNumber.UseVisualStyleBackColor = false;
            // 
            // txt_To
            // 
            this.txt_To.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_To.Location = new System.Drawing.Point(435, 7);
            this.txt_To.Name = "txt_To";
            this.txt_To.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.txt_To.OfficeCustomColor = System.Drawing.Color.Black;
            this.txt_To.Size = new System.Drawing.Size(69, 21);
            this.txt_To.TabIndex = 4;
            this.txt_To.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.txt_To.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_From_KeyPress);
            // 
            // rdb_FromNumber
            // 
            this.rdb_FromNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_FromNumber.AutoSize = true;
            this.rdb_FromNumber.BackColor = System.Drawing.Color.Transparent;
            this.rdb_FromNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_FromNumber.Location = new System.Drawing.Point(627, 9);
            this.rdb_FromNumber.Name = "rdb_FromNumber";
            this.rdb_FromNumber.Size = new System.Drawing.Size(69, 17);
            this.rdb_FromNumber.TabIndex = 1;
            this.rdb_FromNumber.TabStop = true;
            this.rdb_FromNumber.Text = "از شماره:";
            this.rdb_FromNumber.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(505, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "تا شماره:";
            // 
            // txt_From
            // 
            this.txt_From.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_From.Location = new System.Drawing.Point(558, 7);
            this.txt_From.Name = "txt_From";
            this.txt_From.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.txt_From.OfficeCustomColor = System.Drawing.Color.Black;
            this.txt_From.Size = new System.Drawing.Size(69, 21);
            this.txt_From.TabIndex = 2;
            this.txt_From.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.txt_From.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_From_KeyPress);
            // 
            // crystalReportViewer6
            // 
            this.crystalReportViewer6.ActiveViewIndex = -1;
            this.crystalReportViewer6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer6.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer6.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer6.Name = "crystalReportViewer6";
            this.crystalReportViewer6.ShowCloseButton = false;
            this.crystalReportViewer6.ShowGotoPageButton = false;
            this.crystalReportViewer6.ShowGroupTreeButton = false;
            this.crystalReportViewer6.ShowParameterPanelButton = false;
            this.crystalReportViewer6.ShowRefreshButton = false;
            this.crystalReportViewer6.ShowTextSearchButton = false;
            this.crystalReportViewer6.Size = new System.Drawing.Size(790, 341);
            this.crystalReportViewer6.TabIndex = 1;
            this.crystalReportViewer6.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.crystalReportViewer5);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel5.Size = new System.Drawing.Size(792, 363);
            this.panel5.TabIndex = 3;
            // 
            // crystalReportViewer5
            // 
            this.crystalReportViewer5.ActiveViewIndex = -1;
            this.crystalReportViewer5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer5.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer5.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer5.Name = "crystalReportViewer5";
            this.crystalReportViewer5.ShowCloseButton = false;
            this.crystalReportViewer5.ShowGotoPageButton = false;
            this.crystalReportViewer5.ShowGroupTreeButton = false;
            this.crystalReportViewer5.ShowParameterPanelButton = false;
            this.crystalReportViewer5.ShowRefreshButton = false;
            this.crystalReportViewer5.ShowTextSearchButton = false;
            this.crystalReportViewer5.Size = new System.Drawing.Size(792, 363);
            this.crystalReportViewer5.TabIndex = 0;
            this.crystalReportViewer5.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.crystalReportViewer4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel4.Size = new System.Drawing.Size(792, 355);
            this.panel4.TabIndex = 4;
            // 
            // crystalReportViewer4
            // 
            this.crystalReportViewer4.ActiveViewIndex = -1;
            this.crystalReportViewer4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer4.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer4.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer4.Name = "crystalReportViewer4";
            this.crystalReportViewer4.ShowCloseButton = false;
            this.crystalReportViewer4.ShowGotoPageButton = false;
            this.crystalReportViewer4.ShowGroupTreeButton = false;
            this.crystalReportViewer4.ShowParameterPanelButton = false;
            this.crystalReportViewer4.ShowRefreshButton = false;
            this.crystalReportViewer4.ShowTextSearchButton = false;
            this.crystalReportViewer4.Size = new System.Drawing.Size(792, 355);
            this.crystalReportViewer4.TabIndex = 0;
            this.crystalReportViewer4.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bar1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.bt_First,
            this.bt_Second,
            this.bt_Last});
            this.bar1.Location = new System.Drawing.Point(0, 427);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(794, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 5;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // bt_First
            // 
            this.bt_First.BeginGroup = true;
            this.bt_First.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_First.Image = ((System.Drawing.Image)(resources.GetObject("bt_First.Image")));
            this.bt_First.Name = "bt_First";
            this.bt_First.Text = "طرح یک";
            this.bt_First.Click += new System.EventHandler(this.bt_First_Click);
            // 
            // bt_Second
            // 
            this.bt_Second.BeginGroup = true;
            this.bt_Second.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_Second.Image = ((System.Drawing.Image)(resources.GetObject("bt_Second.Image")));
            this.bt_Second.Name = "bt_Second";
            this.bt_Second.Text = "طرح دو";
            this.bt_Second.Click += new System.EventHandler(this.bt_Second_Click);
            // 
            // bt_Last
            // 
            this.bt_Last.BeginGroup = true;
            this.bt_Last.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_Last.Image = ((System.Drawing.Image)(resources.GetObject("bt_Last.Image")));
            this.bt_Last.Name = "bt_Last";
            this.bt_Last.Text = "طرح سه";
            this.bt_Last.Click += new System.EventHandler(this.bt_Last_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.crystalReportViewer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 56);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel1.Size = new System.Drawing.Size(794, 371);
            this.panel1.TabIndex = 6;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowGotoPageButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowParameterPanelButton = false;
            this.crystalReportViewer1.ShowRefreshButton = false;
            this.crystalReportViewer1.ShowTextSearchButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(794, 371);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // chk_Logo
            // 
            this.chk_Logo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_Logo.AutoSize = true;
            this.chk_Logo.Location = new System.Drawing.Point(640, 37);
            this.chk_Logo.Name = "chk_Logo";
            this.chk_Logo.Size = new System.Drawing.Size(148, 17);
            this.chk_Logo.TabIndex = 13;
            this.chk_Logo.Text = "چاپ بدون لوگو و نام شرکت";
            this.chk_Logo.UseVisualStyleBackColor = true;
            // 
            // Form_ReturnBuyFactorPrint
            // 
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form_ReturnBuyFactorPrint";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "چاپ فاکتور مرجوعی خرید";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_ReturnBuyFactorPrint_FormClosing);
            this.Load += new System.EventHandler(this.Form_FactorPrint_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX bt_Display;
        private System.Windows.Forms.RadioButton rdb_CurrentNumber;
        private Janus.Windows.GridEX.EditControls.EditBox txt_To;
        private System.Windows.Forms.RadioButton rdb_FromNumber;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.EditControls.EditBox txt_From;
        private System.Windows.Forms.Panel panel5;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer5;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer6;
        private System.Windows.Forms.Panel panel4;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer4;
        private System.Windows.Forms.Label label2;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker2;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker1;
        private System.Windows.Forms.RadioButton rdb_Date;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem bt_First;
        private DevComponents.DotNetBar.ButtonItem bt_Second;
        private DevComponents.DotNetBar.ButtonItem bt_Last;
        private System.Windows.Forms.Panel panel1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.CheckBox chk_Logo;
    }
}