namespace PSHOP._06_Reports
{
    partial class Form01_ReportForm
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
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.btn_Design = new DevComponents.DotNetBar.ButtonItem();
            this.stiViewerControl1 = new Stimulsoft.Report.Viewer.StiViewerControl();
            this.btn_Search = new DevComponents.DotNetBar.ButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.SuspendLayout();
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.EnableDrillDown = false;
            this.crystalReportViewer1.EnableRefresh = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowGotoPageButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowParameterPanelButton = false;
            this.crystalReportViewer1.ShowRefreshButton = false;
            this.crystalReportViewer1.ShowTextSearchButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(784, 442);
            this.crystalReportViewer1.TabIndex = 1;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.BackColor = System.Drawing.Color.AliceBlue;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.bar1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btn_Design,
            this.btn_Search});
            this.bar1.Location = new System.Drawing.Point(0, 0);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(784, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Metro;
            this.bar1.TabIndex = 14;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // btn_Design
            // 
            this.btn_Design.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_Design.ForeColor = System.Drawing.Color.Black;
            this.btn_Design.Image = global::PSHOP.Properties.Resources.Printer;
            this.btn_Design.Name = "btn_Design";
            this.btn_Design.Text = "طراحی چاپ";
            this.btn_Design.Click += new System.EventHandler(this.btn_Design_Click);
            // 
            // stiViewerControl1
            // 
            this.stiViewerControl1.AllowDrop = true;
            this.stiViewerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stiViewerControl1.Location = new System.Drawing.Point(0, 25);
            this.stiViewerControl1.Name = "stiViewerControl1";
            this.stiViewerControl1.PageViewMode = Stimulsoft.Report.Viewer.StiPageViewMode.SinglePage;
            this.stiViewerControl1.Report = null;
            this.stiViewerControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.stiViewerControl1.ShowZoom = true;
            this.stiViewerControl1.Size = new System.Drawing.Size(784, 417);
            this.stiViewerControl1.TabIndex = 15;
            this.stiViewerControl1.Visible = false;
            // 
            // btn_Search
            // 
            this.btn_Search.BeginGroup = true;
            this.btn_Search.ForeColor = System.Drawing.Color.Black;
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Text = "مشاهده";
            this.btn_Search.Click += new System.EventHandler(this.btn_Search_Click);
            // 
            // Form01_ReportForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 442);
            this.Controls.Add(this.stiViewerControl1);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.crystalReportViewer1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Form01_ReportForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form01_ReportForm_FormClosing);
            this.Load += new System.EventHandler(this.Form01_ReportForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private DevComponents.DotNetBar.Bar bar1;
        private Stimulsoft.Report.Viewer.StiViewerControl stiViewerControl1;
        private DevComponents.DotNetBar.ButtonItem btn_Design;
        private DevComponents.DotNetBar.ButtonItem btn_Search;
    }
}