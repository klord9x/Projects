using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
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
            catch (WebDriverException)
            {
                return null;
            }
        }

        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => driver.FindElementSafe(by));
            }
            return driver.FindElementSafe(by);
        }

        public static IReadOnlyCollection<IWebElement> FindElementsSafe(this IWebElement element, IWebDriver driver, By by, int timeoutInSeconds = 15)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(drv => element.FindElements(by));
            try
            {
                return element.FindElements(by);
            }
            catch (StaleElementReferenceException ex)
            {
                Console.WriteLine(ex.Message);
//                return element.FindElements(by);
            }

            return element.FindElements(by);
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
            try
            {
                wait.Until(wd => wd.ExecuteJavaScript<string>("return document.readyState") == "complete");
            }
            catch (NoSuchWindowException e)
            {
                Console.WriteLine(e);
//                driver.Close();
//                throw;
            }
            catch (WebDriverTimeoutException e)
            { Console.WriteLine(e); }
            catch( WebDriverException e)
            { Console.WriteLine(e);  }
        }

        public static void ClickSafe(this IWebElement element, IWebDriver driver)
        {
            try
            {
                element.Click();
            }
            catch (WebDriverException e)
            {
//                driver.Close();
                Console.WriteLine(e);
//                throw;
            }
            
            driver.WaitForLoad();
        }
    }
}
