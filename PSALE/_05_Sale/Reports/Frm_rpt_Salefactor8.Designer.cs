namespace PSHOP._05_Sale.Reports
{
    partial class Frm_rpt_Salefactor8
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
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.btn_Design = new DevComponents.DotNetBar.ButtonItem();
            this.stiViewerControl1 = new Stimulsoft.Report.Viewer.StiViewerControl();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.SuspendLayout();
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bar1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btn_Design,
            this.buttonItem1});
            this.bar1.Location = new System.Drawing.Point(0, 417);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(784, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.bar1.TabIndex = 14;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // btn_Design
            // 
            this.btn_Design.ForeColor = System.Drawing.Color.Black;
            this.btn_Design.Name = "btn_Design";
            this.btn_Design.Text = "طراحی فاکتور";
            this.btn_Design.Click += new System.EventHandler(this.button1_Click);
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
            this.stiViewerControl1.Size = new System.Drawing.Size(784, 417);
            this.stiViewerControl1.TabIndex = 15;
            // 
            // buttonItem1
            // 
            this.buttonItem1.ForeColor = System.Drawing.Color.Black;
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "مشاهده";
            this.buttonItem1.Click += new System.EventHandler(this.buttonItem1_Click);
            // 
            // Frm_rpt_Salefactor8
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 442);
            this.Controls.Add(this.stiViewerControl1);
            this.Controls.Add(this.bar1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_rpt_Salefactor8";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "فاکتور فروش 8 سانتی";
            this.Load += new System.EventHandler(this.Frm_rpt_Salefactor8_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem btn_Design;
        private Stimulsoft.Report.Viewer.StiViewerControl stiViewerControl1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
    }
}