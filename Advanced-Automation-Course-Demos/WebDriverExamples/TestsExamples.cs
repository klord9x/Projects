using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using WebDriverExamples.Pages;

namespace WebDriverExamples
{
    [TestFixture]
    public class TestsExamples
    {
        private IWebDriver driver;
        private WebDriverWait wait;

        [SetUp]
        public void TestInit()
        {
            this.driver = new ChromeDriver();
            this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(value: 30));
        }

        [TearDown]
        public void TestCleanup()
        {
            this.driver.Quit();
        }

        [Test]
        public void GettingStartedWebDriver()
        {
            driver.Navigate().GoToUrl("http://www.telerik.com/");

            IWebElement yourAccountLink = driver.FindElement(By.Id("hlYourAccount"));

            Assert.AreEqual("Your Account", yourAccountLink.Text);
        }

        [Test]
        public void TestAllWebElements()
        {
            driver.Navigate().GoToUrl("https://automatetheplanet.com/healthy-diet-menu-generator");

            var additionalSugarCheckbox = driver.FindElement(By.Id("ninja_forms_field_18"));
            additionalSugarCheckbox.Click();

            var ventiRadioButton = driver.FindElement(By.Name("ninja_forms_field_19"));
            additionalSugarCheckbox.Click();

            var burgersSelect = new SelectElement(driver.FindElement(By.Name("ninja_forms_field_21")));
            burgersSelect.SelectByText("10 x Double Cheeseburgers");

            var firstNameTextInput = driver.FindElement(By.XPath("//*[@id='ninja_forms_field_23']"));
            firstNameTextInput.Clear();
            firstNameTextInput.SendKeys("Anton");

            var mainHeadline = driver.FindElement(By.TagName("h2"));

            // Assert
            Assert.AreEqual("Awesome Healthy Menu Generator", mainHeadline.Text);
        }

        [Test]
        public void TestAllWebElements_PageObjects()
        {
            var page = new DietMenuGeneratorPage(this.driver);
            page.Navigate();
            page.PopulateMenu("10 x Double Cheeseburgers", "Anton");

            page.AssertHeadline();
        }

        [Test]
        public void TestLoginGoogle()
        {
            this.driver.Navigate().GoToUrl("https://accounts.google.com/ServiceLogin");
            var emailInput = this.driver.FindElement(By.Id("Email"));
            emailInput.Clear();
            emailInput.SendKeys("gencho2021@gmail.com");
            var nextBtn = this.driver.FindElement(By.Id("next"));
            nextBtn.Click();

            var passInput = wait.Until(ExpectedConditions.ElementExists(By.Id("Passwd")));
            passInput.Clear();
            passInput.SendKeys("asdfgg12345");
            var signInBtn = this.driver.FindElement(By.Id("signIn"));
            signInBtn.Click();
            var welcomeLabel = this.driver.FindElement(By.XPath("//*[contains(text(),'Welcome,')]"));

            // Assert
            Assert.AreEqual("Welcome, Gencho Genchev", welcomeLabel.Text);

            driver.Navigate().GoToUrl("https://calendar.google.com/calendar");
            var arrowSmallBtn = driver.FindElement(By.XPath("//*[contains(text(),'▼')]"));
            arrowSmallBtn.Click();
            // .FindElement(By.ClassName("qab-container gcal-popup"))
            var quickAddInput = driver.FindElement(By.TagName("textarea"));
            string uniqueMeetingName = Guid.NewGuid().ToString();
            quickAddInput.SendKeys(uniqueMeetingName);
            var quickAddBtn = driver.FindElement(By.XPath("//div[contains(text(),'Add') and @class='goog-imageless-button-content']"));
            quickAddBtn.Click();

            var saveMeetingBtn = wait.Until(ExpectedConditions.ElementExists(By.XPath("//div[contains(text(),'Save')]")));
            saveMeetingBtn.Click();

            var eventMeeting = driver.FindElement(By.XPath("//*[contains(text(),'8pm')]/parent::div[1]/parent::td[1]/following-sibling::td[3]/div/div/div/dl/dd/div/span"));
            Assert.AreEqual(uniqueMeetingName, eventMeeting.Text);

            eventMeeting.Click();
            var deleteEventBtn = driver.FindElement(By.XPath("//div[contains(text(),'Delete')]"));
            deleteEventBtn.Click();
        }
    }
}
