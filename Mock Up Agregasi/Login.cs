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

namespace Mock_Up_Agregasi
{
    public partial class Login : Form
    {
public Login()        
        {
            InitializeComponent();
        }
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
        

        private void Login_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            btnIN.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnIN.Width, btnIN.Height, 10, 10));

            CB_User.Items.Clear();

            //init Data
            List<varDataState> list = new List<varDataState>();
            list.Add(new varDataState() { ID = "01", Name = "Administrator" });
            list.Add(new varDataState() { ID = "02", Name = "Supervisor" });
            list.Add(new varDataState() { ID = "03", Name = "Operator 1" });
            list.Add(new varDataState() { ID = "04", Name = "Operator 2" });

            //set display member and value member for combobox
            CB_User.DataSource = list;
            CB_User.ValueMember = "ID";
            CB_User.DisplayMember = "Name";

        }

        
        private void btnMin_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to close appliaction?", "Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void btnIN_Click(object sender, EventArgs e)
        {
            if (CB_User.Text == "Administrator" && tbPass.Text == "admin123")
            {
                varGlobal.Username = CB_User.Text;
                FormMain mainMenu = new FormMain();
                mainMenu.Show();
                this.Hide();

            }
            else if (CB_User.Text == "Supervisor" && tbPass.Text == "spv123")
            {
                varGlobal.Username = CB_User.Text;
                FormMain mainMenu = new FormMain();
                mainMenu.Show();
                this.Hide();
            }
            else if (CB_User.Text == "Operator 1" && tbPass.Text == "opt123")
            {
                varGlobal.Username = CB_User.Text;
                FormMain mainMenu = new FormMain();
                mainMenu.Show();
                this.Hide();
            }
            else if (CB_User.Text == "Operator 2" && tbPass.Text == "opt321")
            {
                varGlobal.Username = CB_User.Text;
                FormMain mainMenu = new FormMain();
                mainMenu.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username or Password NotFound", "Perhatian..", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
