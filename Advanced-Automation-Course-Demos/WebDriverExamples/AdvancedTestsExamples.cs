using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System;
using WebDriverExamples.Pages;

namespace WebDriverExamples
{
    [TestFixture]
    public class AdvancedTestsExamples
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void TestInit()
        {
            var profile = new FirefoxProfile();
            profile.AddExtension(@"C:\FirefoxCustomExtensions\ad_blocker.xpi");
            this.driver = new FirefoxDriver(profile);
            this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(value: 30));
        }

        [TearDown]
        public void TestCleanup()
        {
            this.driver.Quit();
        }
    }
}
