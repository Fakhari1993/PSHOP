namespace PSHOP._05_Sale.Reports
{
    partial class Form_TotalSaleFactorPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_TotalSaleFactorPrint));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.chk_gift = new System.Windows.Forms.CheckBox();
            this.chk_ShowDate = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.rb_insertorder = new System.Windows.Forms.RadioButton();
            this.rb_goodCode = new System.Windows.Forms.RadioButton();
            this.rb_goodname = new System.Windows.Forms.RadioButton();
            this.chk_ShowEcoCode = new System.Windows.Forms.CheckBox();
            this.mlt_ACC = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.chk_Logo = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.faDatePicker2 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.rdb_Date = new System.Windows.Forms.RadioButton();
            this.rdb_CurrentNumber = new System.Windows.Forms.RadioButton();
            this.txt_To = new Janus.Windows.GridEX.EditControls.EditBox();
            this.rdb_FromNumber = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_From = new Janus.Windows.GridEX.EditControls.EditBox();
            this.chk_ShowSen = new System.Windows.Forms.CheckBox();
            this.chk_ShowCustomerBill = new System.Windows.Forms.CheckBox();
            this.bt_Display = new DevComponents.DotNetBar.ButtonX();
            this.dataSet_Sale21 = new PSHOP._05_Sale.DataSet_Sale2();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.btn_Design = new DevComponents.DotNetBar.ButtonItem();
            this.btn_L = new DevComponents.DotNetBar.ButtonItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stiViewerControl1 = new Stimulsoft.Report.Viewer.StiViewerControl();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_ACC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.chk_gift);
            this.panelEx1.Controls.Add(this.chk_ShowDate);
            this.panelEx1.Controls.Add(this.label3);
            this.panelEx1.Controls.Add(this.uiGroupBox1);
            this.panelEx1.Controls.Add(this.chk_ShowEcoCode);
            this.panelEx1.Controls.Add(this.mlt_ACC);
            this.panelEx1.Controls.Add(this.chk_Logo);
            this.panelEx1.Controls.Add(this.label2);
            this.panelEx1.Controls.Add(this.faDatePicker2);
            this.panelEx1.Controls.Add(this.faDatePicker1);
            this.panelEx1.Controls.Add(this.rdb_Date);
            this.panelEx1.Controls.Add(this.rdb_CurrentNumber);
            this.panelEx1.Controls.Add(this.txt_To);
            this.panelEx1.Controls.Add(this.rdb_FromNumber);
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Controls.Add(this.txt_From);
            this.panelEx1.Controls.Add(this.chk_ShowSen);
            this.panelEx1.Controls.Add(this.chk_ShowCustomerBill);
            this.panelEx1.Controls.Add(this.bt_Display);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(862, 106);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // chk_gift
            // 
            this.chk_gift.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_gift.AutoSize = true;
            this.chk_gift.Checked = true;
            this.chk_gift.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_gift.Location = new System.Drawing.Point(469, 80);
            this.chk_gift.Name = "chk_gift";
            this.chk_gift.Size = new System.Drawing.Size(49, 17);
            this.chk_gift.TabIndex = 16;
            this.chk_gift.Text = "جایزه";
            this.chk_gift.UseVisualStyleBackColor = true;
            // 
            // chk_ShowDate
            // 
            this.chk_ShowDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_ShowDate.AutoSize = true;
            this.chk_ShowDate.Location = new System.Drawing.Point(262, 10);
            this.chk_ShowDate.Name = "chk_ShowDate";
            this.chk_ShowDate.Size = new System.Drawing.Size(93, 17);
            this.chk_ShowDate.TabIndex = 8;
            this.chk_ShowDate.Text = "چاپ بدون تاریخ";
            this.chk_ShowDate.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(542, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "مرتب سازی کالا براساس:";
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiGroupBox1.Controls.Add(this.rb_insertorder);
            this.uiGroupBox1.Controls.Add(this.rb_goodCode);
            this.uiGroupBox1.Controls.Add(this.rb_goodname);
            this.uiGroupBox1.Location = new System.Drawing.Point(207, 74);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(257, 29);
            this.uiGroupBox1.TabIndex = 11;
            // 
            // rb_insertorder
            // 
            this.rb_insertorder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rb_insertorder.AutoSize = true;
            this.rb_insertorder.BackColor = System.Drawing.Color.Transparent;
            this.rb_insertorder.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_insertorder.Location = new System.Drawing.Point(5, 9);
            this.rb_insertorder.Name = "rb_insertorder";
            this.rb_insertorder.Size = new System.Drawing.Size(70, 17);
            this.rb_insertorder.TabIndex = 2;
            this.rb_insertorder.TabStop = true;
            this.rb_insertorder.Text = "ترتیب ثبت";
            this.rb_insertorder.UseVisualStyleBackColor = false;
            this.rb_insertorder.CheckedChanged += new System.EventHandler(this.rb_insertorder_CheckedChanged);
            // 
            // rb_goodCode
            // 
            this.rb_goodCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rb_goodCode.AutoSize = true;
            this.rb_goodCode.BackColor = System.Drawing.Color.Transparent;
            this.rb_goodCode.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_goodCode.Location = new System.Drawing.Point(100, 9);
            this.rb_goodCode.Name = "rb_goodCode";
            this.rb_goodCode.Size = new System.Drawing.Size(54, 17);
            this.rb_goodCode.TabIndex = 1;
            this.rb_goodCode.TabStop = true;
            this.rb_goodCode.Text = "کد کالا";
            this.rb_goodCode.UseVisualStyleBackColor = false;
            this.rb_goodCode.CheckedChanged += new System.EventHandler(this.rb_goodCode_CheckedChanged);
            // 
            // rb_goodname
            // 
            this.rb_goodname.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rb_goodname.AutoSize = true;
            this.rb_goodname.BackColor = System.Drawing.Color.Transparent;
            this.rb_goodname.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rb_goodname.Location = new System.Drawing.Point(182, 9);
            this.rb_goodname.Name = "rb_goodname";
            this.rb_goodname.Size = new System.Drawing.Size(56, 17);
            this.rb_goodname.TabIndex = 0;
            this.rb_goodname.TabStop = true;
            this.rb_goodname.Text = "نام کالا";
            this.rb_goodname.UseVisualStyleBackColor = false;
            this.rb_goodname.CheckedChanged += new System.EventHandler(this.rb_goodname_CheckedChanged);
            // 
            // chk_ShowEcoCode
            // 
            this.chk_ShowEcoCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_ShowEcoCode.AutoSize = true;
            this.chk_ShowEcoCode.Location = new System.Drawing.Point(393, 10);
            this.chk_ShowEcoCode.Name = "chk_ShowEcoCode";
            this.chk_ShowEcoCode.Size = new System.Drawing.Size(125, 17);
            this.chk_ShowEcoCode.TabIndex = 7;
            this.chk_ShowEcoCode.Text = "چاپ بدون کد اقتصادی";
            this.chk_ShowEcoCode.UseVisualStyleBackColor = true;
            // 
            // mlt_ACC
            // 
            this.mlt_ACC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            mlt_ACC_DesignTimeLayout.LayoutString = resources.GetString("mlt_ACC_DesignTimeLayout.LayoutString");
            this.mlt_ACC.DesignTimeLayout = mlt_ACC_DesignTimeLayout;
            this.mlt_ACC.DisplayMember = "ACC_Name";
            this.mlt_ACC.Location = new System.Drawing.Point(207, 53);
            this.mlt_ACC.Name = "mlt_ACC";
            this.mlt_ACC.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.mlt_ACC.OfficeCustomColor = System.Drawing.Color.Navy;
            this.mlt_ACC.SelectedIndex = -1;
            this.mlt_ACC.SelectedItem = null;
            this.mlt_ACC.Size = new System.Drawing.Size(311, 21);
            this.mlt_ACC.TabIndex = 10;
            this.mlt_ACC.ValueMember = "ACC_Code";
            this.mlt_ACC.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.mlt_ACC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_ACC_KeyPress);
            // 
            // chk_Logo
            // 
            this.chk_Logo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_Logo.AutoSize = true;
            this.chk_Logo.Checked = true;
            this.chk_Logo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Logo.Location = new System.Drawing.Point(31, 31);
            this.chk_Logo.Name = "chk_Logo";
            this.chk_Logo.Size = new System.Drawing.Size(148, 17);
            this.chk_Logo.TabIndex = 13;
            this.chk_Logo.Text = "چاپ بدون لوگو و نام شرکت";
            this.chk_Logo.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(643, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "تا تاریخ:";
            // 
            // faDatePicker2
            // 
            this.faDatePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker2.Location = new System.Drawing.Point(542, 53);
            this.faDatePicker2.Name = "faDatePicker2";
            this.faDatePicker2.Size = new System.Drawing.Size(95, 20);
            this.faDatePicker2.TabIndex = 6;
            this.faDatePicker2.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker2.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker2_KeyPress);
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker1.Location = new System.Drawing.Point(690, 53);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(95, 20);
            this.faDatePicker1.TabIndex = 5;
            this.faDatePicker1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker1.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker1_KeyPress);
            // 
            // rdb_Date
            // 
            this.rdb_Date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_Date.AutoSize = true;
            this.rdb_Date.BackColor = System.Drawing.Color.Transparent;
            this.rdb_Date.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_Date.Location = new System.Drawing.Point(794, 55);
            this.rdb_Date.Name = "rdb_Date";
            this.rdb_Date.Size = new System.Drawing.Size(60, 17);
            this.rdb_Date.TabIndex = 4;
            this.rdb_Date.TabStop = true;
            this.rdb_Date.Text = "از تاریخ:";
            this.rdb_Date.UseVisualStyleBackColor = false;
            // 
            // rdb_CurrentNumber
            // 
            this.rdb_CurrentNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_CurrentNumber.AutoSize = true;
            this.rdb_CurrentNumber.BackColor = System.Drawing.Color.Transparent;
            this.rdb_CurrentNumber.Checked = true;
            this.rdb_CurrentNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_CurrentNumber.Location = new System.Drawing.Point(772, 10);
            this.rdb_CurrentNumber.Name = "rdb_CurrentNumber";
            this.rdb_CurrentNumber.Size = new System.Drawing.Size(82, 17);
            this.rdb_CurrentNumber.TabIndex = 0;
            this.rdb_CurrentNumber.TabStop = true;
            this.rdb_CurrentNumber.Text = "شماره جاری";
            this.rdb_CurrentNumber.UseVisualStyleBackColor = false;
            // 
            // txt_To
            // 
            this.txt_To.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_To.Location = new System.Drawing.Point(542, 29);
            this.txt_To.Name = "txt_To";
            this.txt_To.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.txt_To.OfficeCustomColor = System.Drawing.Color.Black;
            this.txt_To.Size = new System.Drawing.Size(95, 21);
            this.txt_To.TabIndex = 3;
            this.txt_To.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.txt_To.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_From_KeyPress);
            // 
            // rdb_FromNumber
            // 
            this.rdb_FromNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_FromNumber.AutoSize = true;
            this.rdb_FromNumber.BackColor = System.Drawing.Color.Transparent;
            this.rdb_FromNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_FromNumber.Location = new System.Drawing.Point(785, 31);
            this.rdb_FromNumber.Name = "rdb_FromNumber";
            this.rdb_FromNumber.Size = new System.Drawing.Size(69, 17);
            this.rdb_FromNumber.TabIndex = 1;
            this.rdb_FromNumber.TabStop = true;
            this.rdb_FromNumber.Text = "از شماره:";
            this.rdb_FromNumber.UseVisualStyleBackColor = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(636, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "تا شماره:";
            // 
            // txt_From
            // 
            this.txt_From.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_From.Location = new System.Drawing.Point(690, 29);
            this.txt_From.Name = "txt_From";
            this.txt_From.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.txt_From.OfficeCustomColor = System.Drawing.Color.Black;
            this.txt_From.Size = new System.Drawing.Size(95, 21);
            this.txt_From.TabIndex = 2;
            this.txt_From.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.txt_From.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_From_KeyPress);
            // 
            // chk_ShowSen
            // 
            this.chk_ShowSen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_ShowSen.AutoSize = true;
            this.chk_ShowSen.BackColor = System.Drawing.Color.Transparent;
            this.chk_ShowSen.Location = new System.Drawing.Point(21, 10);
            this.chk_ShowSen.Name = "chk_ShowSen";
            this.chk_ShowSen.Size = new System.Drawing.Size(159, 17);
            this.chk_ShowSen.TabIndex = 12;
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
            this.chk_ShowCustomerBill.Location = new System.Drawing.Point(207, 31);
            this.chk_ShowCustomerBill.Name = "chk_ShowCustomerBill";
            this.chk_ShowCustomerBill.Size = new System.Drawing.Size(311, 17);
            this.chk_ShowCustomerBill.TabIndex = 9;
            this.chk_ShowCustomerBill.Text = "نمایش مانده حساب مشتری در انتهای فاکتور بر اساس حساب:";
            this.chk_ShowCustomerBill.UseVisualStyleBackColor = false;
            this.chk_ShowCustomerBill.CheckedChanged += new System.EventHandler(this.chk_ShowCustomerBill_CheckedChanged);
            // 
            // bt_Display
            // 
            this.bt_Display.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bt_Display.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Display.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bt_Display.Location = new System.Drawing.Point(26, 53);
            this.bt_Display.Name = "bt_Display";
            this.bt_Display.Size = new System.Drawing.Size(150, 21);
            this.bt_Display.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bt_Display.TabIndex = 14;
            this.bt_Display.Text = "مشاهده";
            this.bt_Display.Click += new System.EventHandler(this.bt_Display_Click);
            // 
            // dataSet_Sale21
            // 
            this.dataSet_Sale21.DataSetName = "DataSet_Sale2";
            this.dataSet_Sale21.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
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
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.btn_Design});
            this.bar1.Location = new System.Drawing.Point(0, 427);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(862, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Metro;
            this.bar1.TabIndex = 13;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // btn_Design
            // 
            this.btn_Design.Name = "btn_Design";
            this.btn_Design.Text = "طراحی فاکتور دلخواه";
            this.btn_Design.Click += new System.EventHandler(this.btn_Design_Click_1);
            // 
            // btn_L
            // 
            this.btn_L.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.btn_L.FontBold = true;
            this.btn_L.Image = ((System.Drawing.Image)(resources.GetObject("btn_L.Image")));
            this.btn_L.Name = "btn_L";
            this.btn_L.Text = "افقی";
            //this.btn_L.Click += new System.EventHandler(this.btn_Design_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stiViewerControl1);
            this.panel1.Controls.Add(this.crystalReportViewer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 106);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel1.Size = new System.Drawing.Size(862, 321);
            this.panel1.TabIndex = 14;
            // 
            // stiViewerControl1
            // 
            this.stiViewerControl1.AllowDrop = true;
            this.stiViewerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stiViewerControl1.Location = new System.Drawing.Point(0, 0);
            this.stiViewerControl1.Name = "stiViewerControl1";
            this.stiViewerControl1.PageViewMode = Stimulsoft.Report.Viewer.StiPageViewMode.SinglePage;
            this.stiViewerControl1.Report = null;
            this.stiViewerControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.stiViewerControl1.ShowZoom = true;
            this.stiViewerControl1.Size = new System.Drawing.Size(862, 321);
            this.stiViewerControl1.TabIndex = 3;
            this.stiViewerControl1.Visible = false;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.EnableDrillDown = false;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.ShowCloseButton = false;
            this.crystalReportViewer1.ShowGotoPageButton = false;
            this.crystalReportViewer1.ShowGroupTreeButton = false;
            this.crystalReportViewer1.ShowParameterPanelButton = false;
            this.crystalReportViewer1.ShowRefreshButton = false;
            this.crystalReportViewer1.ShowTextSearchButton = false;
            this.crystalReportViewer1.Size = new System.Drawing.Size(862, 321);
            this.crystalReportViewer1.TabIndex = 2;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // Form_TotalSaleFactorPrint
            // 
            this.ClientSize = new System.Drawing.Size(862, 452);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form_TotalSaleFactorPrint";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "چاپ فاکتور فروش";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FactorPrint_FormClosing);
            this.Load += new System.EventHandler(this.Form_FactorPrint_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_ACC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX bt_Display;
        private System.Windows.Forms.CheckBox chk_ShowSen;
        private System.Windows.Forms.CheckBox chk_ShowCustomerBill;
        private System.Windows.Forms.Label label2;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker2;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker1;
        private System.Windows.Forms.RadioButton rdb_Date;
        private System.Windows.Forms.RadioButton rdb_CurrentNumber;
        private Janus.Windows.GridEX.EditControls.EditBox txt_To;
        private System.Windows.Forms.RadioButton rdb_FromNumber;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.EditControls.EditBox txt_From;
        private DataSet_Sale2 dataSet_Sale21;
        private System.Windows.Forms.PrintDialog printDialog1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private System.Windows.Forms.CheckBox chk_Logo;
        private System.Windows.Forms.CheckBox chk_ShowDate;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mlt_ACC;
        private DevComponents.DotNetBar.Bar bar1;
        private System.Windows.Forms.Panel panel1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.CheckBox chk_ShowEcoCode;
        private DevComponents.DotNetBar.ButtonItem btn_Design;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rb_goodname;
        private System.Windows.Forms.RadioButton rb_goodCode;
        private System.Windows.Forms.RadioButton rb_insertorder;
        private Stimulsoft.Report.Viewer.StiViewerControl stiViewerControl1;
        private DevComponents.DotNetBar.ButtonItem btn_L;
        private System.Windows.Forms.CheckBox chk_gift;
    }
}