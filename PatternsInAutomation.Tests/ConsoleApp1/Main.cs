using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.IO;
using AutoDataVPBank.Beginners.Pages.FecreditPage;
using OpenQA.Selenium.Firefox;

namespace AutoDataVPBank
{
    public partial class Main : Form
    {
        public IWebDriver Driver { get; set; }
        public WebDriverWait Wait { get; set; }
        public Main()
        {
            InitializeComponent();
        }


        [TestInitialize]
        public void SetupDriver()
        {
            //var chromeDriverService = ChromeDriverService.CreateDefaultService();
            //chromeDriverService.HideCommandPromptWindow = true;
            //string curFile = @"C:\extensions\0.0.10_0.crx";
            //var options = new ChromeOptions();
            //if (File.Exists(curFile))
            //{
            //    options.AddExtension(Path.GetFullPath(curFile));
            //}
            //this.Driver = new ChromeDriver(chromeDriverService, options);
            var firefoxDriverService = FirefoxDriverService.CreateDefaultService();
            firefoxDriverService.HideCommandPromptWindow = true;
            this.Driver = new FirefoxDriver(firefoxDriverService);
            this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(30));
        }

        [TestCleanup]
        public void Teardown()
        {
            this.Driver.Quit();
        }

        [TestMethod]
        public void LoginFinnOne()
        {
            FecreditLoginPage loginPage = new FecreditLoginPage(this.Driver);
            loginPage.Navigate();
            loginPage.Login(txtUser.Text,txtPass.Text,txtSignFo.Text,txtSignTo.Text,cboActive.Text, this.cboActive.Text);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.txtUser.Text = @"CC100278";
            this.txtPass.Text = @"Khoinguyen@2";
            this.txtSignFo.Text = @"02/05/2016";
            this.txtSignTo.Text = @"02/05/2016";
            this.cboActive.SelectedItem = "Detail Data Entry";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetupDriver();
            LoginFinnOne();
            Teardown();
//            this.Close();
        }
       
    }
}
