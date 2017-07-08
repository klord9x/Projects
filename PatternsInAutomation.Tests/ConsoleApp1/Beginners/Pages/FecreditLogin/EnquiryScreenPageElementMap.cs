using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Beginners.Pages.FecreditLogin
{
    class EnquiryScreenPageElementMap
    {
        private readonly IWebDriver _browser;

        public EnquiryScreenPageElementMap(IWebDriver browser)
        {
            this._browser = browser;
        }

        public IWebElement TxtSignedToElement
        {
            get
            {
                return this._browser.FindElement(By.Name("signedTo"));
            }
        }

        public IWebElement TxtSingedElement
        {
            get
            {
                return this._browser.FindElement(By.Name("signed"));
            }
        }

        public IWebElement SelectBoxSelProductElement
        {
            get
            {
                return this._browser.FindElement(By.Name("selProduct"));
            }
        }

        public IWebElement SelectBoxSelActivityIdElement
        {
            get
            {
                return this._browser.FindElement(By.Name("selActivityId"));
            }
        }

        public IWebElement BtnBtnSearchElement
        {
            get
            {
                return this._browser.FindElement(By.Name("btnSearch"));
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
                return this._browser.FindElement(By.XPath("/html/body/form/div[3]/table/tbody/tr/td[1]"));
            }
        }

        public IWebElement BtnPage2Click2Element
        {
            get
            {
                return this._browser.FindElement(By.XPath("//*[@id='178']/div "));
            }
        }
    }
}
