namespace PSHOP._05_Sale.LegalFactors.Reports
{
    partial class Form_LegalFactorPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_LegalFactorPrint));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.bt_Display = new DevComponents.DotNetBar.ButtonX();
            this.rdb_CurrentNumber = new System.Windows.Forms.RadioButton();
            this.txt_To = new Janus.Windows.GridEX.EditControls.EditBox();
            this.rdb_FromNumber = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_From = new Janus.Windows.GridEX.EditControls.EditBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.bt_First = new DevComponents.DotNetBar.ButtonItem();
            this.bt_Second = new DevComponents.DotNetBar.ButtonItem();
            this.bt_Third = new DevComponents.DotNetBar.ButtonItem();
            this.bt_Fourth = new DevComponents.DotNetBar.ButtonItem();
            this.bt_Fifth = new DevComponents.DotNetBar.ButtonItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.stiViewerControl1 = new Stimulsoft.Report.Viewer.StiViewerControl();
            this.btn_Custom = new DevComponents.DotNetBar.ButtonItem();
            this.btn_Design = new DevComponents.DotNetBar.ButtonItem();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.bt_Display);
            this.panelEx1.Controls.Add(this.rdb_CurrentNumber);
            this.panelEx1.Controls.Add(this.txt_To);
            this.panelEx1.Controls.Add(this.rdb_FromNumber);
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Controls.Add(this.txt_From);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(794, 61);
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
            this.bt_Display.Location = new System.Drawing.Point(34, 32);
            this.bt_Display.Name = "bt_Display";
            this.bt_Display.Size = new System.Drawing.Size(83, 21);
            this.bt_Display.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bt_Display.TabIndex = 5;
            this.bt_Display.Text = "مشاهده";
            this.bt_Display.Click += new System.EventHandler(this.bt_Display_Click);
            // 
            // rdb_CurrentNumber
            // 
            this.rdb_CurrentNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_CurrentNumber.AutoSize = true;
            this.rdb_CurrentNumber.BackColor = System.Drawing.Color.Transparent;
            this.rdb_CurrentNumber.Checked = true;
            this.rdb_CurrentNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_CurrentNumber.Location = new System.Drawing.Point(700, 10);
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
            this.txt_To.Location = new System.Drawing.Point(453, 32);
            this.txt_To.Name = "txt_To";
            this.txt_To.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.txt_To.OfficeCustomColor = System.Drawing.Color.Black;
            this.txt_To.Size = new System.Drawing.Size(95, 21);
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
            this.rdb_FromNumber.Location = new System.Drawing.Point(713, 34);
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
            this.label1.Location = new System.Drawing.Point(554, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "تا شماره:";
            // 
            // txt_From
            // 
            this.txt_From.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_From.Location = new System.Drawing.Point(610, 32);
            this.txt_From.Name = "txt_From";
            this.txt_From.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.txt_From.OfficeCustomColor = System.Drawing.Color.Black;
            this.txt_From.Size = new System.Drawing.Size(95, 21);
            this.txt_From.TabIndex = 2;
            this.txt_From.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.txt_From.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_From_KeyPress);
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel5.Size = new System.Drawing.Size(792, 363);
            this.panel5.TabIndex = 3;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel2.Size = new System.Drawing.Size(792, 369);
            this.panel2.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel4.Size = new System.Drawing.Size(792, 369);
            this.panel4.TabIndex = 4;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel3.Size = new System.Drawing.Size(792, 369);
            this.panel3.TabIndex = 3;
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bar1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.bt_First,
            this.bt_Second,
            this.bt_Third,
            this.bt_Fourth,
            this.bt_Fifth,
            this.btn_Custom,
            this.btn_Design});
            this.bar1.Location = new System.Drawing.Point(0, 427);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(794, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 8;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // bt_First
            // 
            this.bt_First.BeginGroup = true;
            this.bt_First.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_First.Image = ((System.Drawing.Image)(resources.GetObject("bt_First.Image")));
            this.bt_First.Name = "bt_First";
            this.bt_First.Text = "فاکتور رسمی";
            this.bt_First.Click += new System.EventHandler(this.bt_First_Click);
            // 
            // bt_Second
            // 
            this.bt_Second.BeginGroup = true;
            this.bt_Second.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_Second.Image = ((System.Drawing.Image)(resources.GetObject("bt_Second.Image")));
            this.bt_Second.Name = "bt_Second";
            this.bt_Second.Text = "طرح دو (بدون شماره فاکتور)";
            this.bt_Second.Click += new System.EventHandler(this.bt_Second_Click);
            // 
            // bt_Third
            // 
            this.bt_Third.BeginGroup = true;
            this.bt_Third.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_Third.Image = ((System.Drawing.Image)(resources.GetObject("bt_Third.Image")));
            this.bt_Third.Name = "bt_Third";
            this.bt_Third.Text = "طرح سه";
            this.bt_Third.Click += new System.EventHandler(this.bt_Third_Click);
            // 
            // bt_Fourth
            // 
            this.bt_Fourth.BeginGroup = true;
            this.bt_Fourth.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_Fourth.Image = ((System.Drawing.Image)(resources.GetObject("bt_Fourth.Image")));
            this.bt_Fourth.Name = "bt_Fourth";
            this.bt_Fourth.Text = "طرح چهار";
            this.bt_Fourth.Click += new System.EventHandler(this.bt_Fourth_Click);
            // 
            // bt_Fifth
            // 
            this.bt_Fifth.BeginGroup = true;
            this.bt_Fifth.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_Fifth.Image = ((System.Drawing.Image)(resources.GetObject("bt_Fifth.Image")));
            this.bt_Fifth.Name = "bt_Fifth";
            this.bt_Fifth.Text = "طرح پنجم";
            this.bt_Fifth.Click += new System.EventHandler(this.bt_Fifth_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.crystalReportViewer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 61);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel1.Size = new System.Drawing.Size(794, 366);
            this.panel1.TabIndex = 9;
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
            this.crystalReportViewer1.Size = new System.Drawing.Size(794, 366);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // stiViewerControl1
            // 
            this.stiViewerControl1.AllowDrop = true;
            this.stiViewerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stiViewerControl1.Location = new System.Drawing.Point(0, 61);
            this.stiViewerControl1.Name = "stiViewerControl1";
            this.stiViewerControl1.PageViewMode = Stimulsoft.Report.Viewer.StiPageViewMode.SinglePage;
            this.stiViewerControl1.Report = null;
            this.stiViewerControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.stiViewerControl1.ShowZoom = true;
            this.stiViewerControl1.Size = new System.Drawing.Size(794, 366);
            this.stiViewerControl1.TabIndex = 10;
            this.stiViewerControl1.Visible = false;
            // 
            // btn_Custom
            // 
            this.btn_Custom.BeginGroup = true;
            this.btn_Custom.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_Custom.Image = ((System.Drawing.Image)(resources.GetObject("btn_Custom.Image")));
            this.btn_Custom.Name = "btn_Custom";
            this.btn_Custom.Text = "طرح دلخواه";
            this.btn_Custom.Click += new System.EventHandler(this.btn_Custom_Click);
            // 
            // btn_Design
            // 
            this.btn_Design.Name = "btn_Design";
            this.btn_Design.Text = "طراحی طرح دلخواه";
            this.btn_Design.Click += new System.EventHandler(this.btn_Design_Click);
            // 
            // Form_LegalFactorPrint
            // 
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.stiViewerControl1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form_LegalFactorPrint";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "چاپ فاکتور فروش";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FactorPrint_FormClosing);
            this.Load += new System.EventHandler(this.Form_FactorPrint_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
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
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem bt_First;
        private DevComponents.DotNetBar.ButtonItem bt_Second;
        private DevComponents.DotNetBar.ButtonItem bt_Third;
        private DevComponents.DotNetBar.ButtonItem bt_Fourth;
        private System.Windows.Forms.Panel panel1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private DevComponents.DotNetBar.ButtonItem bt_Fifth;
        private Stimulsoft.Report.Viewer.StiViewerControl stiViewerControl1;
        private DevComponents.DotNetBar.ButtonItem btn_Custom;
        private DevComponents.DotNetBar.ButtonItem btn_Design;
    }
}