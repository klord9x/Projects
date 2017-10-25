using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using static AutoDataVPBank.Library;

namespace AutoDataVPBank
{
    internal static class Safe
    {
        public static IWebElement FindElementSafe(this IWebDriver driver, By by)
        {
            WaitForLoad(driver);
            try
            {
                return driver.FindElement(by);
            }
            catch (NoSuchElementException e)
            {
                Logg.Error(e.Message);
                return FindElementSafe(driver, by);
            }
            catch (StaleElementReferenceException e)
            {
                Logg.Error(e.Message);
                return FindElementSafe(driver, by);
            }
            catch (WebDriverException e)
            {
                Logg.Error(e.Message);
                return null;
            }
        }

        public static IWebElement FindElement(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElementSafeV2(by));
            }
            return driver.FindElementSafeV2(by);
        }

        public static IReadOnlyCollection<IWebElement> FindElementsSafe(this IWebElement element, IWebDriver driver,
            By by, int timeoutInSeconds = 15)
        {
            WaitForLoad(driver);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            wait.Until(drv => element.FindElements(by));
            try
            {
                return element.FindElements(by);
            }
            catch (StaleElementReferenceException ex)
            {
                Logg.Error(ex.Message);
            }

            return element.FindElements(by);
        }

        public static IReadOnlyCollection<IWebElement> FindElements(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(drv => drv.FindElements(by).Count > 0 ? drv.FindElements(by) : null);
            }
            return driver.FindElements(by);
        }

        /// <summary>
        ///     Find an element, waiting until a timeout is reached if necessary.
        /// </summary>
        /// <param name="context">The search context.</param>
        /// <param name="by">Method to find elements.</param>
        /// <param name="timeout">How many seconds to wait.</param>
        /// <param name="displayed">Require the element to be displayed?</param>
        /// <returns>The found element.</returns>
        public static IWebElement FindElement(this ISearchContext context, By by, uint timeout, bool displayed = false)
        {
            var wait = new DefaultWait<ISearchContext>(context) {Timeout = TimeSpan.FromSeconds(timeout)};
            wait.IgnoreExceptionTypes(typeof(NoSuchElementException));
            return wait.Until(ctx =>
            {
                var elem = ctx.FindElement(by);
                if (displayed && !elem.Displayed)
                    return null;

                return elem;
            });
        }

        public static string GetAttributeSafe(this IWebElement element, string attr)
        {
            return element?.GetAttribute(attr);
        }

        public static string TextSafe(this IWebElement element)
        {
            return element?.Text.Trim();
        }

        public static void WaitForLoad(this IWebDriver driver, int timeoutSec = 60)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
            try
            {
                wait.Until(wd => wd.ExecuteJavaScript<string>("return document.readyState") == "complete");
            }
            catch (NoSuchWindowException e)
            {
                Logg.Error(e.Message);
//                driver.Close();
//                throw;
            }
