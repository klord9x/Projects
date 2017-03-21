using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using NUnit.Framework;

namespace WebDriverExamples.Pages
{
    public partial class DietMenuGeneratorPage
    {
        private readonly IWebDriver driver;
        private readonly string url = @"https://automatetheplanet.com/healthy-diet-menu-generator";

        [FindsBy(How = How.Id, Using = "ninja_forms_field_18")]
        private IWebElement additionalSugarCheckbox;

        [FindsBy(How = How.Id, Using = "ninja_forms_field_21")]
        private IWebElement burgersSelectElement;

        [FindsBy(How = How.XPath, Using = "//*[@id='ninja_forms_field_23']")]
        private IWebElement firstNameTextInput;

        [FindsBy(How = How.TagName, Using = "h2")]
        private IWebElement mainHeadline;

        [FindsBy(How = How.Id, Using = "ninja_forms_field_19")]
        private IWebElement ventiRadioButton;

        public DietMenuGeneratorPage(IWebDriver browser)
        {
            this.driver = browser;
            PageFactory.InitElements(browser, this);
        }

        public void Navigate()
        {
            driver.Navigate().GoToUrl(this.url);
        }

        public void PopulateMenu(string burgerType, string firstName)
        {
            this.additionalSugarCheckbox.Click();
            additionalSugarCheckbox.Click();
            var burgersSelect = new OpenQA.Selenium.Support.UI.SelectElement(burgersSelectElement);
            burgersSelect.SelectByText(burgerType);
            firstNameTextInput.Clear();
            firstNameTextInput.SendKeys(firstName);
        }

        public void AssertHeadline()
        {
            Assert.AreEqual("Awesome Healthy Menu Generator", mainHeadline.Text, "");
        }
    }
}
