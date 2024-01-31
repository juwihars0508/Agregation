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
            lblUser.Text = varGlobal.Username;
            panel2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width, panel2.Height, 20, 20));
            panel3.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel3.Width, panel3.Height, 20, 20));
            btn_LogOut.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn_LogOut.Width, btn_LogOut.Height, 20, 20));
            btn_MenuAggregation.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn_MenuAggregation.Width, btn_MenuAggregation.Height, 20, 20));
            btn_EditData.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn_EditData.Width, btn_EditData.Height, 20, 20));
            btn_Setting.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn_Setting.Width, btn_Setting.Height, 20, 20));
            btn_MenuReport.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn_MenuReport.Width, btn_MenuReport.Height, 20, 20));
        }

        private void btn_MenuAggregation_Click(object sender, EventArgs e)
        {

        }
    }
}
