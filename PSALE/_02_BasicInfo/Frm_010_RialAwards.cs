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
    public partial class Frm_010_RialAwards : Form
    {
        bool _del;
        bool _BackSpace = false;

        string _FromDate, _ToDate;
        decimal _FromPrice = 0, _ToPrice = 0;
        int _Kala = -1;
        double _QtyAward = 0;

        public Frm_010_RialAwards(bool del)
        {
            _del = del;
            InitializeComponent();
        }
        
        private void Frm_005_Javayez_Load(object sender, EventArgs e)
        {
            this.table_004_CommodityAndIngredientsTableAdapter.Fill(
                this.dataSet_05_Awards.table_004_CommodityAndIngredients);
            this.table_040_PersonGroupsTableAdapter.Fill(
                this.dataSet_05_Awards.Table_040_PersonGroups);
            this.table_040_RialAwardsTableAdapter.Fill(
                this.dataSet_05_Awards.Table_040_RialAwards);

            gridEX1.DropDowns[0].SetDataBinding(dataSet_05_Awards, "Table_040_PersonGroups");
            gridEX1.DropDowns[1].SetDataBinding(dataSet_05_Awards, "table_004_CommodityAndIngredients");

            _FromDate = FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##");
            _ToDate = FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##");

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();
                this.table_040_RialAwardsBindingSource.EndEdit();
                this.table_040_RialAwardsTableAdapter.Update(dataSet_05_Awards.Table_040_RialAwards);
                  if(sender==bt_Save || sender==this)
                Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد", "Information");
            }
            catch (System.Data.SqlClient.SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, this.Name);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;

            }
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            try
            {
                if (_del)
                //if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف اطلاعات هستید؟",
                //    "", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                //    MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {
                    this.table_040_RialAwardsBindingSource.RemoveCurrent();
                    //this.table_028_AwardBindingSource.EndEdit();
                    //this.table_028_AwardTableAdapter.Update(dataSet_EtelaatPaye.Table_028_Award);
                    //Class_BasicOperation.ShowMsg("", "حذف اطلاعات با موفقیت انجام گرفت", "Information");
                }
                else
                    MessageBox.Show("کاربر گرامی شما امکان حذف اطلاعات را ندارید");

            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
            }
        }

        private void Frm_005_Javayez_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.P)
                bt_Print_Click(sender, e);
        }

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void bt_ExportToExcel_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                System.IO.FileStream fs = ((System.IO.FileStream)saveFileDialog1.OpenFile());
                gridEXExporter1.Export(fs);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void gridEX1_CellValueChanged(object sender,
            Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;
        }

        private void gridEX1_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            if (gridEX1.Row == -1)
            {
                if (gridEX1.Col != gridEX1.RootTable.Columns["column02"].Index)
                    gridEX1.SetValue("column02", _FromDate);
                else
                    _FromDate = gridEX1.GetValue("column02").ToString();

                if (gridEX1.Col != gridEX1.RootTable.Columns["column03"].Index)
                    gridEX1.SetValue("column03", _ToDate);
                else
                    _ToDate = gridEX1.GetValue("column03").ToString();

                if (gridEX1.Col != gridEX1.RootTable.Columns["column04"].Index)
                    gridEX1.SetValue("column04", _FromPrice);
                else
                    _FromPrice = Convert.ToDecimal(gridEX1.GetValue("column04"));

                if (gridEX1.Col != gridEX1.RootTable.Columns["column05"].Index)
                    gridEX1.SetValue("column05", _ToPrice);
                else
                    _ToPrice = Convert.ToDecimal(gridEX1.GetValue("column05"));

                if (gridEX1.Col != gridEX1.RootTable.Columns["column06"].Index)
                    gridEX1.SetValue("column06", _Kala);
                else
                    _Kala = Convert.ToInt32(gridEX1.GetValue("column06"));

                if (gridEX1.Col != gridEX1.RootTable.Columns["column07"].Index)
                    gridEX1.SetValue("column07", _QtyAward);
                else
                    _QtyAward = Convert.ToDouble(gridEX1.GetValue("column07"));
            }

        }

        private void gridEX1_AddingRecord(object sender, CancelEventArgs e)
        {
            string _serverdatetime;
            _serverdatetime = Class_BasicOperation.ServerGetDate();
            gridEX1.SetValue("column08", Class_BasicOperation._UserName);
            gridEX1.SetValue("column10", Class_BasicOperation._UserName);
            gridEX1.SetValue("column09", _serverdatetime);
            gridEX1.SetValue("column11", _serverdatetime);
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            bt_Save_Click(sender, e);
            if (pageSetupDialog1.ShowDialog() == DialogResult.OK)
                printPreviewDialog1.ShowDialog();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (toolStripButton1.Text == "جستجو")
            {
                toolStripButton1.Text = "ورود اطلاعات جدید";
                gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.False;
                gridEX1.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;

                gridEX1.Select();
            }
            else
            {
                toolStripButton1.Text = "جستجو";
                gridEX1.AllowAddNew = Janus.Windows.GridEX.InheritableBoolean.True;
                gridEX1.FilterMode = Janus.Windows.GridEX.FilterMode.None;

                gridEX1.MoveToNewRecord();
            }
        }

     

        

    }
}
