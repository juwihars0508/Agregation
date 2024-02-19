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
    public class RawPrinterHelper
    {
        // Structure and API declarions:
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public class DOCINFOA
        {
            [MarshalAs(UnmanagedType.LPStr)] public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)] public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)] public string pDataType;
        }
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool OpenPrinter([MarshalAs(UnmanagedType.LPStr)] string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", EntryPoint = "ClosePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartDocPrinterA", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, Int32 level, [In, MarshalAs(UnmanagedType.LPStruct)] DOCINFOA di);

        [DllImport("winspool.Drv", EntryPoint = "EndDocPrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "StartPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "EndPagePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", EntryPoint = "WritePrinter", SetLastError = true, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern bool WritePrinter(IntPtr hPrinter, IntPtr pBytes, Int32 dwCount, out Int32 dwWritten);

        // SendBytesToPrinter()
        // When the function is given a printer name and an unmanaged array
        // of bytes, the function sends those bytes to the print queue.
        // Returns true on success, false on failure.
        public static bool SendBytesToPrinter(string szPrinterName, IntPtr pBytes, Int32 dwCount)
        {
            Int32 dwError = 0, dwWritten = 0;
            IntPtr hPrinter = new IntPtr(0);
            DOCINFOA di = new DOCINFOA();
            bool bSuccess = false; // Assume failure unless you specifically succeed.

            di.pDocName = "My C#.NET RAW Document";
            di.pDataType = "RAW";

            // Open the printer.
            if (OpenPrinter(szPrinterName.Normalize(), out hPrinter, IntPtr.Zero))
            {
                // Start a document.
                if (StartDocPrinter(hPrinter, 1, di))
                {
                    // Start a page.
                    if (StartPagePrinter(hPrinter))
                    {
                        // Write your bytes.
                        bSuccess = WritePrinter(hPrinter, pBytes, dwCount, out dwWritten);
                        EndPagePrinter(hPrinter);
                    }
                    EndDocPrinter(hPrinter);
                }
                ClosePrinter(hPrinter);
            }
            // If you did not succeed, GetLastError may give more information
            // about why not.
            if (bSuccess == false)
            {
                dwError = Marshal.GetLastWin32Error();
            }
            return bSuccess;
        }

        public static bool SendFileToPrinter(string szPrinterName, string szFileName)
        {
            // Open the file.
            FileStream fs = new FileStream(szFileName, FileMode.Open);
            // Create a BinaryReader on the file.
            BinaryReader br = new BinaryReader(fs);
            // Dim an array of bytes big enough to hold the file's contents.
            Byte[] bytes = new Byte[fs.Length];
            bool bSuccess = false;
            // Your unmanaged pointer.
            IntPtr pUnmanagedBytes = new IntPtr(0);
            int nLength;

            nLength = Convert.ToInt32(fs.Length);
            // Read the contents of the file into the array.
            bytes = br.ReadBytes(nLength);
            // Allocate some unmanaged memory for those bytes.
            pUnmanagedBytes = Marshal.AllocCoTaskMem(nLength);
            // Copy the managed byte array into the unmanaged array.
            Marshal.Copy(bytes, 0, pUnmanagedBytes, nLength);
            // Send the unmanaged bytes to the printer.
            bSuccess = SendBytesToPrinter(szPrinterName, pUnmanagedBytes, nLength);
            // Free the unmanaged memory that you allocated earlier.
            Marshal.FreeCoTaskMem(pUnmanagedBytes);
            return bSuccess;
        }
        public static bool SendStringToPrinter(string szPrinterName, string szString)
        {
            IntPtr pBytes;
            Int32 dwCount;
            // How many characters are in the string?
            dwCount = szString.Length;
            // Assume that the printer is expecting ANSI text, and then convert
            // the string to ANSI text.
            pBytes = Marshal.StringToCoTaskMemAnsi(szString);
            // Send the converted ANSI string to the printer.
            SendBytesToPrinter(szPrinterName, pBytes, dwCount);
            Marshal.FreeCoTaskMem(pBytes);
            return true;
        }
    }


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
        public static string fileMinRange = @"Data\MinRange.txt";
        public static string fileMaxRange = @"Data\MaxRange.txt";
        public static string fileQtyCase = @"Data\QtyCase.txt";
        public static string fileLastData = @"Data\LastData.txt";
        public static string fileDataPrint = @"Data\AgragationLabel.prn";
        public static string filePrinterName = @"Data\PrinterName.txt";
        public static string fileFileNamePrn = @"Data\AgregationFileName.txt";
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
        public static string lastBatch;
        public static string woNo;
        public static string dataKodeRecipe;

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
