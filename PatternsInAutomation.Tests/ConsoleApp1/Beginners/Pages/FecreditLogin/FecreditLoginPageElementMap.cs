using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Beginners.Pages.FecreditLogin
{
    class FecreditLoginPageElementMap
    {
        private readonly IWebDriver _browser;

        public FecreditLoginPageElementMap(IWebDriver browser)
        {
            this._browser = browser;
        }

        public IWebElement TxtNameElement
        {
            get
            {
                return this._browser.FindElement(By.Name("TxtUID"));
            }
        }

        public IWebElement TxtPasswordElement
        {
            get
            {
                return this._browser.FindElement(By.Name("TxtPWD"));
            }
        }

        public IWebElement DataActionElement
        {
            get
            {
                return this._browser.FindElement(By.Name("DataAction"));
            }
        }
    }
}
