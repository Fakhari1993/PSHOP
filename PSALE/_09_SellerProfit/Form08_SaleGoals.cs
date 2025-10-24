using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._09_SellerProfit
{
    public partial class Form08_SaleGoals : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);

        public Form08_SaleGoals()
        {
            InitializeComponent();
        }

        private void Form08_SaleGoals_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.Table_77_SaleGoal' table. You can move, or remove it, as needed.
            this.table_77_SaleGoalTableAdapter.Fill(this.dataSet1.Table_77_SaleGoal);
            gridEX1.DropDowns[0].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, "Select * from PeopleScope(8,3)"), "");
            gridEX1.DropDowns[2].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_75_SaleType"), "");
            gridEX1.DropDowns[1].SetDataBinding(clDoc.ReturnTable(ConBase.ConnectionString, @"DECLARE @t TABLE (ID INT  ,Titel NVARCHAR(50))

                                                                                                                    INSERT INTO @t
                                                                                                                    (
	                                                                                                                    ID,
	                                                                                                                    [Titel]
                                                                                                                    )
                                                                                                                    VALUES( 1,N'فروردين') ,
                                                                                                                    ( 2,N'ارديبهشت') ,
                                                                                                                    ( 3,N'خرداد') ,
                                                                                                                    ( 4,N'تير') ,
                                                                                                                    ( 5,N'مرداد') ,
                                                                                                                    ( 6,N'شهريور') ,
                                                                                                                    ( 7,N'مهر') ,
                                                                                                                    ( 8,N'آبان') ,
                                                                                                                    ( 9,N'آذر') ,
                                                                                                                    ( 10,N'دي') ,
                                                                                                                    ( 11,N'بهمن') ,
                                                                                                                    ( 12,N'اسفند')  
                                                                                                                    SELECT * FROM @t t"), "");
            gridEX1.Select();
            gridEX1.Col = 1;

        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void gridEX1_AddingRecord(object sender, CancelEventArgs e)
        {
            gridEX1.SetValue("Column04", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column05", Class_BasicOperation.ServerDate());
            gridEX1.SetValue("Column06", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column07", Class_BasicOperation.ServerDate());
        }

        private void gridEX1_CellUpdated(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                Class_BasicOperation.GridExDropDownRemoveFilter(sender, "Column00");
            }
            catch { }
        }

        private void gridEX1_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            gridEX1.CurrentCellDroppedDown = true;

            try
            {
                if (e.Column.Key == "Column00")
                    Class_BasicOperation.FilterGridExDropDown(sender, "Column00", "Column01", "Column02", gridEX1.EditTextBox.Text, Class_BasicOperation.FilterColumnType.ACCColumn);
            }
            catch { }
            gridEX1.SetValue("Column06", Class_BasicOperation._UserName);
            gridEX1.SetValue("Column07", Class_BasicOperation.ServerDate());
        }

        private void gridEX1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                gridEX1.UpdateData();
                table_77_SaleGoalBindingSource.EndEdit();
                this.table_77_SaleGoalTableAdapter.Update(this.dataSet1.Table_77_SaleGoal);


                Class_BasicOperation.ShowMsg("", " ثبت با موفقیت انجام شد", "Information");

            }
            catch (SqlException es)
            {
                Class_BasicOperation.CheckSqlExp(es, this.Name);
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            gridEXExporter1.GridEX = gridEX1;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.FileStream File = (System.IO.FileStream)saveFileDialog1.OpenFile();
                gridEXExporter1.Export(File);
                Class_BasicOperation.ShowMsg("", "عملیات ارسال با موفقیت انجام گرفت", "Information");
            }
        }

        private void Form08_SaleGoals_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
        }

        private void gridEX1_DeletingRecord(object sender, Janus.Windows.GridEX.RowActionCancelEventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (!UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 232))
            {
                e.Cancel = true;
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف اطلاعات را ندارید", "Stop");
                return;
            }
        }
    }
}
