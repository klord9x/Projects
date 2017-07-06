using OpenQA.Selenium;
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
            IAlert alert = _browser.SwitchTo().Alert();
            System.Console.WriteLine(alert.Text);
            alert.Accept();
        }
    }
}
