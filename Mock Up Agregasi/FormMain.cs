using MySql.Data.MySqlClient;
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

namespace Mock_Up_Agregasi
{
    public partial class FormMain : Form
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

        SQLConfig config = new SQLConfig();

        public FormMain()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void btnMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            timer1.Start();
            
            lblUser.Text = varGlobal.Username;
            panel2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width, panel2.Height, 20, 20));
            panel3.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel3.Width, panel3.Height, 20, 20));
            btn_LogOut.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn_LogOut.Width, btn_LogOut.Height, 20, 20));
            btn_MenuAggregation.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn_MenuAggregation.Width, btn_MenuAggregation.Height, 20, 20));
            btn_EditData.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn_EditData.Width, btn_EditData.Height, 20, 20));
            btn_RePrint.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn_RePrint.Width, btn_RePrint.Height, 20, 20));
            btn_MenuReport.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn_MenuReport.Width, btn_MenuReport.Height, 20, 20));
        }

        private void btn_MenuAggregation_Click(object sender, EventArgs e)
        {
            if (varGlobal.woNo == null && varGlobal.dataKodeRecipe == null)
            {
                MessageBox.Show("Silahkan Input Produk pada Menu Edit","Perhatian!!..", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                FormData formData = new FormData();
                formData.Show();
                this.Hide();
            }
            
        }

        private void btn_LogOut_Click(object sender, EventArgs e)
        {
            Login frmLogin = new Login();
            frmLogin.Show();
            this.Hide();
        }

        private void btn_EditData_Click(object sender, EventArgs e)
        {
            DataEditAgg dataEditAgg = new DataEditAgg();
            dataEditAgg.Show();
            this.Hide();
        }

        private void btn_MenuReport_Click(object sender, EventArgs e)
        {
            FormDataReport formDataReport = new FormDataReport();
            formDataReport.Show();
            this.Hide(); 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
        }

        private void btn_Setting_Click(object sender, EventArgs e)
        {
            FormRePrint formRePrint = new FormRePrint();
            formRePrint.Show();
            this.Hide();
        }

        //private void updateFlagWO()
        //{
        //    config.Init_Con();
        //    config.con.Open();
        //    string sql = "update tblworkorder set status=1 where wo_no='" + cbWO.Text + "' and kodeRecipe='" + VdataKodeRecipe + "'";
        //    MySqlCommand cmd = new MySqlCommand(sql, config.con);
        //    cmd.ExecuteNonQuery();
        //    config.con.Close();
        //}

        private void btnCloseWO_Click(object sender, EventArgs e)
        {
            //updateFlagWO();
            //MessageBox.Show("Data WO telah ter-Close", "Perhatian");
        }
    }
}
