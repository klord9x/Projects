using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using System;
using WebDriverCloudLoadTesting.Pages.AutomateThePlanet;

namespace WebDriverCloudLoadTesting
{
    [TestClass]
    public class AutomateThePlanetTest
    {
        private IWebDriver driver;
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void SetupTest()
        {
            this.driver = new PhantomJSDriver();
            this.driver.Manage().Timeouts().ImplicitlyWait(new TimeSpan(0, 0, 30));
        }

        [TestCleanup]
        public void TeardownTest()
        {
            this.driver.Quit();
        }

        [TestMethod]
        public void TestInTheCloud()
        {
            var homePage = new HomePage(this.driver);
            this.TestContext.BeginTimer("Automate The Planet Home Page- Navigate");
            homePage.Navigate();
            this.TestContext.EndTimer("Automate The Planet Home Page- Navigate");
            homePage.AssertHeadline();
            this.TestContext.BeginTimer("Automate The Planet- Go to Blog");
            homePage.GoToBlog();
            var blogPage = new BlogPage(this.driver);
            blogPage.WaitForSubscribeWidget();
            this.TestContext.EndTimer("Automate The Planet- Go to Blog");
            blogPage.AssertTitle();
        }
    }
}
