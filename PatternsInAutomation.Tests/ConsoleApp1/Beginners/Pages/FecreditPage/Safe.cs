using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using OpenQA.Selenium.Support.Extensions;

namespace AutoDataVPBank.Beginners.Pages.FecreditPage
{
    static class Safe
    {
        public static IWebElement FindElementSafe(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElementSafe(by));
            }
            return driver.FindElementSafe(by);
        }

        public static string GetAttributeSafe(this IWebElement element, string attr)
        {
            return element?.GetAttribute(attr);
        }

        public static string TextSafe(this IWebElement element)
        {
            return element?.Text;
        }

        public static void WaitForLoad(this IWebDriver driver, int timeoutSec = 15)
        {
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
            wait.Until(wd => wd.ExecuteJavaScript<string>("return document.readyState") == "complete");
        }

//        public static string ExecuteJavaScriptSafe(this IWebDriver driver, string script)
//        {
//            return driver.ExecuteJavaScript<string>(script);
//        }
    }
}
