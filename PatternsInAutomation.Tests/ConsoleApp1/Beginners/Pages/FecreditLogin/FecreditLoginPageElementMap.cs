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

        public IWebElement BtnPage1CasarchElement
        {
            get
            {
                return this._browser.FindElement(By.Name("btnCASARCH"));
            }
        }

        public IWebElement BtnPage1ExitElement
        {
            get
            {
                return this._browser.FindElement(By.Name("btnEXIT"));
            }
        }

        public IWebElement BtnPage2Click1Element
        {
            get
            {
//                return this._browser.FindElement(By.XPath("/html/body/form/div[3]"));
//                return this._browser.FindElement(By.ClassName("menuRow"));
//                return this._browser.FindElement(By.CssSelector("td.menuTree"));
                return this._browser.FindElement(By.CssSelector("td.menuRow"));
            }
        }

        public IWebElement BtnPage2Click2Element
        {
            get
            {
//                return this._browser.FindElement(By.XPath("//*[@id='178']/div "));
                return this._browser.FindElement(By.ClassName("menu"));
            }
        }


    }
}
