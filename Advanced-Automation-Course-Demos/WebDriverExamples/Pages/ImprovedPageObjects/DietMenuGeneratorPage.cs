using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using NUnit.Framework;
using OpenQA.Selenium.Support.UI;
using System;

namespace WebDriverExamples.Pages.ImprovedPageObjects
{
    public partial class DietMenuGeneratorPage
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        private readonly string url = @"https://automatetheplanet.com/healthy-diet-menu-generator";

        public DietMenuGeneratorPage(IWebDriver browser)
        {
            this.driver = browser;
            this.wait = new WebDriverWait(this.driver, TimeSpan.FromSeconds(value: 30));
            PageFactory.InitElements(browser, this);
        }

        public void Navigate()
        {
            driver.Navigate().GoToUrl(this.url);
        }

        public void PopulateMenu(string burgerType, string firstName)
        {
            this.AdditionalSugarCheckbox.Click();
            this.AdditionalSugarCheckbox.Click();
            var burgersSelect = new OpenQA.Selenium.Support.UI.SelectElement(this.BurgersSelectElement);
            burgersSelect.SelectByText(burgerType);
            this.FirstNameTextInput.Clear();
            this.FirstNameTextInput.SendKeys(firstName);
        }
    }
}
