namespace PSHOP._04_Buy.Reports
{
    partial class Frm_Rpt_Blance
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
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.checkwithLogo = new System.Windows.Forms.CheckBox();
            this.btDesign = new Janus.Windows.EditControls.UIButton();
            this.bt_Print = new Janus.Windows.EditControls.UIButton();
            this.stiViewerControl1 = new Stimulsoft.Report.Viewer.StiViewerControl();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.panelEx1.Controls.Add(this.checkwithLogo);
            this.panelEx1.Controls.Add(this.btDesign);
            this.panelEx1.Controls.Add(this.bt_Print);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelEx1.Size = new System.Drawing.Size(684, 39);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.Azure;
            this.panelEx1.Style.BackColor2.Color = System.Drawing.Color.PowderBlue;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 8;
            // 
            // checkwithLogo
            // 
            this.checkwithLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkwithLogo.AutoSize = true;
            this.checkwithLogo.Location = new System.Drawing.Point(581, 11);
            this.checkwithLogo.Name = "checkwithLogo";
            this.checkwithLogo.Size = new System.Drawing.Size(90, 17);
            this.checkwithLogo.TabIndex = 20;
            this.checkwithLogo.Text = "چاپ بدون لوگو";
            this.checkwithLogo.UseVisualStyleBackColor = true;
            // 
            // btDesign
            // 
            this.btDesign.Location = new System.Drawing.Point(12, 9);
            this.btDesign.Name = "btDesign";
            this.btDesign.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.btDesign.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btDesign.Size = new System.Drawing.Size(109, 23);
            this.btDesign.TabIndex = 19;
            this.btDesign.Text = "طراحی";
            this.btDesign.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.btDesign.Click += new System.EventHandler(this.btDesign_Click);
            // 
            // bt_Print
            // 
            this.bt_Print.Location = new System.Drawing.Point(127, 9);
            this.bt_Print.Name = "bt_Print";
            this.bt_Print.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.bt_Print.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.bt_Print.Size = new System.Drawing.Size(109, 23);
            this.bt_Print.TabIndex = 19;
            this.bt_Print.Text = "نمایش";
            this.bt_Print.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.bt_Print.Click += new System.EventHandler(this.bt_Print_Click);
            // 
            // stiViewerControl1
            // 
            this.stiViewerControl1.AllowDrop = true;
            this.stiViewerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stiViewerControl1.Location = new System.Drawing.Point(0, 39);
            this.stiViewerControl1.Name = "stiViewerControl1";
            this.stiViewerControl1.PageViewMode = Stimulsoft.Report.Viewer.StiPageViewMode.SinglePage;
            this.stiViewerControl1.Report = null;
            this.stiViewerControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.stiViewerControl1.ShowZoom = true;
            this.stiViewerControl1.Size = new System.Drawing.Size(684, 413);
            this.stiViewerControl1.TabIndex = 9;
            // 
            // Frm_Rpt_Blance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 452);
            this.Controls.Add(this.stiViewerControl1);
            this.Controls.Add(this.panelEx1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_Rpt_Blance";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "گزارش";
            this.Load += new System.EventHandler(this.Frm_Rpt_Blance_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.CheckBox checkwithLogo;
        private Janus.Windows.EditControls.UIButton btDesign;
        private Janus.Windows.EditControls.UIButton bt_Print;
        private Stimulsoft.Report.Viewer.StiViewerControl stiViewerControl1;
    }
}