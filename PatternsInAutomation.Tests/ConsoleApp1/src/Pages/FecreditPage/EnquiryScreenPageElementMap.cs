using AutoDataVPBank.core;
using OpenQA.Selenium;

namespace AutoDataVPBank.Pages.FecreditPage
{
    internal class EnquiryScreenPageElementMap: BasePageElementMap
    {
        public IWebElement TxtSignedToElement => Browser.FindElementSafeV2(By.Name("signedTo"));

        public IWebElement TxtSingedElement => Browser.FindElementSafeV2(By.Name("signed"));

        public IWebElement SelectBoxSelProductElement => Browser.FindElementSafeV2(By.Name("selProduct"));

        public IWebElement SelectBoxSelActivityIdElement => Browser.FindElementSafeV2(By.Name("selActivityId"));

        public IWebElement BtnBtnSearchElement => Browser.FindElementSafeV2(By.Name("btnSearch"));

        public IWebElement BtnPage1ExitElement => Browser.FindElementSafeV2(By.Name("btnEXIT"));

        public IWebElement BtnPage2Click1Element => Browser.FindElementSafeV2(
            By.XPath("/html/body/form/div[3]/table/tbody/tr/td[1]"));

        public IWebElement BtnPage2Click2Element => Browser.FindElementSafeV2(By.XPath("//*[@id='178']/div "));

        public IWebElement LinkQdeDetailElement => Browser.FindElementSafeV2(By.PartialLinkText("QDE"));
        public IWebElement TabSourcingElement => Browser.FindElementSafeV2(By.CssSelector("#apy_b0i1font"));
        public IWebElement TabDemographicElement => Browser.FindElementSafeV2(By.CssSelector("#apy_b0i2font"));
        public IWebElement TabPersonalElement => Browser.FindElementSafeV2(By.CssSelector("#apy_b1i1font"));
    }
}
