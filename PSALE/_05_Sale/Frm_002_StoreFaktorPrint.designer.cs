namespace PSHOP._05_Sale
{
    partial class Frm_002_StoreFaktorPrint
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
            Janus.Windows.GridEX.GridEXLayout mlt_Customer_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_002_StoreFaktorPrint));
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.uiButton1 = new Janus.Windows.EditControls.UIButton();
            this.mlt_Customer = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.faDatePicker2 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Customer)).BeginInit();
            this.SuspendLayout();
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackColor = System.Drawing.Color.AliceBlue;
            this.uiGroupBox1.Controls.Add(this.faDatePicker2);
            this.uiGroupBox1.Controls.Add(this.label1);
            this.uiGroupBox1.Controls.Add(this.faDatePicker1);
            this.uiGroupBox1.Controls.Add(this.uiButton1);
            this.uiGroupBox1.Controls.Add(this.mlt_Customer);
            this.uiGroupBox1.Controls.Add(this.label9);
            this.uiGroupBox1.Controls.Add(this.label8);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(387, 310);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = " ";
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker1.Font = new System.Drawing.Font("B Mitra", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.faDatePicker1.IsDefault = true;
            this.faDatePicker1.Location = new System.Drawing.Point(47, 60);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(200, 40);
            this.faDatePicker1.TabIndex = 0;
            this.faDatePicker1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker1.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker1_KeyPress);
            // 
            // uiButton1
            // 
            this.uiButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiButton1.Location = new System.Drawing.Point(112, 246);
            this.uiButton1.Name = "uiButton1";
            this.uiButton1.Size = new System.Drawing.Size(75, 32);
            this.uiButton1.TabIndex = 3;
            this.uiButton1.Text = "چاپ";
            this.uiButton1.ToolTipText = "Ctrl+P";
            this.uiButton1.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // mlt_Customer
            // 
            this.mlt_Customer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mlt_Customer.AutoComplete = false;
            mlt_Customer_DesignTimeLayout.LayoutString = resources.GetString("mlt_Customer_DesignTimeLayout.LayoutString");
            this.mlt_Customer.DesignTimeLayout = mlt_Customer_DesignTimeLayout;
            this.mlt_Customer.DisplayMember = "name";
            this.mlt_Customer.Location = new System.Drawing.Point(47, 176);
            this.mlt_Customer.Margin = new System.Windows.Forms.Padding(6);
            this.mlt_Customer.Name = "mlt_Customer";
            this.mlt_Customer.SelectedIndex = -1;
            this.mlt_Customer.SelectedItem = null;
            this.mlt_Customer.Size = new System.Drawing.Size(200, 37);
            this.mlt_Customer.TabIndex = 2;
            this.mlt_Customer.ValueMember = "id";
            this.mlt_Customer.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.mlt_Customer.TextChanged += new System.EventHandler(this.mlt_Customer_TextChanged);
            this.mlt_Customer.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_Customer_KeyPress);
            this.mlt_Customer.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mlt_Customer_KeyUp);
            this.mlt_Customer.Leave += new System.EventHandler(this.mlt_Customer_Leave);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Location = new System.Drawing.Point(263, 180);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 29);
            this.label9.TabIndex = 23;
            this.label9.Text = "خریدار*:";
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Location = new System.Drawing.Point(263, 66);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(109, 29);
            this.label8.TabIndex = 22;
            this.label8.Text = "از تاریخ فاکتور*:";
            // 
            // faDatePicker2
            // 
            this.faDatePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker2.Font = new System.Drawing.Font("B Mitra", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.faDatePicker2.IsDefault = true;
            this.faDatePicker2.Location = new System.Drawing.Point(47, 117);
            this.faDatePicker2.Name = "faDatePicker2";
            this.faDatePicker2.Size = new System.Drawing.Size(200, 40);
            this.faDatePicker2.TabIndex = 1;
            this.faDatePicker2.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker2.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker1_KeyPress);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(263, 123);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 29);
            this.label1.TabIndex = 25;
            this.label1.Text = "تا تاریخ فاکتور*:";
            // 
            // Frm_002_StoreFaktorPrint
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(387, 310);
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("B Mitra", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Frm_002_StoreFaktorPrint";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "چاپ تجمیعی فاکتور فروش";
            this.Load += new System.EventHandler(this.Frm_002_StoreFaktorPrint_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_002_StoreFaktorPrint_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_Customer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_Customer;
        private Janus.Windows.EditControls.UIButton uiButton1;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker1;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker2;
        private System.Windows.Forms.Label label1;
    }
}