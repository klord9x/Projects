using OpenQA.Selenium;

namespace AutoDataVPBank.Beginners.Pages.FecreditPage
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
                return this._browser.FindElementSafe(By.Name("signedTo"));
            }
        }

        public IWebElement TxtSingedElement
        {
            get
            {
                return this._browser.FindElementSafe(By.Name("signed"));
            }
        }

        public IWebElement SelectBoxSelProductElement
        {
            get
            {
                return this._browser.FindElementSafe(By.Name("selProduct"));
            }
        }

        public IWebElement SelectBoxSelActivityIdElement
        {
            get
            {
                return this._browser.FindElementSafe(By.Name("selActivityId"));
            }
        }

        public IWebElement BtnBtnSearchElement
        {
            get
            {
                return this._browser.FindElementSafe(By.Name("btnSearch"));
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
                return this._browser.FindElementSafe(By.XPath("//*[@id='178']/div "));
            }
        }
    }
}
