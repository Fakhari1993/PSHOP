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
    public partial class Form008_PersonContact : Form
    {
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        public Form008_PersonContact()
        {
            InitializeComponent();
        }

        private void Form008_PersonContact_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_045_PersonScope' table. You can move, or remove it, as needed.
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_040_PersonGroups' table. You can move, or remove it, as needed.
            this.table_040_PersonGroupsTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_040_PersonGroups);

            this.table_060_ProvinceInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_060_ProvinceInfo);
            this.table_065_CityInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_065_CityInfo);
            this.table_160_StatesTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_160_States);

            gridEX1.DropDowns["Province"].DataSource = table_060_ProvinceInfoBindingSource;
            gridEX1.DropDowns["City"].DataSource = table_065_CityInfoBindingSource;
            gridEX1.DropDowns["State"].DataSource = table_160_StatesBindingSource;

            SqlDataAdapter Adapter = new SqlDataAdapter("Select Column00,Column01 from Table_040_PersonGroups", ConBase);
            DataTable Group = new DataTable();
            Adapter.Fill(Group);
            gridEX1.DropDowns["Member"].SetDataBinding(Group, "");
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_045_PersonInfoContacts' table. You can move, or remove it, as needed.
            this.table_045_PersonInfoTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_045_PersonInfo);
            this.table_045_PersonScopeTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_045_PersonScope);
            
            // TODO: This line of code loads data into the 'dataSet_EtelaatPaye.Table_045_PersonInfoContacts' table. You can move, or remove it, as needed.
            this.table_045_PersonInfoContactsTableAdapter.Fill(this.dataSet_EtelaatPaye.Table_045_PersonInfoContacts);

        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                gridEX2.UpdateData();
               
                this.table_045_PersonInfoContactsBindingSource.EndEdit();
                this.table_045_PersonInfoContactsTableAdapter.Update(this.dataSet_EtelaatPaye.Table_045_PersonInfoContacts);

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


            this.Cursor = Cursors.Default;
        }

        private void gridEX2_AddingRecord(object sender, CancelEventArgs e)
        {
            try
            {
                gridEX2.SetValue("Column07", Class_BasicOperation._UserName);
                gridEX2.SetValue("Column08", Class_BasicOperation.ServerDate());
                gridEX2.SetValue("Column09", Class_BasicOperation._UserName);
                gridEX2.SetValue("Column10", Class_BasicOperation.ServerDate());
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX2_CellValueChanged(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                gridEX2.SetValue("Column09", Class_BasicOperation._UserName);
                gridEX2.SetValue("Column10", Class_BasicOperation.ServerDate());
            }
            catch (Exception ex)
            {
                Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;
            }
        }

        private void gridEX2_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void Form008_PersonContact_FormClosing(object sender, FormClosingEventArgs e)
        {
            gridEX1.RemoveFilters();

        }

        private void Form008_PersonContact_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                bt_Save_Click(sender, e);
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }
    }
}
