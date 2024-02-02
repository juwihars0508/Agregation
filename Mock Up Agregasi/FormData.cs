using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
        public FormData()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        public string VDataTarget;
        public string Vdata_GTINCodeMatrix;
        public string VdataKodeRecipe;

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

        public string ReadData;
        public string ReadData2;

        SQLConfig config = new SQLConfig();
        usableFunction UsableFunction = new usableFunction();

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

            load_DatacbWO();
            disab();
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

                cbWO.Items.Add(dr[0].ToString());
                VdataKodeRecipe = dr[1].ToString();


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
            panel12.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, panel12.Width, panel12.Height, 20, 20));
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
                lbTargetQty.Text = dr[8].ToString();
                VDataTarget = dr[8].ToString();
                //Vdata_GTINCodeMatrix = dr[5].ToString();



            }
            dr.Close();
            config.con.Close();
        }

        private void loadDataTarget()
        {
            config.Init_Con();
            config.con.Open();
            string sql = "select DISTINCT * from  tblworkorder where wo_no='" + cbWO.Text + "'";
            MySqlCommand cmd = new MySqlCommand(sql, config.con);
            MySqlDataReader dr;
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {

                lbLotNo.Text = dr[3].ToString();
                lbProductName.Text = dr[4].ToString();
                //lbTargetQty.Text = dr[8].ToString();
                //VDataTarget = dr[8].ToString();
                Vdata_GTINCodeMatrix = dr[5].ToString();



            }
            dr.Close();
            config.con.Close();
        }

        private void cbWO_SelectedValueChanged(object sender, EventArgs e)
        {

            loadDataWO();
           
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cbWO.Text.Length != 0 )
            {
                Connect_COM();
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
            btnPause.BackColor = Color.Yellow;
            btnStop.BackColor = Color.Red;
            cbWO.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == true)
            {
                serialPort1.Close();
            }
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
        }

        private void btnDataEdit_Click(object sender, EventArgs e)
        {
            DataEditAgg dataEditAgg = new DataEditAgg();
            dataEditAgg.Show();
            this.Hide();
        }
    }
}
