using OpenQA.Selenium;

namespace AutoDataVPBank.Beginners.Pages.FecreditPage
{
    public class BasePageElementMap
    {
        protected readonly IWebDriver Browser;

        protected BasePageElementMap()
        {
            Browser = DriverFactory.Browser;
        }
    }
}
