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
            

            btnSave.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnSave.Width, btnSave.Height, 20, 20));
            btnBack.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnBack.Width, btnBack.Height, 20, 20));
            loadDataProduct();
            disab();
            varGlobal.GetNilai(varUtility.fileMinRange);
            tbMinWeight.Text = Nilai.StringNilai;
            varGlobal.GetNilai(varUtility.fileMaxRange);
            tbMaxWeight.Text = Nilai.StringNilai;
            varGlobal.GetNilai(varUtility.fileQtyCase);
            tbQtyCase.Text = Nilai.StringNilai;


        }

        private void loadDataProduct()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT DISTINCT wo_no, kodeRecipe, Product FROM tblhistory_print WHERE STATUS='1'";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                
                vData_wo = dr[0].ToString();
                vData_kodeRecipe = dr[1].ToString();
                cbProduct.Items.Add(dr[2].ToString());

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
            }
            else if (btnSave.Text == "Save")
            {
                varGlobal.product = cbProduct.Text;
                varGlobal.qtyCase = tbQtyCase.Text;
                varGlobal.stdWieght = tbSTDWeight.Text;
                varGlobal.minWeight = tbMinWeight.Text;
                varGlobal.maxWeight = tbMaxWeight.Text;
                varGlobal.dataPrint = tbDataPrint.Text;
                Nilai.StringNilai = tbMinWeight.Text;
                varGlobal.SaveNilai(varUtility.fileMinRange);
                Nilai.StringNilai = tbMaxWeight.Text;
                varGlobal.SaveNilai(varUtility.fileMaxRange);
                Nilai.StringNilai = tbQtyCase.Text;
                varGlobal.SaveNilai(varUtility.fileQtyCase);
                MessageBox.Show("Data Ter-Set", "Information");
                btnSave.Text = "Set";
                disab();
            }
        }

        private void Enab()
        {
            cbProduct.Enabled = true;
            tbQtyCase.Enabled = true;

            tbSTDWeight.Enabled = true;
            tbMinWeight.Enabled = true;
            tbMaxWeight.Enabled = true;
            //btnSave.Enabled = true;
        }

        private void disab()
        {
            cbProduct.Enabled = false;
            tbQtyCase.Enabled = false;

            tbSTDWeight.Enabled = false;
            tbMinWeight.Enabled = false;
            tbMaxWeight.Enabled = false;
            //btnSave.Enabled = false;
        }


    }
}
