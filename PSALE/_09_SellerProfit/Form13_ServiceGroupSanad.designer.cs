namespace PSHOP._09_SellerProfit
{
    partial class Form13_ServiceGroupSanad
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
            System.Windows.Forms.Label label7;
            System.Windows.Forms.Label label10;
            Janus.Windows.GridEX.GridEXLayout mlt_Bes_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form13_ServiceGroupSanad));
            Janus.Windows.GridEX.GridEXLayout mlt_Bed_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.txt_sharh = new Janus.Windows.GridEX.EditControls.EditBox();
            this.uiGroupBox3 = new Janus.Windows.EditControls.UIGroupBox();
            this.mlt_Bes = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.label9 = new System.Windows.Forms.Label();
            this.mlt_Bed = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_sanad = new Janus.Windows.EditControls.UIButton();
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.rdb_To = new System.Windows.Forms.RadioButton();
            this.txt_To = new Janus.Windows.GridEX.EditControls.EditBox();
            this.txt_LastNum = new Janus.Windows.GridEX.EditControls.EditBox();
            this.rdb_last = new System.Windows.Forms.RadioButton();
            this.rdb_New = new System.Windows.Forms.RadioButton();
            label7 = new System.Windows.Forms.Label();
            label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).BeginInit();
            this.uiGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Bes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Bed)).BeginInit();
            this.SuspendLayout();
            // 
            // label7
            // 
            label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label7.AutoSize = true;
            label7.BackColor = System.Drawing.Color.Transparent;
            label7.Location = new System.Drawing.Point(145, 67);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(68, 13);
            label7.TabIndex = 71;
            label7.Text = "* تاریخ سند :";
            // 
            // label10
            // 
            label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label10.AutoSize = true;
            label10.BackColor = System.Drawing.Color.Transparent;
            label10.Location = new System.Drawing.Point(645, 98);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(64, 13);
            label10.TabIndex = 73;
            label10.Text = "شرح سند*:";
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackColor = System.Drawing.Color.AliceBlue;
            this.uiGroupBox1.Controls.Add(label10);
            this.uiGroupBox1.Controls.Add(this.txt_sharh);
            this.uiGroupBox1.Controls.Add(this.uiGroupBox3);
            this.uiGroupBox1.Controls.Add(this.btn_sanad);
            this.uiGroupBox1.Controls.Add(this.faDatePicker1);
            this.uiGroupBox1.Controls.Add(label7);
            this.uiGroupBox1.Controls.Add(this.rdb_To);
            this.uiGroupBox1.Controls.Add(this.txt_To);
            this.uiGroupBox1.Controls.Add(this.txt_LastNum);
            this.uiGroupBox1.Controls.Add(this.rdb_last);
            this.uiGroupBox1.Controls.Add(this.rdb_New);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.FrameStyle = Janus.Windows.EditControls.FrameStyle.Top;
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(721, 131);
            this.uiGroupBox1.TabIndex = 0;
            // 
            // txt_sharh
            // 
            this.txt_sharh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_sharh.Location = new System.Drawing.Point(134, 94);
            this.txt_sharh.MaxLength = 1000;
            this.txt_sharh.Multiline = true;
            this.txt_sharh.Name = "txt_sharh";
            this.txt_sharh.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txt_sharh.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt_sharh.Size = new System.Drawing.Size(505, 21);
            this.txt_sharh.TabIndex = 8;
            this.txt_sharh.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.txt_sharh.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_sharh_KeyPress);
            // 
            // uiGroupBox3
            // 
            this.uiGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiGroupBox3.Controls.Add(this.mlt_Bes);
            this.uiGroupBox3.Controls.Add(this.label9);
            this.uiGroupBox3.Controls.Add(this.mlt_Bed);
            this.uiGroupBox3.Controls.Add(this.label3);
            this.uiGroupBox3.Location = new System.Drawing.Point(3, 9);
            this.uiGroupBox3.Name = "uiGroupBox3";
            this.uiGroupBox3.Size = new System.Drawing.Size(715, 47);
            this.uiGroupBox3.TabIndex = 1;
            this.uiGroupBox3.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2010;
            // 
            // mlt_Bes
            // 
            this.mlt_Bes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_Bes.AutoComplete = false;
            mlt_Bes_DesignTimeLayout.LayoutString = resources.GetString("mlt_Bes_DesignTimeLayout.LayoutString");
            this.mlt_Bes.DesignTimeLayout = mlt_Bes_DesignTimeLayout;
            this.mlt_Bes.DisplayMember = "ACC_Name";
            this.mlt_Bes.Location = new System.Drawing.Point(247, 15);
            this.mlt_Bes.Name = "mlt_Bes";
            this.mlt_Bes.SelectedIndex = -1;
            this.mlt_Bes.SelectedItem = null;
            this.mlt_Bes.Size = new System.Drawing.Size(123, 21);
            this.mlt_Bes.TabIndex = 1;
            this.mlt_Bes.ValueMember = "ACC_Code";
            this.mlt_Bes.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.mlt_Bes.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_BuyBed_KeyPress);
            this.mlt_Bes.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mlt_BuyBed_KeyUp);
            this.mlt_Bes.Leave += new System.EventHandler(this.mlt_SaleBed_Leave);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(376, 19);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 13);
            this.label9.TabIndex = 91;
            this.label9.Text = "حساب بستانکار:";
            // 
            // mlt_Bed
            // 
            this.mlt_Bed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_Bed.AutoComplete = false;
            mlt_Bed_DesignTimeLayout.LayoutString = resources.GetString("mlt_Bed_DesignTimeLayout.LayoutString");
            this.mlt_Bed.DesignTimeLayout = mlt_Bed_DesignTimeLayout;
            this.mlt_Bed.DisplayMember = "ACC_Name";
            this.mlt_Bed.Location = new System.Drawing.Point(492, 15);
            this.mlt_Bed.Name = "mlt_Bed";
            this.mlt_Bed.SelectedIndex = -1;
            this.mlt_Bed.SelectedItem = null;
            this.mlt_Bed.Size = new System.Drawing.Size(136, 21);
            this.mlt_Bed.TabIndex = 0;
            this.mlt_Bed.ValueMember = "ACC_Code";
            this.mlt_Bed.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.mlt_Bed.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_BuyBed_KeyPress);
            this.mlt_Bed.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mlt_BuyBed_KeyUp);
            this.mlt_Bed.Leave += new System.EventHandler(this.mlt_SaleBed_Leave);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(632, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 87;
            this.label3.Text = "حساب بدهکار:";
            // 
            // btn_sanad
            // 
            this.btn_sanad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_sanad.Location = new System.Drawing.Point(6, 93);
            this.btn_sanad.Name = "btn_sanad";
            this.btn_sanad.Size = new System.Drawing.Size(105, 23);
            this.btn_sanad.TabIndex = 9;
            this.btn_sanad.Text = "صدور سند";
            this.btn_sanad.Click += new System.EventHandler(this.btn_sanad_Click);
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker1.IsDefault = true;
            this.faDatePicker1.Location = new System.Drawing.Point(6, 63);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(136, 20);
            this.faDatePicker1.TabIndex = 7;
            this.faDatePicker1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker1.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker1_KeyPress);
            // 
            // rdb_To
            // 
            this.rdb_To.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_To.AutoSize = true;
            this.rdb_To.BackColor = System.Drawing.Color.Transparent;
            this.rdb_To.Location = new System.Drawing.Point(307, 65);
            this.rdb_To.Name = "rdb_To";
            this.rdb_To.Size = new System.Drawing.Size(120, 17);
            this.rdb_To.TabIndex = 5;
            this.rdb_To.Text = "اضافه به سند شماره";
            this.rdb_To.UseVisualStyleBackColor = false;
            this.rdb_To.CheckedChanged += new System.EventHandler(this.rdb_To_CheckedChanged);
            // 
            // txt_To
            // 
            this.txt_To.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_To.Location = new System.Drawing.Point(228, 63);
            this.txt_To.Name = "txt_To";
            this.txt_To.Size = new System.Drawing.Size(77, 21);
            this.txt_To.TabIndex = 6;
            this.txt_To.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.txt_To.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_To_KeyPress);
            // 
            // txt_LastNum
            // 
            this.txt_LastNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_LastNum.Enabled = false;
            this.txt_LastNum.Location = new System.Drawing.Point(440, 63);
            this.txt_LastNum.Name = "txt_LastNum";
            this.txt_LastNum.Size = new System.Drawing.Size(81, 21);
            this.txt_LastNum.TabIndex = 4;
            this.txt_LastNum.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.txt_LastNum.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_To_KeyPress);
            // 
            // rdb_last
            // 
            this.rdb_last.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_last.AutoSize = true;
            this.rdb_last.BackColor = System.Drawing.Color.Transparent;
            this.rdb_last.Location = new System.Drawing.Point(520, 65);
            this.rdb_last.Name = "rdb_last";
            this.rdb_last.Size = new System.Drawing.Size(116, 17);
            this.rdb_last.TabIndex = 3;
            this.rdb_last.Text = "اضافه به آخرین سند";
            this.rdb_last.UseVisualStyleBackColor = false;
            this.rdb_last.CheckedChanged += new System.EventHandler(this.rdb_last_CheckedChanged);
            // 
            // rdb_New
            // 
            this.rdb_New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_New.AutoSize = true;
            this.rdb_New.BackColor = System.Drawing.Color.Transparent;
            this.rdb_New.Checked = true;
            this.rdb_New.Location = new System.Drawing.Point(642, 65);
            this.rdb_New.Name = "rdb_New";
            this.rdb_New.Size = new System.Drawing.Size(71, 17);
            this.rdb_New.TabIndex = 2;
            this.rdb_New.TabStop = true;
            this.rdb_New.Text = "سند جدید";
            this.rdb_New.UseVisualStyleBackColor = false;
            this.rdb_New.CheckedChanged += new System.EventHandler(this.rdb_New_CheckedChanged);
            // 
            // Form13_ServiceGroupSanad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 131);
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form13_ServiceGroupSanad";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تنظیمات سند";
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox3)).EndInit();
            this.uiGroupBox3.ResumeLayout(false);
            this.uiGroupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Bes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Bed)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.EditControls.UIButton btn_sanad;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker1;
        private System.Windows.Forms.RadioButton rdb_To;
        private Janus.Windows.GridEX.EditControls.EditBox txt_To;
        private Janus.Windows.GridEX.EditControls.EditBox txt_LastNum;
        private System.Windows.Forms.RadioButton rdb_last;
        private System.Windows.Forms.RadioButton rdb_New;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox3;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Bes;
        private System.Windows.Forms.Label label9;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Bed;
        private System.Windows.Forms.Label label3;
        private Janus.Windows.GridEX.EditControls.EditBox txt_sharh;
    }
}