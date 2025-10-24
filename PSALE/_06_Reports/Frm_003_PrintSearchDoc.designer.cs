namespace PSHOP._06_Reports
{
    partial class Frm_003_PrintSearchDoc
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
            this.label5 = new System.Windows.Forms.Label();
            this.cmbPage = new System.Windows.Forms.ComboBox();
            this.panelEx1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.Office2010;
            this.panelEx1.Controls.Add(this.label5);
            this.panelEx1.Controls.Add(this.cmbPage);
            this.panelEx1.Controls.Add(this.checkwithLogo);
            this.panelEx1.Controls.Add(this.btDesign);
            this.panelEx1.Controls.Add(this.bt_Print);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.panelEx1.Size = new System.Drawing.Size(704, 39);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.Color = System.Drawing.Color.Azure;
            this.panelEx1.Style.BackColor2.Color = System.Drawing.Color.PowderBlue;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 7;
            // 
            // checkwithLogo
            // 
            this.checkwithLogo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkwithLogo.AutoSize = true;
            this.checkwithLogo.Location = new System.Drawing.Point(601, 11);
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
            this.stiViewerControl1.Size = new System.Drawing.Size(704, 403);
            this.stiViewerControl1.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(542, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "الگو چاپ :";
            // 
            // cmbPage
            // 
            this.cmbPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPage.BackColor = System.Drawing.Color.White;
            this.cmbPage.DisplayMember = "Name";
            this.cmbPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPage.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cmbPage.ForeColor = System.Drawing.Color.Black;
            this.cmbPage.FormattingEnabled = true;
            this.cmbPage.Location = new System.Drawing.Point(324, 9);
            this.cmbPage.Name = "cmbPage";
            this.cmbPage.Size = new System.Drawing.Size(212, 21);
            this.cmbPage.TabIndex = 24;
            this.cmbPage.ValueMember = "ID";
            // 
            // Frm_003_PrintSearchDoc
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 442);
            this.Controls.Add(this.stiViewerControl1);
            this.Controls.Add(this.panelEx1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Frm_003_PrintSearchDoc";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.Text = "گزارش دفتر روزنامه";
            this.Load += new System.EventHandler(this.Frm_003_PrintSearchDoc_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private Janus.Windows.EditControls.UIButton btDesign;
        private Janus.Windows.EditControls.UIButton bt_Print;
        private Stimulsoft.Report.Viewer.StiViewerControl stiViewerControl1;
        private System.Windows.Forms.CheckBox checkwithLogo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbPage;
    }
}