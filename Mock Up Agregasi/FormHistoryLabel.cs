using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mock_Up_Agregasi.Includes;

namespace Mock_Up_Agregasi
{
    public partial class FormHistoryLabel : Form
    {
        public FormHistoryLabel()
        {
            InitializeComponent();
        }

        SQLConfig config = new SQLConfig();

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormData formData = new FormData();
            formData.Show();
            this.Hide();
        }

        private void loadDataHistoryLabel()
        {
            string sql = "SELECT woNo AS 'No WO', productCode AS 'Product Code', productName AS 'Product Name', noBatch AS 'Batch No', expdate AS 'EXP Date', qtyCarton AS 'Qty Carton', WeightCarton AS 'Weight', cartonNo AS 'Carton No', dataBarcode AS 'Barcode Label' FROM tblhistory_printlabel";
            config.Load_DTG(sql,dgv);
        }

        private void FormHistoryLabel_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblUser.Text = varGlobal.Username;
            loadDataHistoryLabel();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
        }
    }
}
