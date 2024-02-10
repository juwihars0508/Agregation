using System;
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
        public string VdataExpDate;
        public string VdataQty;
        public string VdataWeight;
        public string VdataPrint;
        public string VdataWoNoTemp;
        public int vCounterOK;
        public int vCounterNG;
        public int vCounterLastReadCode;
        public int vCounterQtyWO;


        SQLConfig config = new SQLConfig();
        usableFunction UsableFunction = new usableFunction();

        delegate void SetLabel(string msg);

        void SetLabelMethod(string msg)
        {
            lbTimbang.Text = msg;

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

        private void Form1_Load(object sender, EventArgs e)
        {
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
            btnRevise.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btnRevise.Width, btnRevise.Height, 20, 20));

            load_DatacbWO();
            disab();
            varGlobal.GetNilai(varUtility.fileQtyCase);
            lbQtyCaseTarget.Text = Nilai.StringNilai;
            varGlobal.GetNilai(varUtility.fileLastData);
            lbTotalCase.Text = Nilai.StringNilai;
            
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
                VDataTarget = AvData.ToString();
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
            config.Init_Con();
            config.con.Open();
            string sql = "SELECT DISTINCT wo_no, kodeRecipe FROM tblhistory_print WHERE STATUS=1";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                VdataWoNo = dr[0].ToString();
                VdataKodeRecipe = dr[1].ToString();
                checkDataWO();
                if (VdataWoNo != VdataWoNoTemp)
                {
                    cbWO.Items.Add(dr[0].ToString());
                }
                else
                {
                    cbWO.Items.Clear(); 
                }


            }

            dr.Close();
            config.con.Close();

            
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

        private void loadDataForPrint()
        {
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
                VdataExpDate = dr[5].ToString();
                



            }
            dr.Close();
            config.con.Close();
            VdataQty = lbQtyCaseTarget.Text;
            VdataWoNo = cbWO.Text;
            VdataPrint = varGlobal.dataPrint;
        }

        private void cbWO_SelectedValueChanged(object sender, EventArgs e)
        {

            loadDataWO();
            loadCountDataPrint();
            loadDataForPrint();
            kodeotomatis();
           
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
                connect_Tower();
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
            closeTowerLamp();
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

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            ReadData = serialPort1.ReadExisting();
            //SetText(ReadData);
            //ReadData2 = ReadData.Substring(7, 4);
            //ReadData2 = ReadData.Substring(6, 6);
            SetLabelText(lbLastReadCodeScan, ReadData);
            SetText(ReadData);
        }

        private void printLabel()
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
                         "^FT284,494^A0N,28,28^FH\"^CI28^FD" + VdataQty + "^FS^CI27" +
                         "^FT284,552^A0N,28,28^FH\"^CI28^FD" + lbWeight.Text + "^FS^CI27" +
                         "^FT579,753^BXN,6,200,0,0,1,_,1" +
                         "^FH\"^FD" + VdataPrint + lb_idCarton.Text + "^FS" +
                         "^FT51,110^A0N,51,51^FH\"^CI28^FD" + VdataProductName + "^FS^CI27" +
                         "^FT51,165^A0N,42,43^FH\"^CI28^FD" + VdataWoNo + "^FS^CI27" +
                         "^FT68,607^A0N,28,28^FH\"^CI28^FDCarton No.^FS^CI27" +
                         "^FT256,607^A0N,28,28^FH\"^CI28^FD:^FS^CI27" +
                         "^FT290,607^A0N,28,28^FH\"^CI28^FD" + lbTotalCase.Text + "^FS^CI27" +
                         "^PQ1,,,Y" +
                         "^XZ";

            //RawPrinterHelper.SendFileToPrinter(vPrinterName, fileprn);
            RawPrinterHelper.SendStringToPrinter(vPrinterName, SC1);
            //}
            //}
            saveHistoryPrintLabel();
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
            //SetText(ReadData);
            ReadData2 = ReadData.Substring(7, 4);
            //ReadData2 = ReadData.Substring(6, 6);
            SetLabelText1(lbTimbang , ReadData2);
            
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

                //dataCounter = lbCount.Text;
                //cetak_OK();
                lampu_Hijau();
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
                    printLabel();
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
                    btnRevise.Visible = true;
                    lampu_Merah();
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
            string Vweight = lbWeight.Text.Substring(0, 4);
            double weight = Convert.ToDouble(Vweight);
            if (Convert.ToDouble(Vdata_minRange) < weight && Convert.ToDouble(Vdata_maxRange) > weight)
            {
                pnlOK.Visible = true;
                pnlNG.Visible = false;
                //lbCount1.ForeColor = Color.Green;
                //Thread.Sleep(3000);
                //lbCount1.ForeColor = Color.Black;
            }
            else
            {
                pnlOK.Visible = false;
                pnlNG.Visible = true;
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
                progressBar2.Maximum = Convert.ToInt32(lbTargetQty.Text);

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
                    await Task.Delay(1000);
                    pbCompleted.Visible = false;
                    vCounterLastReadCode = 0;
                    SignalTimbang();
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
        
        private void checkdataScan()
        {
            config.Init_Con();
            config.con.Open();
            string query = "select * from tblhistory_scan where dataScan='" + lbLastReadCodeScan.Text + "'";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(query, config.con);
            DataTable dt2 = new DataTable();
            dataAdapter1.Fill(dt2);

            if (dt2.Rows.Count != 0)
            {
                checkflag();
            }
            else
            {
                vCounterQtyWO++;
                vCounterLastReadCode++;
                saveHistoryScan();
                update_DataAgregate();
                loadCountDataAvailable();
                lbActualQtyCase.Text = vCounterLastReadCode.ToString();
                progressBar1.Value = vCounterLastReadCode;
                progressBar2.Value = vCounterQtyWO;
                //saveHistoryScan();
            }
            config.con.Close();
        }
        private async void checkflag()
        {
            int flagNo = 0;
            config.Init_Con();
            config.con.Open();
            string query = "select * from tblhistory_scan where dataScan='" + lbLastReadCodeScan.Text + "' and flag='" + flagNo + "'";
            MySqlDataAdapter dataAdapter1 = new MySqlDataAdapter(query, config.con);
            DataTable dt = new DataTable();
            dataAdapter1.Fill(dt);

            if (dt.Rows.Count != 0)
            {
                updateFlag();
                vCounterQtyWO++;
                vCounterLastReadCode++;
                //saveHistoryScan();
                //update_DataAgregate();
                //loadCountDataAvailable();
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
            saveCartonRealese();
            kodeotomatis();
            lbCartonSuccesfull.Text = lbTotalCase.Text;
            //tbScanBarcode.Text = "";
        }

        private void saveCartonRealese()
        {
            string vDataAction = "CREATE";
            string vDataActive = "True";
            config.Init_Con();
            config.con.Open();
            string sql = "INSERT INTO  `tblcartonrealease`(idCarton,kodeRecipe, woNo,productName,noBatch,countCarton,dataScanRealese,dateCreate,action,is_Active)values('" + lb_idCarton.Text + "','" + VdataKodeRecipe + "','" + cbWO.Text + "','" + lbProductName.Text + "','" + lbLotNo.Text + "','" + lbTotalCase.Text + "', '" + lbLastReadCodeRealese.Text + "','" + DateTime.Now + "','" + vDataAction + "','" + vDataActive + "')";
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
            if (serialTowerLamp.IsOpen == true)
            {
                serialTowerLamp.Write("NG");
            }
        }

        public void lampu_Hijau()
        {
            if (serialTowerLamp.IsOpen == true)
            {
                serialTowerLamp.Write("OK");
            }
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
                kode = "CR" + VdataNoBatch + gabung.Substring(gabung.ToString().Length - 4, 4);
            }
            else
            {
                kode = "CR" + VdataNoBatch + "0001";
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
            updateFlagZero();
            btnRevise.Visible = false;
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

        }
    }
}
