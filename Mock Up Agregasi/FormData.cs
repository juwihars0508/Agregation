using System;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mock_Up_Agregasi.Includes;
using MySql.Data.MySqlClient;
//using Org.BouncyCastle.Utilities;

namespace Mock_Up_Agregasi
{
    public partial class FormData : Form
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

        public string CR = System.Char.ConvertFromUtf32(13);
        public string LF = System.Char.ConvertFromUtf32(10);

        public FormData()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        


        //Variable Data COM Timbangan
        public string vCOMportTimbangan;
        public string vBaudRateTimbangan;
        public string vDataBitsTimbangan;
        public string vStopBitsTimbangan;
        public string vParityTimbangan;

        //Variable Data COM CB
        public string vCOMportCB;
        public string vBaudRateCB;
        public string vDataBitsCB;
        public string vStopBitsCB;
        public string vParityCB;

        //variable COM Port Setting
        public string VCOMPort;
        public string VBaudrate;
        public string VDataBits;
        public string VStopBits;
        public string VParity;

        //General variable
        public string ReadData;
        public string ReadData2;
        public string ReadDataTimbang;
        public string ReadDataTimbang2;
        public string dataStatus;
        public string VDataTarget;
        public string Vdata_GTINCodeMatrix;
        public string VdataKodeRecipe;
        public string Vdata_minRange;
        public string Vdata_maxRange;
        public string VdataNoBatch;
        public string VdataProductKode;
        public string VdataProductName;
        public string VdataWoNo;
        public string VdataMfgDate;
        public string VdataExpDate;
        public string VdataQty;
        public string VdataWeight;
        public string VdataPrint;
        public string VdataWoNoTemp;
        public string VdataKetKemasan;
        public int vCounterOK;
        public int vCounterNG;
        public int vCounterLastReadCode;
        public int vCounterQtyWO;

        //variable Data PLC
        public string TX;
        public string FCS;
        public string RXD;

        public string txt_kirim;
        public string Txt_terima;


        SQLConfig config = new SQLConfig();
        usableFunction UsableFunction = new usableFunction();


        public delegate void AddDataDelegate(String myString);
        public AddDataDelegate myDelegate;

        delegate void SetLabel(string msg);

        void SetLabelMethod(string msg)
        {
            lbTimbang.Text = msg;

        }

        public void AddDataMethod(String myString)
        {
            textBox1.AppendText(myString);
        }



        private void SetLabelText1(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                label.Invoke((Action)(() => SetLabelText1(label, text)));
                return;
            }

