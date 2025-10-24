using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevComponents.DotNetBar;
using Janus.Windows.GridEX;

namespace PSHOP._08_Order2
{
    public partial class Form07_OrderDraft : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        DataTable M_DT;
        int DraftNum = 0;
        SqlParameter DraftId;
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        public Form07_OrderDraft()
        {
            InitializeComponent();
        }

        private void Form07_OrderDraft_Load(object sender, EventArgs e)
        {



            try
            {

                DataSet DS = new DataSet();
                SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT [columnid]
                                                              ,[column01]
                                                              ,[column02] from Table_002_SalesTypes", ConBase);
                Adapter.Fill(DS, "SaleType");

                this.gridEX1.DropDowns["Ware"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from Table_001_PWHRS");
                this.gridEX1.DropDowns["DraftType"].DataSource = clDoc.ReturnTable(Properties.Settings.Default.WHRS, "Select * from table_005_PwhrsOperation where Column16=1");

                this.gridEX1.DropDowns["SaleType"].DataSource = DS.Tables["SaleType"];
                this.gridEX1.DropDowns["SaleBoss"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, @"SELECT dbo.Table_045_PersonInfo.ColumnId AS id,
                                                                                           dbo.Table_045_PersonInfo.Column01 AS code,
                                                                                           dbo.Table_045_PersonInfo.Column02 AS name
                                                                                    FROM   dbo.Table_045_PersonInfo
                                                                                    WHERE  dbo.Table_045_PersonInfo.ColumnId 
                                                                                           IN (SELECT tps.Column01
                                                                                               FROM   Table_045_PersonScope tps
                                                                                               WHERE  tps.Column02 = 3)");
                DataTable CustomerTable = clDoc.ReturnTable
               (ConBase.ConnectionString, @"SELECT     dbo.Table_045_PersonInfo.ColumnId ,
                                                        dbo.Table_045_PersonInfo.Column01 ,
                                                        dbo.Table_045_PersonInfo.Column02 
                        FROM       
                                              dbo.Table_045_PersonInfo  
                              ");
                this.gridEX1.DropDowns["Customer"].DataSource = CustomerTable;

                //خواندن کالاها
                DataTable GoodTable = clGood.MahsoolInfo( 0);
                GoodbindingSource.DataSource = clGood.MahsoolInfo( 0);
                gridEX_Detail.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
                gridEX_Detail.DropDowns["GoodName"].SetDataBinding(GoodTable, "");







                dataSet_Foroosh.EnforceConstraints = false;
                this.table_005_OrderHeaderTableAdapter.FillByHavale(this.dataSet_Foroosh.Table_005_OrderHeader);
                this.table_006_OrderDetailsTableAdapter.FillByHavale(this.dataSet_Foroosh.Table_006_OrderDetails);
                dataSet_Foroosh.EnforceConstraints = true;





                this.WindowState = FormWindowState.Maximized;




            }
            catch
            {

            }




        }

        private void FirstRemain(int GoodCode, string Ware, string Date)
        {


            M_DT = new DataTable();

            string CommandText = @"SELECT     View_2.column02, ISNULL(View_3.DCarSum, 0) AS 
            DCarSum, ISNULL(View_3.DPakSum, 0) AS DPakSum, 
            ISNULL(View_3.DNumSum, 0) AS DNumSum, 
            ISNULL(View_3.DTNumSum, 0) AS DTNumSum, 
            View_2.RCarSum, View_2.RPakSum, 
            View_2.RNumSum, View_2.RTNumSum, 
            View_2.RCarSum - ISNULL(View_3.DCarSum, 0) AS 
            MCarSum, View_2.RPakSum - ISNULL(View_3.DPakSum, 0) AS 
            MPakSum, View_2.RNumSum - ISNULL(View_3.DNumSum, 0) AS 
            MNumSum, View_2.RTNumSum - ISNULL(View_3.DTNumSum, 0) 
            AS MTNumSum
            FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02, 
            SUM(dbo.Table_012_Child_PwhrsReceipt.column04) AS RCarSum, 
            SUM(dbo.Table_012_Child_PwhrsReceipt.column05) AS RPakSum, 
            SUM(dbo.Table_012_Child_PwhrsReceipt.column06) AS RNumSum, 
            SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS RTNumSum
            FROM         dbo.Table_012_Child_PwhrsReceipt INNER JOIN
            dbo.Table_011_PwhrsReceipt ON 
            dbo.Table_012_Child_PwhrsReceipt.column01 = 
            dbo.Table_011_PwhrsReceipt.columnid
            WHERE     (dbo.Table_011_PwhrsReceipt.column02 <= N'{2}') AND 
            (dbo.Table_011_PwhrsReceipt.column03 = {0}) AND 
            (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
            GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02) as 
            View_2 LEFT OUTER JOIN
            (SELECT     dbo.Table_008_Child_PwhrsDraft.column02, 
            SUM(dbo.Table_008_Child_PwhrsDraft.column04) AS DCarSum, 
            SUM(dbo.Table_008_Child_PwhrsDraft.column05) AS DPakSum, 
            SUM(dbo.Table_008_Child_PwhrsDraft.column06) AS DNumSum, 
            SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS DTNumSum
            FROM         dbo.Table_008_Child_PwhrsDraft INNER JOIN
            dbo.Table_007_PwhrsDraft ON 
            dbo.Table_008_Child_PwhrsDraft.column01 = 
            dbo.Table_007_PwhrsDraft.columnid
            WHERE     (dbo.Table_007_PwhrsDraft.column02 <= N'{2}') AND 
            (dbo.Table_007_PwhrsDraft.column03 = {0}) AND 
            (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
            GROUP BY dbo.Table_008_Child_PwhrsDraft.column02) as 
            View_3 ON View_2.column02 = View_3.column02";

            CommandText = string.Format(CommandText, Ware,
            GoodCode, Date);

            SqlDataAdapter M_Adapter = new SqlDataAdapter(CommandText, ConWare);
            M_Adapter.Fill(M_DT);

            //MessageBox.Show(M_DT.Rows.Count.ToString());
        }

        private void CheckEssentialItems(string date)
        {




            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Detail.GetRows())
            {


                DataTable dt = new DataTable();
                SqlDataAdapter Adapter = new SqlDataAdapter(@"select * from Table_006_OrderDetails where columnid=" + item.Cells["columnid"].Value + "", ConSale);
                Adapter.Fill(dt);

                if ((Convert.ToDouble(dt.Rows[0]["column17"].ToString()) + Convert.ToDouble(item.Cells["column17"].Value)) > Convert.ToDouble(dt.Rows[0]["column06"].ToString()))

                    throw new Exception("تعداد خروجی بیشتر از میزان سفارش است");



            }



            bool _Send2Exit = true;
            bool _MojodiIsLow = false;
            bool _ExtNumIsHigh = false;

            gridEX1.UpdateData();
            gridEX_Detail.UpdateData();
            gridEX_Detail.Update();


            if (gridEX1.GetRow().IsChecked)
            {

                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Detail.GetRows())
                {
                    if (!clGood.IsGoodInWare(Int16.Parse(gridEX1.GetValue("Column40").ToString()),
                        int.Parse(item.Cells["Column02"].Value.ToString())))
                        throw new Exception("کالای " + item.Cells[5].Text + " در این انبار فعال نمی باشد ");
                }

                //چک کردن اینکه تعداد خروجی صفر یا بیشتر از تعداد سفارش نباشد

                if ((double.Parse(gridEX_Detail.GetTotal(gridEX_Detail.RootTable.Columns["Column17"],
                    AggregateFunction.Sum).ToString()) == 0) &
                (double.Parse(gridEX_Detail.GetTotal(gridEX_Detail.RootTable.Columns["Column16"],
                AggregateFunction.Sum).ToString()) == 0))
                    _Send2Exit = false;

                if (_Send2Exit)
                {
                    //تعداد خروجی
                    for (int i = 0; i < gridEX_Detail.RowCount; i++)
                    {
                        gridEX_Detail.Row = i;

                        gridEX_Detail.SetValue("Selector", false);
                        gridEX_Detail.UpdateData();


                        if ((double.Parse(gridEX_Detail.GetValue("Column17").ToString()) >
                            double.Parse(gridEX_Detail.GetValue("Column06").ToString())) |
                            (double.Parse(gridEX_Detail.GetValue("Column16").ToString()) >
                            double.Parse(gridEX_Detail.GetValue("Column04").ToString())))
                        {
                            _Send2Exit = false;
                            _ExtNumIsHigh = true;
                            gridEX_Detail.SetValue("Selector", true);
                            gridEX_Detail.UpdateData();
                        }


                    }
                    if (_ExtNumIsHigh)
                        throw new Exception("تعداد خروجی بیشتر از میزان سفارش است");

                    //چک کردن مانده موجودی
                    if (!_ExtNumIsHigh)
                    {
                        for (int j = 0; j < gridEX_Detail.RowCount; j++)
                        {
                            gridEX_Detail.Row = j;

                            double _17 = double.Parse(gridEX_Detail.GetValue("Column17").ToString());
                            double _16 = double.Parse(gridEX_Detail.GetValue("Column16").ToString());
                            double M17 = 0;
                            double M16 = 0;

                            if ((double.Parse(gridEX_Detail.GetValue("Column17").ToString()) > 0) ||
                                   (double.Parse(gridEX_Detail.GetValue("Column16").ToString()) > 0))
                            {
                                FirstRemain(int.Parse(gridEX_Detail.GetValue("Column02").ToString()), gridEX1.GetValue("Column40").ToString(), date);

                                if (M_DT.Rows.Count == 0)
                                {
                                    _Send2Exit = false;
                                    _MojodiIsLow = true;
                                    gridEX_Detail.SetValue("Selector", true);
                                }
                                else
                                {
                                    M17 = double.Parse(M_DT.Rows[0]["MTNumSum"].ToString());
                                    M16 = double.Parse(M_DT.Rows[0]["MCarSum"].ToString());

                                    double TotalNumber = 0;
                                    foreach (Janus.Windows.GridEX.GridEXRow dr in gridEX_Detail.GetRows())
                                    {
                                        if (dr.Cells["Column02"].Value.ToString() == gridEX_Detail.GetValue("Column02").ToString())
                                            TotalNumber += Convert.ToDouble(dr.Cells["Column17"].Value);
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
                                                                                   WHERE  ColumnId = " + gridEX_Detail.GetValue("Column02") + @"
                                                                               ),
                                                                               0
                                                                           ) AS Column16", ConWareGood);
                                            mojoodimanfi = Convert.ToBoolean(Command.ExecuteScalar());

                                        }
                                    }
                                    catch
                                    {
                                    }

                                    if (
                                        (double.Parse(M_DT.Rows[0]["MTNumSum"].ToString()) < TotalNumber && !mojoodimanfi)
                                        //|
                                        //(double.Parse(M_DT.Rows[0]["MCarSum"].ToString()) <
                                        //double.Parse(gridEX_Detail.GetValue("Column16").ToString()))
                                        )
                                    {
                                        _Send2Exit = false;
                                        _MojodiIsLow = true;
                                        gridEX_Detail.SetValue("Selector", true);
                                    }
                                }

                                gridEX_Detail.SetValue("Cartoon", M16);
                                gridEX_Detail.SetValue("Mojodi", M17);
                                gridEX_Detail.UpdateData();
                            }
                        }

                        if (_MojodiIsLow)
                            throw new Exception("موجودی کالا در سطرهای مشخص شده کافی نیست");
                    }

                }
                else
                    throw new Exception("تعداد خروجی هیچ کالایی مشخص نیست");
            }

        }

        private void table_005_OrderHeaderBindingSource_PositionChanged(object sender, EventArgs e)
        {


        }

        private void gridEX1_RowCheckStateChanged(object sender, RowCheckStateChangeEventArgs e)
        {
            try
            {
                {
                    if (e.CheckState == Janus.Windows.GridEX.RowCheckState.Checked)
                    {
                        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Detail.GetRows())
                        {
                            item.BeginEdit();
                            //عدد
                            if (Convert.ToDouble(item.Cells["Column14"].Value) == 0)
                                item.Cells["Column14"].Value = item.Cells["Column05"].Value.ToString();

                            //کارتن
                            if (Convert.ToDouble(item.Cells["Column16"].Value) == 0)

                                item.Cells["Column16"].Value = item.Cells["Column04"].Value.ToString();
                            //بسته
                            if (Convert.ToDouble(item.Cells["Column15"].Value) == 0)

                                item.Cells["Column15"].Value = item.Cells["Column03"].Value.ToString();
                            //کل
                            gridEX_Detail.SetValue("column17",
               Convert.ToDouble((Convert.ToDecimal(gridEX_Detail.GetValue("column16").ToString()) *
                Convert.ToDecimal(gridEX_Detail.GetValue("column28").ToString())) +
                (Convert.ToDecimal(gridEX_Detail.GetValue("column15").ToString()) *
                Convert.ToDecimal(gridEX_Detail.GetValue("column29").ToString())) +
                Convert.ToDecimal(gridEX_Detail.GetValue("column14").ToString())));

                            //item.Cells["Column17"].Value = item.Cells["Column06"].Value.ToString();

                            //if (Convert.ToDouble(item.Cells["column04"].Value) > 0)
                            //{
                            //    item.Cells["column16"].Value = Convert.ToDouble(item.Cells["BoxDiff"].Value);
                            //}


                            item.EndEdit();
                        }
                    }
                    else
                    {
                        //foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Detail.GetRows())
                        //{
                        //    item.BeginEdit();
                        //    //عدد


                        //    item.Cells["Column14"].Value = 0;
                        //    //کارتن
                        //    item.Cells["Column16"].Value = 0;
                        //    //بسته
                        //    item.Cells["Column15"].Value = 0;
                        //    //کل
                        //    item.Cells["Column17"].Value = 0;
                        //    item.EndEdit();
                        //}
                    }
                }


                gridEX_Detail.UpdateData();
            }
            catch { }

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {

            try
            {

                if (gridEX1.GetCheckedRows().Count() != 1)
                {
                    Class_BasicOperation.ShowMsg("", "فقط یک سفارش انتخاب کنید", "Information");
                    return;
                }

                if (gridEX1.
                    GetRow().IsChecked)
                {
                    string Date = string.Empty;

                    Form05_DraftTime frm = new Form05_DraftTime();
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                    {

                        Date = frm.Date;
                    }


                    if (!string.IsNullOrEmpty(Date))
                    {

                        CheckEssentialItems(Date);
                        if (DialogResult.Yes == MessageBox.Show("آیا مایل به صدور حواله هستید؟",
                      "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                      MessageBoxDefaultButton.Button1,
                      MessageBoxOptions.RightAlign))
                        {
                            this.Cursor = Cursors.WaitCursor;
                            gridEX_Detail.UpdateData();

                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                            {
                                Con.Open();

                                SqlTransaction sqlTran = Con.BeginTransaction();
                                SqlCommand Command = Con.CreateCommand();
                                Command.Transaction = sqlTran;

                                try
                                {
                                    Command.CommandText = GenerateCommandText(Date);
                                    Command.Parameters.Add(DraftId);
                                    Command.ExecuteNonQuery();
                                    sqlTran.Commit();
                                }
                                catch (Exception es)
                                {
                                    sqlTran.Rollback();
                                    this.Cursor = Cursors.Default;
                                    Class_BasicOperation.CheckExceptionType(es, this.Name);
                                }

                                //محاسبه ارزش
                                try
                                {
                                    Fifo(Date);
                                }
                                catch
                                {
                                }
                                int i = gridEX1.CurrentRow.Position;
                                int j = gridEX_Detail.CurrentRow.Position;

                                dataSet_Foroosh.EnforceConstraints = false;
                                this.table_005_OrderHeaderTableAdapter.FillByHavale(this.dataSet_Foroosh.Table_005_OrderHeader);
                                this.table_006_OrderDetailsTableAdapter.FillByHavale(this.dataSet_Foroosh.Table_006_OrderDetails);
                                dataSet_Foroosh.EnforceConstraints = true;
                                gridEX1.MoveTo(i);
                                gridEX_Detail.MoveTo(j);
                                Class_BasicOperation.ShowMsg("", "ثبت اطلاعات انجام شد" +

                                    Environment.NewLine + "شماره حواله انبار:" +
                                  DraftNum.ToString(), "Information");



                                this.Cursor = Cursors.Default;



                            }
                        }


                    }
                }

                else
                    Class_BasicOperation.ShowMsg("", "سفارشی انتخاب نشده است", "Information");
            }



            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
                this.Cursor = Cursors.Default;
            }




        }

        private void gridEX_Detail_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
                GoodbindingSource.Filter = "GoodID=" + gridEX_Detail.GetValue("Column02").ToString();
                gridEX_Detail.SetValue("Column01", gridEX1.GetValue("columnid"));
                gridEX_Detail.SetValue("Column10", Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodbindingSource.CurrencyManager.Current)["SaleBoxPrice"].ToString())));
                gridEX_Detail.SetValue("Column09", Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePackPrice"].ToString())));
                gridEX_Detail.SetValue("Column08", Convert.ToInt64(Convert.ToDouble(((DataRowView)GoodbindingSource.CurrencyManager.Current)["SalePrice"].ToString())));
                gridEX_Detail.SetValue("Column11", 0);
                gridEX_Detail.SetValue("Column12", 0);
                gridEX_Detail.SetValue("Column28", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInBox"].ToString());
                gridEX_Detail.SetValue("Column29", ((DataRowView)GoodbindingSource.CurrencyManager.Current)["NumberInPack"].ToString());

                gridEX_Detail.SetValue("Column19", Class_BasicOperation._UserName);
                gridEX_Detail.SetValue("Column20", Class_BasicOperation.ServerDate());
                gridEX_Detail.SetValue("Column21", Class_BasicOperation._UserName);
                gridEX_Detail.SetValue("Column22", Class_BasicOperation.ServerDate());

                //if (Convert.ToDouble(gridEX_Detail.GetValue("column04")) > 0)
                //{
                //    gridEX_Detail.SetValue("column16", Convert.ToDouble(gridEX_Detail.GetValue("BoxDiff")));
                //}

                try
                {

                    gridEX_Detail.SetValue("column17",
                    Convert.ToDouble((Convert.ToDecimal(gridEX_Detail.GetValue("column16").ToString()) *
                     Convert.ToDecimal(gridEX_Detail.GetValue("column28").ToString())) +
                     (Convert.ToDecimal(gridEX_Detail.GetValue("column15").ToString()) *
                     Convert.ToDecimal(gridEX_Detail.GetValue("column29").ToString())) +
                     Convert.ToDecimal(gridEX_Detail.GetValue("column14").ToString())));
                }
                catch
                {
                }
            }
            catch { }
        }

        private void gridEX_Detail_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column02");
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "column002");
            }
            catch { }
            try
            {
                //درج نام کالا، کد کالا
                if (e.Column.Key == "column002")
                    gridEX_Detail.SetValue("column02", gridEX_Detail.GetValue("column002").ToString());
                else if (e.Column.Key == "column02")
                    gridEX_Detail.SetValue("column002", gridEX_Detail.GetValue("column02").ToString());
            }
            catch { }
            try
            {
                gridEX_Detail.SetValue("column17",
                  Convert.ToDouble((Convert.ToDecimal(gridEX_Detail.GetValue("column16").ToString()) *
                   Convert.ToDecimal(gridEX_Detail.GetValue("column28").ToString())) +
                   (Convert.ToDecimal(gridEX_Detail.GetValue("column15").ToString()) *
                   Convert.ToDecimal(gridEX_Detail.GetValue("column29").ToString())) +
                   Convert.ToDecimal(gridEX_Detail.GetValue("column14").ToString())));


                if (!mnu_CalculatePrice.Checked)
                {
                    //gridEX_Order.SetValue("column13",
                    //    Convert.ToDouble(gridEX_Order.GetValue("column04")) *
                    //    Convert.ToDouble(gridEX_Order.GetValue("column10")));

                    gridEX_Detail.SetValue("column13",
                   (Convert.ToDouble(gridEX_Detail.GetValue("column16")) *
                   Convert.ToDouble(gridEX_Detail.GetValue("column10"))) +
                     (Convert.ToDouble(gridEX_Detail.GetValue("column15")) *
                   Convert.ToDouble(gridEX_Detail.GetValue("column09"))) +
                     (Convert.ToDouble(gridEX_Detail.GetValue("column14")) *
                   Convert.ToDouble(gridEX_Detail.GetValue("column08")))
                   );

                }
                else
                {
                    gridEX_Detail.SetValue("column13",
                            Convert.ToInt64(Convert.ToDouble(gridEX_Detail.GetValue("column08").ToString()) *
                         Convert.ToDouble(gridEX_Detail.GetValue("column17"))));
                }


            }
            catch
            {


            }
        }

        private void gridEX_Detail_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX_Detail_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX_Detail.CurrentCellDroppedDown = true;
            try
            {
                if (e.Column.Key == "column002")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column002", "GoodCode", "GoodName", gridEX_Detail.EditTextBox.Text, Class_BasicOperation.FilterColumnType.GoodCode);

                else if (e.Column.Key == "column02")
                    Class_BasicOperation.FilterGridExDropDown(sender, "column02", "GoodCode", "GoodName", gridEX_Detail.EditTextBox.Text, Class_BasicOperation.FilterColumnType.Others);
            }
            catch { }
        }

        private void gridEX_Detail_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            if (Convert.ToDouble(gridEX_Detail.GetValue("column06")) > 0)
            {
                e.Cancel = true;
                MessageBox.Show("امکان حذف سفارش های ثبت شده وجود ندارد");
            }
        }

        private void gridEX_Detail_EditingCell(object sender, EditingCellEventArgs e)
        {
            //try
            //{
            //    if (gridEX_Detail.GetValue("Column31").ToString() == "True")
            //        e.Cancel = true;
            //}
            //catch { }
        }

        private string GenerateCommandText(string Date)
        {
            Classes.Class_GoodInformation cl = new Classes.Class_GoodInformation();
            GridEXRow OrderRow = gridEX1.GetRow();

            DraftNum = clDoc.MaxNumber(Properties.Settings.Default.WHRS, "Table_007_PwhrsDraft", "Column01");

            DraftId = new SqlParameter("DraftID", SqlDbType.Int);
            DraftId.Direction = ParameterDirection.Output;

            string CommandTxt = null;
            int _OrderID = Convert.ToInt32(OrderRow.Cells["columnid"].Value);

            //درج حواله
            CommandTxt += @"
            INSERT INTO Table_007_PwhrsDraft  ([column01]
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
           ,[Column26]) Values ( " + DraftNum + " ,'" + Date
                            + "'," + OrderRow.Cells["Column40"].Value + "," +
                            OrderRow.Cells["Column41"].Value + "," +
                            OrderRow.Cells["Column03"].Value.ToString() +
                            ",'حواله صادر از سفارش ش " +
                            OrderRow.Cells["Column01"].Text.ToString() + "- تاریخ " +
                            OrderRow.Cells["Column02"].Text.ToString() + "-" +
                            OrderRow.Cells["column26"].Text.ToString() +
                            "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" +
                            Class_BasicOperation._UserName
                            + "',getdate(),0,'" + Class_BasicOperation._UserName + "',getdate(),0,0," +
                            _OrderID + ",0,0,0,0,0,0,null,0,1); SET @DraftID=SCOPE_IDENTITY(); ";

            //درج کالاها
            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Detail.GetRows())
            {
                if (double.Parse(item.Cells["Column17"].Value.ToString()) > 0)
                {
                    GoodbindingSource.Filter = "GoodID=" + item.Cells["Column02"].Value.ToString();

                    DataRowView GoodRow = (DataRowView)GoodbindingSource.CurrencyManager.Current;
                    double TotalPrice = 0;
                    Double SingleValue = 0;
                    if (!Class_BasicOperation._WareType)
                        SingleValue = cl.GoodValue(int.Parse(item.Cells["Column02"].Value.ToString()), short.Parse(OrderRow.Cells["Column40"].Value.ToString()));
                    Double TotalValue = Math.Round(SingleValue * Convert.ToDouble(item.Cells["Column17"].Value.ToString()), 4);

                    TotalPrice = Convert.ToDouble(item.Cells["column16"].Value.ToString()) *
                        Convert.ToDouble(item.Cells["Column10"].Value.ToString());
                    TotalPrice += (Convert.ToDouble(item.Cells["column15"].Value.ToString()) *
                        Convert.ToDouble(item.Cells["Column09"].Value.ToString()));
                    TotalPrice += (Convert.ToDouble(item.Cells["column14"].Value.ToString()) *
                        Convert.ToDouble(item.Cells["Column08"].Value.ToString()));

                    CommandTxt += @"INSERT INTO Table_008_Child_PwhrsDraft (
            [column01]
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
          , [Column33]
          , [Column34]
           , [Column35]) VALUES(@DraftID," +
                         item.Cells["Column02"].Value.ToString() + "," +
                     GoodRow["CountUnit"].ToString() + "," +
                     item.Cells["Column16"].Value.ToString() +
                     "," + item.Cells["Column15"].Value.ToString() + ","
                     + item.Cells["Column14"].Value.ToString() + "," +
                     item.Cells["Column17"].Value.ToString() + "," +
                     item.Cells["Column10"].Value.ToString() + "," +
                     item.Cells["Column09"].Value.ToString() + "," +
                     item.Cells["Column08"].Value.ToString() + "," +
                      item.Cells["Column13"].Value.ToString()
                     + ",NULL,NULL,NULL," + SingleValue + "," + TotalValue + ",'" + Class_BasicOperation._UserName +
                     "',getdate(),'" + Class_BasicOperation._UserName
                     + "',getdate(),NULL,NULL,NULL,0,0,0," + _OrderID + "," +
                     item.Cells["ColumnId"].Value.ToString() + "," +
                     (item.Cells["Column31"].Value.ToString() == "True" ? "1" : "0") + ",NULL,NULL," +
                     item.Cells["Column28"].Value.ToString() + "," + item.Cells["Column29"].Value.ToString() + "," + GoodRow["Weight"].ToString() + "," +
                     Convert.ToDouble(GoodRow["Weight"].ToString()) * Convert.ToDouble(item.Cells["Column17"].Value.ToString()) + ");";

                }
            }


            foreach (Janus.Windows.GridEX.GridEXRow item in gridEX_Detail.GetRows())
            {
                item.BeginEdit();

                DataTable dt = new DataTable();
                SqlDataAdapter Adapter = new SqlDataAdapter(@"select * from Table_006_OrderDetails where columnid=" + item.Cells["columnid"].Value + "", ConSale);
                Adapter.Fill(dt);

                //try
                //{

                CommandTxt += "UPDATE " + ConSale.Database + ".dbo.Table_006_OrderDetails set column14=" + (Convert.ToDouble(dt.Rows[0]["column14"]) + Convert.ToDouble(item.Cells["column14"].Value)) + " , Column23=1,Column24='" + Date + "',Column25=getdate(),Column26='" + Class_BasicOperation._UserName + "',Column27=" + Convert.ToInt32(DraftId.Value) + " where columnid=" + item.Cells["columnid"].Value + " ";
                CommandTxt += "UPDATE " + ConSale.Database + ".dbo.Table_006_OrderDetails set column15=" + (Convert.ToDouble(dt.Rows[0]["column15"]) + Convert.ToDouble(item.Cells["column15"].Value)) + " , Column23=1,Column24='" + Date + "',Column25=getdate(),Column26='" + Class_BasicOperation._UserName + "',Column27=" + Convert.ToInt32(DraftId.Value) + " where columnid=" + item.Cells["columnid"].Value + " ";
                CommandTxt += "UPDATE " + ConSale.Database + ".dbo.Table_006_OrderDetails set column16=" + (Convert.ToDouble(dt.Rows[0]["column16"]) + Convert.ToDouble(item.Cells["column16"].Value)) + " , Column23=1,Column24='" + Date + "',Column25=getdate(),Column26='" + Class_BasicOperation._UserName + "',Column27=" + Convert.ToInt32(DraftId.Value) + " where columnid=" + item.Cells["columnid"].Value + " ";
                CommandTxt += "UPDATE " + ConSale.Database + ".dbo.Table_006_OrderDetails set column17=" + (Convert.ToDouble(dt.Rows[0]["column17"]) + Convert.ToDouble(item.Cells["column17"].Value)) + " , Column23=1,Column24='" + Date + "',Column25=getdate(),Column26='" + Class_BasicOperation._UserName + "',Column27=" + Convert.ToInt32(DraftId.Value) + " where columnid=" + item.Cells["columnid"].Value + " ";




            }





            //ثبت شماره برگه خروج در کالاهای سفارش
            //CommandTxt += "UPDATE "+ConSale.Database+".dbo.Table_006_OrderDetails set Column23=1,Column24='" + Date + "',Column25=getdate(),Column26='" + Class_BasicOperation._UserName + "',Column27=" + Convert.ToInt32(DraftId.Value) + " where column01="+_OrderID+" ";
            //CommandTxt += "UPDATE " + ConSale.Database + ".dbo.Table_005_OrderHeader SET Column33=1,Column40=" + this.gridEX1.GetValue("Column40") + ",Column41=" + this.gridEX1.GetValue("Column41") + ",column30=getdate(),column31='" + Class_BasicOperation._UserName + "' where ColumnId=" + _OrderID;





            //   clDoc.RunSqlCommand(ConSale.ConnectionString, "UPDATE Table_005_OrderHeader SET Column33=1,Column40=" + this.gridEX1.GetValue("Column40") + ",Column41=" + this.gridEX1.GetValue("Column41") + ",column30=getdate(),column31='" + Class_BasicOperation._UserName + "' where ColumnId=" +
            //this.gridEX1.GetValue("columnid"));

            return CommandTxt;
        }

        private void Fifo(string Date)
        {
            //محاسبه ارزش حواله
            SqlDataAdapter goodAdapter = new SqlDataAdapter(
                "Select * from Table_008_Child_PwhrsDraft where Column01=" + DraftId.Value, ConWare);
            DataTable Table = new DataTable();
            goodAdapter.Fill(Table);



            //محاسبه ارزش و ذخیره آن در جدول Child1
            if (Class_BasicOperation._WareType)
            {
                foreach (DataRow item in Table.Rows)
                {
                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	 [dbo].[PR_00_NewFIFO]  @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + gridEX1.GetValue("column40"), ConWare);
                    DataTable TurnTable = new DataTable();
                    Adapter.Fill(TurnTable);
                    DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + Convert.ToInt32(DraftId.Value) + " and DetailID=" + item["Columnid"].ToString());
                    clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                        + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString());
                }
            }
            else
            {
                foreach (DataRow item in Table.Rows)
                {
                    //SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	 [dbo].[PR_05_AVG]  @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + gridEX1.GetValue("column40"), ConWare);
                    SqlDataAdapter Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item["Column02"].ToString() + ", @WareCode = " + gridEX1.GetValue("column40") + ",@Date='" + Date + "',@id=" + DraftId.Value + ",@residid=0", ConWare);

                    DataTable TurnTable = new DataTable();
                    Adapter.Fill(TurnTable);
                    // DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + Convert.ToInt32(DraftId.Value) + " and DetailID=" + item["Columnid"].ToString());
                    clDoc.RunSqlCommand(ConWare.ConnectionString, "UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                        + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item["Column07"].ToString()), 4) + " where ColumnId=" + item["ColumnId"].ToString());

                }
            }



            ////محاسبه ارزش و ذخیره آن در جدول Child1
            //using (SqlConnection Connection = new SqlConnection(Properties.Settings.Default.WHRS))
            //{
            //    Connection.Open();
            //    foreach (DataRow item in Table.Rows)
            //    {
            //        SqlDataAdapter Adapter = new SqlDataAdapter(
            //            "EXEC	 [dbo].[PR_00_NewFIFO]  @GoodParameter = " +
            //            item["Column02"].ToString() + ", @WareCode = " +
            //            gridEX1.GetValue("Column40"), Connection);
            //        DataTable TurnTable = new DataTable();
            //        Adapter.Fill(TurnTable);
            //        DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + DraftId.Value +
            //            " and DetailID=" + item["Columnid"].ToString());
            //        SqlCommand UpdateCommand = new SqlCommand(
            //            "UPDATE Table_008_Child_PwhrsDraft SET  Column15=" +
            //            Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
            //            + " , Column16=" + Math.Round(
            //            double.Parse(Row[0]["DTotalPrice"].ToString()), 4) +
            //            " where ColumnId=" + item["ColumnId"].ToString(), Connection);
            //        UpdateCommand.ExecuteNonQuery();
            //    }
            //}

        }

        private string OrderSelectCommands(string WhereType)
        {
            string CommandText = null;

            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 125))
            {
                CommandText =
                    @"SELECT     dbo.Table_005_OrderHeader.column01, dbo.Table_005_OrderHeader.column02, dbo.Table_005_OrderHeader.column03, dbo.Table_005_OrderHeader.column04, 
                    dbo.Table_005_OrderHeader.column05, dbo.Table_005_OrderHeader.column06, dbo.Table_005_OrderHeader.column07, dbo.Table_005_OrderHeader.column08, 
                    dbo.Table_005_OrderHeader.column09, dbo.Table_005_OrderHeader.column10, dbo.Table_005_OrderHeader.column11, dbo.Table_005_OrderHeader.column12, 
                    dbo.Table_005_OrderHeader.column13, dbo.Table_005_OrderHeader.column14, dbo.Table_005_OrderHeader.column15, dbo.Table_005_OrderHeader.column16, 
                    dbo.Table_005_OrderHeader.column17, dbo.Table_005_OrderHeader.column18, dbo.Table_005_OrderHeader.column19, dbo.Table_005_OrderHeader.column20, 
                    dbo.Table_005_OrderHeader.column21, dbo.Table_005_OrderHeader.column22, dbo.Table_005_OrderHeader.column23, dbo.Table_005_OrderHeader.column24, 
                    dbo.Table_005_OrderHeader.column25, dbo.Table_005_OrderHeader.column26, dbo.Table_005_OrderHeader.column27, dbo.Table_005_OrderHeader.column28, 
                    dbo.Table_005_OrderHeader.column29, dbo.Table_005_OrderHeader.column30, dbo.Table_005_OrderHeader.column31, dbo.Table_005_OrderHeader.column32, 
                    dbo.Table_005_OrderHeader.column33, dbo.Table_005_OrderHeader.column34, dbo.Table_005_OrderHeader.column35, dbo.Table_005_OrderHeader.column36, 
                    dbo.Table_005_OrderHeader.column37, dbo.Table_005_OrderHeader.column38, dbo.Table_005_OrderHeader.column39, dbo.Table_005_OrderHeader.column40, dbo.Table_005_OrderHeader.column41
                    , dbo.Table_005_OrderHeader.columnid, derivedtbl_1.TotalBox, 
                    derivedtbl_1.TotalExitBox, derivedtbl_1.Difference, CityTable.Column00 AS Province, ExitTable.column02 AS ExitDate
                    FROM         dbo.Table_005_OrderHeader INNER JOIN
                    (SELECT     column01, SUM(column04) AS TotalBox, SUM(column16) AS TotalExitBox, SUM(column04) - SUM(column16) AS Difference
                    FROM         dbo.Table_006_OrderDetails
                    GROUP BY column01) AS derivedtbl_1 ON dbo.Table_005_OrderHeader.columnid = derivedtbl_1.column01 LEFT OUTER JOIN
                    (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, 
                    column14, column15, column16, column17, column18, column19, column20, column21, column22, column23, column24
                    FROM         " + ConWare.Database + @".dbo.Table_009_ExitPwhrs) AS ExitTable ON dbo.Table_005_OrderHeader.columnid = ExitTable.column18 LEFT OUTER JOIN
                    (SELECT     Column00, Column01
                    FROM         " + ConBase.Database + @".dbo.Table_065_CityInfo) AS CityTable ON dbo.Table_005_OrderHeader.column05 = CityTable.Column01 
                    WHERE     (NOT (dbo.Table_005_OrderHeader.column03 IN
                    (SELECT     " + ConBase.Database + @".dbo.Table_045_PersonScope.Column01
                    FROM         " + ConBase.Database + @".dbo.Table_040_PersonGroups INNER JOIN
                    " + ConBase.Database + @".dbo.Table_045_PersonScope ON 
                    " + ConBase.Database + @".dbo.Table_040_PersonGroups.Column00 = " + ConBase.Database +
@".dbo.Table_045_PersonScope.Column02
                    WHERE     (" +
                                 ConBase.Database +
                                  ".dbo.Table_040_PersonGroups.Column29 = 1))))";



            }

            else
            {
                CommandText =
                        @"SELECT     dbo.Table_005_OrderHeader.column01, dbo.Table_005_OrderHeader.column02, dbo.Table_005_OrderHeader.column03, dbo.Table_005_OrderHeader.column04, 
                    dbo.Table_005_OrderHeader.column05, dbo.Table_005_OrderHeader.column06, dbo.Table_005_OrderHeader.column07, dbo.Table_005_OrderHeader.column08, 
                    dbo.Table_005_OrderHeader.column09, dbo.Table_005_OrderHeader.column10, dbo.Table_005_OrderHeader.column11, dbo.Table_005_OrderHeader.column12, 
                    dbo.Table_005_OrderHeader.column13, dbo.Table_005_OrderHeader.column14, dbo.Table_005_OrderHeader.column15, dbo.Table_005_OrderHeader.column16, 
                    dbo.Table_005_OrderHeader.column17, dbo.Table_005_OrderHeader.column18, dbo.Table_005_OrderHeader.column19, dbo.Table_005_OrderHeader.column20, 
                    dbo.Table_005_OrderHeader.column21, dbo.Table_005_OrderHeader.column22, dbo.Table_005_OrderHeader.column23, dbo.Table_005_OrderHeader.column24, 
                    dbo.Table_005_OrderHeader.column25, dbo.Table_005_OrderHeader.column26, dbo.Table_005_OrderHeader.column27, dbo.Table_005_OrderHeader.column28, 
                    dbo.Table_005_OrderHeader.column29, dbo.Table_005_OrderHeader.column30, dbo.Table_005_OrderHeader.column31, dbo.Table_005_OrderHeader.column32, 
                    dbo.Table_005_OrderHeader.column33, dbo.Table_005_OrderHeader.column34, dbo.Table_005_OrderHeader.column35, dbo.Table_005_OrderHeader.column36, 
                    dbo.Table_005_OrderHeader.column37, dbo.Table_005_OrderHeader.column38, dbo.Table_005_OrderHeader.column39, dbo.Table_005_OrderHeader.column40, dbo.Table_005_OrderHeader.column41,
                dbo.Table_005_OrderHeader.columnid, derivedtbl_1.TotalBox, 
                    derivedtbl_1.TotalExitBox, derivedtbl_1.Difference 
                    FROM         dbo.Table_005_OrderHeader INNER JOIN
                    (SELECT     column01, SUM(column04) AS TotalBox, SUM(column16) AS TotalExitBox, SUM(column04) - SUM(column16) AS Difference
                    FROM         dbo.Table_006_OrderDetails
                    GROUP BY column01) AS derivedtbl_1 ON dbo.Table_005_OrderHeader.columnid = derivedtbl_1.column01 ";


            }



            return CommandText;
        }

        private void Form07_OrderDraft_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Detail.RemoveFilters();
            gridEX1.RemoveFilters();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            Form07_OrderDraft_Load(null, null);
        }

    }
}
