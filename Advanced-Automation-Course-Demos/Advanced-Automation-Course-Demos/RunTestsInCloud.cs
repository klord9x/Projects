using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System;
using OpenQA.Selenium.Remote;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;

namespace Advanced_Automation_Course_Demos
{
    [TestClass]
    public class RunTestsInCloud
    {
        public string username = "gencho2020@yahoo.com";
        public string authkey = "u7ca82558b33a017";
        private IWebDriver driver;

        [TestInitialize]
        public void SetupTest()
        {
            // Start by setting the capabilities
            var caps = new DesiredCapabilities();
            caps.SetCapability("name", "Basic Example");
            caps.SetCapability("build", "1.0");

            ////caps.SetCapability("browser_api_name", "Chrome52");
            ////caps.SetCapability("os_api_name", "Win7x64-C1");
            ////caps.SetCapability("screen_resolution", "1920x1080");

            // Not working- there is a bug.
            caps.SetCapability("browser_api_name", "FF46");
            caps.SetCapability("os_api_name", "Mac10.11");
            caps.SetCapability("screen_resolution", "1400x900");


            caps.SetCapability("record_video", "true");
            caps.SetCapability("record_network", "true");
            caps.SetCapability("username", username);
            caps.SetCapability("password", authkey);

            // Start the remote webdriver
            this.driver =
                ////new ChromeDriver();
                new RemoteWebDriver(new Uri("http://hub.crossbrowsertesting.com:80/wd/hub"), caps, TimeSpan.FromSeconds(180));
        }

        [TestCleanup]
        public void TeardownTest()
        {
            this.driver.Quit();
        }

        [TestMethod]
        public void ScrollFocusToControl_InCloud_ShouldFail()
        {
            this.driver.Navigate().GoToUrl(@"http://automatetheplanet.com/compelling-sunday-14022016/");
            IWebElement link = driver.FindElement(By.PartialLinkText("Previous post"));
            string jsToBeExecuted = string.Format("window.scroll(0, {0});", link.Location.Y);
            ((IJavaScriptExecutor) driver).ExecuteScript(jsToBeExecuted);
            link.Click();
            Assert.AreEqual<string>("10 Advanced WebDriver Tips and Tricks - Part 1", driver.Title);
        }

        [TestMethod]
        public void ScrollFocusToControl_InCloud_ShouldPass()
        {
            this.driver.Navigate().GoToUrl(@"https://automatetheplanet.com/multiple-files-page-objects-item-templates/");
            IWebElement link = driver.FindElement(By.PartialLinkText("TFS Test API"));
            string jsToBeExecuted = string.Format("window.scroll(0, {0});", link.Location.Y);
            ((IJavaScriptExecutor) driver).ExecuteScript(jsToBeExecuted);

            var wait = new WebDriverWait(driver, TimeSpan.FromMinutes(1));
            var clickableElement = wait.Until(ExpectedConditions.ElementToBeClickable(By.PartialLinkText("TFS Test API")));

            clickableElement.Click();
            Assert.AreEqual<string>("TFS Test API Archives - Automate The Planet", driver.Title);
        }
    }
}
