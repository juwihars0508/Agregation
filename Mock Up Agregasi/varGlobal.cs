using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Windows.Forms;

namespace Mock_Up_Agregasi
{

    class varUtility
    {
        public static string fileIpAddress = @"Data\IPAddress_String.txt";
        public static string filePort = @"Data\Port_String.txt";
        public static string fileIpAddressCamera = @"Data\IPAddressCamera_String.txt";
        public static string filePortCamera = @"Data\PortCamera_String.txt";
        public static string filePrefixString = @"Data\DataPrefix_String.txt";
        public static string fileComSetting = @"Data\ComSetting.txt";
        public static string fileComSettingTimbangan = @"Data\ComSettingTimbangan.txt";
        public static string fileComSettingCB = @"Data\ComSettingCB.txt";
        //public static string fileLineProcess = @"Data\Flag_String.txt";
        //public static string fileBulanProduksi = @"Data\BulanProduksi.txt";
        //public static string fileBulan10 = @"Data\Bulan10.txt";
        //public static string fileBulan11 = @"Data\Bulan11.txt";
        //public static string fileBulan12 = @"Data\Bulan12.txt";
        //public static string fileMingguPertama = @"Data\MingguPertama.txt";
        //public static string fileMingguKedua = @"Data\MingguKedua.txt";
        //public static string fileMingguKetiga = @"Data\MingguKetiga.txt";
        //public static string fileLineCellAssy = @"Data\LineCellAssy.txt";



    }

    class varRounded
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

        public varRounded(Form region, int width, int Height, int round1, int round2)
        {
            region.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, width, Height, round1, round2));
        }
    }

    class varDataState
    {
        public string ID { get; set; }
        public string Name { get; set; }

    }
    class varGlobal
    {
        public static string alamatIPServer;
        public static int port;
        public static string terimapesandariserver;
        public static string terimapesandiserver;
        public static string pesankoneksi;
        public static string statuskoneksi;        
        public static string Username;
        public static string product;
        public static string dataPrint;
        public static string qtyCase;
        public static string qtyTargetInner;
        public static string stdWieght;
        public static string minWeight;
        public static string maxWeight;

        public static Nilai GetNilai(string path)
        {
            Nilai valuesColletion = new Nilai();
            using (var f = new StreamReader(path))
            {
                string line = string.Empty;
                while ((line = f.ReadLine()) != null)
                {
                    var parts = line;
                    valuesColletion = new Nilai(parts);
                }
            }

            return valuesColletion;
        }

        public static Nilai SaveNilai(string path)
        {
            Nilai valuesCollection = new Nilai();
            using (var f = new StreamWriter(path))
            {
                string line = string.Empty;
                if (line != null)
                {
                    line = (Nilai.StringNilai);
                    f.Write(line, FileMode.Append);
                    f.Flush();
                }

            }
            return valuesCollection;
        }

        public static List<Values> GetValues(string path)
        {
            List<Values> valuesCollection = new List<Values>();

            using (var f = new StreamReader(path))
            {
                string line = string.Empty;
                while ((line = f.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    valuesCollection.Add(new Values(parts[0], parts[1], parts[2], parts[3], parts[4]));
                }
            }

            return valuesCollection;
        }

        public static List<Values> SaveValues(string path)
        {
            List<Values> valuesCollection = new List<Values>();

            using (var f = new StreamWriter(path))
            {
                string line = string.Empty;


                if (line != null)
                {
                    //var parts = line.Split(',');

                    line = (Values.StringCOM + "," + Values.IntBaudRate + "," + Values.IntDataBits + "," + Values.IntStopBits + "," + Values.IntParity);
                    f.Write(line, FileMode.Append);
                    f.Flush();
                }


            }

            return valuesCollection;
        }

    }

    class Nilai
    {
        public static string StringNilai { get; set; }

        public Nilai()
        {

        }

        public Nilai(string S_Nilai)
        {
            StringNilai = S_Nilai;
        }
    }

    class Values
    {
        public static string StringCOM { get; set; }
        public static string IntBaudRate { get; set; }
        public static string IntDataBits { get; set; }
        public static string IntStopBits { get; set; }
        public static string IntParity { get; set; }

        public Values()
        {
        }

        public Values(string S_COMName, string i_Baudrate, string i_DataBits, string i_StopBits, string i_Parity)
        {
            StringCOM = S_COMName;
            IntBaudRate = i_Baudrate;
            IntDataBits = i_DataBits;
            IntStopBits = i_StopBits;
            IntParity = i_Parity;
        }
    }
}
