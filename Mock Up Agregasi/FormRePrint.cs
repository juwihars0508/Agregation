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
using MySql.Data.MySqlClient;

namespace Mock_Up_Agregasi
{
    public partial class FormRePrint : Form
    {
        public FormRePrint()
        {
            InitializeComponent();
        }

        SQLConfig config = new SQLConfig();

        string VdataNoBatch;
        string VdataExpDate;
        string VdataQty;
        string VWeight;
        string VdataPrint;
        string VdataProductName;
        string VdataMfgDate;
        string VCartonNo;
        string VdataKetKemasan;
        string VWoNo;
        string VProductKode;


        private void loadDataHistoryLabel()
        {
            string sql = "SELECT woNo AS 'No WO', productCode AS 'Product Code', productName AS 'Product Name', noBatch AS 'Batch No', expdate AS 'EXP Date', qtyCarton AS 'Qty Carton', WeightCarton AS 'Weight', cartonNo AS 'Carton No', dataBarcode AS 'Barcode Label' FROM tblhistory_printlabel";
            config.Load_DTG(sql, dgv);
        }

        private void loadDataHistoryLabel_Find()
        {
            string sql = "SELECT woNo AS 'No WO', productCode AS 'Product Code', productName AS 'Product Name', noBatch AS 'Batch No', expdate AS 'EXP Date', qtyCarton AS 'Qty Carton', WeightCarton AS 'Weight', cartonNo AS 'Carton No', dataBarcode AS 'Barcode Label' FROM tblhistory_printlabel where woNo Like '" + tbFind.Text + "%' or noBatch Like '" + tbFind.Text + "%'";
            config.Load_DTG(sql, dgv);
        }

        private void FormRePrint_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblUser.Text = varGlobal.Username;
            loadDataHistoryLabel();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormMain formMain = new FormMain();
            formMain.Show();
            this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            loadDataHistoryLabel_Find();
        }

