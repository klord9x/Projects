using OpenQA.Selenium;

namespace AutoDataVPBank.Beginners.Pages.FecreditPage
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
                return this._browser.FindElementSafe(By.Name("TxtUID"));
            }
        }

        public IWebElement TxtPasswordElement
        {
            get
            {
                return this._browser.FindElementSafe(By.Name("TxtPWD"));
            }
        }

        public IWebElement DataActionElement
        {
            get
            {
                return this._browser.FindElementSafe(By.Name("DataAction"));
            }
        }

        public IWebElement BtnPage1CasarchElement
        {
            get
            {
                return this._browser.FindElementSafe(By.Name("btnCASARCH"));
            }
        }

        public IWebElement BtnPage1CasElement
        {
            get
            {
                return this._browser.FindElementSafe(By.Name("btnCAS"));
            }
        }

        public IWebElement BtnPage1ExitElement
        {
            get
            {
                return this._browser.FindElementSafe(By.Name("btnEXIT"));
            }
        }

        public IWebElement BtnPage2Click1Element
        {
            get
            {
                return this._browser.FindElementSafe(By.XPath("/html/body/form/div[3]/table/tbody/tr/td[1]"));
            }
        }

        public IWebElement BtnPage2Click2Element
        {
            get
            {
                ////*[@id="178"]/div
                return this._browser.FindElementSafe(By.XPath("//*[@id='178']/div "));
            }
        }

        public IWebElement BtnPage2CasClick2Element
        {
            get
            {
                ////*[@id="178"]/div
                return this._browser.FindElementSafe(By.XPath("//*[@id='4619']/div "));
            }
        }

    }
}
