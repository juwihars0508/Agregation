using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Mock_Up_Agregasi.Includes;

namespace Mock_Up_Agregasi
{
    public partial class FormReport : Form
    {
        public FormReport()
        {
            InitializeComponent();
        }

        public string sql;
        SQLConfig config = new SQLConfig();

        private void FormReport_Load(object sender, EventArgs e)
        {
            //sql = "SELECT * FROM viewdataagregate";
            //reports(sql, "CRDataAgregate");
            string reportname = "CRDataAgregate";
            config.Init_Con();
            config.con.Open();
            string sql = "select woNo, productName, noBatch, countCarton, dataScanRealese from tblcartonrealease ";
            MySqlDataAdapter da = new MySqlDataAdapter(sql,config.con);
            DataSet dataReport = new DataSet();
            da.Fill(dataReport, "dataMasterBox");
            config.con.Close();

            CrystalDecisions.CrystalReports.Engine.ReportDocument reportdoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            string strReportPath = Application.StartupPath + "\\reports\\" + reportname + ".rpt";


            reportdoc.Load(strReportPath);

            reportdoc.SetDataSource(dataReport);

            crystalReportViewer1.ReportSource = reportdoc;
        }

        private void reports(string sql, string rptname)
        {

            try
            {

                config.loadReports(sql);

                string reportname = rptname;


                CrystalDecisions.CrystalReports.Engine.ReportDocument reportdoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                string strReportPath = Application.StartupPath + "\\reports\\" + reportname + ".rpt";


                reportdoc.Load(strReportPath);
                reportdoc.SetDataSource(config.dt);

                crystalReportViewer1.ReportSource = reportdoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "No crystal reports installed. Pls. contact administrator.");
            }


        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormDataReport formDataReport = new FormDataReport();
            formDataReport.Show();
            this.Hide();
        }
    }
}