        private void printLabel()
        {
            varGlobal.GetNilai(varUtility.filePrinterName);
            string vPrinterName = Nilai.StringNilai;
            varGlobal.GetNilai(varUtility.fileFileNamePrn);
            string vDataPrint = Nilai.StringNilai;

            //VdataQty = lbActualQtyCase.Text;

            string fileprn = @"Data\" + vDataPrint + ".prn";
            string s = "^XA" +
                      "^FH\"^FDVdatamatrix123456789012asddasdad^FS" +
                        "^ FT270,284 ^ A0N,28,28 ^ FH\"^CI28 ^ FDVProduct Name1 ^ FS ^ CI27" +
                        "^ FT270,340 ^ A0N,28,28 ^ FH\"^CI28 ^ FDVProduct Code ^ FS ^ CI27" +
                        "^ FT274,384 ^ A0N,28,28 ^ FH\"^CI28 ^ FDVBatch No ^ FS ^ CI27" +
                        "^ FT274,435 ^ A0N,28,28 ^ FH\"^CI28 ^ FDVExp Date ^ FS ^ CI27" +
                        "^ FT270,485 ^ A0N,28,28 ^ FH\"^CI28 ^ FDVQty ^ FS ^ CI27" +
                        "^ FT275,543 ^ A0N,28,28 ^ FH\"^CI28 ^ FDVWeight ^ FS ^ CI27" +
                        "^XZ";

            string SC = "^XA" +
                        "^MMT" +
                        "^PW831" +
                        "^LL806" +
                        "^LS0" +
                        "^FT29,101 ^A0N,51,51 ^FH\"^CI28 ^FDVProduct Name ^FS ^CI27" +
                        "^FO5,5 ^GB820,790,8 ^FS" +
                        "^FT29,156 ^A0N,42,43 ^FH\"^CI28 ^FDVWo Number ^FS ^CI27" +
                        "^FT46,284 ^A0N,28,28 ^FH\"^CI28 ^FDProduct Name ^FS ^CI27" +
                        "^FT46,340 ^A0N,28,28 ^FH\"^CI28 ^FDProduct Code ^FS ^CI27" +
                        "^FT46,384 ^A0N,28,28 ^FH\"^CI28 ^FDBatch No ^FS ^CI27" +
                        "^FT46,435 ^A0N,28,28 ^FH\"^CI28 ^FDExp Date ^FS ^CI27" +
                        "^FT46,485 ^A0N,28,28 ^FH\"^CI28 ^FDQty ^FS ^CI27" +
                        "^FT51,543 ^A0N,28,28 ^FH\"^CI28 ^FDWeight ^FS ^CI27" +
                        "^FT234,284 ^A0N,28,28 ^FH\"^CI28 ^FD:^FS ^CI27" +
                        "^FT234,340 ^A0N,28,28 ^FH\"^CI28 ^FD:^FS ^CI27" +
                        "^FT234,384 ^A0N,28,28 ^FH\"^CI28 ^FD:^FS ^CI27" +
                        "^FT234,433 A0N,28,28 ^FH\"^CI28 ^FD:^FS ^CI27" +
                        "^FT234,485 ^A0N,28,28 ^FH\"^CI28 ^FD:^FS ^CI27" +
                        "^FT234,543 ^A0N,28,28 ^FH\"^CI28 ^FD:^FS ^CI27" +
                        "^FT641,717 ^BXN,3,200,0,0,1,_,1" +
                        "^FH\"^FDVdatamatrix123456789012asddasdad ^FS" +
                        "^FT270,284^A0N,28,28^FH\"^CI28^FDVProduct Name1 ^FS^CI27" +
                        "^FT270,340 ^A0N,28,28 ^FH\"^CI28 ^FDVProduct Code ^FS ^CI27" +
                        "^FT274,384 ^A0N,28,28 ^FH\"^CI28 ^FDVBatch No ^FS ^CI27" +
                        "^FT274,435 ^A0N,28,28 ^FH\"^CI28 ^FDVExp Date ^FS ^CI27" +
                        "^FT270,485 ^A0N,28,28 ^FH\"^CI28 ^FDVQty ^FS ^CI27" +
                        "^FT275,543 ^A0N,28,28 ^FH\"^CI28 ^FDVWeight ^FS ^CI27" +
                        "^XZ";

            //string SC1 = "^XA" +
            //             "~TA000" +
            //             "~JSN" +
            //             "^LT0" +
            //             "^MNW" +
            //             "^MTD" +
            //             "^PON" +
            //             "^PMN" +
            //             "^LH0,0" +
            //             "^JMA" +
            //             "^PR6,6" +
            //             "~SD15" +
            //             "^JUS" +
            //             "^LRN" +
            //             "^CI27" +
            //             "^PA0,1,1,0" +
            //             "^XZ" +
            //             "^XA" +
            //             "^MMT" +
            //             "^PW831" +
            //             "^LL639" +
            //             "^LS0" +
            //             "^FO7,5^GB820,632,8^FS" +
            //             //"^FT68,293^A0N,28,28^FH\"^CI28^FDProduct Code^FS^CI27" +
            //             "^FT68,215^A0N,28,28^FH\"^CI28^FDBatch No^FS^CI27" +
            //             "^FT68,262^A0N,28,28^FH\"^CI28^FDMfg Date^FS^CI27" +
            //             "^FT68,310^A0N,28,28^FH\"^CI28^FDExp Date^FS^CI27" +
            //             "^FT68,417^A0N,28,28^FH\"^CI28^FDQty^FS^CI27" +
            //             "^FT68,367^A0N,28,28^FH\"^CI28^FDWeight^FS^CI27" +
            //             //"^FT256,292^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
            //             "^FT256,215^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
            //             "^FT256,262^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
            //             "^FT256,310^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
            //             "^FT256,367^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
            //             "^FT256,417^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
            //             //"^FT290,293^A0N,28,28^FH\"^CI28^FD" + VdataProductKode + "^FS^CI27" +
            //             "^FT290,215^A0N,28,28^FH\"^CI28^FD" + VdataNoBatch + "^FS^CI27" +
            //             "^FT290,262^A0N,28,28^FH\"^CI28^FD" + VdataMfgDate + "^FS^CI27" +
            //             "^FT290,310^A0N,28,28^FH\"^CI28^FD" + VdataExpDate + "^FS^CI27" +
            //             "^FT290,417^A0N,28,28^FH\"^CI28^FD" + VdataQty + "^FS^CI27" +
            //             "^FT290,367^A0N,28,28^FH\"^CI28^FD" + lbWeight.Text + "^FS^CI27" +
            //             "^FT579,606^BXN,6,200,0,0,1,_,1" +
            //             "^FH\"^FD" + VdataPrint + lb_idCarton.Text + "^FS" +
            //             "^FT51,110^A0N,51,51^FH\"^CI28^FD" + VdataProductName + "^FS^CI27" +
            //             //"^FT51,165^A0N,42,43^FH\"^CI28^FD" + VdataWoNo + "^FS^CI27" +
            //             "^FT68,474^A0N,28,28^FH\"^CI28^FDCarton No.^FS^CI27" +
            //             "^FT256,474^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
            //             "^FT290,474^A0N,28,28^FH\"^CI28^FD" + lbTotalCase.Text + "^FS^CI27" +
            //             "^PQ1,,,Y" +
            //             "^XZ";
            string SC2 = "~SD15" +
                          "^JUS" +
                          "^LRN" +
                          "^CI27" +
                          "^PA0,1,1,0" +
                          "^XZ" +
                          "^XA" +
                          "^MMT" +
                          "^PW831" +
                          "^LL639" +
                          "^LS0" +
                          "^FT20,265^A0N,85,86^FH\"^CI28^FDNo. Batch^FS^CI27" +
                          "^FT248,391^A0N,34,33^FH\"^CI28^FDED^FS^CI27" +
                          "^FT248,497^A0N,34,33^FH\"^CI28^FDIsi^FS^CI27" +
                          "^FT248,448^A0N,34,33^FH\"^CI28^FDBerat^FS^CI27" +
                          "^FT374,265^A0N,85,162^FH\"^CI28^FD:^FS^CI27" +
                          "^FT314,340^A0N,34,33^FH\"^CI28^FD:^FS^CI27" +
                          "^FT314,389^A0N,34,33^FH\"^CI28^FD:^FS^CI27" +
                          "^FT314,448^A0N,34,33^FH\"^CI28^FD:^FS^CI27" +
                          "^FT314,497^A0N,34,33^FH\"^CI28^FD:^FS^CI27" +
                          "^FT444,265^A0N,85,86^FH\"^CI28^FD" + VdataNoBatch + "^FS^CI27" +  // Batch
                          "^FT338,391^A0N,34,33^FH\"^CI28^FD" + VdataExpDate + "^FS^CI27" + // Exp Date
                          "^FT338,497^A0N,34,33^FH\"^CI28^FD" + VdataQty + "^FS^CI27" +    // Qty
                          "^FT338,448^A0N,34,33^FH\"^CI28^FD" + VWeight + "^FS^CI27" + // Weight
                          "^FT37,494^BXN,8,200,0,0,1,_,1" +
                          "^FH\"^FD" + VdataPrint + "^FS" + // Datamatrix
                          "^FT24,108^A0N,85,86^FH\"^CI28^FD" + VdataProductName + "^FS^CI27" + // Product Name
                          "^FT575,451^A0N,102,101^FH\"^CI28^FD" + VCartonNo + "^FS^CI27" + // No. karton
                          "^FT248,343^A0N,34,33^FH\"^CI28^FDMD^FS^CI27" +
                          "^FT338,343^A0N,34,33^FH\"^CI28^FD" + VdataMfgDate + "^FS^CI27" +  //MFG Date
                          "^FO7,5^GB820,632,8^FS" +
                          "^FO507,310^GB284,202,6^FS" +
                          "^FT32,173^A0N,51,51^FH\"^CI28^FD" + VdataKetKemasan + "^FS^CI27" +
                          "^PQ1,,,Y" +
                          "^XZ";



            //RawPrinterHelper.SendFileToPrinter(vPrinterName, fileprn);
            RawPrinterHelper.SendStringToPrinter(vPrinterName, SC2);
            //}
            //}
            //saveHistoryPrintLabel();
            //saveCartonRealese();
            //kodeotomatis();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            VWoNo = dgv.SelectedRows[0].Cells[0].Value.ToString();
            VProductKode = dgv.SelectedRows[0].Cells[1].Value.ToString();
            VdataProductName = dgv.SelectedRows[0].Cells[2].Value.ToString();
            VdataNoBatch = dgv.SelectedRows[0].Cells[3].Value.ToString();
            VdataExpDate = dgv.SelectedRows[0].Cells[4].Value.ToString();
            VdataQty = dgv.SelectedRows[0].Cells[5].Value.ToString();
            VWeight = dgv.SelectedRows[0].Cells[6].Value.ToString();
            VCartonNo = dgv.SelectedRows[0].Cells[7].Value.ToString();
            VdataPrint = dgv.SelectedRows[0].Cells[8].Value.ToString();
            loadKetProd();
            loadKetMFGDate();

        }

