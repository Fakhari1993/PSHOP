namespace PSHOP._06_Reports._03_Buy
{
    partial class Form46_RequestBuy
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
            Janus.Windows.GridEX.GridEXLayout mlt_Person_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form46_RequestBuy));
            System.Windows.Forms.Label label2;
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.mlt_Person = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.txt_date = new Janus.Windows.GridEX.EditControls.MaskedEditBox();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Person)).BeginInit();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.uiButton1);
            this.uiGroupBox1.Controls.Add(this.txt_date);
            this.uiGroupBox1.Controls.Add(this.mlt_Person);
            this.uiGroupBox1.Controls.Add(label2);
            this.uiGroupBox1.Controls.Add(label1);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(284, 262);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.VisualStyle = Janus.Windows.UI.Dock.PanelVisualStyle.Office2007;
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.Color.Transparent;
            label1.Location = new System.Drawing.Point(233, 50);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(38, 13);
            label1.TabIndex = 48;
            label1.Text = "*تاریخ:";
            // 
            // mlt_Person
            // 
            this.mlt_Person.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            mlt_Person_DesignTimeLayout.LayoutString = resources.GetString("mlt_Person_DesignTimeLayout.LayoutString");
            this.mlt_Person.DesignTimeLayout = mlt_Person_DesignTimeLayout;
            this.mlt_Person.DisplayMember = "column02";
            this.mlt_Person.Location = new System.Drawing.Point(25, 79);
            this.mlt_Person.Name = "mlt_Person";
            this.mlt_Person.SelectedIndex = -1;
            this.mlt_Person.SelectedItem = null;
            this.mlt_Person.Size = new System.Drawing.Size(151, 21);
            this.mlt_Person.TabIndex = 1;
            this.mlt_Person.ValueMember = "columnid";
            this.mlt_Person.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.mlt_Person.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.maskedEditBox1_KeyPress);
            // 
            // label2
            // 
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Location = new System.Drawing.Point(182, 84);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(89, 13);
            label2.TabIndex = 50;
            label2.Text = "*درخواست کننده:";
            // 
            // txt_date
            // 
            this.txt_date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_date.Location = new System.Drawing.Point(25, 42);
            this.txt_date.Mask = "0000/00/00";
            this.txt_date.Name = "txt_date";
            this.txt_date.Size = new System.Drawing.Size(151, 21);
            this.txt_date.TabIndex = 0;
            this.txt_date.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.txt_date.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.maskedEditBox1_KeyPress_1);
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton1.Location = new System.Drawing.Point(47, 123);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(110, 23);
            this.uiButton1.TabIndex = 2;
            this.uiButton1.Text = "ثبت درخواست";
            this.uiButton1.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // Form46_RequestBuy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form46_RequestBuy";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ثبت درخواست خرید";
            this.Load += new System.EventHandler(this.Form46_RequestBuy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Person)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private Janus.Windows.GridEX.EditControls.MaskedEditBox txt_date;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Person;
    }
}