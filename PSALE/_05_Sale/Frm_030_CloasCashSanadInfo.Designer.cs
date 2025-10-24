namespace PSHOP._05_Sale
{
    partial class Frm_030_CloasCashSanadInfo
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
            Janus.Windows.GridEX.GridEXLayout mlt_ACC_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_030_CloasCashSanadInfo));
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.chk_person = new Janus.Windows.EditControls.UICheckBox();
            this.Btn_Save = new Janus.Windows.EditControls.UIButton();
            this.txt_Cover = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.mlt_ACC = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_ACC)).BeginInit();
            this.SuspendLayout();
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanelManager1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiPanelManager1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.uiPanelManager1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            this.uiPanel0.Id = new System.Guid("f8730298-80e3-4543-bc93-989b4e848445");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("f8730298-80e3-4543-bc93-989b4e848445"), Janus.Windows.UI.Dock.PanelDockStyle.Fill, new System.Drawing.Size(576, 104), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("f8730298-80e3-4543-bc93-989b4e848445"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(3, 3);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(576, 104);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "Panel 0";
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.uiGroupBox1);
            this.uiPanel0Container.Location = new System.Drawing.Point(1, 1);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(574, 102);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.TabPage;
            this.uiGroupBox1.Controls.Add(this.label37);
            this.uiGroupBox1.Controls.Add(this.mlt_ACC);
            this.uiGroupBox1.Controls.Add(this.chk_person);
            this.uiGroupBox1.Controls.Add(this.Btn_Save);
            this.uiGroupBox1.Controls.Add(this.txt_Cover);
            this.uiGroupBox1.Controls.Add(this.label7);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.None;
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiGroupBox1.OfficeCustomColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.uiGroupBox1.Size = new System.Drawing.Size(574, 102);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // chk_person
            // 
            this.chk_person.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_person.BackColor = System.Drawing.Color.Transparent;
            this.chk_person.Location = new System.Drawing.Point(27, 43);
            this.chk_person.Name = "chk_person";
            this.chk_person.Size = new System.Drawing.Size(144, 23);
            this.chk_person.TabIndex = 2;
            this.chk_person.Text = "ثبت در دفتر اشخاص";
            this.chk_person.CheckedChanged += new System.EventHandler(this.chk_person_CheckedChanged);
            // 
            // Btn_Save
            // 
            this.Btn_Save.Location = new System.Drawing.Point(224, 77);
            this.Btn_Save.Name = "Btn_Save";
            this.Btn_Save.Size = new System.Drawing.Size(75, 17);
            this.Btn_Save.TabIndex = 3;
            this.Btn_Save.Text = "صدور سند";
            this.Btn_Save.VisualStyle = Janus.Windows.UI.VisualStyle.Office2010;
            this.Btn_Save.Click += new System.EventHandler(this.Btn_Save_Click);
            // 
            // txt_Cover
            // 
            this.txt_Cover.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_Cover.Location = new System.Drawing.Point(10, 14);
            this.txt_Cover.Name = "txt_Cover";
            this.txt_Cover.Size = new System.Drawing.Size(492, 21);
            this.txt_Cover.TabIndex = 0;
            this.txt_Cover.Text = "بستن صندوق فروش";
            this.txt_Cover.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_Cover_KeyPress);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Location = new System.Drawing.Point(508, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 30;
            this.label7.Text = "شرح سند:";
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label37.AutoSize = true;
            this.label37.BackColor = System.Drawing.Color.Transparent;
            this.label37.Location = new System.Drawing.Point(433, 48);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(137, 13);
            this.label37.TabIndex = 122;
            this.label37.Text = "سرفصل صندوق دریافت نقد:";
            // 
            // mlt_ACC
            // 
            this.mlt_ACC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_ACC.AutoComplete = false;
            mlt_ACC_DesignTimeLayout.LayoutString = resources.GetString("mlt_ACC_DesignTimeLayout.LayoutString");
            this.mlt_ACC.DesignTimeLayout = mlt_ACC_DesignTimeLayout;
            this.mlt_ACC.DisplayMember = "ACC_Name";
            this.mlt_ACC.Location = new System.Drawing.Point(209, 44);
            this.mlt_ACC.Name = "mlt_ACC";
            this.mlt_ACC.SelectedIndex = -1;
            this.mlt_ACC.SelectedItem = null;
            this.mlt_ACC.Size = new System.Drawing.Size(218, 21);
            this.mlt_ACC.TabIndex = 1;
            this.mlt_ACC.ValueMember = "ACC_Code";
            this.mlt_ACC.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.mlt_ACC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_ACC_KeyPress);
            this.mlt_ACC.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mlt_ACC_KeyUp);
            this.mlt_ACC.Leave += new System.EventHandler(this.mlt_ACC_Leave);
            // 
            // Frm_030_CloasCashSanadInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 110);
            this.Controls.Add(this.uiPanel0);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Frm_030_CloasCashSanadInfo";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تنظیمات سند";
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_ACC)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.GridEX.EditControls.EditBox txt_Cover;
        private System.Windows.Forms.Label label7;
        private Janus.Windows.EditControls.UIButton Btn_Save;
        private Janus.Windows.EditControls.UICheckBox chk_person;
        private System.Windows.Forms.Label label37;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_ACC;
    }
}