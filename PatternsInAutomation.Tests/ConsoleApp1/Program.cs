﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using PatternsInAutomation.Tests.Beginners;
using PatternsInAutomation.Tests.Beginners.Selenium.Bing.Pages;
using System;
using POP = PatternsInAutomation.Tests.Beginners.Pages.BingMainPage;

namespace ConsoleApp1
{
    class Program
    {
        public IWebDriver Driver { get; set; }
        public WebDriverWait Wait { get; set; }

        static void Main(string[] args)
        {
            Program a = new Program();
            a.SetupTest();

            a.SearchTextInBing_First();
        }


        [TestInitialize]
        public void SetupTest()
        {
            this.Driver = new FirefoxDriver();
            this.Wait = new WebDriverWait(this.Driver, TimeSpan.FromSeconds(30));
        }

        [TestCleanup]
        public void TeardownTest()
        {
            this.Driver.Quit();
        }

        [TestMethod]
        public void SearchTextInBing_First()
        {
            BingMainPage bingMainPage = new BingMainPage(this.Driver);
            bingMainPage.Navigate();
            bingMainPage.Search("Automate The Planet");
            bingMainPage.ValidateResultsCount("340,000 RESULTS");
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
    }
}
