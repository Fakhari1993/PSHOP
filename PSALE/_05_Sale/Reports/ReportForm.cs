using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;
using System.Xml;
using System.Drawing.Printing;
using Stimulsoft.Controls;
using Stimulsoft.Controls.Win;
using Stimulsoft.Report;
using Stimulsoft.Report.Win;
using Stimulsoft.Report.Design;
using Stimulsoft.Database;

namespace PSHOP._05_Sale.Reports
{
    public partial class ReportForm : Form
    {
        Classes.Class_Settle cl_Settle = new Classes.Class_Settle();
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        DataTable HeaderTable;
        string _Type;
        DataTable Org;
        DataTable TotalSettleInfo;
        string[] Sign;
        short _PrintStyle = 1;
        List<string> List = new List<string>();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        int _SaleNumber;
        bool _BackSpace = false, _Agg = false, _ClubPrint;
        int ttype;
        double _Price;
        public ReportForm(int SaleNumber, bool ClubPrint,string Type,int _T,double Price)
        {
            InitializeComponent();
            _SaleNumber = SaleNumber;
            _ClubPrint = ClubPrint;
            _Type = Type;
            ttype = _T;
            _Price = Price;
        }

        public ReportForm(List<string> _List, bool ClubPrint)
        {
            InitializeComponent();
            List = _List;
            _ClubPrint = ClubPrint;
        }

        public ReportForm(DataTable _HeaderTable, DataTable _DetailTable, bool ClubPrint)
        {
            InitializeComponent();
            HeaderTable = _HeaderTable;
         
            _ClubPrint = ClubPrint;
            _Agg = true;
        }

        public void Form_FactorPrint_Load(object sender, EventArgs e)
        {
            try
            {



                //faDatePicker1.SelectedDateTime = DateTime.Now.AddMonths(-1);
                //faDatePicker2.SelectedDateTime = DateTime.Now;
                //chk_ShowCustomerBill.Checked = Properties.Settings.Default.ShowCustomerBill;
                //chk_ShowSen.Checked = Properties.Settings.Default.ShowSaleFactorSentence;
                chk_Logo.Checked = Properties.Settings.Default.DontShowLogo;
                _PrintStyle = Properties.Settings.Default.SaleFactorStyle;
               // mlt_ACC.DataSource = clDoc.ReturnTable(ConAcnt.ConnectionString, "Select ACC_Code,ACC_Name from AllHeaders()");
                //if (Properties.Settings.Default.SaveACCRemain.Trim() != "")
                //    mlt_ACC.Value = Properties.Settings.Default.SaveACCRemain;

                //if (Properties.Settings.Default.printorder == 1)
                //    rb_goodCode.Checked = true;
                //else if (Properties.Settings.Default.printorder == 2)
                //    rb_goodname.Checked = true;
                //else
                //    rb_insertorder.Checked = true;

                bt_Display_Click(sender, e);
            }
            catch { }

        }

        private void Form_FactorPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
          //  Properties.Settings.Default.ShowSaleFactorSentence = chk_ShowSen.Checked;
           // Properties.Settings.Default.ShowCustomerBill = chk_ShowCustomerBill.Checked;
           // Properties.Settings.Default.SaleFactorStyle = _PrintStyle;
            Properties.Settings.Default.DontShowLogo = chk_Logo.Checked;
            //if (mlt_ACC.Text.Trim() != "")
            //    Properties.Settings.Default.SaveACCRemain = mlt_ACC.Value.ToString();
            Properties.Settings.Default.Save();

            try
            {
                var Rpt = (CrystalDecisions.CrystalReports.Engine.ReportDocument)crystalReportViewer1.ReportSource;
                Rpt.Database.Dispose();
                Rpt.Close();
                Rpt.Dispose();
                GC.Collect();
            }
            catch
            {
            }

        }

        private void txt_From_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void faDatePicker1_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox = (FarsiLibrary.Win.Controls.FADatePicker)sender;


