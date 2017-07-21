using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.IO;
using AutoDataVPBank.Beginners.Pages.FecreditPage;
using OpenQA.Selenium.Firefox;
using System.Globalization;
using static AutoDataVPBank.Beginners.Pages.FecreditPage.Safe;

namespace AutoDataVPBank
{
    public partial class Main : Form
    {
        public IWebDriver Driver { get; set; }
        public WebDriverWait Wait { get; set; }
        private string _serial = "";
        public Main()
        {
            InitializeComponent();
        }


        [TestInitialize]
        public void SetupDriver()
        {
            if (cboBrowser.SelectedItem.ToString() == "Chrome")
            {
                var chromeDriverService = ChromeDriverService.CreateDefaultService();
                //chromeDriverService.HideCommandPromptWindow = true;
                string curFile = @"C:\extensions\0.0.10_0.crx";
                var options = new ChromeOptions();
                if (File.Exists(curFile))
                {
                    options.AddExtension(Path.GetFullPath(curFile));
                }
                this.Driver = new ChromeDriver(chromeDriverService, options);
            }
            else
            {
                var firefoxDriverService = FirefoxDriverService.CreateDefaultService();
                //firefoxDriverService.FirefoxBinaryPath = Path.GetFullPath(@"C:\Program Files\Mozilla Firefox\firefox.exe");
                //FirefoxBinary firefoxBinary = new FirefoxBinary(Path.GetFullPath(@"C:\Program Files\Mozilla Firefox\firefox.exe"));
                //FirefoxBinary firefoxBinary = new FirefoxBinary(Path.GetFullPath(@"C:\Program Files\Nightly\firefox.exe"));
                //FirefoxProfile firefoxProfile = new FirefoxProfile();
                //firefoxDriverService.HideCommandPromptWindow = true;
                this.Driver = new FirefoxDriver(firefoxDriverService);
            }

            this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(30));
        }

        [TestCleanup]
        public void Teardown()
        {
            this.Driver.Quit();
        }

        
        private void Main_Load(object sender, EventArgs e)
        {
            //getSerial
            _serial = CheckKey.getSerial();
            this.txtSerial.Text = _serial;
            //
            //this.labContactMe.Text = "Contact me: CÔNG TY TNHH CÔNG NGHỆ METAFAT";
            //this.labEmail.Text = "Email: metafatvn@gmail.com - Phone: 0896892998";
            //

            this.cboActive.Items.AddRange(new object[] {
            "Select",
            "Reject Review"});
            this.txtUser.Text = @"CC100278";
            this.txtPass.Text = @"Khoinguyen@2";
            this.txtSignFo.Text = @"01/07/2017";
            this.txtSignTo.Text = @"03/07/2017";
            this.cboActive.SelectedItem = "Reject Review";
            this.cboBrowser.SelectedItem = "Firefox";
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (!CheckKey.checkSerial(_serial))
            {
                Application.Exit();
                return;
            }
            try
            {
                DateTime dateAsignFrom = DateTime.ParseExact(this.txtSignFo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                DateTime dateAsignTo = DateTime.ParseExact(this.txtSignTo.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                if (dateAsignTo.Subtract(dateAsignFrom).TotalDays >= 30 || dateAsignFrom >= dateAsignTo)
                {
                    MessageBox.Show("Lỗi ngày tháng!");
                    return;
                }
            }
            catch
            {
                MessageBox.Show("Lối thông tin nhâp!");
                return;
            }
            string text = this.btnRun.Text;
            if (text == "RUN")
            {
                //this.btnRun.Text = "STOP";
                SetupDriver();
                Safe.LoginFinnOne(this.Driver, txtSignFo.Text, txtSignTo.Text, cboActive.Text, txtUser.Text, txtPass.Text, this.radioButtonCAS.Checked, 0, null);
                Teardown();
                this.Close();
            }
            if (text == "STOP")
            {
                this.btnRun.Text = "RUN";
            }
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)//
        {
            //MessageBox.Show(e.KeyCode.ToString());
            if (e.Control && e.KeyCode == System.Windows.Forms.Keys.K)
                this.txtSerial.Visible = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.txtSerial.Visible = true;
        }

    }
}
