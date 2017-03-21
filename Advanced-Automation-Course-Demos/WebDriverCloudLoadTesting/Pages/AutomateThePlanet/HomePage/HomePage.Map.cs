using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;

namespace WebDriverCloudLoadTesting.Pages.AutomateThePlanet
{
    public partial class HomePage
    {
        [FindsBy(How = How.XPath, Using = "//*/h1")]
        public IWebElement MainHeadline { get; set; }

        [FindsBy(How = How.LinkText, Using = "Go to the blog")]
        public IWebElement GoToTheBlogLink { get; set; }
    }
}