//            catch (WebDriverTimeoutException e)
//            { Logg.Error(e); }
//            catch( WebDriverException e)
//            { Logg.Error(e);  }
        }

        public static void WaitForPageLoad(this IWebDriver driver, int maxWaitTimeInSeconds)
        {
            var state = string.Empty;
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(maxWaitTimeInSeconds));

                //Checks every 500 ms whether predicate returns true if returns exit otherwise keep trying till it returns ture
                wait.Until(d =>
                {
                    try
                    {
                        state = ((IJavaScriptExecutor) driver).ExecuteScript(@"return document.readyState").ToString();
                    }
                    catch (InvalidOperationException)
                    {
                        //Ignore
                    }
                    catch (NoSuchWindowException)
                    {
                        //when popup is closed, switch to last windows
                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                    }
                    catch (WebDriverException e)
                    {
                        Logg.Error(e.Message);
                        throw;
                    }
                    //In IE7 there are chances we may get state as loaded instead of complete
                    return state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) ||
                           state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase);
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
                if (driver.WindowHandles.Count == 1)
                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                state = ((IJavaScriptExecutor) driver).ExecuteScript(@"return document.readyState").ToString();
                if (!(state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) ||
                      state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase)))
                    throw;
            }
        }

        public static void ClickSafe(this IWebElement element, IWebDriver driver)
        {
            try
            {
                if (element == null) return;
                element.Click();
                driver.WaitForPageLoad(15);
            }
            catch (WebDriverException e)
            {
                Logg.Error(e.Message);
            }
            catch (Exception e)
            {
                 Logg.Error(e.Message);
                throw;
            }
        }

        public static void WaitingPageRefreshed(this IWebDriver browser, By by)
        {
            //TODO: Try find result if exist Maxtimeout = 5': 
            var s = new Stopwatch();
            s.Start();
            while (s.Elapsed < TimeSpan.FromSeconds(300))
                try
                {
                    var resultElement = browser.FindElement(by, 5);
                    if (resultElement != null)
                        break;
                }
                catch (Exception e)
                {
                    Logg.Error(e);
                }

            s.Stop();
        }

        public static IWebElement FindElementSafeV2(this IWebDriver driver, By by, bool displayed = true)
        {
            WaitForPageLoad(driver, Set.CloneNickSettings.PageLoad.Times.GetValue());
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(Set.CloneNickSettings.SignUp.Times.GetValue()));
                return wait.Until(d =>
                {
                    try
                    {
                        var elem = d.FindElement(by);
                        if (displayed && !elem.Displayed)
                            return null;
                        return elem;
                    }
                    catch (NoSuchElementException e)
                    {
                        Logg.Error(e.Message);
                        return null;
                    }
                    catch (InvalidOperationException e)
                    {
                        Logg.Error(e.Message);
                        return null;
                    }
                    catch (Exception e)
                    {
                        Logg.Error(e.Message);
                        throw;
                    }
                });
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);

                return null;
            }
        }

        public static IWebElement ExistElement(this IWebDriver driver, By by)
        {
            try
            {
                return driver.FindElement(by);
            }
            catch (NoSuchElementException e)
            {
                Logg.Info(e.Message);
                return null;
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                return null;
            }
        }

        public static IWebElement ExistElement(this IWebDriver driver, List<By> listBy)
        {
            if (listBy == null) throw new ArgumentNullException(nameof(listBy));
            try
            {
                foreach (var by in listBy)
                    try
                    {
                        var element = driver.FindElement(by);
                        if (element != null)
                            return element;
                    }
                    catch (NoSuchElementException e)
                    {
                        Logg.Info(e.Message);
                    }
                    catch (Exception e)
                    {
                        Logg.Error(e.Message);
                        return null;
                    }
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                return null;
            }

            return null;
        }

        public static void NavigateSafe(this IWebDriver driver, string url, bool sleep = false, int refresh = 0)
        {
            if (refresh > 0)
                while (refresh >= 0)
                {
                    try
                    {
                        driver.Navigate().GoToUrl(url);
                        if (sleep)
                            SleepSafe(0, Set.CloneNickSettings.Default.Times.GetValue());

                        if (driver.Url.Contains(url))
                            driver.WaitForPageLoad(Set.CloneNickSettings.PageLoad.Times.GetValue());
                    }

                    catch (WebDriverTimeoutException e)
                    {
                        Logg.Error(@"Time out: " + e.Message);
                    }
                    catch (NoSuchElementException)
                    {
                        Logg.Error(@"Can not Navigate to: " + url);
                    }
                    catch (WebDriverException e)
                    {
                        //TODO: Why fail, it crash app, don't continue ???
                        Logg.Error(@"Driver fail: " + e.Message);

                        //Facebook.RestartFacebook();
                        throw;
                    }

                    refresh--;
                }
            else
                try
                {
                    driver.Navigate().GoToUrl(url);
                    if (sleep)
                        SleepSafe(0, Set.CloneNickSettings.Default.Times.Min);
                    //var driver2 = DriverFactory.Browser.Value;
                    //driver2.Navigate().GoToUrl(url);

                    if (driver.Url.Contains(url))
                        driver.WaitForPageLoad(Set.CloneNickSettings.PageLoad.Times.GetValue());
                    else
                        Logg.Error(@"Can not Navigate to: " + url);
                }
                catch (NoSuchWindowException)
                {
                }
                catch (WebDriverException e)
                {
                    Logg.Error(@"Driver fail: " + e.Message);
                    throw;
                }
        }

        public static void RefreshSafe(this IWebDriver driver, int time = 0, bool wait = true, bool sleep = false)
        {
            try
            {
                if (driver == null)
                    return;
                driver.Navigate().Refresh();
                if (wait)
                    driver.WaitForPageLoad(Set.CloneNickSettings.PageLoad.Times.GetValue());
                if (sleep)
                    SleepSafe(0, time > 0 ? time : 69);
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                //
                throw;
            }
        }

        public static void SleepSafe(int minTime, int maxTime, string text = "", int mult = 0,
            [CallerFilePath] string file = "",
            [CallerMemberName] string member = "",
            [CallerLineNumber] int line = 0)
        {
            try
            {
                if (mult == 0)
                    mult = 1000;
                if (minTime == 0)
                    Sleep(maxTime * mult, $" {Path.GetFileName(file)}|{member}({line}): {text}");
                else if (minTime > 0)
                    Sleep(RandomNumber.Between(minTime, maxTime) * mult,
                        $" {Path.GetFileName(file)}|{member}({line}): {text}");
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }

        private static void Sleep(int time, string from)
        {
            try
            {
                //CancelTask();
                if (time >= 1000)
                    Logg.Info("Times= " + time + from);
                Thread.Sleep(time);
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);

                throw;
            }
        }

        //TODO: Still fail error
        public static void CloseSafe(this IWebDriver driver)
        {
            try
            {
                if (driver?.CurrentWindowHandle == null) return;
                driver.Close();
                driver.Quit();
            }
            catch (NullReferenceException e)
            {
                 Logg.Error(e.Message);
                KillProcess();
                throw;
            }
            catch (WebDriverException e)
            {
                 Logg.Error(e.Message);
                KillProcess();
                throw;
            }
            catch (Exception e)
            {
                 Logg.Error(e.Message);
                KillProcess();
                throw;
            }
        }

        /*public static void WaitForPageLoad(this IWebDriver driver, int maxWaitTimeInSeconds)
        {
            var state = string.Empty;
            if (driver == null)
            {
                return;
            }
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(maxWaitTimeInSeconds));

                //Checks every 500 ms whether predicate returns true if returns exit otherwise keep trying till it returns ture
                wait.Until(d =>
                {
                    try
                    {
                        state = ((IJavaScriptExecutor)driver).ExecuteScript(@"return document.readyState").ToString();
                    }
                    catch (InvalidOperationException)
                    {
                        //Ignore
                    }
                    catch (NoSuchWindowException)
                    {
                        //when popup is closed, switch to last windows
                        driver.SwitchTo().Window(driver.WindowHandles.Last());
                    }
                    catch (WebDriverException)
                    {

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
                if (driver.WindowHandles.Count == 1)
                {
                    driver.SwitchTo().Window(driver.WindowHandles[0]);
                }
                state = ((IJavaScriptExecutor)driver).ExecuteScript(@"return document.readyState").ToString();
                if (!(state.Equals("complete", StringComparison.InvariantCultureIgnoreCase) || state.Equals("loaded", StringComparison.InvariantCultureIgnoreCase)))
                    throw;
            }
        }*/
    }
}
