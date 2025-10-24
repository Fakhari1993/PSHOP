using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;

namespace PSHOP._05_Sale
{
    public partial class Frm_003_MojoodiMaghtai_NegativeDate : Form
    {
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConAcnt = new SqlConnection(Properties.Settings.Default.ACNT);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();

        string Date1;
        string Date2;
        bool _BackSpace = false;
        string whr = string.Empty;

        Class_UserScope UserScope = new Class_UserScope();

        public Frm_003_MojoodiMaghtai_NegativeDate()
        {
            InitializeComponent();
        }

      

        private void Frm_003_MojoodiMaghtai_NegativeDate_Load(object sender, EventArgs e)
        {
            SqlDataAdapter Adapter = new SqlDataAdapter(@"Select * from Table_001_PWHRS
                                                          where columnid not in (select Column02 from " + ConAcnt.Database + ".[dbo].[Table_200_UserAccessInfo] where Column03=5 and Column01=N'" + Class_BasicOperation._UserName + @"')
                                                                ", ConWare);
            DataTable WareTable = new DataTable();
            Adapter.Fill(WareTable);


            Adapter = new SqlDataAdapter("Select * from Table_070_CountUnitInfo", ConBase);
            DataTable Table = new DataTable();
            Adapter.Fill(Table);
            gridEX2.DropDowns["Unit"].DataSource = Table;
            gridEX1.DropDowns["Unit"].DataSource = Table;


            DataRow Row = WareTable.NewRow();
            Row["ColumnId"] = 0;
            Row["Column02"] = "همه انبارها";
            WareTable.Rows.InsertAt(Row, 0);
            mlt_Ware.DropDownDataSource = WareTable;
            mlt_Ware.DropDownList.SetValue("ColumnId", 0);


          gridEX2.DropDowns["WHRS"].DataSource =  gridEX1.DropDowns["WHRS"].DataSource = clDoc.ReturnTable(ConWare.ConnectionString, @"select ColumnId,Column02 from Table_001_PWHRS");
         
          
            faDate1.SelectedDateTime = DateTime.Now;
            faDate2.SelectedDateTime = DateTime.Now;
        }

        private void faDate1_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDate1.HideDropDown();
                    faDate2.Select();
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void faDate1_TextChanged_1(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox =
                    (FarsiLibrary.Win.Controls.FADatePicker)sender;


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

        private void faDate2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Class_BasicOperation.isNotDigit(e.KeyChar))
                e.Handled = true;
            else
                if (e.KeyChar == 13)
                {
                    faDate2.HideDropDown();
                    bt_Display_Click(sender, e);
                }

            if (e.KeyChar == 8)
                _BackSpace = true;
            else
                _BackSpace = false;
        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            try
            {
                if (mlt_Ware.DropDownList.GetCheckedRows().Count() == 0)
                {
                    MessageBox.Show("انبار مورد نظر را انتخاب کنید");
                    return;
                }

                if (faDate1.SelectedDateTime.HasValue && faDate1.Text.Trim() != "" || faDate2.SelectedDateTime.HasValue && faDate2.Text.Trim() != "")
                {
                    Date1 = faDate1.Text;
                    Date2 = faDate2.Text;

                    //یک انبار خاص
                    if (mlt_Ware.Text.Trim() != "" && mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString() != "0")
                    {
                        string whr = string.Empty;

                        foreach (Janus.Windows.GridEX.GridEXRow dr in mlt_Ware.DropDownList.GetCheckedRows())
                        {
                            if (dr.Cells["ColumnId"].Value.ToString() != "0")
                                whr += dr.Cells["ColumnId"].Value + ",";
                        }
                        whr = whr.TrimEnd(',');

                        SqlDataAdapter Adapter = new SqlDataAdapter(


@"


select * from (
select s.column02 date,schild.column02 goodid,sum(schild.column07)TotalNumber,s.column42 WHRS,g.column01 Goodcode,g.column02 GoodName,g.column07 UnitCount,
          
                     isnull(  (   SELECT SUM(rch.column07)  
                                     FROM   dbo.Table_011_PwhrsReceipt r
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt rch
                                                 ON  r.columnid = 
                                                     rch.column01
                                     WHERE  (r.column03 = s.column42)
                                            AND (rch.column02 = schild.column02)
                                            AND r.column02  <=s.column02
								 ANd r.column02<'" + Date1 + @"'
                                     GROUP BY
                                            rch.column02
                                
                              ),0)
					-      isnull(  (   SELECT SUM(dch.column07)  
                                     FROM   dbo.Table_007_PwhrsDraft d
                                            INNER JOIN dbo.Table_008_Child_PwhrsDraft dch
                                                 ON  d.columnid = 
                                                     dch.column01
                                     WHERE  (d.column03 = s.column42)
                                            AND (dch.column02 = schild.column02)
                                            AND d.column02  <=s.column02
								 ANd d.column02<'" + Date1 + @"'
                                     GROUP BY
                                            dch.column02
                                
                              ),0)AS Firstperiod,

						 isnull(  (   SELECT SUM(rch.column07)  
                                     FROM   dbo.Table_011_PwhrsReceipt r
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt rch
                                                 ON  r.columnid = 
                                                     rch.column01
                                     WHERE  (r.column03 = s.column42)
                                            AND (rch.column02 = schild.column02)
                                            AND r.column02  <=s.column02
								  ANd 
								  r.column02>='" + Date1 + @"' and
								   r.column02<=s.column02
                                     GROUP BY
                                            rch.column02
                                
                              ),0)
						-       isnull(  (   SELECT SUM(dch.column07)  
                                     FROM   dbo.Table_007_PwhrsDraft d
                                            INNER JOIN dbo.Table_008_Child_PwhrsDraft dch
                                                 ON  d.columnid = 
                                                     dch.column01
                                     WHERE  (d.column03 = s.column42)
                                            AND (dch.column02 = schild.column02)
                                            AND d.column02  <=s.column02
								 ANd d.column02>='" + Date1 + @"' 
								 and d.column02<=s.column02
                                     GROUP BY
                                            dch.column02
                                
                              ),0)AS RTotalNumber
					
					,   

						 isnull(  (   SELECT SUM(rch.column07)  
                                     FROM   dbo.Table_011_PwhrsReceipt r
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt rch
                                                 ON  r.columnid = 
                                                     rch.column01
                                     WHERE  (r.column03 = s.column42)
                                            AND (rch.column02 = schild.column02)
                                            AND r.column02  <=s.column02
								  ANd 
								  --r.column02>='" + Date1 + @"' and
								   r.column02<=s.column02
                                     GROUP BY
                                            rch.column02
                                
                              ),0)
						-       isnull(  (   SELECT SUM(dch.column07)  
                                     FROM   dbo.Table_007_PwhrsDraft d
                                            INNER JOIN dbo.Table_008_Child_PwhrsDraft dch
                                                 ON  d.columnid = 
                                                     dch.column01
                                     WHERE  (d.column03 = s.column42)
                                            AND (dch.column02 = schild.column02)
                                            AND d.column02  <=s.column02
								 --ANd d.column02>='" + Date1 + @"' 
								 and d.column02<=s.column02
                                     GROUP BY
                                            dch.column02
                                
                              ),0)AS RemainTotal
               
 from " + ConSale.Database + @".dbo.Table_010_SaleFactor s Inner join " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor  schild on s.columnid=schild.column01
 left join table_004_CommodityAndIngredients g on schild.column02=g.columnid

group by schild.column07,s.column02,schild.column02,s.column42,g.column01 ,g.column02,g.column07

having s.column42=" + whr.TrimEnd(',') + " and s.column02>='" + Date1 + @"' and s.column02<='" + Date2 + @"'  ) as t where t.RemainTotal<0 

 order by date ,goodid   ", ConWare);



                        Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText, ConBase.Database);
                        DataTable Table = new DataTable();
                        Adapter.Fill(Table);
                        bindingSource1 = new BindingSource();
                        bindingSource1.DataSource = Table;
                        bindingNavigator2.BindingSource = bindingSource1;

                        if (chk_ontatal.Checked)
                        {
                            gridEX1.Visible = true;
                            gridEX2.Visible = false;
                            gridEX1.DataSource = bindingSource1;
                        }

                        else
                        {
                            gridEX1.Visible = false;
                            gridEX2.Visible = true;
                            gridEX2.DataSource = bindingSource1;
                        }
                    }
                    //همه انبارها
                    else if (mlt_Ware.Text.Trim() != "" && mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString() == "0")
                    {
                        SqlDataAdapter Adapter = new SqlDataAdapter(
@" select * from (
select s.column02 date,schild.column02 goodid,sum(schild.column07)TotalNumber,s.column42 WHRS,g.column01 Goodcode,g.column02 GoodName,g.column07 UnitCount,
          
                     isnull(  (   SELECT SUM(rch.column07)  
                                     FROM   dbo.Table_011_PwhrsReceipt r
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt rch
                                                 ON  r.columnid = 
                                                     rch.column01
                                     WHERE  (r.column03 = s.column42)
                                            AND (rch.column02 = schild.column02)
                                            AND r.column02  <=s.column02
								 ANd r.column02<'" + Date1 + @"'
                                     GROUP BY
                                            rch.column02
                                
                              ),0)
					-      isnull(  (   SELECT SUM(dch.column07)  
                                     FROM   dbo.Table_007_PwhrsDraft d
                                            INNER JOIN dbo.Table_008_Child_PwhrsDraft dch
                                                 ON  d.columnid = 
                                                     dch.column01
                                     WHERE  (d.column03 = s.column42)
                                            AND (dch.column02 = schild.column02)
                                            AND d.column02  <=s.column02
								 ANd d.column02<'" + Date1 + @"'
                                     GROUP BY
                                            dch.column02
                                
                              ),0)AS Firstperiod,

						 isnull(  (   SELECT SUM(rch.column07)  
                                     FROM   dbo.Table_011_PwhrsReceipt r
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt rch
                                                 ON  r.columnid = 
                                                     rch.column01
                                     WHERE  (r.column03 = s.column42)
                                            AND (rch.column02 = schild.column02)
                                            AND r.column02  <=s.column02
								  ANd 
								  r.column02>='" + Date1 + @"' and
								   r.column02<=s.column02
                                     GROUP BY
                                            rch.column02
                                
                              ),0)
						-       isnull(  (   SELECT SUM(dch.column07)  
                                     FROM   dbo.Table_007_PwhrsDraft d
                                            INNER JOIN dbo.Table_008_Child_PwhrsDraft dch
                                                 ON  d.columnid = 
                                                     dch.column01
                                     WHERE  (d.column03 = s.column42)
                                            AND (dch.column02 = schild.column02)
                                            AND d.column02  <=s.column02
								 ANd d.column02>='" + Date1 + @"' 
								 and d.column02<=s.column02
                                     GROUP BY
                                            dch.column02
                                
                              ),0)AS RTotalNumber
					
					,   

						 isnull(  (   SELECT SUM(rch.column07)  
                                     FROM   dbo.Table_011_PwhrsReceipt r
                                            INNER JOIN dbo.Table_012_Child_PwhrsReceipt rch
                                                 ON  r.columnid = 
                                                     rch.column01
                                     WHERE  (r.column03 = s.column42)
                                            AND (rch.column02 = schild.column02)
                                            AND r.column02  <=s.column02
								  ANd 
								  --r.column02>='" + Date1 + @"' and
								   r.column02<=s.column02
                                     GROUP BY
                                            rch.column02
                                
                              ),0)
						-       isnull(  (   SELECT SUM(dch.column07)  
                                     FROM   dbo.Table_007_PwhrsDraft d
                                            INNER JOIN dbo.Table_008_Child_PwhrsDraft dch
                                                 ON  d.columnid = 
                                                     dch.column01
                                     WHERE  (d.column03 = s.column42)
                                            AND (dch.column02 = schild.column02)
                                            AND d.column02  <=s.column02
								 --ANd d.column02>='" + Date1 + @"' 
								 and d.column02<=s.column02
                                     GROUP BY
                                            dch.column02
                                
                              ),0)AS RemainTotal
               
 from " + ConSale.Database + @".dbo.Table_010_SaleFactor s Inner join " + ConSale.Database + @".dbo.Table_011_Child1_SaleFactor  schild on s.columnid=schild.column01
 left join table_004_CommodityAndIngredients g on schild.column02=g.columnid

group by schild.column07,s.column02,schild.column02,s.column42,g.column01 ,g.column02,g.column07

having  s.column02>='" + Date1 + @"' and s.column02<='" + Date2 + @"'  ) as t where t.RemainTotal<0 

 order by date ,goodid      "

                                                      , ConWare);
                        Adapter.SelectCommand.CommandText = string.Format(Adapter.SelectCommand.CommandText,
                            Date1, ConBase.Database);
                        DataTable Table = new DataTable();
                        Adapter.Fill(Table);
                        bindingSource1 = new BindingSource();
                        bindingSource1.DataSource = Table;
                        bindingNavigator2.BindingSource = bindingSource1;
                        gridEX1.DataSource = bindingSource1;
                        gridEX2.DataSource = bindingSource1;

                        //if (chk_ontatal.Checked)
                        //{
                        //    gridEX1.Visible = true;
                        //    gridEX2.Visible = false;
                        //    gridEX1.DataSource = bindingSource1;
                        //}

                        //else
                        //{
                        //    gridEX1.Visible = false;
                        //    gridEX2.Visible = true;
                        //    gridEX2.DataSource = bindingSource1;
                        //}

                    }
                }
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void faDate2_TextChanged(object sender, EventArgs e)
        {
            if (!_BackSpace)
            {
                FarsiLibrary.Win.Controls.FADatePicker textBox =
                    (FarsiLibrary.Win.Controls.FADatePicker)sender;


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

        private void gridEX2_FormattingRow(object sender, Janus.Windows.GridEX.RowLoadEventArgs e)
        {

        }


 

        private void bt_Print_Click(object sender, EventArgs e)
        {
            string whrsSale = string.Empty;
            if (mlt_Ware.Text.Trim() != "" && mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString() != "0")
            {
                foreach (Janus.Windows.GridEX.GridEXRow dr in mlt_Ware.DropDownList.GetCheckedRows())
                {

                    if (dr.Cells["ColumnId"].Value.ToString() != "0")
                        whrsSale += dr.Cells["ColumnId"].Value + ",";


                }
                whrsSale = whrsSale.TrimEnd(',');
            }
            else if (mlt_Ware.Text.Trim() != "" && mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString() == "0")
            {

                whrsSale = "0";
            
            }
            


            if ( Date1 == "" || Date2 == "")
            {
                MessageBox.Show("لطفا برای چاپ بارکد مورد نظر را انتخاب نمایید");
                return;
            }
            else
            {
                _05_Sale.Reports.Frm_SaleFactorNegative frm = new _05_Sale.Reports.Frm_SaleFactorNegative(whrsSale, Date1, Date2);
                frm.Show();
            }

            //if (chk_ontatal.Checked)
            //{

            //    if (gridEX1.RowCount > 0)
            //    {
            //        try
            //        {
            //            gridEXPrintDocument1.GridEX = gridEX1;
            //            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
            //                if (printDialog1.ShowDialog() == DialogResult.OK)
            //                {
            //                    gridEXPrintDocument1.PageHeaderLeft = "گزارش موجودی منفی کالا";
            //                    gridEXPrintDocument1.PageHeaderRight = " تا تاریخ " + faDate2.Text + "***" + " انبار: " + mlt_Ware.Text;
            //                    gridEXPrintDocument1.PageFooterLeft =
            //          FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd") +
            //          "**" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
            //                    gridEXPrintDocument1.PageFooterRight =
            //                      " کاربر " + Class_BasicOperation._UserName;
            //                    printPreviewDialog1.ShowDialog();
            //                }
            //        }
            //        catch { }


            //        /*
            //        DataTable Table = dataSet_001_Gozareshat.Rpt_Maghta_Tedad.Clone();
            //        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
            //        {
            //            Table.Rows.Add(item.Cells["GoodCode"].Text, item.Cells["GoodName"].Text,
            //                item.Cells["TINumberInBox"].Value.ToString(),
            //                item.Cells["TINumberInPack"].Value.ToString(),
            //                item.Cells["InDetail"].Value.ToString(),
            //                item.Cells["InTotal"].Value.ToString(),
            //             (item.Cells["TONumberInBox"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TONumberInBox"].Value.ToString()) ? Convert.ToDouble(item.Cells["TONumberInBox"].Value.ToString()) : Convert.ToDouble(0)),
            //             (item.Cells["TONumberInPack"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TONumberInPack"].Value.ToString()) ? Convert.ToDouble(item.Cells["TONumberInPack"].Value.ToString()) : Convert.ToDouble(0)),
            //                item.Cells["OutDetail"].Value.ToString(),
            //                item.Cells["OutTotal"].Value.ToString(),
            //             (item.Cells["TRNumberInBox"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TRNumberInBox"].Value.ToString()) ? Convert.ToDouble(item.Cells["TRNumberInBox"].Value.ToString()) : Convert.ToDouble(0)),
            //             (item.Cells["TRNumberInPack"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TRNumberInPack"].Value.ToString()) ? Convert.ToDouble(item.Cells["TRNumberInPack"].Value.ToString()) : Convert.ToDouble(0)),
            //                item.Cells["ReDetail"].Value.ToString(),
            //                item.Cells["ReTotal"].Value.ToString(),
            //                item.Cells["Project"].Text,
            //             (item.Cells["TOTotalWeight"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TOTotalWeight"].Value.ToString()) ? Convert.ToDouble(item.Cells["TOTotalWeight"].Value.ToString()) : Convert.ToDouble(0)),
            //             (item.Cells["TITotalWeight"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TITotalWeight"].Value.ToString()) ? Convert.ToDouble(item.Cells["TITotalWeight"].Value.ToString()) : Convert.ToDouble(0)),
            //             (item.Cells["TRTotalWeight"].Value != null && !string.IsNullOrWhiteSpace(item.Cells["TRTotalWeight"].Value.ToString()) ? Convert.ToDouble(item.Cells["TRTotalWeight"].Value.ToString()) : Convert.ToDouble(0))
            //                );

            //        }
            //        if (Table.Rows.Count > 0)
            //        {
            //            _05_Gozareshat.Form01_ReportForm frm = new Form01_ReportForm
            //                (Table, 3, (mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString() == "0" ? "همه انبارها" : mlt_Ware.Text), "تا تاریخ: " +
            //                Date1);
            //            frm.ShowDialog();
            //        }*/
            //    }

            //}
            //else
            //{
            //    if (gridEX2.RowCount > 0)
            //    {
            //        try
            //        {
            //            gridEXPrintDocument1.GridEX = gridEX2;
            //            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
            //                if (printDialog1.ShowDialog() == DialogResult.OK)
            //                {
            //                    gridEXPrintDocument1.PageHeaderLeft = "گزارش موجودی منفی کالا";
            //                    gridEXPrintDocument1.PageHeaderRight = " تا تاریخ " + faDate2.Text + "***" + " انبار: " + mlt_Ware.Text;
            //                    gridEXPrintDocument1.PageFooterLeft =
            //          FarsiLibrary.Utils.PersianDate.Now.ToString("yyyy/mm/dd") +
            //          "**" + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
            //                    gridEXPrintDocument1.PageFooterRight =
            //                      " کاربر " + Class_BasicOperation._UserName;
            //                    printPreviewDialog1.ShowDialog();
            //                }
            //        }
            //        catch { }

            //        /*
            //        DataTable Table = dataSet_001_Gozareshat.Rpt_Maghta_Tedad.Clone();
            //        foreach (Janus.Windows.GridEX.GridEXRow item in gridEX2.GetRows())
            //        {
            //            Table.Rows.Add(item.Cells["GoodCode"].Text, item.Cells["GoodName"].Text,
            //                item.Cells["InBox"].Value.ToString(),
            //                item.Cells["InPack"].Value.ToString(),
            //                item.Cells["InDetail"].Value.ToString(),
            //                item.Cells["InTotal"].Value.ToString(),
            //                item.Cells["OutBox"].Value.ToString(),
            //                item.Cells["OutPack"].Value.ToString(),
            //                item.Cells["OutDetail"].Value.ToString(),
            //                item.Cells["OutTotal"].Value.ToString(),
            //                item.Cells["ReBox"].Value.ToString(),
            //                item.Cells["RePack"].Value.ToString(),
            //                item.Cells["ReDetail"].Value.ToString(),
            //                item.Cells["ReTotal"].Value.ToString(),
            //                item.Cells["Project"].Text,
            //                item.Cells["OTotalWeight"].Text,
            //                item.Cells["ITotalWeight"].Text,
            //                item.Cells["RTotalWeight"].Text);

            //        }
            //        if (Table.Rows.Count > 0)
            //        {
            //            _05_Gozareshat.Form01_ReportForm frm = new Form01_ReportForm
            //                (Table, 3, (mlt_Ware.DropDownList.GetCheckedRows()[0].Cells["ColumnId"].Value.ToString() == "0" ? "همه انبارها" : mlt_Ware.Text), "تا تاریخ: " +
            //                Date1);
            //            frm.ShowDialog();
            //        }*/
            //    }
            //}




        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            if (chk_ontatal.Checked)
                gridEXExporter1.GridEX = gridEX1;

            else

                gridEXExporter1.GridEX = gridEX2;
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                FileStream fs = ((FileStream)saveFileDialog1.OpenFile());
                gridEXExporter1.Export(fs);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void Frm_003_MojoodiMaghtai_NegativeDate_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX2.RemoveFilters();
            gridEX1.RemoveFilters();
        }

        private void Frm_003_MojoodiMaghtai_NegativeDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
                faDate1.Select();
        }
    }
}
