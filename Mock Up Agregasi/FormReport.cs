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
using CrystalDecisions.CrystalReports.Engine;

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
        private void GetDataWOCarton()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "select woNo from tblcartonrealease group by woNo";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sql, config.con);
            DataTable dt = new DataTable();
            dataAdapter1.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    CbNo_WO.Items.Add(row[0].ToString());
                    CbNo_WO.SelectedIndex = 0;
                }


            }
            else
            {

            }
            config.con.Close();
        }
        private void GetDataBatchCarton()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "select noBatch from tblcartonrealease group by noBatch";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sql, config.con);
            DataTable dt = new DataTable();
            dataAdapter1.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    cbBatch.Items.Add(row[0].ToString());
                    cbBatch.SelectedIndex = 0;
                }


            }
            else
            {

            }
            config.con.Close();
        }

        private void ViewReportAgregate()
        {
            //sql = "SELECT * FROM viewdataagregate";
            //reports(sql, "CRDataAgregate");
            string reportname = "CRDataAgregate";
            config.Init_Con();
            config.con.Open();
            string sql = "select idCarton, woNo, productName, noBatch, countCarton, dataScanRealese from tblcartonrealease where woNo='" + CbNo_WO.Text + "' and noBatch='" + cbBatch.Text + "'  ";
            MySqlDataAdapter da = new MySqlDataAdapter(sql, config.con);
            DataSet dataReport = new DataSet();
            da.Fill(dataReport, "dataMasterBox");

            string sql1 = "select idCarton, dataScan from tblhistory_scan ";
            MySqlDataAdapter da1 = new MySqlDataAdapter(sql1, config.con);

            da1.Fill(dataReport, "dataInnerBox");


            config.con.Close();

            CrystalDecisions.CrystalReports.Engine.ReportDocument reportdoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            string strReportPath = Application.StartupPath + "\\reports\\" + reportname + ".rpt";


            reportdoc.Load(strReportPath);

            CrystalDecisions.CrystalReports.Engine.FieldDefinition FieldDef;
            FieldDef = reportdoc.Database.Tables[0].Fields["idCarton"];
            reportdoc.DataDefinition.Groups[0].ConditionField = FieldDef;

            reportdoc.SetDataSource(dataReport);

            crystalReportViewer1.ReportSource = reportdoc;
        }
        private void FormReport_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblUser.Text = varGlobal.Username;
            GetDataBatchCarton();
            GetDataWOCarton();
            ////sql = "SELECT * FROM viewdataagregate";
            ////reports(sql, "CRDataAgregate");
            //string reportname = "CRDataAgregate";
            //config.Init_Con();
            //config.con.Open();
            //string sql = "select idCarton, woNo, productName, noBatch, countCarton, dataScanRealese from tblcartonrealease ";
            //MySqlDataAdapter da = new MySqlDataAdapter(sql,config.con);
            //DataSet dataReport = new DataSet();
            //da.Fill(dataReport, "dataMasterBox");

            //string sql1 = "select idCarton, dataScan from tblhistory_scan ";
            //MySqlDataAdapter da1 = new MySqlDataAdapter(sql1, config.con);
            
            //da1.Fill(dataReport, "dataInnerBox");


            //config.con.Close();

            //CrystalDecisions.CrystalReports.Engine.ReportDocument reportdoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            //string strReportPath = Application.StartupPath + "\\reports\\" + reportname + ".rpt";


            //reportdoc.Load(strReportPath);

            //CrystalDecisions.CrystalReports.Engine.FieldDefinition FieldDef;
            //FieldDef = reportdoc.Database.Tables[0].Fields["idCarton"];
            //reportdoc.DataDefinition.Groups[0].ConditionField = FieldDef;

            //reportdoc.SetDataSource(dataReport);

            //crystalReportViewer1.ReportSource = reportdoc;
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

        private void btn_Search_Click(object sender, EventArgs e)
        {
            ViewReportAgregate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblUser.Text = DateTime.Now.ToString();
        }
    }
}
