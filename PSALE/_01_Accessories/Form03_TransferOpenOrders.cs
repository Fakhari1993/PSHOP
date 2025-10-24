using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PSHOP._01_Accessories
{
    public partial class Form03_TransferOpenOrders : Form
    {
        SqlConnection ConSale = new SqlConnection(Properties.Settings.Default.SALE);
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        Classes.Class_GoodInformation clGood = new Classes.Class_GoodInformation();

        public Form03_TransferOpenOrders()
        {
            InitializeComponent();
        }

        private void Form03_TransferOpenOrders_Load(object sender, EventArgs e)
        {
            DataSet DataSet1 = new DataSet();
            DataTable HeaderTable = clDoc.ReturnTable(ConSale.ConnectionString, "Select * from Table_005_OrderHeader where Column13=0 and Column33=0");
            DataTable DetailTable = clDoc.ReturnTable(ConSale.ConnectionString, @"Select * from Table_006_OrderDetails where Column01 IN (Select ColumnId from 
                    Table_005_OrderHeader where Column13=0 and Column33=0)");
            DataSet1.Tables.Add(HeaderTable);
            DataSet1.Tables[0].TableName = "Header";
            DataSet1.Tables.Add(DetailTable);
            DataSet1.Tables[1].TableName = "Detail";

            DataRelation Relation1 = new DataRelation("R_Header_Detail", DataSet1.Tables["Header"].Columns["ColumnId"], DataSet1.Tables["Detail"].Columns["Column01"]);

            ForeignKeyConstraint Fkc1 = new ForeignKeyConstraint("F_Header_Detail", DataSet1.Tables["Header"].Columns["ColumnId"], DataSet1.Tables["Detail"].Columns["Column01"]);
            Fkc1.UpdateRule = Rule.Cascade;
            Fkc1.AcceptRejectRule = AcceptRejectRule.None;
            Fkc1.DeleteRule = Rule.None;

            DataSet1.Tables["Detail"].Constraints.Add(Fkc1);
            DataSet1.Relations.Add(Relation1);

            gridEX1.DataSource = DataSet1.Tables["Header"];
            gridEX2.DataSource = DataSet1.Tables["Header"];
            gridEX2.DataMember = "R_Header_Detail";

            
            gridEX1.DropDowns["Customer"].DataSource=clDoc.ReturnTable(ConBase.ConnectionString, "Select ColumnId,Column01,Column02,Column29 from Table_045_PersonInfo");
            gridEX1.DropDowns["City"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "Select Column00,Column01,Column02 from Table_065_CityInfo");
            gridEX1.DropDowns["Vehicle"].DataSource=clDoc.ReturnTable(ConBase.ConnectionString,"Select Column00,Column01 from Table_115_VehicleType");
            gridEX1.DropDowns["SaleType"].DataSource = clDoc.ReturnTable(ConBase.ConnectionString, "SELECT * FROM Table_002_SalesTypes");
           


            gridEX2.DropDowns["Good"].SetDataBinding(clGood.GoodInfo(), "");
            gridEX2.DropDowns[1].SetDataBinding(clGood.GoodInfo(), "");

            gridEX1.DropDowns["Prefactor"].SetDataBinding(clDoc.ReturnTable(ConSale.ConnectionString,
                "Select ColumnId,Column01 from Table_007_FactorBefore"), "");

            cmb_year.ComboBox.DataSource = clDoc.ReturnTable(ConSale.ConnectionString, "Select name from sys.databases where name<>'" + ConSale.Database + "' and name like 'PSALE%'");
            cmb_year.ComboBox.DisplayMember = "Name";
            cmb_year.ComboBox.ValueMember = "Name";

        }

        private void bt_Display_Click(object sender, EventArgs e)
        {
            gridEX1.RemoveFilters();
            if (gridEX1.GetCheckedRows().Length > 0 && cmb_year.Text.Trim() != "")
            {
                if (DialogResult.Yes == MessageBox.Show("در صورت انتقال سفارشات با شماره جاری به سال مالی انتخاب شده انتقال خواهند یافت"+Environment.NewLine+"آیا مایل به ادامه هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                {
                    List<int> IDs = new List<int>();
                  
                    foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetCheckedRows())
                    {
                        IDs.Add(int.Parse(item.Cells["ColumnId"].Value.ToString()));
                    }

                    bool _insert = false;
                    for (int i = 0; i < IDs.Count; i++)
                    {
                        using (SqlConnection Con = new SqlConnection(ConSale.ConnectionString.Replace(ConSale.Database, cmb_year.Text)))
                        {
                            Con.Open();
                            SqlCommand InsertHeader = new SqlCommand(@"INSERT INTO Table_005_OrderHeader 
                            select (SELECT ISNULL((Select MAX(Column01)+1  from " + Con.Database + @".dbo.Table_005_OrderHeader),1))
                            ,[column02],[column03],[column04],[column05]
                            ,[column06],[column07],[column08],[column09],[column10]
                            ,[column11],[column12],[column13],[column14],[column15],[column16]
                            ,[column17],[column18],[column19],[column20],[column21],[column22]
                            ,[column23],[column24],[column25],[column26],[column27],[column28]
                            ,[column29],[column30],[column31],[column32],[column33],[column34]
                            ,[column35],[column36],[column37],[column38] 
                            from " + ConSale.Database + ".dbo.Table_005_OrderHeader where columnid=" + IDs[i].ToString()+"; SET @Key=SCOPE_IDENTITY()", Con);
                            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                            Key.Direction = ParameterDirection.Output;
                            InsertHeader.Parameters.Add(Key);
                            InsertHeader.ExecuteNonQuery();
                            int HeaderId = int.Parse(Key.Value.ToString());

                            SqlCommand InsertDetail = new SqlCommand(@"INSERT INTO Table_006_OrderDetails (
                            [column01],[column02],[column03],[column04],[column05],[column06],[column07]
                            ,[column08],[column09],[column10],[column11],[column12],[column13],[column14]
                            ,[column15],[column16],[column17],[column19],[column20]
                            ,[column21],[column22],[column23],[column24],[column25],[column26],[column27]
                            ,[column28],[column29],[column30],[column31],[column32])
                            SELECT "+HeaderId+@",[column02],[column03],[column04],[column05],[column06],[column07]
                            ,[column08],[column09],[column10],[column11],[column12],[column13],[column14]
                            ,[column15],[column16],[column17],[column19],[column20]
                            ,[column21],[column22],[column23],[column24],[column25],[column26],[column27]
                            ,[column28],[column29],[column30],[column31],[column32]
                            FROM "+ConSale.Database+".[dbo].[Table_006_OrderDetails] where column01=" + IDs[i].ToString(), Con);
                            InsertDetail.ExecuteNonQuery();
                            _insert = true;
                        }
                    }
                    if (_insert)
                        Class_BasicOperation.ShowMsg("", "انتقال سفارشات با موفقیت صورت گرفت", "Information");
                }
            }
            else Class_BasicOperation.ShowMsg("", "اطلاعات مورد نیاز را تکمیل کنید", "Warning");
        }
    }
}
