using OpenQA.Selenium;

namespace AutoDataVPBank.core
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
