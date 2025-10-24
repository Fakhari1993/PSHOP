namespace PSHOP._08_Order2
{
    partial class Form05_DraftTime
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
            this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.bt_Yes = new Janus.Windows.EditControls.UIButton();
            this.bt_No = new Janus.Windows.EditControls.UIButton();
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
            this.uiTab1.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // uiTab1
            // 
            this.uiTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTab1.FirstTabOffset = 3;
            this.uiTab1.Location = new System.Drawing.Point(0, 0);
            this.uiTab1.Name = "uiTab1";
            this.uiTab1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiTab1.OfficeCustomColor = System.Drawing.Color.Crimson;
            this.uiTab1.Size = new System.Drawing.Size(369, 57);
            this.uiTab1.TabDisplay = Janus.Windows.UI.Tab.TabDisplay.Image;
            this.uiTab1.TabIndex = 0;
            this.uiTab1.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1});
            this.uiTab1.TabStripAlignment = Janus.Windows.UI.Tab.TabStripAlignment.Right;
            this.uiTab1.VisualStyle = Janus.Windows.UI.Tab.TabVisualStyle.Office2010;
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.bt_Yes);
            this.uiTabPage1.Controls.Add(this.bt_No);
            this.uiTabPage1.Controls.Add(this.faDatePicker1);
            this.uiTabPage1.Controls.Add(this.label1);
            this.uiTabPage1.Location = new System.Drawing.Point(1, 1);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.Size = new System.Drawing.Size(345, 55);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "New Tab";
            // 
            // bt_Yes
            // 
            this.bt_Yes.Location = new System.Drawing.Point(88, 10);
            this.bt_Yes.Name = "bt_Yes";
            this.bt_Yes.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Black;
            this.bt_Yes.OfficeCustomColor = System.Drawing.Color.MediumSlateBlue;
            this.bt_Yes.Size = new System.Drawing.Size(75, 23);
            this.bt_Yes.TabIndex = 1;
            this.bt_Yes.Text = "تأیید";
            this.bt_Yes.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.bt_Yes.Click += new System.EventHandler(this.bt_Yes_Click);
            // 
            // bt_No
            // 
            this.bt_No.Location = new System.Drawing.Point(5, 10);
            this.bt_No.Name = "bt_No";
            this.bt_No.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Black;
            this.bt_No.OfficeCustomColor = System.Drawing.Color.MediumSlateBlue;
            this.bt_No.Size = new System.Drawing.Size(75, 23);
            this.bt_No.TabIndex = 2;
            this.bt_No.Text = "لغو";
            this.bt_No.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.bt_No.Click += new System.EventHandler(this.bt_No_Click);
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Location = new System.Drawing.Point(170, 11);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(117, 20);
            this.faDatePicker1.TabIndex = 0;
            this.faDatePicker1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker1.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker1_KeyPress);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(289, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "تاریخ حواله";
            // 
            // Form05_DraftTime
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(369, 57);
            this.ControlBox = false;
            this.Controls.Add(this.uiTab1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form05_DraftTime";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "تاریخ حواله";
            this.Load += new System.EventHandler(this.Form05_CancelOrdersDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
            this.uiTab1.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            this.uiTabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.UI.Tab.UITab uiTab1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
        private System.Windows.Forms.Label label1;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker1;
        private Janus.Windows.EditControls.UIButton bt_Yes;
        private Janus.Windows.EditControls.UIButton bt_No;

    }
}