            label.Text = text;
        }

        delegate void SetLabelDelegate(Label label, string text);
        private void SetLabelText(Label label, string text)
        {
            if (label.InvokeRequired)
            {
                label.Invoke(
                    new SetLabelDelegate(SetLabelText),
                    new object[] { label, text }
                );
                return;
            }

            label.Text = text;
        
        }

        delegate void SetTextCallback(string text);
        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (tbTemp.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                tbTemp.Text = text;
            }

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void panel10_Paint(object sender, PaintEventArgs e)
        {
            panel10.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel10.Width, panel10.Height, 20, 20));
        }

        private void progressBar2_Click(object sender, EventArgs e)
        {
            progressBar2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, progressBar2.Width, progressBar2.Height, 20, 20));
        }

        private void userAkses()
        {
            if ( lblUser.Text == "Administrator" || lblUser.Text == "Supervisor")
            {
                btnCloseWO.Visible = true;
            }
            else
            {
                btnCloseWO.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
            lblUser.Text = varGlobal.Username;

            userAkses();

            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            btnDataEdit.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnDataEdit.Width, btnDataEdit.Height, 20, 20));
            btnHistoryLabel.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnHistoryLabel.Width, btnHistoryLabel.Height, 20, 20));
            btnCloseWO.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnCloseWO.Width, btnCloseWO.Height, 20, 20));
            btnPause.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnPause.Width, btnPause.Height, 20, 20));
            btnStop.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnStop.Width, btnStop.Height, 20, 20));
            btnStart.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnStart.Width, btnStart.Height, 20, 20));
            btnBack.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnBack.Width, btnBack.Height, 20, 20));
            btnReadyPrint.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnReadyPrint.Width, btnReadyPrint.Height, 20, 20));

            load_DatacbWO();
            disab();
            varGlobal.GetNilai(varUtility.fileQtyCase);
            lbQtyCaseTarget.Text = Nilai.StringNilai;
            varGlobal.GetNilai(varUtility.fileLastData);
            lbTotalCase.Text = Nilai.StringNilai;
            lbStatusEcer.Text = varGlobal.lastBatch;

            this.myDelegate = new AddDataDelegate(AddDataMethod);

            //lbStatusEcer.Text = varGlobal.vStatusEcer;
            vCounterOK = 0;
            vCounterNG = 0;
            vCounterLastReadCode = 0;
            vCounterQtyWO = 0;
        }

        private void countDataAgregate()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT COUNT(id) FROM tblhistory_print WHERE wo_no='" + cbWO.Text + "' and kodeRecipe='" + VdataKodeRecipe + "' and STATUS=1 and status_agregation=1";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                lbTargetQty.Text = dr[0].ToString();
                VDataTarget = dr[0].ToString();


            }

            dr.Close();

            config.con.Close();
        }
        private void loadCountDataAvailable()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT COUNT(id) FROM tblhistory_print WHERE wo_no='" + cbWO.Text + "' and kodeRecipe='" + VdataKodeRecipe + "' and STATUS=1 and status_agregation=1";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                string CountData = dr[0].ToString();
                //VDataTarget = dr[0].ToString();
                int AvData = Convert.ToInt32(VDataTarget) - Convert.ToInt32(CountData);
                //VDataTarget = AvData.ToString();
                lbTargetQty.Text = AvData.ToString();

            }

            dr.Close();
            config.con.Close();
        }

        private void loadCountDataPrint()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT COUNT(id) FROM tblhistory_print WHERE wo_no='" + cbWO.Text + "' and kodeRecipe='"+ VdataKodeRecipe + "' and STATUS=1 and status_agregation=0";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                lbTargetQty.Text = dr[0].ToString();
                VDataTarget = dr[0].ToString();


            }

            dr.Close();
            config.con.Close();
        }

        private void checkDataWO()
        {
            int flag = 1;
            config.Init_Con();
            config.con.Open();
            string sql = "select wo_no from tblworkorder where wo_no='" + VdataWoNo +"' and kodeRecipe='" + VdataKodeRecipe + "' and status='" + flag + "'";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(sql, config.con);
            DataTable dt = new DataTable();
            dataAdapter1.Fill(dt);
            if (dt.Rows.Count != 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    VdataWoNoTemp = row[0].ToString();
                    
                }

                
            }
            else
            {

            }
            config.con.Close();
        }

        private void load_DatacbWO()
        {
            cbWO.Items.Clear();           
            cbWO.Items.Add(varGlobal.woNo);
            VdataKodeRecipe = varGlobal.dataKodeRecipe;
            
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            panel2.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel2.Width, panel2.Height, 20, 20));
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            panel3.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel3.Width, panel3.Height, 20, 20));
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {
            panel4.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel4.Width, panel4.Height, 20, 20));
        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {
            panel5.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel5.Width, panel5.Height, 20, 20));
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {
            panel6.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel6.Width, panel6.Height, 20, 20));
        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {
            panel7.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel7.Width, panel7.Height, 20, 20));
        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {
            panel8.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel8.Width, panel8.Height, 20, 20));
        }
        
        private void panel9_Paint(object sender, PaintEventArgs e)
        {
            panel9.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel9.Width, panel9.Height, 20, 20));
        }

        private void panel11_Paint(object sender, PaintEventArgs e)
        {
            panel11.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel11.Width, panel11.Height, 20, 20));
        }

        private void panel12_Paint(object sender, PaintEventArgs e)
        {
            //panel12.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel12.Width, panel12.Height, 20, 20));
        }

        private void panel13_Paint(object sender, PaintEventArgs e)
        {
            panel13.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel13.Width, panel13.Height, 20, 20));
        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {
            panel14.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel14.Width, panel14.Height, 20, 20));
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {
            progressBar1.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, progressBar1.Width, progressBar1.Height, 20, 20));
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        
        private void loadDataWO()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "select  * from tblworkorder where wo_no='" + cbWO.Text + "' and kodeRecipe='" + VdataKodeRecipe + "'";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                lbLotNo.Text = dr[5].ToString();
                lbProductName.Text = dr[4].ToString();
                VdataNoBatch = dr[4].ToString();
                //lbTargetQty.Text = dr[8].ToString();
                //VDataTarget = dr[8].ToString();
                Vdata_GTINCodeMatrix = dr[9].ToString();



            }
            dr.Close();
            config.con.Close();
        }

        private void get_qtyCartonRealease()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "select Max(countCarton) from tblcartonrealease where woNo='" + cbWO.Text + "'";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                lbCartonSuccesfull.Text = dr[0].ToString().PadLeft(3, '0');

            }
            dr.Close();
            config.con.Close();
        }

        private void get_qtyCarton()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "select cartonNo from tblhistory_printlabel where woNo='" + cbWO.Text + "' order by id desc limit 1";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                lbTotalCase.Text = dr[0].ToString().PadLeft(3, '0');
                
            }
            dr.Close();
            config.con.Close();


        }

        private void loadKetProd()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "select bentukSediaan from  tblproduk where kodeProduk='" + VdataProductKode + "'";
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

        private void loadDataForPrint()
        {
            string dataMfgDate_temp = string.Empty;
            string dataExpDate_temp = string.Empty;
            config.Init_Con();
            config.con.Open();
            string sql = "select * from  viewdatawo where kodeRecipe='" + VdataKodeRecipe + "'";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                VdataProductKode = dr[2].ToString();
                VdataProductName = dr[3].ToString();
                VdataNoBatch = dr[4].ToString();
                dataMfgDate_temp = dr[5].ToString();
                dataExpDate_temp = dr[6].ToString();
                



            }
            dr.Close();
            config.con.Close();
            //VdataQty = lbQtyCaseTarget.Text;
            string thn_MfgDate = dataMfgDate_temp.Substring(0, 4);
            string bln_MfgDate = dataMfgDate_temp.Substring(5, 2);
            string tgl_mfgDate = dataMfgDate_temp.Substring(8,2);
            VdataMfgDate = bln_MfgDate + "-" + thn_MfgDate;
            string thn_ExpDate = dataExpDate_temp.Substring(0, 4);
            string bln_ExpDate = dataExpDate_temp.Substring(5, 2);
            string tgl_ExpDate = dataExpDate_temp.Substring(8, 2);
            VdataExpDate =  bln_ExpDate + "-" + thn_ExpDate;
            VdataWoNo = cbWO.Text;
            VdataPrint = varGlobal.dataPrint;
        }

        private void cbWO_SelectedValueChanged(object sender, EventArgs e)
        {

            loadDataWO();
            loadCountDataPrint();
            loadKetProd();
            loadDataForPrint();
            kodeotomatis();
            get_qtyCarton();
            get_qtyCartonRealease();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cbWO.Text.Length != 0 )
            {
                Connect_COM();
                connect_Timbangan();
                //connect_Tower();
                enab();
            }
            else
            {
                MessageBox.Show("Silahkan Pilih WO terlebih dahulu", "Perhatian", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
            
        }

        private void disab()
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;
            btnPause.Enabled = false;
            btnStart.BackColor = Color.Gray;
            btnPause.BackColor = Color.Gray;
            btnStop.BackColor = Color.Gray;
            cbWO.Enabled = true;
            btnBack.Enabled = true;
        }
    

        private void enab()
        {
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            btnPause.Enabled = true;
            btnStart.BackColor = Color.Green;
            btnPause.BackColor = Color.Gray;
            btnStop.BackColor = Color.Gray;
            cbWO.Enabled = false;
            btnBack.Enabled = false;
        }

        private void closeTimbangan()
        {
            if (serialTimbangan.IsOpen == true)
            {
                serialTimbangan.Close();
            }
        }

        private void closeTowerLamp()
        {
            if (serialTowerLamp.IsOpen == true)
            {
                serialTowerLamp.Close();
            }
        }


        private void closeScanner()
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
            }

        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            closeTimbangan();
            closeScanner();
            //closeTowerLamp();
            disab();
        }

        private void GetData_ComSettingTimbangan()
        {
            varGlobal.GetValues(varUtility.fileComSettingTimbangan);
            vCOMportTimbangan = Values.StringCOM;
            vBaudRateTimbangan = Values.IntBaudRate;
            vDataBitsTimbangan = Values.IntDataBits;
            vStopBitsTimbangan = Values.IntStopBits;
            vParityTimbangan = Values.IntParity;

        }

        private void GetData_ComSettingCB()
        {
            varGlobal.GetValues(varUtility.fileComSettingCB);
            vCOMportCB = Values.StringCOM;
            vBaudRateCB = Values.IntBaudRate;
            vDataBitsCB = Values.IntDataBits;
            vStopBitsCB = Values.IntStopBits;
            vParityCB = Values.IntParity;

        }

        private void GetCOMSetting()
        {
            varGlobal.GetValues(varUtility.fileComSetting);
            VCOMPort = Values.StringCOM;
            VBaudrate = Values.IntBaudRate;
            VDataBits = Values.IntDataBits;
            VStopBits = Values.IntStopBits;
            VParity = Values.IntParity;

        }

        public void connect_Tower()
        {
            GetData_ComSettingCB();
            serialTowerLamp.PortName = vCOMportCB;
            serialTowerLamp.BaudRate = Convert.ToInt32(vBaudRateCB);
            serialTowerLamp.DataBits = Convert.ToInt32(vDataBitsCB);
            serialTowerLamp.StopBits = (StopBits)Enum.Parse(typeof(StopBits), vStopBitsCB);
            serialTowerLamp.Parity = (Parity)Enum.Parse(typeof(Parity), vParityCB);

            //tmrReaderSerialData.Interval = 1000;
            //tmrReaderSerialData.Enabled = true;

            if (!serialTowerLamp.IsOpen == true)
            {
                serialTowerLamp.Open();
                //P_Connected();

            }
            else
            {
                //P_Disconnected();
                MessageBox.Show("Error opening to serial port :: ", "Error!");
            }
        }

        public void connect_Timbangan()
        {
            GetData_ComSettingTimbangan();
            serialTimbangan.PortName = vCOMportTimbangan;
            serialTimbangan.BaudRate = Convert.ToInt32(vBaudRateTimbangan);
            serialTimbangan.DataBits = Convert.ToInt32(vDataBitsTimbangan);
            serialTimbangan.StopBits = (StopBits)Enum.Parse(typeof(StopBits), vStopBitsTimbangan);
            serialTimbangan.Parity = (Parity)Enum.Parse(typeof(Parity), vParityTimbangan);

            //tmrReaderSerialData.Interval = 1000;
            //tmrReaderSerialData.Enabled = true;

            if (!serialTimbangan.IsOpen == true)
            {
                serialTimbangan.Open();
                //T_Connected();

            }
            else
            {
                //T_Disconneted();
                MessageBox.Show("Error opening to serial port :: ", "Error!");
            }
        }

        private void Connect_COM()
        {
            if ( serialPort1.IsOpen == false)
            {
                try
                {
                    GetCOMSetting();
                    serialPort1.PortName = VCOMPort;
                    serialPort1.BaudRate = Convert.ToInt32(VBaudrate);
                    serialPort1.DataBits = Convert.ToInt32(VDataBits);
                    serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), VStopBits);
                    serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), VParity);

                    serialPort1.Open();
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private void lbLastReadCodeScan_TextChanged(object sender, EventArgs e)
        {

            
        }

        private async void  serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //ReadData = serialPort1.ReadExisting();
            //SetText(ReadData);
            //ReadData2 = ReadData.Substring(7, 4);
            //ReadData2 = ReadData.Substring(6, 6);
            //SetLabelText(lbLastReadCodeScan, ReadData);
            //SetText(ReadData);

            SerialPort sp = (SerialPort)sender;
            string s = sp.ReadExisting();
            string plainText = s.Replace("\n", "").Replace("\r", "");
            //string s = sp.ReadLine();
            await Task.Delay(50);
            textBox1.Invoke(this.myDelegate, new Object[] { plainText });
            
        }

        private void printLabel()
        {
            varGlobal.GetNilai(varUtility.filePrinterName);
            string vPrinterName = Nilai.StringNilai;
            varGlobal.GetNilai(varUtility.fileFileNamePrn);
            string vDataPrint = Nilai.StringNilai;

            VdataQty = lbActualQtyCase.Text;

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

            string SC1 = "^XA" +
                         "~TA000" +
                         "~JSN" +
                         "^LT0" +
                         "^MNW" +
                         "^MTD" +
                         "^PON" +
                         "^PMN" +
                         "^LH0,0" +
                         "^JMA" +
                         "^PR6,6" +
                         "~SD15" +
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
                         "^FO7,5^GB820,632,8^FS" +
                         //"^FT68,293^A0N,28,28^FH\"^CI28^FDProduct Code^FS^CI27" +
                         "^FT68,215^A0N,28,28^FH\"^CI28^FDBatch No^FS^CI27" +
                         "^FT68,262^A0N,28,28^FH\"^CI28^FDMfg Date^FS^CI27" +
                         "^FT68,310^A0N,28,28^FH\"^CI28^FDExp Date^FS^CI27" +
                         "^FT68,417^A0N,28,28^FH\"^CI28^FDQty^FS^CI27" +
                         "^FT68,367^A0N,28,28^FH\"^CI28^FDWeight^FS^CI27" +
                         //"^FT256,292^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT256,215^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT256,262^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT256,310^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT256,367^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT256,417^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         //"^FT290,293^A0N,28,28^FH\"^CI28^FD" + VdataProductKode + "^FS^CI27" +
                         "^FT290,215^A0N,28,28^FH\"^CI28^FD" + VdataNoBatch + "^FS^CI27" +
                         "^FT290,262^A0N,28,28^FH\"^CI28^FD" + VdataMfgDate + "^FS^CI27" +
                         "^FT290,310^A0N,28,28^FH\"^CI28^FD" + VdataExpDate + "^FS^CI27" +
                         "^FT290,417^A0N,28,28^FH\"^CI28^FD" + VdataQty + "^FS^CI27" +
                         "^FT290,367^A0N,28,28^FH\"^CI28^FD" + lbWeight.Text + "^FS^CI27" +
                         "^FT579,606^BXN,6,200,0,0,1,_,1" +
                         "^FH\"^FD" + VdataPrint + lb_idCarton.Text + "^FS" +
                         "^FT51,110^A0N,51,51^FH\"^CI28^FD" + VdataProductName + "^FS^CI27" +
                         //"^FT51,165^A0N,42,43^FH\"^CI28^FD" + VdataWoNo + "^FS^CI27" +
                         "^FT68,474^A0N,28,28^FH\"^CI28^FDCarton No.^FS^CI27" +
                         "^FT256,474^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT290,474^A0N,28,28^FH\"^CI28^FD" + lbTotalCase.Text + "^FS^CI27" +
                         "^PQ1,,,Y" +
                         "^XZ";
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
                          "^FT338,448^A0N,34,33^FH\"^CI28^FD" + lbWeight.Text + "^FS^CI27" + // Weight
                          "^FT37,494^BXN,8,200,0,0,1,_,1" +
                          "^FH\"^FD" + VdataPrint + lb_idCarton.Text + "^FS" + // Datamatrix
                          "^FT24,108^A0N,85,86^FH\"^CI28^FD" + VdataProductName + "^FS^CI27" + // Product Name
                          "^FT575,451^A0N,102,101^FH\"^CI28^FD" + lbTotalCase.Text + "^FS^CI27" + // No. karton
                          "^FT248,343^A0N,34,33^FH\"^CI28^FDMD^FS^CI27" +
                          "^FT338,343^A0N,34,33^FH\"^CI28^FD" + VdataMfgDate + "^FS^CI27" +  //MFG Date
                          "^FO7,5^GB820,632,8^FS" +
                          "^FO507,310^GB284,202,6^FS" +
                          "^FT32,173^A0N,51,51^FH\"^CI28^FD" + VdataKetKemasan +"^FS^CI27" +
                          "^PQ1,,,Y" +
                          "^XZ";
                          


            //RawPrinterHelper.SendFileToPrinter(vPrinterName, fileprn);
            RawPrinterHelper.SendStringToPrinter(vPrinterName, SC2);
            //}
            //}
            saveHistoryPrintLabel();
            saveCartonRealese();
            kodeotomatis();
        }

        private void saveHistoryPrintLabel()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "INSERT INTO `tblhistory_printlabel`(`woNo`, `productCode`, `productName`, `noBatch`, `expDate`, `qtyCarton`, `weightCarton`, `cartonNo`, `dataBarcode`)values('"+ VdataWoNo + "','" + VdataProductKode + "','" + VdataProductName + "', '" + VdataNoBatch + "', '" + VdataExpDate + "','" + VdataQty + "', '" + lbWeight.Text + "', '" + lbTotalCase.Text + "','" + VdataPrint + lb_idCarton.Text + "')";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            cmd.ExecuteNonQuery();
            config.con.Close();

        }

        private void btnDataEdit_Click(object sender, EventArgs e)
        {
            varGlobal.GetNilai(varUtility.filePrinterName);
            string vPrinterName = Nilai.StringNilai;
            varGlobal.GetNilai(varUtility.fileFileNamePrn);
            string vDataPrint = Nilai.StringNilai;

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
                        "^CI27" +
                        "^XZ" +
                        "^XA" +
                        "^FT284,292^A0N,28,28^FH\"^CI28^FDVProduct Name^FS^CI27" +
                        "^FT284,348^A0N,28,28^FH\"^CI28^FDVProduct Code^FS^CI27" +
                        "^FT284,401^A0N,28,28^FH\"^CI28^FDVBatch No^FS^CI27" +
                        "^FT284,444^A0N,28,28^FH\"^CI28^FDVExp Date^FS^CI27" +
                        "^FT284,494^A0N,28,28^FH\"^CI28^FDVQty^FS^CI27" +
                        "^FT284,552^A0N,28,28^FH\"^CI28^FDVWeight^FS^CI27" +
                        "^FT594,760^BXN,10,200,0,0,1,_,1" +
                        "^FH\"^FD90GKL0323406501A110B12321726011221585665D643844^FS" +
                        "^FT51,110^A0N,51,51^FH\"^CI28^FDVProduct Name^FS^CI27" +
                        "^FT51,165^A0N,42,43^FH\"^CI28^FDVWo Number^FS^CI27" +
                        "^PQ0,,,Y" +
                         "^XZ";

            string SC1 = "^XA" +
                         "~TA000" +
                         "~JSN" +
                         "^LT0" +
                         "^MNW" +
                         "^MTD" +
                         "^PON" +
                         "^PMN" +
                         "^LH0,0" +
                         "^JMA" +
                         "^PR6,6" +
                         "~SD15" +
                         "^JUS" +
                         "^LRN" +
                         "^CI27" +
                         "^PA0,1,1,0" +
                         "^XZ" +
                         "^XA" +
                         "^MMT" +
                         "^PW831" +
                         "^LL799" +
                         "^LS0" +
                         "^FO20,14^GB804,777,8^FS" +
                         "^FT68,292^A0N,28,28^FH\"^CI28^FDProduct Name^FS^CI27" +
                         "^FT68,348^A0N,28,28^FH\"^CI28^FDProduct Code^FS^CI27" +
                         "^FT68,393^A0N,28,28^FH\"^CI28^FDBatch No^FS^CI27" +
                         "^FT68,444^A0N,28,28^FH\"^CI28^FDExp Date^FS^CI27" +
                         "^FT68,494^A0N,28,28^FH\"^CI28^FDQty^FS^CI27" +
                         "^FT73,552^A0N,28,28^FH\"^CI28^FDWeight^FS^CI27" +
                         "^FT256,292^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT256,348^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT256,393^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT256,442^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT256,494^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT256,552^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT284,292^A0N,28,28^FH\"^CI28^FD" + VdataProductName + "^FS^CI27" +
                         "^FT284,348^A0N,28,28^FH\"^CI28^FD" + VdataProductKode + "^FS^CI27" +
                         "^FT284,401^A0N,28,28^FH\"^CI28^FD" + VdataNoBatch + "^FS^CI27" +
                         "^FT284,444^A0N,28,28^FH\"^CI28^FD" + VdataExpDate + "^FS^CI27" +
                         "^FT284,494^A0N,28,28^FH\"^CI28^FD"+ VdataQty +"^FS^CI27" +
                         "^FT284,552^A0N,28,28^FH\"^CI28^FD" + lbWeight.Text + "^FS^CI27" +
                         "^FT579,753^BXN,6,200,0,0,1,_,1" +
                         "^FH\"^FD"+ VdataPrint +"^FS" +
                         "^FT51,110^A0N,51,51^FH\"^CI28^FD"+ VdataProductName +"^FS^CI27" +
                         "^FT51,165^A0N,42,43^FH\"^CI28^FD" + VdataWoNo + "^FS^CI27" +
                         "^PQ1,,,Y" +
                         "^XZ";
            
            RawPrinterHelper.SendStringToPrinter(vPrinterName, SC1);
            //RawPrinterHelper.SendFileToPrinter(vPrinterName, fileprn);
            //}
            //}
        }

        private void serialTimbangan_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            ReadData = serialTimbangan.ReadLine();
            //ReadData = serialTimbangan.ReadExisting();
            //SetText(ReadData);
            ReadData2 = ReadData.Substring(7, 5);  //before (7,4)
            //ReadData2 = ReadData.Substring(6, 6);
            SetLabelText1(lbTimbang , ReadData2);

            //SetLabelText1(lbTimbang, ReadData);

            //SetLabelText(tbtimbang2, ReadData2);
        }

        private void lbTimbang_TextChanged(object sender, EventArgs e)
        {
            tbTimbang.Text = ReadData;
        }

        private void tbTimbang_TextChanged(object sender, EventArgs e)
        {
            //if (tbTimbang.Text != "")
            //{
            //    //lbTimbang.Text = tbTimbang.Text.Substring(8,6);
            //    //tbTimbang.Text = lbTimbang.Text;
            //    lbWeight.Text = ReadData2 + " KG";
            //    //lbWeight.Text = textBox1.Text + " KG";
            //    timbangan();
            //    lbTimbang.Text = "";


            //}

            //tbTimbang.Clear();
        }

        private void timbangan()
        {
            int vCounter;
            
            if (lbStatusEcer.Text == "true")
            {
                //getLastDataCount();
                dataStatus = "MS";
                vCounter = Convert.ToInt32(lbTotalCase.Text);
                vCounter = vCounter + 1;

                lbTotalCase.Text = (vCounter).ToString().PadLeft(3, '0');
                lbCartonSuccesfull.Text = lbTotalCase.Text;

                vCounterOK++;
                lbBoxGood.Text = (vCounterOK).ToString().PadLeft(3, '0');
                lbBoxGood.Text = lbBoxGood.Text;

                pnlOK.Visible = true;
                pnlNG.Visible = false;
                lbNotifWeight.Text = "";

                btnReadyPrint.Enabled = true;

                //dataCounter = lbCount.Text;
                //cetak_OK();
                //lampu_Hijau();
                //dataSimpan();
                //loadData();
                //saveLastDataCount();
            }
            else
            {
                ValidasiBerat();
                if (pnlOK.Visible == true)
                {
                    //getLastDataCount();
                    dataStatus = "MS";
                    //vCounterOK = Convert.ToInt32(lbBoxGood.Text);
                    //vCounterOK = vCounterOK + 1;
                    vCounterOK++;
                    lbBoxGood.Text = (vCounterOK).ToString().PadLeft(3, '0');
                    lbBoxGood.Text = lbBoxGood.Text;
                    //lbCount1.ForeColor = Color.Green;
                    //Thread.Sleep(3000);
                    //lbCount1.ForeColor = Color.Black;
                    //dataCounter = lbCount.Text;
                    //cetak_OK();
                    lampu_Hijau();
                    //dataSimpan();
                    //loadData();
                    //saveLastDataCount();
                    
                    tbScanBarcode.Focus();
                    vCounter = Convert.ToInt32(lbTotalCase.Text);
                    vCounter = vCounter + 1;
                    
                    lbTotalCase.Text = (vCounter).ToString().PadLeft(3, '0');
                    //printLabel();
                    btnReadyPrint.Enabled = true;
                    //closeTimbangan();
                }
                else
                {
                    //getLastDataCount();
                    dataStatus = "TMS";
                    //vCounterNG = Convert.ToInt32(lbBoxNG.Text);
                    //vCounterNG = vCounterNG + 1;

                    vCounterNG++;
                    lbBoxNG.Text = (vCounterNG).ToString().PadLeft(3, '0');
                    lbBoxNG.Text = lbBoxNG.Text;
                    //cetak_NG();
                    btnReadyPrint.Enabled = false;
                    //lampu_Merah();
                    //dataSimpan();
                    //loadData();


                }

                
            }

        }

        //========================== Validate Berat =======///
        private void ValidasiBerat()
        {
            varGlobal.GetNilai(varUtility.fileMinRange);
            Vdata_minRange = Nilai.StringNilai;
            varGlobal.GetNilai(varUtility.fileMaxRange);
            Vdata_maxRange = Nilai.StringNilai;
            string Vweight = lbWeight.Text.Substring(0, 5);  // before (0,4)
            double weight = Convert.ToDouble(Vweight);
            if (Convert.ToDouble(Vdata_minRange) <= weight && Convert.ToDouble(Vdata_maxRange) >= weight)
            {
                pnlOK.Visible = true;
                pnlNG.Visible = false;
                lbNotifWeight.Visible = false;
                lbNotifWeight.Text = "";
                //lbCount1.ForeColor = Color.Green;
                //Thread.Sleep(3000);
                //lbCount1.ForeColor = Color.Black;
            }
            else
            {
                pnlOK.Visible = false;
                pnlNG.Visible = true;
                lbNotifWeight.Visible = true;
                if(Convert.ToDouble(Vdata_minRange) >= weight)
                {
                    lbNotifWeight.Text = "Lower";
                    lampu_Merah();
                }
                else if (Convert.ToDouble(Vdata_maxRange) <= weight)
                {
                    lbNotifWeight.Text = "Upper";
                    lampu_Merah();
                }
            }


        }

        private void lbTimbang_TextChanged_1(object sender, EventArgs e)
        {

            
            tbtimbang2.Text = ReadData2;

        }

        private void tbtimbang2_TextChanged(object sender, EventArgs e)
        {
            if (tbtimbang2.Text != "")
            {
                //lbTimbang.Text = tbTimbang.Text.Substring(8,6);
                //tbTimbang.Text = lbTimbang.Text;
                lbWeight.Text = ReadData2 + " KG";
                //lbWeight.Text = textBox1.Text + " KG";
                timbangan();
                lbTimbang.Text = "";


            }

            tbtimbang2.Clear();
        }

        private async void tbLastReadCodeTemp_TextChanged(object sender, EventArgs e)
        {
            //tbLastReadCodeTemp.Text = lbLastReadCodeScan.Text;
            if(tbLastReadCodeTemp.Text != "")
            {
                progressBar1.Minimum = 0;
                progressBar1.Maximum = Convert.ToInt32(lbQtyCaseTarget.Text);

                progressBar2.Minimum = 0;
                progressBar2.Maximum = Convert.ToInt32(VDataTarget);

                //vCounterQtyWO++;
                //vCounterLastReadCode++;

                //progressBar2.Value = vCounterQtyWO;
                //lbActualQtyCase.Text = vCounterLastReadCode.ToString();
                //saveHistoryScan();
                //check_dataPrint();
                checkdataScan();
                if (vCounterLastReadCode == Convert.ToInt32(lbQtyCaseTarget.Text))
                {
                    pbCompleted.Visible = true;
                    //MessageBox.Show("Qty Box TerCapai, Silahkan Timbang");
                    lampu_Hijau();
                    await Task.Delay(1000);
                    pbCompleted.Visible = false;
                    vCounterLastReadCode = 0;
                    //SignalTimbang();
                    //connect_Timbangan();
                }
                //lbLastReadCodeScan.Text = "";
            }
            tbLastReadCodeTemp.Clear();
            
        }
        private void updateFlag()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "update tblhistory_scan set flag=1 where dataScan='" + lbLastReadCodeScan.Text + "' and flag=0";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            cmd.ExecuteNonQuery();
            config.con.Close();
        }
        
        private async void checkdataScan()
        {
            config.Init_Con();
            config.con.Open();
            string query = "select * from tblhistory_print where data_print='" + lbLastReadCodeScan.Text + "'";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(query, config.con);
            DataTable dt2 = new DataTable();
            dataAdapter1.Fill(dt2);

            if (dt2.Rows.Count != 0)
            {
                checkDoubleData();
            }
            else
            {
                
                //PB_Warning.Visible = true;

                MessageBox.Show("Data Tidak Terbaca Kamera", "Perhatian!!..");
                await Task.Delay(10);
                //PB_Warning.Visible = false;
                //saveHistoryScan();
            }
            config.con.Close();
        }
        private async void checkDoubleData()
        {
            int flagNo = 1;
            config.Init_Con();
            config.con.Open();
            string query = "select * from tblhistory_scan where dataScan='" + lbLastReadCodeScan.Text + "' and flag='" + flagNo + "'";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(query, config.con);
            DataTable dt = new DataTable();
            dataAdapter1.Fill(dt);

            if (dt.Rows.Count != 0)
            {
                //dt count != (not null) NG (Data Has ready Save)
                //PB_Warning.Visible = true;
                MessageBox.Show("Data Sudah Pernah Ter-scan", "Perhatian!!..");

                await Task.Delay(10);
                //PB_Warning.Visible = false;
                //updateFlag();
                //vCounterQtyWO++;
                //vCounterLastReadCode++;
                //saveHistoryScan();
                //update_DataAgregate();
                //loadCountDataAvailable();
                //lbActualQtyCase.Text = vCounterLastReadCode.ToString();
                //progressBar1.Value = vCounterLastReadCode;
                
            }
            else
            {
                //dt count == (null) OK (Data Save)
                vCounterQtyWO++;
                vCounterLastReadCode++;
                saveHistoryScan();
                update_DataAgregate();
                loadCountDataAvailable();
                lbActualQtyCase.Text = vCounterLastReadCode.ToString();
                progressBar1.Value = vCounterLastReadCode;
                progressBar2.Value = vCounterQtyWO;

                int hitungTarget = Convert.ToInt32(VDataTarget) - vCounterQtyWO;
                lbTargetQty.Text = hitungTarget.ToString();
                
               
                //Already
            }
            config.con.Close();
        }
        private async void check_dataPrint()
        {
            
            int flag = 1;
            config.Init_Con();
            config.con.Open();
            string sql = "select * from tblhistory_scan where dataScan='"+ lbLastReadCodeScan.Text +"' and flag='"+ flag +"'";
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(sql,config.con);
            DataTable dt3 = new DataTable();
            dataAdapter.Fill(dt3);
            if (dt3.Rows.Count == 0)
            {
                vCounterQtyWO++;
                vCounterLastReadCode++;
                saveHistoryScan();
                update_DataAgregate();
                loadCountDataAvailable();
                lbActualQtyCase.Text = vCounterLastReadCode.ToString();
                progressBar1.Value = vCounterLastReadCode;
                progressBar2.Value = vCounterQtyWO;

            }
            else
            {

                PB_Warning.Visible = true;
                
                //MessageBox.Show("Data Already Scanned", "Perhatian!!..");
                await Task.Delay(1000);
                PB_Warning.Visible = false;

            }
            config.con.Close();

        }

        private void update_DataAgregate()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "update tblhistory_print set status_agregation=1 where kodeRecipe='" + VdataKodeRecipe + "' and noBatch='" + VdataNoBatch + "' and data_print='" + lbLastReadCodeScan.Text + "'";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            cmd.ExecuteNonQuery();
            config.con.Close();
        }

        private void saveHistoryScan()
        {
            int flag = 1;
            config.Init_Con();
            config.con.Open();
            string sql = "INSERT INTO  `tblhistory_scan`(idCarton,woNo,noBatch,productName,dataScan,dateCreate,flag)values('" + lb_idCarton.Text + "','" + cbWO.Text + "','" + lbLotNo.Text + "','" + lbProductName.Text + "', '" + lbLastReadCodeScan.Text + "','" + DateTime.Now + "','" + flag + "')";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            cmd.ExecuteNonQuery();
            config.con.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            FormMain formMain = new FormMain();
            formMain.Show();
            this.Hide();
        }

        private void tbScanBarcode_TextChanged(object sender, EventArgs e)
        {
            //lbLastReadCodeRealese.Text = tbScanBarcode.Text;
            //tbScanBarcode.Text = "";
        }

        private void lbLastReadCodeRealese_TextChanged(object sender, EventArgs e)
        {
            //saveCartonRealese();
            //kodeotomatis();
            lbCartonSuccesfull.Text = lbTotalCase.Text;
            //tbScanBarcode.Text = "";
        }

        private void saveCartonRealese()
        {
            string vDataAction = "CREATE";
            string vDataActive = "TRUE";
            config.Init_Con();
            config.con.Open();
            string sql = "INSERT INTO  `tblcartonrealease`(idCarton,kodeRecipe, woNo,productName,noBatch,countCarton,dataScanRealese,dateCreate,action,is_Active)values('" + lb_idCarton.Text + "','" + VdataKodeRecipe + "','" + cbWO.Text + "','" + lbProductName.Text + "','" + lbLotNo.Text + "','" + lbTotalCase.Text + "', '" + VdataPrint + lb_idCarton.Text + "','" + DateTime.Now + "','" + vDataAction + "','" + vDataActive + "')";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            cmd.ExecuteNonQuery();
            config.con.Close();
        }

        private void tbTemp_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void tbTemp_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {

            }
        }

        private void tbTemp_TextChanged_1(object sender, EventArgs e)
        {
            tbLastReadCodeTemp.Text = ReadData;
        }

        private void tbScanBarcode_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == 13)
            //{
                lbLastReadCodeRealese.Text = tbScanBarcode.Text;
            //}
        }

        public void SignalTimbang()
        {
            if (serialPort1.IsOpen == true)
            {
                //serialTimbangan.ReadTimeout = 3000;
                serialTimbangan.Write("Q" + CR + LF);
                //tbTimbang.Text = serialPort1.ReadLine();
            }
        }

        public void lampu_Merah()
        {
            txt_kirim = "00SC02";
            WritePLC();
            txt_kirim = "00WD" + "1000" + "0001";
            WritePLC();
            //if (serialTowerLamp.IsOpen == true)
            //{
            //    serialTowerLamp.Write("NG");
            //}
        }

        public void lampu_Hijau()
        {
            txt_kirim = "00SC02";
            WritePLC();
            txt_kirim = "00WD" + "0100" + "0001";
            WritePLC();
            //if (serialTowerLamp.IsOpen == true)
            //{
            //    serialTowerLamp.Write("OK");
            //}
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void kodeotomatis()
        {
            int tambahsatu;
            string kode;
            config.Init_Con();
            config.con.Open();
            MySqlCommand cmd = new MySqlCommand("select idCarton from tblcartonrealease where idCarton in(select max(idCarton) from tblcartonrealease) order by idCarton desc", config.con);
            MySqlDataReader sdr = cmd.ExecuteReader();
            sdr.Read();
            if (sdr.HasRows)
            {
                tambahsatu = Convert.ToInt32(sdr[0].ToString().Substring(sdr[0].ToString().Length - 4, 4)) + 1;
                string gabung = "0000" + tambahsatu;
                kode = "CR" +  gabung.Substring(gabung.ToString().Length - 4, 4);
            }
            else
            {
                kode = "CR" +  "0001";
            }
            sdr.Close();
            lb_idCarton.Text = kode;
            config.con.Close();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            btnPause.BackColor = Color.Green;
            btnStart.BackColor = Color.Gray;
            btnStop.BackColor = Color.Gray;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            closeScanner();
            closeTimbangan();
            closeTowerLamp();
            

        }

        private void tbScanBarcode_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                lbLastReadCodeRealeseTemp.Text = tbScanBarcode.Text;
                //MessageBox.Show("TES");
                tbScanBarcode.Text = "";
            }
        }

        private void lbLastReadCodeRealeseTemp_TextChanged(object sender, EventArgs e)
        {

        }

        private void lbLastReadCodeRealeseTemp_TextChanged_1(object sender, EventArgs e)
        {
            lbLastReadCodeRealese.Text = lbLastReadCodeRealeseTemp.Text;
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void updateFlagZero()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "update tblhistory_scan set flag=0 where idCarton='" + lb_idCarton.Text + "' and flag=1";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            cmd.ExecuteNonQuery();
            config.con.Close();
        }
        private void btnRevise_Click(object sender, EventArgs e)
        {
            //updateFlagZero();
            loadKetProd();
            printLabel();
            //saveCartonRealese();
            //kodeotomatis();
            btnReadyPrint.Enabled = false;
            tbScanBarcode.Focus();
        }

        private void updateFlagWO()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "update tblworkorder set status=1 where wo_no='" + cbWO.Text + "' and kodeRecipe='" + VdataKodeRecipe + "'";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            cmd.ExecuteNonQuery();
            config.con.Close();
        }

        private void btnCloseWO_Click(object sender, EventArgs e)
        {
            updateFlagWO();
            MessageBox.Show("Data WO telah ter-Close","Perhatian");
        }

        private void btnHistoryLabel_Click(object sender, EventArgs e)
        {
            FormHistoryLabel formHistoryLabel = new FormHistoryLabel();
            formHistoryLabel.Show();
            this.Close();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private async void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text != "")
            {

                progressBar1.Minimum = 0;
                progressBar1.Maximum = Convert.ToInt32(lbQtyCaseTarget.Text);

                progressBar2.Minimum = 0;
                progressBar2.Maximum = Convert.ToInt32(VDataTarget);

                lbLastReadCodeScan.Text = textBox1.Text;
                checkdataScan();
                if (vCounterLastReadCode == Convert.ToInt32(lbQtyCaseTarget.Text))
                {
                    pbCompleted.Visible = true;
                    MessageBox.Show("Qty Box TerCapai, Silahkan Timbang");
                    await Task.Delay(1000);
                    pbCompleted.Visible = false;
                    vCounterLastReadCode = 0;
                    //SignalTimbang();
                    //connect_Timbangan();
                }

            }

            textBox1.Clear();
        }

        private void WritePLC()
        {
            //string bufferRXD = null;
            //string buffer_TX = null;
            connect_Tower();
            //"00WD" + "1010" + "000A"
            TX = "@" + txt_kirim;
            GetFCS();
            lbTX.Text = TX + FCS + "*";
            communicate();

            serialTowerLamp.Close();
            lbRXD.Text = RXD;
        }


        private void GetFCS()
        {
            //This will calculate the FCS value for the communications
            int L = 0;
            string A = null;
            string TJ = null;
            L = TX.Length;
            A = "0";
            for (var J = 1; J <= L; J++)
            {
                TJ = TX.Substring(J - 1, 1);
                A = (Strings.Asc(TJ) ^ Convert.ToInt32(A)).ToString();

            }
            FCS = Convert.ToString(Convert.ToInt64(A), 16).ToUpper();
            if (FCS.Length == 1)
            {
                FCS = "0" + FCS;
            }
        }
        private void GetFCSOld()
        {
            //This will calculate the FCS value for the communications
            int L = 0;
            string A = null;
            string TJ = null;
            L = TX.Length;
            A = "0";
            for (var J = 1; J <= L; J++)
            {
                TJ = TX.Substring(J - 1, 1);
                A = (Microsoft.VisualBasic.Strings.Asc(TJ) ^ Convert.ToInt32(A)).ToString();

            }
            FCS = Convert.ToString(Convert.ToInt64(A), 16).ToUpper();
            if (FCS.Length == 1)
            {
                FCS = "0" + FCS;
            }
        }

        private void communicate()
        {
            //This will communicate to the Omron PLC
            string BufferTX = null;
            string fcs_rxd = null;
            try
            {
                RXD = "";
                BufferTX = TX + FCS + "*" + "\r";
                //Send the information out the serial port
                serialTowerLamp.Write(BufferTX);
                //Sleep for 50 msec so the information can be sent on the port
                System.Threading.Thread.Sleep(50);
                //Set the timeout for the serial port at 100 msec
                serialTowerLamp.ReadTimeout = 100;
                //Read up to the carriage return
                RXD = (serialTowerLamp.ReadTo("\r"));
            }
            catch (Exception)
            {
                //If an error occurs then indicate communication error
                RXD = "Communication Error";
            }
            //Get the FCS of the returned information
            fcs_rxd = RXD.Substring(RXD.Length - 3, 2);
            if (RXD.Substring(0, 1) == "@")
            {
                TX = RXD.Substring(0, RXD.Length - 3);
            }
            else if (RXD.Substring(2, 1) == "@")
            {
                TX = RXD.Substring(2, RXD.Length - 5);
                RXD = RXD.Substring(2, RXD.Length - 1);
            }
            //Check the FCS of the return information. If they are not the same then an error has occurred.
            GetFCS();
            if (FCS != fcs_rxd)
            {
                RXD = "Communication Error";
            }

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString();
        }

        private void cbLastBatch_CheckedChanged(object sender, EventArgs e)
        {
            if (cbLastBatch.Checked == true)
            {
                //tbJmlBotol.Enabled = true;
                varGlobal.lastBatch = "true";
                lbStatusEcer.Text = "true";
                //pnlBotol.Visible = true;
            }
            else
            {
                //tbJmlBotol.Enabled = false;
                varGlobal.lastBatch = "false";
                lbStatusEcer.Text = "false";
                //pnlBotol.Visible = false;
            }
        }

        private void label3_Click_1(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            lampu_Merah();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lampu_Hijau();
        }
    }
}
