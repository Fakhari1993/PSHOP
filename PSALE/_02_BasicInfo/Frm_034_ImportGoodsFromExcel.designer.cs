namespace PSHOP._02_BasicInfo
{
    partial class Frm_034_ImportGoodsFromExcel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Frm_034_ImportGoodsFromExcel));
            Janus.Windows.GridEX.GridEXLayout gridEX_Excel_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX1_Layout_0 = new Janus.Windows.GridEX.GridEXLayout();
            this.panelEx1 = new DevComponents.DotNetBar.PanelEx();
            this.bt_OpenFile = new Janus.Windows.EditControls.UIButton();
            this.txt_FileName = new Janus.Windows.GridEX.EditControls.EditBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bar1 = new DevComponents.DotNetBar.Bar();
            this.bt_NextSheet = new DevComponents.DotNetBar.ButtonItem();
            this.lbl_SheetName = new DevComponents.DotNetBar.LabelItem();
            this.bt_PrevSheet = new DevComponents.DotNetBar.ButtonItem();
            this.progressBarItem1 = new DevComponents.DotNetBar.ProgressBarItem();
            this.bt_Confirm = new DevComponents.DotNetBar.ButtonItem();
            this.gridEX_Detail = new Janus.Windows.GridEX.GridEX();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.uiPanelManager1 = new Janus.Windows.UI.Dock.UIPanelManager(this.components);
            this.uiPanel0 = new Janus.Windows.UI.Dock.UIPanel();
            this.uiPanel0Container = new Janus.Windows.UI.Dock.UIPanelInnerContainer();
            this.gridEX_Excel = new Janus.Windows.GridEX.GridEX();
            this.panelEx2 = new DevComponents.DotNetBar.PanelEx();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.panelEx1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Detail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).BeginInit();
            this.uiPanel0.SuspendLayout();
            this.uiPanel0Container.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Excel)).BeginInit();
            this.panelEx2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelEx1
            // 
            this.panelEx1.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx1.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx1.Controls.Add(this.bt_OpenFile);
            this.panelEx1.Controls.Add(this.txt_FileName);
            this.panelEx1.Controls.Add(this.label1);
            this.panelEx1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelEx1.Location = new System.Drawing.Point(0, 0);
            this.panelEx1.Name = "panelEx1";
            this.panelEx1.Size = new System.Drawing.Size(794, 45);
            this.panelEx1.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx1.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx1.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx1.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx1.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx1.Style.GradientAngle = 90;
            this.panelEx1.TabIndex = 1;
            // 
            // bt_OpenFile
            // 
            this.bt_OpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bt_OpenFile.Image = ((System.Drawing.Image)(resources.GetObject("bt_OpenFile.Image")));
            this.bt_OpenFile.Location = new System.Drawing.Point(457, 11);
            this.bt_OpenFile.Name = "bt_OpenFile";
            this.bt_OpenFile.OfficeColorScheme = Janus.Windows.UI.OfficeColorScheme.Custom;
            this.bt_OpenFile.OfficeCustomColor = System.Drawing.Color.Yellow;
            this.bt_OpenFile.Size = new System.Drawing.Size(26, 21);
            this.bt_OpenFile.TabIndex = 2;
            this.bt_OpenFile.ToolTipText = "باز کردن فایل Excel";
            this.bt_OpenFile.VisualStyle = Janus.Windows.UI.VisualStyle.Office2007;
            this.bt_OpenFile.Click += new System.EventHandler(this.bt_OpenFile_Click);
            // 
            // txt_FileName
            // 
            this.txt_FileName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_FileName.Location = new System.Drawing.Point(489, 11);
            this.txt_FileName.Name = "txt_FileName";
            this.txt_FileName.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Black;
            this.txt_FileName.Size = new System.Drawing.Size(241, 21);
            this.txt_FileName.TabIndex = 1;
            this.txt_FileName.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(736, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "نام فایل:";
            // 
            // bar1
            // 
            this.bar1.AntiAlias = true;
            this.bar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bar1.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bar1.Items.AddRange(new DevComponents.DotNetBar.BaseItem[] {
            this.bt_NextSheet,
            this.lbl_SheetName,
            this.bt_PrevSheet,
            this.progressBarItem1,
            this.bt_Confirm});
            this.bar1.Location = new System.Drawing.Point(0, 427);
            this.bar1.Name = "bar1";
            this.bar1.Size = new System.Drawing.Size(794, 25);
            this.bar1.Stretch = true;
            this.bar1.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.bar1.TabIndex = 2;
            this.bar1.TabStop = false;
            this.bar1.Text = "bar1";
            // 
            // bt_NextSheet
            // 
            this.bt_NextSheet.BeginGroup = true;
            this.bt_NextSheet.Image = ((System.Drawing.Image)(resources.GetObject("bt_NextSheet.Image")));
            this.bt_NextSheet.Name = "bt_NextSheet";
            this.bt_NextSheet.Text = "buttonItem1";
            this.bt_NextSheet.Click += new System.EventHandler(this.bt_NextSheet_Click);
            // 
            // lbl_SheetName
            // 
            this.lbl_SheetName.ForeColor = System.Drawing.Color.Black;
            this.lbl_SheetName.Name = "lbl_SheetName";
            this.lbl_SheetName.Text = "Sheet";
            // 
            // bt_PrevSheet
            // 
            this.bt_PrevSheet.BeginGroup = true;
            this.bt_PrevSheet.Image = ((System.Drawing.Image)(resources.GetObject("bt_PrevSheet.Image")));
            this.bt_PrevSheet.Name = "bt_PrevSheet";
            this.bt_PrevSheet.Text = "buttonItem2";
            this.bt_PrevSheet.Click += new System.EventHandler(this.bt_PrevSheet_Click);
            // 
            // progressBarItem1
            // 
            // 
            // 
            // 
            this.progressBarItem1.BackStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
            this.progressBarItem1.ChunkGradientAngle = 0F;
            this.progressBarItem1.MenuVisibility = DevComponents.DotNetBar.eMenuVisibility.VisibleAlways;
            this.progressBarItem1.Name = "progressBarItem1";
            this.progressBarItem1.ProgressType = DevComponents.DotNetBar.eProgressItemType.Marquee;
            this.progressBarItem1.RecentlyUsed = false;
            // 
            // bt_Confirm
            // 
            this.bt_Confirm.BeginGroup = true;
            this.bt_Confirm.ButtonStyle = DevComponents.DotNetBar.eButtonStyle.ImageAndText;
            this.bt_Confirm.ColorTable = DevComponents.DotNetBar.eButtonColor.Blue;
            this.bt_Confirm.FontBold = true;
            this.bt_Confirm.ForeColor = System.Drawing.Color.Black;
            this.bt_Confirm.Name = "bt_Confirm";
            this.bt_Confirm.Stretch = true;
            this.bt_Confirm.Text = "تأیید";
            this.bt_Confirm.Click += new System.EventHandler(this.bt_Confirm_Click);
            // 
            // gridEX_Detail
            // 
            this.gridEX_Detail.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Detail.AlternatingColors = true;
            this.gridEX_Detail.AlternatingRowFormatStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Detail.BuiltInTextsData = resources.GetString("gridEX_Detail.BuiltInTextsData");
            this.gridEX_Detail.CardWidth = 751;
            this.gridEX_Detail.CellSelectionMode = Janus.Windows.GridEX.CellSelectionMode.SingleCell;
            this.gridEX_Detail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Detail.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX_Detail.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.gridEX_Detail.FilterRowButtonStyle = Janus.Windows.GridEX.FilterRowButtonStyle.ConditionOperatorDropDown;
            this.gridEX_Detail.FilterRowFormatStyle.BackColor = System.Drawing.Color.MistyRose;
            this.gridEX_Detail.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.Lavender;
            this.gridEX_Detail.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Detail.FilterRowFormatStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Detail.FilterRowUpdateMode = Janus.Windows.GridEX.FilterRowUpdateMode.WhenValueChanges;
            this.gridEX_Detail.FocusCellFormatStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Detail.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Detail.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX_Detail.GroupByBoxVisible = false;
            this.gridEX_Detail.HeaderFormatStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Detail.Location = new System.Drawing.Point(3, 48);
            this.gridEX_Detail.Name = "gridEX_Detail";
            this.gridEX_Detail.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX_Detail.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Detail.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
            this.gridEX_Detail.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX_Detail.RecordNavigator = true;
            this.gridEX_Detail.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridEX_Detail.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_Detail.RowHeaderFormatStyle.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX_Detail.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Detail.SelectionMode = Janus.Windows.GridEX.SelectionMode.MultipleCellSelection;
            this.gridEX_Detail.SettingsKey = "Form07_ViewDrafts_Detail4";
            this.gridEX_Detail.Size = new System.Drawing.Size(539, 376);
            this.gridEX_Detail.TabIndex = 3;
            this.gridEX_Detail.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Detail.TotalRowFormatStyle.BackColor = System.Drawing.Color.LightSlateGray;
            this.gridEX_Detail.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX_Detail.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX_Detail.VisualStyle = Janus.Windows.GridEX.VisualStyle.VS2010;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Excel File|*.xlsx";
            // 
            // uiPanelManager1
            // 
            this.uiPanelManager1.ContainerControl = this;
            this.uiPanel0.Id = new System.Guid("1c2a21b9-d994-48b7-88ce-92ef39f19e97");
            this.uiPanelManager1.Panels.Add(this.uiPanel0);
            // 
            // Design Time Panel Info:
            // 
            this.uiPanelManager1.BeginPanelInfo();
            this.uiPanelManager1.AddDockPanelInfo(new System.Guid("1c2a21b9-d994-48b7-88ce-92ef39f19e97"), Janus.Windows.UI.Dock.PanelDockStyle.Right, new System.Drawing.Size(249, 376), true);
            this.uiPanelManager1.AddFloatingPanelInfo(new System.Guid("1c2a21b9-d994-48b7-88ce-92ef39f19e97"), new System.Drawing.Point(-1, -1), new System.Drawing.Size(-1, -1), false);
            this.uiPanelManager1.EndPanelInfo();
            // 
            // uiPanel0
            // 
            this.uiPanel0.AllowPanelDrag = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.AllowPanelDrop = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.AllowResize = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.CaptionStyle = Janus.Windows.UI.Dock.PanelCaptionStyle.Dark;
            this.uiPanel0.CaptionVisible = Janus.Windows.UI.InheritableBoolean.True;
            this.uiPanel0.CloseButtonVisible = Janus.Windows.UI.InheritableBoolean.False;
            this.uiPanel0.InnerContainer = this.uiPanel0Container;
            this.uiPanel0.Location = new System.Drawing.Point(542, 48);
            this.uiPanel0.Name = "uiPanel0";
            this.uiPanel0.Size = new System.Drawing.Size(249, 376);
            this.uiPanel0.TabIndex = 4;
            this.uiPanel0.Text = "Excel ترتیب ستونهای فایل";
            this.uiPanel0.TextAlignment = Janus.Windows.UI.Dock.PanelTextAlignment.Far;
            // 
            // uiPanel0Container
            // 
            this.uiPanel0Container.Controls.Add(this.gridEX_Excel);
            this.uiPanel0Container.Controls.Add(this.panelEx2);
            this.uiPanel0Container.Location = new System.Drawing.Point(5, 23);
            this.uiPanel0Container.Name = "uiPanel0Container";
            this.uiPanel0Container.Size = new System.Drawing.Size(243, 352);
            this.uiPanel0Container.TabIndex = 0;
            // 
            // gridEX_Excel
            // 
            this.gridEX_Excel.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
            this.gridEX_Excel.AlternatingColors = true;
            gridEX_Excel_DesignTimeLayout.LayoutString = resources.GetString("gridEX_Excel_DesignTimeLayout.LayoutString");
            this.gridEX_Excel.DesignTimeLayout = gridEX_Excel_DesignTimeLayout;
            this.gridEX_Excel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridEX_Excel.GroupByBoxVisible = false;
            this.gridEX_Excel.Location = new System.Drawing.Point(0, 0);
            this.gridEX_Excel.Name = "gridEX_Excel";
            this.gridEX_Excel.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
            this.gridEX_Excel.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX_Excel.Size = new System.Drawing.Size(243, 255);
            this.gridEX_Excel.TabIndex = 20;
            this.gridEX_Excel.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2007;
            // 
            // panelEx2
            // 
            this.panelEx2.CanvasColor = System.Drawing.SystemColors.Control;
            this.panelEx2.ColorSchemeStyle = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
            this.panelEx2.Controls.Add(this.label3);
            this.panelEx2.Controls.Add(this.label2);
            this.panelEx2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelEx2.Location = new System.Drawing.Point(0, 255);
            this.panelEx2.Name = "panelEx2";
            this.panelEx2.Size = new System.Drawing.Size(243, 97);
            this.panelEx2.Style.Alignment = System.Drawing.StringAlignment.Center;
            this.panelEx2.Style.BackColor1.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBackground;
            this.panelEx2.Style.Border = DevComponents.DotNetBar.eBorderType.SingleLine;
            this.panelEx2.Style.BorderColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelBorder;
            this.panelEx2.Style.ForeColor.ColorSchemePart = DevComponents.DotNetBar.eColorSchemePart.PanelText;
            this.panelEx2.Style.GradientAngle = 90;
            this.panelEx2.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.label3.Font = new System.Drawing.Font("B Traffic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label3.Location = new System.Drawing.Point(0, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(243, 63);
            this.label3.TabIndex = 2;
            this.label3.Text = " ستونهای \" موجودی منفی دارد، مواد اولیه، محصولات، خدمات، اموال و فعال\" را با مقاد" +
                "یر صفر یا یک کامل کنید. ";
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Font = new System.Drawing.Font("B Traffic", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(178)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(243, 21);
            this.label2.TabIndex = 1;
            this.label2.Text = "**سطر اول فایل را با عناوین ستونها پر کنید**";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // gridEX1
            // 
            this.gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.AllowColumnDrag = false;
            this.gridEX1.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.Black;
            this.gridEX1.BackColor = System.Drawing.Color.LightBlue;
            this.gridEX1.BlendColor = System.Drawing.Color.Transparent;
            this.gridEX1.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.gridEX1.CardBorders = false;
            this.gridEX1.CardColumnHeaderFormatStyle.BackColor = System.Drawing.Color.Transparent;
            this.gridEX1.CardHeaders = false;
            this.gridEX1.CardInnerSpacing = 6;
            this.gridEX1.CardSpacing = 11;
            this.gridEX1.CenterSingleCard = false;
            this.gridEX1.Enabled = false;
            this.gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            this.gridEX1.ExpandableCards = false;
            this.gridEX1.FilterRowFormatStyle.BackColor = System.Drawing.Color.LavenderBlush;
            this.gridEX1.FilterRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
            this.gridEX1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.gridEX1.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.gridEX1.GroupByBoxVisible = false;
            this.gridEX1.HeaderFormatStyle.BackColorGradient = System.Drawing.Color.Transparent;
            this.gridEX1.HideSelection = Janus.Windows.GridEX.HideSelection.Highlight;
            this.gridEX1.Indent = 20;
            gridEX1_Layout_0.IsCurrentLayout = true;
            gridEX1_Layout_0.Key = "PERP";
            gridEX1_Layout_0.LayoutString = resources.GetString("gridEX1_Layout_0.LayoutString");
            this.gridEX1.Layouts.AddRange(new Janus.Windows.GridEX.GridEXLayout[] {
            gridEX1_Layout_0});
            this.gridEX1.Location = new System.Drawing.Point(80, 71);
            this.gridEX1.Name = "gridEX1";
            this.gridEX1.NewRowEnterKeyBehavior = Janus.Windows.GridEX.NewRowEnterKeyBehavior.None;
            this.gridEX1.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
            this.gridEX1.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
            this.gridEX1.NewRowPosition = Janus.Windows.GridEX.NewRowPosition.BottomRow;
            this.gridEX1.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Silver;
            this.gridEX1.OfficeCustomColor = System.Drawing.Color.SteelBlue;
            this.gridEX1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gridEX1.RowFormatStyle.BackColor = System.Drawing.Color.Transparent;
            this.gridEX1.RowHeaders = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.ScrollBarWidth = 20;
            this.gridEX1.Size = new System.Drawing.Size(69, 41);
            this.gridEX1.TabIndex = 18;
            this.gridEX1.TotalRow = Janus.Windows.GridEX.InheritableBoolean.True;
            this.gridEX1.TotalRowFormatStyle.BackColor = System.Drawing.Color.Azure;
            this.gridEX1.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
            this.gridEX1.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
            this.gridEX1.View = Janus.Windows.GridEX.View.CardView;
            this.gridEX1.Visible = false;
            this.gridEX1.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
            // 
            // Frm_034_ImportGoodsFromExcel
            // 
            this.ClientSize = new System.Drawing.Size(794, 452);
            this.Controls.Add(this.gridEX_Detail);
            this.Controls.Add(this.gridEX1);
            this.Controls.Add(this.uiPanel0);
            this.Controls.Add(this.bar1);
            this.Controls.Add(this.panelEx1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "Frm_034_ImportGoodsFromExcel";
            this.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "خواندن اطلاعات از فایل Excel";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelEx1.ResumeLayout(false);
            this.panelEx1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Detail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanelManager1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiPanel0)).EndInit();
            this.uiPanel0.ResumeLayout(false);
            this.uiPanel0Container.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX_Excel)).EndInit();
            this.panelEx2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevComponents.DotNetBar.PanelEx panelEx1;
        private System.Windows.Forms.Label label1;
        private Janus.Windows.EditControls.UIButton bt_OpenFile;
        private Janus.Windows.GridEX.EditControls.EditBox txt_FileName;
        private DevComponents.DotNetBar.Bar bar1;
        private DevComponents.DotNetBar.ButtonItem bt_NextSheet;
        private DevComponents.DotNetBar.LabelItem lbl_SheetName;
        private DevComponents.DotNetBar.ButtonItem bt_PrevSheet;
        private Janus.Windows.GridEX.GridEX gridEX_Detail;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private DevComponents.DotNetBar.ButtonItem bt_Confirm;
        private DevComponents.DotNetBar.ProgressBarItem progressBarItem1;
        private Janus.Windows.UI.Dock.UIPanelManager uiPanelManager1;
        private Janus.Windows.UI.Dock.UIPanel uiPanel0;
        private Janus.Windows.UI.Dock.UIPanelInnerContainer uiPanel0Container;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private Janus.Windows.GridEX.GridEX gridEX_Excel;
        private DevComponents.DotNetBar.PanelEx panelEx2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;

    }
}