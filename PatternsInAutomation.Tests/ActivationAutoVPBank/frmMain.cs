using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Windows.Forms;
using AutoDataVPBank;
using QLicense;

namespace ActivationAutoVPBank
{
    public partial class FrmMain : Form
    {
        private byte[] _certPubicKeyData;
        private SecureString _certPwd = new SecureString();

        public FrmMain()
        {
            InitializeComponent();

            _certPwd.AppendChar('d');
            _certPwd.AppendChar('e');
            _certPwd.AppendChar('m');
            _certPwd.AppendChar('o');
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //Read public key from assembly
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (MemoryStream mem = new MemoryStream())
            {
                assembly.GetManifestResourceStream("ActivationAutoVPBank.LicenseSign.pfx")?.CopyTo(mem);

                _certPubicKeyData = mem.ToArray();
            }

            //Initialize the path for the certificate to sign the XML license file
            licSettings.CertificatePrivateKeyData = _certPubicKeyData;
            licSettings.CertificatePassword = _certPwd;

            //Initialize a new license object
            licSettings.License = new MyLicense(); 
        }

        private void licSettings_OnLicenseGenerated(object sender, QLicense.Windows.Controls.LicenseGeneratedEventArgs e)
        {
            //Event raised when license string is generated. Just show it in the text box
            licString.LicenseString = e.LicenseBASE64String;
        }


        private void btnGenSvrMgmLic_Click(object sender, EventArgs e)
        {
            //Event raised when "Generate License" button is clicked. 
            //Call the core library to generate the license
            licString.LicenseString = LicenseHandler.GenerateLicenseBASE64String(
                new MyLicense(),
                _certPubicKeyData,
                _certPwd);
        }

    }
}
