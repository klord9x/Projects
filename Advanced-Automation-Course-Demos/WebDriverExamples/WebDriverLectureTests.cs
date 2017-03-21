using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDriverExamples
{
    [TestFixture]
    public class WebDriverLectureTests
    {
        private IWebDriver driver;

        [SetUp]
        public void TestInit()
        {
            this.driver = new ChromeDriver();
        }


        [TearDown]
        public void TestCleanup()
        {
            this.driver.Quit();
        }

        [Test]
        public void NavigateToMavenDownloadPage()
        {
            // Act 1
            this.driver.Navigate().GoToUrl(@"http://www.seleniumhq.org/docs/03_webdriver.jsp#introducing-the-selenium-webdriver-api-by-example");
            var settingUpLink = this.driver.FindElement(By.LinkText("Setting Up a Selenium-WebDriver Project"));
            settingUpLink.Click();

            //Assert 1
            var currentActualUrl = this.driver.Url;
            Assert.AreEqual("http://www.seleniumhq.org/docs/03_webdriver.jsp#setting-up-a-selenium-webdriver-project", this.driver.Url);

            // Act 2
            var mavenDownloadLink = this.driver.FindElement(By.LinkText("Maven download page"));
            mavenDownloadLink.Click();

            // Assert 2
            currentActualUrl = this.driver.Url;
            Assert.AreEqual("http://www.seleniumhq.org/download/maven.jsp", currentActualUrl);
        }
    }
}
