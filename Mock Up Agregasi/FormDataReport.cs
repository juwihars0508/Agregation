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
using ClosedXML.Report;
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
            this.generateReport();
            //using (SaveFileDialog sfd = new SaveFileDialog() {Filter="Excel Workbook|*.xlsx" })
            //{
            //    if(sfd.ShowDialog() == DialogResult.OK)
            //    {
            //        try
            //        {
            //            using(XLWorkbook workbook = new XLWorkbook())
            //            {
            //                workbook.Worksheets.Add(dt, "Report BPOM");
            //                workbook.SaveAs(sfd.FileName);
            //            }
            //            MessageBox.Show("You Have Successfully Export");

            //        }
            //        catch(Exception ex)
            //        {
            //            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //}

            




            

        }

        private void FormDataReport_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblUser.Text = varGlobal.Username;
            btnRptBpom.BackColor = Color.DeepSkyBlue;
            btnDataReport.BackColor = Color.Gray;
            pnlReportBPOM.Visible = true;
            getDataCB_Product();
            //loadData();
        }

        private void loadDataGridView()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT ACTION AS 'Action', productName AS 'ID Kemasan', dataScan AS 'Barcode' ,  kodeNIE AS  'NIE', ' ' AS 'Lot No'  ,  expDate AS 'Exp Date' ,  noBatch AS 'Batch No' , ' ' as 'GTIN'  , is_Active AS  'IS_ACTIVE',   is_Sample AS 'IS_SAMPLE',  is_Reject AS 'IS_REJECT',   mfdDate AS 'MFG DATE', dataScanRealese as 'SEKUNDER' ,   ' ' as'TERSIER' FROM viewdatareportagregation where productName='" + cbFilterProduct.Text + "' AND kodeNIE= '" + cbFilterNIE.Text + "' AND noBatch='" + cbFilterBatch.Text  + "'";

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
            getDataCB_Product();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
        }

        private void getDataCB_Product()
        {


            cbFilterProduct.Items.Clear();
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT productName FROM viewdatareportagregation  GROUP BY productName";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sql, config.con);
            DataTable dt = new DataTable();
            dataAdapter1.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    cbFilterProduct.Items.Add(row[0].ToString());
                    //cb_wo.Enabled = true;


                }

                cbFilterProduct.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Data  Tidak Di temukan", "Perhatian");


            }
            config.con.Close();

        }

        private void getDataCB_NIE()
        {

            cbFilterNIE.Items.Clear();
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT kodeNIE FROM viewdatareportagregation where productName='" + cbFilterProduct.Text + "' GROUP BY kodeNIE";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sql, config.con);
            DataTable dt = new DataTable();
            dataAdapter1.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    cbFilterNIE.Items.Add(row[0].ToString());
                    //cb_wo.Enabled = true;


                }

                cbFilterNIE.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Data  Tidak Di temukan", "Perhatian");


            }
            config.con.Close();
        }

        private void getDataCB_BatchNo()
        {





            cbFilterBatch.Items.Clear();
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT noBatch FROM viewdatareportagregation where productName='" + cbFilterProduct.Text + "' GROUP BY noBatch";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sql, config.con);
            DataTable dt = new DataTable();
            dataAdapter1.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {

                    cbFilterBatch.Items.Add(row[0].ToString());
                    //cb_wo.Enabled = true;


                }

                cbFilterBatch.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Data  Tidak Di temukan", "Perhatian");


            }
            config.con.Close();
        }

        private void cbFilterProduct_SelectedValueChanged(object sender, EventArgs e)
        {
            getDataCB_NIE();
            getDataCB_BatchNo();
        }

        private void btnViewData_Click(object sender, EventArgs e)
        {
            loadDataGridView();
        }

        private void generateReport()
        {
            IXLWorkbook work = (new XLTemplate("template.xlsx")).Workbook;
            IXLWorksheet sheet = work.Worksheet("Sheet1");
            int currCellY = 2;
            if (this.dt.Rows.Count != 0)
            {
                //sheet.Cell(1, 3).Value = DateTime.Now.ToString();
                //sheet.Cell(2, 3).Value = this.getLogInUser();
                foreach (DataRow data in this.dt.Rows)
                {
                    //sheet.Cell(currCellY, 0).Value = data[0].ToString();
                    sheet.Cell(currCellY, 1).Value = data[0].ToString();
                    sheet.Cell(currCellY, 2).Value = data[1].ToString();
                    sheet.Cell(currCellY, 3).Value = data[2].ToString();
                    sheet.Cell(currCellY, 4).Value = data[3].ToString();
                    sheet.Cell(currCellY, 5).Value = data[4].ToString();
                    sheet.Cell(currCellY, 6).Value = data[5].ToString();
                    sheet.Cell(currCellY, 7).Value = data[6].ToString();
                    sheet.Cell(currCellY, 8).Value = data[7].ToString();
                    sheet.Cell(currCellY, 9).Value = data[8].ToString();
                    sheet.Cell(currCellY, 10).Value = data[9].ToString();
                    sheet.Cell(currCellY, 11).Value = data[10].ToString();
                    sheet.Cell(currCellY, 12).Value = data[11].ToString();
                    sheet.Cell(currCellY, 13).Value = data[12].ToString();
                    sheet.Cell(currCellY, 14).Value = data[13].ToString();
                    currCellY++;
                }
                SaveFileDialog saveFileDialog1 = new SaveFileDialog()
                {
                    Filter = "xlsx Files|*.xlsx",
                    Title = "Save an Excel File"
                };
                saveFileDialog1.ShowDialog();
                if (saveFileDialog1.FileName == "")
                {
                    MessageBox.Show("Generate Report Failed");
                    //LogCollector.writetoLogDb("Event Report Generate Fail", "SYSTEM", this.getLogInUser());
                }
                else
                {
                    work.SaveAs(saveFileDialog1.FileName);
                    MessageBox.Show("Generate Complete");
                    //LogCollector.writetoLogDb("Event Report Generated", "SYSTEM", this.getLogInUser());
                }
            }
            else
            {
                MessageBox.Show("Data Empty");
            }
        }
    }
}
