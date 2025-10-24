namespace PSHOP._05_Sale.Reports
{
    partial class Form_SaleFactorPrint2
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
            Janus.Windows.GridEX.GridEXLayout mlt_ACC_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_SaleFactorPrint2));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.SettelmentWithFactor = new System.Windows.Forms.CheckBox();
            this.ChkEcoCode = new System.Windows.Forms.CheckBox();
            this.mlt_ACC = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.chk_ShowDate = new System.Windows.Forms.CheckBox();
            this.chk_ShowSen = new System.Windows.Forms.CheckBox();
            this.chk_ShowCustomerBill = new System.Windows.Forms.CheckBox();
            this.bt_Display = new DevComponents.DotNetBar.ButtonX();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.dataSet_Sale21 = new PSHOP._05_Sale.DataSet_Sale2();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_ACC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale21)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.buttonX1);
            this.panelEx1.Controls.Add(this.SettelmentWithFactor);
            this.panelEx1.Controls.Add(this.ChkEcoCode);
            this.panelEx1.Controls.Add(this.mlt_ACC);
            this.panelEx1.Controls.Add(this.chk_ShowDate);
            this.panelEx1.Controls.Add(this.chk_ShowSen);
            this.panelEx1.Controls.Add(this.chk_ShowCustomerBill);
            this.panelEx1.Controls.Add(this.bt_Display);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(489, 141);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.buttonX1.Location = new System.Drawing.Point(77, 85);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Size = new System.Drawing.Size(59, 21);
            this.buttonX1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.buttonX1.TabIndex = 7;
            this.buttonX1.Text = "طراحی";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // SettelmentWithFactor
            // 
            this.SettelmentWithFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SettelmentWithFactor.AutoSize = true;
            this.SettelmentWithFactor.BackColor = System.Drawing.Color.Transparent;
            this.SettelmentWithFactor.Checked = true;
            this.SettelmentWithFactor.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SettelmentWithFactor.Location = new System.Drawing.Point(126, 62);
            this.SettelmentWithFactor.Name = "SettelmentWithFactor";
            this.SettelmentWithFactor.Size = new System.Drawing.Size(188, 17);
            this.SettelmentWithFactor.TabIndex = 5;
            this.SettelmentWithFactor.Text = "نمایش مانده با احتساب فاکتور جاری";
            this.SettelmentWithFactor.UseVisualStyleBackColor = false;
            // 
            // ChkEcoCode
            // 
            this.ChkEcoCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChkEcoCode.AutoSize = true;
            this.ChkEcoCode.Checked = true;
            this.ChkEcoCode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkEcoCode.Location = new System.Drawing.Point(401, 35);
            this.ChkEcoCode.Name = "ChkEcoCode";
            this.ChkEcoCode.Size = new System.Drawing.Size(79, 17);
            this.ChkEcoCode.TabIndex = 1;
            this.ChkEcoCode.Text = "کد اقتصادی";
            this.ChkEcoCode.UseVisualStyleBackColor = true;
            // 
            // mlt_ACC
            // 
            this.mlt_ACC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            mlt_ACC_DesignTimeLayout.LayoutString = resources.GetString("mlt_ACC_DesignTimeLayout.LayoutString");
            this.mlt_ACC.DesignTimeLayout = mlt_ACC_DesignTimeLayout;
            this.mlt_ACC.DisplayMember = "ACC_Name";
            this.mlt_ACC.Location = new System.Drawing.Point(3, 33);
            this.mlt_ACC.Name = "mlt_ACC";
            this.mlt_ACC.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.mlt_ACC.OfficeCustomColor = System.Drawing.Color.Navy;
            this.mlt_ACC.SelectedIndex = -1;
            this.mlt_ACC.SelectedItem = null;
            this.mlt_ACC.Size = new System.Drawing.Size(311, 21);
            this.mlt_ACC.TabIndex = 4;
            this.mlt_ACC.ValueMember = "ACC_Code";
            this.mlt_ACC.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // chk_ShowDate
            // 
            this.chk_ShowDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_ShowDate.AutoSize = true;
            this.chk_ShowDate.Checked = true;
            this.chk_ShowDate.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_ShowDate.Location = new System.Drawing.Point(430, 12);
            this.chk_ShowDate.Name = "chk_ShowDate";
            this.chk_ShowDate.Size = new System.Drawing.Size(50, 17);
            this.chk_ShowDate.TabIndex = 0;
            this.chk_ShowDate.Text = " تاریخ";
            this.chk_ShowDate.UseVisualStyleBackColor = true;
            // 
            // chk_ShowSen
            // 
            this.chk_ShowSen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_ShowSen.AutoSize = true;
            this.chk_ShowSen.BackColor = System.Drawing.Color.Transparent;
            this.chk_ShowSen.Location = new System.Drawing.Point(321, 62);
            this.chk_ShowSen.Name = "chk_ShowSen";
            this.chk_ShowSen.Size = new System.Drawing.Size(159, 17);
            this.chk_ShowSen.TabIndex = 2;
            this.chk_ShowSen.Text = "نمایش جمله انتهایی فاکتورها";
            this.chk_ShowSen.UseVisualStyleBackColor = false;
            // 
            // chk_ShowCustomerBill
            // 
            this.chk_ShowCustomerBill.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_ShowCustomerBill.AutoSize = true;
            this.chk_ShowCustomerBill.BackColor = System.Drawing.Color.Transparent;
            this.chk_ShowCustomerBill.Checked = true;
            this.chk_ShowCustomerBill.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_ShowCustomerBill.Location = new System.Drawing.Point(3, 12);
            this.chk_ShowCustomerBill.Name = "chk_ShowCustomerBill";
            this.chk_ShowCustomerBill.Size = new System.Drawing.Size(311, 17);
            this.chk_ShowCustomerBill.TabIndex = 3;
            this.chk_ShowCustomerBill.Text = "نمایش مانده حساب مشتری در انتهای فاکتور بر اساس حساب:";
            this.chk_ShowCustomerBill.UseVisualStyleBackColor = false;
            // 
            // bt_Display
            // 
            this.bt_Display.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bt_Display.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Display.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bt_Display.Location = new System.Drawing.Point(12, 85);
            this.bt_Display.Name = "bt_Display";
            this.bt_Display.Size = new System.Drawing.Size(59, 21);
            this.bt_Display.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bt_Display.TabIndex = 6;
            this.bt_Display.Text = "مشاهده";
            this.bt_Display.Click += new System.EventHandler(this.bt_Display_Click);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // buttonItem1
            // 
            this.buttonItem1.BeginGroup = true;
            this.buttonItem1.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.buttonItem1.Image = ((System.Drawing.Image)(resources.GetObject("buttonItem1.Image")));
            this.buttonItem1.Name = "buttonItem1";
            this.buttonItem1.Text = "طرح 12";
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bar1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar1.Location = new System.Drawing.Point(0, 116);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(489, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 13;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // dataSet_Sale21
            // 
            this.dataSet_Sale21.DataSetName = "DataSet_Sale2";
            this.dataSet_Sale21.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Form_SaleFactorPrint2
            // 
            this.ClientSize = new System.Drawing.Size(489, 141);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form_SaleFactorPrint2";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "چاپ فاکتور فروش";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FactorPrint_FormClosing);
            this.Load += new System.EventHandler(this.Form_FactorPrint_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_ACC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale21)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DataSet_Sale2 dataSet_Sale21;
        private System.Windows.Forms.PrintDialog printDialog1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private DevComponents.DotNetBar.Bar bar1;
        private System.Windows.Forms.CheckBox ChkEcoCode;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_ACC;
        private System.Windows.Forms.CheckBox chk_ShowDate;
        private System.Windows.Forms.CheckBox chk_ShowSen;
        private System.Windows.Forms.CheckBox chk_ShowCustomerBill;
        private DevComponents.DotNetBar.ButtonX bt_Display;
        private System.Windows.Forms.CheckBox SettelmentWithFactor;
        private DevComponents.DotNetBar.ButtonX buttonX1;
    }
}