using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevComponents.AdvTree;
using System.Data.SqlClient;
using System.Collections;
using Janus.Windows.GridEX;
namespace PSHOP._02_BasicInfo
{
    public partial class Frm_030_SaleManChart : Form
    {
        Classes.Class_Documents clDoc = new Classes.Class_Documents();
        SqlConnection ConBase = new SqlConnection(Properties.Settings.Default.BASE);
        string commad = string.Empty;

        public Frm_030_SaleManChart()
        {
            InitializeComponent();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {

            if (mCC_Person.Value != null && !string.IsNullOrWhiteSpace(mCC_Person.Value.ToString()))
            {
                if (Convert.ToInt32(lblID.Text) <= 0)
                {
                    ArrayList myAL = new ArrayList();
                    myAL.Add(mCC_Person.Value);
                    myAL.Add(txt_DarsadHamkariAsli.Value);
                    myAL.Add(txt_DarsadMajmooe.Value);
                    myAL.Add(txt_DarsadMostaghim.Value);
                    myAL.Add(txt_SatheHamkari.Value);
                    Node df = new Node();
                    df.Text = mCC_Person.Text;
                    df.Tag = myAL;
                    string commad = string.Empty;
                    object op = advTree5.SelectedValue;
                    if (advTree5.SelectedNode != null)
                    {
                        try
                        {
                            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                            Key.Direction = ParameterDirection.Output;
                            commad = @"INSERT INTO  [dbo].[Table_045_SaleManChart]
                                               ([Column00]
                                               ,[Column01]
                                               ,[Column02]
                                               ,[Column03]
                                               ,[Column04]
                                               ,[Column05]
                                               ,[Column07]
                                               ,[Column08]
                                               ,[Column09]
                                               ,[Column10]
                                               ,[Column11])
                                         VALUES
                                               (null
                                               ," + mCC_Person.Value + @"
                                               ," + advTree5.SelectedNode.DataKey + @"
                                               ," + txt_DarsadHamkariAsli.Value + @"
                                               ," + txt_DarsadMostaghim.Value + @"
                                               ," + txt_SatheHamkari.Value + @"
                                               ," + txt_DarsadMajmooe.Value + @"
                                               ,'" + Class_BasicOperation._UserName + @"'
                                               ,getdate() 
                                               ,'" + Class_BasicOperation._UserName + @"'
                                               ,getdate() ); SET @Key=SCOPE_IDENTITY()";

                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
                            {
                                Con.Open();
                                SqlCommand InsertHeader = new SqlCommand(commad, Con);
                                InsertHeader.Parameters.Add(Key);
                                InsertHeader.ExecuteNonQuery();
                            }
                            df.DataKey = Key.Value;
                            advTree5.SelectedNode.Nodes.Add(df);
                            advTree5.SelectedNode.Expand();
                        }
                        catch (System.Data.SqlClient.SqlException es)
                        {
                            Class_BasicOperation.CheckSqlExp(es, this.Name);
                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;

                        }


                    }
                    else
                    {
                        try
                        {

                            SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                            Key.Direction = ParameterDirection.Output;
                            commad = @"INSERT INTO  [dbo].[Table_045_SaleManChart]
                                               ([Column00]
                                               ,[Column01]
                                               ,[Column02]
                                               ,[Column03]
                                               ,[Column04]
                                               ,[Column05]
                                               ,[Column07]
                                               ,[Column08]
                                               ,[Column09]
                                               ,[Column10]
                                               ,[Column11])
                                         VALUES
                                               (null
                                               ," + mCC_Person.Value + @"
                                               ,null
                                               ," + txt_DarsadHamkariAsli.Value + @"
                                               ," + txt_DarsadMostaghim.Value + @"
                                               ," + txt_SatheHamkari.Value + @"
                                               ," + txt_DarsadMajmooe.Value + @"
                                               ,'" + Class_BasicOperation._UserName + @"'
                                               ,getdate()
                                               ,'" + Class_BasicOperation._UserName + @"'
                                               ,getdate()); SET @Key=SCOPE_IDENTITY()";

                            using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
                            {
                                Con.Open();
                                SqlCommand InsertHeader = new SqlCommand(commad, Con);
                                InsertHeader.Parameters.Add(Key);


                                InsertHeader.ExecuteNonQuery();
                            }
                            df.DataKey = Key.Value;
                            advTree5.Nodes.Add(df);
                            df.Expand();

                        }
                        catch (System.Data.SqlClient.SqlException es)
                        {
                            Class_BasicOperation.CheckSqlExp(es, this.Name);
                        }
                        catch (Exception ex)
                        {
                            Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;

                        }

                    }
                    cleare();

                }
                else//EDit
                {
                    try
                    {

                        SqlParameter Key = new SqlParameter("Key", SqlDbType.Int);
                        Key.Direction = ParameterDirection.Output;
                        string commad = @"Update  [dbo].[Table_045_SaleManChart]
                                            Set     
                                                [Column01]=" + mCC_Person.Value + @"
                                                
                                               ,[Column03]=" + txt_DarsadHamkariAsli.Value + @"
                                               ,[Column04]=" + txt_DarsadMostaghim.Value + @"
                                               ,[Column05]=" + txt_SatheHamkari.Value + @"
                                               ,[Column07]=" + txt_DarsadMajmooe.Value + @"
                                               
                                               ,[Column10]='" + Class_BasicOperation._UserName + @"'
                                               ,[Column11] = getdate() WHere ColumnId= " + lblID.Text;


                        using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
                        {
                            Con.Open();
                            SqlCommand InsertHeader = new SqlCommand(commad, Con);
                            InsertHeader.ExecuteNonQuery();
                        }

                    }
                    catch (System.Data.SqlClient.SqlException es)
                    {
                        Class_BasicOperation.CheckSqlExp(es, this.Name);
                    }
                    catch (Exception ex)
                    {
                        Class_BasicOperation.CheckExceptionType(ex, this.Name); this.Cursor = Cursors.Default;

                    }

                    createTree();


                }



            }
        }


        public void DeleteNodesRecursive(Node oParentNode)
        {

            commad += @"  Delete from  [dbo].[Table_045_SaleManChart] Where ColumnId=" + oParentNode.DataKey;
            commad += "   Delete from  [dbo].[Table_045_SaleManChart] where  Column02=" + oParentNode.DataKey;

            foreach (Node oSubNode in oParentNode.Nodes)
            {
                DeleteNodesRecursive(oSubNode);
            }
        }
        private void uiButton2_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName, "Column11", 199))
            {
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    commad = string.Empty;
                    DeleteNodesRecursive(advTree5.SelectedNode);
                    using (SqlConnection Con = new SqlConnection(Properties.Settings.Default.BASE))
                    {
                        Con.Open();
                        SqlTransaction sqlTran = Con.BeginTransaction();
                        SqlCommand Command = Con.CreateCommand();
                        Command.Transaction = sqlTran;
                        try
                        {
                            Command.CommandText = commad;
                            Command.ExecuteNonQuery();
                            sqlTran.Commit();
                            createTree();
                            cleare();
                        }
                        catch (Exception es)
                        {
                            sqlTran.Rollback();
                            this.Cursor = Cursors.Default;
                            Class_BasicOperation.CheckExceptionType(es, this.Name);
                        }
                    }
                  
                }
                catch
                {
                }
                this.Cursor = Cursors.Default;
            }
            else
                Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف  را ندارید", "None");


        }
        private void createTree()
        {

            this.Cursor = Cursors.WaitCursor;
            advTree5.Nodes.Clear();
            advTree5.Refresh();
            DataTable chart = clDoc.ReturnTable
             (ConBase.ConnectionString, @"SELECT k.*,p.Column02 as Name FROM Table_045_SaleManChart k join Table_045_PersonInfo p on p.ColumnId=k.Column01  ");
            foreach (DataRow sr in chart.Rows)
            {
                if (sr["Column02"] != null && !string.IsNullOrWhiteSpace(sr["Column02"].ToString()))
                {
                    advTree5.SelectedNode = advTree5.FindNodeByDataKey(sr["Column02"]);
                    ArrayList myAL = new ArrayList();
                    myAL.Add(sr["Column01"]);
                    myAL.Add(sr["Column03"]);
                    myAL.Add(sr["Column07"]);
                    myAL.Add(sr["Column04"]);
                    myAL.Add(sr["Column05"]);
                    Node df = new Node();
                    df.Text = sr["Name"].ToString();
                    df.DataKey = sr["ColumnId"];

                    df.Tag = myAL;
                    advTree5.SelectedNode.Nodes.Add(df);

                }
                else
                {
                    ArrayList myAL = new ArrayList();
                    myAL.Add(sr["Column01"]);
                    myAL.Add(sr["Column03"]);
                    myAL.Add(sr["Column07"]);
                    myAL.Add(sr["Column04"]);
                    myAL.Add(sr["Column05"]);
                    Node df = new Node();
                    df.Text = sr["Name"].ToString();
                    df.DataKey = sr["ColumnId"];
                    df.Tag = myAL;
                    advTree5.Nodes.Add(df);
                }
            }
            cleare();

            this.Cursor = Cursors.Default;
        }
        private void Frm_030_SaleManChart_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet_05_Awards.Table_045_SaleManChart' table. You can move, or remove it, as needed.
            this.table_045_SaleManChartTableAdapter.Fill(this.dataSet_05_Awards.Table_045_SaleManChart);
            DataTable CustomerTable = clDoc.ReturnTable
              (ConBase.ConnectionString, @"SELECT
	                                    tpi.ColumnId,
 
	                                    tpi.Column01,
	                                    tpi.Column02 
	                                     
                                    FROM
	                                    Table_045_PersonInfo tpi    where tpi.Column12=1");
            mCC_Person.DataSource = CustomerTable;
            mCC_Person.SelectAll();

            createTree();

        }

        private void mCC_Person_KeyUp(object sender, KeyEventArgs e)
        {
            Class_BasicOperation.FilterMultiColumns(sender, "Column02", "Column01");

        }

        private void mCC_Person_Leave(object sender, EventArgs e)
        {
            Class_BasicOperation.MultiColumnsRemoveFilter(sender);

        }

        private void mCC_Person_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender is Janus.Windows.GridEX.EditControls.MultiColumnCombo)
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
                else if (!char.IsControl(e.KeyChar))
                    ((Janus.Windows.GridEX.EditControls.MultiColumnCombo)sender).DroppedDown = true;
            }
            else
            {
                if (e.KeyChar == 13)
                    Class_BasicOperation.isEnter(e.KeyChar);
            }
        }

        private void cleare()
        {
            advTree5.SelectedNode = null;
            mCC_Person.Value = null;
            txt_DarsadHamkariAsli.Value = 0;
            txt_DarsadMajmooe.Value = 0;
            txt_DarsadMostaghim.Value = 0;
            txt_SatheHamkari.Value = 0;
            mCC_Person.Focus();
            mCC_Person.SelectAll();
            lblID.Text = "-1";

        }

        private void Frm_030_SaleManChart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Control)
                uiButton1_Click(sender, e);
            else if (e.KeyCode == Keys.E && e.Control)
                btn_Edit_Click(sender, e);
            else if (e.KeyCode == Keys.D && e.Control)
                uiButton2_Click(sender, e);
            else if (e.KeyCode == Keys.N && e.Control)
                uiButton1_Click_1(sender, e);




        }

        private void advTree5_AfterNodeSelect(object sender, AdvTreeNodeEventArgs e)
        {

            try
            {
                Node H = advTree5.SelectedNode;

                mCC_Person.Value = ((ArrayList)H.Tag)[0];
                txt_DarsadHamkariAsli.Value = ((ArrayList)H.Tag)[1];
                txt_DarsadMajmooe.Value = ((ArrayList)H.Tag)[2];
                txt_DarsadMostaghim.Value = ((ArrayList)H.Tag)[3];
                txt_SatheHamkari.Value = ((ArrayList)H.Tag)[4];
                lblID.Text = H.DataKey.ToString();
            }
            catch
            {
            }


        }

        private void uiButton1_Click_1(object sender, EventArgs e)
        {
            mCC_Person.Value = null;
            txt_DarsadHamkariAsli.Value = 0;
            txt_DarsadMajmooe.Value = 0;
            txt_DarsadMostaghim.Value = 0;
            txt_SatheHamkari.Value = 0;
            mCC_Person.Focus();
            mCC_Person.SelectAll();
            lblID.Text = "-1";
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                Node H = advTree5.SelectedNode;

                mCC_Person.Value = ((ArrayList)H.Tag)[0];
                txt_DarsadHamkariAsli.Value = ((ArrayList)H.Tag)[1];
                txt_DarsadMajmooe.Value = ((ArrayList)H.Tag)[2];
                txt_DarsadMostaghim.Value = ((ArrayList)H.Tag)[3];
                txt_SatheHamkari.Value = ((ArrayList)H.Tag)[4];
                lblID.Text = H.DataKey.ToString();
            }
            catch
            {
            }
        }

        
    }
}
