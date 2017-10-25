using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using AutoDataVPBank.core;
using AutoDataVPBank.Pages.FecreditPage;
using QLicense;
using static AutoDataVPBank.Library;

namespace AutoDataVPBank
{
    public partial class MainForm : Form
    {
        private readonly string _serial = @"admin@123";
        private static MainForm _inst;
        private static readonly FecreditLoginPage FecreditLoginPage = FecreditLoginPage.GetInstance;
        private readonly string _uniqKey = FingerPrint.Value();
        byte[] _certPubicKeyData;

        public MainForm()
        {
            InitializeComponent();
            // Initialize log4net.
            log4net.Config.XmlConfigurator.Configure();
        }

        public static MainForm GetInstance
        {
            get
            {
                if (_inst == null || _inst.IsDisposed)
                    _inst = new MainForm();
                return _inst;
            }
        }

        private void Main_Load(object sender, EventArgs e)
        {
            DefaultData();
            CheckLicense();
        }

        private void DefaultData()
        {
            //getSerial
            //_serial = CheckKey.getSerial();
            //Logg.Debug("Serial Key:" + FingerPrint.Value());
            txtSerial.Text = @"Please input serial here...";
            //
            labContactMe.Text = @"Contact me: CÔNG TY TNHH CÔNG NGHỆ METAFAT";
            labEmail.Text = @"Email: metafatvn@gmail.com - Phone: 0896892998";
            //
            var listProduct = new List<Product>
            {
                new Product {Val = "AUTO", Dis = "PRODUCT CATEGORY FOR AUTO LOANS"},
                new Product {Val = "PERSONAL", Dis = "PRODUCT CATEGORY FOR PERSONAL LOANS"}
            };
            cboProDuct.DataSource = listProduct.ToList();
            cboProDuct.DisplayMember = "dis";
            cboProDuct.ValueMember = "val";
            cboActive.Items.AddRange(new object[]
            {
                "Select",
                "Reject Review"
            });

            txtUser.Text = @"CC100278";
            txtPass.Text = @"Duyminh@2";
            txtSignFo.Text = @"01/10/2017";
            txtSignTo.Text = @"03/10/2017";
            cboActive.SelectedItem = "Reject Review";
            cboBrowser.SelectedItem = "Chrome"; //Firefox
            cboProDuct.SelectedValue = "AUTO";
            GetSetting();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //if (!CheckKey.checkSerial(_serial))
                //{
                //    Application.Exit();
                //    return;
                //}
                //TODO: Check serial 
                if (!CheckSerial())
                {
                    SendMail(_uniqKey);
                    return;
                }
                try
                {
                    var dateAsignFrom =
                        DateTime.ParseExact(txtSignFo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    var dateAsignTo = DateTime.ParseExact(txtSignTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (dateAsignTo.Subtract(dateAsignFrom).TotalDays >= 30 || dateAsignFrom >= dateAsignTo)
                    {
                        MessageBox.Show(@"Lỗi ngày tháng!");
                        return;
                    }
                }
                catch
                {
                    MessageBox.Show(@"Lối thông tin nhâp!");
                    Logg.Error(@"Lối thông tin nhâp!");
                    return;
                }
                var text = btnRun.Text;
                switch (text)
                {
                    case "Start":
                        FecreditLoginPage.Start();
                        break;
                    default:
                        BaseWorker.Stop();
                        break;
                }
            }
            catch (Exception exception)
            {
                Logg.Error(exception.Message);
                throw;
            }
            
        }

        private bool CheckSerial()
        {
            try
            {
                var serial = txtSerial.Text;
                if (serial == _serial)
                {
                    return true;
                }
                if (serial.Length == _uniqKey.Length)
                {
                    if (serial == _uniqKey) return true;
                    MessageBox.Show(@"Serial INVALID!");
                    return false;
                }
                MessageBox.Show(@"Serial INCORRECT!");
                return false;
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }

        private bool CheckLicense()
        {
            //Initialize variables with default values
            MyLicense _lic = null;
            string _msg = string.Empty;
            LicenseStatus _status = LicenseStatus.UNDEFINED;

            //Read public key from assembly
            Assembly _assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream _mem = new MemoryStream())
            {
                _assembly.GetManifestResourceStream("AutoDataVPBank.LicenseVerify.cer")?.CopyTo(_mem);

                _certPubicKeyData = _mem.ToArray();
            }

            //Check if the XML license file exists
            if (File.Exists("license.lic"))
            {
                _lic = (MyLicense)LicenseHandler.ParseLicenseFromBASE64String(
                    typeof(MyLicense),
                    File.ReadAllText("license.lic"),
                    _certPubicKeyData,
                    out _status,
                    out _msg);
            }
            else
            {
                _status = LicenseStatus.INVALID;
                _msg = "Your copy of this application is not activated";
            }

            switch (_status)
            {
                case LicenseStatus.VALID:

                    //TODO: If license is valid, you can do extra checking here
                    //TODO: E.g., check license expiry date if you have added expiry date property to your license entity
                    //TODO: Also, you can set feature switch here based on the different properties you added to your license entity 

                    //Here for demo, just show the license information and RETURN without additional checking       
                    licInfo.ShowLicenseInfo(_lic);

                    return true;

                default:
                    //for the other status of license file, show the warning message
                    //and also popup the activation form for user to activate your application
                    MessageBox.Show(_msg, string.Empty, MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    using (FrmActivation frm = new FrmActivation())
                    {
                        frm.CertificatePublicKeyData = _certPubicKeyData;
                        frm.ShowDialog();

                        //Exit the application after activation to reload the license file 
                        //Actually it is not nessessary, you may just call the API to reload the license file
                        //Here just simplied the demo process

                        Application.Exit();
                    }
                    break;
            }
            return false;
        }

        private void Main_KeyDown(object sender, KeyEventArgs e) //
        {
            //MessageBox.Show(e.KeyCode.ToString());
            if (e.Control && e.KeyCode == Keys.K)
                txtSerial.Visible = true;
        }
    }
}
