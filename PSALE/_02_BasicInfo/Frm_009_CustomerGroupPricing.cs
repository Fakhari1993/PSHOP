using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._02_BasicInfo
{
    public partial class Frm_009_CustomerGroupPricing : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        Classes.Class_Documents ClDoc = new Classes.Class_Documents();
        bool _Del = false;

        public Frm_009_CustomerGroupPricing(bool Del)
        {
            InitializeComponent();
            _Del = Del;
        }

        private void Frm_009_CustomerGroupPricing_Load(object sender, EventArgs e)
        {
            DataTable CustomerGroupTable = ClDoc.ReturnTable(ConBase.ConnectionString, "SELECT Column00,Column01 from Table_040_PersonGroups where Column08=1");
            gridEX1.DropDowns["Group"].SetDataBinding(CustomerGroupTable, "");
            gridEX2.DropDowns["Group"].SetDataBinding(CustomerGroupTable, "");

            DataTable GoodTable = ClDoc.ReturnTable(ConWare.ConnectionString, @"select table_004_CommodityAndIngredients.columnid,
                table_004_CommodityAndIngredients .column01,
                table_004_CommodityAndIngredients .column02,
                table_004_CommodityAndIngredients .column03,
                table_004_CommodityAndIngredients .column04,
                table_004_CommodityAndIngredients .column07,
                table_003_SubsidiaryGroup.column03 as SubName,
                table_002_MainGroup.column02 as MainName
                from table_004_CommodityAndIngredients inner join
                table_003_SubsidiaryGroup on table_004_CommodityAndIngredients.column03=
                table_003_SubsidiaryGroup.column01 and table_004_CommodityAndIngredients.column04=
                table_003_SubsidiaryGroup.columnid 
                inner join table_002_MainGroup on
                table_004_CommodityAndIngredients.column03=table_002_MainGroup.columnid");
            gridEX1.DropDowns["Good"].SetDataBinding(GoodTable, "");
            gridEX1.DropDowns["GoodCode"].SetDataBinding(GoodTable, "");
            gridEX2.DropDowns["Good"].SetDataBinding(GoodTable, "");

            this.table_029_CustomerGroupGoodPricingTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_029_CustomerGroupGoodPricing);

        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            if (this.table_029_CustomerGroupGoodPricingBindingSource.Count > 0)
            {
                try
                {
                    if (!_Del)
                        throw new Exception("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف سطر انتخاب شده هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        this.table_029_CustomerGroupGoodPricingBindingSource.RemoveCurrent();
                        this.table_029_CustomerGroupGoodPricingBindingSource.EndEdit();
                        this.table_029_CustomerGroupGoodPricingTableAdapter.Update(dataSet_EtelaatPaye.Table_029_CustomerGroupGoodPricing);
                        this.table_029_CustomerGroupGoodPricingTableAdapter.Fill(dataSet_EtelaatPaye.Table_029_CustomerGroupGoodPricing);
                        Class_BasicOperation.ShowMsg("", "حذف اطلاعات انجام شد", "Information");
                    }
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (this.table_029_CustomerGroupGoodPricingBindingSource.Count > 0)
            {
                try
                {
                    this.table_029_CustomerGroupGoodPricingBindingSource.EndEdit();
                    this.table_029_CustomerGroupGoodPricingTableAdapter.Update(dataSet_EtelaatPaye.Table_029_CustomerGroupGoodPricing);
                      if(sender==bt_Save || sender==this)
                    Class_BasicOperation.ShowMsg("", "ثبت اطلاعات با موفقیت انجام شد", "Information");
                }
                catch (Exception ex)
                {
                    Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                }

            }
        }

        private void mnu_Print_Click(object sender, EventArgs e)
        {
            if (table_029_CustomerGroupGoodPricingBindingSource.Count > 0)
            {
                bt_Save_Click(sender, e);
                DataTable Table = dataSet_EtelaatePaye_Report.Rpt_Pricing.Clone();
                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetRows())
                {
                    Table.Rows.Add(item.Cells["Column01"].Text, item.Cells["Column002"].Text,
                        item.Cells["Column03"].Value.ToString(), item.Cells["Column04"].Value.ToString(),
                        item.Cells["Column05"].Value.ToString(), item.Cells["Column06"].Value.ToString(), item.Cells["Column07"].Value.ToString(),
                        item.Cells["Column02"].Text);
                }
                _02_BasicInfo.Reports.ReportForm frm = new Reports.ReportForm(2, Table);
                frm.ShowDialog();

            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                System.IO.FileStream fs = ((System.IO.FileStream)saveFileDialog1.OpenFile());
                gridEXExporter1.Export(fs);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void Frm_009_CustomerGroupPricing_Activated(object sender, EventArgs e)
        {
            gridEX1.MoveToNewRecord();
        }

        private void Frm_009_CustomerGroupPricing_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                gridEX2.Select();
                gridEX2.Row = gridEX2.FilterRow.Position;
            }
            else if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.P)
                mnu_Print_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;
        }

     
    }
}
