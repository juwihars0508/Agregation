using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mock_Up_Agregasi.Includes;
using MySql.Data.MySqlClient;

namespace Mock_Up_Agregasi
{
    public partial class DataEditAgg : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
          int nLeftRect,     // x-coordinate of upper-left corner
          int nTopRect,      // y-coordinate of upper-left corner
          int nRightRect,    // x-coordinate of lower-right corner
          int nBottomRect,   // y-coordinate of lower-right corner
          int nWidthEllipse, // height of ellipse
          int nHeightEllipse // width of ellipse
        );
        public DataEditAgg()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        SQLConfig config = new SQLConfig();
        usableFunction UsableFunction = new usableFunction();

        public string vData_wo;
        public string vData_kodeRecipe;
        public string vDateTrans;
        public string VdataWoNo;
        public string VdataKodeRecipe;
        public string VdataWoNoTemp;

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            //panel2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width, panel2.Height, 20, 20));
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            //panel3.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel3.Width, panel3.Height, 20, 20));
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            //panel4.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel4.Width, panel4.Height, 20, 20));
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            //panel5.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel5.Width, panel5.Height, 20, 20));
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            //panel6.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel6.Width, panel6.Height, 20, 20));
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            //panel7.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel7.Width, panel7.Height, 20, 20));
        }

        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            //panel9.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel9.Width, panel9.Height, 20, 20));
        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            //panel10.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel10.Width, panel10.Height, 20, 20));
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {
            //panel8.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel8.Width, panel8.Height, 20, 20));
        }

        private void DataEditAgg_Load(object sender, EventArgs e)
        {

            timer1.Start();
            lblUser.Text = varGlobal.Username;

            btnSave.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSave.Width, btnSave.Height, 20, 20));
            btnBack.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnBack.Width, btnBack.Height, 20, 20));
            //loadDataProduct();
            //load_DatacbWO();
            disab();
            varGlobal.GetNilai(varUtility.fileMinRange);
            tbMinWeight.Text = Nilai.StringNilai;
            varGlobal.GetNilai(varUtility.fileMaxRange);
            tbMaxWeight.Text = Nilai.StringNilai;
            varGlobal.GetNilai(varUtility.fileQtyCase);
            tbQtyCase.Text = Nilai.StringNilai;


        }
        private void load_DatacbWO()
        {
            cb_Wo.Items.Clear();
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT  wo_no, kodeRecipe FROM tblhistory_print WHERE STATUS=1 and waktu like '" + vDateTrans + "%'Group by wo_no";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sql, config.con);
            DataTable dt = new DataTable();
            dataAdapter1.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    VdataWoNo = row[0].ToString();
                    VdataKodeRecipe = row[1].ToString();
                    checkDataWO();
                    if (VdataWoNo != VdataWoNoTemp)
                    {
                        cb_Wo.Items.Add(row[0].ToString());
                        cb_Wo.Enabled = true;
                    }
                    else
                    {
                        cb_Wo.Items.Clear();
                        MessageBox.Show("WO Sudah Terclose");
                        
                    }

                }


            }
            else
            {
                MessageBox.Show("WO Pada Tanggal Ini Tidak Di temukan","Perhatian");
                cb_Wo.Text = "";
                tb_Product.Text = "";
                tbDataPrint.Text = "";
                cb_Wo.Enabled = false;
                tb_Product.Enabled = false;
                
            }
            config.con.Close();
        }
        private void load_DatacbWO_old()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT  wo_no, kodeRecipe FROM tblhistory_print WHERE STATUS=1 and waktu like '" + vDateTrans + "%'Group by wo_no";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                VdataWoNo = dr[0].ToString();
                VdataKodeRecipe = dr[1].ToString();
                checkDataWO();
                if (VdataWoNo != VdataWoNoTemp)
                {
                    cb_Wo.Items.Add(dr[0].ToString());
                }
                else
                {
                    cb_Wo.Items.Clear();
                }


            }

            dr.Close();
            config.con.Close();


        }

        private void checkDataWO()
        {
            int flag = 1;
            config.Init_Con();
            config.con.Open();
            string sql = "select wo_no from tblworkorder where wo_no='" + VdataWoNo + "' and kodeRecipe='" + VdataKodeRecipe + "' and status='" + flag + "'";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sql, config.con);
            DataTable dt = new DataTable();
            dataAdapter1.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    VdataWoNoTemp = row[0].ToString();

                }


            }
            else
            {

            }
            config.con.Close();
        }

        private void loadDataProduct()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT  wo_no, kodeRecipe, Product FROM tblhistory_print WHERE wo_no='" + cb_Wo.Text + "' and STATUS='1' GROUP BY wo_no";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                
                vData_wo = dr[0].ToString();
                vData_kodeRecipe = dr[1].ToString();
                tb_Product.Text = dr[2].ToString();

            }

            dr.Close();
            
            config.con.Close();
        }

        private void cbProduct_SelectedValueChanged(object sender, EventArgs e)
        {
            loadDataPrint();
        }

        private void loadDataPrint()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT  GTIN_code FROM tblworkorder WHERE wo_no='" + vData_wo + "' and kodeRecipe='" + vData_kodeRecipe +"' ";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                tbDataPrint.Text =   dr[0].ToString();



            }

            dr.Close();

            config.con.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormData formData = new FormData();
            formData.Show();
            this.Hide();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            

            if (btnSave.Text == "Set")
            {
                Enab();
                btnSave.Text = "Save";
                btnBack.Enabled = false;
            }
            else if (btnSave.Text == "Save")
            {
                if (cb_Wo.Text.Length == 0)
                {
                    MessageBox.Show("Data WO Tidak boleh Kosong", "Perhatian", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (tb_Product.Text.Length == 0)
                {
                    MessageBox.Show("Data Product Tidak Boleh Kosong", "Perhatian", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (tbQtyCase.Text.Length == 0)
                {
                    MessageBox.Show("Data Qty Case Tidak boleh Kosong", "Perhatian", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (tbSTDWeight.Text.Length == 0)
                {
                    MessageBox.Show("Data STD Weight Tidak Boleh Kosong", "Perhatian", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else if (tbDataPrint.Text.Length == 0)
                {
                    MessageBox.Show("Data Print Tidak Boleh Kosong","Perhatian",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                }
                else
                {
                    varGlobal.product = tb_Product.Text;
                    varGlobal.qtyCase = tbQtyCase.Text;
                    varGlobal.stdWieght = tbSTDWeight.Text;
                    varGlobal.minWeight = tbMinWeight.Text;
                    varGlobal.maxWeight = tbMaxWeight.Text;
                    varGlobal.dataPrint = tbDataPrint.Text;
                    varGlobal.woNo = cb_Wo.Text;
                    varGlobal.dataKodeRecipe = vData_kodeRecipe;
                    Nilai.StringNilai = tbMinWeight.Text;
                    varGlobal.SaveNilai(varUtility.fileMinRange);
                    Nilai.StringNilai = tbMaxWeight.Text;
                    varGlobal.SaveNilai(varUtility.fileMaxRange);
                    Nilai.StringNilai = tbQtyCase.Text;
                    varGlobal.SaveNilai(varUtility.fileQtyCase);
                    MessageBox.Show("Data Ter-Set", "Information");
                    btnSave.Text = "Set";
                    btnBack.Enabled = true;
                    disab();
                }
                
            }
        }

        private void Enab()
        {
            tb_Product.Enabled = true;
            tbQtyCase.Enabled = true;
            cb_Wo.Enabled = true;
            dtp.Enabled = true;
            tbSTDWeight.Enabled = true;
            tbMinWeight.Enabled = true;
            tbMaxWeight.Enabled = true;
            //btnSave.Enabled = true;
        }

        private void disab()
        {
            tb_Product.Enabled = false;
            tbQtyCase.Enabled = false;
            cb_Wo.Enabled = false;
            dtp.Enabled = false;
            tbSTDWeight.Enabled = false;
            tbMinWeight.Enabled = false;
            tbMaxWeight.Enabled = false;
            //btnSave.Enabled = false;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {
            vDateTrans = dtp.Value.ToString();
            vDateTrans = vDateTrans.Substring(0, 10);
            //load_DatacbWO();
        }

        private void cb_Wo_SelectedValueChanged(object sender, EventArgs e)
        {
            loadDataProduct();
            loadDataPrint();
        }

        private void cbLastBatch_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLastBatch.Checked == true)
            {
                //tbJmlBotol.Enabled = true;
                varGlobal.lastBatch = "true";
                //pnlBotol.Visible = true;
            }
            else
            {
                //tbJmlBotol.Enabled = false;
                varGlobal.lastBatch = "false";
                //pnlBotol.Visible = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
        }
    }
}
