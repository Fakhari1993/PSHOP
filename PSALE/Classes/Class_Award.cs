using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
namespace PSHOP.Classes
{
    class  Class_Award
    {
        public static bool OrderCalculateAward(int OrderId, string OrderDate)
        {
            bool _Include = false;

            Classes.Class_GoodInformation clGood = new Class_GoodInformation();
            DataSet DataSet1;
            SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
            SqlConnection ConWhrs = new SqlConnection(Properties.Settings.Default.WHRS);
            ConSale.Open();

            //1: Delete All Gifts from Order
            SqlCommand Command = new SqlCommand(
                "Delete From Table_006_OrderDetails where Column31=1 and Column01=" + OrderId, ConSale);
            Command.ExecuteNonQuery();

            //1.1: Read GoodInformation
            BindingSource BindingSource1 = new BindingSource();
            BindingSource1.DataSource = clGood.GoodInfo();

            //2: Read Gifts for Order's Good
            SqlDataAdapter GiftAdapter = new SqlDataAdapter(@"
SELECT     dbo.Table_028_Award.column01 AS GoodID, dbo.Table_028_Award.column08 AS GiftGood, 
                      SUM(CASE WHEN dbo.Table_028_Award.column06 = dbo.Table_028_Award.column07 THEN Floor(dbo.Table_006_OrderDetails.column04 / dbo.Table_028_Award.column06) 
                      * dbo.Table_028_Award.column11 ELSE CASE WHEN dbo.Table_028_Award.column06 <= dbo.Table_006_OrderDetails.column04 AND 
                      dbo.Table_028_Award.column07 >= dbo.Table_006_OrderDetails.column04 THEN dbo.Table_028_Award.column11 ELSE 0 END END) AS GiftNumber, 
                      {3}.dbo.table_004_CommodityAndIngredients.column09 AS InCartoon, dbo.Table_006_OrderDetails.column04 AS QtyCartoon, dbo.Table_006_OrderDetails.column06 AS QtyTotal
FROM         dbo.Table_028_Award INNER JOIN
                      dbo.Table_006_OrderDetails ON dbo.Table_028_Award.column01 = dbo.Table_006_OrderDetails.column02 AND 
                      dbo.Table_028_Award.column06 < dbo.Table_006_OrderDetails.column04 INNER JOIN
                      dbo.Table_005_OrderHeader ON dbo.Table_006_OrderDetails.column01 = dbo.Table_005_OrderHeader.columnid INNER JOIN
                      {3}.dbo.table_004_CommodityAndIngredients ON dbo.Table_028_Award.column08 = {3}.dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
                      {2}.dbo.Table_045_PersonScope ON dbo.Table_005_OrderHeader.column03 = {2}.dbo.Table_045_PersonScope.Column01 AND 
                      dbo.Table_028_Award.column17 = {2}.dbo.Table_045_PersonScope.Column02
WHERE     (dbo.Table_028_Award.column02 <= '{1}') AND (dbo.Table_028_Award.column03 >= '{1}') AND (dbo.Table_006_OrderDetails.column01 = {0}) AND 
                      (dbo.Table_006_OrderDetails.column31 = 0) AND (dbo.Table_006_OrderDetails.column07 = 1)
GROUP BY dbo.Table_028_Award.column01, dbo.Table_028_Award.column08, {3}.dbo.table_004_CommodityAndIngredients.column09, dbo.Table_006_OrderDetails.column04, 
                      dbo.Table_006_OrderDetails.column06", ConSale);
            GiftAdapter.SelectCommand.CommandText = 
                string.Format(GiftAdapter.SelectCommand.CommandText, OrderId.ToString(),
                OrderDate, ConBase.Database, ConWhrs.Database);
            DataSet1 = new DataSet();
            GiftAdapter.Fill(DataSet1, "Gift");
            int GiftNumber = DataSet1.Tables["Gift"].Rows.Count;
            if (GiftNumber > 0)
            {
                //if (DialogResult.Yes == MessageBox.Show(
                //    "به این سفارش جایزه تعلق گرفته است. آیا مایل به اضافه کردن جوایز به سفارش هستید؟",
                //    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                //    MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                //{
                    foreach (DataRow item in DataSet1.Tables["Gift"].Rows)
                    {
                        BindingSource1.Filter = "GoodID=" + item["GoodID"].ToString();
                        if (BindingSource1.Count > 0)
                        {
                            if (float.Parse(item["GiftNumber"].ToString()) > 0)
                            {
                                DataRowView Row = ((DataRowView)BindingSource1.CurrencyManager.Current);
                                SqlCommand Insert = new SqlCommand(
                                    "INSERT INTO Table_006_OrderDetails VALUES(" +
                                    OrderId.ToString() + "," + item["GiftGood"].ToString() + " ,0," +
                                    (float.Parse(item["GiftNumber"].ToString())).ToString() + ",0," +
                                    (float.Parse(item["GiftNumber"].ToString()) *
                                    float.Parse(item["column09"].ToString())).ToString() +
                                    ",1,0,0,0,0,0,0,0,0,0,0,'" + Class_BasicOperation._UserName
                                + "',getdate(),'" + Class_BasicOperation._UserName +
                                "',getdate(),null,null,null,null,null," + Row["NumberInBox"].ToString() +
                                "," + Row["NumberInPack"].ToString() + "," +
                                (decimal.Parse(Row["Tavan"].ToString()) *
                                decimal.Parse(item["GiftNumber"].ToString())).ToString() + ",1," +
                                (decimal.Parse(Row["Hajm"].ToString()) *
                                decimal.Parse(item["GiftNumber"].ToString())).ToString() + ")", ConSale);
                                Insert.ExecuteNonQuery();
                            }
                        }
                    }
                    _Include = true;

                //}
                //else
                //{
                //    _Include = false;
                //    return _Include;
                //}
            }
            return _Include;
        }

        public static bool SaleAward_Box(int SaleId, string SaleDate,int Prefactor,bool joz)
        {
            bool include = false;
            DataSet DataSet1;
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
            SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
            SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
            ConSale.Open();

            SqlDataAdapter GoodAdapter = new SqlDataAdapter("Select ColumnId,Column01,Column02,Column07,Column09,Column08,Column22,Column34,Column40 from table_004_CommodityAndIngredients", ConWare);
            DataTable GoodTable = new DataTable();
            GoodAdapter.Fill(GoodTable);
            
            SqlCommand Command = new SqlCommand(
                "Delete From Table_011_Child1_SaleFactor where Column30=1 and Column01=" + 
                SaleId, ConSale);
            Command.ExecuteNonQuery();

            //2: Read Gifts for Factor's Good - Cartoon

            SqlDataAdapter GiftAdapter = new SqlDataAdapter(@"
            SELECT     dbo.Table_028_Award.column01 AS GoodID, dbo.Table_028_Award.column08 AS GiftGood, 
            SUM(CASE WHEN dbo.Table_028_Award.column06 = dbo.Table_028_Award.column07 THEN Floor(dbo.Table_011_Child1_SaleFactor.Column04 / dbo.Table_028_Award.column06) 
            * dbo.Table_028_Award.column11 ELSE CASE WHEN dbo.Table_028_Award.column06 <= dbo.Table_011_Child1_SaleFactor.Column04 AND 
            dbo.Table_028_Award.column07 >= dbo.Table_011_Child1_SaleFactor.Column04 THEN dbo.Table_028_Award.column11 ELSE 0 END END) AS GiftNumber, 
            {3}.dbo.table_004_CommodityAndIngredients.column09 AS InCartoon
            FROM         dbo.Table_028_Award INNER JOIN
            dbo.Table_011_Child1_SaleFactor ON dbo.Table_028_Award.column01 = dbo.Table_011_Child1_SaleFactor.column02  INNER JOIN
            dbo.Table_010_SaleFactor ON dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid INNER JOIN
            {3}.dbo.table_004_CommodityAndIngredients ON dbo.Table_028_Award.column08 = {3}.dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
            {2}.dbo.Table_045_PersonScope ON dbo.Table_010_SaleFactor.column03 = {2}.dbo.Table_045_PersonScope.Column01 AND 
            dbo.Table_028_Award.column17 = {2}.dbo.Table_045_PersonScope.Column02
            WHERE     (dbo.Table_028_Award.column02 <= '{1}') AND (dbo.Table_028_Award.column03 >= '{1}') AND (dbo.Table_011_Child1_SaleFactor.column01 = {0}) AND 
            (dbo.Table_011_Child1_SaleFactor.column30 = 0) 
            GROUP BY dbo.Table_028_Award.column01, dbo.Table_028_Award.column08, {3}.dbo.table_004_CommodityAndIngredients.column09", ConSale);


//            //2: Read Gifts for Factor's Good - Kol
//            SqlDataAdapter GiftAdapter = new SqlDataAdapter(@"
//SELECT     dbo.Table_028_Award.column01 AS GoodID, dbo.Table_028_Award.column08 AS GiftGood, 
//SUM(CASE WHEN dbo.Table_028_Award.column06 = dbo.Table_028_Award.column07 THEN 
//Floor(dbo.Table_011_Child1_SaleFactor.column07 / dbo.Table_028_Award.column06) * 
//dbo.Table_028_Award.column11 ELSE dbo.Table_028_Award.column11 END) AS GiftNumber, 
//{3}.dbo.table_004_CommodityAndIngredients.column09 
//
//FROM         {2}.dbo.Table_045_PersonScope INNER JOIN
//                      {3}.dbo.table_004_CommodityAndIngredients INNER JOIN
//                      dbo.Table_028_Award ON {3}.dbo.table_004_CommodityAndIngredients.columnid = 
//dbo.Table_028_Award.column08 INNER JOIN
//                      dbo.Table_010_SaleFactor INNER JOIN
//                      dbo.Table_011_Child1_SaleFactor ON dbo.Table_010_SaleFactor.columnid = 
//dbo.Table_011_Child1_SaleFactor.column01 ON 
//                      dbo.Table_028_Award.column01 = dbo.Table_011_Child1_SaleFactor.column02 ON 
//                      {2}.dbo.Table_045_PersonScope.Column01 = dbo.Table_010_SaleFactor.column03 AND 
//                      {2}.dbo.Table_045_PersonScope.Column02 = dbo.Table_028_Award.column17  
//
//WHERE     (dbo.Table_028_Award.column02 <= N'{1}') AND 
//(dbo.Table_028_Award.column03 >= N'{1}') AND (dbo.Table_011_Child1_SaleFactor.column01 = {0} 
//AND (dbo.Table_011_Child1_SaleFactor.column30 = 0))
//GROUP BY dbo.Table_028_Award.column01, dbo.Table_028_Award.column08, 
//{3}.dbo.table_004_CommodityAndIngredients.column09", ConSale);


            GiftAdapter.SelectCommand.CommandText = 
                string.Format(GiftAdapter.SelectCommand.CommandText, SaleId.ToString(),
                SaleDate, ConBase.Database, ConWare.Database);
            DataSet1 = new DataSet();
            GiftAdapter.Fill(DataSet1, "Gift");
            int GiftNumber = DataSet1.Tables["Gift"].Rows.Count;
            if (GiftNumber > 0)
            {
                //if (DialogResult.Yes == MessageBox.Show("به این فاکتور جایزه تعلق گرفته است. آیا مایل به اضافه کردن جوایز به فاکتور هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                //{
                    foreach (DataRow item in DataSet1.Tables["Gift"].Rows)
                    {
                        GoodTable.DefaultView.RowFilter = "ColumnId=" + item["GiftGood"].ToString();

                        if (float.Parse(item["GiftNumber"].ToString()) > 0)
                        {
                            //Cartoon
                            SqlCommand Insert = new SqlCommand(
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
           ,[Column37]) VALUES(" +
                                SaleId + "," + item["GiftGood"].ToString() + "," +
                                GoodTable.DefaultView[0]["Column07"].ToString() + "," +
                                Convert.ToDouble(item["GiftNumber"].ToString()) + ",0,0," +
                                (Convert.ToDouble(item["GiftNumber"].ToString()) *
                                    Convert.ToDouble(item["InCartoon"].ToString())).ToString() +
                                "," + (!joz ? GoodTable.DefaultView[0]["Column40"] : "0") + ",0," + (joz ? GoodTable.DefaultView[0]["Column34"] : "0") +
                                @"," + (joz ? Convert.ToDouble(GoodTable.DefaultView[0]["Column34"])*(Convert.ToDouble(item["GiftNumber"].ToString()) *
                                    Convert.ToDouble(item["InCartoon"].ToString())) : Convert.ToDouble(GoodTable.DefaultView[0]["Column40"]) * (Convert.ToDouble(item["GiftNumber"].ToString()) *
                                    Convert.ToDouble(item["InCartoon"].ToString()))) + ",NULL,NULL,NULL,0,100,"+(joz ? Convert.ToDouble(GoodTable.DefaultView[0]["Column34"])*(Convert.ToDouble(item["GiftNumber"].ToString()) *
                                    Convert.ToDouble(item["InCartoon"].ToString())) : Convert.ToDouble(GoodTable.DefaultView[0]["Column40"]) * (Convert.ToDouble(item["GiftNumber"].ToString()) *
                                    Convert.ToDouble(item["InCartoon"].ToString())))+",0,0,0,NULL,NULL,NULL,"
                            + Prefactor + ",0,0,0,0,0,1,"+
                             GoodTable.DefaultView[0]["Column09"].ToString() + ","+
                             GoodTable.DefaultView[0]["Column08"].ToString() + ",100,NULL,NULL," + GoodTable.DefaultView[0]["Column22"].ToString() + "," +
                         Convert.ToDouble(item["GiftNumber"].ToString()) * Convert.ToDouble(GoodTable.DefaultView[0]["Column22"].ToString()) + ")", ConSale);

                            ////Joz-Kol
                            //SqlCommand Insert = new SqlCommand(
                            //    "INSERT INTO Table_011_Child1_SaleFactor VALUES(" +
                            //    SaleId + "," + item["GiftGood"].ToString() + "," +
                            //    GoodTable.DefaultView[0]["Column07"].ToString() + ",0,0," +
                            //    float.Parse(item["GiftNumber"].ToString()) + "," +
                            //    float.Parse(item["GiftNumber"].ToString()) +
                            //    ",0,0,0,0,NULL,NULL,NULL,NULL,0,0,0,0,0,NULL,NULL,NULL,"
                            //+ Prefactor + ",0,0,0,0,0,1)", ConSale);

                            Insert.ExecuteNonQuery();
                        }
                    }
                //    include = true;
                //}
                //else return false;
            }
            return include;


        }

        public static bool SaleAward_Detial(int SaleId, string SaleDate, int Prefactor)
        {
            bool include = false;
            DataSet DataSet1;
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
            SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
            SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
            ConSale.Open();

            SqlDataAdapter GoodAdapter = new SqlDataAdapter("Select ColumnId,Column01,Column02,Column07,Column09,Column08,Column22,Column34 from table_004_CommodityAndIngredients", ConWare);
            DataTable GoodTable = new DataTable();
            GoodAdapter.Fill(GoodTable);

            SqlCommand Command = new SqlCommand(
                "Delete From Table_011_Child1_SaleFactor where Column30=1 and Column01=" +
                SaleId, ConSale);
            Command.ExecuteNonQuery();

            //2: Read Gifts for Factor's Good - Cartoon

            SqlDataAdapter GiftAdapter = new SqlDataAdapter(@"
            SELECT     dbo.Table_028_Award.column01 AS GoodID, dbo.Table_028_Award.column08 AS GiftGood, 
            SUM(CASE WHEN dbo.Table_028_Award.column06 = dbo.Table_028_Award.column07 THEN Floor(dbo.Table_011_Child1_SaleFactor.Column06 / dbo.Table_028_Award.column06) 
            * dbo.Table_028_Award.column11 ELSE CASE WHEN dbo.Table_028_Award.column06 <= dbo.Table_011_Child1_SaleFactor.Column06 AND 
            dbo.Table_028_Award.column07 >= dbo.Table_011_Child1_SaleFactor.Column06 THEN dbo.Table_028_Award.column11 ELSE 0 END END) AS GiftNumber, 
            {3}.dbo.table_004_CommodityAndIngredients.column09 AS InCartoon
            FROM         dbo.Table_028_Award INNER JOIN
            dbo.Table_011_Child1_SaleFactor ON dbo.Table_028_Award.column01 = dbo.Table_011_Child1_SaleFactor.column02  
            INNER JOIN
            dbo.Table_010_SaleFactor ON dbo.Table_011_Child1_SaleFactor.column01 = dbo.Table_010_SaleFactor.columnid INNER JOIN
            {3}.dbo.table_004_CommodityAndIngredients ON dbo.Table_028_Award.column08 = {3}.dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
            {2}.dbo.Table_045_PersonScope ON dbo.Table_010_SaleFactor.column03 = {2}.dbo.Table_045_PersonScope.Column01 AND 
            dbo.Table_028_Award.column17 = {2}.dbo.Table_045_PersonScope.Column02
            WHERE     (dbo.Table_028_Award.column02 <= '{1}') AND (dbo.Table_028_Award.column03 >= '{1}') AND (dbo.Table_011_Child1_SaleFactor.column01 = {0}) AND 
            (dbo.Table_011_Child1_SaleFactor.column30 = 0) 
            GROUP BY dbo.Table_028_Award.column01, dbo.Table_028_Award.column08, {3}.dbo.table_004_CommodityAndIngredients.column09", ConSale);


            GiftAdapter.SelectCommand.CommandText =
                string.Format(GiftAdapter.SelectCommand.CommandText, SaleId.ToString(),
                SaleDate, ConBase.Database, ConWare.Database);
            DataSet1 = new DataSet();
            GiftAdapter.Fill(DataSet1, "Gift");
            int GiftNumber = DataSet1.Tables["Gift"].Rows.Count;
            if (GiftNumber > 0)
            {
                foreach (DataRow item in DataSet1.Tables["Gift"].Rows)
                {
                    GoodTable.DefaultView.RowFilter = "ColumnId=" + item["GiftGood"].ToString();

                    if (float.Parse(item["GiftNumber"].ToString()) > 0)
                    {
                        //Cartoon
                        SqlCommand Insert = new SqlCommand(
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
           ,[Column37]) VALUES(" +
                            SaleId + "," + item["GiftGood"].ToString() + "," +
                            GoodTable.DefaultView[0]["Column07"].ToString() + ",0,0," +
                            float.Parse(item["GiftNumber"].ToString()) + "," +
                            float.Parse(item["GiftNumber"].ToString())  +
                            ",0,0," + GoodTable.DefaultView[0]["Column34"] + "," + Convert.ToDecimal(GoodTable.DefaultView[0]["Column34"]) * Convert.ToDecimal(item["GiftNumber"]) + 
                            @",NULL,NULL,NULL,0,100,"+ Convert.ToDecimal(GoodTable.DefaultView[0]["Column34"]) * Convert.ToDecimal(item["GiftNumber"])+
                            @",0,0,0,NULL,NULL,NULL,"
                        + Prefactor + ",0,0,0,0,0,1,"+
                         GoodTable.DefaultView[0]["Column09"].ToString()+","+
                         GoodTable.DefaultView[0]["Column08"].ToString() + ",100,NULL,NULL,"+GoodTable.DefaultView[0]["Column22"].ToString()+","+
                         Convert.ToDouble(item["GiftNumber"].ToString())*Convert.ToDouble(GoodTable.DefaultView[0]["Column22"].ToString()) +")", ConSale);
                        Insert.ExecuteNonQuery();
                    }
                }
            }
            return include;


        }

        public static DataTable OrderAwardByGoods_Box(int OrderId, string OrderDate)
        {
            DataSet DataSetAward;
            SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
            SqlConnection ConWhrs = new SqlConnection(Properties.Settings.Default.WHRS);

            //2: Read Gifts for Order's Good
            SqlDataAdapter GiftAdapter = new SqlDataAdapter(@"

            SELECT     dbo.Table_028_Award.column01 AS GoodID, dbo.Table_028_Award.column08 AS GiftGood, 
            SUM(CASE WHEN dbo.Table_028_Award.column06 = dbo.Table_028_Award.column07 THEN Floor(dbo.Table_006_OrderDetails.column04 / dbo.Table_028_Award.column06) 
            * dbo.Table_028_Award.column11 ELSE CASE WHEN dbo.Table_028_Award.column06 <= dbo.Table_006_OrderDetails.column04 AND 
            dbo.Table_028_Award.column07 >= dbo.Table_006_OrderDetails.column04 THEN dbo.Table_028_Award.column11 ELSE 0 END END) AS GiftNumber, 
            {3}.dbo.table_004_CommodityAndIngredients.column09 AS InCartoon, dbo.Table_006_OrderDetails.column04 AS QtyCartoon, dbo.Table_006_OrderDetails.column06 AS QtyTotal
            FROM         dbo.Table_028_Award INNER JOIN
            dbo.Table_006_OrderDetails ON dbo.Table_028_Award.column01 = dbo.Table_006_OrderDetails.column02  INNER JOIN
            dbo.Table_005_OrderHeader ON dbo.Table_006_OrderDetails.column01 = dbo.Table_005_OrderHeader.columnid INNER JOIN
            {3}.dbo.table_004_CommodityAndIngredients ON dbo.Table_028_Award.column08 = {3}.dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
            {2}.dbo.Table_045_PersonScope ON dbo.Table_005_OrderHeader.column03 = {2}.dbo.Table_045_PersonScope.Column01 AND 
            dbo.Table_028_Award.column17 = {2}.dbo.Table_045_PersonScope.Column02
            WHERE     (dbo.Table_028_Award.column02 <= '{1}') AND (dbo.Table_028_Award.column03 >= '{1}') AND (dbo.Table_006_OrderDetails.column01 = {0}) AND 
            (dbo.Table_006_OrderDetails.column31 = 0) AND (dbo.Table_006_OrderDetails.column07 = 1)
            GROUP BY dbo.Table_028_Award.column01, dbo.Table_028_Award.column08, {3}.dbo.table_004_CommodityAndIngredients.column09, dbo.Table_006_OrderDetails.column04, 
            dbo.Table_006_OrderDetails.column06", ConSale);

            GiftAdapter.SelectCommand.CommandText =
                string.Format(GiftAdapter.SelectCommand.CommandText, OrderId.ToString(),
                OrderDate, ConBase.Database, ConWhrs.Database);
            DataSetAward = new DataSet();
            GiftAdapter.Fill(DataSetAward, "TableGift");

            return DataSetAward.Tables["TableGift"];
        }

        public static DataTable OrderAwardByGoods_Detail(int OrderId, string OrderDate)
        {
            DataSet DataSetAward;
            SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
            SqlConnection ConWhrs = new SqlConnection(Properties.Settings.Default.WHRS);

            //2: Read Gifts for Order's Good
            SqlDataAdapter GiftAdapter = new SqlDataAdapter(@"

            SELECT     dbo.Table_028_Award.column01 AS GoodID, dbo.Table_028_Award.column08 AS GiftGood, 
            SUM(CASE WHEN dbo.Table_028_Award.column06 = dbo.Table_028_Award.column07 THEN Floor(dbo.Table_006_OrderDetails.Column05 / dbo.Table_028_Award.column06) 
            * dbo.Table_028_Award.column11 ELSE CASE WHEN dbo.Table_028_Award.column06 <= dbo.Table_006_OrderDetails.Column05 AND 
            dbo.Table_028_Award.column07 >= dbo.Table_006_OrderDetails.Column05 THEN dbo.Table_028_Award.column11 ELSE 0 END END) AS GiftNumber, 
            {3}.dbo.table_004_CommodityAndIngredients.column09 AS InCartoon, dbo.Table_006_OrderDetails.Column05 AS QtyCartoon, dbo.Table_006_OrderDetails.column06 AS QtyTotal
            FROM         dbo.Table_028_Award INNER JOIN
            dbo.Table_006_OrderDetails ON dbo.Table_028_Award.column01 = dbo.Table_006_OrderDetails.column02 AND 
            dbo.Table_028_Award.column06 < dbo.Table_006_OrderDetails.Column05 INNER JOIN
            dbo.Table_005_OrderHeader ON dbo.Table_006_OrderDetails.column01 = dbo.Table_005_OrderHeader.columnid INNER JOIN
            {3}.dbo.table_004_CommodityAndIngredients ON dbo.Table_028_Award.column08 = {3}.dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
            {2}.dbo.Table_045_PersonScope ON dbo.Table_005_OrderHeader.column03 = {2}.dbo.Table_045_PersonScope.Column01 AND 
            dbo.Table_028_Award.column17 = {2}.dbo.Table_045_PersonScope.Column02
            WHERE     (dbo.Table_028_Award.column02 <= '{1}') AND (dbo.Table_028_Award.column03 >= '{1}') AND (dbo.Table_006_OrderDetails.column01 = {0}) AND 
            (dbo.Table_006_OrderDetails.column31 = 0) AND (dbo.Table_006_OrderDetails.column07 = 1)
            GROUP BY dbo.Table_028_Award.column01, dbo.Table_028_Award.column08, {3}.dbo.table_004_CommodityAndIngredients.column09, dbo.Table_006_OrderDetails.Column05, 
            dbo.Table_006_OrderDetails.column06", ConSale);

            GiftAdapter.SelectCommand.CommandText =
                string.Format(GiftAdapter.SelectCommand.CommandText, OrderId.ToString(),
                OrderDate, ConBase.Database, ConWhrs.Database);
            DataSetAward = new DataSet();
            GiftAdapter.Fill(DataSetAward, "TableGift");

            return DataSetAward.Tables["TableGift"];
        }

        public static DataTable OrderAwardByRials(int OrderId, string OrderDate)
        {
            DataSet DataSetAward;
            SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
            SqlConnection ConWhrs = new SqlConnection(Properties.Settings.Default.WHRS);

            //2: Read Gifts for Order's Good
            SqlDataAdapter GiftAdapter = new SqlDataAdapter(@"
SELECT     dbo.Table_040_RialAwards.column06 AS GiftGood, 
{3}.dbo.table_004_CommodityAndIngredients.column09 AS InCartoon, 
                      SUM(CASE WHEN dbo.Table_040_RialAwards.column04 = 
                      dbo.Table_040_RialAwards.column05 THEN 
                      Floor(OrderInfo.TotalPrice / dbo.Table_040_RialAwards.column04) 
                      * dbo.Table_040_RialAwards.column07 WHEN 
                      OrderInfo.TotalPrice >= dbo.Table_040_RialAwards.column04 AND 
                      OrderInfo.TotalPrice <= dbo.Table_040_RialAwards.column05 THEN 
                      dbo.Table_040_RialAwards.column07 ELSE 0 END) AS GiftNumber
FROM         dbo.Table_040_RialAwards INNER JOIN
                      {2}.dbo.Table_045_PersonScope ON 
                      dbo.Table_040_RialAwards.column01 = 
                      {2}.dbo.Table_045_PersonScope.Column02 INNER JOIN
                      {3}.dbo.table_004_CommodityAndIngredients ON 
                      dbo.Table_040_RialAwards.column06 = 
                      {3}.dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
                          (SELECT     SUM(dbo.Table_006_OrderDetails.column13) AS 
                          TotalPrice, dbo.Table_005_OrderHeader.columnid AS 
                          OrderId, dbo.Table_005_OrderHeader.column03 AS CustomerId
                            FROM          dbo.Table_006_OrderDetails INNER JOIN
                                                   dbo.Table_005_OrderHeader ON 
                                                   dbo.Table_006_OrderDetails.column01 = 
                                                   dbo.Table_005_OrderHeader.columnid
                            GROUP BY dbo.Table_005_OrderHeader.columnid, 
                            dbo.Table_005_OrderHeader.column03
                            HAVING      (dbo.Table_005_OrderHeader.columnid = {0})) AS 
                            OrderInfo ON {2}.dbo.Table_045_PersonScope.Column01 = 
                            OrderInfo.CustomerId
WHERE     (dbo.Table_040_RialAwards.column02 <= N'{1}') AND 
(dbo.Table_040_RialAwards.column03 >= N'{1}') AND 
                      (CASE WHEN dbo.Table_040_RialAwards.column04 = 
                      dbo.Table_040_RialAwards.column05 THEN 
                      Floor(OrderInfo.TotalPrice / dbo.Table_040_RialAwards.column04) 
                      * dbo.Table_040_RialAwards.column07 WHEN 
                      OrderInfo.TotalPrice >= dbo.Table_040_RialAwards.column04 AND 
                      OrderInfo.TotalPrice <= dbo.Table_040_RialAwards.column05 THEN 
                      dbo.Table_040_RialAwards.column07 ELSE 0 END > 0)
GROUP BY dbo.Table_040_RialAwards.column06, 
{3}.dbo.table_004_CommodityAndIngredients.column09", ConSale);

            GiftAdapter.SelectCommand.CommandText =
                string.Format(GiftAdapter.SelectCommand.CommandText, OrderId.ToString(),
                OrderDate, ConBase.Database, ConWhrs.Database);
            DataSetAward = new DataSet();
            GiftAdapter.Fill(DataSetAward, "TableGift");

            return DataSetAward.Tables["TableGift"];
        }

        public static DataTable OrderAwardByTotalQty(int OrderId, string OrderDate)
        {
            DataSet DataSetAward;
            SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
            SqlConnection ConWhrs = new SqlConnection(Properties.Settings.Default.WHRS);

            //2: Read Gifts for Order's Good
            SqlDataAdapter GiftAdapter = new SqlDataAdapter(@"
SELECT     dbo.Table_045_TotalQtyAwards.column06 AS GiftGood, 
{3}.dbo.table_004_CommodityAndIngredients.column09 AS InCartoon, 
                      SUM(CASE WHEN Table_045_TotalQtyAwards.column04 = 
                      Table_045_TotalQtyAwards.column05 THEN 
                      Floor(OrderInfo.TotalQty / Table_045_TotalQtyAwards.column04) 
                      * Table_045_TotalQtyAwards.column07 WHEN 
                      OrderInfo.TotalQty >= Table_045_TotalQtyAwards.column04 AND 
                      OrderInfo.TotalQty <= Table_045_TotalQtyAwards.column05 
                      THEN Table_045_TotalQtyAwards.column07 ELSE 0 END) AS GiftNumber
FROM         dbo.Table_045_TotalQtyAwards INNER JOIN
                      {2}.dbo.Table_045_PersonScope ON 
                      dbo.Table_045_TotalQtyAwards.column01 = 
                      {2}.dbo.Table_045_PersonScope.Column02 INNER JOIN
                      {3}.dbo.table_004_CommodityAndIngredients ON 
                      dbo.Table_045_TotalQtyAwards.column06 = 
                      {3}.dbo.table_004_CommodityAndIngredients.columnid INNER JOIN
                          (SELECT     SUM(dbo.Table_006_OrderDetails.column04) AS 
                          TotalQty, dbo.Table_005_OrderHeader.columnid AS 
                          OrderId, dbo.Table_005_OrderHeader.column03 AS CustomerId
                            FROM          dbo.Table_006_OrderDetails INNER JOIN
                                                   dbo.Table_005_OrderHeader ON 
                                                   dbo.Table_006_OrderDetails.column01 = 
                                                   dbo.Table_005_OrderHeader.columnid
                            WHERE      (dbo.Table_006_OrderDetails.column31 = 0)
                            GROUP BY dbo.Table_005_OrderHeader.columnid, 
                            dbo.Table_005_OrderHeader.column03
                            HAVING      (dbo.Table_005_OrderHeader.columnid = {0})) AS 
                            OrderInfo ON {2}.dbo.Table_045_PersonScope.Column01 = 
                            OrderInfo.CustomerId
WHERE     (dbo.Table_045_TotalQtyAwards.column02 <= N'{1}') AND 
(dbo.Table_045_TotalQtyAwards.column03 >= N'{1}') AND 
                      (CASE WHEN Table_045_TotalQtyAwards.column04 = 
                      Table_045_TotalQtyAwards.column05 THEN 
                      Floor(OrderInfo.TotalQty / Table_045_TotalQtyAwards.column04) 
                      * Table_045_TotalQtyAwards.column07 WHEN 
                      OrderInfo.TotalQty >= Table_045_TotalQtyAwards.column04 AND 
                      OrderInfo.TotalQty <= Table_045_TotalQtyAwards.column05 THEN 
                      Table_045_TotalQtyAwards.column07 ELSE 0 END > 0)
GROUP BY dbo.Table_045_TotalQtyAwards.column06, 
{3}.dbo.table_004_CommodityAndIngredients.column09", ConSale);

            GiftAdapter.SelectCommand.CommandText =
                string.Format(GiftAdapter.SelectCommand.CommandText, OrderId.ToString(),
                OrderDate, ConBase.Database, ConWhrs.Database);
            DataSetAward = new DataSet();
            GiftAdapter.Fill(DataSetAward, "TableGift");

            return DataSetAward.Tables["TableGift"];
        }

        public static DataTable OrderAwardByVehcile(int OrderId, string OrderDate)
        {
            DataSet DataSetAward;
            SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
            SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
            SqlConnection ConWhrs = new SqlConnection(Properties.Settings.Default.WHRS);

            //2: Read Gifts for Order's Good
            SqlDataAdapter GiftAdapter = new SqlDataAdapter(@"
SELECT     {3}.dbo.table_004_CommodityAndIngredients.columnid AS 
GiftGood, {3}.dbo.table_004_CommodityAndIngredients.column09 AS InCartoon, 
                      SUM(dbo.Table_050_VehicleAwards.column06) AS GiftNumber
FROM         dbo.Table_050_VehicleAwards INNER JOIN
                      {2}.dbo.Table_045_PersonScope ON 
                      dbo.Table_050_VehicleAwards.column01 = 
                      {2}.dbo.Table_045_PersonScope.Column02 INNER JOIN
                      dbo.Table_005_OrderHeader ON 
                      {2}.dbo.Table_045_PersonScope.Column01 = 
                      dbo.Table_005_OrderHeader.column03 AND 
                      dbo.Table_050_VehicleAwards.column04 = 
                      dbo.Table_005_OrderHeader.column07 INNER JOIN
                      {3}.dbo.table_004_CommodityAndIngredients ON 
                      dbo.Table_050_VehicleAwards.column05 = 
                      {3}.dbo.table_004_CommodityAndIngredients.columnid
WHERE     (dbo.Table_050_VehicleAwards.column02 <= N'{1}') AND 
(dbo.Table_050_VehicleAwards.column03 >= N'{1}') AND 
(dbo.Table_005_OrderHeader.columnid = {0})
GROUP BY {3}.dbo.table_004_CommodityAndIngredients.columnid, 
{3}.dbo.table_004_CommodityAndIngredients.column09", ConSale);

            GiftAdapter.SelectCommand.CommandText =
                string.Format(GiftAdapter.SelectCommand.CommandText, OrderId.ToString(),
                OrderDate, ConBase.Database, ConWhrs.Database);
            DataSetAward = new DataSet();
            GiftAdapter.Fill(DataSetAward, "TableGift");

            return DataSetAward.Tables["TableGift"];
        }

    }
}
