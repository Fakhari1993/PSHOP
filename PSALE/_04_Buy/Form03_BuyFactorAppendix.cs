using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;


namespace PSHOP._04_Buy
{
    public partial class Form03_BuyFactorAppendix : Form
    {
        int _HeaderId,_HeaderNum;

        public Form03_BuyFactorAppendix(int HeaderId, int HeaderNum)
        {
            InitializeComponent();
            _HeaderId = HeaderId;
            _HeaderNum = HeaderNum;
            Classes.Class_Documents clDoc = new Classes.Class_Documents();
            toolStripLabel1.Text +=_HeaderNum.ToString();
            gridEX1.RootTable.Columns["Column00"].DefaultValue = _HeaderId;
        }

        private void Form09_Appendix_Load(object sender, EventArgs e)
        {
            this.table_69_BuyFactorAttachmentsTableAdapter.FillByheader(this.dataSet_Buy.Table_69_BuyFactorAttachments, _HeaderId);



        }

        private void gridEX1_ColumnButtonClick(object sender, Janus.Windows.GridEX.ColumnActionEventArgs e)
        {
            try
            {
                if (gridEX1.Row == -1)
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(openFileDialog1.FileName, @"^[\P{L}A-Za-z]*$"))
                        {
                            foreach (string item in openFileDialog1.FileNames)
                            {
                                Janus.Windows.GridEX.GridEXRow Row = gridEX1.GetRow();
                                Row.BeginEdit();
                                Row.Cells["Column00"].Value = _HeaderId;
                                Row.Cells["Column01"].Value = "ضمیمه";
                                Row.Cells["Column02"].Value = item;
                                Row.EndEdit();

                            }
                            gridEX1.UpdateData();
                        }
                        else Class_BasicOperation.ShowMsg("خطا در درج مسیر", "نامگذاری مسیر بر اساس حروف انگلیسی نمی باشد", "Warning");

                    }
                }
                else
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        if (System.Text.RegularExpressions.Regex.IsMatch(openFileDialog1.FileName, @"^[\P{L}A-Za-z]*$"))
                        {
                            gridEX1.SetValue("Column02", openFileDialog1.FileName);
                            gridEX1.UpdateData();
                            gridEX1_CurrentCellChanged(sender, e);
                        }
                        else Class_BasicOperation.ShowMsg("خطا در ویرایش مسیر", "نامگذاری مسیر بر اساس حروف انگلیسی نمی باشد", "Warning");
                    }
                }
            }
            catch
            {
            }
        }

        private void bt_Save_Click(object sender, EventArgs e)
        {
            if (this.table_69_BuyFactorAttachmentsBindingSource.Count > 0)
            {
                try
                {
                    this.Validate();
                    this.table_69_BuyFactorAttachmentsBindingSource.EndEdit();
                    this.table_69_BuyFactorAttachmentsTableAdapter.Update(dataSet_Buy.Table_69_BuyFactorAttachments);

                    Class_BasicOperation.ShowMsg("", "اطلاعات ذخیره شد","Information");
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
        }

        private void bt_Del_Click(object sender, EventArgs e)
        {
            Class_UserScope UserScope = new Class_UserScope();
            if (UserScope.CheckScope(Class_BasicOperation._UserName,"Column10",96))
            {
                if (this.table_69_BuyFactorAttachmentsBindingSource.Count > 0)

                {
                    if (DialogResult.Yes == MessageBox.Show("آیا مایل به حذف پیوست(ها) هستید؟", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign))
                    {
                        try
                        {
                            if (gridEX1.GetCheckedRows().Length == 0)
                            {
                                this.table_69_BuyFactorAttachmentsBindingSource.RemoveCurrent();
                                this.table_69_BuyFactorAttachmentsBindingSource.EndEdit();
                                this.table_69_BuyFactorAttachmentsTableAdapter.Update(dataSet_Buy.Table_69_BuyFactorAttachments);



                                Class_BasicOperation.ShowMsg("", "حذف پیوست(ها) انجام شد", "Information");
                            }
                            else
                            {
                                foreach (Janus.Windows.GridEX.GridEXRow item in gridEX1.GetCheckedRows())
                                {
                                    item.Delete();
                                }
                                this.table_69_BuyFactorAttachmentsBindingSource.EndEdit();
                                this.table_69_BuyFactorAttachmentsTableAdapter.Update(dataSet_Buy.Table_69_BuyFactorAttachments);


                                Class_BasicOperation.ShowMsg("", "حذف پیوست(ها) انجام شد", "Information");
                            }
                        }
                        catch (Exception ex)
                        {
                            this.table_69_BuyFactorAttachmentsTableAdapter.FillByheader(this.dataSet_Buy.Table_69_BuyFactorAttachments, _HeaderId);



                            Class_BasicOperation.CheckExceptionType(ex, this.Name);                              this.Cursor = Cursors.Default;
                        }
                    }
                }
            }
            else Class_BasicOperation.ShowMsg("", "کاربر گرامی شما امکان حذف پیوست را ندارید","Warning");
        }

        private void gridEX1_CurrentCellChanged(object sender, EventArgs e)
        {
            try
            {
                pictureBox1.Image = Image.FromFile(gridEX1.GetValue("Column02").ToString());
                toolTip1.SetToolTip(pictureBox1, gridEX1.GetValue("Column02").ToString());
            }
            catch 
            {
                pictureBox1.Image = null;
                toolTip1.SetToolTip(pictureBox1, null);
            }
        }

        private void Form09_Appendix_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox1.Dock = DockStyle.Fill;
                panelEx1.AutoScroll = true;
            }
            else
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                panelEx1.AutoScroll = false;
            }
        }

        private void gridEX1_Error(object sender, Janus.Windows.GridEX.ErrorEventArgs e)
        {
            e.DisplayErrorMessage = false;
            Class_BasicOperation.CheckExceptionType(e.Exception, this.Name);
        }

        private void Form09_Appendix_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
                bt_Save_Click(sender, e);
            else if (e.Control && e.KeyCode == Keys.D)
                bt_Del_Click(sender, e);
        }
        private void gridEX1_UpdatingCell(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Column.Key == "Column01" || e.Column.Key == "Column02")
                    if (e.Value.ToString().Trim() == "")
                        e.Value = DBNull.Value;
            }
            catch
            {
            }
        }

        private void gridEX1_UpdatingCell_1(object sender, Janus.Windows.GridEX.UpdatingCellEventArgs e)
        {
            try
            {
                if (e.Column.Key == "Column01" || e.Column.Key == "Column02")
                    if (e.Value.ToString().Trim() == "")
                        e.Value = DBNull.Value;
            }
            catch
            {
            }
        }

        private void bt_Print_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < gridEX1.SelectedItems.Count; i++)
            {
                Process p = new Process();
                p.StartInfo.FileName = gridEX1.SelectedItems[i].GetRow().Cells["Column02"].Text.Trim();
                p.StartInfo.Verb = "Print";
                p.Start();
            }
          
        }

        

       

      
    }
}
