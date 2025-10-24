using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._06_Reports._03_Buy
{

    public partial class Form46_RequestBuy : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        SqlConnection ConWare = new SqlConnection(Properties.Settings.Default.WHRS);
        Class_UserScope UserScope = new Class_UserScope();
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_GoodInformation ClGood = new Classes.Class_GoodInformation();
        Janus.Windows.GridEX.GridEX gh;
        public Form46_RequestBuy(Janus.Windows.GridEX.GridEX _k)
        {
            InitializeComponent();
            gh = _k;
        }

        private void maskedEditBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13 || !char.IsControl(e.KeyChar))
                mlt_Person.DroppedDown = true;
            else Class_BasicOperation.isEnter(e.KeyChar);
        }

        private void Form46_RequestBuy_Load(object sender, EventArgs e)
        {
            //*************Fill MultiColumns
            DataTable PersonTable = clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02 from Table_045_PersonInfo");
            mlt_Person.DataSource = PersonTable;
            txt_date.Text = FarsiLibrary.Utils.PersianDate.Now.ToString("####/##/##");
        }

        private void maskedEditBox1_KeyPress_1(object sender, KeyPressEventArgs e)
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

        private void uiButton1_Click(object sender, EventArgs e)
        {
            try
            {
                string command = string.Empty;

                if (!string.IsNullOrWhiteSpace(txt_date.Text) && mlt_Person.Value != null &&
                    !string.IsNullOrWhiteSpace(mlt_Person.Value.ToString()) &&
                    Convert.ToDouble(gh.GetTotalRow().Cells["forecastvalue"].Value.ToString()) > 0)
                {
                    this.Cursor = Cursors.WaitCursor;

                    SqlParameter num;
                    num = new SqlParameter("num", SqlDbType.Int);
                    num.Direction = ParameterDirection.Output;

                    command = @" declare @Key int  
                        set @num=(SELECT ISNULL(Max( Column01),0)+1  from [dbo].[Table_023_RequestBuy]) 
                                    INSERT INTO  [dbo].[Table_023_RequestBuy]
                                           ([Column01]
                                           ,[Column02]
                                           ,[Column03]
                                           ,[Column04]
                                           ,[Column05]
                                           ,[Column06]
                                           ,[Column07]
                                           ,[Column08]
                                           ,[Column09]
                                           ,[Column10]
                                           ,[Column11]
                                           ,[Column12])
                                     VALUES
                                           (@num
                                           ,'" + txt_date.Text + @"'
                                           ," + mlt_Person.Value + @"
                                           ,N'درخواست خرید صادر شده از گزارش پیش بینی خرید'
                                           ,'" + Class_BasicOperation._UserName + @"'
                                           ,getdate()
                                           ,'" + Class_BasicOperation._UserName + @"'
                                           ,getdate()
                                           ,NULL
                                           ,NULL
                                           ,NULL
                                           ,NULL); SET @Key=SCOPE_IDENTITY();";


                    foreach (Janus.Windows.GridEX.GridEXRow item in gh.GetCheckedRows())
                    {
                        if (Convert.ToDecimal(item.Cells["forecastvalue"].Value) > 0)

                            command += @" INSERT INTO  [dbo].[Table_024_Child_RequestBuy]
                                                   ([Column01]
                                                   ,[Column02]
                                                   ,[Column03]
                                                   ,[Column04]
                                                   ,[Column05]
                                                   ,[Column06]
                                                   ,[Column07]
                                                   ,[Column08]
                                                   ,[Column09]
                                                   ,[Column10]
                                                   ,[Column11]
                                                   ,[Column12]
                                                   ,[Column13]
                                                   ,[Column14]
                                                   ,[Column15]
                                                   ,[Column16]
                                                   ,[Column17]
                                                   ,[Column18]
                                                   ,[Column19]
                                                   ,[Column20]
                                                   ,[Column21]
                                                   ,[Column22]
                                                   ,[Column23]
                                                   ,[Column24]
                                                   ,[Column25]
                                                   ,[Column26]
                                                   ,[Column27]
                                                   ,[Column28]
                                                   ,[Column29]
                                                   ,[Column30]
                                                   ,[Column31]
                                                   ,[Column32]
                                                   ,[Column33]
                                                   ,[Column34])
                                             VALUES
                                                   (@Key
                                                   ," + item.Cells["id"].Value + @"
                                                   ,(select column07 from table_004_CommodityAndIngredients where columnid=" + item.Cells["id"].Value + @")
                                                   ," + item.Cells["forecastvalue"].Value + @"
                                                   ,NULL
                                                   ,NULL
                                                   ,NULL
                                                   ,0
                                                   ,0
                                                   ,NULL
                                                   ,NULL
                                                   ,'" + Class_BasicOperation._UserName + @"'
                                                   ,getdate()
                                                   ,'" + Class_BasicOperation._UserName + @"'
                                                   ,getdate()
                                                   ,NULL
                                                   ,NULL
                                                   ,NULL
                                                   ,0
                                                   ,NULL
                                                   ,0
                                                   ,0
                                                   ," + item.Cells["forecastvalue"].Value + @"
                                                   ,0
                                                   ,0
                                                   ,0
                                                   ,0
                                                   ,NULL
                                                   ,(select column09 from table_004_CommodityAndIngredients where columnid=" + item.Cells["id"].Value + @")
                                                   ,(select column08 from table_004_CommodityAndIngredients where columnid=" + item.Cells["id"].Value + @")
                                                   ,NULL
                                                   ,NULL
                                                   ,NULL
                                                   ,NULL)";



                    }

                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.WHRS))
                    {
                        Con.Open();

                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;

                        try
                        {
                            Command.CommandText = command;
                            Command.Parameters.Add(num);
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();
                            Class_BasicOperation.ShowMsg("", "درخواست خرید با شماره " + num.Value + " با موفقیت ثبت گردید", "Information");
                            this.DialogResult = DialogResult.Yes;
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
                else
                {
                    MessageBox.Show("درخواست صادر نمیشود");

                }
            }

            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name);
            }
            this.Cursor = Cursors.Default;

        }
    }
}
