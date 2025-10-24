namespace PSHOP._05_Sale.Reports
{
    partial class Form_SaleFactorPrint1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form_SaleFactorPrint1));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.ChkEcoCode = new System.Windows.Forms.CheckBox();
            this.SettelmentWithFactor = new System.Windows.Forms.CheckBox();
            this.mlt_ACC = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.chk_ShowDate = new System.Windows.Forms.CheckBox();
            this.chk_Logo = new System.Windows.Forms.CheckBox();
            this.chk_ShowSen = new System.Windows.Forms.CheckBox();
            this.chk_ShowCustomerBill = new System.Windows.Forms.CheckBox();
            this.bt_Display = new DevComponents.DotNetBar.ButtonX();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.rdb_CurrentNumber = new System.Windows.Forms.RadioButton();
            this.txt_From = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label1 = new System.Windows.Forms.Label();
            this.rdb_FromNumber = new System.Windows.Forms.RadioButton();
            this.txt_To = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rdb_Date = new System.Windows.Forms.RadioButton();
            this.faDatePicker2 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.dataSet_Sale21 = new PSHOP._05_Sale.DataSet_Sale2();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.btn_Design = new DevComponents.DotNetBar.ButtonItem();
            this.bt_10 = new DevComponents.DotNetBar.ButtonItem();
            this.bt_16 = new DevComponents.DotNetBar.ButtonItem();
            this.bt_22 = new DevComponents.DotNetBar.ButtonItem();
            this.bt_5 = new DevComponents.DotNetBar.ButtonItem();
            this.bt_11 = new DevComponents.DotNetBar.ButtonItem();
            this.bt_6 = new DevComponents.DotNetBar.ButtonItem();
            this.bt_12 = new DevComponents.DotNetBar.ButtonItem();
            this.bt_18 = new DevComponents.DotNetBar.ButtonItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.stiViewerControl1 = new Stimulsoft.Report.Viewer.StiViewerControl();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_ACC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.ChkEcoCode);
            this.panelEx1.Controls.Add(this.SettelmentWithFactor);
            this.panelEx1.Controls.Add(this.mlt_ACC);
            this.panelEx1.Controls.Add(this.chk_ShowDate);
            this.panelEx1.Controls.Add(this.chk_Logo);
            this.panelEx1.Controls.Add(this.chk_ShowSen);
            this.panelEx1.Controls.Add(this.chk_ShowCustomerBill);
            this.panelEx1.Controls.Add(this.bt_Display);
            this.panelEx1.Controls.Add(this.uiGroupBox1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(794, 109);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // ChkEcoCode
            // 
            this.ChkEcoCode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ChkEcoCode.AutoSize = true;
            this.ChkEcoCode.Checked = true;
            this.ChkEcoCode.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ChkEcoCode.Location = new System.Drawing.Point(0, 7);
            this.ChkEcoCode.Name = "ChkEcoCode";
            this.ChkEcoCode.Size = new System.Drawing.Size(79, 17);
            this.ChkEcoCode.TabIndex = 13;
            this.ChkEcoCode.Text = "کد اقتصادی";
            this.ChkEcoCode.UseVisualStyleBackColor = true;
            // 
            // SettelmentWithFactor
            // 
            this.SettelmentWithFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SettelmentWithFactor.AutoSize = true;
            this.SettelmentWithFactor.BackColor = System.Drawing.Color.Transparent;
            this.SettelmentWithFactor.Location = new System.Drawing.Point(258, 53);
            this.SettelmentWithFactor.Name = "SettelmentWithFactor";
            this.SettelmentWithFactor.Size = new System.Drawing.Size(188, 17);
            this.SettelmentWithFactor.TabIndex = 11;
            this.SettelmentWithFactor.Text = "نمایش مانده با احتساب فاکتور جاری";
            this.SettelmentWithFactor.UseVisualStyleBackColor = false;
            // 
            // mlt_ACC
            // 
            this.mlt_ACC.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            mlt_ACC_DesignTimeLayout.LayoutString = resources.GetString("mlt_ACC_DesignTimeLayout.LayoutString");
            this.mlt_ACC.DesignTimeLayout = mlt_ACC_DesignTimeLayout;
            this.mlt_ACC.DisplayMember = "ACC_Name";
            this.mlt_ACC.Location = new System.Drawing.Point(200, 76);
            this.mlt_ACC.Name = "mlt_ACC";
            this.mlt_ACC.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.mlt_ACC.OfficeCustomColor = System.Drawing.Color.Navy;
            this.mlt_ACC.SelectedIndex = -1;
            this.mlt_ACC.SelectedItem = null;
            this.mlt_ACC.Size = new System.Drawing.Size(246, 21);
            this.mlt_ACC.TabIndex = 12;
            this.mlt_ACC.ValueMember = "ACC_Code";
            this.mlt_ACC.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            this.mlt_ACC.ValueChanged += new System.EventHandler(this.mlt_ACC_ValueChanged);
            this.mlt_ACC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mlt_ACC_KeyPress);
            this.mlt_ACC.Leave += new System.EventHandler(this.mlt_ACC_Leave);
            // 
            // chk_ShowDate
            // 
            this.chk_ShowDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_ShowDate.AutoSize = true;
            this.chk_ShowDate.Location = new System.Drawing.Point(399, 7);
            this.chk_ShowDate.Name = "chk_ShowDate";
            this.chk_ShowDate.Size = new System.Drawing.Size(93, 17);
            this.chk_ShowDate.TabIndex = 7;
            this.chk_ShowDate.Text = "چاپ بدون تاریخ";
            this.chk_ShowDate.UseVisualStyleBackColor = true;
            // 
            // chk_Logo
            // 
            this.chk_Logo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_Logo.AutoSize = true;
            this.chk_Logo.Checked = true;
            this.chk_Logo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Logo.Location = new System.Drawing.Point(80, 7);
            this.chk_Logo.Name = "chk_Logo";
            this.chk_Logo.Size = new System.Drawing.Size(148, 17);
            this.chk_Logo.TabIndex = 9;
            this.chk_Logo.Text = "چاپ بدون لوگو و نام شرکت";
            this.chk_Logo.UseVisualStyleBackColor = true;
            // 
            // chk_ShowSen
            // 
            this.chk_ShowSen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_ShowSen.AutoSize = true;
            this.chk_ShowSen.BackColor = System.Drawing.Color.Transparent;
            this.chk_ShowSen.Location = new System.Drawing.Point(234, 7);
            this.chk_ShowSen.Name = "chk_ShowSen";
            this.chk_ShowSen.Size = new System.Drawing.Size(159, 17);
            this.chk_ShowSen.TabIndex = 8;
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
            this.chk_ShowCustomerBill.Location = new System.Drawing.Point(135, 31);
            this.chk_ShowCustomerBill.Name = "chk_ShowCustomerBill";
            this.chk_ShowCustomerBill.Size = new System.Drawing.Size(311, 17);
            this.chk_ShowCustomerBill.TabIndex = 10;
            this.chk_ShowCustomerBill.Text = "نمایش مانده حساب مشتری در انتهای فاکتور بر اساس حساب:";
            this.chk_ShowCustomerBill.UseVisualStyleBackColor = false;
            this.chk_ShowCustomerBill.CheckedChanged += new System.EventHandler(this.chk_ShowCustomerBill_CheckedChanged);
            // 
            // bt_Display
            // 
            this.bt_Display.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bt_Display.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Display.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bt_Display.Location = new System.Drawing.Point(12, 76);
            this.bt_Display.Name = "bt_Display";
            this.bt_Display.Size = new System.Drawing.Size(59, 21);
            this.bt_Display.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bt_Display.TabIndex = 14;
            this.bt_Display.Text = "مشاهده";
            this.bt_Display.Click += new System.EventHandler(this.bt_Display_Click);
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uiGroupBox1.Controls.Add(this.rdb_CurrentNumber);
            this.uiGroupBox1.Controls.Add(this.txt_From);
            this.uiGroupBox1.Controls.Add(this.label1);
            this.uiGroupBox1.Controls.Add(this.rdb_FromNumber);
            this.uiGroupBox1.Controls.Add(this.txt_To);
            this.uiGroupBox1.Controls.Add(this.label2);
            this.uiGroupBox1.Controls.Add(this.rdb_Date);
            this.uiGroupBox1.Controls.Add(this.faDatePicker2);
            this.uiGroupBox1.Controls.Add(this.faDatePicker1);
            this.uiGroupBox1.Location = new System.Drawing.Point(458, 18);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(334, 79);
            this.uiGroupBox1.TabIndex = 15;
            // 
            // rdb_CurrentNumber
            // 
            this.rdb_CurrentNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_CurrentNumber.AutoSize = true;
            this.rdb_CurrentNumber.BackColor = System.Drawing.Color.Transparent;
            this.rdb_CurrentNumber.Checked = true;
            this.rdb_CurrentNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_CurrentNumber.Location = new System.Drawing.Point(248, 9);
            this.rdb_CurrentNumber.Name = "rdb_CurrentNumber";
            this.rdb_CurrentNumber.Size = new System.Drawing.Size(82, 17);
            this.rdb_CurrentNumber.TabIndex = 0;
            this.rdb_CurrentNumber.TabStop = true;
            this.rdb_CurrentNumber.Text = "شماره جاری";
            this.rdb_CurrentNumber.UseVisualStyleBackColor = false;
            // 
            // txt_From
            // 
            this.txt_From.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_From.Location = new System.Drawing.Point(166, 29);
            this.txt_From.Name = "txt_From";
            this.txt_From.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.txt_From.OfficeCustomColor = System.Drawing.Color.Black;
            this.txt_From.Size = new System.Drawing.Size(95, 21);
            this.txt_From.TabIndex = 2;
            this.txt_From.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.txt_From.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_From_KeyPress);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(114, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "تا شماره:";
            // 
            // rdb_FromNumber
            // 
            this.rdb_FromNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_FromNumber.AutoSize = true;
            this.rdb_FromNumber.BackColor = System.Drawing.Color.Transparent;
            this.rdb_FromNumber.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_FromNumber.Location = new System.Drawing.Point(261, 31);
            this.rdb_FromNumber.Name = "rdb_FromNumber";
            this.rdb_FromNumber.Size = new System.Drawing.Size(69, 17);
            this.rdb_FromNumber.TabIndex = 1;
            this.rdb_FromNumber.TabStop = true;
            this.rdb_FromNumber.Text = "از شماره:";
            this.rdb_FromNumber.UseVisualStyleBackColor = false;
            // 
            // txt_To
            // 
            this.txt_To.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_To.Location = new System.Drawing.Point(18, 29);
            this.txt_To.Name = "txt_To";
            this.txt_To.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.txt_To.OfficeCustomColor = System.Drawing.Color.Black;
            this.txt_To.Size = new System.Drawing.Size(95, 21);
            this.txt_To.TabIndex = 3;
            this.txt_To.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            this.txt_To.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_From_KeyPress);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(119, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "تا تاریخ:";
            // 
            // rdb_Date
            // 
            this.rdb_Date.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdb_Date.AutoSize = true;
            this.rdb_Date.BackColor = System.Drawing.Color.Transparent;
            this.rdb_Date.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdb_Date.Location = new System.Drawing.Point(270, 55);
            this.rdb_Date.Name = "rdb_Date";
            this.rdb_Date.Size = new System.Drawing.Size(60, 17);
            this.rdb_Date.TabIndex = 4;
            this.rdb_Date.TabStop = true;
            this.rdb_Date.Text = "از تاریخ:";
            this.rdb_Date.UseVisualStyleBackColor = false;
            // 
            // faDatePicker2
            // 
            this.faDatePicker2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker2.Location = new System.Drawing.Point(18, 53);
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
            this.faDatePicker1.Location = new System.Drawing.Point(166, 53);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(95, 20);
            this.faDatePicker1.TabIndex = 5;
            this.faDatePicker1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            this.faDatePicker1.TextChanged += new System.EventHandler(this.faDatePicker1_TextChanged);
            this.faDatePicker1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.faDatePicker1_KeyPress);
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
            this.bar1.Size = new System.Drawing.Size(794, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.Office2007;
            this.bar1.TabIndex = 13;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // btn_Design
            // 
            this.btn_Design.ForeColor = System.Drawing.Color.Black;
            this.btn_Design.Name = "btn_Design";
            this.btn_Design.Text = "طراحی فاکتور";
            this.btn_Design.Click += new System.EventHandler(this.btn_Design_Click);
            // 
            // bt_10
            // 
            this.bt_10.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_10.FontBold = true;
            this.bt_10.Image = ((System.Drawing.Image)(resources.GetObject("bt_10.Image")));
            this.bt_10.Name = "bt_10";
            this.bt_10.Text = "10";
            this.bt_10.Click += new System.EventHandler(this.bt_10_Click);
            // 
            // bt_16
            // 
            this.bt_16.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_16.FontBold = true;
            this.bt_16.Image = ((System.Drawing.Image)(resources.GetObject("bt_16.Image")));
            this.bt_16.Name = "bt_16";
            this.bt_16.Text = "16";
            this.bt_16.Click += new System.EventHandler(this.bt_16_Click);
            // 
            // bt_22
            // 
            this.bt_22.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_22.FontBold = true;
            this.bt_22.Image = ((System.Drawing.Image)(resources.GetObject("bt_22.Image")));
            this.bt_22.Name = "bt_22";
            this.bt_22.Text = "22";
            this.bt_22.Click += new System.EventHandler(this.bt_22_Click);
            // 
            // bt_5
            // 
            this.bt_5.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_5.FontBold = true;
            this.bt_5.Image = ((System.Drawing.Image)(resources.GetObject("bt_5.Image")));
            this.bt_5.Name = "bt_5";
            this.bt_5.Text = "5";
            this.bt_5.Click += new System.EventHandler(this.bt_5_Click);
            // 
            // bt_11
            // 
            this.bt_11.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_11.FontBold = true;
            this.bt_11.Image = ((System.Drawing.Image)(resources.GetObject("bt_11.Image")));
            this.bt_11.Name = "bt_11";
            this.bt_11.Text = "11";
            this.bt_11.Click += new System.EventHandler(this.bt_11_Click);
            // 
            // bt_6
            // 
            this.bt_6.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_6.FontBold = true;
            this.bt_6.Image = ((System.Drawing.Image)(resources.GetObject("bt_6.Image")));
            this.bt_6.Name = "bt_6";
            this.bt_6.Text = "6";
            this.bt_6.Click += new System.EventHandler(this.bt_6_Click);
            // 
            // bt_12
            // 
            this.bt_12.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_12.FontBold = true;
            this.bt_12.Image = ((System.Drawing.Image)(resources.GetObject("bt_12.Image")));
            this.bt_12.Name = "bt_12";
            this.bt_12.Text = "12";
            this.bt_12.Click += new System.EventHandler(this.bt_12_Click);
            // 
            // bt_18
            // 
            this.bt_18.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_18.FontBold = true;
            this.bt_18.Image = ((System.Drawing.Image)(resources.GetObject("bt_18.Image")));
            this.bt_18.Name = "bt_18";
            this.bt_18.Text = "18";
            this.bt_18.Click += new System.EventHandler(this.bt_18_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.stiViewerControl1);
            this.panel1.Controls.Add(this.crystalReportViewer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 109);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel1.Size = new System.Drawing.Size(794, 318);
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
            this.stiViewerControl1.Size = new System.Drawing.Size(794, 318);
            this.stiViewerControl1.TabIndex = 3;
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
            this.crystalReportViewer1.Size = new System.Drawing.Size(794, 318);
            this.crystalReportViewer1.TabIndex = 2;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // Form_SaleFactorPrint1
            // 
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Form_SaleFactorPrint1";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "چاپ فاکتور فروش";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FactorPrint_FormClosing);
            this.Load += new System.EventHandler(this.Form_FactorPrint_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mlt_ACC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
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
        private DevComponents.DotNetBar.ButtonItem bt_10;
        private DevComponents.DotNetBar.ButtonItem bt_16;
        private DevComponents.DotNetBar.ButtonItem bt_5;
        private DevComponents.DotNetBar.ButtonItem bt_11;
        private DevComponents.DotNetBar.ButtonItem bt_6;
        private DevComponents.DotNetBar.ButtonItem bt_12;
        private DevComponents.DotNetBar.ButtonItem bt_18;
        private System.Windows.Forms.Panel panel1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private DevComponents.DotNetBar.ButtonItem bt_22;
        private Stimulsoft.Report.Viewer.StiViewerControl stiViewerControl1;
        private DevComponents.DotNetBar.ButtonItem btn_Design;
        private System.Windows.Forms.CheckBox SettelmentWithFactor;
        private System.Windows.Forms.CheckBox ChkEcoCode;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
    }
}