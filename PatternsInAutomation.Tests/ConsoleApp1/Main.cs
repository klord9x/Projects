﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using PatternsInAutomation.Tests.Beginners;
using PatternsInAutomation.Tests.Beginners.Selenium.Bing.Pages;
using System;
using AutoDataVPBank.Beginners.Pages.FecreditLogin;
using POP = PatternsInAutomation.Tests.Beginners.Pages.BingMainPage;
using OpenQA.Selenium.Chrome;
using System.IO;
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
        public void SetupTest()
        {
            System.Environment.SetEnvironmentVariable("webdriver.chrome.driver",
                @"C:/chromedriver_win32/chromedriver.exe");
            var options = new ChromeOptions();
            options.AddExtension(Path.GetFullPath(@"C:\extensions\0.0.10_0.crx"));
            //            var driver = new ChromeDriver(options);
            this.Driver = new ChromeDriver(options);
            this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(30));
            //            String mainHandle = this.Driver.CurrentWindowHandle;
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

        [TestMethod]
        public void SearchTextInBing_Second()
        {
            POP.BingMainPage bingMainPage = new POP.BingMainPage(this.Driver);
            bingMainPage.Navigate();
            bingMainPage.Search("Automate The Planet");
            bingMainPage.Validate().ResultsCount("264,000 RESULTS");
        }

        [TestMethod]
        public void ClickEveryHrefMenu()
        {
            this.Driver.Navigate().GoToUrl(@"http://www.telerik.com/");
            // get the menu div
            var menuList = this.Driver.FindElement(By.Id("GeneralContent_T73A12E0A142_Col01"));
            // get all links from the menu div
            var menuHrefs = menuList.FindElements(By.ClassName("Bar-menu-link"));

            // Now start clicking and navigating back
            foreach (var currentHref in menuHrefs)
            {
                this.Driver.Navigate().GoToUrl(@"http://www.telerik.com/");
                currentHref.Click();
                string currentElementHref = currentHref.GetAttribute("href");
                Assert.IsTrue(this.Driver.Url.Contains(currentElementHref));
                // Now the same will happen for the next href
            }
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
            SetupTest();
            LoginFinnOne();
            this.Close();
        }

    }
}
