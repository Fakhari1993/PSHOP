namespace PSHOP._05_Sale
{
    partial class Frm_035_TransferInformationDialog
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label column04Label;
            Janus.Windows.GridEX.GridEXLayout mlt_FunctionRecipt_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_035_TransferInformationDialog));
            Janus.Windows.GridEX.GridEXLayout mlt_FunctionDraft_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.mlt_FunctionRecipt = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.mlt_FunctionDraft = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.uiButton2 = new Janus.Windows.EditControls.UIButton();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            label1 = new System.Windows.Forms.Label();
            column04Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_FunctionRecipt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_FunctionDraft)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Location = new System.Drawing.Point(347, 21);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(54, 13);
            label1.TabIndex = 65;
            label1.Text = "نوع رسید:";
            // 
            // column04Label
            // 
            column04Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            column04Label.AutoSize = true;
            column04Label.BackColor = System.Drawing.Color.Transparent;
            column04Label.Location = new System.Drawing.Point(347, 48);
            column04Label.Name = "column04Label";
            column04Label.Size = new System.Drawing.Size(53, 13);
            column04Label.TabIndex = 63;
            column04Label.Text = "نوع حواله:";
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.TabPage;
            this.uiGroupBox1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.uiGroupBox1.Controls.Add(this.mlt_FunctionRecipt);
            this.uiGroupBox1.Controls.Add(label1);
            this.uiGroupBox1.Controls.Add(this.mlt_FunctionDraft);
            this.uiGroupBox1.Controls.Add(column04Label);
            this.uiGroupBox1.Controls.Add(this.uiButton2);
            this.uiGroupBox1.Controls.Add(this.uiButton1);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.None;
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiGroupBox1.OfficeCustomColor = System.Drawing.Color.SlateBlue;
            this.uiGroupBox1.Size = new System.Drawing.Size(412, 117);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // mlt_FunctionRecipt
            // 
            this.mlt_FunctionRecipt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            mlt_FunctionRecipt_DesignTimeLayout.LayoutString = resources.GetString("mlt_FunctionRecipt_DesignTimeLayout.LayoutString");
            this.mlt_FunctionRecipt.DesignTimeLayout = mlt_FunctionRecipt_DesignTimeLayout;
            this.mlt_FunctionRecipt.DisplayMember = "column02";
            this.mlt_FunctionRecipt.Location = new System.Drawing.Point(12, 16);
            this.mlt_FunctionRecipt.Name = "mlt_FunctionRecipt";
            this.mlt_FunctionRecipt.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Black;
            this.mlt_FunctionRecipt.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.mlt_FunctionRecipt.SelectedIndex = -1;
            this.mlt_FunctionRecipt.SelectedItem = null;
            this.mlt_FunctionRecipt.Size = new System.Drawing.Size(329, 21);
            this.mlt_FunctionRecipt.TabIndex = 64;
            this.mlt_FunctionRecipt.ValueMember = "columnid";
            this.mlt_FunctionRecipt.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // mlt_FunctionDraft
            // 
            this.mlt_FunctionDraft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            mlt_FunctionDraft_DesignTimeLayout.LayoutString = resources.GetString("mlt_FunctionDraft_DesignTimeLayout.LayoutString");
            this.mlt_FunctionDraft.DesignTimeLayout = mlt_FunctionDraft_DesignTimeLayout;
            this.mlt_FunctionDraft.DisplayMember = "column02";
            this.mlt_FunctionDraft.Location = new System.Drawing.Point(12, 43);
            this.mlt_FunctionDraft.Name = "mlt_FunctionDraft";
            this.mlt_FunctionDraft.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Black;
            this.mlt_FunctionDraft.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.mlt_FunctionDraft.SelectedIndex = -1;
            this.mlt_FunctionDraft.SelectedItem = null;
            this.mlt_FunctionDraft.Size = new System.Drawing.Size(329, 21);
            this.mlt_FunctionDraft.TabIndex = 62;
            this.mlt_FunctionDraft.ValueMember = "columnid";
            this.mlt_FunctionDraft.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // uiButton2
            // 
            this.uiButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton2.Location = new System.Drawing.Point(12, 82);
            this.uiButton2.Name = "uiButton2";
            this.uiButton2.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiButton2.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.uiButton2.Size = new System.Drawing.Size(175, 23);
            this.uiButton2.TabIndex = 59;
            this.uiButton2.Text = "لغو      Ctrl+Q";
            this.uiButton2.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiButton2.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton1.Location = new System.Drawing.Point(222, 82);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiButton1.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.uiButton1.Size = new System.Drawing.Size(175, 23);
            this.uiButton1.TabIndex = 58;
            this.uiButton1.Text = "صدور حواله    Ctrl+S";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // Frm_035_TransferInformationDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 117);
            this.ControlBox = false;
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "Frm_035_TransferInformationDialog";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "اطلاعات حواله انبار";
            this.Load += new System.EventHandler(this.Frm_010_DraftInformationDialog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_010_DraftInformationDialog_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_FunctionRecipt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_FunctionDraft)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.EditControls.UIButton uiButton2;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_FunctionRecipt;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_FunctionDraft;
    }
}