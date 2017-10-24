﻿using System.Collections.Generic;
using AutoDataVPBank.core;
using OpenQA.Selenium;

namespace AutoDataVPBank.Pages.FecreditPage
{
    public class FecreditLoginPageElementMap : BasePageElementMap
    {
        public IWebElement TxtNameElement => Browser.FindElementSafeV2(By.Name("TxtUID"));

        public IWebElement TxtPasswordElement => Browser.FindElementSafeV2(By.Name("TxtPWD"));

        public IWebElement DataActionElement => Browser.FindElementSafeV2(By.Name("DataAction"));

        public IWebElement BtnPage1CasarchElement => Browser.FindElementSafeV2(By.Name("btnCASARCH"));

        public IWebElement BtnPage1CasElement => Browser.FindElementSafeV2(By.Name("btnCAS"));

        public IWebElement BtnPage1ExitElement => Browser.FindElementSafeV2(By.Name("btnEXIT"));

        public IWebElement BtnPage2Click1Element => Browser.FindElementSafeV2(
            By.XPath("/html/body/form/div[3]/table/tbody/tr/td[1]"));

        public IWebElement BtnPage2Click2Element
        {
            get
            {
                var listBy = new List<By>
                {
                    By.XPath("//*[@id='178']/div[1]")
                };
                return Browser.ExistElement(listBy);
            }
        }

        public IWebElement BtnPage2CasClick2Element
        {
            get
            {
                var listBy = new List<By>
                {
                    By.XPath("//*[@id='4619']/div[1]"),
                    By.CssSelector("#4619 > div:nth-child(1)")
                };
                return Browser.ExistElement(listBy);
            }
        }
        
    }
}
