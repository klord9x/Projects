using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace WebDriverCloudLoadTesting.Pages.AutomateThePlanet
{
    public partial class HomePage
    {
        private readonly IWebDriver driver;
        private readonly string url = @"http://automatetheplanet.com";

        public HomePage(IWebDriver browser)
        {
            this.driver = browser;
            PageFactory.InitElements(browser, this);
        }

        public void Navigate()
        {
            this.driver.Navigate().GoToUrl(this.url);
        }

        public void GoToBlog()
        {
            this.GoToTheBlogLink.Click();
        }
    }
}
