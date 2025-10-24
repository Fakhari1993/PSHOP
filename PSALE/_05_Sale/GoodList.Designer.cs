namespace PSHOP._05_Sale
{
    partial class GoodList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GoodList));
            Janus.Windows.GridEX.GridEXLayout gridEX_SaleFactor_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.bindingNavigator1 = new System.Windows.Forms.BindingNavigator(this.components);
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.gridEX_SaleFactor = new Janus.Windows.GridEX.GridEX();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            this.gridEXPrintDocument1 = new Janus.Windows.GridEX.GridEXPrintDocument();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.gridEXExporter1 = new Janus.Windows.GridEX.Export.GridEXExporter(this.components);
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).BeginInit();
            this.bindingNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_SaleFactor)).BeginInit();
            this.SuspendLayout();
            // 
            // bindingNavigator1
            // 
            this.bindingNavigator1.AddNewItem = null;
            this.bindingNavigator1.BackgroundImage = global::PSHOP.Properties.Resources.me_bg;
            this.bindingNavigator1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.bindingNavigator1.CountItem = null;
            this.bindingNavigator1.DeleteItem = null;
            this.bindingNavigator1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bindingNavigator1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripSeparator11,
            this.toolStripButton1});
            this.bindingNavigator1.Location = new System.Drawing.Point(0, 0);
            this.bindingNavigator1.MoveFirstItem = null;
            this.bindingNavigator1.MoveLastItem = null;
            this.bindingNavigator1.MoveNextItem = null;
            this.bindingNavigator1.MovePreviousItem = null;
            this.bindingNavigator1.Name = "bindingNavigator1";
            this.bindingNavigator1.PositionItem = null;
            this.bindingNavigator1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.bindingNavigator1.Size = new System.Drawing.Size(704, 25);
            this.bindingNavigator1.TabIndex = 11;
            this.bindingNavigator1.Text = "bindingNavigator2";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(55, 22);
            this.toolStripDropDownButton1.Text = "اکسل";
            this.toolStripDropDownButton1.Click += new System.EventHandler(this.toolStripDropDownButton1_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButton1.ForeColor = System.Drawing.Color.Black;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.toolStripButton1.Size = new System.Drawing.Size(46, 22);
            this.toolStripButton1.Text = "چاپ";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // gridEX_SaleFactor
            // 
            this.gridEX_SaleFactor.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_SaleFactor.AlternatingColors = true;
            this.gridEX_SaleFactor.BuiltInTextsData = resources.GetString("gridEX_SaleFactor.BuiltInTextsData");
            gridEX_SaleFactor_DesignTimeLayout.LayoutString = resources.GetString("gridEX_SaleFactor_DesignTimeLayout.LayoutString");
            this.gridEX_SaleFactor.DesignTimeLayout = gridEX_SaleFactor_DesignTimeLayout;
            this.gridEX_SaleFactor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_SaleFactor.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_SaleFactor.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_SaleFactor.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_SaleFactor.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_SaleFactor.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_SaleFactor.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_SaleFactor.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_SaleFactor.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_SaleFactor.GridLines = Janus.Windows.GridEX.GridLines.Vertical;
            this.gridEX_SaleFactor.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX_SaleFactor.GroupByBoxVisible = false;
            this.gridEX_SaleFactor.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX_SaleFactor.Location = new System.Drawing.Point(0, 25);
            this.gridEX_SaleFactor.Name = "gridEX_SaleFactor";
            this.gridEX_SaleFactor.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_SaleFactor.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_SaleFactor.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_SaleFactor.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_SaleFactor.Size = new System.Drawing.Size(704, 417);
            this.gridEX_SaleFactor.TabIndex = 12;
            this.gridEX_SaleFactor.TableHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_SaleFactor.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.gridEX_SaleFactor.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.White;
            this.gridEX_SaleFactor.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_SaleFactor.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_SaleFactor.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            // 
            // printDialog1
            // 
            this.printDialog1.Document = this.gridEXPrintDocument1;
            this.printDialog1.UseEXDialog = true;
            // 
            // gridEXPrintDocument1
            // 
            this.gridEXPrintDocument1.FitColumns = Janus.Windows.GridEX.FitColumnsMode.SizingColumns;
            this.gridEXPrintDocument1.GridEX = this.gridEX_SaleFactor;
            this.gridEXPrintDocument1.HeaderDistance = 20;
            this.gridEXPrintDocument1.PageHeaderCenter = " ";
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Document = this.gridEXPrintDocument1;
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // gridEXExporter1
            // 
            this.gridEXExporter1.GridEX = this.gridEX_SaleFactor;
            // 
            // pageSetupDialog1
            // 
            this.pageSetupDialog1.Document = this.gridEXPrintDocument1;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "Excel files|*.xls;*.xlsx";
            // 
            // GoodList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 442);
            this.Controls.Add(this.gridEX_SaleFactor);
            this.Controls.Add(this.bindingNavigator1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "GoodList";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "لیست کالاهای ناموجود";
            this.Load += new System.EventHandler(this.GoodList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.bindingNavigator1)).EndInit();
            this.bindingNavigator1.ResumeLayout(false);
            this.bindingNavigator1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_SaleFactor)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingNavigator bindingNavigator1;
        private System.Windows.Forms.ToolStripButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private Janus.Windows.GridEX.GridEX gridEX_SaleFactor;
        private System.Windows.Forms.PrintDialog printDialog1;
        private Janus.Windows.GridEX.GridEXPrintDocument gridEXPrintDocument1;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private Janus.Windows.GridEX.Export.GridEXExporter gridEXExporter1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;

    }
}