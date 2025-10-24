namespace PSALE.Sale
{
    partial class Frm_006_ResidDialog
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
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label column04Label;
            System.Windows.Forms.Label column03Label;
            Janus.Windows.GridEX.GridEXLayout multiColumnCombo1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_006_ResidDialog));
            Janus.Windows.Common.JanusColorScheme janusColorScheme1 = new Janus.Windows.Common.JanusColorScheme();
            Janus.Windows.GridEX.GridEXLayout multiColumnCombo2_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.uiGroupBox1 = new Janus.Windows.EditControls.UIGroupBox();
            this.buttonX2 = new DevComponents.DotNetBar.ButtonX();
            this.buttonX1 = new DevComponents.DotNetBar.ButtonX();
            this.multiColumnCombo1 = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.VisualStyleManager1 = new Janus.Windows.Common.VisualStyleManager(this.components);
            this.multiColumnCombo2 = new Janus.Windows.GridEX.EditControls.MultiColumnCombo();
            this.faDatePicker1 = new FarsiLibrary.Win.Controls.FADatePicker();
            this.dataSet_Sale1 = new PSALE.Sale.DataSet_Sale();
            this.table_011_PwhrsReceiptTableAdapter = new PSALE.Sale.DataSet_SaleTableAdapters.Table_011_PwhrsReceiptTableAdapter();
            this.table_012_Child_PwhrsReceiptTableAdapter = new PSALE.Sale.DataSet_SaleTableAdapters.Table_012_Child_PwhrsReceiptTableAdapter();
            this.tableAdapterManager = new PSALE.Sale.DataSet_SaleTableAdapters.TableAdapterManager();
            label2 = new System.Windows.Forms.Label();
            column04Label = new System.Windows.Forms.Label();
            column03Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).BeginInit();
            this.uiGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.multiColumnCombo1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiColumnCombo2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale1)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label2.AutoSize = true;
            label2.BackColor = System.Drawing.Color.Transparent;
            label2.Location = new System.Drawing.Point(312, 34);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(44, 13);
            label2.TabIndex = 61;
            label2.Text = "* تاریخ :";
            // 
            // column04Label
            // 
            column04Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            column04Label.AutoSize = true;
            column04Label.BackColor = System.Drawing.Color.Transparent;
            column04Label.Location = new System.Drawing.Point(279, 111);
            column04Label.Name = "column04Label";
            column04Label.Size = new System.Drawing.Size(77, 13);
            column04Label.TabIndex = 60;
            column04Label.Text = "* عملکرد انبار :";
            // 
            // column03Label
            // 
            column03Label.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            column03Label.AutoSize = true;
            column03Label.BackColor = System.Drawing.Color.Transparent;
            column03Label.Location = new System.Drawing.Point(316, 74);
            column03Label.Name = "column03Label";
            column03Label.Size = new System.Drawing.Size(40, 13);
            column03Label.TabIndex = 62;
            column03Label.Text = "* انبار :";
            // 
            // uiGroupBox1
            // 
            this.uiGroupBox1.BackgroundStyle = Janus.Windows.EditControls.BackgroundStyle.TabPage;
            this.uiGroupBox1.Controls.Add(this.buttonX2);
            this.uiGroupBox1.Controls.Add(this.buttonX1);
            this.uiGroupBox1.Controls.Add(column03Label);
            this.uiGroupBox1.Controls.Add(this.multiColumnCombo1);
            this.uiGroupBox1.Controls.Add(this.multiColumnCombo2);
            this.uiGroupBox1.Controls.Add(label2);
            this.uiGroupBox1.Controls.Add(this.faDatePicker1);
            this.uiGroupBox1.Controls.Add(column04Label);
            this.uiGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.uiGroupBox1.Name = "uiGroupBox1";
            this.uiGroupBox1.Office2007ColorScheme = Janus.Windows.UI.Office2007ColorScheme.Custom;
            this.uiGroupBox1.Size = new System.Drawing.Size(372, 202);
            this.uiGroupBox1.TabIndex = 0;
            this.uiGroupBox1.Text = "اطلاعات تکمیلی رسید انبار";
            this.uiGroupBox1.VisualStyleManager = this.VisualStyleManager1;
            // 
            // buttonX2
            // 
            this.buttonX2.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX2.BackColor = System.Drawing.Color.Transparent;
            this.buttonX2.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.buttonX2.Location = new System.Drawing.Point(189, 146);
            this.buttonX2.Name = "buttonX2";
            this.buttonX2.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(7, 3, 2, 8);
            this.buttonX2.Size = new System.Drawing.Size(79, 37);
            this.buttonX2.TabIndex = 64;
            this.buttonX2.Text = "انصراف";
            this.buttonX2.Click += new System.EventHandler(this.buttonX2_Click);
            // 
            // buttonX1
            // 
            this.buttonX1.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.buttonX1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonX1.BackColor = System.Drawing.Color.Transparent;
            this.buttonX1.ColorTable = DevComponents.DotNetBar.eButtonColor.Office2007WithBackground;
            this.buttonX1.Location = new System.Drawing.Point(274, 146);
            this.buttonX1.Name = "buttonX1";
            this.buttonX1.Shape = new DevComponents.DotNetBar.RoundRectangleShapeDescriptor(7, 3, 2, 8);
            this.buttonX1.Size = new System.Drawing.Size(79, 37);
            this.buttonX1.TabIndex = 63;
            this.buttonX1.Text = "ذخیره رسید";
            this.buttonX1.Click += new System.EventHandler(this.buttonX1_Click);
            // 
            // multiColumnCombo1
            // 
            this.multiColumnCombo1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            multiColumnCombo1_DesignTimeLayout.LayoutString = resources.GetString("multiColumnCombo1_DesignTimeLayout.LayoutString");
            this.multiColumnCombo1.DesignTimeLayout = multiColumnCombo1_DesignTimeLayout;
            this.multiColumnCombo1.DisplayMember = "column02";
            this.multiColumnCombo1.Location = new System.Drawing.Point(27, 106);
            this.multiColumnCombo1.Name = "multiColumnCombo1";
            this.multiColumnCombo1.SelectedIndex = -1;
            this.multiColumnCombo1.SelectedItem = null;
            this.multiColumnCombo1.Size = new System.Drawing.Size(248, 21);
            this.multiColumnCombo1.TabIndex = 59;
            this.multiColumnCombo1.ValueMember = "columnid";
            this.multiColumnCombo1.VisualStyleManager = this.VisualStyleManager1;
            // 
            // VisualStyleManager1
            // 
            janusColorScheme1.HighlightTextColor = System.Drawing.SystemColors.HighlightText;
            janusColorScheme1.Name = "Scheme0";
            janusColorScheme1.Office2007ColorScheme = Janus.Windows.Common.Office2007ColorScheme.Custom;
            janusColorScheme1.Office2007CustomColor = System.Drawing.Color.SteelBlue;
            janusColorScheme1.VisualStyle = Janus.Windows.Common.VisualStyle.Office2007;
            this.VisualStyleManager1.ColorSchemes.Add(janusColorScheme1);
            this.VisualStyleManager1.DefaultColorScheme = "Scheme0";
            // 
            // multiColumnCombo2
            // 
            this.multiColumnCombo2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            multiColumnCombo2_DesignTimeLayout.LayoutString = resources.GetString("multiColumnCombo2_DesignTimeLayout.LayoutString");
            this.multiColumnCombo2.DesignTimeLayout = multiColumnCombo2_DesignTimeLayout;
            this.multiColumnCombo2.DisplayMember = "column02";
            this.multiColumnCombo2.Location = new System.Drawing.Point(27, 69);
            this.multiColumnCombo2.Name = "multiColumnCombo2";
            this.multiColumnCombo2.SelectedIndex = -1;
            this.multiColumnCombo2.SelectedItem = null;
            this.multiColumnCombo2.SelectInDataSource = true;
            this.multiColumnCombo2.Size = new System.Drawing.Size(248, 21);
            this.multiColumnCombo2.TabIndex = 58;
            this.multiColumnCombo2.ValueMember = "columnid";
            this.multiColumnCombo2.VisualStyleManager = this.VisualStyleManager1;
            // 
            // faDatePicker1
            // 
            this.faDatePicker1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.faDatePicker1.IsDefault = true;
            this.faDatePicker1.Location = new System.Drawing.Point(27, 34);
            this.faDatePicker1.Name = "faDatePicker1";
            this.faDatePicker1.Size = new System.Drawing.Size(248, 20);
            this.faDatePicker1.TabIndex = 57;
            this.faDatePicker1.Theme = FarsiLibrary.Win.Enums.ThemeTypes.Office2007;
            // 
            // dataSet_Sale1
            // 
            this.dataSet_Sale1.DataSetName = "DataSet_Sale";
            this.dataSet_Sale1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // table_011_PwhrsReceiptTableAdapter
            // 
            this.table_011_PwhrsReceiptTableAdapter.ClearBeforeFill = true;
            // 
            // table_012_Child_PwhrsReceiptTableAdapter
            // 
            this.table_012_Child_PwhrsReceiptTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.Table_007_FactorBeforeTableAdapter = null;
            this.tableAdapterManager.Table_007_PwhrsDraftTableAdapter = null;
            this.tableAdapterManager.Table_008_Child_PwhrsDraftTableAdapter = null;
            this.tableAdapterManager.Table_008_Child1_FactorBeforeTableAdapter = null;
            this.tableAdapterManager.Table_009_Child2_FactorBeforeTableAdapter = null;
            this.tableAdapterManager.Table_010_SaleFactorTableAdapter = null;
            this.tableAdapterManager.Table_011_Child1_SaleFactorTableAdapter = null;
            this.tableAdapterManager.Table_011_PwhrsReceiptTableAdapter = this.table_011_PwhrsReceiptTableAdapter;
            this.tableAdapterManager.Table_012_Child2_SaleFactorTableAdapter = null;
            this.tableAdapterManager.Table_018_MarjooiSaleTableAdapter = null;
            this.tableAdapterManager.Table_019_Child1_MarjooiSaleTableAdapter = null;
            this.tableAdapterManager.Table_020_Child2_MarjooiSaleTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = PSALE.Sale.DataSet_SaleTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // Frm_006_ResidDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 204);
            this.Controls.Add(this.uiGroupBox1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.Name = "Frm_006_ResidDialog";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "مشخصات رسید";
            this.Load += new System.EventHandler(this.Frm_006_ResidDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiGroupBox1)).EndInit();
            this.uiGroupBox1.ResumeLayout(false);
            this.uiGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.multiColumnCombo1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiColumnCombo2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Janus.Windows.EditControls.UIGroupBox uiGroupBox1;
        internal Janus.Windows.Common.VisualStyleManager VisualStyleManager1;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo multiColumnCombo1;
        private Janus.Windows.GridEX.EditControls.MultiColumnCombo multiColumnCombo2;
        private FarsiLibrary.Win.Controls.FADatePicker faDatePicker1;
        private DevComponents.DotNetBar.ButtonX buttonX2;
        private DevComponents.DotNetBar.ButtonX buttonX1;
        private DataSet_Sale dataSet_Sale1;
        private PSALE.Sale.DataSet_SaleTableAdapters.Table_011_PwhrsReceiptTableAdapter table_011_PwhrsReceiptTableAdapter;
        private PSALE.Sale.DataSet_SaleTableAdapters.Table_012_Child_PwhrsReceiptTableAdapter table_012_Child_PwhrsReceiptTableAdapter;
        private PSALE.Sale.DataSet_SaleTableAdapters.TableAdapterManager tableAdapterManager;
    }
}