                if (textBox.Text.Length == 4)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
                else if (textBox.Text.Length == 7)
                {
                    textBox.Text += "/";
                    textBox.SelectionStart = textBox.Text.Length;
                }
            }
        }

        private void faDatePicker1_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (Class_BasicOperation.isNotDigit(e.KeyChar))
            //    e.Handled = true;
            //else
            //    if (e.KeyChar == 13)
            //    {
            //        faDatePicker1.HideDropDown();
            //        faDatePicker2.Select();
            //    }

            //if (e.KeyChar == 8)
            //    _BackSpace = true;
            //else
            //    _BackSpace = false;
        }

        private void faDatePicker2_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (Class_BasicOperation.isNotDigit(e.KeyChar))
            //    e.Handled = true;
            //else
            //    if (e.KeyChar == 13)
            //    {
            //        faDatePicker2.HideDropDown();
            //        chk_ShowCustomerBill.Focus();
            //    }

            //if (e.KeyChar == 8)
            //    _BackSpace = true;
            //else
            //    _BackSpace = false;
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            if (!_Agg)
            {
                if (ttype == 1)
                {
                    string HeaderSelectText = null;
                    HeaderSelectText = @"SELECT     TOP (100) PERCENT FactorTable.FactorID AS ID, FactorTable.DetailId, FactorTable.Serial, FactorTable.LegalNumber, FactorTable.Date, FactorTable.Responsible, 
                      FactorTable.CustomerID, FactorTable.P2Name, FactorTable.P2NationalCode, FactorTable.P2ECode, FactorTable.P2SabtCode, FactorTable.P2Address, 
                      FactorTable.P2Tel, FactorTable.Mobile, FactorTable.P2Fax, FactorTable.P2PostalCode, FactorTable.P2Code, FactorTable.GoodCode, FactorTable.GoodName, 
                      FactorTable.Box, FactorTable.BoxPrice, FactorTable.Pack, FactorTable.PackPrice, FactorTable.Number, FactorTable.TotalNumber, FactorTable.SinglePrice, 
                      FactorTable.TotalPrice, FactorTable.DiscountPercent, FactorTable.DiscountPrice, FactorTable.TaxPrice, FactorTable.TotalWeight, FactorTable.SingleWeight, 
                      FactorTable.TaxPercent, FactorTable.NetPrice, FactorTable.Description, 'SettleInfo' AS SettleInfo, FactorTable.NumberInBox, FactorTable.RowDesc, FactorTable.Zarib, 
                      FactorTable.Series, FactorTable.ExpirationDate, FactorTable.NumberInBox AS Expr1, FactorTable.NumberInPack, CityTable.Column02 AS CityName, 
                      ProvinceTable.Column01 AS ProvinceName, FactorTable.Feild, FactorTable.GoodDesc ,'-' AS charPrice,FactorTable.Column35 as ExpDate,FactorTable.Column34 as BuildSeri, PersonInfoTable.Column02

FROM         (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
                       FROM           " + ConBase.Database + @".dbo.Table_060_ProvinceInfo) AS ProvinceTable INNER JOIN
                          (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                            FROM           " + ConBase.Database + @".dbo.Table_065_CityInfo) AS CityTable ON ProvinceTable.Column00 = CityTable.Column00 RIGHT OUTER JOIN
                        ( SELECT     dbo.Table_070_AmaniFactor.columnid AS FactorID, dbo.Table_075_Child_AmaniFactor.columnid AS DetailId, dbo.Table_070_AmaniFactor.column01 AS Serial, 
                      dbo.Table_070_AmaniFactor.Column37 AS LegalNumber, dbo.Table_070_AmaniFactor.column02 AS Date, dbo.Table_070_AmaniFactor.column03 AS CustomerID, 
                      PersonTable.Column02 AS P2Name, PersonTable.Column09 AS P2NationalCode, PersonTable.Column141 AS P2ECode, PersonTable.Column142 AS P2SabtCode, 
                      PersonTable.Column01 AS P2Code, PersonTable.Column06 AS P2Address, PersonTable.Column07 AS P2Tel, PersonTable.Column19 AS Mobile, 
                      PersonTable.Column08 AS P2Fax, PersonTable.Column13 AS P2PostalCode, PersonTable.Column10 AS Feild, GoodTable.column01 AS GoodCode, 
                      dbo.Table_075_Child_AmaniFactor.column04 AS Box, dbo.Table_075_Child_AmaniFactor.column08 AS BoxPrice, 
                      dbo.Table_075_Child_AmaniFactor.column05 AS Pack, dbo.Table_075_Child_AmaniFactor.column09 AS PackPrice, 
                      dbo.Table_075_Child_AmaniFactor.column06 AS Number, dbo.Table_075_Child_AmaniFactor.column07 AS TotalNumber, 
                      dbo.Table_075_Child_AmaniFactor.column10 AS SinglePrice, dbo.Table_075_Child_AmaniFactor.column11 AS TotalPrice, 
                      dbo.Table_075_Child_AmaniFactor.column16 AS DiscountPercent, dbo.Table_075_Child_AmaniFactor.column17 AS DiscountPrice, 
                      dbo.Table_075_Child_AmaniFactor.column18 AS TaxPercent, dbo.Table_075_Child_AmaniFactor.column19 AS TaxPrice, 
                      dbo.Table_075_Child_AmaniFactor.Column37 AS TotalWeight, dbo.Table_075_Child_AmaniFactor.Column36 AS SingleWeight, 
                      dbo.Table_075_Child_AmaniFactor.column20 AS NetPrice, GoodTable.column02 AS GoodName, dbo.Table_070_AmaniFactor.column05 AS Responsible, 
                      dbo.Table_070_AmaniFactor.column06 AS Description, CountUnitTable.Column01 AS CountUnitName, dbo.Table_075_Child_AmaniFactor.column23 AS RowDesc, 
                      dbo.Table_075_Child_AmaniFactor.Column31 AS NumberInBox, dbo.Table_075_Child_AmaniFactor.Column32 AS NumberInPack, 
                      dbo.Table_075_Child_AmaniFactor.Column33 AS Zarib, dbo.Table_075_Child_AmaniFactor.Column34 AS Series, 
                      dbo.Table_075_Child_AmaniFactor.Column35 AS ExpirationDate, PersonTable.Column21 AS ProvinceId, PersonTable.Column22 AS CityId, 
                      dbo.Table_070_AmaniFactor.column21 AS PayCash, dbo.Table_075_Child_AmaniFactor.column23 AS GoodDesc, dbo.Table_075_Child_AmaniFactor.column34, dbo.Table_075_Child_AmaniFactor.column35
FROM         dbo.Table_070_AmaniFactor INNER JOIN
                      dbo.Table_075_Child_AmaniFactor ON dbo.Table_070_AmaniFactor.columnid = dbo.Table_075_Child_AmaniFactor.column01 INNER JOIN
                          (SELECT     ColumnId, Column00, Column01, Column02, Column03, Column04, Column05, ISNULL
                                                       ((SELECT     Column01
                                                           FROM          " + ConBase.Database + @".dbo.Table_060_ProvinceInfo AS tpi
                                                           WHERE     (Column00 =  " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column21)), ' ') + ' ' + ISNULL
                                                       ((SELECT     Column02
                                                           FROM          " + ConBase.Database + @".dbo.Table_065_CityInfo AS tpi
                                                           WHERE     (Column01 =  " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column22)), ' ') + ' ' + ISNULL(Column06, ' ') AS Column06, Column07, Column08, 
                                                   Column09, Column10, Column11, Column12, Column13, Column21, Column22, Column19, Column141, Column142
                            FROM           " + ConBase.Database + @".dbo.Table_045_PersonInfo) AS PersonTable ON dbo.Table_070_AmaniFactor.column03 = PersonTable.ColumnId LEFT OUTER JOIN
                          (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, 
                                                   column14, column15, column16, column17, column18, column19, column20, column21, column22, column23, column24, column25, column26, column27, 
                                                   column28, column29, column30, column31
                            FROM           " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients) AS GoodTable ON 
                      dbo.Table_075_Child_AmaniFactor.column02 = GoodTable.columnid LEFT OUTER JOIN
                          (SELECT     Column00, Column01
                            FROM           " + ConBase.Database + @".dbo.Table_070_CountUnitInfo) AS CountUnitTable ON dbo.Table_075_Child_AmaniFactor.column03 = CountUnitTable.Column00)
                       AS FactorTable ON CityTable.Column01 = FactorTable.CityId LEFT OUTER JOIN
                          (SELECT     PersonId, Groups
                            FROM          " + ConBase.Database + @" .dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1) AS derivedtbl_1 ON 
                      FactorTable.CustomerID = derivedtbl_1.PersonId LEFT OUTER JOIN
                          (SELECT     ColumnId, Column01, Column02, Column21, Column22
                            FROM           " + ConBase.Database + @".dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1) AS PersonInfoTable ON 
                      FactorTable.Responsible = PersonInfoTable.ColumnId
WHERE     (FactorTable.Serial = " + _SaleNumber + @")
ORDER BY ID, FactorTable.DetailId";

                    HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);



                    HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
                }
                else if (ttype == 2)
                {

                    string HeaderSelectText = null;
                    HeaderSelectText = @"SELECT     TOP (100) PERCENT FactorTable.FactorID AS ID, FactorTable.DetailId, FactorTable.Serial, FactorTable.LegalNumber, FactorTable.Date, FactorTable.Responsible, 
                      FactorTable.CustomerID, FactorTable.P2Name, FactorTable.P2NationalCode, FactorTable.P2ECode, FactorTable.P2SabtCode, FactorTable.P2Address, 
                      FactorTable.P2Tel, FactorTable.Mobile, FactorTable.P2Fax, FactorTable.P2PostalCode, FactorTable.P2Code, FactorTable.GoodCode, FactorTable.GoodName, 
                      FactorTable.Box, FactorTable.BoxPrice, FactorTable.Pack, FactorTable.PackPrice, FactorTable.Number, FactorTable.TotalNumber, FactorTable.SinglePrice, 
                      FactorTable.TotalPrice, FactorTable.DiscountPercent, FactorTable.DiscountPrice, FactorTable.TaxPrice, FactorTable.TotalWeight, FactorTable.SingleWeight, 
                      FactorTable.TaxPercent, FactorTable.NetPrice, FactorTable.Description, 'SettleInfo' AS SettleInfo, FactorTable.NumberInBox, FactorTable.RowDesc, FactorTable.Zarib, 
                      FactorTable.Series, FactorTable.ExpirationDate, FactorTable.NumberInBox AS Expr1, FactorTable.NumberInPack, CityTable.Column02 AS CityName, 
                      ProvinceTable.Column01 AS ProvinceName, FactorTable.Feild, FactorTable.GoodDesc ,'-' AS charPrice,FactorTable.Column35 as ExpDate,FactorTable.Column34 as BuildSeri, PersonInfoTable.Column02

FROM         (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06
                       FROM           " + ConBase.Database + @".dbo.Table_060_ProvinceInfo) AS ProvinceTable INNER JOIN
                          (SELECT     Column00, Column01, Column02, Column03, Column04, Column05, Column06, Column07, Column08
                            FROM           " + ConBase.Database + @".dbo.Table_065_CityInfo) AS CityTable ON ProvinceTable.Column00 = CityTable.Column00 RIGHT OUTER JOIN
                        ( SELECT     dbo.Table_080_ReturnAmani.columnid AS FactorID, dbo.Table_085_ReturnAmaniChild.columnid AS DetailId, dbo.Table_080_ReturnAmani.column01 AS Serial, 
                      dbo.Table_080_ReturnAmani.Column37 AS LegalNumber, dbo.Table_080_ReturnAmani.column02 AS Date, dbo.Table_080_ReturnAmani.column03 AS CustomerID, 
                      PersonTable.Column02 AS P2Name, PersonTable.Column09 AS P2NationalCode, PersonTable.Column141 AS P2ECode, PersonTable.Column142 AS P2SabtCode, 
                      PersonTable.Column01 AS P2Code, PersonTable.Column06 AS P2Address, PersonTable.Column07 AS P2Tel, PersonTable.Column19 AS Mobile, 
                      PersonTable.Column08 AS P2Fax, PersonTable.Column13 AS P2PostalCode, PersonTable.Column10 AS Feild, GoodTable.column01 AS GoodCode, 
                      dbo.Table_085_ReturnAmaniChild.column04 AS Box, dbo.Table_085_ReturnAmaniChild.column08 AS BoxPrice, 
                      dbo.Table_085_ReturnAmaniChild.column05 AS Pack, dbo.Table_085_ReturnAmaniChild.column09 AS PackPrice, 
                      dbo.Table_085_ReturnAmaniChild.column06 AS Number, dbo.Table_085_ReturnAmaniChild.column07 AS TotalNumber, 
                      dbo.Table_085_ReturnAmaniChild.column10 AS SinglePrice, dbo.Table_085_ReturnAmaniChild.column11 AS TotalPrice, 
                      dbo.Table_085_ReturnAmaniChild.column16 AS DiscountPercent, dbo.Table_085_ReturnAmaniChild.column17 AS DiscountPrice, 
                      dbo.Table_085_ReturnAmaniChild.column18 AS TaxPercent, dbo.Table_085_ReturnAmaniChild.column19 AS TaxPrice, 
                      dbo.Table_085_ReturnAmaniChild.Column37 AS TotalWeight, dbo.Table_085_ReturnAmaniChild.Column36 AS SingleWeight, 
                      dbo.Table_085_ReturnAmaniChild.column20 AS NetPrice, GoodTable.column02 AS GoodName, dbo.Table_080_ReturnAmani.column05 AS Responsible, 
                      dbo.Table_080_ReturnAmani.column06 AS Description, CountUnitTable.Column01 AS CountUnitName, dbo.Table_085_ReturnAmaniChild.column23 AS RowDesc, 
                      dbo.Table_085_ReturnAmaniChild.Column31 AS NumberInBox, dbo.Table_085_ReturnAmaniChild.Column32 AS NumberInPack, 
                      dbo.Table_085_ReturnAmaniChild.Column33 AS Zarib, dbo.Table_085_ReturnAmaniChild.Column34 AS Series, 
                      dbo.Table_085_ReturnAmaniChild.Column35 AS ExpirationDate, PersonTable.Column21 AS ProvinceId, PersonTable.Column22 AS CityId, 
                      dbo.Table_080_ReturnAmani.column21 AS PayCash, dbo.Table_085_ReturnAmaniChild.column23 AS GoodDesc, dbo.Table_085_ReturnAmaniChild.column34, dbo.Table_085_ReturnAmaniChild.column35
FROM         dbo.Table_080_ReturnAmani INNER JOIN
                      dbo.Table_085_ReturnAmaniChild ON dbo.Table_080_ReturnAmani.columnid = dbo.Table_085_ReturnAmaniChild.column01 INNER JOIN
                          (SELECT     ColumnId, Column00, Column01, Column02, Column03, Column04, Column05, ISNULL
                                                       ((SELECT     Column01
                                                           FROM          " + ConBase.Database + @".dbo.Table_060_ProvinceInfo AS tpi
                                                           WHERE     (Column00 =  " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column21)), ' ') + ' ' + ISNULL
                                                       ((SELECT     Column02
                                                           FROM          " + ConBase.Database + @".dbo.Table_065_CityInfo AS tpi
                                                           WHERE     (Column01 =  " + ConBase.Database + @".dbo.Table_045_PersonInfo.Column22)), ' ') + ' ' + ISNULL(Column06, ' ') AS Column06, Column07, Column08, 
                                                   Column09, Column10, Column11, Column12, Column13, Column21, Column22, Column19, Column141, Column142
                            FROM           " + ConBase.Database + @".dbo.Table_045_PersonInfo) AS PersonTable ON dbo.Table_080_ReturnAmani.column03 = PersonTable.ColumnId LEFT OUTER JOIN
                          (SELECT     columnid, column01, column02, column03, column04, column05, column06, column07, column08, column09, column10, column11, column12, column13, 
                                                   column14, column15, column16, column17, column18, column19, column20, column21, column22, column23, column24, column25, column26, column27, 
                                                   column28, column29, column30, column31
                            FROM           " + ConWare.Database + @".dbo.table_004_CommodityAndIngredients) AS GoodTable ON 
                      dbo.Table_085_ReturnAmaniChild.column02 = GoodTable.columnid LEFT OUTER JOIN
                          (SELECT     Column00, Column01
                            FROM           " + ConBase.Database + @".dbo.Table_070_CountUnitInfo) AS CountUnitTable ON dbo.Table_085_ReturnAmaniChild.column03 = CountUnitTable.Column00)
                       AS FactorTable ON CityTable.Column01 = FactorTable.CityId LEFT OUTER JOIN
                          (SELECT     PersonId, Groups
                            FROM          " + ConBase.Database + @" .dbo.FN_01_PersonGroup() AS FN_01_PersonGroup_1) AS derivedtbl_1 ON 
                      FactorTable.CustomerID = derivedtbl_1.PersonId LEFT OUTER JOIN
                          (SELECT     ColumnId, Column01, Column02, Column21, Column22
                            FROM           " + ConBase.Database + @".dbo.Table_045_PersonInfo AS Table_045_PersonInfo_1) AS PersonInfoTable ON 
                      FactorTable.Responsible = PersonInfoTable.ColumnId
WHERE     (FactorTable.Serial = " + _SaleNumber + @")
ORDER BY ID, FactorTable.DetailId";

                    HeaderSelectText = string.Format(HeaderSelectText, ConBase.Database, ConWare.Database);
                    HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, HeaderSelectText);
                }
            }

            foreach (DataRow item in HeaderTable.Rows)
            {
                //double FinalPrice = Convert.ToDouble(item["NetTotal"].ToString()) +
                //            Convert.ToDouble(item["Ezafat"].ToString()) - Convert.ToDouble(item["Kosoorat"].ToString()) -
                //            Convert.ToDouble(item["SpecialGroup"].ToString()) - Convert.ToDouble(item["SpecialCustomer"].ToString())
                //            - Convert.ToDouble(item["VolumeGroup"].ToString());
                item.BeginEdit();
                item["CharPrice"] = FarsiLibrary.Utils.ToWords.ToString(Convert.ToInt64( _Price)) + " ریال";
                item.EndEdit();
            }




            //فراخوانی عناوین امضا
            Sign = clDoc.Signature(10);


            //جدول اطلاعات تسویه فاکتورها
         //   DataTable CustomerTable = HeaderTable.DefaultView.ToTable(true, new string[] { "CustomerId", "ID", "Date", "DocId" });
            //DataTable CustomerSettleInfoTbl = new DataTable();
           // TotalSettleInfo = dataSet_Sale21.FN_01_SettleInfo.Clone();
            //foreach (DataRow item in CustomerTable.Rows)
            //{
            //    SqlDataAdapter Adapter = new SqlDataAdapter("Select * from FN_01_SettleInfo(" + item["CustomerId"].ToString() + ")", ConSale);
            //    CustomerSettleInfoTbl = new DataTable();
            //    Adapter.Fill(CustomerSettleInfoTbl);
            //    TotalSettleInfo.Rows.Add(item["CustomerId"].ToString(),
            //        CustomerSettleInfoTbl.Rows[0]["Column00"].ToString(),
            //        (CustomerSettleInfoTbl.Rows[0]["Column01"].ToString().Trim()==""?"0":CustomerSettleInfoTbl.Rows[0]["Column01"].ToString()),
            //        (CustomerSettleInfoTbl.Rows[0]["Column02"].ToString().Trim() == "" ? "0" : CustomerSettleInfoTbl.Rows[0]["Column02"].ToString()),
            //        (CustomerSettleInfoTbl.Rows[0]["Column03"].ToString().Trim() == "" ? "0" : CustomerSettleInfoTbl.Rows[0]["Column03"].ToString()),
            //        (CustomerSettleInfoTbl.Rows[0]["Column04"].ToString().Trim()==""?"0":CustomerSettleInfoTbl.Rows[0]["Column04"].ToString()), item["ID"].ToString());

            //}
            try
            {
              //  String Sentence = null;

//                if (chk_ShowCustomerBill.Checked && mlt_ACC.Text.Trim() != "")
//                {
//                    foreach (DataRow item in CustomerTable.Rows)
//                    {

//                        double X = 0;
//                        double Y = 0;

//                        double Remain = Convert.ToInt64(clDoc.ReturnTable(ConAcnt.ConnectionString, @"select ISNULL((
//                    Select SUM(Column11)-SUM(Column12) from Table_065_SanadDetail inner join Table_060_SanadHead
//                    ON Table_065_SanadDetail.Column00=Table_060_SanadHead.ColumnId
//                    where Table_065_SanadDetail.Column07=" + item["CustomerId"].ToString() + @" and   Table_065_SanadDetail.Column01='" + mlt_ACC.Value.ToString() + @"' and
//                    Table_060_SanadHead.Column01<='" + item["Date"].ToString() + @"'
//                    group by Table_065_SanadDetail.Column07),0) as Remain").Rows[0][0].ToString());

//                        Sentence = null;
//                        double FinalPrice = Convert.ToDouble(HeaderTable.Compute("Max( NetTotal )", "ID=" + item["ID"]))
//                                             + Convert.ToDouble(HeaderTable.Compute("Max( Ezafat )", "ID=" + item["ID"]))
//                                             - Convert.ToDouble(HeaderTable.Compute("Max( Kosoorat )", "ID=" + item["ID"]))
//                                             - Convert.ToDouble(HeaderTable.Compute("Max( SpecialGroup )", "ID=" + item["ID"]))
//                                            - Convert.ToDouble(HeaderTable.Compute("Max( SpecialCustomer )", "ID=" + item["ID"]))
//                                             - Convert.ToDouble(HeaderTable.Compute("Max( VolumeGroup )", "ID=" + item["ID"]));

//                        if (item["DocId"].ToString() != "0")
//                        {


//                            X = Remain - FinalPrice;
//                            Y = X + FinalPrice;

//                            Sentence = " مانده حساب بدون احتساب این فاکتور : " + Math.Abs(X).ToString("n0") + " ریال" + (X < 0 ? " بستانکار " : (X > 0 ? " بدهکار " : " ")) + Environment.NewLine +
//                                    " مانده حساب بااحتساب این فاکتور: " + Math.Abs(Y).ToString("n0") + " ریال" + (Y < 0 ? " بستانکار " : (Y > 0 ? " بدهکار " : " "));

//                        }
//                        else
//                        {



//                            X = Remain;
//                            Y = X + FinalPrice;

//                            Sentence = " مانده حساب بدون احتساب این فاکتور : " + Math.Abs(X).ToString("n0") + " ریال" + (X < 0 ? " بستانکار " : (X > 0 ? " بدهکار " : " ")) + Environment.NewLine +
//                                   " مانده حساب با احتساب این فاکتور: " + Math.Abs(Y).ToString("n0") + " ریال" + (Y < 0 ? " بستانکار " : (Y > 0 ? " بدهکار " : " "));
//                        }



//                        TotalSettleInfo.Rows.Add(item["CustomerId"].ToString(),
//                               Sentence,
//                               "0",
//                               "0",
//                               "0",
//                               "0", item["ID"].ToString());

//                    }
//                }
//                else if (chk_ShowCustomerBill.Checked && mlt_ACC.Text.Trim() == "")
//                {

//                    foreach (DataRow item in CustomerTable.Rows)
//                    {

//                        double X = 0;
//                        double Y = 0;

//                        Int64 Remain = Convert.ToInt64(clDoc.ReturnTable(ConAcnt.ConnectionString, @"select ISNULL((
//                                Select SUM(Column11)-SUM(Column12) from Table_065_SanadDetail inner join Table_060_SanadHead
//                                ON Table_065_SanadDetail.Column00=Table_060_SanadHead.ColumnId
//                                where Table_065_SanadDetail.Column07=" + item["CustomerId"].ToString() + @" and
//                                Table_060_SanadHead.Column01<='" + item["Date"].ToString() + @"'
//                                group by Table_065_SanadDetail.Column07),0) as Remain").Rows[0][0].ToString());
//                        Sentence = null;
//                        double FinalPrice = Convert.ToDouble(HeaderTable.Compute("Max( NetTotal )", "ID=" + item["ID"]))
//                                             + Convert.ToDouble(HeaderTable.Compute("Max( Ezafat )", "ID=" + item["ID"]))
//                                             - Convert.ToDouble(HeaderTable.Compute("Max( Kosoorat )", "ID=" + item["ID"]))
//                                             - Convert.ToDouble(HeaderTable.Compute("Max( SpecialGroup )", "ID=" + item["ID"]))
//                                            - Convert.ToDouble(HeaderTable.Compute("Max( SpecialCustomer )", "ID=" + item["ID"]))
//                                             - Convert.ToDouble(HeaderTable.Compute("Max( VolumeGroup )", "ID=" + item["ID"]));

//                        if (item["DocId"].ToString() != "0")
//                        {

//                            X = Remain - FinalPrice;
//                            Y = X + FinalPrice;

//                            Sentence = " مانده حساب بدون احتساب این فاکتور : " + Math.Abs(X).ToString("n0") + " ریال" + (X < 0 ? " بستانکار " : (X > 0 ? " بدهکار " : " ")) + Environment.NewLine +
//                                     " مانده حساب با احتساب این فاکتور: " + Math.Abs(Y).ToString("n0") + " ریال" + (Y < 0 ? " بستانکار " : (Y > 0 ? " بدهکار " : " "));
//                        }
//                        else
//                        {
//                            X = Remain;
//                            Y = X + FinalPrice;

//                            Sentence = " مانده حساب بدون احتساب این فاکتور : " + Math.Abs(X).ToString("n0") + " ریال" + (X < 0 ? " بستانکار " : (X > 0 ? " بدهکار " : " ")) + Environment.NewLine +
//                                    " مانده حساب با احتساب این فاکتور: " + Math.Abs(Y).ToString("n0") + " ریال" + (Y < 0 ? " بستانکار " : (Y > 0 ? " بدهکار " : " "));
//                        }



//                        TotalSettleInfo.Rows.Add(item["CustomerId"].ToString(),
//                               Sentence,
//                               "0",
//                               "0",
//                               "0",
//                               "0", item["ID"].ToString());

//                    }


//                }
            }
            catch { }


            Org = Class_BasicOperation.LogoTable();
            if (_PrintStyle == 26 || _PrintStyle == 27)
            {
               
                crystalReportViewer1.Visible = false;
            }

            else
            {
                
                crystalReportViewer1.Visible = true;
            }

             btn_30_Click(sender, e);
                 

        }

  

        private void chk_ShowCustomerBill_CheckedChanged(object sender, EventArgs e)
        {
            //if (chk_ShowCustomerBill.Checked)
            //    mlt_ACC.Enabled = true;
            //else mlt_ACC.Enabled = false;
        }

        private void mlt_ACC_KeyPress(object sender, KeyPressEventArgs e)
        {
          //  mlt_ACC.DroppedDown = true;
        }

     
        private void btn_Design_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("LCustomReport.mrt");
            stireport.Design();
        }

        private void rb_goodname_CheckedChanged(object sender, EventArgs e)
        {
            //if (rb_goodname.Checked)
            //{
            //    Properties.Settings.Default.printorder = 2;
            //    Properties.Settings.Default.Save();
            //    bt_Display_Click(null, null);
            //}
        }

        private void rb_goodCode_CheckedChanged(object sender, EventArgs e)
        {
            //if (rb_goodCode.Checked)
            //{
            //    Properties.Settings.Default.printorder = 1;
            //    Properties.Settings.Default.Save();
            //    bt_Display_Click(null, null);
            //}
        }

        private void rb_insertorder_CheckedChanged(object sender, EventArgs e)
        {
            //if (rb_insertorder.Checked)
            //{
            //    Properties.Settings.Default.printorder = 0;
            //    Properties.Settings.Default.Save();
            //    bt_Display_Click(null, null);
            //}
        }

        private void btn_P_Click(object sender, EventArgs e)
        {
            StiReport stireport = new StiReport();
            stireport.Load("PCustomReport.mrt");
            stireport.Design();
        }

     

        private void btn_30_Click(object sender, EventArgs e)
        {

            crystalReportViewer1.Visible = true;
            this.Cursor = Cursors.WaitCursor;
            _05_Sale.Reports.Rpt_02_SaleFactor14Amani Rpt1 = new Rpt_02_SaleFactor14Amani();
            //TextObject Type1 = (TextObject)Rpt1.ReportDefinition.ReportObjects["Text31"];
            //TextObject Type2 = (TextObject)Rpt1.ReportDefinition.ReportObjects["Text35"];
            //TextObject Type3 = (TextObject)Rpt1.ReportDefinition.ReportObjects["Text36"];

            //DataTable PriceTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column02 from Table_002_SalesTypes");
            //Type1.Text = PriceTable.Rows[0]["Column02"].ToString();
            //Type2.Text = PriceTable.Rows[1]["Column02"].ToString();
            //Type3.Text = PriceTable.Rows[2]["Column02"].ToString();

            if (!chk_Logo.Checked)
            {
                Rpt1.Subreports["Logo"].SetDataSource(Org);
                Rpt1.Subreports["Company"].SetDataSource(Org);
            }
            else
            {
                Rpt1.Subreports["Logo"].SetDataSource(Org.Clone());
                Rpt1.Subreports["Company"].SetDataSource(Org.Clone());
            }

            Rpt1.SetDataSource(HeaderTable);
            if (!chk_Logo.Checked)
            {

                Rpt1.SetParameterValue("P1Name", Org.Rows[0]["Column01"].ToString());
                Rpt1.SetParameterValue("P1ECode", Org.Rows[0]["Column06"].ToString());
                Rpt1.SetParameterValue("P1NCode", Org.Rows[0]["Column07"].ToString());
                Rpt1.SetParameterValue("P1Address", Org.Rows[0]["Column02"].ToString());
                Rpt1.SetParameterValue("P1Tel", Org.Rows[0]["Column03"].ToString());
                Rpt1.SetParameterValue("P1PostalCode", Org.Rows[0]["Column14"].ToString());
            }
            else
            {
                Rpt1.SetParameterValue("P1Name", " ");
                Rpt1.SetParameterValue("P1ECode", " ");
                Rpt1.SetParameterValue("P1NCode", " ");
                Rpt1.SetParameterValue("P1Address", " ");
                Rpt1.SetParameterValue("P1Tel", " ");
                Rpt1.SetParameterValue("P1PostalCode", " ");
            }
            Rpt1.SetParameterValue("Param3", Sign[0]);
            Rpt1.SetParameterValue("Param4", Sign[1]);
            Rpt1.SetParameterValue("Param5", Sign[2]);
            Rpt1.SetParameterValue("Param6", Sign[3]);
            Rpt1.SetParameterValue("Param7", Sign[4]);
            Rpt1.SetParameterValue("Param8", Sign[5]);
            Rpt1.SetParameterValue("Param9", Sign[6]);
            Rpt1.SetParameterValue("Param10", Sign[7]);
            Rpt1.SetParameterValue("Type", _Type);
            crystalReportViewer1.ReportSource = Rpt1;
            this.Cursor = Cursors.Default;
            _PrintStyle = 28;
            if (List.Count > 0)
            {
                PrinterSettings getprinterName = new PrinterSettings();
                Rpt1.PrintOptions.PrinterName = getprinterName.PrinterName;
                Rpt1.PrintToPrinter(1, true, 1, 1000);
            }
        }

        




    }
}