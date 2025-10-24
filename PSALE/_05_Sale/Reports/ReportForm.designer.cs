namespace PSHOP._05_Sale.Reports
{
    partial class ReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportForm));
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.chk_Logo = new System.Windows.Forms.CheckBox();
            this.bt_Display = new DevComponents.DotNetBar.ButtonX();
            this.dataSet_Sale21 = new PSHOP._05_Sale.DataSet_Sale2();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.buttonItem1 = new DevComponents.DotNetBar.ButtonItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.tableAdapterManager1 = new PSHOP._02_BasicInfo.Reports.DataSet_EtelaatePaye_ReportTableAdapters.TableAdapterManager();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale21)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.chk_Logo);
            this.panelEx1.Controls.Add(this.bt_Display);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(862, 48);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 0;
            // 
            // chk_Logo
            // 
            this.chk_Logo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chk_Logo.AutoSize = true;
            this.chk_Logo.Checked = true;
            this.chk_Logo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_Logo.Location = new System.Drawing.Point(702, 14);
            this.chk_Logo.Name = "chk_Logo";
            this.chk_Logo.Size = new System.Drawing.Size(148, 17);
            this.chk_Logo.TabIndex = 13;
            this.chk_Logo.Text = "چاپ بدون لوگو و نام شرکت";
            this.chk_Logo.UseVisualStyleBackColor = true;
            // 
            // bt_Display
            // 
            this.bt_Display.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
            this.bt_Display.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_Display.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
            this.bt_Display.Location = new System.Drawing.Point(516, 14);
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
            // panel1
            // 
            this.panel1.Controls.Add(this.crystalReportViewer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 48);
            this.panel1.Name = "panel1";
            this.panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.panel1.Size = new System.Drawing.Size(862, 404);
            this.panel1.TabIndex = 14;
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
            this.crystalReportViewer1.Size = new System.Drawing.Size(862, 404);
            this.crystalReportViewer1.TabIndex = 2;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // tableAdapterManager1
            // 
            this.tableAdapterManager1.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager1.Connection = null;
            this.tableAdapterManager1.Table_000_OrgInfoTableAdapter = null;
            this.tableAdapterManager1.UpdateOrder = PSHOP._02_BasicInfo.Reports.DataSet_EtelaatePaye_ReportTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // ReportForm
            // 
            this.ClientSize = new System.Drawing.Size(862, 452);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "ReportForm";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "چاپ فاکتور فروش";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FactorPrint_FormClosing);
            this.Load += new System.EventHandler(this.Form_FactorPrint_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet_Sale21)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private DevComponents.DotNetBar.ButtonX bt_Display;
        private DataSet_Sale2 dataSet_Sale21;
        private System.Windows.Forms.PrintDialog printDialog1;
        private DevComponents.DotNetBar.ButtonItem buttonItem1;
        private System.Windows.Forms.CheckBox chk_Logo;
        private System.Windows.Forms.Panel panel1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private _02_BasicInfo.Reports.DataSet_EtelaatePaye_ReportTableAdapters.TableAdapterManager tableAdapterManager1;
    }
}