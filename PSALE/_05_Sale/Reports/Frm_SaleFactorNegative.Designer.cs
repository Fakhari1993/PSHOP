namespace PSHOP._05_Sale.Reports
{
    partial class Frm_SaleFactorNegative
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_SaleFactorNegative));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn_dig = new System.Windows.Forms.ToolStripButton();
            this.btn_Prw = new System.Windows.Forms.ToolStripButton();
            this.stiViewerControl1 = new Stimulsoft.Report.Viewer.StiViewerControl();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn_dig,
            this.btn_Prw});
            this.toolStrip1.Location = new System.Drawing.Point(0, 482);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(813, 25);
            this.toolStrip1.TabIndex = 111;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btn_dig
            // 
            this.btn_dig.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_dig.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_dig.Image = ((System.Drawing.Image)(resources.GetObject("btn_dig.Image")));
            this.btn_dig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_dig.Name = "btn_dig";
            this.btn_dig.Size = new System.Drawing.Size(95, 22);
            this.btn_dig.Text = "طراحی دلخواه";
            this.btn_dig.Click += new System.EventHandler(this.btn_dig_Click);
            // 
            // btn_Prw
            // 
            this.btn_Prw.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btn_Prw.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_Prw.Image = ((System.Drawing.Image)(resources.GetObject("btn_Prw.Image")));
            this.btn_Prw.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn_Prw.Name = "btn_Prw";
            this.btn_Prw.Size = new System.Drawing.Size(59, 22);
            this.btn_Prw.Text = "مشاهده";
            this.btn_Prw.Click += new System.EventHandler(this.btn_Prw_Click);
            // 
            // stiViewerControl1
            // 
            this.stiViewerControl1.AllowDrop = true;
            this.stiViewerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stiViewerControl1.Location = new System.Drawing.Point(0, 0);
            this.stiViewerControl1.Name = "stiViewerControl1";
            this.stiViewerControl1.PageViewMode = Stimulsoft.Report.Viewer.StiPageViewMode.SinglePage;
            this.stiViewerControl1.Report = null;
            this.stiViewerControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.stiViewerControl1.ShowZoom = true;
            this.stiViewerControl1.Size = new System.Drawing.Size(813, 482);
            this.stiViewerControl1.TabIndex = 112;
            // 
            // Frm_SaleFactorNegative
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(813, 507);
            this.Controls.Add(this.stiViewerControl1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_SaleFactorNegative";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "گزارش موجودی منفی روزانه";
            this.Load += new System.EventHandler(this.Frm_SaleFactorNegative_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn_dig;
        private System.Windows.Forms.ToolStripButton btn_Prw;
        private Stimulsoft.Report.Viewer.StiViewerControl stiViewerControl1;
    }
}