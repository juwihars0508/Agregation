using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using Mock_Up_Agregasi.Includes;
using MySql.Data.MySqlClient;

namespace Mock_Up_Agregasi
{
    public partial class FormDataReport : Form
    {
        public FormDataReport()
        {
            InitializeComponent();
        }

        SQLConfig config = new SQLConfig();
        usableFunction funct = new usableFunction();
        public DataTable dt;
        private void btnExport_Click(object sender, EventArgs e)
        {

            using (SaveFileDialog sfd = new SaveFileDialog() {Filter="Excel Workbook|*.xlsx" })
            {
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using(XLWorkbook workbook = new XLWorkbook())
                        {
                            workbook.Worksheets.Add(dt, "Report BPOM");
                            workbook.SaveAs(sfd.FileName);
                        }
                        MessageBox.Show("You Have Successfully Export");

                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            //dataGridView1.SelectAll();
            //DataObject copydata = dataGridView1.GetClipboardContent();
            //if (copydata != null) Clipboard.SetDataObject(copydata);
            //Microsoft.Office.Interop.Excel.Application xlapp = new Microsoft.Office.Interop.Excel.Application();
            //xlapp.Visible = true;
            //Microsoft.Office.Interop.Excel.Workbook xlWbook;
            //Microsoft.Office.Interop.Excel.Worksheet xlsheet;
            //object miseddata = System.Reflection.Missing.Value;
            //xlWbook = xlapp.Workbooks.Add(miseddata);

            //xlsheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWbook.Worksheets.get_Item(1);
            //Microsoft.Office.Interop.Excel.Range xlr = (Microsoft.Office.Interop.Excel.Range)xlsheet.Cells[1, 1];
            //xlr.Select();

            //xlsheet.PasteSpecial(xlr, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);




            

        }

        private void FormDataReport_Load(object sender, EventArgs e)
        {
            btnRptBpom.BackColor = Color.DeepSkyBlue;
            btnDataReport.BackColor = Color.Gray;
            pnlReportBPOM.Visible = true;

            loadData();
        }

        private void loadData()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT ACTION AS 'Action', productName AS 'ID Kemasan', dataScanRealese AS 'Barcode' ,  kodeNIE AS  'NIE', ' ' AS 'Lot No'  ,  expDate AS 'Exp Date' ,  noBatch AS 'Batch No' , ' ' as 'GTIN'  , is_Active AS  'IS_ACTIVE',   is_Sample AS 'IS_SAMPLE',  is_Reject AS 'IS_REJECT',   mfdDate AS 'MFG DATE', GTIN_code as 'SEKUNDER' ,   ' ' as'TERSIER' FROM viewdatareportagregation";

            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sql, config.con);
            dt = new DataTable();
            dataAdapter1.Fill(dt);

            dataGridView1.DataSource = dt;

            funct.ResponsiveDtg(dataGridView1);
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            config.con.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormMain formMain = new FormMain();
            formMain.Show();
            this.Hide();
        }

        private void btnDataReport_Click(object sender, EventArgs e)
        {
            pnlReportBPOM.Visible = false;
            FormReport formReport = new FormReport();
            formReport.Show();
            this.Hide();
        }

        private void btnRptGnrl_Click(object sender, EventArgs e)
        {
            pnlReportBPOM.Visible = true;

        }
    }
}
