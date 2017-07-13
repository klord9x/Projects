using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return wait.Until(drv => drv.FindElementSafe(by));
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

        public static IReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => (drv.FindElements(by).Count > 0) ? drv.FindElements(by) : null);
            }
            return driver.FindElements(by);
        }

        /// <summary>
        /// Find an element, waiting until a timeout is reached if necessary.
        /// </summary>
        /// <param name="context">The search context.</param>
        /// <param name="by">Method to find elements.</param>
        /// <param name="timeout">How many seconds to wait.</param>
        /// <param name="displayed">Require the element to be displayed?</param>
        /// <returns>The found element.</returns>
        /// 
        public static IWebElement FindElement(this ISearchContext context, By by, uint timeout, bool displayed = false)
        {
            var wait = new DefaultWait<ISearchContext>(context) {Timeout = TimeSpan.FromSeconds(timeout)};
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return wait.Until(ctx => {
                var elem = ctx.FindElement(by);
                if (displayed && !elem.Displayed)
                    return null;

                return elem;
            });
        }

        public static string GetAttributeSafe(this IWebElement element, string attr)
        {
            if (element == null)
            {
                return null;
            }
            return element.GetAttribute(attr);
        }

        public static string TextSafe(this IWebElement element)
        {
            if (element == null)
            {
                return null;
            }
            return element.Text.Trim();
        }

        public static void WaitForLoad(this IWebDriver driver, int timeoutSec = 60)
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
//            catch (WebDriverTimeoutException e)
//            { Console.WriteLine(e); }
//            catch( WebDriverException e)
//            { Console.WriteLine(e);  }
        }

        public static void WaitForPageLoad(this IWebDriver _driver, int maxWaitTimeInSeconds)
        {
            string state = string.Empty;
            try
            {
                WebDriverWait wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(maxWaitTimeInSeconds));

                //Checks every 500 ms whether predicate returns true if returns exit otherwise keep trying till it returns ture
                wait.Until(d => {

                    try
                    {
                        state = ((IJavaScriptExecutor)_driver).ExecuteScript(@"return document.readyState").ToString();
                    }
                    catch (InvalidOperationException)
                    {
                        //Ignore
                    }
                    catch (NoSuchWindowException)
                    {
                        //when popup is closed, switch to last windows
                        _driver.SwitchTo().Window(_driver.WindowHandles.Last());
                    }
                    //In IE7 there are chances we may get state as loaded instead of complete
                    return (state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase));

                });
            }
            catch (TimeoutException)
            {
                //sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
                if (!state.Equals("interactive", StringComparison.InvariantCultureIgnoreCase))
                    throw;
            }
            catch (NullReferenceException)
            {
                //sometimes Page remains in Interactive mode and never becomes Complete, then we can still try to access the controls
                if (!state.Equals("interactive", StringComparison.InvariantCultureIgnoreCase))
                    throw;
            }
            catch (WebDriverException)
            {
                if (_driver.WindowHandles.Count == 1)
                {
                    _driver.SwitchTo().Window(_driver.WindowHandles[0]);
                }
                state = ((IJavaScriptExecutor)_driver).ExecuteScript(@"return document.readyState").ToString();
                if (!(state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase)))
                    throw;
            }
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
