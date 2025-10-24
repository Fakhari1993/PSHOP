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
    public partial class Frm_029_OLDCloseCash : Form
    {
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConMain = new SqlConnection(Properties.Settings.Default.MAIN);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConBank = new SqlConnection(Properties.Settings.Default.BANK);
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();
        Class_UserScope UserScope = new Class_UserScope();


        DataTable CheckTable = new DataTable();
        DataTable CheckHavaleTable = new DataTable();
        DataTable otherCheckHavaleTable = new DataTable();

        DataTable waredt = new DataTable();
        DataTable discountdt = new DataTable();
        DataTable taxdt = new DataTable();
        DataTable factordt = new DataTable();
        DataTable Sanaddt = new DataTable();
        DataTable iddt = new DataTable();
        DataTable bahaDT = new DataTable();

        Classes.Class_Documents clDoc = new Classes.Class_Documents();



        public Frm_029_OLDCloseCash()
        {
            InitializeComponent();
        }

        private void Frm_029_CloseCash_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            DataSet DS = new DataSet();
            SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT N'همه کاربران' AS Column00
                                                                  UNION ALL
                                                                SELECT column00
                                                                FROM   [dbo].[Table_010_UserInfo]
                                                                WHERE  Column03 = 1 and Column06='" + Class_BasicOperation._Year + "'", ConMain);
            Adapter.Fill(DS, "USer");
            cmb_User.ComboBox.DataSource = DS.Tables["USer"];
            cmb_User.ComboBox.DisplayMember = "Column00";
            cmb_User.ComboBox.ValueMember = "Column00";

            DataTable CustomerTable = clDoc.ReturnTable
            (ConBase.ConnectionString, @"select [ColumnId]
                                                              ,[Column00]
                                                              ,[Column01]
                                                              ,[Column02]
                                    FROM   dbo.Table_045_PersonInfo
                                            
                                    WHERE  (dbo.Table_045_PersonInfo.Column12 = 1)");
            gridEX_Header.DropDowns[3].SetDataBinding(CustomerTable, "");

            DataTable saletype = clDoc.ReturnTable(ConBase.ConnectionString, "select * from Table_002_SalesTypes");
            gridEX_Header.DropDowns["saletype"].SetDataBinding(saletype, "");


            DataTable sanad = clDoc.ReturnTable(ConAcnt.ConnectionString, "select * from Table_060_SanadHead");
            gridEX_Header.DropDowns["sanad"].SetDataBinding(sanad, "");



            DataTable draft = clDoc.ReturnTable(ConWare.ConnectionString, "select * from Table_007_PwhrsDraft");
            gridEX_Header.DropDowns["draft"].SetDataBinding(draft, "");


            faDatePickerStrip1.FADatePicker.SelectedDateTime = DateTime.Now;


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

        /*
        private float FirstRemain(int GoodCode, string ware, string date, int? drafid)
        {
            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
            {
                Con.Open();
                string CommandText = string.Empty;
                if (drafid == null)
                    CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE      (dbo.Table_011_PwhrsReceipt.column03 = {0}) AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE     (dbo.Table_007_PwhrsDraft.column03 = {0}) AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1})
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02) AS derivedtbl_1
                       WHERE     (Date <= '{2}')";
                else
                    CommandText = @"SELECT     ISNULL(SUM(InValue) - SUM(OutValue),0) AS Remain
                        FROM         (SELECT     dbo.Table_012_Child_PwhrsReceipt.column02 AS GoodCode, SUM(dbo.Table_012_Child_PwhrsReceipt.column07) AS InValue, 0 AS OutValue, 
                                              dbo.Table_011_PwhrsReceipt.column02 AS Date
                       FROM          dbo.Table_011_PwhrsReceipt INNER JOIN
                                              dbo.Table_012_Child_PwhrsReceipt ON dbo.Table_011_PwhrsReceipt.columnid = dbo.Table_012_Child_PwhrsReceipt.column01
                       WHERE      (dbo.Table_011_PwhrsReceipt.column03 = {0}) AND (dbo.Table_012_Child_PwhrsReceipt.column02 = {1})
                       GROUP BY dbo.Table_012_Child_PwhrsReceipt.column02, dbo.Table_011_PwhrsReceipt.column02
                       UNION ALL
                       SELECT     dbo.Table_008_Child_PwhrsDraft.column02 AS GoodCode, 0 AS InValue, SUM(dbo.Table_008_Child_PwhrsDraft.column07) AS OutValue, 
                                             dbo.Table_007_PwhrsDraft.column02 AS Date
                       FROM         dbo.Table_007_PwhrsDraft INNER JOIN
                                             dbo.Table_008_Child_PwhrsDraft ON dbo.Table_007_PwhrsDraft.columnid = dbo.Table_008_Child_PwhrsDraft.column01
                       WHERE     (dbo.Table_007_PwhrsDraft.column03 = {0}) AND (dbo.Table_008_Child_PwhrsDraft.column02 = {1}) and dbo.Table_007_PwhrsDraft.columnid!=" + drafid + @"
                       GROUP BY dbo.Table_008_Child_PwhrsDraft.column02, dbo.Table_007_PwhrsDraft.column02) AS derivedtbl_1
                       WHERE     (Date <= '{2}')";
                CommandText = string.Format(CommandText, ware, GoodCode, date);
                SqlCommand Command = new SqlCommand(CommandText, Con);
                return float.Parse(Command.ExecuteScalar().ToString());
            }

        }
       */


        private void bt_ExportDoc_Click(object sender, EventArgs e)
        {
            DataTable NotExist = new DataTable();

            try
            {
                if (gridEX_Header.GetRows().Length > 0)
                {



                    chehckessentioal();



                    if (faDatePickerStrip1.FADatePicker.Text != string.Empty)
                    {
                        NotExist.Columns.Add("factornum");
                        NotExist.Columns.Add("goodname");
                        NotExist.Columns.Add("goodcode");
                        NotExist.Columns.Add("goodbarcode");
                        NotExist.Columns.Add("currentStock", typeof(decimal));
                        NotExist.Columns.Add("needStock", typeof(decimal));
                        NotExist.Columns.Add("diff", typeof(decimal));
                        NotExist.Columns.Add("date");




                        ////


                        foreach (DataRow item1 in otherCheckHavaleTable.Rows)
                        {


                            DataTable kala = clDoc.ReturnTable(Properties.Settings.Default.WHRS,
                                         "Select top 1  [column01]  ,[column02],[column06] from table_004_CommodityAndIngredients where columnid=" + int.Parse(item1["column02"].ToString()) + " ");
                            if (kala.Rows.Count > 0)
                            {
                                if (!clGood.IsGoodInWare(Int16.Parse(item1["ware"].ToString()),
                           int.Parse(item1["column02"].ToString())))
                                    throw new Exception("کالای " + kala.Rows[0]["column02"] +
                                        " در انبار انتخاب شده فعال نمی باشد");

                                if (clDoc.IsGood(item1["column02"].ToString()))
                                {
                                    float Remain = FirstRemain(int.Parse(item1["column02"].ToString()), item1["ware"].ToString(), item1["date"].ToString(), Convert.ToInt32(item1["Column09"]));
                                    object sumObject = 0;
                                    sumObject = otherCheckHavaleTable.Compute("Sum(column07)", "column02 = " + item1["column02"] + " and date<='" + item1["date"] + "' and Column09=" + item1["Column09"]);


                                    string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                                        "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                        item1["column02"].ToString());


                                    if (item1["unit"].ToString() != orginalunit)
                                    {
                                        float h = clDoc.GetZarib(Convert.ToInt32(item1["Column02"]), Convert.ToInt16(item1["unit"]), Convert.ToInt16(orginalunit));
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
                                                                                   WHERE  ColumnId = " + item1["column02"] + @"
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
                                        {

                                            //                                            DataTable num = clDoc.ReturnTable(Properties.Settings.Default.SALE,
                                            //                                                                                               @"SELECT tsf.column01
                                            //                                                                    FROM   Table_011_Child1_SaleFactor h
                                            //                                                                           JOIN Table_010_SaleFactor tsf
                                            //                                                                                ON  tsf.columnid = h.column01
                                            //                                                                    WHERE  (
                                            //                                                                               tsf.column02 <= '" + item1["date"].ToString() + @"'
                                            //                                                                           )" + ((cmb_User.ComboBox.SelectedValue.ToString() == "همه کاربران") ? " " :
                                            //                                                                           "AND ( tsf. column13 ='" + cmb_User.ComboBox.SelectedValue.ToString() + @"')") + @"
                                            //                                                                           AND (tsf.column17 = 0)--باطل نيست
                                            //                                                                           AND (tsf.column19 = 0)--مرجوع نيست
                                            //                                                                           AND tsf.Column53 = 0-- بسته نشده
                                            //                                                                          and tsf.Column09!=0 and tsf.Column09=" + item1["Column09"].ToString() + @"
                                            //                                                                            AND h.column02 = " + int.Parse(item1["column02"].ToString()) + "    ");



                                            //                                            string f = string.Empty;
                                            //                                            foreach (DataRow dr in num.Rows)
                                            //                                            {
                                            //                                                f += dr["column01"] + ",";
                                            //                                            }
                                            //                                            if (!string.IsNullOrWhiteSpace(f))
                                            //                                                f = f.TrimEnd(',');


                                            DataRow r = NotExist.NewRow();
                                            //r["factornum"] = item1["column01"].ToString();
                                            // r["factornum"] = f;
                                            r["goodcode"] = kala.Rows[0]["column01"];
                                            r["goodname"] = kala.Rows[0]["column02"];
                                            r["goodbarcode"] = kala.Rows[0]["column06"];
                                            r["currentStock"] = Remain;
                                            r["needStock"] = sumObject;
                                            r["diff"] = Convert.ToDouble(sumObject) - Remain;

                                            r["date"] = item1["date"].ToString();
                                            NotExist.Rows.Add(r);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                DataTable S = clDoc.ReturnTable(Properties.Settings.Default.SALE,
                                                                                               @"SELECT tsf.column01
                                                                                        FROM   Table_010_SaleFactor tsf
                                                                                               JOIN Table_011_Child1_SaleFactor tcsf
                                                                                                    ON tcsf.column01 = tsf.columnid  
                                                                                        WHERE  tcsf.column02=" + item1["column02"]);
                                string ki = string.Empty;
                                foreach (DataRow k in S.Rows)
                                { ki += k["column01"].ToString() + ","; }



                                throw new Exception("کالا با آی دی  " + item1["column02"].ToString() + " در فاکتور های زیر وجود ندارد" + Environment.NewLine + ki.TrimEnd(','));

                            }
                        }


                        ////

                        /////
                        if (NotExist.Rows.Count > 0)
                        {
                            _05_Sale.GoodList2 frm1 = new GoodList2(NotExist, "لیست کالاهای ناموجود برای حواله های صادر شده");
                            frm1.ShowDialog();
                        }
                        if (NotExist.Rows.Count == 0)
                        {
                            NotExist = new DataTable();
                            NotExist.Columns.Add("factornum");
                            NotExist.Columns.Add("goodname");
                            NotExist.Columns.Add("goodcode");
                            NotExist.Columns.Add("goodbarcode");
                            NotExist.Columns.Add("currentStock", typeof(decimal));
                            NotExist.Columns.Add("needStock", typeof(decimal));
                            NotExist.Columns.Add("diff", typeof(decimal));
                            NotExist.Columns.Add("date");
                            foreach (DataRow item1 in this.CheckHavaleTable.Rows)
                            {


                                DataTable kala = clDoc.ReturnTable(Properties.Settings.Default.WHRS,
                                             "Select top 1  [column01]  ,[column02],[column06] from table_004_CommodityAndIngredients where columnid=" + int.Parse(item1["column02"].ToString()) + " ");
                                if (kala.Rows.Count > 0)
                                {
                                    if (!clGood.IsGoodInWare(Int16.Parse(item1["ware"].ToString()),
                               int.Parse(item1["column02"].ToString())))
                                        throw new Exception("کالای " + kala.Rows[0]["column02"] +
                                            " در انبار انتخاب شده فعال نمی باشد");

                                    if (clDoc.IsGood(item1["column02"].ToString()))
                                    {
                                        float Remain = FirstRemain(int.Parse(item1["column02"].ToString()), item1["ware"].ToString(), item1["date"].ToString(), null);
                                        object sumObject = 0;
                                        sumObject = CheckHavaleTable.Compute("Sum(column07)", "column02 = " + item1["column02"] + " and date<='" + item1["date"] + "'");

                                        string orginalunit = clDoc.ExScalar(ConWare.ConnectionString,
                                       "table_004_CommodityAndIngredients", "column07", "ColumnId",
                                       item1["column02"].ToString());


                                        if (item1["unit"].ToString() != orginalunit)
                                        {
                                            float h = clDoc.GetZarib(Convert.ToInt32(item1["column02"]), Convert.ToInt16(item1["unit"]), Convert.ToInt16(orginalunit));
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
                                                                                   WHERE  ColumnId = " + item1["column02"] + @"
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
                                            {

                                                DataTable num = clDoc.ReturnTable(Properties.Settings.Default.SALE,
                                                                                                   @"SELECT tsf.column01
                                                                    FROM   Table_011_Child1_SaleFactor h
                                                                           JOIN Table_010_SaleFactor tsf
                                                                                ON  tsf.columnid = h.column01
                                                                    WHERE  (
                                                                               tsf.column02 <= '" + item1["date"].ToString() + @"'
                                                                           )" + ((cmb_User.ComboBox.SelectedValue.ToString() == "همه کاربران") ? " " :
                                                                               "AND ( tsf. column13 ='" + cmb_User.ComboBox.SelectedValue.ToString() + @"')") + @"
                                                                           AND (tsf.column17 = 0)--باطل نيست
                                                                           AND (tsf.column19 = 0)--مرجوع نيست
                                                                           AND tsf.Column53 = 0-- بسته نشده
                                                                          and tsf.Column09=0
                                                                            AND h.column02 = " + int.Parse(item1["column02"].ToString()) + "    ");



                                                string f = string.Empty;
                                                foreach (DataRow dr in num.Rows)
                                                {
                                                    f += dr["column01"] + ",";
                                                }
                                                if (!string.IsNullOrWhiteSpace(f))
                                                    f = f.TrimEnd(',');


                                                DataRow r = NotExist.NewRow();
                                                //r["factornum"] = item["column01"].ToString();
                                                r["factornum"] = f;
                                                r["goodcode"] = kala.Rows[0]["column01"];
                                                r["goodname"] = kala.Rows[0]["column02"];
                                                r["goodbarcode"] = kala.Rows[0]["column06"];
                                                r["currentStock"] = Remain;
                                                r["needStock"] = sumObject;
                                                r["diff"] = Convert.ToDouble(sumObject) - Remain;

                                                r["date"] = item1["date"].ToString();
                                                NotExist.Rows.Add(r);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    DataTable S = clDoc.ReturnTable(Properties.Settings.Default.SALE,
                                                                                                   @"SELECT tsf.column01
                                                                                        FROM   Table_010_SaleFactor tsf
                                                                                               JOIN Table_011_Child1_SaleFactor tcsf
                                                                                                    ON tcsf.column01 = tsf.columnid  
                                                                                        WHERE  tcsf.column02=" + item1["column02"]);
                                    string ki = string.Empty;
                                    foreach (DataRow k in S.Rows)
                                    { ki += k["column01"].ToString() + ","; }



                                    throw new Exception("کالا با آی دی  " + item1["column02"].ToString() + " در فاکتور های زیر وجود ندارد" + Environment.NewLine + ki.TrimEnd(','));

                                }
                            }



                            if (NotExist.Rows.Count > 0)
                            {
                                _05_Sale.GoodList2 frm1 = new GoodList2(NotExist, "لیست کالاهای ناموجود برای حواله هایی که قرار است صادر شوند");
                                frm1.ShowDialog();
                            }


                            else
                            {










                                //////////////
                                bt_ExportDoc.Enabled = false;
                                string sanadcmd = string.Empty;
                                sanadcmd = "   declare @draftkey int  declare @DraftNum int declare @DocNum int    declare @DocID int";
                                foreach (DataRow idro in iddt.Rows)
                                {


                                    Sanaddt = new DataTable();

                                    SqlDataAdapter Adapter = new SqlDataAdapter(@"SELECT FactorTable.columnid,
                                               FactorTable.column01,
                                               FactorTable.date,
                                               ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
                                               ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
                                               FactorTable.Bed,
                                               FactorTable.Bes,
                                               FactorTable.NetTotal,FactorTable.person,FactorTable.saletype,FactorTable.Func,FactorTable.ware
                                        FROM   (
                                                   SELECT dbo.Table_010_SaleFactor.columnid,
                                                          dbo.Table_010_SaleFactor.column01,
                                                          dbo.Table_010_SaleFactor.column02 AS Date,
                                                          dbo.Table_010_SaleFactor.column03 AS person,
                                                          dbo.Table_010_SaleFactor.column36 AS saletype,
                                                          dbo.Table_010_SaleFactor.column42 AS ware,
                                                          dbo.Table_010_SaleFactor.column43 AS Func,
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
                                        WHERE  FactorTable.columnid=" + int.Parse(idro["columnid"].ToString()) + @"
                                                                                                           ", ConSale);
                                    Adapter.Fill(Sanaddt);

                                    if (int.Parse(idro["Column09"].ToString()) == 0)
                                    {
                                        sanadcmd += " set @DraftNum=( SELECT ISNULL(Max(Column01),0)+1 as ID from " + ConWare.Database + ".dbo.Table_007_PwhrsDraft)";
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
                                                                                               ,[Column26]) VALUES(@DraftNum,'" + Sanaddt.Rows[0]["date"] + "'," + Sanaddt.Rows[0]["ware"]
                                                                                                                 + "," + (Sanaddt.Rows[0]["Func"] != null && !string.IsNullOrWhiteSpace(Sanaddt.Rows[0]["Func"].ToString()) ? Sanaddt.Rows[0]["Func"] : waredt.Rows[0]["Column02"]) + @", " + Sanaddt.Rows[0]["person"] + ",'" + "حواله صادره بابت فاکتور فروش ش" + Sanaddt.Rows[0]["column01"] +
                                                                                                             "',0,'" + Class_BasicOperation._UserName + "',getdate(),'" + Class_BasicOperation._UserName + "',getdate(),0,NULL,NULL,0," + Sanaddt.Rows[0]["columnid"] + ",0,0,0,0,0,0,0,null,0,0); SET @draftkey=SCOPE_IDENTITY()";

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
                                                                  FROM  [dbo].[Table_011_Child1_SaleFactor] WHERE column01 =" + Sanaddt.Rows[0]["columnid"], ConSale);
                                        DataTable Child1 = new DataTable();
                                        Adapter.Fill(Child1);

                                        foreach (DataRow item1 in Child1.Rows)
                                        {
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
           ,[Column35]) VALUES(@draftkey," + item1["Column02"].ToString() + "," +
                                                   orginalunit + "," + item1["Column04"].ToString() + "," + item1["Column05"].ToString() + "," + value + "," +
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
                                            sanadcmd += "Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column09=@draftkey where ColumnId=" + Sanaddt.Rows[0]["columnid"];
                                        }
                                    }
                                    else
                                        sanadcmd += "SET @draftkey=" + idro["Column109"];
                                    if (int.Parse(idro["Column10"].ToString()) == 0)
                                    {
                                        int LastDocnum = LastDocNum(Sanaddt.Rows[0]["date"].ToString());
                                        if (LastDocnum > 0)
                                            sanadcmd += " set @DocNum=" + LastDocnum + "  SET @DocID=(Select ColumnId from Table_060_SanadHead where Column00=" + LastDocnum + ")";
                                        else
                                            sanadcmd += @" set @DocNum=(SELECT ISNULL((SELECT MAX(Column00)  FROM   Table_060_SanadHead ), 0 )) + 1   INSERT INTO Table_060_SanadHead (Column00,Column01,Column02,Column03,Column04,Column05,Column06)
                VALUES((Select Isnull((Select Max(Column00) from Table_060_SanadHead),0))+1,'" + Sanaddt.Rows[0]["date"] + "',2,0,N'فاکتور فروش','" + Class_BasicOperation._UserName +
                                       "',getdate()); SET @DocID=SCOPE_IDENTITY()";

                                        factordt = new DataTable();
                                        Adapter = new SqlDataAdapter(
                                                                  @"SELECT       isnull( column08,'') as Column07,isnull(column14,'') as Column13
                                                                        FROM            Table_002_SalesTypes
                                                                        WHERE        (columnid = " + Sanaddt.Rows[0]["saletype"] + ") ", ConBase);
                                        Adapter.Fill(factordt);

                                        string[] _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column07"].ToString());

                                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                " + Sanaddt.Rows[0]["person"] + @", NULL , NULL ,
                   " + "'فاکتور فروش " + Sanaddt.Rows[0]["column01"] + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                          Class_BasicOperation._UserName + "',getdate(),0,0,NULL); ";


                                        _AccInfo = clDoc.ACC_Info(this.factordt.Rows[0]["Column13"].ToString());

                                        sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + factordt.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'فاکتور فروش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                                NULL, NULL , NULL ,
                   " + "'تخفیف فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                                                sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                               " + int.Parse(Sanaddt.Rows[0]["person"].ToString()) + @", NULL , NULL ,
                   " + "'تخفیف فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Kosoorat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
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
                                " + int.Parse(Sanaddt.Rows[0]["person"].ToString()) + @", NULL , NULL ,
                   " + "'ارزش افزوده فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "'," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                _AccInfo = clDoc.ACC_Info(dr["Bes"].ToString());

                                                sanadcmd += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(@DocID,'" + dr["Bes"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'ارزش افزوده فاکتور فروش ش " + int.Parse(Sanaddt.Rows[0]["column01"].ToString()) + "',0," + Convert.ToInt64(Math.Round(Convert.ToDouble(dr["Ezafat"].ToString()), 3)) + @",0,0,-1,15," + int.Parse(Sanaddt.Rows[0]["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                  Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";



                                            }


                                        }
                                        sanadcmd += " Update " + ConWare.Database + ".dbo.Table_007_PwhrsDraft set Column07=@DocID,column10='" + Class_BasicOperation._UserName + "',column11=getdate() where ColumnId=@draftkey";
                                        sanadcmd += " Update " + ConSale.Database + ".dbo.Table_010_SaleFactor set Column10=@DocID where ColumnId =" + Sanaddt.Rows[0]["columnid"].ToString();
                                    }

                                }



                                using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                                {
                                    Con.Open();

                                    SqlTransaction sqlTran = Con.BeginTransaction();
                                    SqlCommand Command = Con.CreateCommand();
                                    Command.Transaction = sqlTran;

                                    try
                                    {
                                        Command.CommandText = sanadcmd;
                                        Command.ExecuteNonQuery();
                                        sqlTran.Commit();
                                        //محاسبه ارزش کالا های حواله 

                                        SqlDataAdapter Adapter = new SqlDataAdapter(
                                                                    @"SELECT  
                                                                           tsf.columnid,tsf.column01, tsf.Column09, tsf.Column10 as DocID,dff.column02 as date,dff.column03 as ware
                                                                    FROM   Table_010_SaleFactor tsf join " + ConWare.Database + @".dbo.Table_007_PwhrsDraft dff on dff.columnid=tsf.Column09
                                                                    WHERE  (
                                                                               tsf.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                           )
                                                                          " + ((cmb_User.ComboBox.SelectedValue.ToString() == "همه کاربران") ? " " :
                                                                               "AND ( tsf. column13 ='" + cmb_User.ComboBox.SelectedValue.ToString() + @"')") + @"
                                                                           AND (tsf.column17 = 0)--باطل نيست
                                                                           AND (tsf.column19 = 0)--مرجوع نيست
                                                                           AND tsf.Column53 = 0-- بسته نشده
                                                                           order by   tsf.column02,tsf.column14,tsf.columnid
                                                                           
                                                                             ", ConSale);
                                        DataTable ArzeshTable = new DataTable();
                                        Adapter.Fill(ArzeshTable);
                                        string upcmd = string.Empty;
                                        string notset = string.Empty;
                                        using (SqlConnection Con1 = new SqlConnection(Properties.Settings.Default.WHRS))
                                        {
                                            Con1.Open();
                                            foreach (DataRow ar in ArzeshTable.Rows)
                                            {

                                                try
                                                {
                                                    string bahas = string.Empty;

                                                    SqlDataAdapter goodAdapter = new SqlDataAdapter("Select * from Table_008_Child_PwhrsDraft where Column01=" + ar["Column09"], ConWare);
                                                    DataTable Table = new DataTable();
                                                    goodAdapter.Fill(Table);

                                                    //محاسبه ارزش و ذخیره آن در جدول Child1
                                                    double value = 0;
                                                    foreach (DataRow item2 in Table.Rows)
                                                    {
                                                        if (Class_BasicOperation._WareType)
                                                        {
                                                            Adapter = new SqlDataAdapter("EXEC	 " + (Class_BasicOperation._WareType ? "[dbo].[PR_00_NewFIFO] " : " [dbo].[PR_05_AVG] ") + "  @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + ar["ware"].ToString(), Con);
                                                            DataTable TurnTable = new DataTable();
                                                            Adapter.Fill(TurnTable);
                                                            DataRow[] Row = TurnTable.Select("Kind=2 and ID=" + ar["Column09"] + " and DetailID=" + item2["Columnid"].ToString());
                                                            SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(Row[0]["DsinglePrice"].ToString()), 4)
                                                                + " , Column16=" + Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con1);
                                                            UpdateCommand.ExecuteNonQuery();
                                                            value += Math.Round(double.Parse(Row[0]["DTotalPrice"].ToString()), 4);
                                                        }

                                                        else
                                                        {
                                                            Adapter = new SqlDataAdapter("EXEC	   [dbo].[PR_05_NewAVG]   @GoodParameter = " + item2["Column02"].ToString() + ", @WareCode = " + ar["ware"].ToString() + ",@Date='" + ar["date"].ToString() + "',@id=" + ar["Column09"] + ",@residid=0", ConWare);
                                                            DataTable TurnTable = new DataTable();
                                                            Adapter.Fill(TurnTable);
                                                            SqlCommand UpdateCommand = new SqlCommand("UPDATE Table_008_Child_PwhrsDraft SET  Column15=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4)
                                                          + " , Column16=" + Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4) + " where ColumnId=" + item2["ColumnId"].ToString(), Con1);
                                                            UpdateCommand.ExecuteNonQuery();
                                                            value += Math.Round(double.Parse(TurnTable.Rows[0]["Avrage"].ToString()), 4) * Math.Round(double.Parse(item2["Column07"].ToString()), 4);
                                                        }
                                                    }
                                                    if (Class_BasicOperation._FinType)//بهای تمام شده
                                                    {
                                                        if (value > 0 && Convert.ToInt32(ar["DocID"]) > 0)
                                                        {
                                                            string[] _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column07"].ToString());

                                                            bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + ar["DocID"] + @",'" + bahaDT.Rows[0]["Column07"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NUll, NULL , NULL ,
                   " + "'بهای تمام شده فاکتور فروش ش " + ar["column01"] + "'," + Convert.ToInt64(value) + @",0,0,0,-1,26," + int.Parse(ar["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";


                                                            _AccInfo = clDoc.ACC_Info(bahaDT.Rows[0]["Column13"].ToString());

                                                            bahas += @"    INSERT INTO Table_065_SanadDetail ([Column00]      ,[Column01]      ,[Column02]      ,[Column03]      ,[Column04]      ,[Column05]
              ,[Column06]      ,[Column07]      ,[Column08]      ,[Column09]      ,[Column10]      ,[Column11]    ,[Column12]      ,[Column13]      ,[Column14]      ,[Column15]      ,[Column16]      ,[Column17] ,[Column18]      ,[Column19]      ,[Column20]      ,[Column21]      ,[Column22],[Column23],[Column24]) 
                    VALUES(" + ar["DocID"] + @",'" + bahaDT.Rows[0]["Column13"].ToString() + @"',
                                " + Int16.Parse(_AccInfo[0].ToString()) + ",'" + _AccInfo[1].ToString() + @"',
                                " + (string.IsNullOrEmpty(_AccInfo[2].ToString()) ? "NULL" : "'" + _AccInfo[2].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[3].ToString()) ? "NULL" : "'" + _AccInfo[3].ToString() + "'") + @",
                                " + (string.IsNullOrEmpty(_AccInfo[4].ToString()) ? "NULL" : "'" + _AccInfo[4].ToString() + "'") + @",
                                NULL, NULL , NULL ,
                   " + "'بهای تمام شده فاکتور فروش ش " + ar["column01"] + "',0," + Convert.ToInt64(value) + @",0,0,-1,26," + int.Parse(ar["columnid"].ToString()) + ",'" + Class_BasicOperation._UserName + "',getdate(),'" +
                                                              Class_BasicOperation._UserName + "',getdate(),0,0,NULL);  ";

                                                            clDoc.RunSqlCommand(Properties.Settings.Default.ACNT, bahas);



                                                        }
                                                    }
                                                    upcmd += ar["columnid"].ToString() + ",";

                                                }
                                                catch
                                                { notset += ar["column01"].ToString() + ","; }

                                            }
                                        }
                                        if (!string.IsNullOrWhiteSpace(upcmd))
                                            clDoc.RunSqlCommand(ConSale.ConnectionString, " update Table_010_SaleFactor set column53=1,Column62='" + Class_BasicOperation._UserName + "',Column63=getdate() where columnid in  (" + upcmd.TrimEnd(',') + ")");


                                        if (!string.IsNullOrWhiteSpace(notset) && !Class_BasicOperation._FinType)
                                        {
                                            MessageBox.Show("ارزش حواله فاکتور/های زیر با خطا مواجه شد و بستن صندوق برای این فاکتور/ها انجام نشد" + Environment.NewLine + notset.TrimEnd(','));
                                        }
                                        else if (!string.IsNullOrWhiteSpace(notset) && Class_BasicOperation._FinType)
                                            MessageBox.Show("ارزش حواله و صدور سند بهای تمام شده ی فاکتور/های زیر با خطا مواجه شد و بستن صندوق برای این فاکتور/ها انجام نشد" + Environment.NewLine + notset.TrimEnd(','));
                                        else
                                            MessageBox.Show("عملیات با موفقیت انجام شد");


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

                            bt_Search_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
            bt_ExportDoc.Enabled = true;

        }

        private void bt_Search_Click(object sender, EventArgs e)
        {
            DataTable allfactor = new DataTable();
            try
            {
                if (faDatePickerStrip1.FADatePicker.Text != string.Empty && cmb_User.ComboBox.SelectedValue != null)
                {



                    SqlDataAdapter Adapter = new SqlDataAdapter(
                                                                @"SELECT h.column02 AS column02,h.column03 as unit,
                                                                            tsf.column02 as [date],
                                                                           SUM(h.column07) AS column07 ,tsf.Column42 as ware
                                                                    FROM   Table_011_Child1_SaleFactor h
                                                                           JOIN Table_010_SaleFactor tsf
                                                                                ON  tsf.columnid = h.column01
                                                                    WHERE  (
                                                                               tsf.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                           )
                                                                          " + ((cmb_User.ComboBox.SelectedValue.ToString() == "همه کاربران") ? " " :
                                                                           "AND ( tsf. column13 ='" + cmb_User.ComboBox.SelectedValue.ToString() + @"')") + @"
                                                                           AND (tsf.column17 = 0)--باطل نيست
                                                                           AND (tsf.column19 = 0)--مرجوع نيست
                                                                           AND tsf.Column53 = 0-- بسته نشده
                                                                           AND tsf.Column09=0
                                                                            GROUP BY
                                                                           h.column02,h.column03,
                                                                            tsf.column02,tsf.Column42
                                                                        ORDER BY tsf.column02 

                                                                             ", ConSale);
                    CheckHavaleTable = new DataTable();
                    Adapter.Fill(CheckHavaleTable);

                    otherCheckHavaleTable = new DataTable();
                    Adapter = new SqlDataAdapter(
                                                                @"SELECT h.column02 AS column02,h.column03 as unit,
                                                                            tsf.column02 as [date],
                                                                           SUM(h.column07) AS column07, tsf.Column09,tsf.Column42 as ware
                                                                    FROM   Table_011_Child1_SaleFactor h
                                                                           JOIN Table_010_SaleFactor tsf
                                                                                ON  tsf.columnid = h.column01
                                                                    WHERE  (
                                                                               tsf.column02 <= '" + faDatePickerStrip1.FADatePicker.Text + @"'
                                                                           )
                                                                          " + ((cmb_User.ComboBox.SelectedValue.ToString() == "همه کاربران") ? " " :
                                                                           "AND ( tsf. column13 ='" + cmb_User.ComboBox.SelectedValue.ToString() + @"')") + @"
                                                                           AND (tsf.column17 = 0)--باطل نيست
                                                                           AND (tsf.column19 = 0)--مرجوع نيست
                                                                           AND tsf.Column53 = 0-- بسته نشده
                                                                           AND tsf.Column09!=0
                                                                            GROUP BY
                                                                           h.column02,h.column03,
                                                                            tsf.column02,tsf.Column09,tsf.Column42
                                                                        ORDER BY tsf.column02 
                                                                             ", ConSale);
                    Adapter.Fill(otherCheckHavaleTable);

                    Adapter = new SqlDataAdapter(
                                                            @"SELECT        columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, column14, 
                         column15, column16, column17, column18, column19, column20, column21, column23, column24, column25, column27, Column37, Column38, Column39, Column40, 
                         Column42, Column43, Column44, Column45, Column49, Column50, Column51, Column53, Column54, Column55, Column56, Column57, Column58, Column59, 
                         Column61, Column62, Column63, Column60, Column64, column22, column26, Column28, Column29, Column30, Column31, Column32, Column33, Column34, 
                         Column35, Column36, Column41, Column46, Column47, Column48, Column52,Column28 - Column29 -Column30 - Column31 + Column32- Column33 as FinalPrice
FROM            Table_010_SaleFactor
WHERE      column02<='" + faDatePickerStrip1.FADatePicker.Text + @"'   " + ((cmb_User.ComboBox.SelectedValue.ToString() == "همه کاربران") ? " " :
                                                                         "AND (  column13 ='" + cmb_User.ComboBox.SelectedValue.ToString() + @"')") + @"    and column53=0 and column19=0 and column17=0
                                                                             ", ConSale);
                    Adapter.Fill(allfactor);
                    gridEX_Header.DataSource = allfactor;

                }
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void Frm_029_CloseCash_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX_Header.RemoveFilters();

        }

        private void chehckessentioal()
        {

            discountdt = new DataTable();
            taxdt = new DataTable();
            factordt = new DataTable();
            waredt = new DataTable();
            Sanaddt = new DataTable();
            iddt = new DataTable();
            bahaDT = new DataTable();

            SqlDataAdapter Adapter = new SqlDataAdapter(
                                                            @"SELECT        isnull(Column02,0) as Column02
                                                                        FROM           Table_030_Setting
                                                                        WHERE        (ColumnId in (45,46)) order by ColumnId  ", ConBase);
            Adapter.Fill(waredt);
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
                        throw new Exception("عملکرد انتخاب نشده است");
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
                //                        throw new Exception("انبار انتخاب نشده است");
                //                }

            }
            else
                throw new Exception("  عملرکد تعریف نشده است");




            Adapter = new SqlDataAdapter(@"SELECT DISTINCT tsf.columnid,tsf.Column09,Column10
                                            FROM   Table_010_SaleFactor tsf
                                            WHERE  (tsf.column09 = 0 OR tsf.column10 = 0)
                                                   AND tsf.Column53 = 0
                                                   " + ((cmb_User.ComboBox.SelectedValue.ToString() == "همه کاربران") ? " " :
                                                                          "AND ( tsf. column13 ='" + cmb_User.ComboBox.SelectedValue.ToString() + @"')") + @"
                                                   AND tsf.column02 <= N'" + faDatePickerStrip1.FADatePicker.Text + "' ", ConSale);
            Adapter.Fill(iddt);
            if (iddt.Rows.Count > 0)
            {

                if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 69))
                    throw new Exception("کاربر گرامی شما امکان صدور سند حسابداری را ندارید");


                Adapter = new SqlDataAdapter(
                                                 @"SELECT       isnull( column10,'') as column10 , isnull(column16,'') as column16
                                                                    FROM            Table_024_Discount
                                                                     group by column10,column16
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



                if (Class_BasicOperation._FinType)///سیستم دائمی
                {
                    Adapter = new SqlDataAdapter(
                                                                           @"SELECT        Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08, Column09, Column10, Column11, Column12, Column13, 
                                                                                                    Column14, Column15, Column16
                                                                        FROM            Table_105_SystemTransactionInfo
                                                                        WHERE        (Column00 = 8) ", ConBase);
                    Adapter.Fill(bahaDT);


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

                }

                Adapter = new SqlDataAdapter(
                                                                  @"SELECT       isnull( column08,'') as Column07,isnull(column14,'') as Column13
                                                                        FROM            Table_002_SalesTypes
                                                                          ", ConBase);
                Adapter.Fill(factordt);

                foreach (DataRow dr1 in factordt.Rows)
                {
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.ACNT))
                    {
                        Con.Open();
                        SqlCommand Comm = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   AllHeaders()
                                                                   WHERE  ACC_Code = '" + dr1["Column13"].ToString() + @"'
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
                                                                   WHERE  ACC_Code = '" + dr1["Column07"].ToString() + @"'
                                                               )
                                                                SELECT 1 AS ok
                                                            ELSE
                                                                SELECT 0 AS ok", Con);
                        if (int.Parse(Comm.ExecuteScalar().ToString()) == 0)
                            throw new Exception("شماره حساب معتبر را در تعریف انواع فروش وارد کنید");
                    }
                }

                DataTable TPerson = new DataTable();
                TPerson.Columns.Add("Person", Type.GetType("System.Int32"));
                TPerson.Columns.Add("Account", Type.GetType("System.String"));
                TPerson.Columns.Add("Price", Type.GetType("System.Double"));
                DataTable TAccounts = new DataTable();
                TAccounts.Columns.Add("Account", Type.GetType("System.String"));
                TAccounts.Columns.Add("Price", Type.GetType("System.Double"));



                /*
                foreach (DataRow idro in iddt.Rows)
                {

                    Sanaddt = new DataTable();
                    factordt = new DataTable();
                    Adapter = new SqlDataAdapter(@"SELECT FactorTable.columnid,
                                               FactorTable.column01,
                                               FactorTable.date,
                                               ISNULL(FactorTable.Ezafat, 0) AS Ezafat,
                                               ISNULL(FactorTable.Kosoorat, 0) AS Kosoorat,
                                               FactorTable.Bed,
                                               FactorTable.Bes,
                                               FactorTable.NetTotal,FactorTable.person,FactorTable.saletype,FactorTable.ware,FactorTable.func
                                        FROM   (
                                                   SELECT dbo.Table_010_SaleFactor.columnid,
                                                          dbo.Table_010_SaleFactor.column01,
                                                          dbo.Table_010_SaleFactor.column02 AS Date,
                                                          dbo.Table_010_SaleFactor.column03 AS person,
                                                          dbo.Table_010_SaleFactor.column36 AS saletype,
                                                          dbo.Table_010_SaleFactor.column42 AS ware,
                                                          dbo.Table_010_SaleFactor.column43 AS func,



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
                                        WHERE  FactorTable.columnid=" + int.Parse(idro["columnid"].ToString()) + @"
                                                                                                           ", ConSale);
                    Adapter.Fill(Sanaddt);


                    Adapter = new SqlDataAdapter(
                                                                    @"SELECT       isnull( column08,'') as Column07,isnull(column14,'') as Column13
                                                                        FROM            Table_002_SalesTypes
                                                                        WHERE        (columnid = " + Sanaddt.Rows[0]["saletype"] + ") ", ConBase);
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

                    //تاریخ قبل از آخرین تاریخ قطعی سازی نباشد
                    clDoc.CheckForValidationDate(Sanaddt.Rows[0]["date"].ToString());


                    int LastDocnum = LastDocNum(Sanaddt.Rows[0]["date"].ToString());
                    if (LastDocnum > 0)
                        clDoc.IsFinal(LastDocnum);
                    if (
                   Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()) <= Convert.ToDouble(0)
                   )
                        throw new Exception("امکان صدور سند حسابداری با مبلغ صفر وجود ندارد");

                    if (Class_BasicOperation._FinType)
                    {
                        All_Controls_Row1(bahaDT.Rows[0]["Column13"].ToString(), null, null, null);
                        All_Controls_Row1(bahaDT.Rows[0]["Column07"].ToString(), null, null, null);

                    }


                    All_Controls_Row1(factordt.Rows[0]["Column07"].ToString(), int.Parse(Sanaddt.Rows[0]["person"].ToString()), null, null);
                    All_Controls_Row1(factordt.Rows[0]["Column13"].ToString(), null, null, null);
                    TPerson.Rows.Add(Int32.Parse(Sanaddt.Rows[0]["person"].ToString()), factordt.Rows[0]["Column07"].ToString(), Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"].ToString()));
                    TAccounts.Rows.Add(factordt.Rows[0]["Column13"].ToString(), (-1 * Convert.ToDouble((Sanaddt.Rows[0]["NetTotal"]))));
                    TAccounts.Rows.Add(factordt.Rows[0]["Column07"].ToString(), (Convert.ToDouble(Sanaddt.Rows[0]["NetTotal"])));

                    foreach (DataRow dr in Sanaddt.Rows)
                    {
                        if (dr["ware"] == DBNull.Value || dr["ware"] == null || string.IsNullOrWhiteSpace(dr["ware"].ToString()))
                            throw new Exception("انبار در فاکتور فروش ش " + dr["column01"] + "تعریف نشده است");


                        using (SqlConnection Conacnt = new SqlConnection(Properties.Settings.Default.ACNT))
                        {
                            Conacnt.Open();
                            SqlCommand Commnad = new SqlCommand(@"IF EXISTS (
                                                                   SELECT *
                                                                   FROM   Table_200_UserAccessInfo tuai
                                                                   WHERE  tuai.Column03 = 5
                                                                          AND tuai.Column01 = N'" + Class_BasicOperation._UserName + @"'
                                                                          AND tuai.Column02 = " + dr["ware"] + @"
                                                               )
                                                                SELECT 0 AS ok
                                                            ELSE
                                                                SELECT 1 AS ok", Conacnt);
                            if (int.Parse(Commnad.ExecuteScalar().ToString()) == 0)
                                throw new Exception("برای صدور حواله فاکتور فروش ش " + dr["column01"] + " به انبار انتخاب شده دسترسی ندارید");

                        }

                        if (Convert.ToDouble(dr["Ezafat"]) > 0)
                        {
                            All_Controls_Row1(dr["Bed"].ToString(), int.Parse(dr["person"].ToString()), null, null);
                            All_Controls_Row1(dr["Bes"].ToString(), null, null, null);
                            TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Ezafat"])));
                            TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Ezafat"])));
                            TPerson.Rows.Add(Int32.Parse(dr["person"].ToString()), dr["Bed"].ToString(), Convert.ToDouble(dr["Ezafat"]));


                        }
                        if (Convert.ToDouble(dr["Kosoorat"]) > 0)
                        {
                            All_Controls_Row1(dr["Bes"].ToString(), int.Parse(dr["person"].ToString()), null, null);
                            All_Controls_Row1(dr["Bed"].ToString(), null, null, null);
                            TAccounts.Rows.Add(dr["Bes"].ToString(), (-1 * Convert.ToDouble(dr["Kosoorat"])));
                            TAccounts.Rows.Add(dr["Bed"].ToString(), (Convert.ToDouble(dr["Kosoorat"])));
                            TPerson.Rows.Add(Int32.Parse(dr["person"].ToString()), dr["Bes"].ToString(), Convert.ToDouble(dr["Kosoorat"]));


                        }



                    }




                }*/
                Classes.CheckCredits clCredit = new Classes.CheckCredits();

                clCredit.CheckAccountCredit(TAccounts, 0);
                clCredit.CheckPersonCredit(TPerson, 0);
                //سند اختتامیه صادر نشده باشد
                clDoc.CheckExistFinalDoc();

            }



        }
        public void All_Controls_Row1(string AccountCode, int? Person, Int16? Center, Int16? Project)
        {

            //*********Control Person
            if (AccHasPerson(AccountCode) == 1)
            {
                if (Person == null)
                {

                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasPerson(AccountCode) == 0)
            {
                if (Person != null)
                {

                    throw new Exception("انتخاب شخص برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************ Control Center
            if (AccHasCenter(AccountCode) == 1)
            {
                if (Center == null)
                {

                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasCenter(AccountCode) == 0)
            {
                if (Center != null)
                {

                    throw new Exception("انتخاب مرکز هزینه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
                }
            }
            //************* Control Project
            if (AccHasProject(AccountCode) == 1)
            {
                if (Project == null)
                {

                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " الزامیست");
                }
            }
            else if (AccHasProject(AccountCode) == 0)
            {
                if (Project != null)
                {

                    throw new Exception("انتخاب پروژه برای حساب " + AccountName(AccountCode) + " لازم نمی باشد");
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
        public int LastDocNum(string date)
        {
            using (SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT))
            {
                ConAcnt.Open();
                SqlCommand Select = new SqlCommand("Select Isnull((Select Max(isnull( Column00,0)) from Table_060_SanadHead where Column01='" + date + "'),0)", ConAcnt);
                int Result = int.Parse(Select.ExecuteScalar().ToString());
                return Result;
            }
        }

        private void Frm_029_CloseCash_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_ExportDoc_Click(sender, e);
            else if (e.KeyCode == Keys.F)
            {
                bt_Search_Click(sender, e);

            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                _05_Sale.Form1 frm1 = new Form1();
                frm1.ShowDialog();
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }
    }
}
