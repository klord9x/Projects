using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Beginners.Pages.FecreditLogin
{
    class FecreditLoginPage
    {
        private readonly IWebDriver _browser;
        private const string Name = @"CC100278";
        private const string Password = @"Khoinguyen@2";
        private const string Url = @"https://cps.fecredit.com.vn/finnsso/gateway/SSOGateway?requestID=7000003";

        public FecreditLoginPage(IWebDriver browser)
        {
            this._browser = browser;
        }

        protected FecreditLoginPageElementMap Map
        {
            get { return new FecreditLoginPageElementMap(this._browser); }
        }

        public void Navigate()
        {
            this._browser.Navigate().GoToUrl(Url);
        }
        /// <summary>
        /// #1. Login Url.
        /// </summary>
        public void Login()
        {
            this.Map.TxtNameElement.Clear();
            this.Map.TxtNameElement.SendKeys(Name);

            this.Map.TxtPasswordElement.Clear();
            this.Map.TxtPasswordElement.SendKeys(Password);

            this.Map.DataActionElement.Click();
            /*Check have alert:
             User ID is already logged in. Do you wish to create a new session.
             */
            try
            {
                WebDriverWait wait = new WebDriverWait(_browser, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.AlertIsPresent());
                IAlert alert = _browser.SwitchTo().Alert();
                System.Console.WriteLine(alert.Text);
                alert.Accept();
            }
            catch (Exception e)
            {
                //exception handling
            }

            //            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            //switch to new window. Page 1
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            string page1WindowHandle = _browser.CurrentWindowHandle;
            this.Map.BtnPage1CasarchElement.Click();
            
            //switch to new window. Page 2
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            this.Map.BtnPage2Click1Element.Click();
            this.Map.BtnPage2Click2Element.Click();

            //switch back to original window. Page 1
            _browser.SwitchTo().Window(page1WindowHandle);
            this.Map.BtnPage1ExitElement.Click();

            _browser.SwitchTo().Window(_browser.WindowHandles.Last());






            //if you want to switch back to your first window
            //                        _browser.SwitchTo().Window(_browser.WindowHandles.First());

            // Store the current window handle
            //            String winHandleBefore = _browser.CurrentWindowHandle;

            //            // Perform the click operation that opens new window
            //
            //            // Switch to new window opened
            //            for (String winHandle : _browser.WindowHandles)
            //            {
            //                driver.switchTo().window(winHandle);
            //            }
            //
            //            // Perform the actions on new window
            //
            //            // Close the new window, if that window no more required
            //            driver.close();
            //
            //            // Switch back to original browser (first window)
            //            driver.switchTo().window(winHandleBefore);

            // Continue with original browser (first window)
        }
    }
}
