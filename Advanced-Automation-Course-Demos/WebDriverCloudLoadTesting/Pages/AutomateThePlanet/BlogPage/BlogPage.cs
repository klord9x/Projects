using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;

namespace WebDriverCloudLoadTesting.Pages.AutomateThePlanet
{
    public partial class BlogPage
    {
        private readonly IWebDriver driver;
        private readonly string url = @"http://automatetheplanet.com/blog";

        public BlogPage(IWebDriver browser)
        {
            this.driver = browser;
            PageFactory.InitElements(browser, this);
        }

        public void WaitForSubscribeWidget()
        {
            new WebDriverWait(driver, TimeSpan.FromSeconds(30)).Until(ExpectedConditions.ElementExists((By.ClassName("subscribe"))));
        }
    }
}
