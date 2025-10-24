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


namespace PSHOP._05_Sale
{
    public partial class Frm_027_PreFactorToSaleFactor : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_Discounts ClDiscount = new Classes.Class_Discounts();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        public int _SaleID = 0;
        bool _Arzi;
        DataRowView _Row;
        BindingSource GoodBindingSource = new BindingSource();

        public Frm_027_PreFactorToSaleFactor(bool Arzi,DataRowView Row)
        {
            InitializeComponent();
            _Arzi = Arzi;
            _Row = Row;
        }

        private void Frm_027_PreFactorToSaleFactor_Load(object sender, EventArgs e)
        {
            foreach (Janus.Windows.GridEX.GridEXColumn col in this.gridEX1.RootTable.Columns)
            {
                col.CellStyle.BackColor = Color.White;
                if (col.Key == "Column13" || col.Key == "Column15")
                    col.DefaultValue = Class_BasicOperation._UserName;
                if (col.Key == "Column14" || col.Key == "Column16")
                    col.DefaultValue = Class_BasicOperation.ServerDate();
            }


            gridEX1.DropDowns["Customer"].SetDataBinding(clDoc.ReturnTable(Properties.Settings.Default.BASE,
            @"Select ColumnId,Column01,Column02 from Table_045_PersonInfo where ColumnId=" + _Row["Column03"].ToString()), "");
            gridEX1.DropDowns["Seller"].SetDataBinding(clDoc.ReturnTable(Properties.Settings.Default.BASE, "Select * from PeopleScope(8,3)"), "");
            gridEX1.DropDowns["SaleType"].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "SELECT ColumnId,Column01,Column02 from Table_002_SalesTypes"), "");

            // Add New For Sale Table
            gridEX1.MoveToNewRecord();
            gridEX1.SetValue("Column03", _Row["Column03"].ToString());
            gridEX1.SetValue("Column01", clDoc.MaxNumber(ConSale.ConnectionString, "Table_010_SaleFactor", "Column01").ToString());
            gridEX1.SetValue("Column02", FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd"));
            gridEX1.SetValue("Column05", (_Row["Column05"].ToString().Trim() == "" ? (object)DBNull.Value : _Row["Column05"].ToString()));
            gridEX1.SetValue("Column36", (_Row["Column23"].ToString().Trim() == "" ? (object)DBNull.Value : _Row["Column23"].ToString()));
            gridEX1.SetValue("Column04", (_Row["Column04"].ToString().Trim() == "" ? (object)DBNull.Value : _Row["Column04"].ToString()));
            gridEX1.SetValue("Column21", (_Row["Column15"].ToString().Trim() == "" ? false:true));
            gridEX1.SetValue("Column22", (_Row["Column16"].ToString().Trim() == "" ? (object)DBNull.Value : _Row["Column16"].ToString()));
            gridEX1.SetValue("Column23", (_Row["Column17"].ToString().Trim() == "" ? false:true));
            gridEX1.SetValue("Column24", (_Row["Column18"].ToString().Trim() == "" ? (object)DBNull.Value : _Row["Column18"].ToString()));
            gridEX1.SetValue("Column25", (_Row["Column19"].ToString().Trim() == ""? false:true ));
            gridEX1.SetValue("Column26", (_Row["Column20"].ToString().Trim()==""?DBNull.Value: _Row["Column20"]));
            gridEX1.SetValue("Column27", (_Row["Column21"].ToString().Trim() == "" ? (object)DBNull.Value : _Row["Column21"].ToString()));
            gridEX1.SetValue("Column06", (_Row["Column06"].ToString().Trim() == "" ? (object)DBNull.Value : _Row["Column06"].ToString()));
            GoodBindingSource.DataSource = clGood.GoodInfo();
            gridEX1.Select();
        }

        private void gridEX1_CellValueChanged(object sender, ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;
        }

        private void bt_No_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                if (gridEX1.RootTable.Columns[gridEX1.Col].Key == "column15")
                    gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.None;
                else gridEX1.EnterKeyBehavior = Janus.Windows.GridEX.EnterKeyBehavior.NextCell;
            }
            catch
            {
            }
        }

        private void gridEX1_Error(object sender, ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_UpdatingCell(object sender, UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Value.ToString().Trim() == "")
                    e.Value = DBNull.Value;
            }
            catch
            {
                if (e.Value.ToString().Trim() == "")
                    e.Value = DBNull.Value;
            }
        }

        private void bt_Yes_Click(object sender, EventArgs e)
        {
            try
            {
                //*****ثبت هدر فاکتور فروش
                DataRowView SaleRow = (DataRowView)this.table_010_SaleFactorBindingSource.CurrencyManager.Current;
                SaleRow["Column07"] = _Row["ColumnId"].ToString();
                SaleRow["Column08"] = 0;
                SaleRow["Column09"] = 0;
                SaleRow["Column12"] = _Arzi;
                SaleRow["Column40"] = DBNull.Value;
                SaleRow["Column41"] = 0;
                SaleRow["Column13"] = Class_BasicOperation._UserName;
                SaleRow["Column14"] = Class_BasicOperation.ServerDate();
                SaleRow["Column15"] = Class_BasicOperation._UserName;
                SaleRow["Column16"] = Class_BasicOperation.ServerDate();

                this.table_010_SaleFactorBindingSource.EndEdit();
                this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                _SaleID = int.Parse(SaleRow["ColumnId"].ToString());

                //***************************INSERT GOOD LIST
                SqlDataAdapter Adapter = new SqlDataAdapter(
                    "Select * from Table_008_Child1_FactorBefore where Column01=" +
                    _Row["ColumnId"].ToString(), ConSale);
                DataTable Child1 = new DataTable();
                Adapter.Fill(Child1);
                foreach (DataRow item in Child1.Rows)
                {
                    double SingleWeight=0;
                    double TotalWeight=0;
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                    {
                        GoodBindingSource.Filter="GoodID="+item["Column02"].ToString();
                        if(GoodBindingSource.Count>0)
                        {
                            SingleWeight=Convert.ToDouble(((DataRowView)GoodBindingSource.CurrencyManager.Current)["Weight"].ToString());
                            TotalWeight=Convert.ToDouble(item["Column07"].ToString())*SingleWeight;
                        }
                        Con.Open();
                        SqlCommand ChildInsert = new SqlCommand(
                            @"INSERT INTO Table_011_Child1_SaleFactor ([column01]
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
           ,[column30]
           ,[Column31]
           ,[Column32]
           ,[Column33]
           ,[Column34]
           ,[Column35]
           ,[Column36]
           ,[Column37]) VALUES(" + _SaleID + "," +
                            item["Column02"].ToString()
                            + "," + item["Column03"].ToString() + "," +
                            item["Column04"].ToString() + "," + item["Column05"].ToString() +
                            "," + item["Column06"].ToString() + "," + item["Column07"].ToString() + "," + item["Column08"].ToString() +
                            "," + item["Column09"].ToString() + "," + item["Column10"].ToString() + "," + item["Column11"].ToString() +
                            ",null,null,null,0," + item["Column16"].ToString() + "," +
                            item["Column17"].ToString() + "," + item["Column18"].ToString() + "," + item["Column19"].ToString() + ","
                            + item["Column21"].ToString() + "," +
                            (item["Column22"].ToString().Trim() != "" ? "'" +
                            item["Column22"].ToString() + "'" : "NULL") +
                            "," + (item["Column23"].ToString().Trim() != "" ? "'" +
                            item["Column23"].ToString() + "'" : "NULL") + "," +
                            (item["Column20"].ToString().Trim() != "" ? "'" +
                            item["Column20"].ToString() + "'" : "NULL") +
                            "," + _Row["ColumnId"].ToString() + ",0,0,0," +
                            item["ColumnId"].ToString() + ",0,0," +
                            item["Column27"].ToString() + "," + item["Column28"].ToString() +
                            ",100,null,null,"+SingleWeight+","+TotalWeight+")", Con);

                        ChildInsert.ExecuteNonQuery();
                    }
                }

                //***************************INSERT EXTRA/Reductions
                Adapter = new SqlDataAdapter(
                    "Select * from Table_009_Child2_FactorBefore where Column01=" +
                     _Row["ColumnId"].ToString(), ConSale);
                DataTable Child2 = new DataTable();
                Adapter.Fill(Child2);
                foreach (DataRow item in Child2.Rows)
                {
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.SALE))
                    {
                        Con.Open();
                        SqlCommand ChildInsert = new SqlCommand(
                            @"INSERT INTO Table_012_Child2_SaleFactor ([column01]
                                                                           ,[column02]
                                                                           ,[column03]
                                                                           ,[column04]
                                                                           ,[column05]
                                                                           ,[column06]) VALUES(" + _SaleID + "," +
                            item["Column02"].ToString() + "," + item["Column03"].ToString() +
                            "," + item["Column04"].ToString() +
                            "," + ((bool)item["Column05"] == true ? "1" : "0") +
                            //(item["Column05"].ToString().Trim() == "FALSE" ? "0" : "1")
                            "," + (item["Column06"].ToString().Trim() != "" ? "'" +
                            item["Column06"].ToString() + "'" : "NULL") + ")", Con);
                        ChildInsert.ExecuteNonQuery();
                    }
                }
              
               

                //محاسبه تخفیفات و سایر اطلاعات فاکتور 
                table_011_Child1_SaleFactorTableAdapter.Fill_HeaderId(dataSet_Sale.Table_011_Child1_SaleFactor, _SaleID);
                table_012_Child2_SaleFactorTableAdapter.Fill_HeaderID(dataSet_Sale.Table_012_Child2_SaleFactor, _SaleID);

                SaleRow["Column15"] = Class_BasicOperation._UserName;
                SaleRow["Column16"] = Class_BasicOperation.ServerDate();
                SaleRow["Column34"] = gridEX_List.GetTotal(
                    gridEX_List.RootTable.Columns["Column19"],
                    AggregateFunction.Sum).ToString();
                SaleRow["Column35"] = gridEX_List.GetTotal(
                    gridEX_List.RootTable.Columns["Column17"],
                    AggregateFunction.Sum).ToString();

                //****************Calculate Discounts
                double NetTotal = Convert.ToDouble(
                    gridEX_List.GetTotal(gridEX_List.RootTable.Columns["Column20"],
                    AggregateFunction.Sum).ToString());
                int CustomerCode = int.Parse(SaleRow["Column03"].ToString());
                string Date = SaleRow["Column02"].ToString();
                SaleRow["Column28"] = NetTotal;

                NetTotal = ClDiscount.SpecialGroup(
                    Convert.ToDouble(SaleRow["Column28"].ToString()), CustomerCode, Date);
                SaleRow["Column30"] = NetTotal;

                NetTotal = ClDiscount.VolumeGroup(
                    Convert.ToDouble(SaleRow["Column28"].ToString()) -
                    Convert.ToDouble(SaleRow["Column30"].ToString()), CustomerCode, Date);
                SaleRow["Column29"] = NetTotal;

                NetTotal = ClDiscount.SpecialCustomer(
                    Convert.ToDouble(SaleRow["Column28"].ToString()) -
                    Convert.ToDouble(SaleRow["Column30"].ToString()) -
                    Convert.ToDouble(SaleRow["Column29"].ToString()), CustomerCode, Date);
                SaleRow["Column31"] = NetTotal;

                //Extra-Reductions
                Janus.Windows.GridEX.GridEXFilterCondition Filter =
                    new GridEXFilterCondition(gridEX_Extra.RootTable.Columns["Column05"],
                        ConditionOperator.Equal, false);
                SaleRow["Column32"] = gridEX_Extra.GetTotal(
                    gridEX_Extra.RootTable.Columns["Column04"],
                    AggregateFunction.Sum, Filter).ToString();
                Filter.Value1 = true;
                SaleRow["Column33"] = gridEX_Extra.GetTotal(
                    gridEX_Extra.RootTable.Columns["Column04"],
                    AggregateFunction.Sum, Filter).ToString();

                //if (SaleRow["Column09"].ToString() == "0")
                //{
                //    //if (DialogResult.Yes == MessageBox.Show("آیا مایل به محاسبه جوایز هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                //    Classes.Class_Award.SaleAward_Box(int.Parse(SaleRow["ColumnId"].ToString()),
                //        SaleRow["Column02"].ToString(), (SaleRow["Column07"].ToString() == "" ? 0 : int.Parse(SaleRow["Column07"].ToString())), mnu_CalculatePrice.Checked);
                //}


                this.table_010_SaleFactorBindingSource.EndEdit();
                this.table_010_SaleFactorTableAdapter.Update(dataSet_Sale.Table_010_SaleFactor);
                clDoc.Update_Des_Table(ConSale.ConnectionString,
                  "Table_007_FactorBefore", "Column12", "ColumnId", int.Parse(_Row["ColumnId"].ToString()), _SaleID);

                Class_BasicOperation.ShowMsg("", "ثبت فاکتور فروش با موفقیت انجام گرفت" +
                    Environment.NewLine + "شماره فاکتور فروش:" +
                  gridEX1.GetValue("Column01").ToString(), "Information");
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            }
            catch (System.Data.SqlClient.SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, this.Name);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);

            }
        }

   

       
    }
}
