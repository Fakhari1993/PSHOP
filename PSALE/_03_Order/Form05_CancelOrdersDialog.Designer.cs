namespace PSHOP._03_Order
{
    partial class Form05_CancelOrdersDialog
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
            this.officeFormAdorner1 = new Janus.Windows.Ribbon.OfficeFormAdorner(this.components);
            this.uiTab1 = new Janus.Windows.UI.Tab.UITab();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.bt_Yes = new Janus.Windows.EditControls.UIButton();
            this.bt_No = new Janus.Windows.EditControls.UIButton();
            this.txt_Cause = new Janus.Windows.GridEX.EditControls.EditBox();
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.officeFormAdorner1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).BeginInit();
            this.uiTab1.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // officeFormAdorner1
            // 
            this.officeFormAdorner1.Form = this;
            this.officeFormAdorner1.OfficeColorScheme = Janus.Windows.Ribbon.OfficeColorScheme.Black;
            this.officeFormAdorner1.OfficeCustomColor = System.Drawing.Color.DarkOrchid;
            this.officeFormAdorner1.TitleBarFont = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            // 
            // uiTab1
            // 
            this.uiTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiTab1.FirstTabOffset = 3;
            this.uiTab1.Location = new System.Drawing.Point(0, 0);
            this.uiTab1.Name = "uiTab1";
            this.uiTab1.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.uiTab1.OfficeCustomColor = System.Drawing.Color.Crimson;
            this.uiTab1.Size = new System.Drawing.Size(391, 0);
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
            this.uiTabPage1.Controls.Add(this.txt_Cause);
            this.uiTabPage1.Controls.Add(this.faDatePicker1);
            this.uiTabPage1.Controls.Add(this.label2);
            this.uiTabPage1.Controls.Add(this.label1);
            this.uiTabPage1.Location = new System.Drawing.Point(1, 1);
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.Size = new System.Drawing.Size(367, 0);
            this.uiTabPage1.TabStop = true;
            this.uiTabPage1.Text = "New Tab";
            // 
            // bt_Yes
            // 
            this.bt_Yes.Location = new System.Drawing.Point(94, 10);
            this.bt_Yes.Name = "bt_Yes";
            this.bt_Yes.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Black;
            this.bt_Yes.OfficeCustomColor = System.Drawing.Color.MediumSlateBlue;
            this.bt_Yes.Size = new System.Drawing.Size(75, 23);
            this.bt_Yes.TabIndex = 2;
            this.bt_Yes.Text = "تأیید";
            this.bt_Yes.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.bt_Yes.Click += new System.EventHandler(this.bt_Yes_Click);
            // 
            // bt_No
            // 
            this.bt_No.Location = new System.Drawing.Point(11, 10);
            this.bt_No.Name = "bt_No";
            this.bt_No.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Black;
            this.bt_No.OfficeCustomColor = System.Drawing.Color.MediumSlateBlue;
            this.bt_No.Size = new System.Drawing.Size(75, 23);
            this.bt_No.TabIndex = 3;
            this.bt_No.Text = "لغو";
            this.bt_No.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.bt_No.Click += new System.EventHandler(this.bt_No_Click);
            // 
            // txt_Cause
            // 
            this.txt_Cause.Location = new System.Drawing.Point(11, 41);
            this.txt_Cause.Name = "txt_Cause";
            this.txt_Cause.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Silver;
            this.txt_Cause.Size = new System.Drawing.Size(314, 21);
            this.txt_Cause.TabIndex = 1;
            this.txt_Cause.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Location = new System.Drawing.Point(208, 11);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(117, 20);
            this.faDatePicker1.TabIndex = 0;
            this.faDatePicker1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker1.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker1_KeyPress);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(299, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "علت انصراف:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(299, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "تاریخ انصراف:";
            // 
            // Form05_CancelOrdersDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(391, -28);
            this.ControlBox = false;
            this.Controls.Add(this.uiTab1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form05_CancelOrdersDialog";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "اطلاعات انصراف سفارش";
            this.Load += new System.EventHandler(this.Form05_CancelOrdersDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.officeFormAdorner1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiTab1)).EndInit();
            this.uiTab1.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            this.uiTabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.Ribbon.OfficeFormAdorner officeFormAdorner1;
        private Janus.Windows.UI.Tab.UITab uiTab1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private Janus.Windows.GridEX.EditControls.EditBox txt_Cause;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker1;
        private Janus.Windows.EditControls.UIButton bt_Yes;
        private Janus.Windows.EditControls.UIButton bt_No;

    }
}