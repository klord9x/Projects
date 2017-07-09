﻿using System;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using System.IO;
using AutoDataVPBank.Beginners.Pages.FecreditPage;

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
            var chromeDriverService = ChromeDriverService.CreateDefaultService();
            chromeDriverService.HideCommandPromptWindow = true;
            var options = new ChromeOptions();
            options.AddExtension(Path.GetFullPath(@"C:\extensions\0.0.10_0.crx"));
            this.Driver = new ChromeDriver(chromeDriverService, options);
            this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(30));
        }

        [TestCleanup]
        public void TeardownTest()
        {
            this.Driver.Quit();
        }

        [TestMethod]
        public void LoginFinnOne()
        {
            FecreditLoginPage loginPage = new FecreditLoginPage(this.Driver);
            loginPage.Navigate();
            loginPage.Login(txtUser.Text,txtPass.Text,txtSignFo.Text,txtSignTo.Text,cboActive.Text);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.txtUser.Text = "CC100278";
            this.txtPass.Text = "Khoinguyen@2";
            this.txtSignFo.Text = @"02/05/2016";
            this.txtSignTo.Text = @"02/05/2016";
            this.cboActive.SelectedItem = "Detail Data Entry";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetupDriver();
            LoginFinnOne();
            TeardownTest();
            this.Close();
        }
       
    }
}
