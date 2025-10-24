namespace PSHOP._02_BasicInfo
{
    partial class Frm_030_SaleManChart
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
            Janus.Windows.GridEX.GridEXLayout mCC_Person_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_030_SaleManChart));
            this.btn_AddNode = new Janus.Windows.EditControls.UIButton();
            this.btn_RemoveNode = new Janus.Windows.EditControls.UIButton();
            this.advTree1 = new DevComponents.AdvTree.AdvTree();
            this.advTree2 = new DevComponents.AdvTree.AdvTree();
            this.advTree3 = new DevComponents.AdvTree.AdvTree();
            this.advTree4 = new DevComponents.AdvTree.AdvTree();
            this.advTree5 = new DevComponents.AdvTree.AdvTree();
            this.elementStyle4 = new DevComponents.DotNetBar.ElementStyle();
            this.nodeConnector1 = new DevComponents.AdvTree.NodeConnector();
            this.elementStyle3 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle1 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle2 = new DevComponents.DotNetBar.ElementStyle();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.table_045_SaleManChartBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataSet_05_Awards = new PSHOP._02_BasicInfo.DataSet_05_Awards();
            this.lblID = new System.Windows.Forms.Label();
            this.btn_New = new Janus.Windows.EditControls.UIButton();
            this.txt_DarsadMajmooe = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_SatheHamkari = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_DarsadMostaghim = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txt_DarsadHamkariAsli = new Janus.Windows.GridEX.EditControls.NumericEditBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.mCC_Person = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.table_045_SaleManChartTableAdapter = new PSHOP._02_BasicInfo.DataSet_05_AwardsTableAdapters.Table_045_SaleManChartTableAdapter();
            this.tableAdapterManager = new PSHOP._02_BasicInfo.DataSet_05_AwardsTableAdapters.TableAdapterManager();
            this.table_045_SaleManChartBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.elementStyle5 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle6 = new DevComponents.DotNetBar.ElementStyle();
            this.elementStyle7 = new DevComponents.DotNetBar.ElementStyle();
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table_045_SaleManChartBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_Awards)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mCC_Person)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_045_SaleManChartBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_AddNode
            // 
            this.btn_AddNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_AddNode.Location = new System.Drawing.Point(580, 211);
            this.btn_AddNode.Name = "btn_AddNode";
            this.btn_AddNode.Size = new System.Drawing.Size(135, 23);
            this.btn_AddNode.TabIndex = 5;
            this.btn_AddNode.Text = "ثبت";
            this.btn_AddNode.ToolTipText = "Ctrl+S";
            this.btn_AddNode.VisualStyle = Janus.Windows.UI.VisualStyle.Office2010;
            this.btn_AddNode.Click += new System.EventHandler(this.uiButton1_Click);
            // 
            // btn_RemoveNode
            // 
            this.btn_RemoveNode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_RemoveNode.Location = new System.Drawing.Point(429, 211);
            this.btn_RemoveNode.Name = "btn_RemoveNode";
            this.btn_RemoveNode.Size = new System.Drawing.Size(135, 23);
            this.btn_RemoveNode.TabIndex = 6;
            this.btn_RemoveNode.Text = "حذف";
            this.btn_RemoveNode.ToolTipText = "Ctrl+D";
            this.btn_RemoveNode.VisualStyle = Janus.Windows.UI.VisualStyle.Office2010;
            this.btn_RemoveNode.Click += new System.EventHandler(this.uiButton2_Click);
            // 
            // advTree1
            // 
            this.advTree1.AllowDrop = true;
            // 
            // 
            // 
            this.advTree1.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTree1.Location = new System.Drawing.Point(0, 0);
            this.advTree1.Name = "advTree1";
            this.advTree1.PathSeparator = ";";
            this.advTree1.Size = new System.Drawing.Size(0, 0);
            this.advTree1.TabIndex = 0;
            // 
            // advTree2
            // 
            this.advTree2.AllowDrop = true;
            // 
            // 
            // 
            this.advTree2.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTree2.Location = new System.Drawing.Point(0, 0);
            this.advTree2.Name = "advTree2";
            this.advTree2.PathSeparator = ";";
            this.advTree2.Size = new System.Drawing.Size(0, 0);
            this.advTree2.TabIndex = 0;
            // 
            // advTree3
            // 
            this.advTree3.AllowDrop = true;
            // 
            // 
            // 
            this.advTree3.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTree3.Location = new System.Drawing.Point(0, 0);
            this.advTree3.Name = "advTree3";
            this.advTree3.PathSeparator = ";";
            this.advTree3.Size = new System.Drawing.Size(0, 0);
            this.advTree3.TabIndex = 0;
            // 
            // advTree4
            // 
            this.advTree4.AllowDrop = true;
            // 
            // 
            // 
            this.advTree4.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTree4.Location = new System.Drawing.Point(0, 0);
            this.advTree4.Name = "advTree4";
            this.advTree4.PathSeparator = ";";
            this.advTree4.Size = new System.Drawing.Size(0, 0);
            this.advTree4.TabIndex = 0;
            // 
            // advTree5
            // 
            this.advTree5.AccessibleRole = System.Windows.Forms.AccessibleRole.Outline;
            this.advTree5.AllowDrop = true;
            this.advTree5.AlternateRowColor = System.Drawing.Color.Honeydew;
            this.advTree5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.advTree5.BackColor = System.Drawing.SystemColors.Window;
            // 
            // 
            // 
            this.advTree5.BackgroundStyle.Class = "TreeBorderKey";
            this.advTree5.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.advTree5.CellStyleSelected = this.elementStyle4;
            this.advTree5.GridLinesColor = System.Drawing.Color.LightSteelBlue;
            this.advTree5.Location = new System.Drawing.Point(12, 28);
            this.advTree5.Name = "advTree5";
            this.advTree5.NodesConnector = this.nodeConnector1;
            this.advTree5.NodeSpacing = 5;
            this.advTree5.NodeStyle = this.elementStyle7;
            this.advTree5.PathSeparator = ";";
            this.advTree5.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.advTree5.SelectionBoxStyle = DevComponents.AdvTree.eSelectionStyle.NodeMarker;
            this.advTree5.Size = new System.Drawing.Size(400, 488);
            this.advTree5.Styles.Add(this.elementStyle1);
            this.advTree5.Styles.Add(this.elementStyle2);
            this.advTree5.Styles.Add(this.elementStyle3);
            this.advTree5.Styles.Add(this.elementStyle4);
            this.advTree5.Styles.Add(this.elementStyle5);
            this.advTree5.Styles.Add(this.elementStyle6);
            this.advTree5.Styles.Add(this.elementStyle7);
            this.advTree5.TabIndex = 1008;
            this.advTree5.AfterNodeSelect += new DevComponents.AdvTree.AdvTreeNodeEventHandler(this.advTree5_AfterNodeSelect);
            // 
            // elementStyle4
            // 
            this.elementStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(221)))), ((int)(((byte)(230)))), ((int)(((byte)(247)))));
            this.elementStyle4.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(168)))), ((int)(((byte)(228)))));
            this.elementStyle4.BackColorGradientAngle = 90;
            this.elementStyle4.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle4.BorderBottomWidth = 1;
            this.elementStyle4.BorderColor = System.Drawing.Color.DarkGray;
            this.elementStyle4.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle4.BorderLeftWidth = 1;
            this.elementStyle4.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle4.BorderRightWidth = 1;
            this.elementStyle4.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle4.BorderTopWidth = 1;
            this.elementStyle4.CornerDiameter = 4;
            this.elementStyle4.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle4.Description = "Blue";
            this.elementStyle4.Name = "elementStyle4";
            this.elementStyle4.PaddingBottom = 1;
            this.elementStyle4.PaddingLeft = 1;
            this.elementStyle4.PaddingRight = 1;
            this.elementStyle4.PaddingTop = 1;
            this.elementStyle4.TextColor = System.Drawing.Color.Black;
            // 
            // nodeConnector1
            // 
            this.nodeConnector1.LineColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle3
            // 
            this.elementStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(226)))));
            this.elementStyle3.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(183)))), ((int)(((byte)(201)))), ((int)(((byte)(151)))));
            this.elementStyle3.BackColorGradientAngle = 90;
            this.elementStyle3.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle3.BorderBottomWidth = 1;
            this.elementStyle3.BorderColor = System.Drawing.Color.DarkGray;
            this.elementStyle3.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle3.BorderLeftWidth = 1;
            this.elementStyle3.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle3.BorderRightWidth = 1;
            this.elementStyle3.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle3.BorderTopWidth = 1;
            this.elementStyle3.CornerDiameter = 4;
            this.elementStyle3.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle3.Description = "Green";
            this.elementStyle3.Name = "elementStyle3";
            this.elementStyle3.PaddingBottom = 1;
            this.elementStyle3.PaddingLeft = 1;
            this.elementStyle3.PaddingRight = 1;
            this.elementStyle3.PaddingTop = 1;
            this.elementStyle3.TextColor = System.Drawing.Color.Black;
            // 
            // elementStyle1
            // 
            this.elementStyle1.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle1.Name = "elementStyle1";
            this.elementStyle1.TextColor = System.Drawing.SystemColors.ControlText;
            // 
            // elementStyle2
            // 
            this.elementStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(227)))), ((int)(((byte)(245)))));
            this.elementStyle2.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(158)))), ((int)(((byte)(222)))));
            this.elementStyle2.BackColorGradientAngle = 90;
            this.elementStyle2.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderBottomWidth = 1;
            this.elementStyle2.BorderColor = System.Drawing.Color.DarkGray;
            this.elementStyle2.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderLeftWidth = 1;
            this.elementStyle2.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderRightWidth = 1;
            this.elementStyle2.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle2.BorderTopWidth = 1;
            this.elementStyle2.CornerDiameter = 4;
            this.elementStyle2.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle2.Description = "Purple";
            this.elementStyle2.Name = "elementStyle2";
            this.elementStyle2.PaddingBottom = 1;
            this.elementStyle2.PaddingLeft = 1;
            this.elementStyle2.PaddingRight = 1;
            this.elementStyle2.PaddingTop = 1;
            this.elementStyle2.TextColor = System.Drawing.Color.Black;
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.Controls.Add(this.lblID);
            this.uiGroupBox1.Controls.Add(this.btn_New);
            this.uiGroupBox1.Controls.Add(this.txt_DarsadMajmooe);
            this.uiGroupBox1.Controls.Add(this.label5);
            this.uiGroupBox1.Controls.Add(this.txt_SatheHamkari);
            this.uiGroupBox1.Controls.Add(this.label4);
            this.uiGroupBox1.Controls.Add(this.txt_DarsadMostaghim);
            this.uiGroupBox1.Controls.Add(this.label3);
            this.uiGroupBox1.Controls.Add(this.txt_DarsadHamkariAsli);
            this.uiGroupBox1.Controls.Add(this.label2);
            this.uiGroupBox1.Controls.Add(this.label1);
            this.uiGroupBox1.Controls.Add(this.mCC_Person);
            this.uiGroupBox1.Controls.Add(this.advTree5);
            this.uiGroupBox1.Controls.Add(this.btn_RemoveNode);
            this.uiGroupBox1.Controls.Add(this.btn_AddNode);
            this.uiGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Size = new System.Drawing.Size(730, 543);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = " ";
            // 
            // table_045_SaleManChartBindingSource
            // 
            this.table_045_SaleManChartBindingSource.DataMember = "Table_045_SaleManChart";
            this.table_045_SaleManChartBindingSource.DataSource = this.dataSet_05_Awards;
            // 
            // dataSet_05_Awards
            // 
            this.dataSet_05_Awards.DataSetName = "DataSet_05_Awards";
            this.dataSet_05_Awards.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // lblID
            // 
            this.lblID.Location = new System.Drawing.Point(500, 348);
            this.lblID.Name = "lblID";
            this.lblID.Size = new System.Drawing.Size(0, 0);
            this.lblID.TabIndex = 17;
            this.lblID.Text = "0";
            // 
            // btn_New
            // 
            this.btn_New.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_New.Location = new System.Drawing.Point(429, 34);
            this.btn_New.Name = "btn_New";
            this.btn_New.Size = new System.Drawing.Size(286, 23);
            this.btn_New.TabIndex = 15;
            this.btn_New.Text = "جدید";
            this.btn_New.ToolTipText = "Ctrl+N";
            this.btn_New.VisualStyle = Janus.Windows.UI.VisualStyle.Office2010;
            this.btn_New.Click += new System.EventHandler(this.uiButton1_Click_1);
            // 
            // txt_DarsadMajmooe
            // 
            this.txt_DarsadMajmooe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_DarsadMajmooe.FormatString = "#,##0.###";
            this.txt_DarsadMajmooe.Location = new System.Drawing.Point(429, 177);
            this.txt_DarsadMajmooe.Name = "txt_DarsadMajmooe";
            this.txt_DarsadMajmooe.Size = new System.Drawing.Size(100, 21);
            this.txt_DarsadMajmooe.TabIndex = 4;
            this.txt_DarsadMajmooe.Text = "0";
            this.txt_DarsadMajmooe.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txt_DarsadMajmooe.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.txt_DarsadMajmooe.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mCC_Person_KeyPress);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(533, 182);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(185, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "درصد همکاری براساس درآمد مجموعه:";
            // 
            // txt_SatheHamkari
            // 
            this.txt_SatheHamkari.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_SatheHamkari.BackColor = System.Drawing.Color.AntiqueWhite;
            this.txt_SatheHamkari.FormatString = "#,##0.###";
            this.txt_SatheHamkari.Location = new System.Drawing.Point(429, 148);
            this.txt_SatheHamkari.Name = "txt_SatheHamkari";
            this.txt_SatheHamkari.Size = new System.Drawing.Size(177, 21);
            this.txt_SatheHamkari.TabIndex = 3;
            this.txt_SatheHamkari.Text = "0";
            this.txt_SatheHamkari.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txt_SatheHamkari.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.txt_SatheHamkari.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mCC_Person_KeyPress);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(609, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(109, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "سقف سطح همکاری:";
            // 
            // txt_DarsadMostaghim
            // 
            this.txt_DarsadMostaghim.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_DarsadMostaghim.BackColor = System.Drawing.Color.AntiqueWhite;
            this.txt_DarsadMostaghim.FormatString = "#,##0.###";
            this.txt_DarsadMostaghim.Location = new System.Drawing.Point(429, 118);
            this.txt_DarsadMostaghim.Name = "txt_DarsadMostaghim";
            this.txt_DarsadMostaghim.Size = new System.Drawing.Size(84, 21);
            this.txt_DarsadMostaghim.TabIndex = 2;
            this.txt_DarsadMostaghim.Text = "0";
            this.txt_DarsadMostaghim.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txt_DarsadMostaghim.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.txt_DarsadMostaghim.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mCC_Person_KeyPress);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(516, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(202, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "درصد همکاری مجموعه به صورت مستقیم:";
            // 
            // txt_DarsadHamkariAsli
            // 
            this.txt_DarsadHamkariAsli.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_DarsadHamkariAsli.FormatString = "#,##0.###";
            this.txt_DarsadHamkariAsli.Location = new System.Drawing.Point(429, 90);
            this.txt_DarsadHamkariAsli.Name = "txt_DarsadHamkariAsli";
            this.txt_DarsadHamkariAsli.Size = new System.Drawing.Size(180, 21);
            this.txt_DarsadHamkariAsli.TabIndex = 1;
            this.txt_DarsadHamkariAsli.Text = "0";
            this.txt_DarsadHamkariAsli.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.txt_DarsadHamkariAsli.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.txt_DarsadHamkariAsli.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mCC_Person_KeyPress);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(612, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "درصد همکاری اصلی:";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(626, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "نام مسئول فروش:";
            // 
            // mCC_Person
            // 
            this.mCC_Person.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.mCC_Person.AutoComplete = false;
            mCC_Person_DesignTimeLayout.LayoutString = resources.GetString("mCC_Person_DesignTimeLayout.LayoutString");
            this.mCC_Person.DesignTimeLayout = mCC_Person_DesignTimeLayout;
            this.mCC_Person.DisplayMember = "Column02";
            this.mCC_Person.Location = new System.Drawing.Point(429, 63);
            this.mCC_Person.Name = "mCC_Person";
            this.mCC_Person.SelectedIndex = -1;
            this.mCC_Person.SelectedItem = null;
            this.mCC_Person.Size = new System.Drawing.Size(195, 21);
            this.mCC_Person.TabIndex = 0;
            this.mCC_Person.ValueMember = "ColumnId";
            this.mCC_Person.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            this.mCC_Person.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mCC_Person_KeyPress);
            this.mCC_Person.KeyUp += new System.Windows.Forms.KeyEventHandler(this.mCC_Person_KeyUp);
            this.mCC_Person.Leave += new System.EventHandler(this.mCC_Person_Leave);
            // 
            // table_045_SaleManChartTableAdapter
            // 
            this.table_045_SaleManChartTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_000_OrgInfoTableAdapter = null;
            this.tableAdapterManager.table_004_CommodityAndIngredientsTableAdapter = null;
            this.tableAdapterManager.Table_040_PersonGroupsTableAdapter = null;
            this.tableAdapterManager.Table_040_RialAwardsTableAdapter = null;
            this.tableAdapterManager.Table_045_SaleManChartTableAdapter = this.table_045_SaleManChartTableAdapter;
            this.tableAdapterManager.Table_045_TotalQtyAwardsTableAdapter = null;
            this.tableAdapterManager.Table_050_VehicleAwardsTableAdapter = null;
            this.tableAdapterManager.Table_115_VehicleTypeTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PSHOP._02_BasicInfo.DataSet_05_AwardsTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // table_045_SaleManChartBindingSource1
            // 
            this.table_045_SaleManChartBindingSource1.DataMember = "Table_045_SaleManChart_Table_045_SaleManChart";
            this.table_045_SaleManChartBindingSource1.DataSource = this.table_045_SaleManChartBindingSource;
            // 
            // elementStyle5
            // 
            this.elementStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.elementStyle5.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(224)))), ((int)(((byte)(252)))));
            this.elementStyle5.BackColorGradientAngle = 90;
            this.elementStyle5.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle5.BorderBottomWidth = 1;
            this.elementStyle5.BorderColor = System.Drawing.Color.DarkGray;
            this.elementStyle5.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle5.BorderLeftWidth = 1;
            this.elementStyle5.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle5.BorderRightWidth = 1;
            this.elementStyle5.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle5.BorderTopWidth = 1;
            this.elementStyle5.CornerDiameter = 4;
            this.elementStyle5.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle5.Description = "BlueLight";
            this.elementStyle5.Name = "elementStyle5";
            this.elementStyle5.PaddingBottom = 1;
            this.elementStyle5.PaddingLeft = 1;
            this.elementStyle5.PaddingRight = 1;
            this.elementStyle5.PaddingTop = 1;
            this.elementStyle5.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(84)))), ((int)(((byte)(115)))));
            // 
            // elementStyle6
            // 
            this.elementStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(77)))), ((int)(((byte)(108)))), ((int)(((byte)(152)))));
            this.elementStyle6.BackColor2 = System.Drawing.Color.Navy;
            this.elementStyle6.BackColorGradientAngle = 90;
            this.elementStyle6.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle6.BorderBottomWidth = 1;
            this.elementStyle6.BorderColor = System.Drawing.Color.Navy;
            this.elementStyle6.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle6.BorderLeftWidth = 1;
            this.elementStyle6.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle6.BorderRightWidth = 1;
            this.elementStyle6.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle6.BorderTopWidth = 1;
            this.elementStyle6.CornerDiameter = 4;
            this.elementStyle6.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle6.Description = "BlueNight";
            this.elementStyle6.Name = "elementStyle6";
            this.elementStyle6.PaddingBottom = 1;
            this.elementStyle6.PaddingLeft = 1;
            this.elementStyle6.PaddingRight = 1;
            this.elementStyle6.PaddingTop = 1;
            this.elementStyle6.TextColor = System.Drawing.Color.White;
            // 
            // elementStyle7
            // 
            this.elementStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.elementStyle7.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(224)))), ((int)(((byte)(252)))));
            this.elementStyle7.BackColorGradientAngle = 90;
            this.elementStyle7.BorderBottom = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle7.BorderBottomWidth = 1;
            this.elementStyle7.BorderColor = System.Drawing.Color.DarkGray;
            this.elementStyle7.BorderLeft = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle7.BorderLeftWidth = 1;
            this.elementStyle7.BorderRight = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle7.BorderRightWidth = 1;
            this.elementStyle7.BorderTop = DevComponents.DotNetBar.eStyleBorderType.Solid;
            this.elementStyle7.BorderTopWidth = 1;
            this.elementStyle7.CornerDiameter = 4;
            this.elementStyle7.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.elementStyle7.Description = "BlueLight";
            this.elementStyle7.Name = "elementStyle7";
            this.elementStyle7.PaddingBottom = 1;
            this.elementStyle7.PaddingLeft = 1;
            this.elementStyle7.PaddingRight = 1;
            this.elementStyle7.PaddingTop = 1;
            this.elementStyle7.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(84)))), ((int)(((byte)(115)))));
            // 
            // Frm_030_SaleManChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 543);
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Frm_030_SaleManChart";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "چارت مسئولین فروش";
            this.Load += new System.EventHandler(this.Frm_030_SaleManChart_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Frm_030_SaleManChart_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.advTree1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.advTree5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.table_045_SaleManChartBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_05_Awards)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mCC_Person)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.table_045_SaleManChartBindingSource1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.EditControls.UIButton btn_AddNode;
        private Janus.Windows.EditControls.UIButton btn_RemoveNode;
        private DevComponents.AdvTree.AdvTree advTree1;
        private DevComponents.AdvTree.AdvTree advTree2;
        private DevComponents.AdvTree.AdvTree advTree3;
        private DevComponents.AdvTree.AdvTree advTree4;
        private DevComponents.AdvTree.AdvTree advTree5;
        private DevComponents.AdvTree.NodeConnector nodeConnector1;
        private DevComponents.DotNetBar.ElementStyle elementStyle1;
        private DevComponents.DotNetBar.ElementStyle elementStyle2;
        private DevComponents.DotNetBar.ElementStyle elementStyle3;
        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        private DevComponents.DotNetBar.ElementStyle elementStyle4;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo mCC_Person;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.GridEX.EditControls.NumericEditBox txt_DarsadHamkariAsli;
        private System.Windows.Forms.Label label2;
        private Janus.Windows.GridEX.EditControls.NumericEditBox txt_DarsadMajmooe;
        private System.Windows.Forms.Label label5;
        private Janus.Windows.GridEX.EditControls.NumericEditBox txt_SatheHamkari;
        private System.Windows.Forms.Label label4;
        private Janus.Windows.GridEX.EditControls.NumericEditBox txt_DarsadMostaghim;
        private System.Windows.Forms.Label label3;
        private Janus.Windows.EditControls.UIButton btn_New;
        private System.Windows.Forms.Label lblID;
        private DataSet_05_Awards dataSet_05_Awards;
        private System.Windows.Forms.BindingSource table_045_SaleManChartBindingSource;
        private DataSet_05_AwardsTableAdapters.Table_045_SaleManChartTableAdapter table_045_SaleManChartTableAdapter;
        private DataSet_05_AwardsTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingSource table_045_SaleManChartBindingSource1;
        private DevComponents.DotNetBar.ElementStyle elementStyle5;
        private DevComponents.DotNetBar.ElementStyle elementStyle6;
        private DevComponents.DotNetBar.ElementStyle elementStyle7;
    }
}