using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Management;
using System.Windows.Forms;
using System.Globalization;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Security.AccessControl;

namespace EasyBypass
{
    internal class Program
    {
        public static string getHWID()
        {
            string HWID = System.Security.Principal.WindowsIdentity.GetCurrent().User.Value.Replace("-", "");

            var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            ManagementObjectCollection mbsList = mbs.Get();
            string id = "";

            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorId"].ToString();
                break;
            }
            string aux = HWID + id, hwid_xor = "";
            for (int i = 0; i < aux.Length; i++)
                hwid_xor += aux[i] ^ 0xA;

            return hwid_xor.Replace("1", "i").Replace("4", "A").Replace("3", "e").Replace("5", "S");
        }

        [STAThread]
        static void Main(string[] args)
        {
            string html = string.Empty;
            string url = @"https://github.com/HMCaetano0/FFH4X/blob/master/FFH4X.txt";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.AutomaticDecompression = DecompressionMethods.GZip;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
                html = reader.ReadToEnd();

            if (html.Contains(getHWID()))
            {
                Console.WriteLine("Acessado!");
                Bypass();
            }
            else
            {
                Console.WriteLine("Acesso negado, verifique sua licença");

            }
        }

        public static void Bypass()
        {
            //===================================================
            string[] Prefetch = Directory.GetFiles(@"C:\Windows\prefetch");
            foreach (var file in Prefetch)
            {
                if (file.Contains("A-R"))
                    File.Delete(file);
                if (file.Contains("a-r"))
                    File.Delete(file);
            }

            //===================================================
            string command = "/C fsutil usn deletejournal /D C:";
            Process.Start("cmd.exe", command);



        }
    }
}