        private void loadKetProd()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "select bentukSediaan from  tblproduk where kodeProduk='" + VProductKode + "'";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                VdataKetKemasan = dr[0].ToString();
                //VdataProductName = dr[3].ToString();
                //VdataNoBatch = dr[4].ToString();
                //dataMfgDate_temp = dr[5].ToString();
                //dataExpDate_temp = dr[6].ToString();




            }
            dr.Close();
            config.con.Close();
        }

        private void loadKetMFGDate()
        {
            string dataMfgDate_temp = string.Empty;

            config.Init_Con();
            config.con.Open();
            string sql = "select * from  tblworkorder where wo_No='" + VWoNo + "'";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                dataMfgDate_temp = dr[7].ToString();
                //VdataProductName = dr[3].ToString();
                //VdataNoBatch = dr[4].ToString();
                //dataMfgDate_temp = dr[5].ToString();
                //dataExpDate_temp = dr[6].ToString();




            }
            dr.Close();
            config.con.Close();

            string thn_MfgDate = dataMfgDate_temp.Substring(0, 4);
            string bln_MfgDate = dataMfgDate_temp.Substring(5, 2);
            string tgl_mfgDate = dataMfgDate_temp.Substring(8, 2);
            VdataMfgDate = bln_MfgDate + "-" + thn_MfgDate;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printLabel();
        }
    }
}
