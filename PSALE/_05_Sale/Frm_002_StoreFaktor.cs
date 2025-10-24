using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Janus.Windows.GridEX;
using DevComponents.DotNetBar;
using Stimulsoft.Report;

namespace PSHOP._05_Sale
{
    public partial class Frm_002_StoreFaktor : Form
    {
        bool _del;
        int _ID = 0, ReturnId = 0, ReturnNum = 0, ResidId = 0, ResidNum = 0;
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_Discounts ClDiscount = new Classes.Class_Discounts();
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);

        Class_UserScope UserScope = new Class_UserScope();
        DataSet DS = new DataSet();
        // SqlDataAdapter DraftAdapter, DocAdapter, ReturnAdapter;
        string ReturnDate = null;
        InputLanguage original;
        DataTable discountdt = new DataTable();
        DataTable taxdt = new DataTable();
        DataTable factordt = new DataTable();
        DataTable waredt = new DataTable();
        DataTable Sanaddt = new DataTable();
        Classes.CheckCredits clCredit = new Classes.CheckCredits();
        string EditDocNum, EditDraftNum = string.Empty;
        int LastDocnum = 0;
        bool Tasveh = false;
        bool Isadmin = false;
        Int16 projectId;
        string Tasvieh, Print;
        DataTable storefactor = new DataTable();
        //DataTable bahaDT = new DataTable();
        DataTable setting = new DataTable();
        SqlParameter ReturnDocNum;
        public Frm_002_StoreFaktor(bool del)
        {
            _del = del;
            InitializeComponent();
        }

        public Frm_002_StoreFaktor(bool del, int ID)
        {
            _del = del;
            _ID = ID;

            InitializeComponent();
        }


        public Frm_002_StoreFaktor(bool _Tasveh, string Date, string Person)
        {

            InitializeComponent();
            Tasveh = _Tasveh;
            Date = txt_date.Text;
            Person = mlt_Customer.Value.ToString();

        }

        private void Frm_002_PishFaktor_Load(object sender, EventArgs e)
        {
            
            try
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.MAIN))
                {
                    Con.Open();
                    SqlCommand Select = new SqlCommand("Select Column02 from Table_010_UserInfo where Column00='" + Class_BasicOperation._UserName + "' and Column05=" +
                    Class_BasicOperation._OrgCode + " and Column06='" + Class_BasicOperation._Year + "'", Con);
                    Isadmin = (bool.Parse(Select.ExecuteScalar().ToString()));
                    Con.Close();

                }

                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT tsi.Column05
                                                                        FROM   dbo.Table_295_StoreInfo AS tsi
                                                                               JOIN dbo.Table_296_StoreUsers AS tsu
                                                                                    ON  tsu.Column00 = tsi.ColumnId
                                                                        WHERE tsu.Column01='" + Class_BasicOperation._UserName + "'", ConBase);
                DataTable StoreTable = new DataTable();

                Adapter.Fill(StoreTable);

                if (!Isadmin && StoreTable.Rows.Count == 0) { Class_BasicOperation.ShowMsg("", "کاربر گرامی، فروشگاه شما تعیین نشده است و امکان کار با این فرم را ندارید ", "Stop"); this.Dispose(); }

                else if (StoreTable.Rows.Count > 0) projectId = Convert.ToInt16(StoreTable.Rows[0]["Column05"]);


                storefactor = clDoc.GetDefaultValues();



                string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + StoreTable.Rows[0]["Column05"] + "),0)");


                ToastNotification.ToastBackColor = Color.Aquamarine;
                ToastNotification.ToastForeColor = Color.Black;
                ReturnDocNum = new SqlParameter("ReturnDocNum", SqlDbType.Int);
                ReturnDocNum.Direction = ParameterDirection.Output;
                if (controlremain=="True")
                {
                GoodbindingSource.DataSource = clGood.MahsoolInfo( 0);
                DataTable GoodTable = clGood.MahsoolInfo(0);
                gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                }
                else
                {
                    GoodbindingSource.DataSource = clGood.GoodInfo();
                    DataTable GoodTable = clGood.GoodInfo();
                    gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                    gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                }

                mlt_Ware.DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from Table_001_PWHRS where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + "')");

                Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
                Adapter.Fill(DS, "Discount");
                gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");
                DataTable CustomerTable = clDoc.ReturnTable
              (ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
                                           dbo.Table_045_PersonInfo.Column01 AS code,
                                           dbo.Table_045_PersonInfo.Column02 AS NAME,
                                           dbo.Table_065_CityInfo.Column02 AS shahr,
                                           dbo.Table_060_ProvinceInfo.Column01 AS ostan,
                                           dbo.Table_045_PersonInfo.Column06 AS ADDRESS,
                                           dbo.Table_045_PersonInfo.Column30,
                                           Table_045_PersonInfo.Column07,
                                           Table_045_PersonInfo.Column19 AS Mobile
                                    FROM   dbo.Table_045_PersonInfo
                                           LEFT JOIN dbo.Table_065_CityInfo
                                                ON  dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                                           LEFT JOIN dbo.Table_060_ProvinceInfo
                                                ON  dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00
                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");
                mlt_Customer.DataSource = CustomerTable;


                Adapter = new SqlDataAdapter("SELECT  [Column00] AS countiD, Column01 AS countName FROM Table_070_CountUnitInfo", ConBase);
                Adapter.Fill(DS, "CountUnit");
                gridEX_List.DropDowns["CountUnit"].SetDataBinding(DS.Tables["CountUnit"], "");

                mlt_PersonSale.DataSource = (clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId  ,Column01  ,Column02  from PeopleScope(8,3)"));


                mlt_SaleType.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes");

                mlt_project.DataSource = (clDoc.ReturnTable(ConBase.ConnectionString, "Select * from Table_035_ProjectInfo"));
                mlt_Stor.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, @"select ColumnId,Column00,Column01 from Table_295_StoreInfo");

                if (_ID != 0)
                {


                    using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SALE))
                    {
                        con.Open();
                        int ID = 0;
                        SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_010_SaleFactor where columnid=" + _ID + " and (column44=" + projectId + " or '" + (Isadmin) + "'=N'True')", con);
                        try
                        {
                            ID = int.Parse(Commnad.ExecuteScalar().ToString());
                            this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, _ID);
                            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, _ID);
                            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, _ID);
                            table_010_SaleFactorBindingSource_PositionChanged(sender, e);


                        }
                        catch
                        {
                            MessageBox.Show("شماره فاکتور وارد شده نامعتبر است");
                            this.Close();
                        }
                    }

                }
                try
                {
                    using (SqlConnection ConWare1 = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        ConWare1.Open();
                        SqlCommand Update = new SqlCommand(@"UPDATE Table_032_GoodPrice
                                                SET    [Column01] = REPLACE([Column01], NCHAR(1610), NCHAR(1740))

                                                UPDATE Table_032_GoodPrice
                                                SET    [Column01] = REPLACE([Column01], NCHAR(1603), NCHAR(1705))", ConWare1);
                        Update.ExecuteNonQuery();

                    }
                    using (SqlConnection Conbase1 = new SqlConnection(Properties.Settings.Default.BASE))
                    {
                        Conbase1.Open();
                        SqlCommand Update1 = new SqlCommand(@"UPDATE Table_002_SalesTypes
                                                    SET    [Column02] = REPLACE([Column02], NCHAR(1610), NCHAR(1740))

                                                    UPDATE Table_002_SalesTypes
                                                    SET    [Column02] = REPLACE([Column02], NCHAR(1603), NCHAR(1705))", Conbase1);
                        Update1.ExecuteNonQuery();


                    }


                }
                catch
                {
                }
                Adapter = new SqlDataAdapter(
                                                                    @"SELECT        isnull(Column02,0) as Column02
                                                                        FROM           Table_030_Setting
                                                                        WHERE        (ColumnId in (45,46)) order by ColumnId  ", ConBase);
                Adapter.Fill(waredt);

                DataTable subg = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT DISTINCT tsg.columnid
                                                                                          ,tsg.column03
                                                                                    FROM   table_004_CommodityAndIngredients tcai
                                                                                           JOIN table_003_SubsidiaryGroup tsg
                                                                                                ON  tsg.column01 = tcai.column03
                                                                                                    AND tsg.columnid = tcai.column04
                                                                                    WHERE  tcai.Column51 = 1
                                                                                           AND tcai.column28 = 1
                                                                                           AND tcai.column19 = 1");

                #region کالاهای پر فروش 

                foreach (DataRow dr in subg.Rows)
                {
                    Janus.Windows.UI.Tab.UITabPage uit = new Janus.Windows.UI.Tab.UITabPage();
                    uit.Location = new System.Drawing.Point(3, 1);
                    uit.Name = "uitab" + dr["columnid"].ToString();
                    uit.Size = new System.Drawing.Size(115, 373);
                    uit.TabStop = true;
                    uit.Text = dr["column03"].ToString();
                    ////
                    Janus.Windows.GridEX.GridEX grid = new Janus.Windows.GridEX.GridEX();

                    grid.View = Janus.Windows.GridEX.View.CardView;
                    grid.AllowColumnDrag = false;
                    grid.AllowEdit = Janus.Windows.GridEX.InheritableBoolean.False;
                    grid.AlternatingColors = true;
                    grid.AlternatingRowFormatStyle.BackColor = System.Drawing.Color.MistyRose;
                    grid.AlternatingRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
                    grid.CardWidth = 751;
                    grid.ColumnAutoResize = true;
                    grid.ColumnHeaders = Janus.Windows.GridEX.InheritableBoolean.False;
                    grid.ColumnSetHeaders = Janus.Windows.GridEX.InheritableBoolean.False;
                    grid.ColumnSetNavigation = Janus.Windows.GridEX.ColumnSetNavigation.ColumnSet;



                    /// grid.DataSource = this.table_004_CommodityAndIngredientsBindingSource;
                    //gridEX1_DesignTimeLayout.LayoutString = resources.GetString("gridEX1_DesignTimeLayout.LayoutString");
                    //grid.DesignTimeLayout = gridEX1_DesignTimeLayout;
                    grid.Dock = System.Windows.Forms.DockStyle.Fill;
                    grid.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
                    grid.FocusCellFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
                    grid.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
                    grid.Font = new System.Drawing.Font("B Mitra", 12F);
                    grid.GroupByBoxVisible = false;
                    grid.ImageList = this.imageList1;
                    grid.Location = new System.Drawing.Point(0, 0);
                    grid.Margin = new System.Windows.Forms.Padding(6);
                    grid.Name = "gridEX" + dr["columnid"].ToString();
                    grid.NewRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
                    grid.NewRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
                    grid.OfficeColorScheme = Janus.Windows.GridEX.OfficeColorScheme.Custom;
                    grid.OfficeCustomColor = System.Drawing.Color.SteelBlue;
                    grid.RowHeaderContent = Janus.Windows.GridEX.RowHeaderContent.RowPosition;
                    grid.SettingsKey = "gridEX" + dr["columnid"].ToString();
                    grid.Size = new System.Drawing.Size(115, 373);
                    //grid.TabIndex = 4;
                    grid.TotalRowFormatStyle.BackColor = System.Drawing.Color.LightCyan;
                    grid.TotalRowFormatStyle.BackgroundGradientMode = Janus.Windows.GridEX.BackgroundGradientMode.Vertical;
                    grid.TotalRowPosition = Janus.Windows.GridEX.TotalRowPosition.BottomFixed;
                    grid.UpdateMode = Janus.Windows.GridEX.UpdateMode.CellUpdate;
                    grid.VisualStyle = Janus.Windows.GridEX.VisualStyle.Office2010;
                    grid.CardHeaders = false;
                    grid.CardWidth = 150;
                    grid.AllowCardSizing = false;
                    grid.RootTable = gridEX1.RootTable;
                    grid.RootTable.Columns["Column02"].WordWrap = true;
                    grid.RootTable.Columns["Column02"].MaxLines = 10000;
                    grid.CardSpacing = 2;
                    grid.ScrollBars = Janus.Windows.GridEX.ScrollBars.Vertical;
                    //foreach (Janus.Windows.GridEX.GridEXColumn item in gridEX3.RootTable.Columns)
                    //{
                    //    grid.RootTable.Columns.Add(item);
                    //}
                    grid.DataSource = clDoc.ReturnTable(ConWare.ConnectionString, @"SELECT [columnid]
                                                                                          ,[column01]
                                                                                          ,[column02]
                                                                                    FROM   table_004_CommodityAndIngredients tcai
                                                                                            
                                                                                    WHERE  tcai.Column51 = 1
                                                                                           AND tcai.column28 = 1
                                                                                           AND tcai.column19 = 1 and  tcai.column04=" + dr["columnid"] + " order by column58");
                    uit.Controls.Add(grid);
                    this.uiTab1.TabPages.Add(uit);
                    grid.SelectionChanged += gridEX1_SelectionChanged;
                    grid.Enter += gridEX1_Enter;
                    grid.Click += gridEX1_Click;
                    grid.MouseClick += gridEX1_MouseClick;


                }

                #endregion


                uiTab1.SelectedIndex = 1;

                gridEX_List.Enabled = false;
                uiTab1.Enabled = false;
                this.WindowState = FormWindowState.Maximized;
                table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                if (storefactor.Rows.Count == 0)
                    throw new Exception("کاربر نامعتبر است");
                if (!Convert.ToBoolean(storefactor.Rows[0]["admin"]) &&
                    (storefactor.Rows[0]["project"] == DBNull.Value ||
                    storefactor.Rows[0]["project"] == null ||
                    string.IsNullOrWhiteSpace(storefactor.Rows[0]["project"].ToString())))
                    throw new Exception("فروشگاه کاربر تعیین نشده است");

                if (!Convert.ToBoolean(storefactor.Rows[0]["admin"]))
                {
                    mlt_Ware.ReadOnly = true;
                    mlt_project.ReadOnly = true;
                 
                    mlt_Stor.ReadOnly = true;

                }
                else
                {
                    mlt_Ware.ReadOnly = false;
                    mlt_project.ReadOnly = false;
                   
                    mlt_Stor.ReadOnly = false;
                }



            }
            catch { }
        }

        private void bt_New_Click(object sender, EventArgs e)
        {
            try
            {




                gridEX_List.Enabled = true;
                uiTab1.Enabled = true;
                gb_factor.Enabled = true;
                dataSet_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, 0);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, 0);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, 0);
                dataSet_Sale.EnforceConstraints = true;
                table_010_SaleFactorBindingSource.AddNew();
                txt_date.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd");
                //string
                if (
                   (storefactor.Rows[0]["project"] != DBNull.Value &&
                   storefactor.Rows[0]["project"] != null &&
                   !string.IsNullOrWhiteSpace(storefactor.Rows[0]["project"].ToString())))
                {
                    mlt_Ware.Value = Convert.ToInt16(storefactor.Rows[0]["ware"]);
                    mlt_project.Value = Convert.ToInt16(storefactor.Rows[0]["project"]);

                }


                if ((storefactor.Rows[0]["Number"] != DBNull.Value && storefactor.Rows[0]["Number"] != null
                    && !string.IsNullOrWhiteSpace(storefactor.Rows[0]["Number"].ToString())))
                {
                    mlt_Stor.Value = Convert.ToInt16(storefactor.Rows[0]["Number"]);
                }


                if (
                  (storefactor.Rows[0]["saleman"] != DBNull.Value &&
                  storefactor.Rows[0]["saleman"] != null &&
                  !string.IsNullOrWhiteSpace(storefactor.Rows[0]["saleman"].ToString())) && Properties.Settings.Default.Saler == "")
                {
                    mlt_PersonSale.Value = Convert.ToInt32(storefactor.Rows[0]["saleman"]);
                }

                if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.Saler))
                {
                    mlt_PersonSale.Value = Properties.Settings.Default.Saler;
                }

                if (
                   (storefactor.Rows[0]["buyer"] != DBNull.Value &&
                   storefactor.Rows[0]["buyer"] != null &&
                   !string.IsNullOrWhiteSpace(storefactor.Rows[0]["buyer"].ToString())) )
                {
                    mlt_Customer.Value = Convert.ToInt32(storefactor.Rows[0]["buyer"]);
                }
                //if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.BayerName))
                //{
                //    mlt_Customer.Value = Properties.Settings.Default.BayerName;
                //}

                if (
                  (storefactor.Rows[0]["saletype"] != DBNull.Value &&
                  storefactor.Rows[0]["saletype"] != null &&
                  !string.IsNullOrWhiteSpace(storefactor.Rows[0]["saletype"].ToString())))
                {
                    mlt_SaleType.Value = Convert.ToInt32(storefactor.Rows[0]["saletype"]);
                }

                //if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.SaleType))
                //{
                //    mlt_SaleType.Value = Properties.Settings.Default.SaleType;
                //}



                btn_addtax.Enabled = true;
                dt_edittime.Value = Class_BasicOperation.ServerDate();
                dt_inserttime.Value = Class_BasicOperation.ServerDate();
                txt_edituser.Text = Class_BasicOperation._UserName;
                txt_insertuser.Text = Class_BasicOperation._UserName;
                bt_New.Enabled = false;
                bt_Save.Enabled = true;
                bt_Del.Enabled = true;
                btn_addtax.Enabled = true;
                gridEX_List.AllowAddNew = InheritableBoolean.True;
                gridEX_List.AllowEdit = InheritableBoolean.True;
                gridEX_List.AllowDelete = InheritableBoolean.True;

             


                if (string.IsNullOrWhiteSpace(txt_desc.Text) && !string.IsNullOrWhiteSpace(Properties.Settings.Default.SaleDescription))

                    txt_desc.Text = Properties.Settings.Default.SaleDescription;


                foreach (Janus.Windows.GridEX.GridEXColumn item in this.gridEX_List.RootTable.Columns)
                {
                    if (item.Key == "column20")
                        item.Selectable = false;
                    if (item.Key == "column11")
                        item.Selectable = false;
                    if (item.Key == "column07")
                        item.Selectable = false;

                }
                if (mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()) && mlt_Ware.Value.ToString().All(char.IsDigit))
                {
                    string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + mlt_project.Value + "),0)");

                    if (controlremain == "True")
                    {
                        GoodbindingSource.DataSource = clGood.MahsoolInfo(Convert.ToInt16(mlt_Ware.Value));
                        DataTable GoodTable = clGood.MahsoolInfo(Convert.ToInt16(mlt_Ware.Value));
                        gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                        gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                    }
                    else
                    {
                        GoodbindingSource.DataSource = clGood.GoodInfo();
                        DataTable GoodTable = clGood.GoodInfo();
                        gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                        gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                    }


                }
                if (mlt_project.Value == null || string.IsNullOrWhiteSpace(mlt_project.Value.ToString()))
                    mlt_project.Select();
                else if (mlt_SaleType.Value == null || string.IsNullOrWhiteSpace(mlt_SaleType.Value.ToString()))
                    mlt_SaleType.Select();
                else if (mlt_Ware.Value == null || string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()))
                    mlt_Ware.Select();
                else if (mlt_Customer.Value == null || string.IsNullOrWhiteSpace(mlt_Customer.Value.ToString()))
                {
                    mlt_Customer.Select();

                }
                else
                {
                    txt_GoodCode.Select();
                    txt_GoodCode_Enter(null, null);
                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private float FirstRemain(int GoodCode, string ware, string date, int? drafid)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                string CommandText = string.Empty;
                if (drafid == null)
                    CommandText = @"  SELECT *
           FROM   (
                      SELECT ISNULL(
                                 (
                                     SELECT SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS 
                                            InValue
                                     FROM   dbo.Table_011_PwhrsReceipt
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt
                                                 ON  dbo.Table_011_PwhrsReceipt.columnid = 
                                                     dbo.Table_012_Child_PwhrsReceipt.column01
                                     WHERE  (dbo.Table_011_PwhrsReceipt.column03 = {0})
                                            AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                                            AND dbo.Table_011_PwhrsReceipt.column02 
                                                <= '{2}'
                                     GROUP BY
                                            dbo.Table_012_Child_PwhrsReceipt.column02
                                 ),
                                 0
                             ) AS InValue,
                             (
                                 SELECT ISNULL(
                                            (
                                                SELECT ISNULL(SUM(dbo.Table_008_Child_PwhrsDraft.column07), 0) AS 
                                                       OutValue
                                                FROM   dbo.Table_007_PwhrsDraft
                                                       INNER JOIN dbo.Table_008_Child_PwhrsDraft
                                                            ON  dbo.Table_007_PwhrsDraft.columnid = 
                                                                dbo.Table_008_Child_PwhrsDraft.column01
                                                WHERE  (dbo.Table_007_PwhrsDraft.column03 = {0})
                                                       AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                                                       AND dbo.Table_007_PwhrsDraft.column02 
                                                           <= '{2}'
                                                GROUP BY
                                                       dbo.Table_008_Child_PwhrsDraft.column02
                                            ),
                                            0
                                        )
                             ) AS OutValue
                  ) AS f
       ) AS j";
                else
                    CommandText = @"SELECT j.InValue -j.OutValue AS Remain
FROM   (
           SELECT *
           FROM   (
                      SELECT ISNULL(
                                 (
                                     SELECT SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS 
                                            InValue
                                     FROM   dbo.Table_011_PwhrsReceipt
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt
                                                 ON  dbo.Table_011_PwhrsReceipt.columnid = 
                                                     dbo.Table_012_Child_PwhrsReceipt.column01
                                     WHERE  (dbo.Table_011_PwhrsReceipt.column03 = {0})
                                            AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                                            AND dbo.Table_011_PwhrsReceipt.column02 
                                                <= '{2}'
                                     GROUP BY
                                            dbo.Table_012_Child_PwhrsReceipt.column02
                                 ),
                                 0
                             ) AS InValue,
                             (
                                 SELECT ISNULL(
                                            (
                                                SELECT ISNULL(SUM(dbo.Table_008_Child_PwhrsDraft.column07), 0) AS 
                                                       OutValue
                                                FROM   dbo.Table_007_PwhrsDraft
                                                       INNER JOIN dbo.Table_008_Child_PwhrsDraft
                                                            ON  dbo.Table_007_PwhrsDraft.columnid = 
                                                                dbo.Table_008_Child_PwhrsDraft.column01
                                                WHERE  (dbo.Table_007_PwhrsDraft.column03 = {0})
                                                       AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                                                            AND dbo.Table_007_PwhrsDraft.column02 
                                                           <= '{2}' and dbo.Table_007_PwhrsDraft.columnid!=" + drafid + @"
                                                
                                                GROUP BY
                                                       dbo.Table_008_Child_PwhrsDraft.column02
                                            ),
                                            0
                                        )
                             ) AS OutValue
                  ) AS f
       ) AS j";
                CommandText = string.Format(CommandText, ware, GoodCode, date);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }



        private void Save_Event(object sender, EventArgs e)
        {

            if (this.table_010_SaleFactorBindingSource.Count > 0 &&
                gridEX_List.AllowEdit == InheritableBoolean.True &&
                mlt_Customer.Value != null && !string.IsNullOrWhiteSpace(mlt_Customer.Value.ToString())
                && mlt_SaleType.Value != null && !string.IsNullOrWhiteSpace(mlt_SaleType.Value.ToString())
                && !string.IsNullOrWhiteSpace(txt_date.Text) && txt_date.IsTextValid()
                && mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()))
            {
                EditDocNum = string.Empty;
                EditDraftNum = string.Empty;
                gridEX_List.UpdateData();
                gridEX_Extra.UpdateData();


                int OldDraftNum = 0;

                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");


                    return;
                }
                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column07"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد", "Warning");


                    return;
                }
                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column10"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با قیمت صفر وجود دارد", "Warning");

                    return;
                }
                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(txt_date.Text.Substring(0, 4)),
                 Convert.ToInt32(txt_date.Text.Substring(5, 2)),
                 Convert.ToInt32(txt_date.Text.Substring(8, 2))))
                {

                    Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");


                    return;

                }
                int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());

                chehckessentioal();
                using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Conacnt.Open();
                    SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + mlt_Ware.Value + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                    if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0)
                        throw new Exception("برای صدور حواله به انبار انتخاب شده دسترسی ندارید");

                }

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(mlt_Ware.Value.ToString()),
                        int.Parse(item.Cells["column02"].Value.ToString())))
                        throw new Exception("کالای " + item.Cells["column02"].Text +
                            " در انبار انتخاب شده فعال نمی باشد");
                    #region check Mojodi
                    if (
                        (
                        ((storefactor.Rows[0]["stock"] == null || string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString()))) &&
                        (
                       (mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) &&
                        clDoc.ExScalar(Properties.Settings.Default.BASE, @" SELECT ISNULL(
                                                                       (select ISNULL(Column08,1) Column08 from Table_295_StoreInfo where column05=" + mlt_project.Value + @"),
                                                                       1
                                                                   )") == "False"

                        ))
                        || ((storefactor.Rows[0]["stock"] != null && !string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString())) && !Convert.ToBoolean(storefactor.Rows[0]["stock"]))

                        )
                    {
                        if (clDoc.IsGood(item.Cells["column02"].Value.ToString()))
                        {
                            float Remain = FirstRemain(int.Parse(item.Cells["column02"].Value.ToString()), mlt_Ware.Value.ToString(), txt_date.Text, DraftId);
                            object sumObject = 0;
                            sumObject = ((DataTable)((System.Data.DataRowView)(table_011_Child1_SaleFactorBindingSource.Current)).DataView.Table).Compute("Sum(column07)", "column02 = " + item.Cells["column02"].Value + " and Column01=" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"]);


                            string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                                "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                item.Cells["column02"].Value.ToString());


                            if (item.Cells["column03"].Value.ToString() != orginalunit)
                            {
                                float h = clDoc.GetZarib(Convert.ToInt32(item.Cells["column02"].Value), Convert.ToInt16(item.Cells["column03"].Value), Convert.ToInt16(orginalunit));
                                sumObject = Convert.ToDouble(sumObject) * h;
                            }



                            bool mojoodimanfi = false;
                            try
                            {
                                using (SqlConnection ConWareGood = new SqlConnection(Properties.Settings.Default.WHRS))
                                {

                                    ConWareGood.Open();
                                    SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                               (
                                                                                   SELECT ISNULL(Column16, 0) AS Column16
                                                                                   FROM   table_004_CommodityAndIngredients
                                                                                   WHERE  ColumnId = " + item.Cells["column02"].Value + @"
                                                                               ),
                                                                               0
                                                                           ) AS Column16", ConWareGood);
                                    mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                                }
                            }
                            catch
                            {
                            }


                            if (Remain < Convert.ToDouble(sumObject) && !mojoodimanfi)
                            {
                                throw new Exception("موجودی کالای " + item.Cells["column02"].Text +
                   " در انبار انتخاب شده کافی نمی باشد");
                            }

                        }
                    }
                    #endregion
                }

                if (txt_num.Value.ToString().StartsWith("-"))
                {
                    txt_num.Value = clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "Column01");
                    //((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column61"] = 0;
                    this.table_010_SaleFactorBindingSource.EndEdit();
                }


                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                DataTable fdt = new DataTable();
                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT isnull(Column53,0) as Column53,isnull(column19,0) as Column19,isnull(column45,0) as Column45 FROM Table_010_SaleFactor where columnid=" + RowID, ConSale);
                Adapter.Fill(fdt);

                if (fdt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(fdt.Rows[0]["Column53"]))
                        throw new Exception("به علت بسته شدن صندوق امکان دخیره اطلاعات وجود ندارد");

                    if (Convert.ToBoolean(fdt.Rows[0]["Column19"]))
                        throw new Exception("به علت ارجاع فاکتور امکان دخیره اطلاعات وجود ندارد");

                    if (Convert.ToBoolean(fdt.Rows[0]["Column45"]))
                        throw new Exception("به علت تسویه فاکتور امکان دخیره اطلاعات وجود ندارد");
                }





                int DocId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID);


                string command = string.Empty;
                Boolean ok = true;
                if (DocId > 0)
                {
                    clDoc.IsFinal_ID(DocId);
                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID;



                    Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + DraftId);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + DraftId;
                    //command += "Update     " + ConSale.Database + ".dbo.Table_010_SaleFactor set  Column10=0 where   columnid=" + RowID;


                }
                if (DraftId > 0)
                {

                    OldDraftNum = Convert.ToInt32(clDoc.ExScalarQuery(Properties.Settings.Default.WHRS, @"SELECT ISNULL(
                                                                                                                   (
                                                                                                                       SELECT column01
                                                                                                                       FROM   Table_007_PwhrsDraft
                                                                                                                       WHERE  columnid = " + DraftId + @"
                                                                                                                   ),
                                                                                                                   0
                                                                                                               ) AS column01"));

                    command += "Delete  from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where column01=" + DraftId;
                    command += "Delete  from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft where   columnid=" + DraftId;

                }



                DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                txt_TotalPrice.Value = Convert.ToDouble(
                gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                AggregateFunction.Sum).ToString());
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());

                double Total = double.Parse(txt_TotalPrice.Value.ToString());

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                {
                    if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                    {
                        item.BeginEdit();
                        item.Cells["Column04"].Value = Convert.ToInt64(Convert.ToDouble(
                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                        AggregateFunction.Sum).ToString()) * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100);
                        item.EndEdit();

                    }

                }
                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
               Convert.ToDouble(txt_Extra.Value.ToString()) -
               Convert.ToDouble(txt_Reductions.Value.ToString());


                if (Convert.ToDouble(txt_EndPrice.Value) < Convert.ToDouble(0))
                {
                    Class_BasicOperation.ShowMsg("", "جمع کل فاکتور منفی شده است", "Warning");
                    return;
                }


                Row["Column15"] = Class_BasicOperation._UserName;
                Row["Column16"] = Class_BasicOperation.ServerDate();
                Row["Column34"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                Row["Column35"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                //****************Calculate Discounts

                double NetTotal = Convert.ToDouble(gridEX_List.GetTotal(
                    gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
                int CustomerCode = int.Parse(Row["Column03"].ToString());
                string Date = Row["Column02"].ToString();
                Row["Column28"] = NetTotal;
                Row["Column30"] = 0;
                Row["Column29"] = 0;
                Row["Column31"] = 0;


                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter2 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                Row["Column32"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                Filter2.Value1 = true;
                Row["Column33"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                if (Convert.ToDouble(txt_EndPrice.Value) <= Convert.ToDouble(0) ||
                   Convert.ToDouble(txt_TotalPrice.Value) <= Convert.ToDouble(0))
                {
                    Class_BasicOperation.ShowMsg("", "امکان صدور سند حسابداری با مبلغ صفر وجود ندارد", "Warning");
                    this.Cursor = Cursors.Default;

                    return;
                }

                if (DraftId > 0 || DocId > 0)
                {

                    Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.ACNT, command);
                    Row["Column09"] = 0; Row["Column10"] = 0;
                }

                dataSet_Sale.EnforceConstraints = false;

                this.table_010_SaleFactorBindingSource.EndEdit();
                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_Sale.Table_011_Child1_SaleFactor);
                this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_Sale.Table_012_Child2_SaleFactor);
                dataSet_Sale.EnforceConstraints = true;



                checksanad();
                string sanadcmd = string.Empty;
                SqlParameter DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
                DraftNum.Direction = ParameterDirection.Output;

                SqlParameter DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                DocNum.Direction = ParameterDirection.Output;
                sanadcmd = " declare @DetialID int declare @draftkey int declare @DocID int set @DraftNum=" + (OldDraftNum > 0 ? OldDraftNum.ToString() : "( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft)") + @"";

                sanadcmd += @" INSERT INTO " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ([column01]
                                                                                               ,[column02]
                                                                                               ,[column03]
                                                                                               ,[column04]
                                                                                               ,[column05]
                                                                                               ,[column06]
                                                                                               ,[column07]
                                                                                               ,[column08]
                                                                                               ,[column09]
                                                                                               ,[column10]
                                                                                               ,[column11]
                                                                                               ,[column12]
                                                                                               ,[column13]
                                                                                               ,[column14]
                                                                                               ,[column15]
                                                                                               ,[column16]
                                                                                               ,[column17]
                                                                                               ,[column18]
                                                                                               ,[column19]
                                                                                               ,[Column20]
                                                                                               ,[Column21]
                                                                                               ,[Column22]
                                                                                               ,[Column23]
                                                                                               ,[Column24]
                                                                                               ,[Column25]
                                                                                               ,[Column26]) VALUES(@DraftNum,'" + txt_date.Text + "'," + mlt_Ware.Value
                             + "," + waredt.Rows[0]["Column02"] + @", " + mlt_Customer.Value + ",N' حواله صادره بابت فاکتور فروش ش" + txt_num.Value +
                             "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + ",0,0,0,0,0,0,0,null,0,0); SET @draftkey=SCOPE_IDENTITY()";

                Adapter = new SqlDataAdapter(
                                                            @"SELECT  [columnid] ,[column01] ,[column02] ,[column03] ,[column04] ,[column05] ,[column06] ,[column07] ,[column08] ,[column09]
                                                                      ,[column10]
                                                                      ,[column11]
                                                                      ,[column12]
                                                                      ,[column13]
                                                                      ,[column14]
                                                                      ,[column15]
                                                                      ,[column16]
                                                                      ,[column17]
                                                                      ,[column18]
                                                                      ,[column19]
                                                                      ,[column20]
                                                                      ,[column21]
                                                                      ,[column22]
                                                                      ,[column23]
                                                                      ,[column24]
                                                                      ,[column25]
                                                                      ,[column26]
                                                                      ,[column27]
                                                                      ,[column28]
                                                                      ,[column29]
                                                                      ,[column30]
                                                                      ,[Column31]
                                                                      ,[Column32]
                                                                      ,[Column33]
                                                                    ,Column34,Column35,Column36,Column37
                                                                  FROM  [dbo].[Table_011_Child1_SaleFactor] WHERE column01 =" +
                                                      ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString(), ConSale);
                DataTable Child1 = new DataTable();
                Adapter.Fill(Child1);
                string salepric = string.Empty;
                foreach (DataRow item1 in Child1.Rows)
                {
                    string Price = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column30 from Table_295_StoreInfo where Columnid ="+mlt_Stor.Value+"),0)");
                    if (Price!="False" )
                    {
                        salepric += " Update " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients set Column34=" + item1["column10"] + " where columnid=" + item1["Column02"];
                        
                    }

                    if (clDoc.IsGood(item1["Column02"].ToString()))
                    {
                        double value = Convert.ToDouble(item1["Column07"]);
                        string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                            "table_004_CommodityAndIngredients", "column07", "ColumnId",
                            item1["Column02"].ToString());


                        if (item1["column03"].ToString() != orginalunit)
                        {
                            float h = clDoc.GetZarib(Convert.ToInt32(item1["Column02"]), Convert.ToInt16(item1["column03"]), Convert.ToInt16(orginalunit));
                            value = value * h;
                        }

                        sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]) VALUES(@draftkey," + item1["Column02"].ToString() + "," + orginalunit
                           + "," + item1["Column04"].ToString() + "," + item1["Column05"].ToString() + "," + value + "," +
                            value + "," + item1["Column08"].ToString() + "," + item1["Column09"].ToString() + "," + item1["Column10"].ToString() + "," +
                            item1["Column11"].ToString() + ",NULL,NULL," + (item1["Column22"].ToString().Trim() == "" ? "NULL" : item1["Column22"].ToString())
                            + ",0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL," +
                            (item1["Column14"].ToString().Trim() == "" ? "NULL" : item1["Column14"].ToString()) + "," +
                            item1["Column15"].ToString() +
                                ",0,0,0,0," + (item1["Column30"].ToString() == "True" ? "1" : "0") + "," +
                                (item1["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column34"].ToString() + "'") + "," +
                                (item1["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column35"].ToString() + "'")
                                + "," + item1["Column31"].ToString()
                                + "," + item1["Column32"].ToString() + "," + item1["Column36"].ToString() + "," + item1["Column37"].ToString() + ")";
                    }
                }
                sanadcmd += "Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column09=@draftkey,column15='" + Class_BasicOperation._UserName + "',column16=getdate() where ColumnId=" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                if (LastDocnum > 0)
                    sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                else
                    sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + txt_date.Text + "',2,0,N'فاکتور فروش','" + Class_BasicOperation._UserName +
               "',getdate()); SET @DocID=SCOPE_IDENTITY()";

                string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());

                sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + mlt_Customer.Value + @", NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @"  ,
                   " + "'فاکتور فروش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL); set @DetialID=SCOPE_IDENTITY() ";


                bool detal = false;
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                                   (
                                                                       SELECT ISNULL(Column02, 'False') AS Column02
                                                                       FROM   Table_030_Setting
                                                                       WHERE  ColumnId = 63
                                                                   ),
                                                                   'False'
                                                               )Column02", Con);
                    detal = Convert.ToBoolean(Comm.ExecuteScalar());

                }
                if (detal)
                    foreach (DataRow citem1 in Child1.Rows)
                    {

                        sanadcmd += @" INSERT INTO Table_075_SanadDetailNotes (Column00,Column01,Column02,Column03,Column04) Values (@DetialID,1,(select Column02 from " + ConWare.Database + ".dbo.table_004_CommodityAndIngredients where ColumnId=" + citem1["Column02"].ToString() + " ) ," +
                            citem1["Column07"].ToString() + "," + citem1["Column10"].ToString() + ")";

                    }


                _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @"  ,
                   " + "N'فاکتور فروش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";



                foreach (DataRow dr in Sanaddt.Rows)
                {
                    if (dr["Kosoorat"] != null &&
                        dr["Kosoorat"].ToString() != string.Empty &&
                        Convert.ToDouble(dr["Kosoorat"]) > Convert.ToDouble(0))
                    {


                        _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @"  ,
                   " + "'تخفیف فاکتور فروش ش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                        _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               " + mlt_Customer.Value + @", NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @"  ,
                   " + "'تخفیف فاکتور فروش ش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                    }

                    if (dr["Ezafat"] != null &&
                      dr["Ezafat"].ToString() != string.Empty &&
                      Convert.ToDouble(dr["Ezafat"]) > Convert.ToDouble(0))
                    {

                        _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + mlt_Customer.Value + @", NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @"  ,
                   " + "'ارزش افزوده فاکتور فروش ش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                        _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @"  ,
                   " + "'ارزش افزوده فاکتور فروش ش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                    }


                }
                sanadcmd += " Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07=@DocID,column10='" + Class_BasicOperation._UserName + "',column11=getdate() where ColumnId=@draftkey";
                sanadcmd += " Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column10=@DocID,column15='" + Class_BasicOperation._UserName + "',column16=getdate() where ColumnId =" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                sanadcmd += salepric;
              
                    //Properties.Settings.Default.TypeSale = mlt_SaleType.Value.ToString();
                    //Properties.Settings.Default.Save();


                Properties.Settings.Default.Saler = mlt_PersonSale.Value.ToString();
                Properties.Settings.Default.Save();

                //Properties.Settings.Default.BayerName = mlt_Customer.Value.ToString();
                //Properties.Settings.Default.Save();


                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();

                    SqlTransaction sqlTran = Con.BeginTransaction();
                    SqlCommand Command = Con.CreateCommand();
                    Command.Transaction = sqlTran;

                    try
                    {
                        Command.CommandText = sanadcmd;
                        Command.Parameters.Add(DocNum);
                        Command.Parameters.Add(DraftNum);
                        Command.ExecuteNonQuery();
                        sqlTran.Commit();
                        #region ارزش گذاری حواله

                        if ((((storefactor.Rows[0]["stock"] == null || string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString())))
                         &&
                         ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) &&
                         clDoc.ExScalar(Properties.Settings.Default.BASE, @" SELECT ISNULL(
                                                                       (select ISNULL(Column08,1) Column08 from Table_295_StoreInfo where column05=" + mlt_project.Value + @"),
                                                                       1
                                                                   )") == "False"

                         ))
                         || ((storefactor.Rows[0]["stock"] != null && !string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString())) && !Convert.ToBoolean(storefactor.Rows[0]["stock"]))

                         )
                            try
                            {
                                string bahas = string.Empty;

                                string did = clDoc.ExScalar(ConWare.ConnectionString,
                                                        "Table_007_PwhrsDraft", "ColumnId", "Column01",
                                                        DraftNum.Value.ToString());

                                string docid = clDoc.ExScalar(ConAcnt.ConnectionString,
                                                        "Table_060_SanadHead", "ColumnId", "Column00",
                                                        DocNum.Value.ToString());

                                SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + did, ConWare);
                                DataTable Table = new DataTable();
                                goodAdapter.Fill(Table);

                                //محاسبه ارزش و ذخیره آن در جدول Child1
                                double value = 0;

                                using (SqlConnection Con1 = new SqlConnection(Properties.Settings.Default.WHRS))
                                {
                                    Con1.Open();

                                    foreach (DataRow item2 in Table.Rows)
                                    {
                                        try
                                        {
                                            if (Class_BasicOperation._WareType)
                                            {
                                                Adapter = new SqlDataAdapter("EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value, Con);
                                                DataTable TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                DataRow[] Row1 = TurnTable.Select("Kind=2 and ID=" + did + " and DetailID=" + item2["Columnid"].ToString());
                                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row1[0]["DsinglePrice"].ToString()), 4)
                                                    + " , Column16=" + Math.Round(double.Parse(Row1[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con1);
                                                UpdateCommand.ExecuteNonQuery();
                                                value += Math.Round(double.Parse(Row1[0]["DTotalPrice"].ToString()), 4);


                                            }

                                            else
                                            {
                                                Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value + ",@Date='" + txt_date.Text + "',@id=" + did + ",@residid=0", ConWare);
                                                DataTable TurnTable = new DataTable();
                                                Adapter.Fill(TurnTable);
                                                SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                              + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con1);
                                                UpdateCommand.ExecuteNonQuery();
                                                value += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4);


                                            }
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                /// <summary>
                                /// /
                                /// در تاریخ 1399/12/26
                                //طبق صحبت با مهندس قرار شد اگه کاربر ادمین به فروشگاهی تعلق نداشت ینی اجازه ثبت فاکتور با موجودی منفی را دارد
                                //همچینن قرار شد در زمان صدور فاکتور فروش سند بهای تمام شده فاکتور ثبت نشود
                                //سطر بهای تمام شده در هنگام بستن صندوق ثبت شود
                                /// </summary>
                                /* if (Class_BasicOperation._FinType)//بهای تمام شده
                                 {
                                     if (value > 0 && Convert.ToInt32(DocNum.Value) > 0)
                                     {
                                         _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column07"].ToString());

                                         bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
           ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                 VALUES(" + docid + @",'" + bahaDT.Rows[0]["Column07"].ToString() + @"',
                             " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                             " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                             " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                             " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                             NUll, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
                " + "'بهای تمام شده فاکتور فروش ش " + txt_num.Value + "'," + Convert.ToInt64(value) + @",0,0,0,-1,26," + did + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                           Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                         _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column13"].ToString());

                                         bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
           ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                 VALUES(" + docid + @",'" + bahaDT.Rows[0]["Column13"].ToString() + @"',
                             " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                             " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                             " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                             " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                             NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
                " + "'بهای تمام شده فاکتور فروش ش " + txt_num.Value + "',0," + Convert.ToInt64(value) + @",0,0,-1,26," + did + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                           Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                                         clDoc.RunSqlCommand(Properties.Settings.Default.ACNT, bahas);



                                     }
                                 }*/
                            }
                            catch
                            {
                            }
                        #endregion

                        EditDocNum = DocNum.Value.ToString();
                        EditDraftNum = DraftNum.Value.ToString();
                        dataSet_Sale.EnforceConstraints = false;
                       
                        this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, Convert.ToInt32(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"]));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, Convert.ToInt32(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"]));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, Convert.ToInt32(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"]));
                        dataSet_Sale.EnforceConstraints = true;
                        table_010_SaleFactorBindingSource_PositionChanged(null, null);

                        bt_New.Enabled = true;


                        Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                          "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره حواله انبار: " + DraftNum.Value, "Information");
                        if (sender == bt_Save)
                        {

                            Tasvieh = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column28 from Table_295_StoreInfo where Column00=" + mlt_Stor.Value + "),0)");
                             Print = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column27 from Table_295_StoreInfo where Column00=" + mlt_Stor.Value + "),0)");

                            if (Tasvieh.ToString() == "True")
                            {
                                if (DialogResult.Yes == MessageBox.Show("آیا مایل به تسویه فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                {
                                    btn_settlement_Click(sender, e);
                                }
                            }

                            if (Print.ToString() == "True")
                            {
                                if (DialogResult.Yes == MessageBox.Show("آیا مایل به چاپ فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                {
                                    #region Print8cmm


                                    string HeaderSelectText = null;
                                    DataTable HeaderTable;
                                    DataTable DetailTable;
                                    HeaderSelectText = @"SELECT     FactorTable.FactorID AS ID,DetailID, FactorTable.Serial, FactorTable.LegalNumber, FactorTable.Date, FactorTable.Responsible, FactorTable.CustomerID, FactorTable.P2Name, FactorTable.Mobile,
                                 FactorTable.P2NationalCode,
                                   FactorTable.P2ECode,FactorTable.Description2,{1}.dbo.table_004_CommodityAndIngredients.Column36 as Ficonsumer ,{1}.dbo.Table_003_InformationProductCash.column04 ,
       FactorTable.GoodNameTotal,
                                   FactorTable.P2SabtCode, FactorTable.P2Address, FactorTable.P2Tel, FactorTable.P2Fax, FactorTable.P2PostalCode, FactorTable.P2Code, FactorTable.GoodCode, 
                                FactorTable.GoodName, FactorTable.Box, FactorTable.BoxPrice, FactorTable.Pack, FactorTable.PackPrice, FactorTable.Number, FactorTable.TotalNumber, 
                                FactorTable.SinglePrice, FactorTable.TotalPrice, FactorTable.DiscountPercent, FactorTable.DiscountPrice, FactorTable.TaxPrice,FactorTable.TotalWeight,FactorTable.TaxPercent, FactorTable.NetPrice, 
                                ISNULL(FactorTable.Ezafat, 0) AS Ezafat, ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat, PersonInfoTable.Column02, FactorTable.NetTotal,FactorTable.Cash,FactorTable.Cart,FactorTable.Etebari,FactorTable.[Check],FactorTable.Bon,FactorTable.VolumeGroup,
                                FactorTable.SpecialGroup, FactorTable.SpecialCustomer, FactorTable.Description, FactorTable.CountUnitName, derivedtbl_1.Groups, '-' AS charPrice, 
                                'SettleInfo' AS SettleInfo, FactorTable.FactorType, FactorTable.NumberInBox, FactorTable.RowDes, FactorTable.Zarib, FactorTable.NumberInBox AS Expr1, 
                                FactorTable.NumberInPack, CityTable.Column02 AS CityName, ProvinceTable.Column01 AS ProvinceName,FactorTable.PayCash,FactorTable.DraftNumber,FactorTable.DocID,FactorTable.Project,FactorTable.BuyerName,FactorTable.SaleType,FactorTable.DocNum
                                FROM         (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
                                FROM         {0}.dbo.Table_060_ProvinceInfo) AS ProvinceTable INNER JOIN
                                (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                                FROM         {0}.dbo.Table_065_CityInfo) AS CityTable ON ProvinceTable.Column00 = CityTable.Column00 RIGHT OUTER JOIN
                                (SELECT     dbo.Table_010_SaleFactor.columnid AS FactorID, dbo.Table_010_SaleFactor.column01 AS Serial, dbo.Table_010_SaleFactor.column37 AS LegalNumber, 
                                dbo.Table_010_SaleFactor.column02 AS Date,isnull(dbo.Table_010_SaleFactor.Column65,'') AS BuyerName,isnull((select column02 from {0}.dbo.Table_002_SalesTypes where columnid=dbo.Table_010_SaleFactor.Column36),'') as SaleType, dbo.Table_010_SaleFactor.column03 AS CustomerID, PersonTable.Column02 AS P2Name,PersonTable.Column19 AS Mobile, 
                                  PersonTable.Column09 AS P2NationalCode,
                                                   PersonTable.Column141 AS P2ECode,
                                                   PersonTable.Column142 AS P2SabtCode, PersonTable.Column01 AS P2Code, PersonTable.Column06 AS P2Address, PersonTable.Column07 AS P2Tel, 
                                PersonTable.Column08 AS P2Fax, PersonTable.Column13 AS P2PostalCode, GoodTable.column01 AS GoodCode, 
                                dbo.Table_011_Child1_SaleFactor.column04 AS Box, dbo.Table_011_Child1_SaleFactor.column08 AS BoxPrice, 
                                dbo.Table_011_Child1_SaleFactor.column05 AS Pack, dbo.Table_011_Child1_SaleFactor.column09 AS PackPrice, 
                                dbo.Table_011_Child1_SaleFactor.column06 AS Number, dbo.Table_011_Child1_SaleFactor.column07 AS TotalNumber, 
                                dbo.Table_011_Child1_SaleFactor.column10 AS SinglePrice, dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice, dbo.Table_011_Child1_SaleFactor.column23 AS Description2,
                                dbo.Table_011_Child1_SaleFactor.column16 AS DiscountPercent, dbo.Table_011_Child1_SaleFactor.column17 AS DiscountPrice, dbo.Table_011_Child1_SaleFactor.column18 as TaxPercent,
                                dbo.Table_011_Child1_SaleFactor.column19 AS TaxPrice,dbo.Table_011_Child1_SaleFactor.column37 as TotalWeight, dbo.Table_011_Child1_SaleFactor.column20 AS NetPrice, GoodTable.column02 AS GoodName,GoodTable.column05 AS GoodNameTotal, 
                                OtherPrice.PlusPrice AS Ezafat, OtherPrice.MinusPrice AS Kosoorat, dbo.Table_010_SaleFactor.column05 AS Responsible, 
                                dbo.Table_010_SaleFactor.Column28 AS NetTotal, dbo.Table_010_SaleFactor.column46 AS Cash,dbo.Table_010_SaleFactor.column47 AS Cart,dbo.Table_010_SaleFactor.column48 AS Etebari,
                                dbo.Table_010_SaleFactor.column52 AS [Check],dbo.Table_010_SaleFactor.column54 AS [Bon],dbo.Table_010_SaleFactor.Column29 AS VolumeGroup, 
                                dbo.Table_010_SaleFactor.Column30 AS SpecialGroup, dbo.Table_010_SaleFactor.Column31 AS SpecialCustomer, 
                                dbo.Table_010_SaleFactor.column06 AS Description, CountUnitTable.Column01 AS CountUnitName, 
                                CASE WHEN Table_010_SaleFactor.Column12 = 0 THEN '***فاکتور ریالی***' ELSE '***فاکتور ارزی***' END AS FactorType, 
                                dbo.Table_011_Child1_SaleFactor.column23 AS RowDes, dbo.Table_011_Child1_SaleFactor.column31 AS NumberInBox, 
                                dbo.Table_011_Child1_SaleFactor.column32 AS NumberInPack, dbo.Table_011_Child1_SaleFactor.Column33 AS Zarib, 
                                PersonTable.Column21 AS ProvinceId, PersonTable.Column22 AS CityId,Table_010_SaleFactor.column21 as PayCash,
                                (Select Column01 from  {1}.dbo.Table_007_PwhrsDraft where Columnid=Table_010_SaleFactor.Column09) as DraftNumber,
                                Table_010_SaleFactor.Column10 as DocId,(select isnull(Column00,0) from " + ConAcnt.Database + @".dbo.Table_060_SanadHead where ColumnId=Table_010_SaleFactor.Column10)  as DocNum,       Project.column02 as Project,dbo.Table_011_Child1_SaleFactor.columnid as DetailID
 

                                FROM         dbo.Table_010_SaleFactor INNER JOIN
                                dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                                (SELECT     ColumnId, Column00, Column01, Column02,Column19, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, 
                                Column11, Column12, Column13, Column21, Column22       ,Column141,
                                                                   Column142
                                FROM         {0}.dbo.Table_045_PersonInfo) AS PersonTable ON dbo.Table_010_SaleFactor.column03 = PersonTable.ColumnId LEFT OUTER JOIN
                                (SELECT     columnid, SUM(PlusPrice) AS PlusPrice, SUM(MinusPrice) AS MinusPrice
                                FROM         (SELECT     Table_010_SaleFactor_2.columnid, SUM(dbo.Table_012_Child2_SaleFactor.column04) AS PlusPrice, 0 AS MinusPrice
                                FROM         dbo.Table_012_Child2_SaleFactor INNER JOIN
                                dbo.Table_010_SaleFactor AS Table_010_SaleFactor_2 ON 
                                dbo.Table_012_Child2_SaleFactor.column01 = Table_010_SaleFactor_2.columnid
                                WHERE     (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                GROUP BY Table_010_SaleFactor_2.columnid, dbo.Table_012_Child2_SaleFactor.column05
                                UNION ALL
                                SELECT     Table_010_SaleFactor_1.columnid, 0 AS PlusPrice, SUM(Table_012_Child2_SaleFactor_1.column04) AS MinusPrice
                                FROM         dbo.Table_012_Child2_SaleFactor AS Table_012_Child2_SaleFactor_1 INNER JOIN
                                dbo.Table_010_SaleFactor AS Table_010_SaleFactor_1 ON 
                                Table_012_Child2_SaleFactor_1.column01 = Table_010_SaleFactor_1.columnid
                                WHERE     (Table_012_Child2_SaleFactor_1.column05 = 1)
                                GROUP BY Table_010_SaleFactor_1.columnid, Table_012_Child2_SaleFactor_1.column05) AS OtherPrice_1
                                GROUP BY columnid) AS OtherPrice ON dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid LEFT OUTER JOIN
                                (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, 
                                column12, column13, column14, column15, column16, column17, column18, column19, column20, column21, column22, column23, 
                                column24, column25, column26, column27, column28, column29, column30, column31
                                FROM         {1}.dbo.table_004_CommodityAndIngredients) AS GoodTable ON 
                                dbo.Table_011_Child1_SaleFactor.column02 = GoodTable.columnid LEFT OUTER JOIN
                                (SELECT     Column00, Column01
                                FROM         {0}.dbo.Table_070_CountUnitInfo) AS CountUnitTable ON dbo.Table_011_Child1_SaleFactor.column03 = CountUnitTable.Column00  LEFT OUTER JOIN (
                                SELECT Column00,
                                       Column02
                                FROM   {0}.dbo.Table_035_ProjectInfo
                            ) AS Project
                            ON  dbo.Table_011_Child1_SaleFactor.column22 = 
                                Project.Column00 ) 
                                AS FactorTable ON CityTable.Column01 = FactorTable.CityId LEFT OUTER JOIN
                                (SELECT     PersonId, Groups
                                FROM         {0}.dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1) AS derivedtbl_1 ON FactorTable.CustomerID = derivedtbl_1.PersonId LEFT OUTER JOIN
                                (SELECT     ColumnId, Column01, Column02, Column21, Column22
                                FROM         {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1) AS PersonInfoTable ON FactorTable.Responsible = PersonInfoTable.ColumnId    LEFT OUTER JOIN
                         {1}.dbo.table_004_CommodityAndIngredients ON FactorTable.GoodCode = {1}.dbo.table_004_CommodityAndIngredients.column01   left outer join 
					   {1}.dbo.Table_003_InformationProductCash ON 
                          {1}.dbo.table_004_CommodityAndIngredients.columnid =  {1}.dbo.Table_003_InformationProductCash.column01";



                                    string DetailSelectText = @"SELECT     dbo.Table_024_Discount.column01 AS Name, dbo.Table_012_Child2_SaleFactor.column04 AS Price, 
                      CASE WHEN Table_024_Discount.column02 = 0 THEN '+' ELSE '-' END AS Type, dbo.Table_012_Child2_SaleFactor.column01 AS Column01 ,
                      dbo.Table_010_SaleFactor.column01 AS HeaderNum, dbo.Table_010_SaleFactor.column02 AS HeaderDate
                      FROM         dbo.Table_012_Child2_SaleFactor INNER JOIN
                      dbo.Table_010_SaleFactor ON dbo.Table_012_Child2_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid LEFT OUTER JOIN
                      dbo.Table_024_Discount ON dbo.Table_012_Child2_SaleFactor.column02 = dbo.Table_024_Discount.columnid";


                                    HeaderSelectText += " WHERE     (FactorTable.Serial = " + txt_num.Text + ")";
                                    DetailSelectText += " WHERE (Table_010_SaleFactor.Column01= " + txt_num.Text + ")";
                                    HeaderSelectText += " ORDER BY  FactorTable.FactorID,FactorTable.DetailID";

                                    HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);
                                    HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
                                    DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);

                                    try
                                    {


                                        StiReport stireport = new StiReport();
                                        stireport.Load("Sales Invoice 80mm.mrt");
                                        stireport.Pages["Page3"].Enabled = true;
                                        stireport.Compile();
                                        stireport.RegData("Rpt_SaleTable", HeaderTable);
                                        stireport.RegData("Rpt_SaleExtra_Table", DetailTable);
                                        //stireport["Image"] = Image.FromStream(stream);
                                        this.Cursor = Cursors.Default;
                                        stireport.Render(false);
                                        stireport.Print(false);

                                    }

                                    catch (Exception ex)
                                    { Class_BasicOperation.CheckExceptionType(ex, this.Name); }




                                    #endregion

                                }
                            }
                            else
                            {
                                if (DialogResult.Yes == MessageBox.Show("آیا مایل به چاپ فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                                {



                                    //List<string> List = new List<string>();

                                    //_05_Sale.Reports.Frm_rpt_Salefactor8 frm = new Reports.Frm_rpt_Salefactor8(List,
                                    //    int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 1);
                                    //frm.Frm_rpt_Salefactor8_Load(null, null);

                                    List<string> List = new List<string>();

                                    _05_Sale.Reports.Form_SaleFactorPrint1 frm = new Reports.Form_SaleFactorPrint1(List,
                                        int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 1);
                                    frm.Form_FactorPrint_Load(null, null);



                                }

                            }
                            bt_New_Click(null, null);

                        }
                    }
                    catch (Exception es)
                    {
                        sqlTran.Rollback();
                        this.Cursor = Cursors.Default;
                        Class_BasicOperation.CheckExceptionType(es, this.Name);
                    }

                    this.Cursor = Cursors.Default;



                }







            }
        }






        //        private void Save_Event_1(object sender, EventArgs e)
        //        {
        //            EditDocNum = string.Empty;
        //            EditDraftNum = string.Empty;
        //            gridEX_List.UpdateData();
        //            gridEX_Extra.UpdateData();
        //            if (this.table_010_SaleFactorBindingSource.Count > 0 &&
        //                gridEX_List.AllowEdit == InheritableBoolean.True &&
        //                mlt_Customer.Value != null && !string.IsNullOrWhiteSpace(mlt_Customer.Value.ToString())
        //                && mlt_SaleType.Value != null && !string.IsNullOrWhiteSpace(mlt_SaleType.Value.ToString())
        //                && !string.IsNullOrWhiteSpace(txt_date.Text) && txt_date.IsTextValid()
        //                && mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()))
        //            {

        //                this.Cursor = Cursors.WaitCursor;
        //                int OldDraftNum = 0;

        //                if (gridEX_List.GetDataRows().Count() == 0)
        //                {
        //                    //Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
        //                    //this.Cursor = Cursors.Default;

        //                    //return;
        //                    throw new Exception("کالایی ثبت نشده است");

        //                }
        //                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column07"], ConditionOperator.Equal, 0, null, -1, 1))
        //                {
        //                    //Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد", "Warning");
        //                    //this.Cursor = Cursors.Default;

        //                    //return;
        //                    throw new Exception("در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد");

        //                }
        //                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column10"], ConditionOperator.Equal, 0, null, -1, 1))
        //                {
        //                    //Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با قیمت صفر وجود دارد", "Warning");
        //                    //this.Cursor = Cursors.Default;

        //                    //return;
        //                    throw new Exception("در میان کالاهای وارد شده کالایی با قیمت صفر وجود دارد");

        //                }
        //                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(txt_date.Text.Substring(0, 4)),
        //                 Convert.ToInt32(txt_date.Text.Substring(5, 2)),
        //                 Convert.ToInt32(txt_date.Text.Substring(8, 2))))
        //                {

        //                    //Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
        //                    //this.Cursor = Cursors.Default;

        //                    //return;
        //                    throw new Exception("تاریخ معتبر نیست");


        //                }
        //                int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());

        //                chehckessentioal();
        //                using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.ACNT))
        //                {
        //                    Conacnt.Open();
        //                    SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
        //                                                                   SELECT *
        //                                                                   FROM   Table_200_UserAccessInfo tuai
        //                                                                   WHERE  tuai.Column03 = 5
        //                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
        //                                                                          AND tuai.Column02 = " + mlt_Ware.Value + @"
        //                                                               )
        //                                                                SELECT 0 AS ok
        //                                                            ELSE
        //                                                                SELECT 1 AS ok", Conacnt);
        //                    if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0)
        //                        throw new Exception("برای صدور حواله به انبار انتخاب شده دسترسی ندارید");

        //                }

        //                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
        //                {
        //                    if (!clGood.IsGoodInWare(Int16.Parse(mlt_Ware.Value.ToString()),
        //                        int.Parse(item.Cells["column02"].Value.ToString())))
        //                        throw new Exception("کالای " + item.Cells["column02"].Text +
        //                            " در انبار انتخاب شده فعال نمی باشد");

        //                    if (
        //                         (
        //                         ((storefactor.Rows[0]["stock"] == null || string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString())))
        //                         &&
        //                         (
        //                        (mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) &&
        //                         clDoc.ExScalar(Properties.Settings.Default.BASE, @" SELECT ISNULL(
        //                                                                       (select ISNULL(Column08,1) Column08 from Table_295_StoreInfo where column05=" + mlt_project.Value + @"),
        //                                                                       1
        //                                                                   )") == "False"

        //                         ))
        //                         || ((storefactor.Rows[0]["stock"] != null && !string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString())) && !Convert.ToBoolean(storefactor.Rows[0]["stock"]))

        //                         )
        //                    {
        //                        if (clDoc.IsGood(item.Cells["column02"].Value.ToString()))
        //                        {
        //                            float Remain = FirstRemain(int.Parse(item.Cells["column02"].Value.ToString()), mlt_Ware.Value.ToString(), txt_date.Text, DraftId);
        //                            object sumObject = 0;

        //                            sumObject = ((DataTable)((System.Data.DataRowView)(table_011_Child1_SaleFactorBindingSource.Current)).DataView.Table).Compute("Sum(column07)", "column02 = " + item.Cells["column02"].Value + " and Column01=" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"]);


        //                            string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
        //                                "table_004_CommodityAndIngredients", "column07", "ColumnId",
        //                                item.Cells["column02"].Value.ToString());


        //                            if (item.Cells["column03"].Value.ToString() != orginalunit)
        //                            {
        //                                float h = clDoc.GetZarib(Convert.ToInt32(item.Cells["column02"].Value), Convert.ToInt16(item.Cells["column03"].Value), Convert.ToInt16(orginalunit));
        //                                sumObject = Convert.ToDouble(sumObject) * h;
        //                            }



        //                            bool mojoodimanfi = false;
        //                            try
        //                            {
        //                                using (SqlConnection ConWareGood = new SqlConnection(Properties.Settings.Default.WHRS))
        //                                {

        //                                    ConWareGood.Open();
        //                                    SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
        //                                                                               (
        //                                                                                   SELECT ISNULL(Column16, 0) AS Column16
        //                                                                                   FROM   table_004_CommodityAndIngredients
        //                                                                                   WHERE  ColumnId = " + item.Cells["column02"].Value + @"
        //                                                                               ),
        //                                                                               0
        //                                                                           ) AS Column16", ConWareGood);
        //                                    mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

        //                                }
        //                            }
        //                            catch
        //                            {
        //                            }


        //                            if (Remain < Convert.ToDouble(sumObject) && !mojoodimanfi)
        //                            {
        //                                throw new Exception("موجودی کالای " + item.Cells["column02"].Text +
        //                   " در انبار انتخاب شده کافی نمی باشد");
        //                            }

        //                        }
        //                    }
        //                }

        //                if (txt_num.Value.ToString().StartsWith("-"))
        //                {
        //                    txt_num.Value = clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "Column01");
        //                    //((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column61"] = 0;
        //                    this.table_010_SaleFactorBindingSource.EndEdit();
        //                }


        //                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
        //                DataTable fdt = new DataTable();
        //                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT isnull(Column53,0) as Column53,isnull(column19,0) as Column19,isnull(column45,0) as Column45 FROM Table_010_SaleFactor where columnid=" + RowID, ConSale);
        //                Adapter.Fill(fdt);

        //                if (fdt.Rows.Count > 0)
        //                {
        //                    if (Convert.ToBoolean(fdt.Rows[0]["Column53"]))
        //                        throw new Exception("به علت بسته شدن صندوق امکان دخیره اطلاعات وجود ندارد");

        //                    if (Convert.ToBoolean(fdt.Rows[0]["Column19"]))
        //                        throw new Exception("به علت ارجاع فاکتور امکان دخیره اطلاعات وجود ندارد");

        //                    if (Convert.ToBoolean(fdt.Rows[0]["Column45"]))
        //                        throw new Exception("به علت تسویه فاکتور امکان دخیره اطلاعات وجود ندارد");
        //                }




        //                DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

        //                int DocId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID);
        //                //int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID);
        //                this.Cursor = Cursors.WaitCursor;


        //                string command = string.Empty;
        //                Boolean ok = true;
        //                if (DocId > 0)
        //                {
        //                    clDoc.IsFinal_ID(DocId);
        //                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID);
        //                    foreach (DataRow item in Table.Rows)
        //                    {
        //                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
        //                    }

        //                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID;



        //                    Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + DraftId);
        //                    foreach (DataRow item in Table.Rows)
        //                    {
        //                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
        //                    }

        //                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + DraftId;
        //                    //command += "Update     " + ConSale.Database + ".dbo.Table_010_SaleFactor set  Column10=0 where   columnid=" + RowID;


        //                }
        //                if (DraftId > 0)
        //                {

        //                    OldDraftNum = Convert.ToInt32(clDoc.ExScalarQuery(Properties.Settings.Default.WHRS, @"SELECT ISNULL(
        //                                                                                                                   (
        //                                                                                                                       SELECT column01
        //                                                                                                                       FROM   Table_007_PwhrsDraft
        //                                                                                                                       WHERE  columnid = " + DraftId + @"
        //                                                                                                                   ),
        //                                                                                                                   0
        //                                                                                                               ) AS column01"));

        //                    command += "Delete  from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where column01=" + DraftId;
        //                    command += "Delete  from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft where   columnid=" + DraftId;
        //                    command += "Update     " + ConSale.Database + ".dbo.Table_010_SaleFactor set  Column09=0  where   columnid=" + RowID;



        //                }





        //                if (!string.IsNullOrWhiteSpace(command))
        //                {
        //                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
        //                    {
        //                        Con.Open();

        //                        SqlTransaction sqlTran = Con.BeginTransaction();
        //                        SqlCommand Command = Con.CreateCommand();
        //                        Command.Transaction = sqlTran;
        //                        try
        //                        {
        //                            Command.CommandText = command;
        //                            Command.ExecuteNonQuery();
        //                            sqlTran.Commit();


        //                        }
        //                        catch (Exception es)
        //                        {
        //                            ok = false;
        //                            sqlTran.Rollback();
        //                            this.Cursor = Cursors.Default;

        //                            Class_BasicOperation.CheckExceptionType(es, this.Name);

        //                        }
        //                    }
        //                }


        //                if (ok)
        //                {
        //                    txt_TotalPrice.Value = Convert.ToDouble(
        //                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
        //                    AggregateFunction.Sum).ToString());
        //                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());

        //                    double Total = double.Parse(txt_TotalPrice.Value.ToString());

        //                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
        //                    {
        //                        if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
        //                        {
        //                            item.BeginEdit();
        //                            item.Cells["Column04"].Value = Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100);
        //                            item.EndEdit();

        //                        }
        //                    }
        //                    Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
        //                    txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
        //                    Filter.Value1 = true;
        //                    txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

        //                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
        //                   Convert.ToDouble(txt_Extra.Value.ToString()) -
        //                   Convert.ToDouble(txt_Reductions.Value.ToString());


        //                    if (Convert.ToDouble(txt_EndPrice.Value) < Convert.ToDouble(0))
        //                    {
        //                        Class_BasicOperation.ShowMsg("", "جمع کل فاکتور منفی شده است", "Warning");
        //                        return;
        //                    }


        //                    Row["Column15"] = Class_BasicOperation._UserName;
        //                    Row["Column16"] = Class_BasicOperation.ServerDate();
        //                    Row["Column34"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
        //                    Row["Column35"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

        //                    //****************Calculate Discounts

        //                    double NetTotal = Convert.ToDouble(gridEX_List.GetTotal(
        //                        gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
        //                    int CustomerCode = int.Parse(Row["Column03"].ToString());
        //                    string Date = Row["Column02"].ToString();
        //                    Row["Column28"] = NetTotal;
        //                    Row["Column30"] = 0;
        //                    Row["Column29"] = 0;
        //                    Row["Column31"] = 0;


        //                    //Extra-Reductions
        //                    Janus.Windows.GridEX.GridEXFilterCondition Filter2 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
        //                    Row["Column32"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
        //                    Filter2.Value1 = true;
        //                    Row["Column33"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
        //                    if (Convert.ToDouble(txt_EndPrice.Value) <= Convert.ToDouble(0) ||
        //                       Convert.ToDouble(txt_TotalPrice.Value) <= Convert.ToDouble(0))
        //                    {
        //                        Class_BasicOperation.ShowMsg("", "امکان صدور سند حسابداری با مبلغ صفر وجود ندارد", "Warning");
        //                        this.Cursor = Cursors.Default;

        //                        return;
        //                    }
        //if(DraftId>0||DocId>0)
        //                    {

        //                        Class_BasicOperation.SqlTransactionMethod(Properties.Settings.Default.ACNT,command);
        //                        Row["Column09"]=0; Row["Column10"]=0;
        //                    }

        //                    dataSet_Sale.EnforceConstraints = false;

        //                    this.table_010_SaleFactorBindingSource.EndEdit();
        //                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
        //                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();
        //                    this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
        //                    this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_Sale.Table_011_Child1_SaleFactor);
        //                    this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_Sale.Table_012_Child2_SaleFactor);

        //  dataSet_Sale.EnforceConstraints = true;
        //                    try
        //                    {

        //                        if (mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()) && mlt_Ware.Value.ToString().All(char.IsDigit))
        //                            Properties.Settings.Default.Ware = (mlt_Ware.Value.ToString());
        //                        Properties.Settings.Default.Save();
        //                        clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + mlt_SaleType.Value + " where ColumnId=54");


        //                    }
        //                    catch
        //                    {
        //                    }
        //                    checksanad();
        //                    string sanadcmd = string.Empty;
        //                    SqlParameter DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
        //                    DraftNum.Direction = ParameterDirection.Output;

        //                    SqlParameter DocNum = new SqlParameter("DocNum", SqlDbType.Int);
        //                    DocNum.Direction = ParameterDirection.Output;
        //                    sanadcmd = " declare @DetialID int declare @draftkey int declare @DocID int set @DraftNum=" + (OldDraftNum > 0 ? OldDraftNum.ToString() : "( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft)") + @"";

        //                    sanadcmd += @" INSERT INTO " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ([column01]
        //                                                                                               ,[column02]
        //                                                                                               ,[column03]
        //                                                                                               ,[column04]
        //                                                                                               ,[column05]
        //                                                                                               ,[column06]
        //                                                                                               ,[column07]
        //                                                                                               ,[column08]
        //                                                                                               ,[column09]
        //                                                                                               ,[column10]
        //                                                                                               ,[column11]
        //                                                                                               ,[column12]
        //                                                                                               ,[column13]
        //                                                                                               ,[column14]
        //                                                                                               ,[column15]
        //                                                                                               ,[column16]
        //                                                                                               ,[column17]
        //                                                                                               ,[column18]
        //                                                                                               ,[column19]
        //                                                                                               ,[Column20]
        //                                                                                               ,[Column21]
        //                                                                                               ,[Column22]
        //                                                                                               ,[Column23]
        //                                                                                               ,[Column24]
        //                                                                                               ,[Column25]
        //                                                                                               ,[Column26]) VALUES(@DraftNum,'" + txt_date.Text + "'," + mlt_Ware.Value
        //                                 + "," + waredt.Rows[0]["Column02"] + @", " + mlt_Customer.Value + ",'" + "حواله صادره بابت فاکتور فروش ش" + txt_num.Value +
        //                                 "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + ",0,0,0,0,0,0,0,null,0,0); SET @draftkey=SCOPE_IDENTITY()";

        //                    Adapter = new SqlDataAdapter(
        //                                                                @"SELECT  [columnid] ,[column01] ,[column02] ,[column03] ,[column04] ,[column05] ,[column06] ,[column07] ,[column08] ,[column09]
        //                                                                      ,[column10]
        //                                                                      ,[column11]
        //                                                                      ,[column12]
        //                                                                      ,[column13]
        //                                                                      ,[column14]
        //                                                                      ,[column15]
        //                                                                      ,[column16]
        //                                                                      ,[column17]
        //                                                                      ,[column18]
        //                                                                      ,[column19]
        //                                                                      ,[column20]
        //                                                                      ,[column21]
        //                                                                      ,[column22]
        //                                                                      ,[column23]
        //                                                                      ,[column24]
        //                                                                      ,[column25]
        //                                                                      ,[column26]
        //                                                                      ,[column27]
        //                                                                      ,[column28]
        //                                                                      ,[column29]
        //                                                                      ,[column30]
        //                                                                      ,[Column31]
        //                                                                      ,[Column32]
        //                                                                      ,[Column33]
        //                                                                    ,Column34,Column35,Column36,Column37
        //                                                                  FROM  [dbo].[Table_011_Child1_SaleFactor] WHERE column01 =" +
        //                                                          ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString(), ConSale);
        //                    DataTable Child1 = new DataTable();
        //                    Adapter.Fill(Child1);
        //                    string salepric = string.Empty;
        //                    foreach (DataRow item1 in Child1.Rows)
        //                    {
        //                        //salepric += " Update " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients set Column34=" + item1["column10"] + " where columnid=" + item1["Column02"];

        //                        if (clDoc.IsGood(item1["Column02"].ToString()))
        //                        {
        //                            double value = Convert.ToDouble(item1["Column07"]);
        //                            string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
        //                                "table_004_CommodityAndIngredients", "column07", "ColumnId",
        //                                item1["Column02"].ToString());


        //                            if (item1["column03"].ToString() != orginalunit)
        //                            {
        //                                float h = clDoc.GetZarib(Convert.ToInt32(item1["Column02"]), Convert.ToInt16(item1["column03"]), Convert.ToInt16(orginalunit));
        //                                value = value * h;
        //                            }

        //                            sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft ([column01]
        //           ,[column02]
        //           ,[column03]
        //           ,[column04]
        //           ,[column05]
        //           ,[column06]
        //           ,[column07]
        //           ,[column08]
        //           ,[column09]
        //           ,[column10]
        //           ,[column11]
        //           ,[column12]
        //           ,[column13]
        //           ,[column14]
        //           ,[column15]
        //           ,[column16]
        //           ,[column17]
        //           ,[column18]
        //           ,[column19]
        //           ,[column20]
        //           ,[column21]
        //           ,[column22]
        //           ,[column23]
        //           ,[column24]
        //           ,[column25]
        //           ,[column26]
        //           ,[column27]
        //           ,[column28]
        //           ,[column29]
        //           ,[Column30]
        //           ,[Column31]
        //           ,[Column32]
        //           ,[Column33]
        //           ,[Column34]
        //           ,[Column35]) VALUES(@draftkey," + item1["Column02"].ToString() + "," + orginalunit
        //                               + "," + item1["Column04"].ToString() + "," + item1["Column05"].ToString() + "," + value + "," +
        //                                value + "," + item1["Column08"].ToString() + "," + item1["Column09"].ToString() + "," + item1["Column10"].ToString() + "," +
        //                                item1["Column11"].ToString() + ",NULL,NULL," + (item1["Column22"].ToString().Trim() == "" ? "NULL" : item1["Column22"].ToString())
        //                                + ",0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL," +
        //                                (item1["Column14"].ToString().Trim() == "" ? "NULL" : item1["Column14"].ToString()) + "," +
        //                                item1["Column15"].ToString() +
        //                                    ",0,0,0,0," + (item1["Column30"].ToString() == "True" ? "1" : "0") + "," +
        //                                    (item1["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column34"].ToString() + "'") + "," +
        //                                    (item1["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column35"].ToString() + "'")
        //                                    + "," + item1["Column31"].ToString()
        //                                    + "," + item1["Column32"].ToString() + "," + item1["Column36"].ToString() + "," + item1["Column37"].ToString() + ")";
        //                        }
        //                    }
        //                    sanadcmd += "Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column09=@draftkey,column15='" + Class_BasicOperation._UserName + "',column16=getdate() where ColumnId=" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

        //                    if (LastDocnum > 0)
        //                        sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
        //                    else
        //                        sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
        //                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + txt_date.Text + "',2,0,N'فاکتور فروش','" + Class_BasicOperation._UserName +
        //                   "',getdate()); SET @DocID=SCOPE_IDENTITY()";

        //                    string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());

        //                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
        //              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
        //                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
        //                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
        //                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
        //                                " + mlt_Customer.Value + @", NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
        //                   " + "'فاکتور فروش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
        //                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL); set @DetialID=SCOPE_IDENTITY() ";


        //                    bool detal = false;
        //                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
        //                    {
        //                        Con.Open();
        //                        SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
        //                                                                   (
        //                                                                       SELECT ISNULL(Column02, 'False') AS Column02
        //                                                                       FROM   Table_030_Setting
        //                                                                       WHERE  ColumnId = 63
        //                                                                   ),
        //                                                                   'False'
        //                                                               )Column02", Con);
        //                        detal = Convert.ToBoolean(Comm.ExecuteScalar());

        //                    }
        //                    if (detal)
        //                        foreach (DataRow citem1 in Child1.Rows)
        //                        {

        //                            sanadcmd += @" INSERT INTO Table_075_SanadDetailNotes (Column00,Column01,Column02,Column03,Column04) Values (@DetialID,1,(select Column02 from " + ConWare.Database + ".dbo.table_004_CommodityAndIngredients where ColumnId=" + citem1["Column02"].ToString() + " ) ," +
        //                                citem1["Column07"].ToString() + "," + citem1["Column10"].ToString() + ")";

        //                        }


        //                    _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

        //                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
        //              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
        //                    VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
        //                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
        //                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
        //                                NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
        //                   " + "'فاکتور فروش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
        //                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";



        //                    foreach (DataRow dr in Sanaddt.Rows)
        //                    {
        //                        if (dr["Kosoorat"] != null &&
        //                            dr["Kosoorat"].ToString() != string.Empty &&
        //                            Convert.ToDouble(dr["Kosoorat"]) > Convert.ToDouble(0))
        //                        {


        //                            _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

        //                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
        //              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
        //                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
        //                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
        //                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
        //                                NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
        //                   " + "'تخفیف فاکتور فروش ش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
        //                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


        //                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

        //                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
        //              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
        //                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
        //                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
        //                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
        //                               " + mlt_Customer.Value + @", NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
        //                   " + "'تخفیف فاکتور فروش ش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
        //                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


        //                        }

        //                        if (dr["Ezafat"] != null &&
        //                          dr["Ezafat"].ToString() != string.Empty &&
        //                          Convert.ToDouble(dr["Ezafat"]) > Convert.ToDouble(0))
        //                        {

        //                            _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

        //                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
        //              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
        //                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
        //                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
        //                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
        //                                " + mlt_Customer.Value + @", NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
        //                   " + "'ارزش افزوده فاکتور فروش ش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
        //                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


        //                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

        //                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
        //              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
        //                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
        //                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
        //                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
        //                                NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
        //                   " + "'ارزش افزوده فاکتور فروش ش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
        //                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



        //                        }


        //                    }
        //                    sanadcmd += " Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07=@DocID,column10='" + Class_BasicOperation._UserName + "',column11=getdate() where ColumnId=@draftkey";
        //                    sanadcmd += " Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column10=@DocID,column15='" + Class_BasicOperation._UserName + "',column16=getdate() where ColumnId =" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
        //                    sanadcmd += salepric;
        //                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
        //                    {
        //                        Con.Open();

        //                        SqlTransaction sqlTran = Con.BeginTransaction();
        //                        SqlCommand Command = Con.CreateCommand();
        //                        Command.Transaction = sqlTran;

        //                        try
        //                        {
        //                            Command.CommandText = sanadcmd;
        //                            Command.Parameters.Add(DocNum);
        //                            Command.Parameters.Add(DraftNum);
        //                            Command.ExecuteNonQuery();
        //                            sqlTran.Commit();
        //                            EditDocNum = DocNum.Value.ToString();
        //                            EditDraftNum = DraftNum.Value.ToString();

        //                            if (
        //                        (
        //                        ((storefactor.Rows[0]["stock"] == null || string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString())))
        //                        &&
        //                        (
        //                       (mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) &&
        //                        clDoc.ExScalar(Properties.Settings.Default.BASE, @" SELECT ISNULL(
        //                                                                       (select ISNULL(Column08,1) Column08 from Table_295_StoreInfo where column05=" + mlt_project.Value + @"),
        //                                                                       1
        //                                                                   )") == "False"

        //                        ))
        //                        || ((storefactor.Rows[0]["stock"] != null && !string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString())) && !Convert.ToBoolean(storefactor.Rows[0]["stock"]))

        //                        )
        //                                try
        //                                {
        //                                    string bahas = string.Empty;

        //                                    string did = clDoc.ExScalar(ConWare.ConnectionString,
        //                                                            "Table_007_PwhrsDraft", "ColumnId", "Column01",
        //                                                            DraftNum.Value.ToString());

        //                                    string docid = clDoc.ExScalar(ConAcnt.ConnectionString,
        //                                                            "Table_060_SanadHead", "ColumnId", "Column00",
        //                                                            DocNum.Value.ToString());

        //                                    SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + did, ConWare);
        //                                    DataTable Table = new DataTable();
        //                                    goodAdapter.Fill(Table);

        //                                    //محاسبه ارزش و ذخیره آن در جدول Child1
        //                                    double value = 0;

        //                                    using (SqlConnection Con1 = new SqlConnection(Properties.Settings.Default.WHRS))
        //                                    {
        //                                        Con1.Open();

        //                                        foreach (DataRow item2 in Table.Rows)
        //                                        {
        //                                            try
        //                                            {
        //                                                if (Class_BasicOperation._WareType)
        //                                                {
        //                                                    Adapter = new SqlDataAdapter("EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value, Con);
        //                                                    DataTable TurnTable = new DataTable();
        //                                                    Adapter.Fill(TurnTable);
        //                                                    DataRow[] Row1 = TurnTable.Select("Kind=2 and ID=" + did + " and DetailID=" + item2["Columnid"].ToString());
        //                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row1[0]["DsinglePrice"].ToString()), 4)
        //                                                        + " , Column16=" + Math.Round(double.Parse(Row1[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con1);
        //                                                    UpdateCommand.ExecuteNonQuery();
        //                                                    value += Math.Round(double.Parse(Row1[0]["DTotalPrice"].ToString()), 4);


        //                                                }

        //                                                else
        //                                                {
        //                                                    Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value + ",@Date='" + txt_date.Text + "',@id=" + did + ",@residid=0", ConWare);
        //                                                    DataTable TurnTable = new DataTable();
        //                                                    Adapter.Fill(TurnTable);
        //                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
        //                                                  + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con1);
        //                                                    UpdateCommand.ExecuteNonQuery();
        //                                                    value += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4);


        //                                                }
        //                                            }
        //                                            catch
        //                                            {
        //                                            }
        //                                        }
        //                                    }
        //                                    /// <summary>
        //                                    /// /
        //                                    /// در تاریخ 1399/12/26
        //                                    //طبق صحبت با مهندس قرار شد اگه کاربر ادمین به فروشگاهی تعلق نداشت ینی اجازه ثبت فاکتور با موجودی منفی را دارد
        //                                    //همچینن قرار شد در زمان صدور فاکتور فروش سند بهای تمام شده فاکتور ثبت نشود
        //                                    //سطر بهای تمام شده در هنگام بستن صندوق ثبت شود
        //                                    /// </summary>
        //                                    /* if (Class_BasicOperation._FinType)//بهای تمام شده
        //                                     {
        //                                         if (value > 0 && Convert.ToInt32(DocNum.Value) > 0)
        //                                         {
        //                                             _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column07"].ToString());

        //                                             bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
        //               ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
        //                     VALUES(" + docid + @",'" + bahaDT.Rows[0]["Column07"].ToString() + @"',
        //                                 " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
        //                                 " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
        //                                 " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
        //                                 " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
        //                                 NUll, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
        //                    " + "'بهای تمام شده فاکتور فروش ش " + txt_num.Value + "'," + Convert.ToInt64(value) + @",0,0,0,-1,26," + did + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
        //                                               Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


        //                                             _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column13"].ToString());

        //                                             bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
        //               ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
        //                     VALUES(" + docid + @",'" + bahaDT.Rows[0]["Column13"].ToString() + @"',
        //                                 " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
        //                                 " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
        //                                 " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
        //                                 " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
        //                                 NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
        //                    " + "'بهای تمام شده فاکتور فروش ش " + txt_num.Value + "',0," + Convert.ToInt64(value) + @",0,0,-1,26," + did + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
        //                                               Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

        //                                             clDoc.RunSqlCommand(Properties.Settings.Default.ACNT, bahas);



        //                                         }
        //                                     }*/
        //                                }
        //                                catch
        //                                {
        //                                }


        //                            dataSet_Sale.EnforceConstraints = false;


        //                            this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, Convert.ToInt32(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"]));
        //                            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, Convert.ToInt32(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"]));
        //                            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, Convert.ToInt32(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"]));


        //                            dataSet_Sale.EnforceConstraints = true;
        //                            table_010_SaleFactorBindingSource_PositionChanged(null, null);
        //                            //if (sender == bt_Save || sender == this)
        //                            //    Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
        //                            //      "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره حواله انبار: " + DraftNum.Value, "Information");
        //                            //if (DialogResult.Yes == MessageBox.Show("آیا مایل به چاپ فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
        //                            //{
        //                            //    List<string> List = new List<string>();
        //                            //    //List.Add(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString());
        //                            //    _05_Sale.Reports.Form_SaleFactorPrint1 frm = new Reports.Form_SaleFactorPrint1(List,
        //                            //        int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 1);
        //                            //    frm.Form_FactorPrint_Load(null, null);

        //                            //    //_05_Sale.Reports.Form_SaleFactorPrint1 frm =
        //                            //    //   new Reports.Form_SaleFactorPrint1(int.Parse(txt_num.Value.ToString()), 19);
        //                            //    //frm.ShowDialog();
        //                            //}
        //                            //bt_New_Click(null, null);
        //                            bt_New.Enabled = true;

        //                        }
        //                        catch (Exception es)
        //                        {
        //                            sqlTran.Rollback();
        //                            this.Cursor = Cursors.Default;
        //                            Class_BasicOperation.CheckExceptionType(es, this.Name);
        //                        }

        //                        this.Cursor = Cursors.Default;

        //                    }

        //                    /*  int _ID = int.Parse(Row["ColumnId"].ToString());
        //                      dataSet_Sale.EnforceConstraints = false;
        //                      this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, _ID);
        //                      this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, _ID);
        //                      this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, _ID);
        //                      dataSet_Sale.EnforceConstraints = true;
        //                      table_010_SaleFactorBindingSource_PositionChanged(sender, e);

        //                      bt_New.Enabled = true;*/
        //                    this.Cursor = Cursors.Default;



        //                }
        //                else
        //                {
        //                    Class_BasicOperation.ShowMsg("", "امکان ثبت وجود ندارد یا اطلاعات کامل نیست", "Information");
        //                }
        //            }
        //        }

        public void Save_Event_settlement(object sender, EventArgs e)
        {
            gridEX_List.UpdateData();
            gridEX_Extra.UpdateData();
            if (this.table_010_SaleFactorBindingSource.Count > 0 &&
                gridEX_List.AllowEdit == InheritableBoolean.True &&
                mlt_Customer.Value != null && !string.IsNullOrWhiteSpace(mlt_Customer.Value.ToString())
                && mlt_SaleType.Value != null && !string.IsNullOrWhiteSpace(mlt_SaleType.Value.ToString())
                && !string.IsNullOrWhiteSpace(txt_date.Text) && txt_date.IsTextValid()
                && mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()))
            {

                this.Cursor = Cursors.WaitCursor;
                int OldDraftNum = 0;

                string RowID1 = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                //if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID1) != 0)
                //    throw new Exception("  این فاکتور حواله صادر شده است");

                //if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID1) != 0)
                //    throw new Exception("  این فاکتور سند صادر شده است");
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL( Column45,0) from Table_010_SaleFactor where ColumnId=" + RowID1 + "),0)", Con);
                    if (Convert.ToBoolean(Comm.ExecuteScalar()) == true)

                        throw new Exception("این فاکتور قبلا تسویه شده است");
                }


                if (gridEX_List.GetDataRows().Count() == 0)
                {
                    //Class_BasicOperation.ShowMsg("", "کالایی ثبت نشده است", "Warning");
                    //this.Cursor = Cursors.Default;

                    //return;
                    throw new Exception("کالایی ثبت نشده است");

                }
                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column07"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    //Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد", "Warning");
                    //this.Cursor = Cursors.Default;

                    //return;
                    throw new Exception("در میان کالاهای وارد شده کالایی با تعداد کل صفر وجود دارد");

                }
                if (gridEX_List.Find(gridEX_List.RootTable.Columns["column10"], ConditionOperator.Equal, 0, null, -1, 1))
                {
                    //Class_BasicOperation.ShowMsg("", "در میان کالاهای وارد شده کالایی با قیمت صفر وجود دارد", "Warning");
                    //this.Cursor = Cursors.Default;

                    //return;
                    throw new Exception("در میان کالاهای وارد شده کالایی با قیمت صفر وجود دارد");

                }
                if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(txt_date.Text.Substring(0, 4)),
                 Convert.ToInt32(txt_date.Text.Substring(5, 2)),
                 Convert.ToInt32(txt_date.Text.Substring(8, 2))))
                {

                    //Class_BasicOperation.ShowMsg("", "تاریخ معتبر نیست", "Warning");
                    //this.Cursor = Cursors.Default;

                    //return;
                    throw new Exception("تاریخ معتبر نیست");


                }
                int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());

                chehckessentioal();
                using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Conacnt.Open();
                    SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + mlt_Ware.Value + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                    if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0)
                        throw new Exception("برای صدور حواله به انبار انتخاب شده دسترسی ندارید");

                }

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(mlt_Ware.Value.ToString()),
                        int.Parse(item.Cells["column02"].Value.ToString())))
                        throw new Exception("کالای " + item.Cells["column02"].Text +
                            " در انبار انتخاب شده فعال نمی باشد");
                    if (
                        (
                        ((storefactor.Rows[0]["stock"] == null || string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString())))
                        &&
                        (
                       (mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) &&
                        clDoc.ExScalar(Properties.Settings.Default.BASE, @" SELECT ISNULL(
                                                                       (select ISNULL(Column08,1) Column08 from Table_295_StoreInfo where column05=" + mlt_project.Value + @"),
                                                                       1
                                                                   )") == "False"

                        ))
                        || ((storefactor.Rows[0]["stock"] != null && !string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString())) && !Convert.ToBoolean(storefactor.Rows[0]["stock"]))

                        )
                    {
                        if (clDoc.IsGood(item.Cells["column02"].Value.ToString()))
                        {
                            float Remain = FirstRemain(int.Parse(item.Cells["column02"].Value.ToString()), mlt_Ware.Value.ToString(), txt_date.Text, DraftId);
                            object sumObject = 0;
                            sumObject = ((DataTable)((System.Data.DataRowView)(table_011_Child1_SaleFactorBindingSource.Current)).DataView.Table).Compute("Sum(column07)", "column02 = " + item.Cells["column02"].Value + " and Column01=" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"]);


                            string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                                "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                item.Cells["column02"].Value.ToString());


                            if (item.Cells["column03"].Value.ToString() != orginalunit)
                            {
                                float h = clDoc.GetZarib(Convert.ToInt32(item.Cells["column02"].Value), Convert.ToInt16(item.Cells["column03"].Value), Convert.ToInt16(orginalunit));
                                sumObject = Convert.ToDouble(sumObject) * h;
                            }



                            bool mojoodimanfi = false;
                            try
                            {
                                using (SqlConnection ConWareGood = new SqlConnection(Properties.Settings.Default.WHRS))
                                {

                                    ConWareGood.Open();
                                    SqlCommand Command = new SqlCommand(@"SELECT ISNULL(
                                                                               (
                                                                                   SELECT ISNULL(Column16, 0) AS Column16
                                                                                   FROM   table_004_CommodityAndIngredients
                                                                                   WHERE  ColumnId = " + item.Cells["column02"].Value + @"
                                                                               ),
                                                                               0
                                                                           ) AS Column16", ConWareGood);
                                    mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                                }
                            }
                            catch
                            {
                            }


                            if (Remain < Convert.ToDouble(sumObject) && !mojoodimanfi)
                            {
                                throw new Exception("موجودی کالای " + item.Cells["column02"].Text +
                   " در انبار انتخاب شده کافی نمی باشد");
                            }

                        }
                    }
                }

                if (txt_num.Value.ToString().StartsWith("-"))
                {
                    txt_num.Value = clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "Column01");
                    //((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column61"] = 0;
                    this.table_010_SaleFactorBindingSource.EndEdit();
                }


                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                DataTable fdt = new DataTable();
                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT isnull(Column53,0) as Column53,isnull(column19,0) as Column19,isnull(column45,0) as Column45 FROM Table_010_SaleFactor where columnid=" + RowID, ConSale);
                Adapter.Fill(fdt);

                if (fdt.Rows.Count > 0)
                {
                    if (Convert.ToBoolean(fdt.Rows[0]["Column53"]))
                        throw new Exception("به علت بسته شدن صندوق امکان دخیره اطلاعات وجود ندارد");

                    if (Convert.ToBoolean(fdt.Rows[0]["Column19"]))
                        throw new Exception("به علت ارجاع فاکتور امکان دخیره اطلاعات وجود ندارد");
                    if (Convert.ToBoolean(fdt.Rows[0]["Column45"]))
                        throw new Exception("به علت تسویه فاکتور امکان دخیره اطلاعات وجود ندارد");
                }




                DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                int DocId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID);
                //int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID);
                this.Cursor = Cursors.WaitCursor;


                string command = string.Empty;
                Boolean ok = true;
                if (DocId > 0)
                {
                    clDoc.IsFinal_ID(DocId);
                    DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID;



                    Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + DraftId);
                    foreach (DataRow item in Table.Rows)
                    {
                        command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                    }

                    command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + DraftId;
                    command += "Update     " + ConSale.Database + ".dbo.Table_010_SaleFactor set  Column10=0  where   columnid=" + RowID;


                }
                if (DraftId > 0)
                {

                    OldDraftNum = Convert.ToInt32(clDoc.ExScalarQuery(Properties.Settings.Default.WHRS, @"SELECT ISNULL(
                                                                                                                   (
                                                                                                                       SELECT column01
                                                                                                                       FROM   Table_007_PwhrsDraft
                                                                                                                       WHERE  columnid = " + DraftId + @"
                                                                                                                   ),
                                                                                                                   0
                                                                                                               ) AS column01"));
                    command += "Delete  from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where column01=" + DraftId;
                    command += "Delete  from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft where   columnid=" + DraftId;
                    command += "Update     " + ConSale.Database + ".dbo.Table_010_SaleFactor set  Column09=0   where   columnid=" + RowID;



                }





                if (!string.IsNullOrWhiteSpace(command))
                {
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                    {
                        Con.Open();

                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;
                        try
                        {
                            Command.CommandText = command;
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();


                        }
                        catch (Exception es)
                        {
                            ok = false;
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;

                            Class_BasicOperation.CheckExceptionType(es, this.Name);

                        }
                    }
                }


                if (ok)
                {
                    txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                    AggregateFunction.Sum).ToString());
                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());

                    double Total = double.Parse(txt_TotalPrice.Value.ToString());

                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                    {
                        if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                        {
                            item.BeginEdit();
                            item.Cells["Column04"].Value = Convert.ToInt64(Total * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100);
                            item.EndEdit();

                        }
                    }
                    Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                    txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                    Filter.Value1 = true;
                    txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                   Convert.ToDouble(txt_Extra.Value.ToString()) -
                   Convert.ToDouble(txt_Reductions.Value.ToString());


                    if (Convert.ToDouble(txt_EndPrice.Value) < Convert.ToDouble(0))
                    {
                        //Class_BasicOperation.ShowMsg("", "جمع کل فاکتور منفی شده است", "Warning");
                        //this.Cursor = Cursors.Default;

                        //return;
                        throw new Exception("جمع کل فاکتور منفی شده است");

                    }


                    Row["Column15"] = Class_BasicOperation._UserName;
                    Row["Column16"] = Class_BasicOperation.ServerDate();
                    Row["Column34"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column19"], AggregateFunction.Sum).ToString();
                    Row["Column35"] = gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column17"], AggregateFunction.Sum).ToString();

                    //****************Calculate Discounts

                    double NetTotal = Convert.ToDouble(gridEX_List.GetTotal(
                        gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString());
                    int CustomerCode = int.Parse(Row["Column03"].ToString());
                    string Date = Row["Column02"].ToString();
                    Row["Column28"] = NetTotal;
                    Row["Column30"] = 0;
                    Row["Column29"] = 0;
                    Row["Column31"] = 0;


                    //Extra-Reductions
                    Janus.Windows.GridEX.GridEXFilterCondition Filter2 = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                    Row["Column32"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                    Filter2.Value1 = true;
                    Row["Column33"] = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter2).ToString();
                    if (Convert.ToDouble(txt_EndPrice.Value) <= Convert.ToDouble(0) ||
                       Convert.ToDouble(txt_TotalPrice.Value) <= Convert.ToDouble(0))
                    {
                        //Class_BasicOperation.ShowMsg("", "امکان صدور سند حسابداری با مبلغ صفر وجود ندارد", "Warning");
                        //this.Cursor = Cursors.Default;

                        //return;

                        throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

                    }

                    this.table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                    this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_Sale.Table_011_Child1_SaleFactor);
                    this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_Sale.Table_012_Child2_SaleFactor);
                    try
                    {
                        //if (mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()) && mlt_Ware.Value.ToString().All(char.IsDigit))
                        //    Properties.Settings.Default.Ware = (mlt_Ware.Value.ToString());
                        //Properties.Settings.Default.Save();
                        //clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + mlt_SaleType.Value + " where ColumnId=54");


                    }
                    catch
                    {
                    }
                    checksanad();
                    string sanadcmd = string.Empty;
                    SqlParameter DraftNum = new SqlParameter("DraftNum", SqlDbType.Int);
                    DraftNum.Direction = ParameterDirection.Output;

                    SqlParameter DocNum = new SqlParameter("DocNum", SqlDbType.Int);
                    DocNum.Direction = ParameterDirection.Output;
                    sanadcmd = " declare @DetialID int declare @draftkey int declare @DocID int set @DraftNum=" + (OldDraftNum > 0 ? OldDraftNum.ToString() : "( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft)") + @"";

                    sanadcmd += @" INSERT INTO " + ConWare.Database + @".dbo.Table_007_PwhrsDraft ([column01]
                                                                                               ,[column02]
                                                                                               ,[column03]
                                                                                               ,[column04]
                                                                                               ,[column05]
                                                                                               ,[column06]
                                                                                               ,[column07]
                                                                                               ,[column08]
                                                                                               ,[column09]
                                                                                               ,[column10]
                                                                                               ,[column11]
                                                                                               ,[column12]
                                                                                               ,[column13]
                                                                                               ,[column14]
                                                                                               ,[column15]
                                                                                               ,[column16]
                                                                                               ,[column17]
                                                                                               ,[column18]
                                                                                               ,[column19]
                                                                                               ,[Column20]
                                                                                               ,[Column21]
                                                                                               ,[Column22]
                                                                                               ,[Column23]
                                                                                               ,[Column24]
                                                                                               ,[Column25]
                                                                                               ,[Column26]) VALUES(@DraftNum,'" + txt_date.Text + "'," + mlt_Ware.Value
                                 + "," + waredt.Rows[0]["Column02"] + @", " + mlt_Customer.Value + ",'" + "حواله صادره بابت فاکتور فروش ش" + txt_num.Value +
                                 "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString() + ",0,0,0,0,0,0,0,null,0,0); SET @draftkey=SCOPE_IDENTITY()";

                    Adapter = new SqlDataAdapter(
                                                                @"SELECT  [columnid] ,[column01] ,[column02] ,[column03] ,[column04] ,[column05] ,[column06] ,[column07] ,[column08] ,[column09]
                                                                      ,[column10]
                                                                      ,[column11]
                                                                      ,[column12]
                                                                      ,[column13]
                                                                      ,[column14]
                                                                      ,[column15]
                                                                      ,[column16]
                                                                      ,[column17]
                                                                      ,[column18]
                                                                      ,[column19]
                                                                      ,[column20]
                                                                      ,[column21]
                                                                      ,[column22]
                                                                      ,[column23]
                                                                      ,[column24]
                                                                      ,[column25]
                                                                      ,[column26]
                                                                      ,[column27]
                                                                      ,[column28]
                                                                      ,[column29]
                                                                      ,[column30]
                                                                      ,[Column31]
                                                                      ,[Column32]
                                                                      ,[Column33]
                                                                    ,Column34,Column35,Column36,Column37
                                                                  FROM  [dbo].[Table_011_Child1_SaleFactor] WHERE column01 =" +
                                                          ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString(), ConSale);
                    DataTable Child1 = new DataTable();
                    Adapter.Fill(Child1);
                    string salepric = string.Empty;
                    foreach (DataRow item1 in Child1.Rows)
                    {
                        //salepric += " Update " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients set Column34=" + item1["column10"] + " where columnid=" + item1["Column02"];

                        if (clDoc.IsGood(item1["Column02"].ToString()))
                        {
                            double value = Convert.ToDouble(item1["Column07"]);
                            string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                                "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                item1["Column02"].ToString());


                            if (item1["column03"].ToString() != orginalunit)
                            {
                                float h = clDoc.GetZarib(Convert.ToInt32(item1["Column02"]), Convert.ToInt16(item1["column03"]), Convert.ToInt16(orginalunit));
                                value = value * h;
                            }

                            sanadcmd += @"INSERT INTO " + ConWare.Database + @".dbo.Table_008_Child_PwhrsDraft ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]) VALUES(@draftkey," + item1["Column02"].ToString() + "," + orginalunit
                               + "," + item1["Column04"].ToString() + "," + item1["Column05"].ToString() + "," + value + "," +
                                value + "," + item1["Column08"].ToString() + "," + item1["Column09"].ToString() + "," + item1["Column10"].ToString() + "," +
                                item1["Column11"].ToString() + ",NULL,NULL," + (item1["Column22"].ToString().Trim() == "" ? "NULL" : item1["Column22"].ToString())
                                + ",0,0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),NULL,NULL," +
                                (item1["Column14"].ToString().Trim() == "" ? "NULL" : item1["Column14"].ToString()) + "," +
                                item1["Column15"].ToString() +
                                    ",0,0,0,0," + (item1["Column30"].ToString() == "True" ? "1" : "0") + "," +
                                    (item1["Column34"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column34"].ToString() + "'") + "," +
                                    (item1["Column35"].ToString().Trim() == "" ? "NULL" : "'" + item1["Column35"].ToString() + "'")
                                    + "," + item1["Column31"].ToString()
                                    + "," + item1["Column32"].ToString() + "," + item1["Column36"].ToString() + "," + item1["Column37"].ToString() + ")";
                        }
                    }
                    sanadcmd += "Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column09=@draftkey,column15='" + Class_BasicOperation._UserName + "',column16=getdate() where ColumnId=" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (LastDocnum > 0)
                        sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                    else
                        sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + txt_date.Text + "',2,0,N'فاکتور فروش','" + Class_BasicOperation._UserName +
                   "',getdate()); SET @DocID=SCOPE_IDENTITY()";

                    string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());

                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + mlt_Customer.Value + @", NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
                   " + "'فاکتور فروش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL); set @DetialID=SCOPE_IDENTITY() ";


                    bool detal = false;
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
                    {
                        Con.Open();
                        SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                                   (
                                                                       SELECT ISNULL(Column02, 'False') AS Column02
                                                                       FROM   Table_030_Setting
                                                                       WHERE  ColumnId = 63
                                                                   ),
                                                                   'False'
                                                               )Column02", Con);
                        detal = Convert.ToBoolean(Comm.ExecuteScalar());

                    }
                    if (detal)
                        foreach (DataRow citem1 in Child1.Rows)
                        {

                            sanadcmd += @" INSERT INTO Table_075_SanadDetailNotes (Column00,Column01,Column02,Column03,Column04) Values (@DetialID,1,(select Column02 from " + ConWare.Database + ".dbo.table_004_CommodityAndIngredients where ColumnId=" + citem1["Column02"].ToString() + " ) ," +
                                citem1["Column07"].ToString() + "," + citem1["Column10"].ToString() + ")";

                        }


                    _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                    sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
                   " + "'فاکتور فروش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                      Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";



                    foreach (DataRow dr in Sanaddt.Rows)
                    {
                        if (dr["Kosoorat"] != null &&
                            dr["Kosoorat"].ToString() != string.Empty &&
                            Convert.ToDouble(dr["Kosoorat"]) > Convert.ToDouble(0))
                        {


                            _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
                   " + "'تخفیف فاکتور فروش ش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               " + mlt_Customer.Value + @", NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @"  ,
                   " + "'تخفیف فاکتور فروش ش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                        }

                        if (dr["Ezafat"] != null &&
                          dr["Ezafat"].ToString() != string.Empty &&
                          Convert.ToDouble(dr["Ezafat"]) > Convert.ToDouble(0))
                        {

                            _AccInfo = clDoc.ACC_Info(dr["Bed"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bed"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + mlt_Customer.Value + @", NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @"  ,
                   " + "'ارزش افزوده فاکتور فروش ش " + txt_num.Value + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                            _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                            sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @"  ,
                   " + "'ارزش افزوده فاکتور فروش ش " + txt_num.Value + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                        }


                    }
                    sanadcmd += " Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07=@DocID,column10='" + Class_BasicOperation._UserName + "',column11=getdate() where ColumnId=@draftkey";
                    sanadcmd += " Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column10=@DocID,column15='" + Class_BasicOperation._UserName + "',column16=getdate() where ColumnId =" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    sanadcmd += salepric;
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                    {
                        Con.Open();

                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;

                        try
                        {
                            Command.CommandText = sanadcmd;
                            Command.Parameters.Add(DocNum);
                            Command.Parameters.Add(DraftNum);
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();
                            if (
                       (
                       ((storefactor.Rows[0]["stock"] == null || string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString())))
                       &&
                       (
                      (mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) &&
                       clDoc.ExScalar(Properties.Settings.Default.BASE, @" SELECT ISNULL(
                                                                       (select ISNULL(Column08,1) Column08 from Table_295_StoreInfo where column05=" + mlt_project.Value + @"),
                                                                       1
                                                                   )") == "False"

                       ))
                       || ((storefactor.Rows[0]["stock"] != null && !string.IsNullOrWhiteSpace(storefactor.Rows[0]["stock"].ToString())) && !Convert.ToBoolean(storefactor.Rows[0]["stock"]))

                       )
                                try
                                {
                                    string bahas = string.Empty;

                                    string did = clDoc.ExScalar(ConWare.ConnectionString,
                                                            "Table_007_PwhrsDraft", "ColumnId", "Column01",
                                                            DraftNum.Value.ToString());

                                    string docid = clDoc.ExScalar(ConAcnt.ConnectionString,
                                                            "Table_060_SanadHead", "ColumnId", "Column00",
                                                            DocNum.Value.ToString());

                                    SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + did, ConWare);
                                    DataTable Table = new DataTable();
                                    goodAdapter.Fill(Table);

                                    //محاسبه ارزش و ذخیره آن در جدول Child1
                                    double value = 0;

                                    using (SqlConnection Con1 = new SqlConnection(Properties.Settings.Default.WHRS))
                                    {
                                        Con1.Open();

                                        foreach (DataRow item2 in Table.Rows)
                                        {
                                            try
                                            {
                                                if (Class_BasicOperation._WareType)
                                                {
                                                    Adapter = new SqlDataAdapter("EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value, Con);
                                                    DataTable TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    DataRow[] Row1 = TurnTable.Select("Kind=2 and ID=" + did + " and DetailID=" + item2["Columnid"].ToString());
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row1[0]["DsinglePrice"].ToString()), 4)
                                                        + " , Column16=" + Math.Round(double.Parse(Row1[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con1);
                                                    UpdateCommand.ExecuteNonQuery();
                                                    value += Math.Round(double.Parse(Row1[0]["DTotalPrice"].ToString()), 4);


                                                }

                                                else
                                                {
                                                    Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + mlt_Ware.Value + ",@Date='" + txt_date.Text + "',@id=" + did + ",@residid=0", ConWare);
                                                    DataTable TurnTable = new DataTable();
                                                    Adapter.Fill(TurnTable);
                                                    SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                                  + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con1);
                                                    UpdateCommand.ExecuteNonQuery();
                                                    value += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4);


                                                }
                                            }
                                            catch
                                            {
                                            }
                                        }
                                    }
                                    /// <summary>
                                    /// /
                                    /// در تاریخ 1399/12/26
                                    //طبق صحبت با مهندس قرار شد اگه کاربر ادمین به فروشگاهی تعلق نداشت ینی اجازه ثبت فاکتور با موجودی منفی را دارد
                                    //همچینن قرار شد در زمان صدور فاکتور فروش سند بهای تمام شده فاکتور ثبت نشود
                                    //سطر بهای تمام شده در هنگام بستن صندوق ثبت شود
                                    /// </summary>
                                    /* if (Class_BasicOperation._FinType)//بهای تمام شده
                                     {
                                         if (value > 0 && Convert.ToInt32(DocNum.Value) > 0)
                                         {
                                             _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column07"].ToString());

                                             bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
               ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                     VALUES(" + docid + @",'" + bahaDT.Rows[0]["Column07"].ToString() + @"',
                                 " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                 " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                 " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                 " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                 NUll, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
                    " + "'بهای تمام شده فاکتور فروش ش " + txt_num.Value + "'," + Convert.ToInt64(value) + @",0,0,0,-1,26," + did + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                               Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                             _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column13"].ToString());

                                             bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
               ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                     VALUES(" + docid + @",'" + bahaDT.Rows[0]["Column13"].ToString() + @"',
                                 " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                 " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                 " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                 " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                 NULL, NULL , " + ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString())) ? mlt_project.Value : "NULL") + @" ,
                    " + "'بهای تمام شده فاکتور فروش ش " + txt_num.Value + "',0," + Convert.ToInt64(value) + @",0,0,-1,26," + did + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                               Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                                             clDoc.RunSqlCommand(Properties.Settings.Default.ACNT, bahas);



                                         }
                                     }*/
                                }
                                catch
                                {
                                }

                            //if (sender == bt_Save || sender == this)
                            Class_BasicOperation.ShowMsg("", "عملیات با موفقیت انجام شد" + Environment.NewLine +
                              "شماره سند حسابداری: " + DocNum.Value + Environment.NewLine + "شماره حواله انبار: " + DraftNum.Value, "Information");
                            //if (DialogResult.Yes == MessageBox.Show("آیا مایل به چاپ فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            //{
                            //    List<string> List = new List<string>();
                            //    //List.Add(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString());
                            //    _05_Sale.Reports.Form_SaleFactorPrint1 frm = new Reports.Form_SaleFactorPrint1(List,
                            //        int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 1);
                            //    frm.Form_FactorPrint_Load(null, null);

                            //    //_05_Sale.Reports.Form_SaleFactorPrint1 frm =
                            //    //   new Reports.Form_SaleFactorPrint1(int.Parse(txt_num.Value.ToString()), 19);
                            //    //frm.ShowDialog();
                            //}
                            //bt_New_Click(null, null);
                            table_010_SaleFactorBindingSource_PositionChanged(sender, e);
                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }

                        this.Cursor = Cursors.Default;



                    }





                    /*  int _ID = int.Parse(Row["ColumnId"].ToString());
                      dataSet_Sale.EnforceConstraints = false;
                      this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, _ID);
                      this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, _ID);
                      this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, _ID);
                      dataSet_Sale.EnforceConstraints = true;
                      table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                      bt_New.Enabled = true;*/
                    this.Cursor = Cursors.Default;



                }
                else
                {
                    Class_BasicOperation.ShowMsg("", "امکان ثبت وجود ندارد یا اطلاعات کامل نیست", "Information");
                }
            }
        }

        public void bt_Save_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                Save_Event(sender, e);
                //table_010_SaleFactorBindingSource_PositionChanged(sender, e);
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
        }

        private void CheckGoodsPrice()
        {
            List<string> Codes = new List<string>();
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
            {
                Codes.Add(item.Cells["Column02"].Value.ToString());
            }

            DataTable Table = clDoc.ReturnTable(ConWare.ConnectionString, @"declare @t table(GoodCode int,Date nvarchar(50), Price decimal(18,3));
            insert into @t SELECT     Table_012_Child_PwhrsReceipt.column02,  MAX(Table_011_PwhrsReceipt.column02) AS Date,1
            FROM         Table_012_Child_PwhrsReceipt INNER JOIN
            Table_011_PwhrsReceipt ON Table_012_Child_PwhrsReceipt.column01 = Table_011_PwhrsReceipt.columnid
            where Table_012_Child_PwhrsReceipt.column02 in (" + string.Join(",", Codes.ToArray()) + @")
            GROUP BY Table_012_Child_PwhrsReceipt.column02
            order by Table_012_Child_PwhrsReceipt.column02;
            
            declare @t2 table(codekala2 int , gheymat2 decimal(18,3),date2 nvarchar(50)
            ,UNIQUE (codekala2,gheymat2,date2)
            );

            insert into @t2 SELECT   dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.column10, 
            dbo.Table_011_PwhrsReceipt.column02 AS Date
            FROM         dbo.Table_012_Child_PwhrsReceipt INNER JOIN
            dbo.Table_011_PwhrsReceipt ON dbo.Table_012_Child_PwhrsReceipt.column01 = dbo.Table_011_PwhrsReceipt.columnid
            where Table_012_Child_PwhrsReceipt.column02 in (" + string.Join(",", Codes.ToArray()) + @")
            GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_012_Child_PwhrsReceipt.column10, dbo.Table_011_PwhrsReceipt.column02;
            update @t set Price=gheymat2 from @t2 as main_table where GoodCode=codekala2 and Date=date2; 
            select * from @t");

            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
            {
                DataRow[] Row = Table.Select("GoodCode=" + item.Cells["Column02"].Value.ToString());

                if (Row.Length > 0)
                {
                    if (Properties.Settings.Default.ShowPriceAlert == 2)
                    {
                        if (Convert.ToDouble(Row[0]["Price"].ToString()) > Convert.ToDouble(item.Cells["Column10"].Value.ToString()))
                            throw new Exception("قیمت مندرج برای کالای " + item.Cells["Column02"].Text + Environment.NewLine + " کمتر از آخرین قیمت است" +
                                Environment.NewLine + " آخرین ورود این کالا در تاریخ " + Row[0]["Date"].ToString() + " و با قیمت " +
                                Convert.ToDouble(Row[0]["Price"].ToString()).ToString("#,##0.###") + " صورت گرفته است");
                    }
                    else
                    {
                        if (Convert.ToDouble(Row[0]["Price"].ToString()) > Convert.ToDouble(item.Cells["Column10"].Value.ToString()))
                            Class_BasicOperation.ShowMsg("", "قیمت مندرج برای کالای " + item.Cells["Column02"].Text + Environment.NewLine + " کمتر از آخرین قیمت است" +
                                    Environment.NewLine + " آخرین ورود این کالا در تاریخ " + Row[0]["Date"].ToString() + " و با قیمت " +
                                    Convert.ToDouble(Row[0]["Price"].ToString()).ToString("#,##0.###") + " صورت گرفته است", "Warning");
                    }
                }
            }



        }

        public void bt_Del_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {


                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();


                    DataTable fdt = new DataTable();
                    SqlDataAdapter Adapter = new SqlDataAdapter("SELECT isnull(Column53,0) as Column53,isnull(column19,0) as Column19,isnull(column45,0) as Column45 FROM Table_010_SaleFactor where columnid=" + RowID, ConSale);
                    Adapter.Fill(fdt);

                    if (fdt.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(fdt.Rows[0]["Column53"]))
                            throw new Exception("به علت بسته شدن صندوق امکان حذف اطلاعات وجود ندارد");

                        if (Convert.ToBoolean(fdt.Rows[0]["Column19"]))
                            throw new Exception("به علت ارجاع فاکتور امکان حذف اطلاعات وجود ندارد");
                        if (Convert.ToBoolean(fdt.Rows[0]["Column45"]))
                            throw new Exception("به علت تسویه فاکتور امکان حذف اطلاعات وجود ندارد");
                    }
                    if (!_del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");


                    int DocId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID);
                    int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID);
                    bool ok = true;
                    if (DocId > 0)
                    {
                        if (DialogResult.Yes == MessageBox.Show("در صورت حذف فاکتور، سند حسابداری  و حواله انبار مربوطه نیز حذف خواهند شد" + Environment.NewLine + "آیا مایل به حذف فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
                            ok = true;
                        else
                            ok = false;

                    }
                    if (ok)
                    {
                        string command = string.Empty;
                        if (DocId > 0)
                        {
                            clDoc.IsFinal_ID(DocId);
                            DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=15 and Column17=" + RowID;



                            Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + DraftId);
                            foreach (DataRow item in Table.Rows)
                            {
                                command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                            }

                            command += " Delete  from Table_065_SanadDetail where Column00=" + DocId + " and Column16=26 and Column17=" + DraftId;

                        }
                        if (DraftId > 0)
                        {
                            command += "Delete  from " + ConWare.Database + ".dbo.Table_008_Child_PwhrsDraft where column01=" + DraftId;
                            command += "Delete  from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft where   columnid=" + DraftId;


                        }

                        command += "delete from " + ConSale.Database + ".dbo.Table_012_Child2_SaleFactor where column01=" + RowID;
                        command += "delete from " + ConSale.Database + ".dbo.Table_011_Child1_SaleFactor where column01=" + RowID;
                        command += "delete from " + ConSale.Database + ".dbo.Table_010_SaleFactor where columnid=" + RowID;





                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Con.Open();

                            SqlTransaction sqlTran = Con.BeginTransaction();
                            SqlCommand Command = Con.CreateCommand();
                            Command.Transaction = sqlTran;
                            try
                            {
                                Command.CommandText = command;
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();
                                bt_New.Enabled = true;
                                Class_BasicOperation.ShowMsg("", "حذف با موفقیت صورت گرفت", "Information");
                                dataSet_Sale.EnforceConstraints = false;
                                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, 0);
                                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, 0);
                                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, 0);
                                dataSet_Sale.EnforceConstraints = true;

                            }
                            catch (Exception es)
                            {
                                sqlTran.Rollback();
                                this.Cursor = Cursors.Default;
                                Class_BasicOperation.CheckExceptionType(es, this.Name);
                            }
                            this.Cursor = Cursors.Default;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void gridEX_List_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_List_Enter(object sender, EventArgs e)
        {
            try
            {
                if (table_010_SaleFactorBindingSource.Count > 0)
                {

                    if (txt_date.Text == null || txt_date.Text == "")
                    {
                        MessageBox.Show("اطلاعات تاریخ معتبر نمی باشد");

                        return;
                    }
                    else if (mlt_SaleType.Value == null || mlt_SaleType.Value == DBNull.Value || mlt_SaleType.Text == "" || mlt_SaleType.Text == "0")
                    {
                        MessageBox.Show("اطلاعات نوع فروش معتبر نمی باشد");

                        return;
                    }

                    else if (mlt_Customer.Value == null || mlt_Customer.Value == DBNull.Value || mlt_Customer.Text == "" || mlt_Customer.Text == "0")
                    {
                        MessageBox.Show("اطلاعات خریدار معتبر نمی باشد");

                        return;
                    }
                    //setnull();
                    table_010_SaleFactorBindingSource.EndEdit();
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }


        private void gridEX_List_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        
        
        {
            try
            {
                if (e.Column.Key == "column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column02", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);

                else if (e.Column.Key == "GoodCode")
                    Class_BasicOperation.FilterGridExDropDown(sender, "GoodCode", "GoodCode", "GoodName", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);

            }
            catch { }
            try
            {
                if (e.Column.Key == "column22")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column22", "Column01", "Column02", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }
            try
            
            {
                if (e.Column.Key == "column21")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column21", "Column01", "Column02", gridEX_List.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }

            ((Janus.Windows.GridEX.GridEX)sender).CurrentCellDroppedDown = true;
        }



        private void table_010_SaleFactorBindingSource_PositionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {


                    gridEX_List.Enabled = true;
                    uiTab1.Enabled = true;

                    DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                    if (Convert.ToInt32(Row["ColumnId"]) > 0)
                    {
                        if (Row["Column42"] == DBNull.Value || Row["Column42"] == null || string.IsNullOrWhiteSpace(Row["Column42"].ToString()))
                            MessageBox.Show("انبار فاکتور انتخاب نشده است");
                        bt_New.Enabled = true;
                        DataTable dt = new DataTable();

                        SqlDataAdapter Adapter = new SqlDataAdapter("SELECT isnull(Column09,0) as Column09,isnull(column10,0) as column10,isnull(column19,0) as column19 , isnull(column53,0) as column53, isnull(column45,0) as column45 FROM Table_010_SaleFactor where columnid=" + Row["ColumnId"], ConSale);
                        Adapter.Fill(dt);


                        if (/*dt.Rows[0]["column10"] != "0" || dt.Rows[0]["column09"] != "0" ||*/ Convert.ToBoolean(dt.Rows[0]["column19"]) || Convert.ToBoolean(dt.Rows[0]["column53"]) || Convert.ToBoolean(dt.Rows[0]["column45"]))// مرجوعی
                        {

                            gridEX1.AllowEdit = InheritableBoolean.False;
                            gridEX1.Enabled = true;
                            gridEX_List.AllowAddNew = InheritableBoolean.False;
                            gridEX_List.AllowEdit = InheritableBoolean.False;
                            //gridEX_Extra.AllowAddNew = InheritableBoolean.False;
                            //gridEX_Extra.AllowDelete = InheritableBoolean.False;
                            gridEX_List.AllowDelete = InheritableBoolean.False;
                            btn_addtax.Enabled = false;
                            //bt_Del.Enabled = false;
                            //bt_Save.Enabled = false;
                        }
                        if (/*dt.Rows[0]["column10"].ToString() == "0" && dt.Rows[0]["column09"].ToString() == "0" &&*/ !Convert.ToBoolean(dt.Rows[0]["column19"].ToString()) && !Convert.ToBoolean(dt.Rows[0]["column53"]) && !Convert.ToBoolean(dt.Rows[0]["column45"]))// مرجوعی
                        {
                            gridEX1.AllowEdit = InheritableBoolean.True;
                            gridEX1.Enabled = true;
                            gridEX_List.AllowEdit = InheritableBoolean.True;
                            gridEX_Extra.AllowEdit = InheritableBoolean.True;
                            gridEX_List.AllowAddNew = InheritableBoolean.True;
                            gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                            gridEX_Extra.AllowDelete = InheritableBoolean.True;
                            gridEX_List.AllowDelete = InheritableBoolean.True;
                            btn_addtax.Enabled = true;
                            //bt_Del.Enabled = true;
                            //bt_Save.Enabled = true;
                        }

                    }
                    else
                    {
                        btn_addtax.Enabled = false;

                    }
                }
                else
                {
                    btn_addtax.Enabled = false;
                    gridEX_List.Enabled = false;
                    uiTab1.Enabled = false;

                }

            }
            catch
            { }
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());


                }

            }
            catch
            {
            }
        }

        private void bt_ExportDraft_Click(object sender, EventArgs e)
        {/*
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                try
                {
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 71))
                        throw new Exception("کاربر گرامی شما امکان صدور حواله انبار را ندارید");

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID) != 0)
                        throw new Exception("برای این فاکتور حواله صادر شده است");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                        throw new Exception("به علت باطل شدن این فاکتور امکان صدور حواله وجود ندارد");

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", RowID) != 0)
                        throw new Exception("به علت مرجوع شدن این فاکتور امکان صدور حواله انبار وجود ندارد");

                    if (clDoc.AllService(table_011_Child1_SaleFactorBindingSource))
                        throw new Exception("به علت عدم وجود کالا، حواله ای برای این فاکتور صادر نخواهد شد");

                    Save_Event(sender, e);



                    DataTable Table = new DataTable();
                    Table.Columns.Add("GoodID", Type.GetType("System.String"));
                    Table.Columns.Add("GoodName", Type.GetType("System.String"));
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                    {
                        Table.Rows.Add(item.Cells["Column02"].Value,
                            item.Cells["Column02"].Text);
                    }

                    if (gridEX1.GetRow().Cells["Column42"].Text.Trim() != "" && gridEX1.GetRow().Cells["Column43"].Text.Trim() != "")
                    {

                        _05_Sale.Frm_010_DraftInformationDialog frm = new Frm_010_DraftInformationDialog(this.table_011_Child1_SaleFactorBindingSource, ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current),
                            ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column02"].ToString(),
                            Table, Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column42"]),
                            Convert.ToInt16(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"]));
                        frm.ShowDialog();
                    }
                    else Class_BasicOperation.ShowMsg("", "انبار و نوع حواله را مشخص کنید", "None");

                }

                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                DS.Tables["Draft"].Clear();
                DraftAdapter.Fill(DS, "Draft");
                int _ID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                dataSet_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, _ID);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, _ID);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, _ID);
                dataSet_Sale.EnforceConstraints = true;
                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

            }*/
        }


        private void bt_Export_Click(object sender, EventArgs e)
        {

        }

        private void bt_DelDoc_Click(object sender, EventArgs e)
        {

        }

        private void bt_ReturnFactor_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    this.table_010_SaleFactorBindingSource.EndEdit();
                    gridEX_List.UpdateData();
                    gridEX_Extra.UpdateData();
                    if (this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Added) != null
                    || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Detached) != null
                       ||
                       this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Added) != null
                    || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Detached) != null
                        ||
                       this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Added) != null
                    || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Detached) != null
                       )
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save_Event(sender, e);
                        }
                        else if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                        {
                            return;
                        }
                    }



                    if (!((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                    {
                        string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                        if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 72))
                            throw new Exception("کاربر گرامی شما امکان مرجوع کردن فاکتور فروش را ندارید");



                        if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                        {
                            throw new Exception("خطا در ثبت اطلاعات");

                        }

                        if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", RowID) != 0)
                            throw new Exception("این فاکتور قبلا مرجوع شده است");

                        if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID) == 0)
                            throw new Exception("جهت ارجاع یک فاکتور صدور سند حسابداری و حواله انبار، الزامیست");

                        if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", RowID) != 0)
                            throw new Exception("برای این فاکتور، فاکتور مرجوعی صادر شده است");


                        DataTable fdt = new DataTable();
                        SqlDataAdapter Adapter = new SqlDataAdapter("SELECT  isnull(Column52,0) as Column52 ,isnull(Column53,0) as Column53,isnull(column19,0) as column19 FROM Table_010_SaleFactor where columnid=" + RowID, ConSale);
                        Adapter.Fill(fdt);

                        SqlDataAdapter Adapter1 = new SqlDataAdapter("Select * from Table_030_Setting", ConBase);
                        setting = new DataTable();
                        Adapter1.Fill(setting);



                        if (fdt.Rows.Count > 0)

                            if (Convert.ToBoolean(fdt.Rows[0]["Column53"]))
                                throw new Exception("به علت بسته شدن صندوق امکان مرجوع کردن وجود ندارد");




                        dataSet_Sale.EnforceConstraints = false;
                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                        dataSet_Sale.EnforceConstraints = true;

                        if (fdt.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(fdt.Rows[0]["Column52"]) != 0)
                            {

                                if (setting.Rows[81]["Column02"].ToString() == "-1" || setting.Rows[81]["Column02"].ToString() == "" || setting.Rows[79]["Column02"].ToString() == "-1" || setting.Rows[79]["Column02"].ToString() == "")
                                {
                                    MessageBox.Show("لطفا وضعیت استرداد چک و صندوق دریافت کننده را از قسمت اطلاعات پایه فرم تنظیمات تراکنش، تب تنظیمات فروشگاه تکمیل نمایید");
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                            }

                        }







                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به مرجوع کردن این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            //صدور رسید در صورت صادر شدن حواله برای فاکتور فروش
                            DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                            //ثبت عکس فاکتور فروش
                            InvertDoc(Row);


                            dataSet_Sale.EnforceConstraints = false;
                            this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                            dataSet_Sale.EnforceConstraints = true;
                            this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                            //}
                        }

                    }


                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("این فاکتور حواله صادر شده است") ||
                        ex.Message.Contains("این فاکتور سند صادر شده است") ||
                        ex.Message.Contains("این فاکتور قبلا تسویه شده است"))
                    {

                        if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 72))
                            throw new Exception("کاربر گرامی شما امکان مرجوع کردن فاکتور فروش را ندارید");

                        string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                        if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", RowID) != 0)
                            throw new Exception("این فاکتور قبلا مرجوع شده است");

                        if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID) == 0)
                            throw new Exception("جهت ارجاع یک فاکتور صدور سند حسابداری و حواله انبار، الزامیست");

                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به مرجوع کردن این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            //صدور رسید در صورت صادر شدن حواله برای فاکتور فروش
                            DataRowView Row = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;

                            //    //ثبت عکس فاکتور فروش
                            InvertDoc(Row);
                            Class_BasicOperation.ShowMsg("", "ارجاع فاکتور با موفقیت انجام شد" + Environment.NewLine + "شماره سند حسابداری:" + ReturnDocNum.Value + Environment.NewLine + "شماره رسید انبار:" + ResidNum, "Information");

                            dataSet_Sale.EnforceConstraints = false;
                            this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                            this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                            dataSet_Sale.EnforceConstraints = true;

                            this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                            //}
                        }


                    }
                    else
                        Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
            this.Cursor = Cursors.Default;
        }



        string CommandTxt = string.Empty;

        private void TurnBack(DataRowView Row, string Function)
        {
            if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", Row["ColumnId"].ToString()) != 0)
                throw new Exception("برای این فاکتور، فاکتور مرجوعی صادر شده است");

            //if (!string.IsNullOrEmpty(ReturnDate))
            //{

            //درج هدر مرجوعی
            CommandTxt += (@"

set @ReturnNum=(SELECT ISNULL((SELECT MAX(Column01)  FROM   " + ConSale.Database + @".dbo.Table_018_MarjooiSale ), 0 )) + 1
INSERT INTO " + ConSale.Database + @".dbo.Table_018_MarjooiSale  ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[Column18]
           ,[Column19]
           ,[Column20]
           ,[Column21]
           ,[Column22]
           ,[Column23]
           ,[Column24]
            ,Column30,Column28,Column29  ) VALUES( @ReturnNum ,'" + ReturnDate + "'," + Row["Column03"].ToString() + "," +
                 (Row["Column04"].ToString().Trim() == "" ? "NULL" : "'" + Row["Column04"].ToString().Trim() + "'") + "," +
                 (Row["Column05"].ToString().Trim() == "" ? "NULL" : Row["Column05"].ToString().Trim()) + ",'" + "ارجاع فاکتور فروش ش " + Row["Column01"].ToString() + " تاریخ " + Row["Column02"].ToString() + "'," +
                 (Row["Column07"].ToString().Trim() == "" ? "NULL" : Row["Column07"].ToString().Trim()) + ",0,0,0,0," +
                 (Row["Column12"].ToString() == "True" ? 1 : 0) + ",'" + Class_BasicOperation._UserName
                 + "',Getdate(),'" + Class_BasicOperation._UserName + "',Getdate()," + Row["ColumnId"].ToString() + "," + Row["Column28"].ToString() + "," +
                 Row["Column32"].ToString() + "," + Row["Column33"].ToString() + "," + Row["Column34"].ToString() + "," + Row["Column35"].ToString() +
                 "," + (Row["Column40"].ToString().Trim() == "" ? "NULL" : Row["Column40"].ToString()) + "," +
                  Row["Column41"].ToString() + "," +
                  (Row["Column44"] != null && !string.IsNullOrWhiteSpace(Row["Column44"].ToString()) ? Row["Column44"] : "NULL") + "," +
                  (Row["Column42"] != null && !string.IsNullOrWhiteSpace(Row["Column42"].ToString()) ? Row["Column42"] : "NULL") + "," +
                  Function +
                  "); SET @ReturnId=SCOPE_IDENTITY()");
            //InsertHeader.Parameters.Add(Key);
            //InsertHeader.ExecuteNonQuery();
            //ReturnId = int.Parse(Key.Value.ToString());

            //درج دیتیل1
            foreach (DataRowView item in this.table_011_Child1_SaleFactorBindingSource)
            {
                CommandTxt += (@"INSERT INTO " + ConSale.Database + @".dbo.Table_019_Child1_MarjooiSale ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]) VALUES(@ReturnId," + item["Column02"].ToString() +
                    "," + item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() +
                    "," + item["Column07"].ToString() + "," + item["Column08"].ToString() + "," + item["Column09"].ToString() + "," +
                    item["Column10"].ToString() + "," + item["Column11"].ToString() + ",NULL,NULL," +
                    (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + "," +
                    item["Column15"].ToString() + "," + item["Column16"].ToString() + "," + item["Column17"].ToString() + "," + item["Column18"].ToString() + "," + item["Column19"].ToString() + "," + item["Column20"].ToString() +
                    ",NULL," + (item["Column22"].ToString().Trim() != "" ? item["Column22"].ToString() : "NULL") + ",NULL," + (Row["Column07"].ToString().Trim() != "" ? Row["Column07"].ToString() : "0") + ",0,0,0,0,0," +
                    item["Column31"].ToString() + "," + item["Column32"].ToString() + "," +
                    (item["Column34"].ToString() == "" ? "NULL" : "'" + item["Column34"].ToString() + "'") + "," +
                    (item["Column35"].ToString() == "" ? "NULL" : "'" + item["Column35"].ToString() + "'") + "," + item["Column36"].ToString() + "," + item["Column37"].ToString() + ")");
                //InsertDetail.ExecuteNonQuery();
            }

            //درج دیتیل 2
            foreach (DataRowView item in this.table_012_Child2_SaleFactorBindingSource)
            {
                CommandTxt += "INSERT INTO " + ConSale.Database + @".dbo.Table_020_Child2_MarjooiSale VALUES(@ReturnId," + item["Column02"].ToString()
                      + "," + item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + (item["Column05"].ToString() == "True" ? 1 : 0) + "," +
                      (item["Column06"].ToString().Trim() == "" ? "NULL" : item["Column06"].ToString()) + ")";

            }
            //}
            //}
        }

        private void InsertReceipt(DataRowView Row)
        {
            if (Row["Column09"].ToString() == "0")
                return;


            _05_Sale.Frm_011_ResidInformationDialog frm = new Frm_011_ResidInformationDialog();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {
                string Function = frm.FunctionValue;

                //صدور فاکتور مرجوعی
                TurnBack(Row, Function);


                //DraftTable
                DataTable DraftTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_007_PwhrsDraft where ColumnId=" + Row["Column09"].ToString());
                DataTable DraftChild = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_008_Child_PwhrsDraft where Column01=" + Row["Column09"].ToString());

                ResidNum = clDoc.MaxNumber(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column01");
                //, int.Parse(DraftTable.Rows[0]["Column03"].ToString()));

                //**Resid Header
                SqlParameter key = new SqlParameter("Key", SqlDbType.Int);
                key.Direction = ParameterDirection.Output;
                using (SqlConnection conware = new SqlConnection(Properties.Settings.Default.WHRS))
                {
                    conware.Open();
                    SqlCommand Insert = new SqlCommand(@"INSERT INTO Table_011_PwhrsReceipt (
                                                                            [column01],
                                                                            [column02],
                                                                            [column03],
                                                                            [column04],
                                                                            [column05],
                                                                            [column06],
                                                                            [column07],
                                                                            [column08],
                                                                            [column09],
                                                                            [column10],
                                                                            [column11],
                                                                            [column12],
                                                                            [column13],
                                                                            [column14],
                                                                            [Column15],
                                                                            [Column16],
                                                                            [Column17],
                                                                            [Column18],
                                                                            [Column19],
                                                                            [Column20]
                                                                          )  VALUES (" + ResidNum + ",'" + ReturnDate + "'," +
                     DraftTable.Rows[0]["Column03"].ToString() + "," + Function + "," + DraftTable.Rows[0]["Column05"].ToString() + ",'" + "رسید صادرشده از فاکتور مرجوعی شماره " +
                     ReturnNum + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,0," + ReturnId + ",0," +
                     +(DraftTable.Rows[0]["Column23"].ToString() == "True" ? 1 : 0) + "," +
                            (DraftTable.Rows[0]["Column24"].ToString().Trim() == "" ? "NULL" : DraftTable.Rows[0]["Column24"].ToString()) + "," +
                             DraftTable.Rows[0]["Column25"].ToString()
                     + ",1,null); SET @Key=Scope_Identity()", conware);
                    Insert.Parameters.Add(key);
                    Insert.ExecuteNonQuery();
                    ResidId = int.Parse(key.Value.ToString());

                    //Resid Detail
                    //در هنگام صدور فاکتور مرجوعی فروش اگر شماره فاکتور فروش مشخص بود ارزش کالا در حواله مربوطه خوانده شده
                    // وعینا در رسید مربو به فاکتور مرجوعی فروش درج میگردد
                    foreach (DataRow item in DraftChild.Rows)
                    {
                        SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_012_Child_PwhrsReceipt ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]) VALUES (" + ResidId + "," + item["Column02"].ToString() + "," +
                            item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + ",0 ,0,0,0,NULL," +
                            (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                            + "',getdate(),0," + item["Column15"].ToString() + "," + item["Column16"].ToString() +
                            ",0,NULL,NULL," +
                            (item["Column23"].ToString().Trim() == "" ? "NULL" : item["Column23"].ToString()) + "," +
                            item["Column24"].ToString() + ",0,0,0," +
                            (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column30"].ToString() + "'") + "," +
                            (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column31"].ToString() + "'") + "," +
                            item["Column32"].ToString() + "," + item["Column33"].ToString() + "," + item["Column34"].ToString() + "," +
                            item["Column35"].ToString() + ")", conware);
                        InsertDetail.ExecuteNonQuery();
                    }
                }
                //درج شماره رسید در فاکتور مرجوعی
                clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column09", "ColumnId", ReturnId, ResidId);
            }


        }

        //        private int InvertDoc(DataRowView Row)
        //        {
        //            int i = 0;
        //            if (Row["Column10"].ToString().Trim() == "" || Row["Column10"].ToString() == "0")
        //                return i;

        //            DataTable PreDoc = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select * from Table_065_SanadDetail where Column00=" +
        //                Row["Column10"].ToString() + " and ((Column16=15 and Column17=" + Row["ColumnId"].ToString() +
        //                    ") or (Column16=26 and Column17=" + Row["Column09"].ToString() + ") or (Column16=24 and Column17=" + Row["ColumnId"].ToString() + "))");

        //            if (PreDoc.Rows.Count > 0)
        //            {
        //                //Header
        //                //ReturnDocNum = clDoc.LastDocNum() + 1;
        //                //ReturnDocId = clDoc.ExportDoc_Header(ReturnDocNum, ReturnDate, "فاکتور مرجوعی", Class_BasicOperation._UserName);
        //                string CommandTxt = string.Empty;
        //                CommandTxt = "declare @Key int declare @DetialID int declare @ResidID int declare @TotalValue decimal(18,3) declare @value decimal(18,3)   ";
        //                CommandTxt += @" set @ReturnDocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1  INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
        //                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + ReturnDate + "',2,0,'فاکتور مرجوعی','" + Class_BasicOperation._UserName +
        //                       "',getdate()); SET @Key=SCOPE_IDENTITY()";

        //                //Detail
        //                foreach (DataRow item in PreDoc.Rows)
        //                {
        //                    string[] _AccInfo = clDoc.ACC_Info(item["Column01"].ToString());
        //                    // clDoc.ExportDoc_Detail(ReturnDocId, item["Column01"].ToString(), Int16.Parse(_AccInfo[0].ToString()), _AccInfo[1].ToString(), _AccInfo[2].ToString(), _AccInfo[3].ToString(), _AccInfo[4].ToString()
        //                    // , (item["Column07"].ToString().Trim() == "" ? "NULL" : item["Column07"].ToString()), (item["Column08"].ToString().Trim() == "" ? "NULL" : item["Column08"].ToString()),
        //                    // (item["Column09"].ToString().Trim() == "" ? "NULL" : item["Column09"].ToString()), "مرجوعی-" + item["Column10"].ToString().Trim(),
        //                    //Convert.ToInt64(item["Column12"].ToString()),
        //                    //Convert.ToInt64(item["Column11"].ToString()),
        //                    //Convert.ToDouble(item["Column13"].ToString()),
        //                    //Convert.ToDouble(item["Column14"].ToString()),
        //                    //(item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1))
        //                    //, short.Parse((item["Column16"].ToString() == "26" ? 27 : 17).ToString()), (item["Column16"].ToString() == "26" ? ResidId : ReturnId), Class_BasicOperation._UserName,
        //                    //Convert.ToDouble(item["Column22"].ToString()), (short?)null);

        //                    CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
        //              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
        //              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
        //              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
        //                VALUES (@Key,'" + item["Column01"].ToString() + @"',
        //                               " + Int16.Parse(_AccInfo[0].ToString()) + @",
        //                                '" + _AccInfo[1].ToString() + @"',
        //                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
        //                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
        //                                 " + (string.IsNullOrWhiteSpace(item["Column07"].ToString().Trim()) ? "NULL" : item["Column07"].ToString()) + @",
        //                                " + (string.IsNullOrWhiteSpace(item["Column08"].ToString().Trim()) ? "NULL" : item["Column08"].ToString()) + @",
        //                               " + (string.IsNullOrWhiteSpace(item["Column09"].ToString().Trim()) ? "NULL" : item["Column09"].ToString()) + @",
        //                                ' مرجوعی " + item["Column10"].ToString().Trim() + @"',
        //                                " + Convert.ToInt64(item["Column12"].ToString()) + @",
        //                                " + Convert.ToInt64(item["Column11"].ToString()) + @",
        //                                " + Convert.ToDouble(item["Column13"].ToString()) + @",
        //                                " + Convert.ToDouble(item["Column14"].ToString()) + @",
        //                                " + (item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
        //                                " + short.Parse((item["Column16"].ToString() == "26" ? 27 : 17).ToString()) + @",
        //                                " + (item["Column16"].ToString() == "26" ? ResidId : ReturnId) + @",
        //                                '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
        //                                " + Convert.ToDouble(item["Column22"].ToString()) + @",
        //                                NULL)";

        //                }

        //                //درج شماره سند در فاکتور مرجوعی
        //                //clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_018_MarjooiSale", "Column10", "ColumnId", ReturnId, ReturnDocId);
        //                CommandTxt += " Update " + ConSale.Database + ".dbo.Table_018_MarjooiSale set Column10=@Key where ColumnId=" + ReturnId;

        //                //درج شماره سند در رسید انبار
        //                //DataTable Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select * from Table_065_SanadDetail where Column00=" + ReturnDocId + " and Column16=27");
        //                //if (Table.Rows.Count > 0)
        //                // clDoc.Update_Des_Table(ConWare.ConnectionString, "Table_011_PwhrsReceipt", "Column07", "ColumnId", ResidId, ReturnDocId);
        //                CommandTxt += @" IF (Select count(*) from Table_065_SanadDetail where Column00=@Key and Column16=27) >0 Begin  Update " + ConWare.Database + ".dbo.Table_011_PwhrsReceipt set Column07=@Key where ColumnId=" + ResidId + " END";

        //                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
        //                {
        //                    Con.Open();

        //                    SqlTransaction sqlTran = Con.BeginTransaction();
        //                    SqlCommand Command = Con.CreateCommand();
        //                    SqlParameter ReturnDocNum;
        //                    ReturnDocNum = new SqlParameter("ReturnDocNum", SqlDbType.Int);
        //                    ReturnDocNum.Direction = ParameterDirection.Output;

        //                    Command.Parameters.Add(ReturnDocNum);

        //                    Command.Transaction = sqlTran;

        //                    try
        //                    {
        //                        Command.CommandText = CommandTxt;

        //                        Command.ExecuteNonQuery();
        //                        sqlTran.Commit();
        //                        i = Convert.ToInt32(ReturnDocNum.Value);


        //                    }
        //                    catch (Exception es)
        //                    {
        //                        sqlTran.Rollback();
        //                        this.Cursor = Cursors.Default;
        //                        Class_BasicOperation.CheckExceptionType(es, this.Name);
        //                    }

        //                    this.Cursor = Cursors.Default;



        //                }

        //            }

        //            return i;
        //        }
        /// <summary>
        /// Code By Fakhari1399/12/18
        /// </summary>
        /// <param name="Row"></param>
        /// 
        string cmd = "";
        short sanadtype;
        int RefId;
        int Number = 0;
        string Function = "";

        private void InvertDoc(DataRowView Row)
        {
           

            if (Row["Column09"].ToString() == "0")
                return;


            CommandTxt = @"declare @KeyReturn int 
             declare @checkId int  
             declare @ReturnId int 
            declare @ReturnNum int
            declare @ResidNum int
            declare @Residid int 
            declare @DocNum int 
            declare @Key int 
            declare @DetialID int
            declare  @ReturnDocNum  int
            declare @TotalValue decimal(18,3) declare @value decimal(18,3) ";

            #region صدور رسید

            _05_Sale.Frm_011_ResidInformationDialog frm = new Frm_011_ResidInformationDialog();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
            {

               

                ReturnDate = InputBox.Show("تاریخ ارجاع را وارد کنید:");

                if (!string.IsNullOrEmpty(ReturnDate))
                {
                    //صدور فاکتور مرجوعی


                    if (!Classes.PersianDateTimeUtils.IsValidPersianDate(Convert.ToInt32(ReturnDate.Substring(0, 4)),
               Convert.ToInt32(ReturnDate.Substring(5, 2)),
               Convert.ToInt32(ReturnDate.Substring(8, 2))))
                    {

                        throw new Exception("تاریخ معتبر نیست");


                    }
  Function = frm.FunctionValue;


                    TurnBack(Row, Function);

                    DataTable DraftTable = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_007_PwhrsDraft where ColumnId=" + Row["Column09"].ToString());
                    DataTable DraftChild = clDoc.ReturnTable(ConWare.ConnectionString, "Select * from Table_008_Child_PwhrsDraft where Column01=" + Row["Column09"].ToString());


                    CommandTxt += (@"     set @ResidNum=(SELECT ISNULL((SELECT MAX(Column01)  FROM   " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt ), 0 )) + 1

                                                            INSERT INTO " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt (
                                                                            [column01],
                                                                            [column02],
                                                                            [column03],
                                                                            [column04],
                                                                            [column05],
                                                                            [column06],
                                                                            [column07],
                                                                            [column08],
                                                                            [column09],
                                                                            [column10],
                                                                            [column11],
                                                                            [column12],
                                                                            [column13],
                                                                            [column14],
                                                                            [Column15],
                                                                            [Column16],
                                                                            [Column17],
                                                                            [Column18],
                                                                            [Column19],
                                                                            [Column20]
                                                                          )  VALUES (  @ResidNum ,'" + ReturnDate + "'," +
                      DraftTable.Rows[0]["Column03"].ToString() + "," + Function + "," + DraftTable.Rows[0]["Column05"].ToString() + ",'" + "رسید صادرشده از فاکتور مرجوعی شماره " +
                      ReturnNum + "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,0," + ReturnId + ",0," +
                      +(DraftTable.Rows[0]["Column23"].ToString() == "True" ? 1 : 0) + "," +
                             (DraftTable.Rows[0]["Column24"].ToString().Trim() == "" ? "NULL" : DraftTable.Rows[0]["Column24"].ToString()) + "," +
                              DraftTable.Rows[0]["Column25"].ToString()
                      + ",1,null); SET @Residid=Scope_Identity()");



                    foreach (DataRow item in DraftChild.Rows)
                    {
                        CommandTxt += (@"INSERT INTO " + ConWare.Database + @".dbo.Table_012_Child_PwhrsReceipt ([column01]
           ,[column02]
           ,[column03]
           ,[column04]
           ,[column05]
           ,[column06]
           ,[column07]
           ,[column08]
           ,[column09]
           ,[column10]
           ,[column11]
           ,[column12]
           ,[column13]
           ,[column14]
           ,[column15]
           ,[column16]
           ,[column17]
           ,[column18]
           ,[column19]
           ,[column20]
           ,[column21]
           ,[column22]
           ,[column23]
           ,[column24]
           ,[column25]
           ,[column26]
           ,[column27]
           ,[column28]
           ,[column29]
           ,[Column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]) VALUES (@Residid ," + item["Column02"].ToString() + "," +
                             item["Column03"].ToString() + "," + item["Column04"].ToString() + "," + item["Column05"].ToString() + "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + ",0 ,0,0,0,NULL," +
                             (item["Column13"].ToString().Trim() == "" ? "NULL" : item["Column13"].ToString()) + "," + (item["Column14"].ToString().Trim() == "" ? "NULL" : item["Column14"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName
                             + "',getdate(),0," + item["Column15"].ToString() + "," + item["Column16"].ToString() +
                             ",0,NULL,NULL," +
                             (item["Column23"].ToString().Trim() == "" ? "NULL" : item["Column23"].ToString()) + "," +
                             item["Column24"].ToString() + ",0,0,0," +
                             (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column30"].ToString() + "'") + "," +
                             (item["Column30"].ToString().Trim() == "" ? "NULL" : "'" + item["Column31"].ToString() + "'") + "," +
                             item["Column32"].ToString() + "," + item["Column33"].ToString() + "," + item["Column34"].ToString() + "," +
                             item["Column35"].ToString() + ")");
                        //InsertDetail.ExecuteNonQuery();
                    }



            #endregion

                    #region صدور سند

                    if (Row["Column10"].ToString().Trim() == "" || Row["Column10"].ToString() == "0")
                        return;

                    DataTable PreDoc = clDoc.ReturnTable(ConAcnt.ConnectionString,
                         "Select * from Table_065_SanadDetail where Column00=" +
                         Row["Column10"].ToString() +
                         " and (" +
                         "     (Column16=15 and Column17=" + Row["ColumnId"].ToString() + ") " +
                         "or   (Column16=30 and Column17=" + Row["ColumnId"].ToString() + ") " +
                         "or   (Column16=26 and Column17=" + Row["Column09"].ToString() + ") " +
                         "or   (column16=24 and Column17=" + Row["ColumnId"].ToString() + ") " +
                         "or   (Column17 in (select Columnid from  " + ConBank.Database + ".dbo.Table_065_TurnReception  where Column01=" +
                                                        (Row["Column66"].ToString() == "" ? "0" : Row["Column66"].ToString()) + ") and column27=28)) ");




                    if (PreDoc.Rows.Count > 0)
                    {

                        LastDocnum = LastDocNum();
                        if (LastDocnum > 0)
                            clDoc.IsFinal(LastDocnum);

                        if (LastDocnum > 0)
                            CommandTxt += " set @ReturnDocNum=" + LastDocnum + "  SET @Key=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                        else

                            CommandTxt += @" set @ReturnDocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1 
                INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + ReturnDate + "',2,0,'فاکتور مرجوعی','" + Class_BasicOperation._UserName +
                                   "',getdate()); SET @Key=SCOPE_IDENTITY()";




                        if (Row["Column52"].ToString() != "" || Row["Column52"].ToString() != "0")
                        {

                            foreach (DataRow dt in PreDoc.Rows)
                            {
                                if (dt["Column27"].ToString() == "28")
                                {
                                    Number++;
                                }

                            }

                        }

                        if (Number != 0)
                        {
                            if (setting.Rows[79]["Column02"].ToString() == "-1" || setting.Rows[79]["Column02"].ToString() == "")
                            {
                                Class_BasicOperation.ShowMsg("", "لطفا بانک مورد نظر را از قسمت اطلاعات پایه فرم تنظیمات تراکنش، تب تنظیمات فروشگاه تکمیل نمایید", "Warning");

                                this.Cursor = Cursors.Default;

                            }
                            string ACC_Code = clDoc.ExScalar(ConBank.ConnectionString, @"SELECT        " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.Column12 AS AccCode
                                     FROM           
                       " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo WHERE  " + ConBank.Database + @".dbo.Table_020_BankCashAccInfo.ColumnId = " + setting.Rows[79]["Column02"] + "");


                            string[] _ACCDoc = clDoc.ACC_Info(ACC_Code);


                            CommandTxt += @"
                                  
                    INSERT INTO  " + ConBank.Database + @".dbo.Table_065_TurnReception
                                                 ([Column01]      ,[Column02]         ,[Column04]      ,[Column05]
                                                  ,[Column06]       ,[Column07]     ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
                                                    ,[Column12]      ,[Column13]     ,[Column16]   ,[Column18])  
                                                                
 Values (" + Row["Column66"] + ", " + setting.Rows[81]["Column02"] + " ,'" + ReturnDate + "'," + mlt_Customer.Value + ",'" + setting.Rows[79]["Column02"] + "'," + ACC_Code + "," + Int16.Parse(_ACCDoc[0].ToString()) + ",'" + _ACCDoc[1].ToString() + "'," + (string.IsNullOrEmpty(_ACCDoc[2].ToString()) ? "NULL" : "'" + _ACCDoc[2].ToString() + "'") + "," +
             (string.IsNullOrEmpty(_ACCDoc[3].ToString()) ? "NULL" : "'" + _ACCDoc[3].ToString() + "'") + "," +
             (string.IsNullOrEmpty(_ACCDoc[4].ToString()) ? "NULL" : "'" + _ACCDoc[4].ToString() + "'") + ",@ReturnDocNum,'" + Class_BasicOperation._UserName + "',getdate()); SET @KeyReturn=SCOPE_IDENTITY()";


                        }

                        //Detail
                        foreach (DataRow item in PreDoc.Rows)
                        {
                            string[] _AccInfo = clDoc.ACC_Info(item["Column01"].ToString());






                            if (item["column16"].ToString() == "15")
                            {
                                //RefId = ReturnId;

                                sanadtype = 17;

                                CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
                VALUES (@Key,'" + item["Column01"].ToString() + @"',
                               " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                '" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                 " + (string.IsNullOrWhiteSpace(item["Column07"].ToString().Trim()) ? "NULL" : item["Column07"].ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item["Column08"].ToString().Trim()) ? "NULL" : item["Column08"].ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item["Column09"].ToString().Trim()) ? "NULL" : item["Column09"].ToString()) + @",
                                ' مرجوعی " + item["Column10"].ToString().Trim() + @"',
                                " + Convert.ToInt64(item["Column12"].ToString()) + @",
                                " + Convert.ToInt64(item["Column11"].ToString()) + @",
                                " + Convert.ToDouble(item["Column13"].ToString()) + @",
                                " + Convert.ToDouble(item["Column14"].ToString()) + @",
                                " + (item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
                                " + sanadtype + @",@ReturnId,
                                '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                " + Convert.ToDouble(item["Column22"].ToString()) + @",
                                NULL)";





                            }
                            else if (item["column16"].ToString() == "24")
                            {
                                //RefId = ReturnId;
                                sanadtype = 17;

                                CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
                VALUES (@Key,'" + item["Column01"].ToString() + @"',
                               " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                '" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                 " + (string.IsNullOrWhiteSpace(item["Column07"].ToString().Trim()) ? "NULL" : item["Column07"].ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item["Column08"].ToString().Trim()) ? "NULL" : item["Column08"].ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item["Column09"].ToString().Trim()) ? "NULL" : item["Column09"].ToString()) + @",
                                ' مرجوعی " + item["Column10"].ToString().Trim() + @"',
                                " + Convert.ToInt64(item["Column12"].ToString()) + @",
                                " + Convert.ToInt64(item["Column11"].ToString()) + @",
                                " + Convert.ToDouble(item["Column13"].ToString()) + @",
                                " + Convert.ToDouble(item["Column14"].ToString()) + @",
                                " + (item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
                                " + sanadtype + @", @ReturnId,
                                '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                " + Convert.ToDouble(item["Column22"].ToString()) + @",
                                NULL)";


                            }

                            else if (item["column16"].ToString() == "30")
                            {
                                sanadtype = 29;

                                CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
                VALUES (@Key,'" + item["Column01"].ToString() + @"',
                               " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                '" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                 " + (string.IsNullOrWhiteSpace(item["Column07"].ToString().Trim()) ? "NULL" : item["Column07"].ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item["Column08"].ToString().Trim()) ? "NULL" : item["Column08"].ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item["Column09"].ToString().Trim()) ? "NULL" : item["Column09"].ToString()) + @",
                                ' مرجوعی " + item["Column10"].ToString().Trim() + @"',
                                " + Convert.ToInt64(item["Column12"].ToString()) + @",
                                " + Convert.ToInt64(item["Column11"].ToString()) + @",
                                " + Convert.ToDouble(item["Column13"].ToString()) + @",
                                " + Convert.ToDouble(item["Column14"].ToString()) + @",
                                " + (item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
                                " + sanadtype + @", @ReturnId ,
                                '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                " + Convert.ToDouble(item["Column22"].ToString()) + @",
                                NULL)";
                            }

                            else if (item["column16"].ToString() == "26")
                            {
                                //RefId = ResidId;

                                sanadtype = 27;

                                CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
                VALUES (@Key,'" + item["Column01"].ToString() + @"',
                               " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                '" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                 " + (string.IsNullOrWhiteSpace(item["Column07"].ToString().Trim()) ? "NULL" : item["Column07"].ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item["Column08"].ToString().Trim()) ? "NULL" : item["Column08"].ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item["Column09"].ToString().Trim()) ? "NULL" : item["Column09"].ToString()) + @",
                                ' مرجوعی " + item["Column10"].ToString().Trim() + @"',
                                " + Convert.ToInt64(item["Column12"].ToString()) + @",
                                " + Convert.ToInt64(item["Column11"].ToString()) + @",
                                " + Convert.ToDouble(item["Column13"].ToString()) + @",
                                " + Convert.ToDouble(item["Column14"].ToString()) + @",
                                " + (item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
                                " + sanadtype + @", @ResidId ,
                                '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                " + Convert.ToDouble(item["Column22"].ToString()) + @",
                                NULL)";



                            }
                            else if (item["Column27"].ToString() == "28")
                            {


                                CommandTxt += @"INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]
              ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17]
              ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],Column27) 
                VALUES (@Key,'" + item["Column01"].ToString() + @"',
                               " + Int16.Parse(_AccInfo[0].ToString()) + @",
                                '" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                 " + (string.IsNullOrWhiteSpace(item["Column07"].ToString().Trim()) ? "NULL" : item["Column07"].ToString()) + @",
                                " + (string.IsNullOrWhiteSpace(item["Column08"].ToString().Trim()) ? "NULL" : item["Column08"].ToString()) + @",
                               " + (string.IsNullOrWhiteSpace(item["Column09"].ToString().Trim()) ? "NULL" : item["Column09"].ToString()) + @",
                                ' مرجوعی " + item["Column10"].ToString().Trim() + @"',
                                " + Convert.ToInt64(item["Column12"].ToString()) + @",
                                " + Convert.ToInt64(item["Column11"].ToString()) + @",
                                " + Convert.ToDouble(item["Column13"].ToString()) + @",
                                " + Convert.ToDouble(item["Column14"].ToString()) + @",
                                " + (item["Column15"].ToString().Trim() != "" ? Int16.Parse(item["Column15"].ToString()) : Convert.ToInt16(-1)) + @",
                                " + sanadtype + @", @KeyReturn ,
                                '" + Class_BasicOperation._UserName + @"',getdate(),'" + Class_BasicOperation._UserName + @"',getdate(),
                                " + Convert.ToDouble(item["Column22"].ToString()) + @",
                                29)";


                            }


                        }
                    #endregion
                        //درج شماره سند در فاکتور مرجوعی

                        CommandTxt += @" Update " + ConSale.Database + @".dbo.Table_018_MarjooiSale set Column10=@Key,Column09=@ResidId,Column17=" + Row["ColumnId"].ToString() + @" where ColumnId=@ReturnId ;  

                UPDATE " + ConSale.Database + @".dbo.Table_010_SaleFactor SET Column19=1 , Column20=@ReturnId Where ColumnId=" + Row["ColumnId"].ToString() + @" ;

               Update " + ConWare.Database + @".dbo.Table_011_PwhrsReceipt set Column07=@Key where ColumnId= @ResidId  ";

                        CommandTxt += @"select @ReturnDocNum  , @ResidNum , @ReturnNum ";

                        DataTable dtreturn = Class_BasicOperation.SqlTransactionMethod(ConAcnt.ConnectionString, CommandTxt);


                        Class_BasicOperation.ShowMsg("", "ارجاع فاکتور با موفقیت انجام شد" + Environment.NewLine + "شماره سند حسابداری:" + dtreturn.Rows[0][0] + Environment.NewLine + "شماره رسید انبار:" + dtreturn.Rows[0][1] + Environment.NewLine + "شماره فاکتور مرجوعی :" + dtreturn.Rows[0][2], "Information");

                    }
                }
                this.Cursor = Cursors.Default;

                //this.table_010_SaleFactorBindingSource_PositionChanged(snder, e);
                table_010_SaleFactorBindingSource_PositionChanged(null, null);


            }


        }

        private void Frm_002_Faktor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control && bt_New.Enabled)
            {
                bt_New_Click(sender, e);

            }
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.F)
            {
                txt_Search.Select();
                txt_Search.SelectAll();
            }
            else if (e.Control && e.KeyCode == Keys.T)
            {
                btn_addtax_Click(sender, e);
            }
            else if (e.Control && e.KeyCode == Keys.R)
                bt_AddExtraDiscounts_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.Q)
                btn_ok_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.P)
                ForiFctor_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.O)
                rasmiFactor_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.M)
                bt_Paste_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.L)
                bt_DelDoc_Click_1(sender, e);
            else if (e.Control && e.KeyCode == Keys.F8)

                toolStripButton7.ShowDropDown();


            else if (e.KeyCode == Keys.F12)
            {
                try
                {
                    //setnull();
                    table_010_SaleFactorBindingSource.EndEdit();
                    gridEX_List.Select();
                    gridEX_List.Focus();

                    gridEX_List.Col = 4;
                }
                catch (Exception ex)
                {

                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
            else if (e.Control && e.KeyCode == Keys.G)

                btn_save_Click(sender, e);

        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                {
                    _05_Sale.Reports.Form_SaleFactorPrint frm = new Reports.Form_SaleFactorPrint(
                            int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), false);
                    frm.ShowDialog();
                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
            }
        }

        public void bt_Search_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_Search.Text.Trim()))
            {
                try
                {
                    bt_New.Enabled = true;
                    gridEX_Extra.UpdateData();
                    gridEX_List.UpdateData();
                    this.table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    //if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null || dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                    //{
                    //    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    //    {
                    //        bt_Save_Click(sender, e);
                    //    }
                    //}

                    dataSet_Sale.EnforceConstraints = false;
                    int RowID = ReturnIDNumber(int.Parse(txt_Search.Text));


                    this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, RowID);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, RowID);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, RowID);


                    dataSet_Sale.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                    txt_Search.SelectAll();
                }
            }
        }

        private int ReturnIDNumber(int FactorNum)
        {
            using (SqlConnection con = new SqlConnection(Properties.Settings.Default.SALE))
            {
                con.Open();
                int ID = 0;
                SqlCommand Commnad = new SqlCommand("Select ISNULL(columnid,0) from Table_010_SaleFactor where column01=" + FactorNum + " and (column44=" + projectId + " or '" + (Isadmin) + "'=N'True')", con);
                try
                {
                    ID = int.Parse(Commnad.ExecuteScalar().ToString());
                    return ID;
                }
                catch
                {
                    throw new Exception("شماره فاکتور وارد شده نامعتبر است");
                }
            }
        }

        private void txt_Search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else if (e.KeyChar == 13)
                bt_Search_Click(sender, e);
        }

        private void bt_ViewFactors_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 67))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_008_ViewStoreSaleFactors")
                    {
                        item.BringToFront();
                        return;
                    }
                }
                _05_Sale.Frm_008_ViewStoreSaleFactorsOLD frm = new Frm_008_ViewStoreSaleFactorsOLD();
                try
                {
                    frm.MdiParent = MainForm.ActiveForm;
                }
                catch { }
                frm.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void Frm_002_Faktor_Activated(object sender, EventArgs e)
        {
            txt_Search.Focus();
        }

        private void mnu_Documents_Click(object sender, EventArgs e)
        {
            int SanadId = 0;
            if (this.table_010_SaleFactorBindingSource.Count > 0)
                SanadId = (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() == "" ? 0 : int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString()));
            string sanadnum = clDoc.ExScalar(ConAcnt.ConnectionString, @"select Column00 from Table_060_SanadHead where Columnid=" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"].ToString() + "");

            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;


            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 19))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Form01_AccDocument")
                    {
                        item.BringToFront();
                        TextBox txt_S = (TextBox)item.ActiveControl;
                        txt_S.Text = sanadnum;
                        PACNT._2_DocumentMenu.Form01_AccDocument frms = (PACNT._2_DocumentMenu.Form01_AccDocument)item;
                        frms.bt_Search_Click(sender, e);
                        return;
                    }
                }
                PACNT._2_DocumentMenu.Form01_AccDocument frm = new PACNT._2_DocumentMenu.Form01_AccDocument(
                  UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 20), int.Parse(SanadId.ToString()));
                try
                {
                    frm.MdiParent = MainForm.ActiveForm;
                }
                catch
                {
                }
                frm.Show();
                int SaleId = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                dataSet_Sale.EnforceConstraints = false;


                this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, SaleId);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, SaleId);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, SaleId);


                dataSet_Sale.EnforceConstraints = true;
                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_Drafts_Click(object sender, EventArgs e)
        {
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;

            if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column09"].ToString() == "0" || ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column09"].ToString() == "")
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 26))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Form07_ViewDrafts")
                        {
                            item.BringToFront();
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form07_ViewDrafts frm = new PWHRS._03_AmaliyatAnbar.Form07_ViewDrafts();
                    try
                    {
                        frm.MdiParent = MainForm.ActiveForm;
                    }
                    catch { }
                    frm.Show();
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
            else
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 24))
                {
                    foreach (Form item in Application.OpenForms)
                    {
                        if (item.Name == "Form06_RegisterDrafts")
                        {
                            item.BringToFront();
                            ((PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts)item).txt_Search.Text = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column09"].ToString();
                            ((PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts)item).bt_Search_Click(sender, e);
                            return;
                        }
                    }
                    PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts frm = new PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts(
                        UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 21),
                        int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column09"].ToString()));
                    frm.ShowDialog();
                    int SaleId = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    dataSet_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, SaleId);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, SaleId);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, SaleId);
                    dataSet_Sale.EnforceConstraints = true;
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
        }

        private void mnu_ExtraDiscount_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 45))
            {
                _02_BasicInfo.Frm_002_TakhfifEzafeSale ob = new _02_BasicInfo.Frm_002_TakhfifEzafeSale(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 46));
                ob.ShowDialog();
                SqlDataAdapter Adapter = new SqlDataAdapter("SELECT * FROM Table_024_Discount", ConSale);
                DS.Tables["Discount"].Rows.Clear();
                Adapter.Fill(DS, "Discount");
                gridEX_Extra.DropDowns["Type"].SetDataBinding(DS.Tables["Discount"], "");

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_GoodInformation_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 5))
            {
                _02_BasicInfo.Frm_009_AdditionalGoodsInfo ob = new _02_BasicInfo.Frm_009_AdditionalGoodsInfo(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 6));
                ob.ShowDialog();
                Int16 ware = 0;

                if (
                    mlt_Ware.Value != null &&
                    !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()) &&
                    mlt_Ware.Value.ToString().All(char.IsDigit))
                    ware = Convert.ToInt16(mlt_Ware.Value);
                else if (storefactor.Rows[0]["ware"] != null &&
                 !string.IsNullOrWhiteSpace(storefactor.Rows[0]["ware"].ToString()))
                    ware = Convert.ToInt16(storefactor.Rows[0]["ware"]);
                string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + mlt_project.Value + "),0)");
                if (controlremain == "True")
                {
                    GoodbindingSource.DataSource = clGood.MahsoolInfo(ware);
                    DataTable GoodTable = clGood.MahsoolInfo(ware);
                    gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                    gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                }
                else
                {
                    GoodbindingSource.DataSource = clGood.GoodInfo();
                    DataTable GoodTable = clGood.GoodInfo();
                    gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                    gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                }


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");


        }

        private void mnu_Customers_Click(object sender, EventArgs e)
        {
            PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
            PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 5))
            {
                PACNT._1_BasicMenu.Form03_Persons frm = new PACNT._1_BasicMenu.Form03_Persons(
                     UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 6));
                frm.ShowDialog();

                DataTable CustomerTable = clDoc.ReturnTable
            (ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
                                           dbo.Table_045_PersonInfo.Column01 AS code,
                                           dbo.Table_045_PersonInfo.Column02 AS NAME,
                                           dbo.Table_065_CityInfo.Column02 AS shahr,
                                           dbo.Table_060_ProvinceInfo.Column01 AS ostan,
                                           dbo.Table_045_PersonInfo.Column06 AS ADDRESS,
                                           dbo.Table_045_PersonInfo.Column30,
                                           Table_045_PersonInfo.Column07,
                                           Table_045_PersonInfo.Column19 AS Mobile
                                    FROM   dbo.Table_045_PersonInfo
                                           LEFT JOIN dbo.Table_065_CityInfo
                                                ON  dbo.Table_065_CityInfo.Column01 = dbo.Table_045_PersonInfo.Column22
                                           LEFT JOIN dbo.Table_060_ProvinceInfo
                                                ON  dbo.Table_060_ProvinceInfo.Column00 = dbo.Table_065_CityInfo.Column00
                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");

                mlt_Customer.DataSource = CustomerTable;


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");

        }

        private void Frm_002_Faktor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.CurrencyManager.Position > -1)
            {

                try
                {

                    //if (mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()) && mlt_Ware.Value.ToString().All(char.IsDigit))
                    //    Properties.Settings.Default.Ware = (mlt_Ware.Value.ToString());
                    //Properties.Settings.Default.Save();
                    //clDoc.RunSqlCommand(ConBase.ConnectionString, "UPDATE Table_030_Setting SET Column02=" + mlt_SaleType.Value + " where ColumnId=54");

                }
                catch
                {
                }

            }


        }

        private void gridEX_List_EditingCell(object sender, EditingCellEventArgs e)
        {
            //try
            //{
            //    if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)[
            //        "Column09"].ToString() != "0" &&
            //        ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)[
            //        "Column10"].ToString() == "0")
            //    {
            //        if (e.Column.Key == "column08" || e.Column.Key == "column09" || 
            //            e.Column.Key == "column10" || e.Column.Key == "column11" ||
            //            e.Column.Key == "column16" || e.Column.Key == "column18")
            //            e.Cancel = false;
            //        else
            //            e.Cancel = true;
            //    }
            //    else
            //    {
            //        if (gridEX_List.GetRow().Cells["column30"].Value.ToString() == "True")
            //            if (e.Column.Key != "column02" && e.Column.Key != "GoodCode")
            //                e.Cancel = false;
            //            else
            //                e.Cancel = true;
            //    }

            //}
            //catch
            //{
            //}
        }

        private void gridEX_List_FormattingRow(object sender, RowLoadEventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    if (e.Row.RowType == Janus.Windows.GridEX.RowType.Record &&
                        e.Row.Cells["column30"].Value.ToString() == "True")
                        e.Row.RowHeaderImageIndex = 0;
                }
                catch { }
            }
        }

        private void bt_Cancel_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                try
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 73))
                        throw new Exception("کاربر گرامی شما امکان ابطال فاکتور فروش را ندارید");

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID) != 0 || clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID) != 0)
                        throw new Exception("به علت صدور حواله برای این فاکتور، ابطال فاکتور امکانپذیر نیست");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                        throw new Exception("این فاکتور باطل شده است");


                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ابطال این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", Convert.ToInt32(RowID), 1);
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                dataSet_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, _ID);
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, _ID);
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, _ID);
                dataSet_Sale.EnforceConstraints = true;
                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

            }
        }

        private void mnu_CancelCancel_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                try
                {

                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 73))
                        throw new Exception("کاربر گرامی شما امکان لغو ابطال فاکتور فروش را ندارید");

                    if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", RowID) == "True")
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به لغو ابطال این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", int.Parse(RowID), 0);
                        }
                    }


                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                clDoc.Update_Des_Table(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", int.Parse(RowID), 0);
                dataSet_Sale.EnforceConstraints = false;
                this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                dataSet_Sale.EnforceConstraints = true;
                this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

            }
        }

        private void mnu_SaleType_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 3))
            {
                _02_BasicInfo.Frm_007_SaleType ob = new _02_BasicInfo.Frm_007_SaleType(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 4));
                ob.ShowDialog();
                mlt_SaleType.DataSource =
                    clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes");


            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void gridEX_List_DeletingRecord(object sender, RowActionCancelEventArgs e)
        {
            if (e.Row.Cells["column30"].Value.ToString() == "False")
            {
                if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف کالا هستید؟",
                    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {
                    e.Row.Delete();



                    txt_TotalPrice.Value = Convert.ToDouble(
                       gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                       AggregateFunction.Sum).ToString());

                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                    Convert.ToDouble(txt_Reductions.Value.ToString());

                    if (!((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                    {
                        //this.table_010_SaleFactorBindingSource.EndEdit();
                        //this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                        //this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                        //this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                        //this.table_011_Child1_SaleFactorTableAdapter.Update(dataSet_Sale.Table_011_Child1_SaleFactor);
                        //this.table_012_Child2_SaleFactorTableAdapter.Update(dataSet_Sale.Table_012_Child2_SaleFactor);
                        Save_Event(sender, e);
                    }

                }
                else
                    e.Cancel = true;
            }

        }



        private void gridEX_List_CellUpdated(object sender, ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");

            }
            catch { }

            try
            {
                //درج نام کالا، کد کالا
                if (e.Column.Key == "column02")
                    gridEX_List.SetValue("GoodCode", gridEX_List.GetValue("column02").ToString());
                else if (e.Column.Key == "GoodCode")
                    gridEX_List.SetValue("column02", gridEX_List.GetValue("GoodCode").ToString());

                //درج تخفیف، اضافه خطی، واحد شمارش، تعداد در کارتن، تعداد در بسته
                if (e.Column.Key == "column02" || e.Column.Key == "GoodCode" ||
                    gridEX_List.GetRow().Cells["column30"].Text.ToString() == "True")
                {





                    GoodbindingSource.Filter = "GoodID=" +
                        gridEX_List.GetRow().Cells["column02"].Value.ToString();
                    gridEX_List.SetValue("Column41", Convert.ToDouble(((DataRowView)GoodbindingSource.CurrencyManager.Current)["buyprice"]));
                    gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(Int32));
                    dt.Columns.Add("Column00", typeof(Int32));
                    dt.Columns.Add("Column01", typeof(String));
                    dt.Columns.Add("Column02", typeof(Double));

                    dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                    //this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));


                    //gridEX_List.DropDowns[6].SetDataBinding(dt, "");
                    this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                    gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                    gridEX_List.SetValue("tedaddarkartoon",
                        0);
                    gridEX_List.SetValue("tedaddarbaste",
                        0);
                    gridEX_List.SetValue("column03",
                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                    gridEX_List.SetValue("column16", 0);
                    gridEX_List.SetValue("column18", 0);

                    gridEX_List.SetValue("Column36", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Weight"].ToString());
                    double amunt = 0;
                    if (!string.IsNullOrWhiteSpace(mlt_SaleType.Value.ToString()))
                    {
                        DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text + "'");
                        if (dr.Count() > 0)
                        {
                            amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                            gridEX_List.SetValue("column10",
                             dr[0].ItemArray[3]);
                        }
                    }

                    if (amunt == Convert.ToDouble(0))
                    {
                        //gridEX_List.SetValue("column10",
                        //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                        //"SalePrice"].ToString());
                        gridEX_List.SetValue("column10",
                         clDoc.ExScalar(ConWare.ConnectionString,
                               "table_004_CommodityAndIngredients", "column34", "ColumnId",
                               gridEX_List.GetValue("GoodCode").ToString()));

                    }
                    gridEX_List.SetValue("column09",
                          0);
                    gridEX_List.SetValue("column08",
                       0);

                }


                if (e.Column.Key == "column03")
                {

                    string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                               "table_004_CommodityAndIngredients", "column07", "ColumnId",
                               gridEX_List.GetValue("GoodCode").ToString());
                    if (gridEX_List.GetValue("column03").ToString() != orginalunit)
                        gridEX_List.SetValue("column10",
                             ((DataRowView)gridEX_List.RootTable.Columns["column03"].DropDown.FindItem(gridEX_List.GetValue("column03")))["sale"].ToString());
                    else
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("ID", typeof(Int32));
                        dt.Columns.Add("Column00", typeof(Int32));
                        dt.Columns.Add("Column01", typeof(String));
                        dt.Columns.Add("Column02", typeof(Double));

                        dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");

                        double amunt = 0;
                        if (!string.IsNullOrWhiteSpace(mlt_SaleType.Value.ToString()))
                        {
                            DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text + "'");
                            if (dr.Count() > 0)
                            {
                                amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                gridEX_List.SetValue("column10",
                                 dr[0].ItemArray[3]);
                            }
                        }

                        if (amunt == Convert.ToDouble(0))
                        {
                            //gridEX_List.SetValue("column10",
                            //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                            //"SalePrice"].ToString());
                            gridEX_List.SetValue("column10",
                         clDoc.ExScalar(ConWare.ConnectionString,
                               "table_004_CommodityAndIngredients", "column34", "ColumnId",
                               gridEX_List.GetValue("GoodCode").ToString()));

                        }
                    }
                    //if (gridEX_List.GetRow().Cells["column06"].Text.Trim() != "")
                    //{
                    //    float h = clDoc.GetZarib(Convert.ToInt32(gridEX_List.GetValue("GoodCode")),Convert.ToInt16(gridEX_List.GetValue("column03")));
                    //    gridEX_List.SetValue("column07", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);
                    //    gridEX_List.SetValue("column06", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);

                    //}
                }

                if (e.Column.Key == "column06")
                {
                    //float h = clDoc.GetZarib(Convert.ToInt32(gridEX_List.GetValue("GoodCode")), Convert.ToInt16(gridEX_List.GetValue("column03")));

                    //gridEX_List.SetValue("column07", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);
                    //gridEX_List.SetValue("column06", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);

                    gridEX_List.SetValue("column07", gridEX_List.GetValue("column06"));
                }

                Double TotalPrice =
                        Convert.ToInt64(Convert.ToDouble(gridEX_List.GetValue("Column07").ToString()) *
                     Convert.ToDouble(gridEX_List.GetValue("column10")));
                gridEX_List.SetValue("Column11", TotalPrice * Convert.ToDouble(gridEX_List.GetValue("Column33").ToString()) / 100);


                gridEX_List.SetValue("column17", 0);
                gridEX_List.SetValue("column19", 0);
                gridEX_List.SetValue("column20", TotalPrice);


                //محاسبه قیمتهای انتهای فاکتور
                txt_TotalPrice.Value = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                    AggregateFunction.Sum).ToString());
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                    Convert.ToDouble(txt_Reductions.Value.ToString());


            }
            catch
            {
            }
        }

        private void bt_AddExtraDiscounts_Click(object sender, EventArgs e)
        {
            if (gridEX_Extra.AllowAddNew == InheritableBoolean.True && this.table_010_SaleFactorBindingSource.Count > 0 && this.table_011_Child1_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_024_Discount");
                    foreach (DataRow item in Table.Rows)
                    {
                        this.table_012_Child2_SaleFactorBindingSource.AddNew();
                        DataRowView New = (DataRowView)this.table_012_Child2_SaleFactorBindingSource.CurrencyManager.Current;
                        New["Column02"] = item["ColumnId"].ToString();
                        if (item["Column03"].ToString() == "True")
                        {
                            New["Column03"] = 0;
                            New["Column04"] = item["Column04"].ToString();
                        }
                        else
                        {
                            New["Column03"] = item["Column04"].ToString();
                            New["Column04"] = double.Parse(item["Column04"].ToString()) *
                                double.Parse(gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"], AggregateFunction.Sum).ToString()) / 100;
                        }

                        New["Column05"] = item["Column02"].ToString();
                        this.table_012_Child2_SaleFactorBindingSource.EndEdit();
                    }

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }



        private void mnu_ViewReturnFactor_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 22))
            {
                foreach (Form item in Application.OpenForms)
                {
                    if (item.Name == "Frm_013_ReturnFactor")
                    {
                        item.BringToFront();
                        ((PSHOP._05_Sale.Frm_013_ReturnFactor)item).txt_Search.Text = (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column20"].ToString() != "0" ?
                             ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column20"].ToString() : "0");
                        ((PSHOP._04_Buy.Frm_003_FaktorKharid)item).bt_Search_Click(sender, e);
                        return;
                    }
                }

                PSHOP._05_Sale.Frm_013_ReturnFactor ob = new Frm_013_ReturnFactor(UserScope.CheckScope
                    (Class_BasicOperation._UserName, "Column11", 23),
                    (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column20"].ToString().Trim() != "" ? int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column20"].ToString()) :
                    0));

                try
                {
                    ob.MdiParent = MainForm.ActiveForm;
                }
                catch { }
                ob.Show();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }

        private void mnu_ViewWareStock_Click(object sender, EventArgs e)
        {
            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 31))
            {
                PWHRS._05_Gozareshat.Frm_003_MojoodiMaghtaiTedadi ob = new PWHRS._05_Gozareshat.Frm_003_MojoodiMaghtaiTedadi();
                try
                {
                    ob.MdiParent = MainForm.ActiveForm;
                }
                catch { }
                ob.Show();

            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }


        private void bt_Paste_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                if (MessageBox.Show("آیا از کپی کردن این فاکتور مطمئن هستید؟",
                    "توجه", MessageBoxButtons.YesNo) == DialogResult.Yes &&
                    gridEX_List.RowCount > 0)
                {



                    if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                    {
                        MessageBox.Show("ابتدا فاکتور را ذخیره و سپس اقدام به کپی نمایید");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    this.table_010_SaleFactorBindingSource.EndEdit();
                    gridEX_List.UpdateData();
                    gridEX_Extra.UpdateData();
                    if (this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Added) != null
                   || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Detached) != null
                      ||
                      this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Added) != null
                   || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Detached) != null
                       ||
                      this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Added) != null
                   || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Detached) != null
                      )
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save_Event(sender, e);
                        }
                    }

                    //درج هدر فاکتور فروش
                    string _CopyCmd = "";
                    string Id = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    int FactorNum = clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "column01");


                    _CopyCmd = @"DECLARE  @Key INT
                                DECLARE @SaleNum INT 

                                INSERT INTO Table_010_SaleFactor
                                (column01,column02,column03,column04,column05,column06,column07,column08,column09,column10,
                                column12,column13,column14,column15,column16,column17,column19,column20,
                                Column28,Column29,Column30,Column31,Column32,Column33,Column34,Column35,Column36,Column38,Column41,Column42,
                                Column43,Column44,Column61,Column65,Column67)
                                (SELECT ((SELECT ISNULL((SELECT MAX(Column01)  FROM   Table_010_SaleFactor ), 0 )) + 1) ,
                                column02,column03,column04,column05,column06,0,0,0,0,0,'" + Class_BasicOperation._UserName + @"',
                                GETDATE(),'" + Class_BasicOperation._UserName + @"',GETDATE(),0,0,0,Column28,
                                0,0,0,Column32,Column33,Column34,Column35,Column36,Column38,0,Column42,Column43,Column44,
                                0,Column65, 0 FROM Table_010_SaleFactor AS tsf WHERE tsf.columnid =" + Id + @" );SET @Key=SCOPE_IDENTITY()
						  
						  INSERT INTO Table_011_Child1_SaleFactor
                                (column01,column02,column03,column04,column05,column06,column07,
                                column08,column09,column10,column11,column15,column16,column17,
                                column18,column19,column20,column21,column22,column23,
                                column24,column25,column26,column27,column28,column29,column30,Column31,
                                Column32,Column33,Column34,Column35,Column36,Column37,Column38,Column39,Column40,Column41
                                )
                                SELECT 
                                @key,column02,column03,column04,column05,column06,column07,column08,
                                column09,column10,column11,0,column16,column17,column18,column19,column20,
                                column21,column22,column23,0,0,0,0,0,0,0,Column31,Column32,
                                Column33,Column34,Column35,Column36,Column37,Column38,
                                Column39,Column40,Column41 FROM Table_011_Child1_SaleFactor where column01 =" + Id + @"   
                               set @SaleNum=( select  Column01  FROM   Table_010_SaleFactor where ColumnId=@Key)
						 select @SaleNum ";

                    if (gridEX_Extra.RowCount > 0)
                    {


                        _CopyCmd += @"INSERT INTO Table_012_Child2_SaleFactor
                                (column01,column02,column03,column04,column05,column06,column07
                                )
                                ( SELECT @key,column02,column03,column04,column05,column06,column07
	                                FROM Table_012_Child2_SaleFactor AS tcsf WHERE tcsf.column01=" + Id + " )";

                    }





                    DataTable dtcopy = Class_BasicOperation.SqlTransactionMethod(ConSale.ConnectionString, _CopyCmd);
                    gridEX_List.MoveFirst();
                    Class_BasicOperation.ShowMsg("", "کپی فاکتور به شماره" + (dtcopy.Rows[0][0] == "" ? LastDocnum : dtcopy.Rows[0][0]) + "با موفقیت انجام شد", "Information");



                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
                this.Cursor = Cursors.Default;

            }

            this.Cursor = Cursors.Default;

        }

        private void bt_DefineSignatures_Click(object sender, EventArgs e)
        {
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 121))
            {
                _05_Sale.Frm_019_Sale_Signatures frm = new Frm_019_Sale_Signatures();
                frm.ShowDialog();
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
        }



        private void mnu_DefaultDescription_Click(object sender, EventArgs e)
        {
            Frm_025_SaleDefaultDescription frm = new Frm_025_SaleDefaultDescription();
            frm.ShowDialog();
        }

        private void mnu_DelDraft_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    int RowID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID.ToString());

                    if (DraftId != 0)
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 24))
                        {

                            PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;
                            PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                            PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
                            PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                            PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
                            PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;

                            PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts frm = new PWHRS._03_AmaliyatAnbar.Form06_RegisterDrafts
                            (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 25), -1);
                            frm.txt_Search.Text = clDoc.ExScalar(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column01", "ColumnId", DraftId.ToString());
                            frm.bt_Search_Click(sender, e);
                            frm.bt_Del_Click(sender, e);

                        }
                        else
                            Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
                    }

                    dataSet_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, RowID);
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, RowID);
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, RowID);
                    dataSet_Sale.EnforceConstraints = true;
                    txt_Search.SelectAll();
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }

        }


        private Decimal LastBuyGoodPrice(int GoodCode)
        {
            DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, @"declare @t table(GoodCode int,Date nvarchar(50), Price decimal(18,3));
            insert into @t SELECT     Table_016_Child1_BuyFactor.column02,  MAX(Table_015_BuyFactor.column02) AS Date,1
            FROM         Table_016_Child1_BuyFactor INNER JOIN
            Table_015_BuyFactor ON Table_016_Child1_BuyFactor.column01 = Table_015_BuyFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @"
            GROUP BY Table_016_Child1_BuyFactor.column02
            order by Table_016_Child1_BuyFactor.column02;
            
            declare @t2 table(codekala2 int, gheymat2 int,date2 nvarchar(50)
            ,UNIQUE (codekala2,gheymat2,date2)
            );

            insert into @t2 SELECT   dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, 
            dbo.Table_015_BuyFactor.column02 AS Date
            FROM         dbo.Table_016_Child1_BuyFactor INNER JOIN
            dbo.Table_015_BuyFactor ON dbo.Table_016_Child1_BuyFactor.column01 = dbo.Table_015_BuyFactor.columnid
            where Table_016_Child1_BuyFactor.column02=" + GoodCode + @"
            GROUP BY dbo.Table_016_Child1_BuyFactor.column02, dbo.Table_016_Child1_BuyFactor.column10, dbo.Table_015_BuyFactor.column02;
            update @t set Price=gheymat2 from @t2 as main_table where GoodCode=codekala2 and Date=date2; 
            select * from @t");

            if (Table.Rows.Count == 0)
                return 0;
            else
                return Convert.ToDecimal(Table.Rows[0]["Price"].ToString());

        }

        private void bt_NotConfirmDraft_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                int DraftId = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                if (DraftId == 0)
                    return;
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 69))
                {
                    string Message = null;

                    if (clDoc.ExScalar(ConWare.ConnectionString, "Table_007_PwhrsDraft", "Column26", "ColumnId", DraftId.ToString()) == "True")
                    {
                        Message = "آیا مایل به غیر قطعی کردن حواله انبار هستید؟";
                        if (DialogResult.Yes == MessageBox.Show(Message, "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_007_PwhrsDraft SET Column26=0 where ColumnId=" +
                              DraftId);
                            Class_BasicOperation.ShowMsg("", "غیرقطعی کردن حواله انبار با موفقیت انجام گرفت", "Information");
                        }

                    }
                }
            }
        }

        private void gridEX_List_CancelingCellEdit(object sender, ColumnActionCancelEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "GoodCode");

            }
            catch { }
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            try
            {
                // gridEX1.UpdateData();
                table_010_SaleFactorBindingSource.EndEdit();
                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                    dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save_Event(sender, e);
                        if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                        {
                            throw new Exception("خطا در ثبت اطلاعات");

                        }

                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select min(Column01) from Table_010_SaleFactor where   (column44=" + projectId + " or '" + (Isadmin) + "'=N'True') ),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Sale.EnforceConstraints = false;


                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));


                    dataSet_Sale.EnforceConstraints = true;
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                }

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                try
                {
                    //   gridEX1.UpdateData();
                    table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                    if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                        dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save_Event(sender, e);
                            if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                            {
                                throw new Exception("خطا در ثبت اطلاعات");

                            }

                        }
                    }


                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString,
                        "Select ISNULL((Select max(Column01) from Table_010_SaleFactor where Column01<" +
                        ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + " and (column44=" + projectId + " or '" + (Isadmin) + "'=N'True') ),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                        dataSet_Sale.EnforceConstraints = false;


                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));


                        dataSet_Sale.EnforceConstraints = true;
                        this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {

                try
                {
                    //   gridEX1.UpdateData();
                    table_010_SaleFactorBindingSource.EndEdit();
                    this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                    this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                    if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                        dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                    {
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                        {
                            Save_Event(sender, e);
                            if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                            {
                                throw new Exception("خطا در ثبت اطلاعات");

                            }

                        }
                    }

                    DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select Min(Column01) from Table_010_SaleFactor where Column01>" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString() + " and (column44=" + projectId + " or '" + (Isadmin) + "'=N'True') ),0) as Row");
                    if (Table.Rows[0]["Row"].ToString() != "0")
                    {
                        DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());

                        dataSet_Sale.EnforceConstraints = false;


                        this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                        this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));


                        dataSet_Sale.EnforceConstraints = true;
                        this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
            }
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            try
            {
                // gridEX1.UpdateData();
                table_010_SaleFactorBindingSource.EndEdit();
                this.table_011_Child1_SaleFactorBindingSource.EndEdit();
                this.table_012_Child2_SaleFactorBindingSource.EndEdit();

                if (dataSet_Sale.Table_010_SaleFactor.GetChanges() != null || dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges() != null ||
                    dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges() != null)
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save_Event(sender, e);
                        if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString().StartsWith("-"))
                        {
                            throw new Exception("خطا در ثبت اطلاعات");

                        }

                    }
                }

                DataTable Table = clDoc.ReturnTable(ConSale.ConnectionString, "Select ISNULL((Select max(Column01) from Table_010_SaleFactor where (column44=" + projectId + " or '" + (Isadmin) + "'=N'True')),0) as Row");
                if (Table.Rows[0]["Row"].ToString() != "0")
                {
                    DataTable RowId = clDoc.ReturnTable(ConSale.ConnectionString, "Select ColumnId from Table_010_SaleFactor where Column01=" + Table.Rows[0]["Row"].ToString());
                    dataSet_Sale.EnforceConstraints = false;


                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowId.Rows[0]["ColumnId"].ToString()));


                    dataSet_Sale.EnforceConstraints = true;
                    this.table_010_SaleFactorBindingSource_PositionChanged(sender, e);

                }

            }
            catch
            {
            }
        }

        private void SendSMEM()
        {

        }


        private void gridEX_List_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (
                    //((Janus.Windows.GridEX.GridEX)(sender)).Col == 5 &&

                 gridEX_List.GetValue("GoodCode") != null)
                {
                    gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                }
                if (
                    //((Janus.Windows.GridEX.GridEX)(sender)).Col == 14 &&

                 gridEX_List.GetValue("GoodCode") != null)
                {
                    this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                    gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                }
                // gridEX_List.DropDowns["GoodPrice"].SetDataBinding(clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   "), "");
                //this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
            }
            catch
            {
            }
        }

        private void چاپ8سانتیToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.table_010_SaleFactorBindingSource.Count > 0)
            {
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                {
                    List<string> List = new List<string>();
                    List.Add(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString());
                    _05_Sale.Reports.Form_SaleFactorPrint1 frm = new Reports.Form_SaleFactorPrint1(List,
                        int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 23);
                    frm.Form_FactorPrint_Load(sender, e);

                }
                else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
            }
        }

        private void bt_Print_Click_1(object sender, EventArgs e)
        {
            //try
            //{
            //    if (this.table_010_SaleFactorBindingSource.Count > 0)
            //    {
            //        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
            //        {
            //            _05_Sale.Reports.Form_SaleFactorPrint frm = new Reports.Form_SaleFactorPrint(
            //                    int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), false);
            //            frm.ShowDialog();
            //        }
            //        else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
            //    }
            //}
            //catch { }
        }

        private void ForiFctor_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        // Save_Event(sender, e);

                        this.table_010_SaleFactorBindingSource.EndEdit();
                        gridEX_List.UpdateData();
                        gridEX_Extra.UpdateData();
                        if (this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Added) != null
                     || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Detached) != null
                        ||
                        this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Added) != null
                     || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Detached) != null
                         ||
                        this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Added) != null
                     || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Detached) != null
                        )
                        {
                            if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                            {
                                Save_Event(sender, e);
                            }

                        }
                        else
                        {

                            table_012_Child2_SaleFactorBindingSource.CancelEdit();
                            table_011_Child1_SaleFactorBindingSource.CancelEdit();
                            table_010_SaleFactorBindingSource.CancelEdit();
                        }


                        Print = clDoc.ExScalar(ConBase.ConnectionString, @"select Column27 from Table_295_StoreInfo where Column00=" + mlt_Stor.Value + "");

                        if (Print.ToString()=="True")
                        {

                               #region Print8cmm


                                    string HeaderSelectText = null;
                                    DataTable HeaderTable;
                                    DataTable DetailTable;
                                    HeaderSelectText = @"SELECT     FactorTable.FactorID AS ID,DetailID, FactorTable.Serial, FactorTable.LegalNumber, FactorTable.Date, FactorTable.Responsible, FactorTable.CustomerID, FactorTable.P2Name, FactorTable.Mobile,
                                 FactorTable.P2NationalCode,
                                   FactorTable.P2ECode,FactorTable.Description2,{1}.dbo.table_004_CommodityAndIngredients.Column36 as Ficonsumer ,{1}.dbo.Table_003_InformationProductCash.column04 ,
       FactorTable.GoodNameTotal,
                                   FactorTable.P2SabtCode, FactorTable.P2Address, FactorTable.P2Tel, FactorTable.P2Fax, FactorTable.P2PostalCode, FactorTable.P2Code, FactorTable.GoodCode, 
                                FactorTable.GoodName, FactorTable.Box, FactorTable.BoxPrice, FactorTable.Pack, FactorTable.PackPrice, FactorTable.Number, FactorTable.TotalNumber, 
                                FactorTable.SinglePrice, FactorTable.TotalPrice, FactorTable.DiscountPercent, FactorTable.DiscountPrice, FactorTable.TaxPrice,FactorTable.TotalWeight,FactorTable.TaxPercent, FactorTable.NetPrice, 
                                ISNULL(FactorTable.Ezafat, 0) AS Ezafat, ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat, PersonInfoTable.Column02, FactorTable.NetTotal,FactorTable.Cash,FactorTable.Cart,FactorTable.Etebari,FactorTable.[Check],FactorTable.Bon,FactorTable.VolumeGroup,
                                FactorTable.SpecialGroup, FactorTable.SpecialCustomer, FactorTable.Description, FactorTable.CountUnitName, derivedtbl_1.Groups, '-' AS charPrice, 
                                'SettleInfo' AS SettleInfo, FactorTable.FactorType, FactorTable.NumberInBox, FactorTable.RowDes, FactorTable.Zarib, FactorTable.NumberInBox AS Expr1, 
                                FactorTable.NumberInPack, CityTable.Column02 AS CityName, ProvinceTable.Column01 AS ProvinceName,FactorTable.PayCash,FactorTable.DraftNumber,FactorTable.DocID,FactorTable.Project,FactorTable.BuyerName,FactorTable.SaleType,FactorTable.DocNum
                                FROM         (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
                                FROM         {0}.dbo.Table_060_ProvinceInfo) AS ProvinceTable INNER JOIN
                                (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                                FROM         {0}.dbo.Table_065_CityInfo) AS CityTable ON ProvinceTable.Column00 = CityTable.Column00 RIGHT OUTER JOIN
                                (SELECT     dbo.Table_010_SaleFactor.columnid AS FactorID, dbo.Table_010_SaleFactor.column01 AS Serial, dbo.Table_010_SaleFactor.column37 AS LegalNumber, 
                                dbo.Table_010_SaleFactor.column02 AS Date,isnull(dbo.Table_010_SaleFactor.Column65,'') AS BuyerName,isnull((select column02 from {0}.dbo.Table_002_SalesTypes where columnid=dbo.Table_010_SaleFactor.Column36),'') as SaleType, dbo.Table_010_SaleFactor.column03 AS CustomerID, PersonTable.Column02 AS P2Name,PersonTable.Column19 AS Mobile, 
                                  PersonTable.Column09 AS P2NationalCode,
                                                   PersonTable.Column141 AS P2ECode,
                                                   PersonTable.Column142 AS P2SabtCode, PersonTable.Column01 AS P2Code, PersonTable.Column06 AS P2Address, PersonTable.Column07 AS P2Tel, 
                                PersonTable.Column08 AS P2Fax, PersonTable.Column13 AS P2PostalCode, GoodTable.column01 AS GoodCode, 
                                dbo.Table_011_Child1_SaleFactor.column04 AS Box, dbo.Table_011_Child1_SaleFactor.column08 AS BoxPrice, 
                                dbo.Table_011_Child1_SaleFactor.column05 AS Pack, dbo.Table_011_Child1_SaleFactor.column09 AS PackPrice, 
                                dbo.Table_011_Child1_SaleFactor.column06 AS Number, dbo.Table_011_Child1_SaleFactor.column07 AS TotalNumber, 
                                dbo.Table_011_Child1_SaleFactor.column10 AS SinglePrice, dbo.Table_011_Child1_SaleFactor.column11 AS TotalPrice, dbo.Table_011_Child1_SaleFactor.column23 AS Description2,
                                dbo.Table_011_Child1_SaleFactor.column16 AS DiscountPercent, dbo.Table_011_Child1_SaleFactor.column17 AS DiscountPrice, dbo.Table_011_Child1_SaleFactor.column18 as TaxPercent,
                                dbo.Table_011_Child1_SaleFactor.column19 AS TaxPrice,dbo.Table_011_Child1_SaleFactor.column37 as TotalWeight, dbo.Table_011_Child1_SaleFactor.column20 AS NetPrice, GoodTable.column02 AS GoodName,GoodTable.column05 AS GoodNameTotal, 
                                OtherPrice.PlusPrice AS Ezafat, OtherPrice.MinusPrice AS Kosoorat, dbo.Table_010_SaleFactor.column05 AS Responsible, 
                                dbo.Table_010_SaleFactor.Column28 AS NetTotal, dbo.Table_010_SaleFactor.column46 AS Cash,dbo.Table_010_SaleFactor.column47 AS Cart,dbo.Table_010_SaleFactor.column48 AS Etebari,
                                dbo.Table_010_SaleFactor.column52 AS [Check],dbo.Table_010_SaleFactor.column54 AS [Bon],dbo.Table_010_SaleFactor.Column29 AS VolumeGroup, 
                                dbo.Table_010_SaleFactor.Column30 AS SpecialGroup, dbo.Table_010_SaleFactor.Column31 AS SpecialCustomer, 
                                dbo.Table_010_SaleFactor.column06 AS Description, CountUnitTable.Column01 AS CountUnitName, 
                                CASE WHEN Table_010_SaleFactor.Column12 = 0 THEN '***فاکتور ریالی***' ELSE '***فاکتور ارزی***' END AS FactorType, 
                                dbo.Table_011_Child1_SaleFactor.column23 AS RowDes, dbo.Table_011_Child1_SaleFactor.column31 AS NumberInBox, 
                                dbo.Table_011_Child1_SaleFactor.column32 AS NumberInPack, dbo.Table_011_Child1_SaleFactor.Column33 AS Zarib, 
                                PersonTable.Column21 AS ProvinceId, PersonTable.Column22 AS CityId,Table_010_SaleFactor.column21 as PayCash,
                                (Select Column01 from  {1}.dbo.Table_007_PwhrsDraft where Columnid=Table_010_SaleFactor.Column09) as DraftNumber,
                                Table_010_SaleFactor.Column10 as DocId,(select isnull(Column00,0) from " + ConAcnt.Database + @".dbo.Table_060_SanadHead where ColumnId=Table_010_SaleFactor.Column10)  as DocNum,       Project.column02 as Project,dbo.Table_011_Child1_SaleFactor.columnid as DetailID
 

                                FROM         dbo.Table_010_SaleFactor INNER JOIN
                                dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = dbo.Table_011_Child1_SaleFactor.column01 INNER JOIN
                                (SELECT     ColumnId, Column00, Column01, Column02,Column19, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, 
                                Column11, Column12, Column13, Column21, Column22       ,Column141,
                                                                   Column142
                                FROM         {0}.dbo.Table_045_PersonInfo) AS PersonTable ON dbo.Table_010_SaleFactor.column03 = PersonTable.ColumnId LEFT OUTER JOIN
                                (SELECT     columnid, SUM(PlusPrice) AS PlusPrice, SUM(MinusPrice) AS MinusPrice
                                FROM         (SELECT     Table_010_SaleFactor_2.columnid, SUM(dbo.Table_012_Child2_SaleFactor.column04) AS PlusPrice, 0 AS MinusPrice
                                FROM         dbo.Table_012_Child2_SaleFactor INNER JOIN
                                dbo.Table_010_SaleFactor AS Table_010_SaleFactor_2 ON 
                                dbo.Table_012_Child2_SaleFactor.column01 = Table_010_SaleFactor_2.columnid
                                WHERE     (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                GROUP BY Table_010_SaleFactor_2.columnid, dbo.Table_012_Child2_SaleFactor.column05
                                UNION ALL
                                SELECT     Table_010_SaleFactor_1.columnid, 0 AS PlusPrice, SUM(Table_012_Child2_SaleFactor_1.column04) AS MinusPrice
                                FROM         dbo.Table_012_Child2_SaleFactor AS Table_012_Child2_SaleFactor_1 INNER JOIN
                                dbo.Table_010_SaleFactor AS Table_010_SaleFactor_1 ON 
                                Table_012_Child2_SaleFactor_1.column01 = Table_010_SaleFactor_1.columnid
                                WHERE     (Table_012_Child2_SaleFactor_1.column05 = 1)
                                GROUP BY Table_010_SaleFactor_1.columnid, Table_012_Child2_SaleFactor_1.column05) AS OtherPrice_1
                                GROUP BY columnid) AS OtherPrice ON dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid LEFT OUTER JOIN
                                (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, 
                                column12, column13, column14, column15, column16, column17, column18, column19, column20, column21, column22, column23, 
                                column24, column25, column26, column27, column28, column29, column30, column31
                                FROM         {1}.dbo.table_004_CommodityAndIngredients) AS GoodTable ON 
                                dbo.Table_011_Child1_SaleFactor.column02 = GoodTable.columnid LEFT OUTER JOIN
                                (SELECT     Column00, Column01
                                FROM         {0}.dbo.Table_070_CountUnitInfo) AS CountUnitTable ON dbo.Table_011_Child1_SaleFactor.column03 = CountUnitTable.Column00  LEFT OUTER JOIN (
                                SELECT Column00,
                                       Column02
                                FROM   {0}.dbo.Table_035_ProjectInfo
                            ) AS Project
                            ON  dbo.Table_011_Child1_SaleFactor.column22 = 
                                Project.Column00 ) 
                                AS FactorTable ON CityTable.Column01 = FactorTable.CityId LEFT OUTER JOIN
                                (SELECT     PersonId, Groups
                                FROM         {0}.dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1) AS derivedtbl_1 ON FactorTable.CustomerID = derivedtbl_1.PersonId LEFT OUTER JOIN
                                (SELECT     ColumnId, Column01, Column02, Column21, Column22
                                FROM         {0}.dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1) AS PersonInfoTable ON FactorTable.Responsible = PersonInfoTable.ColumnId    LEFT OUTER JOIN
                         {1}.dbo.table_004_CommodityAndIngredients ON FactorTable.GoodCode = {1}.dbo.table_004_CommodityAndIngredients.column01   left outer join 
					   {1}.dbo.Table_003_InformationProductCash ON 
                          {1}.dbo.table_004_CommodityAndIngredients.columnid =  {1}.dbo.Table_003_InformationProductCash.column01";



                                    string DetailSelectText = @"SELECT     dbo.Table_024_Discount.column01 AS Name, dbo.Table_012_Child2_SaleFactor.column04 AS Price, 
                      CASE WHEN Table_024_Discount.column02 = 0 THEN '+' ELSE '-' END AS Type, dbo.Table_012_Child2_SaleFactor.column01 AS Column01 ,
                      dbo.Table_010_SaleFactor.column01 AS HeaderNum, dbo.Table_010_SaleFactor.column02 AS HeaderDate
                      FROM         dbo.Table_012_Child2_SaleFactor INNER JOIN
                      dbo.Table_010_SaleFactor ON dbo.Table_012_Child2_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid LEFT OUTER JOIN
                      dbo.Table_024_Discount ON dbo.Table_012_Child2_SaleFactor.column02 = dbo.Table_024_Discount.columnid";


                                    HeaderSelectText += " WHERE     (FactorTable.Serial = " + txt_num.Text + ")";
                                    DetailSelectText += " WHERE (Table_010_SaleFactor.Column01= " + txt_num.Text + ")";
                                    HeaderSelectText += " ORDER BY  FactorTable.FactorID,FactorTable.DetailID";

                                    HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);
                                    HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
                                    DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, DetailSelectText);

                                    try
                                    {


                                        StiReport stireport = new StiReport();
                                        stireport.Load("Sales Invoice 80mm.mrt");
                                        stireport.Pages["Page3"].Enabled = true;
                                        stireport.Compile();
                                        stireport.RegData("Rpt_SaleTable", HeaderTable);
                                        stireport.RegData("Rpt_SaleExtra_Table", DetailTable);
                                        //stireport["Image"] = Image.FromStream(stream);
                                        this.Cursor = Cursors.Default;
                                        stireport.Render(false);
                                        stireport.Print(false);

                                    }

                                    catch (Exception ex)
                                    { Class_BasicOperation.CheckExceptionType(ex, this.Name); }




                                    #endregion
                        }
                        else
                        {
                            List<string> List = new List<string>();
                            //List.Add(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString());
                            _05_Sale.Reports.Form_SaleFactorPrint1 frm = new Reports.Form_SaleFactorPrint1(List,
                                int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 1);
                            frm.Form_FactorPrint_Load(null, null);

                        }
                   

                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch
            {
                try
                {
                    List<string> List = new List<string>();
                    _05_Sale.Reports.Form_SaleFactorPrint1 frm = new Reports.Form_SaleFactorPrint1(List,
                            int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()), 1);


                }
                catch (Exception ex1)
                {
                    MessageBox.Show(ex1.Message);
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void rasmiFactor_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                        {
                            //Save_Event(sender, e);

                            Save_Event(sender, e);

                            _05_Sale.Reports.Form_SaleFactorPrint1 frm =
                               new Reports.Form_SaleFactorPrint1(int.Parse(txt_num.Value.ToString()), 19);
                            frm.ShowDialog();
                        }
                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch
            {
                try
                {
                    _05_Sale.Reports.Form_SaleFactorPrint1 frm =
                              new Reports.Form_SaleFactorPrint1(int.Parse(txt_num.Value.ToString()), 19);
                    frm.ShowDialog();
                }
                catch
                { }

            }
            this.Cursor = Cursors.Default;
        }

        private void Adifactor_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        //  Save_Event(sender, e);


                        _05_Sale.Reports.Form_SaleFactorPrint1 frm =
                                new Reports.Form_SaleFactorPrint1(int.Parse(txt_num.Value.ToString()), 19);
                        frm.ShowDialog();

                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch
            {

                try
                {
                    _05_Sale.Reports.Form_SaleFactorPrint1 frm =
                               new Reports.Form_SaleFactorPrint1(int.Parse(txt_num.Value.ToString()), 19);
                    frm.ShowDialog();
                }
                catch
                { }

            }
            //try
            //{
            //    if (this.table_010_SaleFactorBindingSource.Count > 0)
            //    {
            //        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
            //        {
            //            Save_Event(sender, e);


            //            _05_Sale.Reports.Form_SaleFactorPrint2 frm =
            //                    new Reports.Form_SaleFactorPrint2(int.Parse(gridEX1.GetValue("column01").ToString()), false, 19);
            //            frm.ShowDialog();

            //        }
            //        else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
            //    }
            //}
            //catch
            //{

            //    try
            //    {
            //        _05_Sale.Reports.Form_SaleFactorPrint2 frm =
            //                   new Reports.Form_SaleFactorPrint2(int.Parse(gridEX1.GetValue("column01").ToString()), false, 19);
            //        frm.ShowDialog();
            //    }
            //    catch
            //    { }

            //}


        }



        private void txt_GoodCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Add)
            {
                txt_Count.Text = (Convert.ToInt32(txt_Count.Text) + 1).ToString();
            }
            else if (e.KeyCode == Keys.Subtract)
            {
                if ((Convert.ToInt32(txt_Count.Text) - 1) > 0)

                    txt_Count.Text = (Convert.ToInt32(txt_Count.Text) - 1).ToString();

            }
        }


        private void addnew()
        {
            txt_Count.Text = "1";
            txt_GoodCode.Text = string.Empty;
            txt_GoodCode.Focus();
            txt_GoodCode.SelectAll();

        }

        DataTable dtBarcode = new DataTable();
        //string cmd = "";
        long codeid = 0;
        double buyprice = 0;
        int ok = 0;
        string Codegenerate = "";
        int NumLength =0;
        string GoodCode = "";
        string GoodId = "";
        string IntNum = "";
        string FloatNum ="";
        string TotalNum ="";
        double TotalPrice = 0;
        string SinglePrice = "";
        double amunt = 0;
        string UnitNumer = "";
        private void InitialNewRow()
        {
            try
            {

                bool isthere = false;


                if (txt_GoodCode.Text != string.Empty)
                {
                    #region چک کردن کالا وارد شده در انبار
                   
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        Con.Open();
                        SqlCommand Comm = new SqlCommand("SELECT tcc.columnid FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.column06='" + txt_GoodCode.Text + "'", Con);
                        codeid = Convert.ToInt64(Comm.ExecuteScalar());




                        Comm = new SqlCommand(@"if exists (select * from table_004_CommodityAndIngredients where column06='" + txt_GoodCode.Text + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok", Con);
                        ok = Convert.ToInt32(Comm.ExecuteScalar());

                    }
                    #endregion
                    if (ok == 1)
                    {
                        #region کد کالا در جدول کالا بوده است

                        #region  درصورت وجود داشتن کالا در لیست

                        if (gridEX_List.GetRows().Count() > 0)
                            {
                                string goodcode;
                                Int16 unit;
                                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                                {
                                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                                    {
                                        Con.Open();
                                        SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                        goodcode = (Comm.ExecuteScalar().ToString());
                                        Comm = new SqlCommand("SELECT tcc.column07 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                        unit = Convert.ToInt16(Comm.ExecuteScalar());
                                    }


                                    if (goodcode == txt_GoodCode.Text && unit == Convert.ToInt16(item.Cells["column03"].Value) && Convert.ToBoolean(item.Cells["column30"].Value) == false)
                                    {

                                        isthere = true;
                                        item.BeginEdit();



                                        item.Cells["Column07"].Value = Convert.ToInt32(item.Cells["Column07"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);
                                        item.Cells["Column06"].Value = Convert.ToInt32(item.Cells["Column06"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);

                                        item.EndEdit();
                                        double TotalPrice;
                                        if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                        {
                                            TotalPrice = Convert.ToDouble(item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(item.Cells["Column07"].Value.ToString());
                                            item.BeginEdit();
                                            item.Cells["column11"].Value = TotalPrice;
                                            item.Cells["column16"].Value = 0;
                                            item.Cells["column18"].Value = 0;
                                            item.Cells["column17"].Value = 0;
                                            item.Cells["Column19"].Value = 0;
                                            item.Cells["Column40"].Value = TotalPrice;
                                            item.Cells["Column20"].Value = TotalPrice;

                                            item.EndEdit();

                                        }
                                        gridEX_List.UpdateData();

                                        //محاسبه قیمتهای انتهای فاکتور
                                        txt_TotalPrice.Value = Convert.ToDouble(
                                            gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                            AggregateFunction.Sum).ToString());

                                        txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                        txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                        Convert.ToDouble(txt_Extra.Value.ToString()) -
                                        Convert.ToDouble(txt_Reductions.Value.ToString());



                                        break;

                                    }

                                }
                                if (!isthere)
                                {

                                    gridEX_List.MoveToNewRecord();
                                    gridEX_List.SetValue("GoodCode", codeid);
                                    gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                                    gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                                    gridEX_List.SetValue("column02", codeid);

                                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                                    {
                                        Con.Open();
                                        SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + codeid + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                        buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                    }
                                    gridEX_List.SetValue("Column41", buyprice);



                                    GoodbindingSource.Filter = "GoodID=" +
                                            gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                    if (GoodbindingSource.CurrencyManager.Position > -1)
                                    {
                                        gridEX_List.SetValue("tedaddarkartoon",
                                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                        gridEX_List.SetValue("tedaddarbaste",
                                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                        gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                        gridEX_List.SetValue("column03",
                                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                        gridEX_List.SetValue("column16",
                                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());



                                        DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                        this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                        gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                        double amunt = 0;
                                        if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                        {
                                            DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                            if (dr.Count() > 0)
                                            {
                                                amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                                gridEX_List.SetValue("column10",
                                                 dr[0].ItemArray[3]);
                                            }
                                        }

                                        if (amunt == Convert.ToDouble(0))
                                        {
                                            //gridEX_List.SetValue("column10",
                                            //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                            //"SalePrice"].ToString());
                                            gridEX_List.SetValue("column10",
                             clDoc.ExScalar(ConWare.ConnectionString,
                                   "table_004_CommodityAndIngredients", "column34", "ColumnId",
                                   gridEX_List.GetValue("GoodCode").ToString()));

                                        }
                                        gridEX_List.SetValue("column09",
                                              0);
                                        gridEX_List.SetValue("column08",
                                           0);

                                        double TotalPrice;
                                        if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                        {
                                            TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                            gridEX_List.SetValue("column11", TotalPrice);
                                            gridEX_List.SetValue("Column16", 0);
                                            gridEX_List.SetValue("Column18", 0);
                                            gridEX_List.SetValue("column17", 0);
                                            gridEX_List.SetValue("Column19", 0);
                                            gridEX_List.SetValue("Column40", TotalPrice);
                                            gridEX_List.SetValue("Column20", TotalPrice);
                                        }


                                        gridEX_List.UpdateData();
                                        //محاسبه قیمتهای انتهای فاکتور
                                        txt_TotalPrice.Value = Convert.ToDouble(
                                            gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                            AggregateFunction.Sum).ToString());

                                        txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                        txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                        Convert.ToDouble(txt_Extra.Value.ToString()) -
                                        Convert.ToDouble(txt_Reductions.Value.ToString());




                                    }
                                    else
                                    {
                                        Class_BasicOperation.ShowMsg("", "لطفا اطلاعات کالا را به روز رسانی کنید", "Warning");
                                        return;
                                    }  


                                }


                            }
                            #endregion

                            #region درصورت وجود نداشتن کالا در لیست
                            else
                            {

                                gridEX_List.MoveToNewRecord();
                                gridEX_List.SetValue("GoodCode", codeid);
                                gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                                gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                                gridEX_List.SetValue("column02", codeid);

                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                                {
                                    Con.Open();
                                    SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + codeid + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                    buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                }
                                gridEX_List.SetValue("Column41", buyprice);
                                {
                                    GoodbindingSource.Filter = "GoodID=" +
                                        gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                    if (GoodbindingSource.CurrencyManager.Position > -1)
                                    {
                                        gridEX_List.SetValue("tedaddarkartoon",
                                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                        gridEX_List.SetValue("tedaddarbaste",
                                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                        gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                        gridEX_List.SetValue("column03",
                                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                        gridEX_List.SetValue("column16",
                                            ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());

                                        DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                        this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                        gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                        if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                        {
                                            DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                            if (dr.Count() > 0)
                                            {
                                                amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                                gridEX_List.SetValue("column10",
                                                 dr[0].ItemArray[3]);
                                            }
                                        }

                                        if (amunt == Convert.ToDouble(0))
                                        {
                                           
                                            gridEX_List.SetValue("column10",
                             clDoc.ExScalar(ConWare.ConnectionString,
                                   "table_004_CommodityAndIngredients", "column34", "ColumnId",
                                   gridEX_List.GetValue("GoodCode").ToString()));

                                        }
                                        gridEX_List.SetValue("column09",
                                              0);
                                        gridEX_List.SetValue("column08",
                                           0);
                                        double TotalPrice;
                                        if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                        {
                                            TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                            gridEX_List.SetValue("column11", TotalPrice);
                                            gridEX_List.SetValue("Column16", 0);
                                            gridEX_List.SetValue("Column18", 0);
                                            gridEX_List.SetValue("column17", 0);
                                            gridEX_List.SetValue("Column19", 0);
                                            gridEX_List.SetValue("Column40", TotalPrice);
                                            gridEX_List.SetValue("Column20", TotalPrice);
                                        }

                                        gridEX_List.UpdateData();

                                        txt_TotalPrice.Value = Convert.ToDouble(
                                            gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                            AggregateFunction.Sum).ToString());
                                        txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                        txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                        Convert.ToDouble(txt_Extra.Value.ToString()) -
                                        Convert.ToDouble(txt_Reductions.Value.ToString());



                                    }
                                    else
                                    {
                                        Class_BasicOperation.ShowMsg("", "لطفا اطلاعات کالا را به روز رسانی کنید", "Warning");
                                        return;
                                    }  
                                }
                            }
                            #endregion
                      



                        #endregion

                    }


                    else
                    {
                        #region کد کالا در جدول کالا نبوده است

                        if (mlt_Stor.Value.ToString() != "" || mlt_Stor.Value.ToString() != "0")
                        {

                            dtBarcode = clDoc.ReturnTable(ConBase.ConnectionString, @"select * from Table_295_StoreInfo where ColumnId=" + mlt_Stor.Value + "");


                        if (dtBarcode.Rows[0]["Column16"].ToString() != "True" && dtBarcode.Rows[0]["Column17"].ToString() != "True")
                        {

                            #region  درصورت وجود داشتن کالا در لیست

                            if (gridEX_List.GetRows().Count() > 0)
                            {

                                 //Codegenerate = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column19"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                 //NumLength = Codegenerate.Length;
                                GoodCode = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column20"])-1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));

                                cmd = clDoc.ExScalar(ConWare.ConnectionString, @"if exists (select * from table_004_CommodityAndIngredients where column06='" + GoodCode + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok");
                                ok = Convert.ToInt32(cmd);

                                if (ok == 1)
                                {

                                    string goodcode;
                                    Int16 unit;

                                    GoodId = clDoc.ExScalar(ConWare.ConnectionString, @"select Columnid from table_004_CommodityAndIngredients where Column06='" + GoodCode + "'");
                                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                                    {
                                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                                        {
                                            Con.Open();
                                            SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                            goodcode = (Comm.ExecuteScalar().ToString());
                                            Comm = new SqlCommand("SELECT tcc.column07 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                            unit = Convert.ToInt16(Comm.ExecuteScalar());
                                        }


                                        if (goodcode == GoodCode && unit == Convert.ToInt16(item.Cells["column03"].Value) && Convert.ToBoolean(item.Cells["column30"].Value) == false)
                                        {

                                            isthere = true;
                                            item.BeginEdit();



                                            item.Cells["Column07"].Value = Convert.ToInt32(item.Cells["Column07"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);
                                            item.Cells["Column06"].Value = Convert.ToInt32(item.Cells["Column06"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);

                                            item.EndEdit();
                                            double TotalPrice;
                                            if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                            {
                                                TotalPrice = Convert.ToDouble(item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(item.Cells["Column07"].Value.ToString());
                                                item.BeginEdit();
                                                item.Cells["column11"].Value = TotalPrice;
                                                item.Cells["column16"].Value = 0;
                                                item.Cells["column18"].Value = 0;
                                                item.Cells["column17"].Value = 0;
                                                item.Cells["Column19"].Value = 0;
                                                item.Cells["Column40"].Value = TotalPrice;
                                                item.Cells["Column20"].Value = TotalPrice;

                                                item.EndEdit();

                                            }
                                            gridEX_List.UpdateData();

                                            //محاسبه قیمتهای انتهای فاکتور
                                            txt_TotalPrice.Value = Convert.ToDouble(
                                                gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                AggregateFunction.Sum).ToString());

                                            txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                            txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                            Convert.ToDouble(txt_Extra.Value.ToString()) -
                                            Convert.ToDouble(txt_Reductions.Value.ToString());



                                            break;

                                        }

                                    }
                                    if (!isthere)
                                    {

                                        gridEX_List.MoveToNewRecord();
                                        gridEX_List.SetValue("GoodCode", GoodId);
                                        gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                                        gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                                        gridEX_List.SetValue("column02", GoodId);

                                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                                        {
                                            Con.Open();
                                            SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + GoodId + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                            buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                        }
                                        gridEX_List.SetValue("Column41", buyprice);



                                        GoodbindingSource.Filter = "GoodID=" +
                                                gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                        if (GoodbindingSource.CurrencyManager.Position > -1)
                                        {
                                            gridEX_List.SetValue("tedaddarkartoon",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                            gridEX_List.SetValue("tedaddarbaste",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                            gridEX_List.SetValue("column03",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                            gridEX_List.SetValue("column16",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());



                                            DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                            this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                            gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                            double amunt = 0;
                                            if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                            {
                                                DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                                if (dr.Count() > 0)
                                                {
                                                    amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                                    gridEX_List.SetValue("column10",
                                                     dr[0].ItemArray[3]);
                                                }
                                            }

                                            if (amunt == Convert.ToDouble(0))
                                            {

                                                gridEX_List.SetValue("column10",
                                 clDoc.ExScalar(ConWare.ConnectionString,
                                       "table_004_CommodityAndIngredients", "column34", "ColumnId",
                                       gridEX_List.GetValue("GoodCode").ToString()));

                                            }
                                            gridEX_List.SetValue("column09",
                                                  0);
                                            gridEX_List.SetValue("column08",
                                               0);

                                            double TotalPrice;
                                            if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                            {
                                                TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                                gridEX_List.SetValue("column11", TotalPrice);
                                                gridEX_List.SetValue("Column16", 0);
                                                gridEX_List.SetValue("Column18", 0);
                                                gridEX_List.SetValue("column17", 0);
                                                gridEX_List.SetValue("Column19", 0);
                                                gridEX_List.SetValue("Column40", TotalPrice);
                                                gridEX_List.SetValue("Column20", TotalPrice);
                                            }


                                            gridEX_List.UpdateData();
                                            //محاسبه قیمتهای انتهای فاکتور
                                            txt_TotalPrice.Value = Convert.ToDouble(
                                                gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                AggregateFunction.Sum).ToString());

                                            txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                            txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                            Convert.ToDouble(txt_Extra.Value.ToString()) -
                                            Convert.ToDouble(txt_Reductions.Value.ToString());




                                        }

                                        else
                                        {
                                            Class_BasicOperation.ShowMsg("", "لطفا اطلاعات کالا را به روز رسانی کنید", "Warning");
                                            return;
                                        }  

                                    }

                                }

                                else
                                {
                                    Class_BasicOperation.ShowMsg("", "کد کالا بارکد وارد شده نامعتبر می باشد", "Warning");
                                    return;

                                }

                            }
                            #endregion

                            #region درصورت وجود نداشتن کالا در لیست
                            else
                            {

                                //Codegenerate = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column19"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                //NumLength = Codegenerate.Length;
                                GoodCode = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column20"])-1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                cmd = clDoc.ExScalar(ConWare.ConnectionString, @"if exists (select * from table_004_CommodityAndIngredients where column06='" + GoodCode + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok");
                                ok = Convert.ToInt32(cmd);

                                if (ok == 1)
                                {
                                    GoodId = clDoc.ExScalar(ConWare.ConnectionString, @"select Columnid from table_004_CommodityAndIngredients where Column06='" + GoodCode + "'");



                                    gridEX_List.MoveToNewRecord();
                                    gridEX_List.SetValue("GoodCode", GoodId);
                                    gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                                    gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                                    gridEX_List.SetValue("column02", GoodId);

                                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                                    {
                                        Con.Open();
                                        SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + GoodId + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                        buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                    }
                                    gridEX_List.SetValue("Column41", buyprice);
                                    {
                                        GoodbindingSource.Filter = "GoodID=" +
                                            gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                        if (GoodbindingSource.CurrencyManager.Position > -1)
                                        {
                                            gridEX_List.SetValue("tedaddarkartoon",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                            gridEX_List.SetValue("tedaddarbaste",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                            gridEX_List.SetValue("column03",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                            gridEX_List.SetValue("column16",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());

                                            DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                            this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                            gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                            if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                            {
                                                DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                                if (dr.Count() > 0)
                                                {
                                                    amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                                    gridEX_List.SetValue("column10",
                                                     dr[0].ItemArray[3]);
                                                }
                                            }

                                            if (amunt == Convert.ToDouble(0))
                                            {

                                                gridEX_List.SetValue("column10",
                                 clDoc.ExScalar(ConWare.ConnectionString,
                                       "table_004_CommodityAndIngredients", "column34", "ColumnId",
                                       gridEX_List.GetValue("GoodCode").ToString()));

                                            }
                                            gridEX_List.SetValue("column09",
                                                  0);
                                            gridEX_List.SetValue("column08",
                                               0);
                                            double TotalPrice;
                                            if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                            {
                                                TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                                gridEX_List.SetValue("column11", TotalPrice);
                                                gridEX_List.SetValue("Column16", 0);
                                                gridEX_List.SetValue("Column18", 0);
                                                gridEX_List.SetValue("column17", 0);
                                                gridEX_List.SetValue("Column19", 0);
                                                gridEX_List.SetValue("Column40", TotalPrice);
                                                gridEX_List.SetValue("Column20", TotalPrice);
                                            }

                                            gridEX_List.UpdateData();

                                            txt_TotalPrice.Value = Convert.ToDouble(
                                                gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                AggregateFunction.Sum).ToString());
                                            txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                            txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                            Convert.ToDouble(txt_Extra.Value.ToString()) -
                                            Convert.ToDouble(txt_Reductions.Value.ToString());



                                        }

                                        else
                                        {
                                            Class_BasicOperation.ShowMsg("", "لطفا اطلاعات کالا را به روز رسانی کنید", "Warning");
                                            return;
                                        }  

                                    }
                                }

                                else
                                {
                                    Class_BasicOperation.ShowMsg("", "کد کالا بارکد وارد شده نامعتبر می باشد", "Warning");

                                }

                            }
                            #endregion
                        }



                      

                        else  if (dtBarcode.Rows[0]["Column16"].ToString() == "True" && dtBarcode.Rows[0]["Column17"].ToString() != "True")
                            {

                                if (gridEX_List.GetRows().Count() > 0)
                                {

                                    #region  درصورت استفاده از مقدار بارکد و موجود بودن آن در لیست

                                     //Codegenerate = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column19"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                     //NumLength = Codegenerate.Length;
                                    GoodCode = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column20"])-1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                    cmd = clDoc.ExScalar(ConWare.ConnectionString, @"if exists (select * from table_004_CommodityAndIngredients where column06='" + GoodCode + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok");
                                    ok =Convert.ToInt32(cmd);

                                    if (ok==1)
                                    {
                                        
                                     GoodId = clDoc.ExScalar(ConWare.ConnectionString, @"select Columnid from table_004_CommodityAndIngredients where Column06='" + GoodCode + "'");

                                     IntNum = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column23"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column21"]));
                                     FloatNum = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column26"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column25"]));
                                     TotalNum = Convert.ToDecimal(IntNum + "." + FloatNum).ToString();

                                    string goodcode;
                                    Int16 unit;
                                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                                    {
                                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                                        {
                                            Con.Open();
                                            SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                            goodcode = (Comm.ExecuteScalar().ToString());
                                            Comm = new SqlCommand("SELECT tcc.column07 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                            unit = Convert.ToInt16(Comm.ExecuteScalar());
                                        }


                                        if (goodcode == GoodCode && unit == Convert.ToInt16(item.Cells["column03"].Value) && Convert.ToBoolean(item.Cells["column30"].Value) == false)
                                        {

                                            isthere = true;
                                            item.BeginEdit();



                                            item.Cells["Column07"].Value = Convert.ToDecimal(item.Cells["Column07"].Value.ToString()) + Convert.ToDecimal(TotalNum);
                                            item.Cells["Column06"].Value = Convert.ToDecimal(item.Cells["Column06"].Value.ToString()) + Convert.ToDecimal(TotalNum);



                                            item.EndEdit();
                                            double TotalPrice;
                                            if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                            {
                                                TotalPrice = Convert.ToDouble(item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(item.Cells["Column07"].Value.ToString());
                                                item.BeginEdit();
                                                item.Cells["column11"].Value = TotalPrice;
                                                item.Cells["column16"].Value = 0;
                                                item.Cells["column18"].Value = 0;
                                                item.Cells["column17"].Value = 0;
                                                item.Cells["Column19"].Value = 0;
                                                item.Cells["Column40"].Value = TotalPrice;
                                                item.Cells["Column20"].Value = TotalPrice;

                                                item.EndEdit();

                                            }
                                            gridEX_List.UpdateData();

                                            //محاسبه قیمتهای انتهای فاکتور
                                            txt_TotalPrice.Value = Convert.ToDouble(
                                                gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                AggregateFunction.Sum).ToString());

                                            txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                            txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                            Convert.ToDouble(txt_Extra.Value.ToString()) -
                                            Convert.ToDouble(txt_Reductions.Value.ToString());



                                            break;

                                        }

                                    }
                                    if (!isthere)
                                    {

                                        gridEX_List.MoveToNewRecord();
                                        gridEX_List.SetValue("GoodCode", GoodId);
                                        gridEX_List.SetValue("Column07", TotalNum);
                                        gridEX_List.SetValue("Column06", TotalNum);
                                        gridEX_List.SetValue("column02", GoodId);

                                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                                        {
                                            Con.Open();
                                            SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + GoodId + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                            buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                        }
                                        gridEX_List.SetValue("Column41", buyprice);



                                        GoodbindingSource.Filter = "GoodID=" +
                                                gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                        if (GoodbindingSource.CurrencyManager.Position > -1)
                                        {
                                            gridEX_List.SetValue("tedaddarkartoon",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                            gridEX_List.SetValue("tedaddarbaste",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                            gridEX_List.SetValue("column03",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                            gridEX_List.SetValue("column16",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());



                                            DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                            this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                            gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                            double amunt = 0;
                                            if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                            {
                                                DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                                if (dr.Count() > 0)
                                                {
                                                    amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                                    gridEX_List.SetValue("column10",
                                                     dr[0].ItemArray[3]);
                                                }
                                            }

                                            if (amunt == Convert.ToDouble(0))
                                            {

                                                gridEX_List.SetValue("column10",
                                 clDoc.ExScalar(ConWare.ConnectionString,
                                       "table_004_CommodityAndIngredients", "column34", "ColumnId",
                                       gridEX_List.GetValue("GoodCode").ToString()));

                                            }
                                            gridEX_List.SetValue("column09",
                                                  0);
                                            gridEX_List.SetValue("column08",
                                               0);

                                            double TotalPrice;
                                            if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                            {
                                                TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                                gridEX_List.SetValue("column11", TotalPrice);
                                                gridEX_List.SetValue("Column16", 0);
                                                gridEX_List.SetValue("Column18", 0);
                                                gridEX_List.SetValue("column17", 0);
                                                gridEX_List.SetValue("Column19", 0);
                                                gridEX_List.SetValue("Column40", TotalPrice);
                                                gridEX_List.SetValue("Column20", TotalPrice);
                                            }


                                            gridEX_List.UpdateData();
                                            //محاسبه قیمتهای انتهای فاکتور
                                            txt_TotalPrice.Value = Convert.ToDouble(
                                                gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                AggregateFunction.Sum).ToString());

                                            txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                            txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                            Convert.ToDouble(txt_Extra.Value.ToString()) -
                                            Convert.ToDouble(txt_Reductions.Value.ToString());




                                        }
                                        else
                                        {
                                            Class_BasicOperation.ShowMsg("", "لطفا اطلاعات کالا را به روز رسانی کنید", "Warning");
                                            return;
                                        }  


                                    }



                                    }
                                    else
                                    {
                                        Class_BasicOperation.ShowMsg("", "کد کالا بارکد وارد شده نامعتبر می باشد", "Warning");

                                    }

                                #endregion

                                }

                                else
                                {

                                    #region درصورت استفاده از مقدار بارکد و موجود نداشتن آن در لیست

                                     //Codegenerate = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column19"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                     //NumLength = Codegenerate.Length;
                                    GoodCode = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column20"])-1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                    cmd = clDoc.ExScalar(ConWare.ConnectionString, @"if exists (select * from table_004_CommodityAndIngredients where column06='" + GoodCode + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok");
                                    ok =Convert.ToInt32(cmd);

                                    if (ok==1)
                                    {
                                     GoodId = clDoc.ExScalar(ConWare.ConnectionString, @"select Columnid from table_004_CommodityAndIngredients where Column06='" + GoodCode + "'");
                                     IntNum = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column23"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column21"]));
                                     FloatNum = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column26"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column25"]));
                                     TotalNum = Convert.ToDecimal(IntNum + "." + FloatNum).ToString();


                                    gridEX_List.MoveToNewRecord();
                                    gridEX_List.SetValue("GoodCode", GoodId);
                                    gridEX_List.SetValue("Column07", TotalNum);
                                    gridEX_List.SetValue("Column06", TotalNum);
                                    gridEX_List.SetValue("column02", GoodId);

                                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                                    {
                                        Con.Open();
                                        SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + GoodId + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                        buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                    }
                                    gridEX_List.SetValue("Column41", buyprice);
                                    {
                                        GoodbindingSource.Filter = "GoodID=" +
                                            gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                        if (GoodbindingSource.CurrencyManager.Position > -1)
                                        {
                                            gridEX_List.SetValue("tedaddarkartoon",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                            gridEX_List.SetValue("tedaddarbaste",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                            gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                            gridEX_List.SetValue("column03",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                            gridEX_List.SetValue("column16",
                                                ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());

                                            DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                            this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                            gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                            double amunt = 0;
                                            if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                            {
                                                DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                                if (dr.Count() > 0)
                                                {
                                                    amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                                    gridEX_List.SetValue("column10",
                                                     dr[0].ItemArray[3]);
                                                }
                                            }

                                            if (amunt == Convert.ToDouble(0))
                                            {

                                                gridEX_List.SetValue("column10",
                                 clDoc.ExScalar(ConWare.ConnectionString,
                                       "table_004_CommodityAndIngredients", "column34", "ColumnId",
                                       gridEX_List.GetValue("GoodCode").ToString()));

                                            }
                                            gridEX_List.SetValue("column09",
                                                  0);
                                            gridEX_List.SetValue("column08",
                                               0);
                                            double TotalPrice;
                                            if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                            {
                                                TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                                gridEX_List.SetValue("column11", TotalPrice);
                                                gridEX_List.SetValue("Column16", 0);
                                                gridEX_List.SetValue("Column18", 0);
                                                gridEX_List.SetValue("column17", 0);
                                                gridEX_List.SetValue("Column19", 0);
                                                gridEX_List.SetValue("Column40", TotalPrice);
                                                gridEX_List.SetValue("Column20", TotalPrice);
                                            }

                                            gridEX_List.UpdateData();

                                            txt_TotalPrice.Value = Convert.ToDouble(
                                                gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                AggregateFunction.Sum).ToString());
                                            txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                            txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                            Convert.ToDouble(txt_Extra.Value.ToString()) -
                                            Convert.ToDouble(txt_Reductions.Value.ToString());



                                        }

                                        else
                                        {
                                            Class_BasicOperation.ShowMsg("", "لطفا اطلاعات کالا را به روز رسانی کنید", "Warning");
                                            return;
                                        }  
                                    }

                                    }
                                   
                                    else
                                    {
                                        Class_BasicOperation.ShowMsg("", "کد کالا بارکد وارد شده نامعتبر می باشد", "Warning");

                                    }
                                    #endregion
                                }
  
                                    
                                   
                            }

                            else if (dtBarcode.Rows[0]["Column16"].ToString() == "True" && dtBarcode.Rows[0]["Column17"].ToString() == "True")
                            {
                                if (gridEX_List.GetRows().Count() > 0)
                                {

                                    #region   استفاده از هم مقدار هم مبلغ بودن در صورت وجود داشتن در لیست

                                     //Codegenerate = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column19"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                     //NumLength = Codegenerate.Length;
                                    GoodCode = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column20"])-1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                    cmd = clDoc.ExScalar(ConWare.ConnectionString, @"if exists (select * from table_004_CommodityAndIngredients where column06='" + GoodCode + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok");
                                    ok = Convert.ToInt32(cmd);

                                    if (ok == 1)
                                    {

                                        GoodId = clDoc.ExScalar(ConWare.ConnectionString, @"select Columnid from table_004_CommodityAndIngredients where Column06='" + GoodCode + "'");

                                        SinglePrice = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column24"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column22"]));

                                        IntNum = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column23"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column21"]));

                                        FloatNum = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column26"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column25"]));

                                        TotalNum = Convert.ToDecimal(IntNum + "." + FloatNum).ToString();

                                        TotalPrice = Convert.ToDouble(SinglePrice) * Convert.ToDouble(TotalNum);

                                        string goodcode;
                                        Int16 unit;
                                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                                        {
                                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                                            {
                                                Con.Open();
                                                SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                                goodcode = (Comm.ExecuteScalar().ToString());
                                                Comm = new SqlCommand("SELECT tcc.column07 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                                unit = Convert.ToInt16(Comm.ExecuteScalar());
                                             }


                                            if (goodcode == GoodCode && unit == Convert.ToInt16(item.Cells["column03"].Value) && Convert.ToBoolean(item.Cells["column30"].Value) == false)
                                            {

                                                isthere = true;
                                                item.BeginEdit();



                                                item.Cells["Column07"].Value = Convert.ToDecimal(item.Cells["Column07"].Value.ToString()) + Convert.ToDecimal(TotalNum);
                                                item.Cells["Column06"].Value = Convert.ToDecimal(item.Cells["Column06"].Value.ToString()) + Convert.ToDecimal(TotalNum);



                                                item.EndEdit();
                                              
                                                if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                                {
                                                    TotalPrice = Convert.ToDouble(item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(item.Cells["Column07"].Value.ToString());
                                                    item.BeginEdit();
                                                    item.Cells["column11"].Value = TotalPrice;
                                                    item.Cells["column16"].Value = 0;
                                                    item.Cells["column18"].Value = 0;
                                                    item.Cells["column17"].Value = 0;
                                                    item.Cells["Column19"].Value = 0;
                                                    item.Cells["Column40"].Value = TotalPrice;
                                                    item.Cells["Column20"].Value = TotalPrice;

                                                    item.EndEdit();

                                                }
                                                gridEX_List.UpdateData();

                                                //محاسبه قیمتهای انتهای فاکتور
                                                txt_TotalPrice.Value = Convert.ToDouble(
                                                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                    AggregateFunction.Sum).ToString());

                                                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                                Convert.ToDouble(txt_Extra.Value.ToString()) -
                                                Convert.ToDouble(txt_Reductions.Value.ToString());



                                                break;

                                            }

                                        }
                                        if (!isthere)
                                        {

                                            gridEX_List.MoveToNewRecord();
                                            gridEX_List.SetValue("GoodCode", GoodId);
                                            gridEX_List.SetValue("Column07", TotalNum);
                                            gridEX_List.SetValue("Column06", TotalNum);
                                            gridEX_List.SetValue("column02", GoodId);

                                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                                            {
                                                Con.Open();
                                                SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + GoodId + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                                buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                            }
                                            gridEX_List.SetValue("Column41", buyprice);



                                            GoodbindingSource.Filter = "GoodID=" +
                                                    gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                            if (GoodbindingSource.CurrencyManager.Position > -1)
                                            {
                                                gridEX_List.SetValue("tedaddarkartoon",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                                gridEX_List.SetValue("tedaddarbaste",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                                gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                                gridEX_List.SetValue("column03",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                                gridEX_List.SetValue("column16",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());



                                                DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                                this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                                gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                                double amunt = 0;
                                                if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                                {
                                                    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                                    if (dr.Count() > 0)
                                                    {
                                                        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                                        gridEX_List.SetValue("column10",
                                                         dr[0].ItemArray[3]);
                                                    }
                                                }

                                                if (amunt == Convert.ToDouble(0))
                                                {

                                                    gridEX_List.SetValue("column10",
                                     clDoc.ExScalar(ConWare.ConnectionString,
                                           "table_004_CommodityAndIngredients", "column34", "ColumnId",
                                           gridEX_List.GetValue("GoodCode").ToString()));

                                                }
                                                gridEX_List.SetValue("column09",
                                                      0);
                                                gridEX_List.SetValue("column08",
                                                   0);

                                                if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                                {
                                                    TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                                    gridEX_List.SetValue("column11", TotalPrice);
                                                    gridEX_List.SetValue("Column16", 0);
                                                    gridEX_List.SetValue("Column18", 0);
                                                    gridEX_List.SetValue("column17", 0);
                                                    gridEX_List.SetValue("Column19", 0);
                                                    gridEX_List.SetValue("Column40", TotalPrice);
                                                    gridEX_List.SetValue("Column20", TotalPrice);
                                                }


                                                gridEX_List.UpdateData();
                                                //محاسبه قیمتهای انتهای فاکتور
                                                txt_TotalPrice.Value = Convert.ToDouble(
                                                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                    AggregateFunction.Sum).ToString());

                                                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                                Convert.ToDouble(txt_Extra.Value.ToString()) -
                                                Convert.ToDouble(txt_Reductions.Value.ToString());




                                            }

                                            else
                                            {
                                                Class_BasicOperation.ShowMsg("", "لطفا اطلاعات کالا را به روز رسانی کنید", "Warning");
                                                return;
                                            }  

                                        }



                                    }
                                    else
                                    {
                                        Class_BasicOperation.ShowMsg("", "کد کالا بارکد وارد شده نامعتبر می باشد", "Warning");

                                    }

                                    #endregion

                                }

                                else
                                {

                                    #region استفاده از هم مقدار هم مبلغ بودن در صورت وجود نداشتن در لیست

                                 //   Codegenerate = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column19"]), Convert.ToInt32(dtBarcode.Rows[0]["Column18"]));
                                 //NumLength = Codegenerate.Length;
                                    GoodCode = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column20"])-1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                cmd = clDoc.ExScalar(ConWare.ConnectionString, @"if exists (select * from table_004_CommodityAndIngredients where column06='" + GoodCode + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok");
                                    ok =Convert.ToInt32(cmd);

                                    if (ok == 1)
                                    {
                                         GoodId = clDoc.ExScalar(ConWare.ConnectionString, @"select Columnid from table_004_CommodityAndIngredients where Column06='" + GoodCode + "'");

                                         SinglePrice = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column24"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column22"]));

                                         IntNum = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column23"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column21"]));

                                         FloatNum = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column26"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column25"]));

                                         TotalNum = Convert.ToDecimal(IntNum + "." + FloatNum).ToString();

                                         TotalPrice = Convert.ToDouble(SinglePrice) * Convert.ToDouble(TotalNum);


                                         gridEX_List.MoveToNewRecord();
                                         gridEX_List.SetValue("GoodCode", GoodId);

                                         gridEX_List.SetValue("column02", GoodId);

                                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                                        {
                                            Con.Open();
                                            SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + GoodId + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                            buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                        }
                                        gridEX_List.SetValue("Column41", buyprice);
                                        {
                                            GoodbindingSource.Filter = "GoodID=" +
                                                gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                            if (GoodbindingSource.CurrencyManager.Position > -1)
                                            {
                                                gridEX_List.SetValue("tedaddarkartoon",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                                gridEX_List.SetValue("tedaddarbaste",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                                gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                                gridEX_List.SetValue("column03",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                                gridEX_List.SetValue("column16",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());

                                                DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                                this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                                gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");



                                                gridEX_List.SetValue("column09",
                                                      0);
                                                gridEX_List.SetValue("column08",
                                                   0);

                                                gridEX_List.SetValue("column10", SinglePrice);
                                                gridEX_List.SetValue("column11", TotalPrice);
                                                gridEX_List.SetValue("Column16", 0);
                                                gridEX_List.SetValue("Column18", 0);
                                                gridEX_List.SetValue("column17", 0);
                                                gridEX_List.SetValue("Column19", 0);
                                                gridEX_List.SetValue("Column40", TotalPrice);
                                                gridEX_List.SetValue("Column20", TotalPrice);


                                                gridEX_List.UpdateData();

                                                txt_TotalPrice.Value = Convert.ToDouble(
                                                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                    AggregateFunction.Sum).ToString());
                                                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                                Convert.ToDouble(txt_Extra.Value.ToString()) -
                                                Convert.ToDouble(txt_Reductions.Value.ToString());



                                            }

                                            else
                                            {
                                                Class_BasicOperation.ShowMsg("", "لطفا اطلاعات کالا را به روز رسانی کنید", "Warning");
                                                return;
                                            }  

                                        }
                                    }
                                    else
                                    {
                                        Class_BasicOperation.ShowMsg("", "کد کالا بارکد وارد شده نامعتبر می باشد", "Warning");

                                    }
                                #endregion
                               
                                }
                            }

                            else if (dtBarcode.Rows[0]["Column16"].ToString() != "True" && dtBarcode.Rows[0]["Column17"].ToString() == "True")
                            {

                               

                                if (gridEX_List.GetRows().Count() > 0)
                                {

                                    #region   استفاده از  مبلغ  در صورت وجود داشتن در لیست

                                    //string Codegenerate = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column19"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                    //int NumLength = Codegenerate.Length;
                                    GoodCode = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column20"])-1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                    cmd = clDoc.ExScalar(ConWare.ConnectionString, @"if exists (select * from table_004_CommodityAndIngredients where column06='" + GoodCode + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok");
                                    ok = Convert.ToInt32(cmd);

                                    if (ok == 1)
                                    {

                                        GoodId = clDoc.ExScalar(ConWare.ConnectionString, @"select Columnid from table_004_CommodityAndIngredients where Column06='" + GoodCode + "'");

                                        SinglePrice = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column24"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column22"]));

                                   

                                        string goodcode;
                                        Int16 unit;
                                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                                        {
                                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                                            {
                                                Con.Open();
                                                SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                                goodcode = (Comm.ExecuteScalar().ToString());
                                                Comm = new SqlCommand("SELECT tcc.column07 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                                unit = Convert.ToInt16(Comm.ExecuteScalar());
                                            }


                                            if (goodcode == GoodCode && unit == Convert.ToInt16(item.Cells["column03"].Value) && Convert.ToBoolean(item.Cells["column30"].Value) == false)
                                            {

                                                isthere = true;
                                                item.BeginEdit();


                                                item.EndEdit();


                                                if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                                {
                                                    item.BeginEdit();
                                                    item.Cells["column11"].Value = Convert.ToDecimal(item.Cells["Column11"].Value.ToString()) + Convert.ToDecimal(SinglePrice);
                                                    item.Cells["column16"].Value = 0;
                                                    item.Cells["column18"].Value = 0;
                                                    item.Cells["column17"].Value = 0;
                                                    item.Cells["Column19"].Value = 0;
                                                    item.Cells["Column40"].Value = SinglePrice;
                                                    item.Cells["Column20"].Value = SinglePrice;
                                                    UnitNumer = Convert.ToDecimal((Convert.ToDecimal(item.Cells["Column11"].Value) / Convert.ToDecimal(item.Cells["Column10"].Value))).ToString();
                                                    item.Cells["Column07"].Value =  Convert.ToDecimal(UnitNumer);
                                                    item.Cells["Column06"].Value = Convert.ToDecimal(UnitNumer);

                                                    item.EndEdit();


                                                }



                                              
                                                gridEX_List.UpdateData();

                                                //محاسبه قیمتهای انتهای فاکتور
                                                txt_TotalPrice.Value = Convert.ToDouble(
                                                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                    AggregateFunction.Sum).ToString());

                                                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                                Convert.ToDouble(txt_Extra.Value.ToString()) -
                                                Convert.ToDouble(txt_Reductions.Value.ToString());



                                                break;

                                            }

                                        }
                                        if (!isthere)
                                        {

                                            gridEX_List.MoveToNewRecord();
                                            gridEX_List.SetValue("GoodCode", GoodId);

                                            gridEX_List.SetValue("column02", GoodId);

                                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                                            {
                                                Con.Open();
                                                SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + GoodId + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                                buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                            }
                                            gridEX_List.SetValue("Column41", buyprice);



                                            GoodbindingSource.Filter = "GoodID=" +
                                                    gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                            if (GoodbindingSource.CurrencyManager.Position > -1)
                                            {
                                                gridEX_List.SetValue("tedaddarkartoon",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                                gridEX_List.SetValue("tedaddarbaste",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                                gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                                gridEX_List.SetValue("column03",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                                gridEX_List.SetValue("column16",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());



                                                DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                                this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                                gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                                double amunt = 0;
                                                if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                                {
                                                    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                                    if (dr.Count() > 0)
                                                    {
                                                        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                                        gridEX_List.SetValue("column10",
                                                         dr[0].ItemArray[3]);
                                                    }
                                                }

                                                if (amunt == Convert.ToDouble(0))
                                                {

                                                    gridEX_List.SetValue("column10",
                                     clDoc.ExScalar(ConWare.ConnectionString,
                                           "table_004_CommodityAndIngredients", "column34", "ColumnId",
                                           gridEX_List.GetValue("GoodCode").ToString()));

                                                }
                                                if (gridEX_List.GetValue("Column10").ToString() == "" || gridEX_List.GetValue("Column10").ToString() == "0")
                                                {
                                                    Class_BasicOperation.ShowMsg("", "برای این بارکد قیمت فروش تعریف نشده است لطفا ابتدا قیمت آن را تعیین کنید", "Warning");
                                                    return;
                                                }
                                                gridEX_List.SetValue("column09",
                                                      0);
                                                gridEX_List.SetValue("column08",
                                                   0);

                                                
                                                UnitNumer = Convert.ToDecimal((Convert.ToDecimal(SinglePrice) / Convert.ToDecimal(gridEX_List.GetValue("Column10")))).ToString();
                                                gridEX_List.SetValue("Column07", UnitNumer);
                                                gridEX_List.SetValue("Column06", UnitNumer);
                                                    gridEX_List.SetValue("column11", SinglePrice);
                                                    gridEX_List.SetValue("Column16", 0);
                                                    gridEX_List.SetValue("Column18", 0);
                                                    gridEX_List.SetValue("column17", 0);
                                                    gridEX_List.SetValue("Column19", 0);
                                                    gridEX_List.SetValue("Column40", SinglePrice);
                                                    gridEX_List.SetValue("Column20", SinglePrice);
                                              


                                                gridEX_List.UpdateData();
                                                //محاسبه قیمتهای انتهای فاکتور
                                                txt_TotalPrice.Value = Convert.ToDouble(
                                                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                    AggregateFunction.Sum).ToString());

                                                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                                Convert.ToDouble(txt_Extra.Value.ToString()) -
                                                Convert.ToDouble(txt_Reductions.Value.ToString());




                                            }
                                            else
                                            {
                                                Class_BasicOperation.ShowMsg("", "لطفا اطلاعات کالا را به روز رسانی کنید", "Warning");
                                                return;
                                            }  


                                        }



                                    }
                                    else
                                    {
                                        Class_BasicOperation.ShowMsg("", "کد کالا بارکد وارد شده نامعتبر می باشد", "Warning");

                                    }

                                    #endregion

                                }

                                else
                                {

                                    #region استفاده  مبلغ در صورت وجود نداشتن در لیست

                                    //Codegenerate = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column19"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                    //NumLength = Codegenerate.Length;
                                    GoodCode = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column20"])-1, Convert.ToInt32(dtBarcode.Rows[0]["Column19"]));
                                    cmd = clDoc.ExScalar(ConWare.ConnectionString, @"if exists (select * from table_004_CommodityAndIngredients where column06='" + GoodCode + @"')

                                                    select 1 as ok
                                                    else
                                                    select 0 as ok");
                                    ok = Convert.ToInt32(cmd);

                                    if (ok == 1)
                                    {
                                        GoodId = clDoc.ExScalar(ConWare.ConnectionString, @"select Columnid from table_004_CommodityAndIngredients where Column06='" + GoodCode + "'");

                                        SinglePrice = txt_GoodCode.Text.Substring(Convert.ToInt32(dtBarcode.Rows[0]["Column24"]) - 1, Convert.ToInt32(dtBarcode.Rows[0]["Column22"]));

                                     


                                        gridEX_List.MoveToNewRecord();
                                        gridEX_List.SetValue("GoodCode", GoodId);
                                       
                                        gridEX_List.SetValue("column02", GoodId);

                                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                                        {
                                            Con.Open();
                                            SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + GoodId + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                            buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                        }
                                        gridEX_List.SetValue("Column41", buyprice);
                                        {
                                            GoodbindingSource.Filter = "GoodID=" +
                                                gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                            if (GoodbindingSource.CurrencyManager.Position > -1)
                                            {
                                                gridEX_List.SetValue("tedaddarkartoon",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                                gridEX_List.SetValue("tedaddarbaste",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                                gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                                gridEX_List.SetValue("column03",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                                gridEX_List.SetValue("column16",
                                                    ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());

                                                DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                                this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                                gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");

                                                if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                                {
                                                    DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                                    if (dr.Count() > 0)
                                                    {
                                                        amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                                        gridEX_List.SetValue("column10",
                                                         dr[0].ItemArray[3]);
                                                    }
                                                }

                                                if (amunt == Convert.ToDouble(0))
                                                {
                                                   
                                                    gridEX_List.SetValue("column10",
                                     clDoc.ExScalar(ConWare.ConnectionString,
                                           "table_004_CommodityAndIngredients", "column34", "ColumnId",
                                           gridEX_List.GetValue("GoodCode").ToString()));

                                                }

                                                gridEX_List.SetValue("column09",
                                                      0);
                                                gridEX_List.SetValue("column08",
                                                   0);

                                                UnitNumer = Convert.ToDecimal((Convert.ToDecimal(SinglePrice) / Convert.ToDecimal(gridEX_List.GetValue("Column10")))).ToString();
                                                gridEX_List.SetValue("Column07", UnitNumer);
                                                gridEX_List.SetValue("Column06", UnitNumer);
                                                gridEX_List.SetValue("column11", SinglePrice);
                                                gridEX_List.SetValue("Column16", 0);
                                                gridEX_List.SetValue("Column18", 0);
                                                gridEX_List.SetValue("column17", 0);
                                                gridEX_List.SetValue("Column19", 0);
                                                gridEX_List.SetValue("Column40", SinglePrice);
                                                gridEX_List.SetValue("Column20", SinglePrice);


                                                gridEX_List.UpdateData();

                                                txt_TotalPrice.Value = Convert.ToDouble(
                                                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                                    AggregateFunction.Sum).ToString());
                                                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                                Convert.ToDouble(txt_Extra.Value.ToString()) -
                                                Convert.ToDouble(txt_Reductions.Value.ToString());



                                            }

                                            else
                                            {
                                                Class_BasicOperation.ShowMsg("", "لطفا اطلاعات کالا را به روز رسانی کنید", "Warning");
                                                return;
                                            }  


                                        }
                                    }
                                    else
                                    {
                                        Class_BasicOperation.ShowMsg("", "کد کالا بارکد وارد شده نامعتبر می باشد", "Warning");

                                    }
                                    #endregion

                                }




                            }

                        }

                        #endregion
                    }

                }

            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Index and length must") || ex.Message.StartsWith("startIndex cannot be larger"))
                {
                    Class_BasicOperation.ShowMsg("", "اندازه طول بارکد با تنظیمات پیش فرض مغایرت دارد", "Warning");
                 
                }

            }
            addnew();

        }

        private void txt_GoodCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                InitialNewRow();
            }


            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
        }

        private void gridEX_List_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
                if (txt_GoodCode.Text == string.Empty)
                {
                    long codeid = 0;

                    string txt_GoodCode1 = string.Empty;

                    {
                        if (gridEX_List.GetRows().Count() > 0)
                        {
                            Int16 unit = 0;

                            if (gridEX_List.GetValue("column02").ToString() != string.Empty)
                            {
                                codeid = Convert.ToInt64(gridEX_List.GetValue("column02").ToString());
                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                                {
                                    Con.Open();
                                    SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + codeid + " ", Con);
                                    txt_GoodCode1 = (Comm.ExecuteScalar()).ToString();
                                    Comm = new SqlCommand("SELECT tcc.column07 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + codeid + "", Con);
                                    unit = Convert.ToInt16(Comm.ExecuteScalar());


                                }

                                string goodcode;
                                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                                {
                                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                                    {
                                        Con.Open();
                                        SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                        goodcode = (Comm.ExecuteScalar().ToString());
                                    }


                                    if (goodcode == txt_GoodCode1 && unit == Convert.ToInt16(item.Cells["column03"].Value) && Convert.ToBoolean(item.Cells["column30"].Value) == false)
                                    {

                                        item.BeginEdit();

                                        //float h = clDoc.GetZarib(Convert.ToInt32(codeid), Convert.ToInt16(item.Cells["column03"].Value));
                                        //item.Cells["Column07"].Value = (float.Parse(this.txt_Count.Text) * h) + Convert.ToDouble(item.Cells["Column07"].Value);
                                        //item.Cells["Column06"].Value = (float.Parse(this.txt_Count.Text) * h) + Convert.ToDouble(item.Cells["Column06"].Value);


                                        item.Cells["Column07"].Value = Convert.ToInt32(item.Cells["Column07"].Value.ToString()) + Convert.ToInt32(gridEX_List.GetValue("Column07"));
                                        item.Cells["Column06"].Value = Convert.ToInt32(item.Cells["Column06"].Value.ToString()) + Convert.ToInt32(gridEX_List.GetValue("Column06"));

                                        item.EndEdit();
                                        double TotalPrice;
                                        if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                        {
                                            TotalPrice = Convert.ToDouble(item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(item.Cells["Column07"].Value.ToString());
                                            item.BeginEdit();
                                            item.Cells["column11"].Value = TotalPrice;
                                            item.Cells["column16"].Value = 0;
                                            item.Cells["column18"].Value = 0;
                                            item.Cells["column17"].Value = 0;
                                            item.Cells["Column19"].Value = 0;
                                            item.Cells["Column40"].Value = TotalPrice;
                                            item.Cells["Column20"].Value = TotalPrice;

                                            item.EndEdit();

                                        }
                                        e.Cancel = true;
                                        gridEX_List.CancelCurrentEdit();
                                        //محاسبه قیمتهای انتهای فاکتور
                                        txt_TotalPrice.Value = Convert.ToDouble(
                                            gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                            AggregateFunction.Sum).ToString());

                                        txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                        txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                        Convert.ToDouble(txt_Extra.Value.ToString()) -
                                        Convert.ToDouble(txt_Reductions.Value.ToString());



                                    }


                                }

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("اضافه کردن کالا با خطا مواجه شد.شرح خطا" + ex.Message);
            }


        }


        private void gridEX_List_RecordAdded(object sender, EventArgs e)
        {
            try
            {
                txt_TotalPrice.Value = Convert.ToDouble(
                       gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                       AggregateFunction.Sum).ToString());
                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                    Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("اضافه کردن کالا با خطا مواجه شد.شرح خطا" + ex.Message);
            }

        }

        private void txt_GoodCode_Enter(object sender, EventArgs e)
        {
            original = InputLanguage.CurrentInputLanguage;
            var culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
            var language = InputLanguage.FromCulture(culture);
            if (InputLanguage.InstalledInputLanguages.IndexOf(language) >= 0)
                InputLanguage.CurrentInputLanguage = language;
            else
                InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage;
            try
            {
                if (table_010_SaleFactorBindingSource.Count > 0)
                {

                    if (txt_date.Text == null || txt_date.Text == "")
                    {
                        MessageBox.Show("اطلاعات تاریخ معتبر نمی باشد");

                        return;
                    }
                    else if (mlt_SaleType.Value == null || mlt_SaleType.Value == DBNull.Value || mlt_SaleType.Text == "" || mlt_SaleType.Text == "0")
                    {
                        MessageBox.Show("اطلاعات نوع فروش معتبر نمی باشد");

                        return;
                    }

                    else if (mlt_Customer.Value == null || mlt_Customer.Value == DBNull.Value || mlt_Customer.Text == "" || mlt_Customer.Text == "0")
                    {
                        MessageBox.Show("اطلاعات خریدار معتبر نمی باشد");

                        return;
                    }
                    //setnull();
                    table_010_SaleFactorBindingSource.EndEdit();
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void txt_GoodCode_Leave(object sender, EventArgs e)
        {


            var culture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");
            var language = InputLanguage.FromCulture(culture);
            InputLanguage.CurrentInputLanguage = language;

        }

        private void gridEX_List_CurrentCellChanging(object sender, CurrentCellChangingEventArgs e)
        {
            if (e.Column != null)
            {
                original = InputLanguage.CurrentInputLanguage;

                if (e.Column.Key == "GoodCode")
                {
                    var culture = System.Globalization.CultureInfo.GetCultureInfo("en-US");
                    var language = InputLanguage.FromCulture(culture);
                    if (InputLanguage.InstalledInputLanguages.IndexOf(language) >= 0)
                        InputLanguage.CurrentInputLanguage = language;
                    else
                        InputLanguage.CurrentInputLanguage = InputLanguage.DefaultInputLanguage;
                }


                else
                {
                    var culture = System.Globalization.CultureInfo.GetCultureInfo("fa-IR");
                    var language = InputLanguage.FromCulture(culture);
                    InputLanguage.CurrentInputLanguage = language;
                }

            }


        }

        private void btn_person_Click(object sender, EventArgs e)
        {
            if (mlt_Customer.Value != null && !string.IsNullOrWhiteSpace(mlt_Customer.Value.ToString()))
                try
                {
                    PACNT.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.ACNT;
                    PACNT.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                    PACNT.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                    PACNT.Class_BasicOperation._FinYear = Class_BasicOperation._Year;

                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column08", 40))
                    {
                        System.Globalization.PersianCalendar pc = new System.Globalization.PersianCalendar();
                        DateTime dt = new DateTime(Convert.ToInt32(FarsiLibrary.Utils.PersianDate.Now.Year),
                               Convert.ToInt32(1),
                               Convert.ToInt32(1), pc);
                        PACNT._4_Person_Menu.Form01_PersonOperationList frm = new PACNT._4_Person_Menu.Form01_PersonOperationList
                            (mlt_Customer.Value.ToString(), dt, Class_BasicOperation.ServerDate());

                        frm.ShowDialog();



                    }
                    else
                        Class_BasicOperation.ShowMsg("",
                            "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
                }
                catch
                {
                }
        }



        private void mlt_Customer_KeyPress(object sender, KeyPressEventArgs e)
        {

            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    //Class_BasicOperation.isEnter(e.KeyChar);
                    txt_GoodCode.Select();
            }


            //if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            //{
            //    if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
            //        ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            //    else
            //        txt_GoodCode.Select();
            //}
            //else
            //    txt_GoodCode.Select();
        }

        private void mlt_Customer_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "name", "code");

        }

        private void mlt_Customer_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void btn_addtax_Click(object sender, EventArgs e)
        {
            try
            {
                if (table_010_SaleFactorBindingSource.Count > 0)
                {
                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();

                    if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", RowID.ToString()) != 0)
                        throw new Exception("به علت ارجاع این فاکتور، ویرایش اضافات و کسورات امکانپذیر نمی باشد");



                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                    {
                        Con.Open();
                        SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL(Column45,0) from Table_010_SaleFactor where ColumnId=" + RowID + "),0)", Con);
                        if (Convert.ToBoolean((Comm.ExecuteScalar().ToString())))
                            throw new Exception("به علت تسویه این فاکتور، ویرایش اضافات و کسورات امکانپذیر نمی باشد");

                        Comm = new SqlCommand("Select ISNULL((Select ISNULL(Column53,0) from Table_010_SaleFactor where ColumnId=" + RowID + "),0)", Con);
                        if (Convert.ToBoolean((Comm.ExecuteScalar().ToString())))
                            throw new Exception("به علت بسته شدن این فاکتور، ویرایش اضافات و کسورات امکانپذیر نمی باشد");

                    }
                }

                table_010_SaleFactorBindingSource.EndEdit();
                table_011_Child1_SaleFactorBindingSource.EndEdit();
                if (table_010_SaleFactorBindingSource.Count > 0 && table_011_Child1_SaleFactorBindingSource.Count > 0)
                {




                    p_tax.Visible = true;
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }


        }

        private void mlt_Customer_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Customer.Value != null &&
                    !string.IsNullOrWhiteSpace(mlt_Customer.Value.ToString()))
                {

                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
                    {
                        Con.Open();
                        SqlCommand Comm = new SqlCommand("Select ISNULL((Select ISNULL(Column30,0) from Table_045_PersonInfo where ColumnId=" + mlt_Customer.Value + "),0)", Con);
                        if (int.Parse(Comm.ExecuteScalar().ToString()) > 0)
                            mlt_SaleType.Value = int.Parse(Comm.ExecuteScalar().ToString());


                    }

                    txt_buyername.Text = mlt_Customer.Text;
                }
            }
            catch
            {
            }
        }
        private void chehckessentioal()
        {

            discountdt = new DataTable();
            factordt = new DataTable();
            //bahaDT = new DataTable();
            //waredt = new DataTable();




            /*  if (Class_BasicOperation._FinType && !Convert.ToBoolean(storefactor.Rows[0]["stock"]))///سیستم دائمی
              {
                  SqlDataAdapter Adapter1 = new SqlDataAdapter(
                                                                         @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                      Column14, Column15, Column16
                                                                          FROM            Table_105_SystemTransactionInfo
                                                                          WHERE        (Column00 = 8) ", ConBase);
                  Adapter1.Fill(bahaDT);


                  using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                  {
                      Con.Open();
                      SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                     SELECT *
                                                                     FROM   AllHeaders()
                                                                     WHERE  ACC_Code = '" + bahaDT.Rows[0]["Column13"].ToString() + @"'
                                                                 )
                                                                  SELECT 1 AS ok
                                                              ELSE
                                                                  SELECT 0 AS ok", Con);
                      if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                          throw new Exception("شماره حساب معتبر برای بهای تمام شده را در تنظیمات فروش وارد کنید");


                  }



                  using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                  {
                      Con.Open();
                      SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                     SELECT *
                                                                     FROM   AllHeaders()
                                                                     WHERE  ACC_Code = '" + bahaDT.Rows[0]["Column07"].ToString() + @"'
                                                                 )
                                                                  SELECT 1 AS ok
                                                              ELSE
                                                                  SELECT 0 AS ok", Con);
                      if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                          throw new Exception("شماره حساب معتبر برای بهای تمام شده را در تنظیمات فروش وارد کنید");
                  }

              }*/

            SqlDataAdapter Adapter = new SqlDataAdapter(
                                               @"SELECT       isnull( column10,'') as column10 , isnull(column16,'') as column16,column02
                                                                    FROM            Table_024_Discount
                                                                     group by column10,column16,column02
                                                                     ", ConSale);
            discountdt = new DataTable();
            Adapter.Fill(discountdt);
            foreach (DataRow dr in discountdt.Rows)
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + dr["Column10"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");

                }





                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + dr["Column16"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("شماره حساب معتبر را در معرفی اضافات و کسورات فروش وارد کنید");
                }


            }


            Adapter = new SqlDataAdapter(
                                                                   @"SELECT       isnull( column08,'') as Column07,isnull(column14,'') as Column13
                                                                        FROM            Table_002_SalesTypes
                                                                        WHERE        (columnid = " + mlt_SaleType.Value + ") ", ConBase);
            Adapter.Fill(factordt);


            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + factordt.Rows[0]["Column13"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                    throw new Exception("شماره حساب معتبر را در تعریف انواع فروش وارد کنید");


            }



            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + factordt.Rows[0]["Column07"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                    throw new Exception("شماره حساب معتبر را در تعریف انواع فروش وارد کنید");
            }



            if (waredt.Rows.Count >= 1)
            {
                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                {
                    Con.Open();
                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   table_005_PwhrsOperation
                                                                   WHERE  columnid = " + waredt.Rows[0]["Column02"] + @"
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                        throw new Exception("عملکرد انبار در قسمت تنظیمات فروشگاه انتخاب نشده است");
                }



                //                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                //                {
                //                    Con.Open();
                //                    SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                //                                                                   SELECT *
                //                                                                   FROM   Table_001_PWHRS
                //                                                                   WHERE  columnid = " + waredt.Rows[1]["Column02"] + @"
                //                                                               )
                //                                                                SELECT 1 AS ok
                //                                                            ELSE
                //                                                                SELECT 0 AS ok", Con);
                //                    if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                //                        throw new Exception("انبار در قسمت تنطیمات انتخاب نشده است");
                //                }

            }
            else
                throw new Exception(" عملکرد انبار در قسمت تنظیمات فروشگاه تعریف نشده است");
            ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column43"] = waredt.Rows[0]["Column02"];



            if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
            {
                LastDocnum = LastDocNum();
                if (LastDocnum > 0)
                    clDoc.IsFinal(LastDocnum);
            }
            else if (Convert.ToInt32(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"]) > 0)
                LastDocnum = Convert.ToInt32(clDoc.ExScalarQuery(Properties.Settings.Default.ACNT, @"SELECT ISNULL((select distinct h.Column00 from Table_060_SanadHead  h INNER JOIN Table_065_SanadDetail d On d.Column00=h.ColumnId where h.ColumnId=" + ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column10"] + @" AND h.Column01='" + txt_date.Text + @"' AND d.Column16=15 ),0)"));




            //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
            clDoc.CheckForValidationDate(txt_date.Text);

            //سند اختتامیه صادر نشده باشد
            clDoc.CheckExistFinalDoc();





        }
        private void checksanad()
        {
            Sanaddt = new DataTable();

            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT FactorTable.columnid,
                                               FactorTable.column01,
                                               FactorTable.date,
                                               ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
                                               ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
                                               FactorTable.Bed,
                                               FactorTable.Bes,
                                               FactorTable.NetTotal
                                        FROM   (
                                                   SELECT dbo.Table_010_SaleFactor.columnid,
                                                          dbo.Table_010_SaleFactor.column01,
                                                          dbo.Table_010_SaleFactor.column02 AS Date,
                                                          OtherPrice.PlusPrice AS Ezafat,
                                                          OtherPrice.MinusPrice AS Kosoorat,
                                                          OtherPrice.Bed,
                                                          OtherPrice.Bes,
                                                          dbo.Table_010_SaleFactor.Column28 AS NetTotal
                                                   FROM   dbo.Table_010_SaleFactor
                                                          
                                                          LEFT OUTER JOIN (
                                                                   SELECT columnid,
                                                                          SUM(PlusPrice) AS PlusPrice,
                                                                          SUM(MinusPrice) AS MinusPrice,
                                                                          Bed,
                                                                          Bes
                                                                   FROM   (
                                                                              SELECT Table_010_SaleFactor_2.columnid,
                                                                                     SUM(dbo.Table_012_Child2_SaleFactor.column04) AS 
                                                                                     PlusPrice,
                                                                                     0 AS MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = dbo.Table_012_Child2_SaleFactor.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_2
                                                                                          ON  dbo.Table_012_Child2_SaleFactor.column01 = 
                                                                                              Table_010_SaleFactor_2.columnid
                                                                              WHERE  (dbo.Table_012_Child2_SaleFactor.column05 = 0)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_2.columnid,
                                                                                     dbo.Table_012_Child2_SaleFactor.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                              UNION ALL
                                                                              SELECT Table_010_SaleFactor_1.columnid,
                                                                                     0 AS PlusPrice,
                                                                                     SUM(Table_012_Child2_SaleFactor_1.column04) AS 
                                                                                     MinusPrice,
                                                                                     td.column10 AS Bed,
                                                                                     td.column16 AS Bes
                                                                              FROM   dbo.Table_012_Child2_SaleFactor AS 
                                                                                     Table_012_Child2_SaleFactor_1
                                                                                     JOIN Table_024_Discount td
                                                                                          ON  td.columnid = 
                                                                                              Table_012_Child2_SaleFactor_1.column02
                                                                                     INNER JOIN dbo.Table_010_SaleFactor AS 
                                                                                          Table_010_SaleFactor_1
                                                                                          ON  
                                                                                              Table_012_Child2_SaleFactor_1.column01 = 
                                                                                              Table_010_SaleFactor_1.columnid
                                                                              WHERE  (Table_012_Child2_SaleFactor_1.column05 = 1)
                                                                              GROUP BY
                                                                                     Table_010_SaleFactor_1.columnid,
                                                                                     Table_012_Child2_SaleFactor_1.column05,
                                                                                     td.column10,
                                                                                     td.column16
                                                                          ) AS OtherPrice_1
                                                                   GROUP BY
                                                                          columnid,
                                                                          OtherPrice_1.Bed,
                                                                          OtherPrice_1.Bes
                                                               ) AS OtherPrice
                                                               ON  dbo.Table_010_SaleFactor.columnid = OtherPrice.columnid
                                               ) AS FactorTable
                                        WHERE  FactorTable.columnid=" + int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()) + @"
                                                                                                           ", ConSale);
            Adapter.Fill(Sanaddt);



            DataTable TPerson = new DataTable();
            TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
            TPerson.Columns.Add("Account", Type.GetType("System.String"));
            TPerson.Columns.Add("Price", Type.GetType("System.Double"));

            DataTable TAccounts = new DataTable();
            TAccounts.Columns.Add("Account", Type.GetType("System.String"));
            TAccounts.Columns.Add("Price", Type.GetType("System.Double"));


            TPerson.Rows.Add(Int32.Parse(mlt_Customer.Value.ToString()), factordt.Rows[0]["Column07"].ToString(), Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()));
            TAccounts.Rows.Add(factordt.Rows[0]["Column13"].ToString(), (-1 * Convert.ToDouble((Sanaddt.Rows[0]["NetTotal"]))));
            TAccounts.Rows.Add(factordt.Rows[0]["Column07"].ToString(), (Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"])));

            foreach (DataRow dr in Sanaddt.Rows)
            {


                if (Convert.ToDouble(dr["Ezafat"]) > 0)
                {

                    TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Ezafat"])));
                    TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Ezafat"])));
                    TPerson.Rows.Add(Int32.Parse(mlt_Customer.Value.ToString()), dr["Bed"].ToString(), Convert.ToDouble(dr["Ezafat"]));


                }
                if (Convert.ToDouble(dr["Kosoorat"]) > 0)
                {

                    TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Kosoorat"])));
                    TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Kosoorat"])));
                    TPerson.Rows.Add(Int32.Parse(mlt_Customer.Value.ToString()), dr["Bes"].ToString(), Convert.ToDouble(dr["Kosoorat"]));


                }



            }
            discountdt = new DataTable();
            factordt = new DataTable();
            //bahaDT = new DataTable();
            Adapter = new SqlDataAdapter(
                                                         @"SELECT       isnull( column10,'') as column10 , isnull(column16,'') as column16,column02
                                                                    FROM            Table_024_Discount
                                                                     group by column10,column16,column02
                                                                     ", ConSale);
            discountdt = new DataTable();
            Adapter.Fill(discountdt);
            foreach (DataRow dr in discountdt.Rows)
            {


                if (!Convert.ToBoolean(dr["column02"]))
                {
                    //اضافات
                    All_Controls_Row1(dr["column10"].ToString(), 1, null, ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString()) ? Convert.ToInt16(mlt_project.Value) : (Int16?)null)));
                    All_Controls_Row1(dr["column16"].ToString(), null, null, ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString()) ? Convert.ToInt16(mlt_project.Value) : (Int16?)null)));
                }
                else
                {
                    //کسورات
                    All_Controls_Row1(dr["column16"].ToString(), 1, null, ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString()) ? Convert.ToInt16(mlt_project.Value) : (Int16?)null)));
                    All_Controls_Row1(dr["column10"].ToString(), null, null, ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString()) ? Convert.ToInt16(mlt_project.Value) : (Int16?)null)));

                }
            }

            Adapter = new SqlDataAdapter(
                                                                               @"SELECT       isnull( column08,'') as Column07,isnull(column14,'') as Column13
                                                                        FROM            Table_002_SalesTypes
                                                                        WHERE        (columnid = " + mlt_SaleType.Value + ") ", ConBase);
            Adapter.Fill(factordt);
            All_Controls_Row1(factordt.Rows[0]["Column07"].ToString(), 1, null, ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString()) ? Convert.ToInt16(mlt_project.Value) : (Int16?)null)));
            All_Controls_Row1(factordt.Rows[0]["Column13"].ToString(), null, null, ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString()) ? Convert.ToInt16(mlt_project.Value) : (Int16?)null)));

            /*  if (Class_BasicOperation._FinType && !Convert.ToBoolean(storefactor.Rows[0]["stock"]))
              {
                  SqlDataAdapter Adapter1 = new SqlDataAdapter(
                                                                         @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                      Column14, Column15, Column16
                                                                          FROM            Table_105_SystemTransactionInfo
                                                                          WHERE        (Column00 = 8) ", ConBase);
                  Adapter1.Fill(bahaDT);
                  All_Controls_Row1(bahaDT.Rows[0]["Column13"].ToString(), null, null, ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString()) ? Convert.ToInt16(mlt_project.Value) : (Int16?)null)));
                  All_Controls_Row1(bahaDT.Rows[0]["Column07"].ToString(), null, null, ((mlt_project.Value != null && !string.IsNullOrWhiteSpace(mlt_project.Value.ToString()) ? Convert.ToInt16(mlt_project.Value) : (Int16?)null)));

              }*/
            clCredit.CheckAccountCredit(TAccounts, 0);
            clCredit.CheckPersonCredit(TPerson, 0);

        }

        public void All_Controls_Row1(string AccountCode, int? Person, Int16? Center, Int16? Project)
        {
            string m = "فاکتور شماره ی " + txt_num.Value + "دخیره شد اما صدور سند به دلیل زیر با خطا مواجه شد" + Environment.NewLine;
            //*********Control Person
            if (AccHasPerson(AccountCode) == 1)
            {
                if (Person == null)
                {
                    bt_New.Enabled = true;
                    throw new Exception(m + "انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasPerson(AccountCode) == 0)
            {
                if (Person != null)
                {
                    bt_New.Enabled = true;
                    throw new Exception(m + "انتخاب شخص برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************ Control Center
            if (AccHasCenter(AccountCode) == 1)
            {
                if (Center == null)
                {

                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasCenter(AccountCode) == 0)
            {
                if (Center != null)
                {
                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************* Control Project
            if (AccHasProject(AccountCode) == 1)
            {
                if (Project == null)
                {
                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasProject(AccountCode) == 0)
            {
                if (Project != null)
                {
                    bt_New.Enabled = true;

                    throw new Exception(m + "انتخاب پروژه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }


            //using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            //{
            //    ConAcnt.Open();
            //    SqlCommand Command = new SqlCommand("Select Control_Person from AllHeaders() where ACC_Code='" + AccountCode + "'", ConAcnt);
            //    if (Person == null && bool.Parse(Command.ExecuteScalar().ToString()))
            //    {
            //        Row.Cells["Column07"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            //        Row.Cells["Column07"].FormatStyle.BackColor = Color.Yellow;
            //        throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }

            ////    Command.CommandText = "Select Control_Center from AllHeaders() where ACC_Code='" + AccountCode + "'";
            ////    if (Center == null && bool.Parse(Command.ExecuteScalar().ToString()))
            ////    {
            ////        Row.Cells["Column08"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            ////        Row.Cells["Column08"].FormatStyle.BackColor = Color.Yellow;
            ////        throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }

            ////    Command.CommandText = "Select Control_Project from AllHeaders() where ACC_Code='" + AccountCode + "'";
            ////    if (Project == null && bool.Parse(Command.ExecuteScalar().ToString()))
            ////    {
            ////        Row.Cells["Column09"].FormatStyle = new Janus.Windows.GridEX.GridEXFormatStyle();
            ////        Row.Cells["Column09"].FormatStyle.BackColor = Color.Yellow;
            ////        throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
            ////    }
            //}

        }
        public Int16 AccHasPerson(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Person from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        private string AccountName(string AccountCode)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Select = new SqlCommand("Select ACC_Name from AllHeaders() where ACC_Code='" + AccountCode + "'", Con);
                string _AccountName = Select.ExecuteScalar().ToString();
                return _AccountName;
            }
        }
        private Int16 AccHasCenter(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Center from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        private Int16 AccHasProject(string ACC)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                Con.Open();
                SqlCommand Comm = new SqlCommand("Select ISNULL((Select Control_Project from AllHeaders() where ACC_Code='" + ACC + "'),0)", Con);
                return Convert.ToInt16(Comm.ExecuteScalar());
            }
        }
        public int LastDocNum()
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Isnull((Select Max(isnull( h.Column00,0)) from Table_060_SanadHead h INNER JOIN Table_065_SanadDetail d On d.Column00=h.ColumnId where h.Column01='" + txt_date.Text + "' AND d.Column16=15),0)", ConAcnt);
                int Result = int.Parse(Select.ExecuteScalar().ToString());
                return Result;
            }
        }



        private void bt_DelDoc_Click_1(object sender, EventArgs e)
        {
            string command = string.Empty;
            DataTable Table = new DataTable();
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {


                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 70))
                        throw new Exception("کاربر گرامی شما امکان حذف سند حسابداری را ندارید");

                    if (((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString().StartsWith("-"))
                    {
                        throw new Exception("فاکتور ثبت نشده است");

                    }
                    int RowID = int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString());
                    int SanadID = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column10", RowID.ToString());
                    int DraftID = clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column09", RowID.ToString());

                    //if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", RowID.ToString()) != 0)
                    //    throw new Exception("به علت ارجاع این فاکتور، حذف سند حسابداری امکانپذیر نمی باشد");

                    DataTable fdt = new DataTable();
                    SqlDataAdapter Adapter = new SqlDataAdapter("SELECT isnull(Column53,0) as Column53,isnull(column19,0) as Column19,isnull(column45,0) as Column45 FROM Table_010_SaleFactor where columnid=" + RowID, ConSale);
                    Adapter.Fill(fdt);

                    if (fdt.Rows.Count > 0)
                    {
                        if (Convert.ToBoolean(fdt.Rows[0]["Column53"]))
                            throw new Exception("به علت بسته شدن صندوق امکان حذف اطلاعات وجود ندارد");

                        if (Convert.ToBoolean(fdt.Rows[0]["Column19"]))
                            throw new Exception("به علت ارجاع فاکتور امکان حذف اطلاعات وجود ندارد");
                        if (Convert.ToBoolean(fdt.Rows[0]["Column45"]))
                            throw new Exception("به علت تسویه فاکتور امکان حذف اطلاعات وجود ندارد");
                    }

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف سند و حواله مربوط به این فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        clDoc.IsFinal_ID(SanadID);

                        Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=15 and Column17=" + RowID);
                        foreach (DataRow item in Table.Rows)
                        {
                            command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                        }

                        command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=15 and Column17=" + RowID;



                        Table = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ColumnID from  Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + DraftID);
                        foreach (DataRow item in Table.Rows)
                        {
                            command += " Delete from Table_075_SanadDetailNotes where Column00=" + item["ColumnId"].ToString();
                        }

                        command += " Delete  from Table_065_SanadDetail where Column00=" + SanadID + " and Column16=26 and Column17=" + DraftID;

                        command += "Delete from " + ConWare.Database + ".dbo. Table_008_Child_PwhrsDraft where column01=" + DraftID;
                        command += "Delete from " + ConWare.Database + ".dbo. Table_007_PwhrsDraft where ColumnId=" + DraftID;



                        command += " UPDATE " + ConSale.Database + ".dbo.Table_010_SaleFactor SET Column10=0,Column09=0,Column15='" + Class_BasicOperation._UserName + "', Column16=getdate() where ColumnId=" + RowID;

                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Con.Open();

                            SqlTransaction sqlTran = Con.BeginTransaction();
                            SqlCommand Command = Con.CreateCommand();
                            Command.Transaction = sqlTran;
                            try
                            {
                                Command.CommandText = command;
                                Command.ExecuteNonQuery();
                                sqlTran.Commit();
                                Class_BasicOperation.ShowMsg("", "حذف سند حسابداری و حواله با موفقیت صورت گرفت", "Information");

                            }
                            catch (Exception es)
                            {
                                sqlTran.Rollback();
                                this.Cursor = Cursors.Default;
                                Class_BasicOperation.CheckExceptionType(es, this.Name);
                            }
                            this.Cursor = Cursors.Default;
                            gridEX1.AllowEdit = InheritableBoolean.True;
                            gridEX1.Enabled = true;
                            btn_addtax.Enabled = true;
                            dataSet_Sale.EnforceConstraints = false;
                            table_010_SaleFactorTableAdapter.Fill_ID(dataSet_Sale.Table_010_SaleFactor, Convert.ToInt32(txt_ID.Text));
                            table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, Convert.ToInt32(txt_ID.Text));
                            table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, Convert.ToInt32(txt_ID.Text));
                            dataSet_Sale.EnforceConstraints = true;
                            gridEX_List.AllowEdit = InheritableBoolean.True;
                            gridEX_Extra.AllowEdit = InheritableBoolean.True;
                            gridEX_List.AllowAddNew = InheritableBoolean.True;
                            gridEX_Extra.AllowAddNew = InheritableBoolean.True;
                            gridEX_Extra.AllowDelete = InheritableBoolean.True;
                            gridEX_List.AllowDelete = InheritableBoolean.True;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void txt_num_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
            else
                Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void txt_date_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
            else
                Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void mlt_SaleType_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
            else
                Class_BasicOperation.isEnter(e.KeyChar);

        }

        private void gridEX_Extra_Enter(object sender, EventArgs e)
        {

            try
            {
                if (table_010_SaleFactorBindingSource.Count > 0)
                {

                    if (txt_date.Text == null || txt_date.Text == "")
                    {
                        MessageBox.Show("اطلاعات تاریخ معتبر نمی باشد");

                        return;
                    }
                    else if (mlt_SaleType.Value == null || mlt_SaleType.Value == DBNull.Value || mlt_SaleType.Text == "" || mlt_SaleType.Text == "0")
                    {
                        MessageBox.Show("اطلاعات نوع فروش معتبر نمی باشد");

                        return;
                    }

                    else if (mlt_Customer.Value == null || mlt_Customer.Value == DBNull.Value || mlt_Customer.Text == "" || mlt_Customer.Text == "0")
                    {
                        MessageBox.Show("اطلاعات خریدار معتبر نمی باشد");

                        return;
                    }
                    //setnull();
                    table_010_SaleFactorBindingSource.EndEdit();
                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX_Extra_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_Extra_RowCountChanged(object sender, EventArgs e)
        {
            try
            {
                //Extra-Reductions
                //Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                //txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                //Filter.Value1 = true;
                //txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                //txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString()) ;
                //txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch
            {
            }
        }

        private void gridEX_Extra_UpdatingCell(object sender, UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Column.Key == "column02")
                {

                    gridEX_Extra.SetValue("column05", (gridEX_Extra.DropDowns["Type"].GetValue("column02")));
                    gridEX_Extra.SetValue("column04", "0");
                    gridEX_Extra.SetValue("column03", "0");

                    if (gridEX_Extra.DropDowns["Type"].GetValue("column03").ToString() == "True")
                    {
                        gridEX_Extra.SetValue("column04", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
                    }
                    else
                    {

                        gridEX_Extra.SetValue("column03", gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());
                        Double darsad;
                        darsad = Convert.ToDouble(gridEX_Extra.DropDowns["Type"].GetValue("column04").ToString());

                        Double kol;
                        kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column20"].Value.ToString());
                        if (kol == 0)
                            return;
                        gridEX_Extra.SetValue("column04",

                            Convert.ToInt64(kol * darsad / 100));
                    }
                }
                else if (e.Column.Key == "column03")
                {
                    Double darsad;
                    darsad = Convert.ToDouble(e.Value.ToString());
                    Double kol;
                    kol = Convert.ToDouble(gridEX_List.GetTotalRow().Cells["column20"].Value.ToString());
                    if (kol == 0)
                        return;
                    gridEX_Extra.SetValue("column04",

                           Convert.ToInt64(kol * darsad / 100));
                }
            }
            catch
            {
            }


        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Extra.GetRows())
                {
                    if (double.Parse(item.Cells["Column03"].Value.ToString()) > 0)
                    {
                        item.BeginEdit();
                        item.Cells["Column04"].Value = Convert.ToInt64(Convert.ToDouble(
                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                        AggregateFunction.Sum).ToString()) * Convert.ToDouble(item.Cells["Column03"].Value.ToString()) / 100);
                        item.EndEdit();

                    }
                }
                gridEX_Extra.UpdateData();

                table_012_Child2_SaleFactorBindingSource.EndEdit();
                p_tax.Visible = false;

                Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
                txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

                txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("ثبت اضافات و کسورات با خطا موجه شد. شرح خطا" + ex.Message);
            }
        }

        private void gridEX_Extra_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX_Extra.CurrentCellDroppedDown = true;

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            p_tax.Visible = false;
            gridEX_Extra.CancelCurrentEdit();
            table_012_Child2_SaleFactorBindingSource.CancelEdit();
            this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, Convert.ToInt32(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString()));
            Janus.Windows.GridEX.GridEXFilterCondition Filter = new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"], ConditionOperator.Equal, false);
            txt_Extra.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();
            Filter.Value1 = true;
            txt_Reductions.Value = gridEX_Extra.GetTotal(gridEX_Extra.RootTable.Columns["Column04"], AggregateFunction.Sum, Filter).ToString();

            txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
            txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) + Convert.ToDouble(txt_Extra.Value.ToString()) - Convert.ToDouble(txt_Reductions.Value.ToString());


        }

        private void txt_desc_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
                else
                {
                    txt_GoodCode.Focus();
                    txt_GoodCode.Select(txt_GoodCode.Text.Length, 0);
                }
            }
            else
            {
                if (e.KeyChar == 13)
                {
                    txt_GoodCode.Focus();
                    txt_GoodCode.Select(txt_GoodCode.Text.Length, 0);
                }
            }
        }

        private void txt_date_TextChanged(object sender, EventArgs e)
        {
            //try
            //{
            //    Int16 w = 0;
            //    if (mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()) && mlt_Ware.Value.ToString().All(char.IsDigit))
            //        w = Convert.ToInt16(mlt_Ware.Value);
            //    GoodbindingSource.DataSource = clGood.MahsoolInfo(txt_date.Text, w);
            //    DataTable GoodTable = clGood.MahsoolInfo(txt_date.Text, w);
            //    gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            //    gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
            //}
            //catch { }
        }

        private void تعریفکالانوعفروشواحدشمارشToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                PWHRS.Class_BasicOperation._UserName = Class_BasicOperation._UserName;
                PWHRS.Class_BasicOperation._FinType = Class_BasicOperation._FinType;
                PWHRS.Class_BasicOperation._FinYear = Class_BasicOperation._Year;
                PWHRS.Class_BasicOperation._WareType = Class_BasicOperation._WareType;
                PWHRS.Class_BasicOperation._OrgCode = Class_BasicOperation._OrgCode;
                PWHRS.Class_ChangeConnectionString.CurrentConnection = Properties.Settings.Default.WHRS;


                Class_UserScope UserScope = new Class_UserScope();

                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 5))
                {

                    PWHRS._02_EtelaatePaye.Frm_001_DefineGoods ob = new PWHRS._02_EtelaatePaye.Frm_001_DefineGoods(UserScope.CheckScope(Class_BasicOperation._UserName, "Column10", 6));
                    ob.ShowDialog();
                    Int16 ware = 0;

                    if (
                        mlt_Ware.Value != null &&
                        !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()) &&
                        mlt_Ware.Value.ToString().All(char.IsDigit))
                        ware = Convert.ToInt16(mlt_Ware.Value);
                    else if (storefactor.Rows[0]["ware"] != null &&
                     !string.IsNullOrWhiteSpace(storefactor.Rows[0]["ware"].ToString()))
                        ware = Convert.ToInt16(storefactor.Rows[0]["ware"]);
                string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + mlt_project.Value + "),0)");
                    if (controlremain=="True")
                    {
                        GoodbindingSource.DataSource = clGood.MahsoolInfo(ware);
                        DataTable GoodTable = clGood.MahsoolInfo(ware);
                        gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                        gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                    }
                    else
                    {
                        GoodbindingSource.DataSource = clGood.GoodInfo();

                        DataTable GoodTable = clGood.GoodInfo();
                        gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                        gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                    }
                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
            catch (Exception ex)
            {
                MessageBox.Show("دسترسی به فرم تعریف کالا با خطا مواجه شد.شرح کالا" + ex.Message);
            }
        }

        private void معرفینوعفروشToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Class_UserScope UserScope = new Class_UserScope();
                if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 3))
                {
                    _02_BasicInfo.Frm_007_SaleType ob = new _02_BasicInfo.Frm_007_SaleType(UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 4));
                    ob.ShowDialog();
                    mlt_SaleType.DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "SELECT columnid,column01,column02,Isnull(Column16,0) as Column16,Isnull(Column17,0) as Column17,Isnull(Column18,0) as Column18,Isnull(Column19,0) as Column19,Isnull(Column20,0) as Column20  from Table_002_SalesTypes");

                }
                else
                    Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "None");
            }
            catch (Exception ex)
            {
                MessageBox.Show("دسترسی به فرم معرفی نوع فروش با خطا مواجه شد.شرح کالا" + ex.Message);
            }
        }

        private void gridEX_List_UpdatingCell(object sender, UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Column.Key == "column03")
                {


                    if (gridEX_List.GetRow().Cells["column06"].Text.Trim() != "")
                    {
                        float h = clDoc.GetZarib(Convert.ToInt32(gridEX_List.GetValue("GoodCode")), Convert.ToInt16(e.InitialValue), Convert.ToInt16(e.Value));
                        gridEX_List.SetValue("column07", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);
                        gridEX_List.SetValue("column06", float.Parse(gridEX_List.GetValue("column06").ToString()) * h);

                    }
                }
            }
            catch { }
        }

        private void mlt_Ware_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void mlt_Ware_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    //Class_BasicOperation.isEnter(e.KeyChar);
                    mlt_PersonSale.Focus();
            }
        }

        private void mlt_Ware_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");

        }

        private void mlt_Ware_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void mlt_Ware_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Ware.Value != null && !string.IsNullOrWhiteSpace(mlt_Ware.Value.ToString()) && mlt_Ware.Value.ToString().All(char.IsDigit))
                {
                    string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + mlt_project.Value + "),0)");

                    if (controlremain=="True")
                    {
                        GoodbindingSource.DataSource = clGood.MahsoolInfo(Convert.ToInt16(mlt_Ware.Value));
                        DataTable GoodTable = clGood.MahsoolInfo(Convert.ToInt16(mlt_Ware.Value));
                        gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                        gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                    }
                    else
                    {
                        GoodbindingSource.DataSource = clGood.GoodInfo();
                        DataTable GoodTable = clGood.GoodInfo();
                        gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                        gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
                    }


                }
            }
            catch
            {
            }
        }

        private void txt_GoodCode_TextChanged(object sender, EventArgs e)
        {
            // InitialNewRow();

        }

        private void Frm_002_StoreFaktor_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void uiTab1_Click(object sender, EventArgs e)
        {

        }

        private void gridEX1_SelectionChanged(object sender, EventArgs e)
        {
            //if (e != null)
            //{
            //    Janus.Windows.GridEX.GridEX j = (Janus.Windows.GridEX.GridEX)sender;

            //    var id = j.GetValue("columnid");
            //    if (id != null && !string.IsNullOrWhiteSpace(id.ToString()))
            //        InitialNewRowwithid(Convert.ToInt32(id));
            //}
        }

        private void gridEX1_Enter(object sender, EventArgs e)
        {
            // gridEX1_SelectionChanged(sender, null);
        }

        private void InitialNewRowwithid(int goodid)
        {
            try
            {
                bool isthere = false;
                if (goodid > 0)
                {

                    Int64 codeid = goodid;
                    double buyprice = 0;
                    int ok = 1;

                    if (ok == 1)
                    {

                        try
                        {

                            table_010_SaleFactorBindingSource.EndEdit();
                        }
                        catch (Exception ex)
                        {

                            Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                            return;
                        }
                        if (gridEX_List.GetRows().Count() > 0)
                        {
                            string goodcode;
                            Int16 unit;
                            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_List.GetRows())
                            {
                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                                {
                                    Con.Open();
                                    SqlCommand Comm = new SqlCommand("SELECT tcc.column06 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                    goodcode = (Comm.ExecuteScalar().ToString());
                                    Comm = new SqlCommand("SELECT tcc.column07 FROM   table_004_CommodityAndIngredients tcc WHERE  tcc.columnid=" + item.Cells["GoodCode"].Value.ToString() + "", Con);
                                    unit = Convert.ToInt16(Comm.ExecuteScalar());
                                }


                                if (Convert.ToInt64(item.Cells["GoodCode"].Value) == codeid && unit == Convert.ToInt16(item.Cells["column03"].Value) && Convert.ToBoolean(item.Cells["column30"].Value) == false)
                                {

                                    isthere = true;
                                    item.BeginEdit();

                                    //float h = clDoc.GetZarib(Convert.ToInt32(codeid), Convert.ToInt16(item.Cells["column03"].Value));
                                    //item.Cells["Column07"].Value = (float.Parse(this.txt_Count.Text) * h) + Convert.ToDouble(item.Cells["Column07"].Value);
                                    //item.Cells["Column06"].Value = (float.Parse(this.txt_Count.Text) * h) + Convert.ToDouble(item.Cells["Column06"].Value);


                                    item.Cells["Column07"].Value = Convert.ToInt32(item.Cells["Column07"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);
                                    item.Cells["Column06"].Value = Convert.ToInt32(item.Cells["Column06"].Value.ToString()) + Convert.ToInt32(this.txt_Count.Text);

                                    item.EndEdit();
                                    double TotalPrice;
                                    if (item.Cells["Column10"].Value.ToString() != string.Empty && item.Cells["Column07"].Value.ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(item.Cells["Column10"].Value.ToString()) * Convert.ToDouble(item.Cells["Column07"].Value.ToString());
                                        item.BeginEdit();
                                        item.Cells["column11"].Value = TotalPrice;
                                        item.Cells["column16"].Value = 0;
                                        item.Cells["column18"].Value = 0;
                                        item.Cells["column17"].Value = 0;
                                        item.Cells["Column19"].Value = 0;
                                        item.Cells["Column40"].Value = TotalPrice;
                                        item.Cells["Column20"].Value = TotalPrice;

                                        item.EndEdit();

                                    }
                                    gridEX_List.UpdateData();

                                    //محاسبه قیمتهای انتهای فاکتور
                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());

                                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());



                                    break;

                                }

                            }
                            if (!isthere)
                            {

                                gridEX_List.MoveToNewRecord();
                                gridEX_List.SetValue("GoodCode", codeid);
                                gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                                gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                                gridEX_List.SetValue("column02", codeid);

                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                                {
                                    Con.Open();
                                    SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + codeid + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                    buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                                }
                                gridEX_List.SetValue("Column41", buyprice);



                                GoodbindingSource.Filter = "GoodID=" +
                                        gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                if (GoodbindingSource.CurrencyManager.Position > -1)
                                {
                                    gridEX_List.SetValue("tedaddarkartoon",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                    gridEX_List.SetValue("tedaddarbaste",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                    gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                    gridEX_List.SetValue("column03",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                    gridEX_List.SetValue("column16",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());



                                    DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                    this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                    gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                    double amunt = 0;
                                    if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                    {
                                        DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                        if (dr.Count() > 0)
                                        {
                                            amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                            gridEX_List.SetValue("column10",
                                             dr[0].ItemArray[3]);
                                        }
                                    }

                                    if (amunt == Convert.ToDouble(0))
                                    {
                                        //gridEX_List.SetValue("column10",
                                        //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                        //"SalePrice"].ToString());
                                        gridEX_List.SetValue("column10",
                         clDoc.ExScalar(ConWare.ConnectionString,
                               "table_004_CommodityAndIngredients", "column34", "ColumnId",
                               gridEX_List.GetValue("GoodCode").ToString()));

                                    }
                                    gridEX_List.SetValue("column09",
                                          0);
                                    gridEX_List.SetValue("column08",
                                       0);

                                    double TotalPrice;
                                    if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                        gridEX_List.SetValue("column11", TotalPrice);
                                        gridEX_List.SetValue("Column16", 0);
                                        gridEX_List.SetValue("Column18", 0);
                                        gridEX_List.SetValue("column17", 0);
                                        gridEX_List.SetValue("Column19", 0);
                                        gridEX_List.SetValue("Column40", TotalPrice);
                                        gridEX_List.SetValue("Column20", TotalPrice);
                                    }


                                    gridEX_List.UpdateData();
                                    //محاسبه قیمتهای انتهای فاکتور
                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());

                                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());




                                }



                            }


                        }
                        else
                        {

                            gridEX_List.MoveToNewRecord();
                            gridEX_List.SetValue("GoodCode", codeid);
                            gridEX_List.SetValue("Column07", Convert.ToInt32(txt_Count.Text));
                            gridEX_List.SetValue("Column06", Convert.ToInt32(txt_Count.Text));
                            gridEX_List.SetValue("column02", codeid);

                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                            {
                                Con.Open();
                                SqlCommand Comm = new SqlCommand(@"SELECT ISNULL(
                                                           (
                                                               SELECT TOP 1 ISNULL(tcbf.column10, 0) AS column10
                                                               FROM   Table_016_Child1_BuyFactor tcbf
                                                                      JOIN Table_015_BuyFactor tbf
                                                                           ON  tbf.columnid = tcbf.column01
                                                               WHERE  tcbf.column02 = " + codeid + @"
                                                                      AND tbf.column02 <= '" + txt_date.Text + @"'
                                                               ORDER BY
                                                                      tbf.column02 DESC,
                                                                      tbf.column06 DESC
                                                           ),
                                                           0
                                                       ) AS buyprice", Con);

                                buyprice = Convert.ToDouble(Comm.ExecuteScalar());
                            }
                            gridEX_List.SetValue("Column41", buyprice);
                            {
                                GoodbindingSource.Filter = "GoodID=" +
                                    gridEX_List.GetRow().Cells["column02"].Value.ToString();
                                if (GoodbindingSource.CurrencyManager.Position > -1)
                                {
                                    gridEX_List.SetValue("tedaddarkartoon",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                                    gridEX_List.SetValue("tedaddarbaste",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());
                                    gridEX_List.DropDowns["CountUnit"].SetDataBinding(clDoc.FillUnitCountByKala(Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString())), "");

                                    gridEX_List.SetValue("column03",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["CountUnit"].ToString());
                                    gridEX_List.SetValue("column16",
                                        ((DataRowView)GoodbindingSource.CurrencyManager.Current)["Discount"].ToString());

                                    DataTable dt = clDoc.ReturnTable(this.ConWare.ConnectionString, @"select * from Table_032_GoodPrice where Column00=" + Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()) + "   ");
                                    this.table_032_GoodPriceTableAdapter.FillByGood(this.dataSet_Sale.Table_032_GoodPrice, Convert.ToInt32(gridEX_List.GetValue("GoodCode").ToString()));
                                    gridEX_List.DropDowns[6].SetDataBinding(table_032_GoodPriceBindingSource, "");
                                    double amunt = 0;
                                    if (!string.IsNullOrWhiteSpace(mlt_SaleType.Text))
                                    {
                                        DataRow[] dr = dt.Select("Column01='" + mlt_SaleType.Text.Trim() + "'");
                                        if (dr.Count() > 0)
                                        {
                                            amunt = Convert.ToDouble(dr[0].ItemArray[3]);
                                            gridEX_List.SetValue("column10",
                                             dr[0].ItemArray[3]);
                                        }
                                    }

                                    if (amunt == Convert.ToDouble(0))
                                    {
                                        //gridEX_List.SetValue("column10",
                                        //    ((DataRowView)GoodbindingSource.CurrencyManager.Current)[
                                        //"SalePrice"].ToString());
                                        gridEX_List.SetValue("column10",
                         clDoc.ExScalar(ConWare.ConnectionString,
                               "table_004_CommodityAndIngredients", "column34", "ColumnId",
                               gridEX_List.GetValue("GoodCode").ToString()));

                                    }
                                    gridEX_List.SetValue("column09",
                                          0);
                                    gridEX_List.SetValue("column08",
                                       0);
                                    double TotalPrice;
                                    if (gridEX_List.GetValue("Column10").ToString() != string.Empty && gridEX_List.GetValue("Column07").ToString() != string.Empty)
                                    {
                                        TotalPrice = Convert.ToDouble(gridEX_List.GetValue("Column10").ToString()) * Convert.ToDouble(gridEX_List.GetValue("Column07").ToString());
                                        gridEX_List.SetValue("column11", TotalPrice);
                                        gridEX_List.SetValue("Column16", 0);
                                        gridEX_List.SetValue("Column18", 0);
                                        gridEX_List.SetValue("column17", 0);
                                        gridEX_List.SetValue("Column19", 0);
                                        gridEX_List.SetValue("Column40", TotalPrice);
                                        gridEX_List.SetValue("Column20", TotalPrice);
                                    }

                                    gridEX_List.UpdateData();

                                    txt_TotalPrice.Value = Convert.ToDouble(
                                        gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                        AggregateFunction.Sum).ToString());
                                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                                    Convert.ToDouble(txt_Reductions.Value.ToString());



                                }
                            }
                        }



                    }





                }
            }
            catch (Exception ex)
            {
            }


        }

        private void gridEX1_Click(object sender, EventArgs e)
        {

        }

        private void gridEX1_MouseClick(object sender, MouseEventArgs e)
        {
            Janus.Windows.GridEX.GridEX j = (Janus.Windows.GridEX.GridEX)sender;

            var id = j.GetValue("columnid");
            if (id != null && !string.IsNullOrWhiteSpace(id.ToString()))
                InitialNewRowwithid(Convert.ToInt32(id));
        }

        private void mlt_SaleType_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                string acc = clDoc.ExScalarQuery(ConBase.ConnectionString, "select Column26 from Table_002_SalesTypes where ColumnId=" + mlt_SaleType.Value);
                if (acc == "False")
                    mlt_Customer.ReadOnly = false;
                else
                    mlt_Customer.ReadOnly = true;

            }
            catch
            {
            }
        }

        private void gridEX_Extra_FormattingRow(object sender, RowLoadEventArgs e)
        {

        }

        private void btn_8_Print_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.table_010_SaleFactorBindingSource.Count > 0)
                {
                    if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                    {
                        if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 128))
                        {
                            Save_Event(sender, e);


                            _05_Sale.Reports.Frm_rpt_Salefactor8 frm =
                               new Reports.Frm_rpt_Salefactor8(int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()));
                            frm.ShowDialog();
                        }
                    }
                    else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان دسترسی به این فرم را ندارید", "Warning");
                }
            }
            catch
            {
                try
                {

                    _05_Sale.Reports.Frm_rpt_Salefactor8 frm =
                       new Reports.Frm_rpt_Salefactor8(int.Parse(((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["Column01"].ToString()));
                    frm.ShowDialog();
                }
                catch
                { }

            }
            this.Cursor = Cursors.Default;
        }

        private void btn_Updategood_Click(object sender, EventArgs e)
        {
            string controlremain = clDoc.ExScalar(ConBase.ConnectionString, @"select isnull((select Column31 from Table_295_StoreInfo where Column05=" + mlt_project.Value + "),0)");
            if (controlremain == "True")
            {
                GoodbindingSource.DataSource = clGood.MahsoolInfo(Convert.ToInt16(mlt_Ware.Value));
                DataTable GoodTable = clGood.MahsoolInfo(Convert.ToInt16(mlt_Ware.Value));
                gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
            }
            else
            {
                GoodbindingSource.DataSource = clGood.GoodInfo();
                DataTable GoodTable = clGood.GoodInfo();
                gridEX_List.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                gridEX_List.DropDowns["GoodName"].SetDataBinding(GoodTable, "");
            }
        }

        private void btn_settlement_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                this.table_010_SaleFactorBindingSource.EndEdit();
                gridEX_List.UpdateData();
                gridEX_Extra.UpdateData();

                if (this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Added) != null
                      || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_010_SaleFactor.GetChanges(DataRowState.Detached) != null
                         ||
                         this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Added) != null
                      || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_011_Child1_SaleFactor.GetChanges(DataRowState.Detached) != null
                          ||
                         this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Modified) != null || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Added) != null
                      || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Deleted) != null || this.dataSet_Sale.Table_012_Child2_SaleFactor.GetChanges(DataRowState.Detached) != null
                         )
                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به ذخیره تغییرات هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        Save_Event(sender, e);
                    }
                    else
                    {
                        return;
                    }
                }
                DataTable dtTransactionInfo = new DataTable();
                dtTransactionInfo = clDoc.ReturnTable(ConBase.ConnectionString, @"select * from Table_105_SystemTransactionInfo");

                if (dtTransactionInfo.Select("Column00=89")[0]["Column07"].ToString() == "" || dtTransactionInfo.Select("Column00=89")[0]["Column13"].ToString() == ""
                    || dtTransactionInfo.Select("Column00=91")[0]["Column13"].ToString() == ""
                    || dtTransactionInfo.Select("Column00=92")[0]["Column07"].ToString() == "" || dtTransactionInfo.Select("Column00=92")[0]["Column13"].ToString() == "")
                {

                    Class_BasicOperation.ShowMsg("", "لطفا سرفصل های تسویه را از تنظیم تراکنش، زیر سیستم فروش انتخاب نمایید", "Warning");
                    this.Cursor = Cursors.Default;
                    return;
                }
                SqlDataAdapter Adapter1 = new SqlDataAdapter("Select * from Table_030_Setting where columnid in (79,80,81) order by Columnid", ConBase);
                setting = new DataTable();
                Adapter1.Fill(setting);

                if (
                    setting.Rows[0]["Column02"] == null || setting.Rows[0]["Column02"] == DBNull.Value || setting.Rows[0]["Column02"].ToString() == "" ||
                    setting.Rows[0]["Column02"].ToString() == "-1" ||
                    setting.Rows[1]["Column02"] == null || setting.Rows[1]["Column02"] == DBNull.Value || setting.Rows[1]["Column02"].ToString() == "" ||
                    setting.Rows[1]["Column02"].ToString() == "-1" ||
                    setting.Rows[2]["Column02"] == null || setting.Rows[2]["Column02"] == DBNull.Value || setting.Rows[2]["Column02"].ToString() == "" ||
                    setting.Rows[2]["Column02"].ToString() == "-1")
                {


                    Class_BasicOperation.ShowMsg("", "لطفا وضعیت استرداد چک و صندوق دریافت کننده را از قسمت اطلاعات پایه فرم تنظیمات تراکنش، تب تنظیمات فروشگاه تکمیل نمایید", "Warning");

                    this.Cursor = Cursors.Default;
                    return;
                }

                if (this.table_010_SaleFactorBindingSource.CurrencyManager.Position > -1)
                {
                    string RowID = ((DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current)["ColumnId"].ToString();
                    string lement = clDoc.ExScalar(ConSale.ConnectionString, @"select isnull(( SELECT Column45 FROM Table_010_SaleFactor  WHERE Columnid=" + RowID + "),0)");

                    string Bs = clDoc.ExScalar(ConSale.ConnectionString, @"select Column53 from Table_010_SaleFactor where ColumnId=" + RowID);
                    if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 165))
                    {
                        MessageBox.Show("کاربر گرامی شما امکان تسویه را ندارید");
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    else if (clDoc.ExScalar(ConSale.ConnectionString, "Table_010_SaleFactor", "Column17", "ColumnId", txt_ID.Text) == "True")
                    {
                        MessageBox.Show("به علت باطل شدن این فاکتور امکان تسویه وجود ندارد");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    else if (clDoc.OperationalColumnValue("Table_010_SaleFactor", "Column20", txt_ID.Text) != 0)
                    {
                        MessageBox.Show("به علت مرجوع شدن این فاکتور امکان تسویه وجود ندارد");
                        this.Cursor = Cursors.Default;
                        return;
                    }



                    else if (Bs == "True")
                        throw new Exception("به علت بسته شدن این فاکتور، تسویه امکانپذیر نمی باشد");

                    else if (lement.ToString() != "False")
                    {
                        MessageBox.Show("این فاکتور قبلا تسویه شده است");
                        this.Cursor = Cursors.Default;
                        return;
                    }



                    _05_Sale.Frm_028_Settelment frm = new Frm_028_Settelment(Convert.ToInt32(txt_ID.Text), Convert.ToInt32(txt_num.Value), Convert.ToInt32(mlt_Customer.Value), txt_date.Text);
                    frm.ShowDialog();



                    dataSet_Sale.EnforceConstraints = false;
                    this.table_010_SaleFactorTableAdapter.Fill_ID(this.dataSet_Sale.Table_010_SaleFactor, int.Parse(RowID));
                    this.table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(this.dataSet_Sale.Table_012_Child2_SaleFactor, int.Parse(RowID));
                    this.table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(this.dataSet_Sale.Table_011_Child1_SaleFactor, int.Parse(RowID));
                    dataSet_Sale.EnforceConstraints = true;
                    txt_TotalPrice.Value = Convert.ToDouble(
                                      gridEX_List.GetTotal(gridEX_List.RootTable.Columns["column20"],
                                      AggregateFunction.Sum).ToString());

                    txt_AfterDis.Value = Convert.ToDouble(txt_TotalPrice.Value.ToString());
                    txt_EndPrice.Value = Convert.ToDouble(txt_AfterDis.Value.ToString()) +
                    Convert.ToDouble(txt_Extra.Value.ToString()) -
                    Convert.ToDouble(txt_Reductions.Value.ToString());

                }
            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
            this.Cursor = Cursors.Default;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {

        }

        private void mlt_Stor_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                string Project = clDoc.ExScalar(ConBase.ConnectionString, @"select Column05 from Table_295_StoreInfo where Columnid=" + mlt_Stor.Value + "");
                mlt_project.Value = Convert.ToInt16(Project);
            }
            catch 
            {

               
            }
           


        }

        private void gridEX_List_SortKeysChanged(object sender, EventArgs e)
        {
            if (gridEX_List.RootTable.SortKeys.Count == 0)
            {
                GridEXSortKey sortKey = new GridEXSortKey();
                //GridEXColumn c = new GridEXColumn();
                sortKey.Column = gridEX_List.RootTable.Columns["Columnid"] ;
                sortKey.SortOrder = Janus.Windows.GridEX.SortOrder.Ascending;
                gridEX_List.RootTable.SortKeys.Add(sortKey);

            }

        }

        private void mlt_PersonSale_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);
        }

        private void mlt_PersonSale_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    //Class_BasicOperation.isEnter(e.KeyChar);
                    txt_desc.Focus();
            }
            //if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            //{
            //    if (!char.IsControl(e.KeyChar) && e.KeyChar != 13)
            //        ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            //    else
            //        //Class_BasicOperation.isEnter(e.KeyChar);
            //        txt_desc.Focus();

            //}
            //else
            //    //Class_BasicOperation.isEnter(e.KeyChar);
            //    txt_desc.Focus();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                try
                {
                    Save_Event(sender, e);
                }
                catch (Exception ex)
                {

                    Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                }
                this.Cursor = Cursors.Default;



            }
            catch (Exception ex)
            {

                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void mlt_SaleType_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "column02", "column01");

        }

        private void mlt_project_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);

            }
        }

        private void mlt_project_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");

        }


    }
}