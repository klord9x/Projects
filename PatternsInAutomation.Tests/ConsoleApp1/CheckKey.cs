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
            HardDrive hd = new HardDrive();
            ProcessorID cpuID = new ProcessorID();
            string hddSerial = hd.SerialHDD();
            string processorID = cpuID.GetProcessorID();
            return processorID + hddSerial;
        }
        public static bool checkSerial(string key)
        {
            return true;
            try
            {
                string server = DecryptRijndael(Properties.Resources.Server);
                string user = DecryptRijndael(Properties.Resources.User);
                string passUser = DecryptRijndael(Properties.Resources.Pass);
                string conection = @"Data Source=" + server + ";Initial Catalog=" + user + ";Persist Security Info=true;User ID=" + user + ";Password=" + passUser + ";";

                string query = @"select * from CUSTOMER where  Keycode = N'" + key + "' and CustomerID=N'" + Properties.Resources.Customer + "'";
                SqlConnection myConnection = new SqlConnection(conection);
                myConnection.Open();
                SqlDataAdapter myAdapter = new SqlDataAdapter(query, myConnection);
                DataSet ds = new DataSet();
                myAdapter.Fill(ds);
                myConnection.Close();
                return ds.Tables[0].Rows.Count > 0 ? true : false;

            }
            catch
            {
                System.Windows.Forms.MessageBox.Show("Lối xác minh! Vui lòng kiểm tra!");
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
                List<HardDrive> lh = new List<HardDrive>();
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                foreach (ManagementObject wmi_HD in searcher.Get())
                {
                    HardDrive hd = new HardDrive();
                    if (wmi_HD["SerialNumber"] == null)
                        hd.SerialNo = "None";
                    else
                    {
                        hd.SerialNo = Convert.ToString(wmi_HD["SerialNumber"]);
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
                string sProcessorID = "";
                string sQuery = "SELECT ProcessorId FROM Win32_Processor";
                ManagementObjectSearcher oManagementObjectSearcher = new ManagementObjectSearcher(sQuery);
                ManagementObjectCollection oCollection = oManagementObjectSearcher.Get();
                foreach (ManagementObject oManagementObject in oCollection)
                {
                    sProcessorID = Convert.ToString(oManagementObject["ProcessorId"]);
                }
                return (sProcessorID.Trim());
            }
        }
    }
}
