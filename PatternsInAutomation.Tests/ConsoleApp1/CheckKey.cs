using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Securitys;
using System.Data.SqlClient;
using System.Data;
using System.Management;

namespace AutoDataVPBank
{
    public static class CheckKey
    {
        public static string getSerial()
        {
            var hd = new HardDrive();
            var cpuID = new ProcessorID();
            var hddSerial = hd.SerialHDD();
            var processorID = cpuID.GetProcessorID();
            return processorID + hddSerial;
        }
        public static bool checkSerial(string key)
        {
            return true;
            try
            {
                var server = DecryptRijndael(Properties.Resources.Server);
                var user = DecryptRijndael(Properties.Resources.User);
                var passUser = DecryptRijndael(Properties.Resources.Pass);
                var conection = @"Data Source=" + server + ";Initial Catalog=" + user + ";Persist Security Info=true;User ID=" + user + ";Password=" + passUser + ";";

                var query = @"select * from CUSTOMER where  Keycode = N'" + key + "' and CustomerID=N'" + Properties.Resources.Customer + "'";
                var myConnection = new SqlConnection(conection);
                myConnection.Open();
                var myAdapter = new SqlDataAdapter(query, myConnection);
                var ds = new DataSet();
                myAdapter.Fill(ds);
                myConnection.Close();
                return ds.Tables[0].Rows.Count > 0 ? true : false;

            }
            catch
            {
                System.Windows.Forms.MessageBox.Show(@"Lối xác minh! Vui lòng kiểm tra!");
                return false;
            }
        }
        private static string DecryptRijndael(string inputString)
        {
            try
            {
                return Securitys.RijndaelManagedEncryption.DecryptRijndael(inputString, Properties.Resources.SaltString);
            }
            catch
            {
                //MessageBox.Show(ex.Message);
                return "";
            }
        }
        private class HardDrive
        {
            public string Model { get; set; }
            public string Type { get; set; }
            public string SerialNo { get; set; }
            public string SerialHDD()
            {
                var lh = new List<HardDrive>();
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    var hd = new HardDrive();
                    if (wmi_HD["SerialNumber"] == null)
                        hd.SerialNo = "None";
                    else
                    {
                        hd.SerialNo = Convert.ToString(wmi_HD["SerialNumber"]).Trim();
                        lh.Add(hd);
                    }
                }
                return lh.Count() > 0 ? lh[0].SerialNo : "";
            }
        }
        private class ProcessorID
        {
            public string GetProcessorID()
            {
                var sProcessorID = "";
                var sQuery = "SELECT ProcessorId FROM Win32_Processor";
                var oManagementObjectSearcher = new ManagementObjectSearcher(sQuery);
                var oCollection = oManagementObjectSearcher.Get();
                foreach (ManagementObject oManagementObject in oCollection)
                {
                    sProcessorID = Convert.ToString(oManagementObject["ProcessorId"]);
                }
                return (sProcessorID.Trim());
            }
        }
    }
}
