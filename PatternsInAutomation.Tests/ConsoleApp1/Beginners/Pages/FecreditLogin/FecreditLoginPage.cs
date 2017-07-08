using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoDataVPBank.Beginners.Pages.FecreditLogin
{
   
    public class ReCord
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string ID_Card_Number { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string Stage { get; set; }
        public string Scheme { get; set; }
        public string Company { get; set; }
        public string Income { get; set; }
        public string DSA_Code { get; set; }
        public string DSA_Name { get; set; }
        public string TSA_Code { get; set; }
        public string TSA_Name { get; set; }
        public string SA_Phone_number { get; set; }

    }
    
    class FecreditLoginPage
    {
        private readonly IWebDriver _browser;
        //private const string Name = @"CC100278";
        //private const string Password = @"Khoinguyen@2";
        private const string Url = @"https://cps.fecredit.com.vn/finnsso/gateway/SSOGateway?requestID=7000003";

        //private const string Signed = "02/05/2016";
        //private const string SignedTo = "02/05/2016";
        IWebElement page = null;

        public FecreditLoginPage(IWebDriver browser)
        {
            this._browser = browser;
        }

        protected FecreditLoginPageElementMap Map
        {
            get { return new FecreditLoginPageElementMap(this._browser); }
        }

        protected EnquiryScreenPageElementMap ScreenMap
        {
            get { return new EnquiryScreenPageElementMap(this._browser); }
        }

        public void Navigate()
        {
            this._browser.Navigate().GoToUrl(Url);
        }
        /// <summary>
        /// #1. Login Url.
        /// </summary>
        public void Login(string User, string pass, string signform, string signto, string active)
        {
            this.Map.TxtNameElement.Clear();
            this.Map.TxtNameElement.SendKeys(User);

            this.Map.TxtPasswordElement.Clear();
            this.Map.TxtPasswordElement.SendKeys(pass);

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

            //switch to new window. Page 1
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            string page1WindowHandle = _browser.CurrentWindowHandle;
            this.Map.BtnPage1CasarchElement.Click();
            
            //switch to new window. Page 2
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
//            _browser.SwitchTo().DefaultContent();
            _browser.SwitchTo().Frame("frameForwardToApp");
            _browser.SwitchTo().Frame("contents");
            this.Map.BtnPage2Click1Element.Click();
            this.Map.BtnPage2Click2Element.Click();

            //switch back to original window. Page 1
            _browser.SwitchTo().Window(page1WindowHandle);
            this.Map.BtnPage1ExitElement.Click();

            //switch to Enquiry Screen.
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            this.EnquiryScreen(signform, signto, active);
        }

        /// <summary>
        /// 2. Enquiry Screen Page, Search content.
        /// </summary>

        public void EnquiryScreen (string Signeform, string signto, string active)
        {
            string page1WindowHandle = _browser.CurrentWindowHandle; //Save Enquiry Screen.
            SelectElement selActivityId = new SelectElement(this.ScreenMap.SelectBoxSelActivityIdElement);
            SelectElement selProduct = new SelectElement(this.ScreenMap.SelectBoxSelProductElement);

            selActivityId.SelectByText(active);
            selProduct.SelectByValue("PERSONAL");

            this.ScreenMap.TxtSignedToElement.Clear();
            this.ScreenMap.TxtSignedToElement.SendKeys(signto);

            this.ScreenMap.TxtSingedElement.Clear();
            this.ScreenMap.TxtSingedElement.SendKeys(Signeform);
//            driver.Manage().Timeouts().SetPageLoadTimeout(timespan);

//            _browser.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(120));

            this.ScreenMap.BtnBtnSearchElement.Click();
            //Waiting search:
            //            Thread.Sleep(250);

            //            _browser.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            // Tell webdriver to wait
            //            WebDriverWait wait = new WebDriverWait(this._browser, TimeSpan.FromSeconds(10));
            //
            //            // Test the autocomplete response - Explicit Wait
            //            IWebElement autocomplete = wait.Until(x => x.FindElement(By.ClassName("BORDERATTRIBUTES")));
            //
            //            string autoCompleteResults = autocomplete.Text;
            //
            //            System.Console.WriteLine(autoCompleteResults);

            //            Assert.That(autoCompleteResults, Is.EqualTo("A la cloche d'Or\r\nParis, France"));
            //            this.WaitForPageLoad();
            //            this._browser.Manage().Timeouts().SetPageLoadTimeout(10, TimeUnit.SECONDS);
            //            WebDriverWait wait = new WebDriverWait(this._browser, TimeSpan.FromSeconds(10));  // you can reuse this one
            //
            //            IWebElement elem = this._browser.FindElement(By.ClassName("BORDERATTRIBUTES"));
            ////            elem.click();
            //            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(elem));

            //@TODO: Handle Dynamic WebTables in Selenium Webdriver
            //BORDERATTRIBUTES

//            _browser.Manage().Timeouts().SetPageLoadTimeout(TimeSpan.FromSeconds(120));

            // xpath of html table
//            var elemTable = this._browser.FindElement(By.ClassName("BORDERATTRIBUTES"));
            var elemTable = this._browser.FindElement(By.XPath("//*[@id='formID206']/table[4]"));

            // Fetch all Row of the table
            List<IWebElement> lstTrElem = new List<IWebElement>(elemTable.FindElements(By.TagName("tr")));
            String strRowData = "";
            String detail1Row = "";//Click <a href="javascript:updateFunc('0')" tabindex="0">2734182</a>
            String FullNameRow = "";//Click <a href="javascript:updateFunc('0')" tabindex="0">2734182</a>
//            String detail2Row = "";//Click <a href="Activity.los?activity=QDE&amp;currentActivity=QDE&amp;txtApplicationNo=2734182&amp;category=PERSONAL&amp;mode=V&amp;dealId=&amp;inBranchID=1&amp;hidCustomerID=2377405">QDE</a>

            ////////////////////////////////////////////////////////////////////////////////////
            Microsoft.Office.Interop.Excel.Application oXL;
            Microsoft.Office.Interop.Excel._Workbook oWB;
            Microsoft.Office.Interop.Excel._Worksheet oSheet;
            Microsoft.Office.Interop.Excel.Range oRng;
            object misvalue = System.Reflection.Missing.Value;
            string pathExcelFile = Application.StartupPath + @"\AutoDataVPBank.xls";
            //check File exits
            if (File.Exists(pathExcelFile))
            {
                File.Delete(pathExcelFile);
            }
            //Start Excel and get Application object.
            oXL = new Microsoft.Office.Interop.Excel.Application();
            oXL.Visible = true;

            //Get a new workbook.
            oWB = (Microsoft.Office.Interop.Excel._Workbook)(oXL.Workbooks.Add(""));
            oSheet = (Microsoft.Office.Interop.Excel._Worksheet)oWB.ActiveSheet;
            //Add table headers going cell by cell.
            List<string> Lheader = new List<string>() { "Full Name", "Gender", "Age", "ID Card Number", "Phone", "State", "Stage", "Scheme", "Company", "Income", "DSA Code", "DSA Name", "TSA Code", "TSA Name", "SA Phone number" };
            for (int i = 0; i < Lheader.Count; i++)
            {
                oSheet.Cells[1, i + 1] = Lheader[i];
            }

            //Format A1:D1 as bold, vertical alignment = center.
            oSheet.get_Range("A1", "O1").Font.Bold = true;
            oSheet.get_Range("A1", "O1").VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;
            int indexRecord = 2;
            List < ReCord >lxxReCords= new List<ReCord>();
            // Traverse each row
            foreach (var elemTr in lstTrElem)
            {
                // Fetch the columns from a particuler row
                List<IWebElement> lstTdElem = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));
                if (lstTdElem.Count > 0)
                {
                    FullNameRow = lstTdElem[2].Text;
                    // Traverse each column
                    foreach (var elemTd in lstTdElem)
                    {
                        // "\t\t" is used for Tab Space between two Text
                        strRowData = strRowData + elemTd.Text + "\t\t";
                        //
                        if (lstTdElem.First() == elemTd)
                        {
                            Console.WriteLine("Get link detail1");
                            detail1Row = elemTd.Text;
                        }

                    }
                    //Click to detail1 Row:
                    //Or can be identified as href link 
                    //Check number
                    int n;
                    bool isNumeric = int.TryParse(detail1Row, out n);
                    if (!isNumeric)
                    {
                        continue;
                    }
                    IWebElement element = this._browser.FindElement(By.PartialLinkText(detail1Row));

                    ReCord recData = this.getDetail2(element);
                   
                    // Create an array to multiple values at once.
                    List<string> arrRecord = new List<string>() { };
                    lxxReCords.Add(recData);
                    arrRecord.Add(FullNameRow);
                    arrRecord.Add(recData.Gender);
                    arrRecord.Add(recData.Age);
                    arrRecord.Add(recData.ID_Card_Number);
                    arrRecord.Add(recData.Phone);
                    arrRecord.Add(recData.State);
                    arrRecord.Add(recData.Stage);
                    arrRecord.Add(recData.Scheme);
                    arrRecord.Add(recData.Company);
                    arrRecord.Add(recData.Income);
                    arrRecord.Add(recData.DSA_Code);
                    arrRecord.Add(recData.DSA_Name);
                    arrRecord.Add(recData.TSA_Code);
                    arrRecord.Add(recData.TSA_Name);
                    arrRecord.Add(recData.SA_Phone_number);
                    string indexRow = "A" + indexRecord;
                    string indexCol = "O" + indexRecord;
                    oSheet.get_Range(indexRow, indexCol).Value2 = arrRecord.ToArray();
                    indexRecord++;


                    //////////////////////////////////////////////////////////////////
                }
                else
                {
                    // To print the data into the console
                    Console.WriteLine("This is Header Row");
                    Console.WriteLine(lstTrElem[0].Text.Replace(" ", "\t\t"));
                }
                Console.WriteLine(strRowData);
                strRowData = String.Empty;

                //                if (lstTrElem.First() == elemTr)
                //                {
                //                    Console.WriteLine("Header");
                //                }
                _browser.SwitchTo().Window(page1WindowHandle);
            }//End foreach
            lxxReCords.ToList().ForEach(i => Console.Write("{0}\t", i));
            Console.WriteLine("");
            //            driver.Quit();

            //AutoFit columns A:D.
            oRng = oSheet.get_Range("A1", "O1");
            oRng.EntireColumn.AutoFit();

            oXL.Visible = false;
            oXL.UserControl = false;
            oWB.SaveAs(pathExcelFile, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            oWB.Close();
            //oXL.Workbooks.Open(pathExcelFile);
            System.Diagnostics.Process.Start(pathExcelFile);
            //...
            //}
            //catch {  }
            //Console.ReadLine();





        }

        /// <summary>
        /// Get detail 1 for Row.
        /// </summary>
        //        public ReCord getDetail1(IWebElement element)
        //        {
        //            //Console.WriteLine("Get detail1 for:" + element.Text);
        //            string page1WindowHandle = _browser.CurrentWindowHandle; //Save Enquiry Screen.
        //            element.Click();
        //            //Switch to last window: detail 1
        //            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
        //
        //            //Get all information from table:BORDERATTRIBUTES
        //            //html/body/form/table
        //            //            var elemTable = this._browser.FindElement(By.ClassName("BORDERATTRIBUTES"));
        //            //            //            var elemTable = this._browser.FindElement(By.XPath("//*[@id='formID206']/table[4]"));
        //            //            // Fetch all Row of the table
        //            //            List<IWebElement> lstTrElem = new List<IWebElement>(elemTable.FindElements(By.TagName("tr")));
        //            //            String strRowData = "";
        //            //
        //            //            // Traverse each row
        //            //            foreach (var elemTr in lstTrElem)
        //            //            {
        //            //                // Fetch the columns from a particuler row
        //            //                List<IWebElement> lstTdElem = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));
        //            //                if (lstTdElem.Count > 0)
        //            //                {
        //            //                    // Traverse each column
        //            //                    foreach (var elemTd in lstTdElem)
        //            //                    {
        //            //                        // "\t\t" is used for Tab Space between two Text
        //            //                        strRowData = strRowData + elemTd.Text + "\t\t";
        //            //                    }
        //            //                }
        //            //                else
        //            //                {
        //            //                    // To print the data into the console
        //            //                    Console.WriteLine("This is Header Detail1");
        //            //                    Console.WriteLine(lstTrElem[0].Text.Replace(" ", "\t\t"));
        //            //                }
        //            //                Console.WriteLine(strRowData);
        //            //                strRowData = String.Empty;
        //            //            }
        //
        //            //TODO: get detail2:
        //            //<a href="Activity.los?activity=QDE&amp;currentActivity=QDE&amp;txtApplicationNo=2734182&amp;category=PERSONAL&amp;mode=V&amp;dealId=&amp;inBranchID=1&amp;hidCustomerID=2377405">QDE</a>
        ////            _browser.SwitchTo().Window(page1WindowHandle);
        ////            return this.getDetail2("");
        //            //switch back to original window. Enquity Screen
        //            
        //
        //
        //
        //        }

        public ReCord getDetail2(IWebElement element1)
        {
            element1.Click();
            //Switch to last window: detail 1
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            IWebElement element = this._browser.FindElement(By.PartialLinkText("QDE"));
            Console.WriteLine("Get detail2 for");
            //Switch to last window: detail 2
            element.Click();
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            //Handler data in detail2:
            //1. Sourcing: /html/body/form/table[5]; /html/body/form/table[6]
            //Handler table by column.
            IWebElement tabSourcing = this._browser.FindElement(By.PartialLinkText("Sourcing"));
            tabSourcing.Click();
            //            IWebElement table1 = this._browser.FindElement(By.XPath("/html/body/form/table[5]"));
            //            IWebElement table2 = this._browser.FindElement(By.XPath("/html/body/form/table[6]"));
            //Just get row by Name, don't need foreach all table.
            string dateOfReceipt = this._browser.FindElement(By.Name("txtDDEDate")).GetAttribute("value");
            string branchCode = this._browser.FindElement(By.Name("txtSourcingBranch")).GetAttribute("value");
            string pos = this._browser.FindElement(By.Name("selPOS")).GetAttribute("value");
            string group = this._browser.FindElement(By.Name("selSchemeGroupDesc")).GetAttribute("value");
            string scheme = this._browser.FindElement(By.Name("selSchemeDesc")).GetAttribute("value");
            string selDSACode = this._browser.FindElement(By.Name("selDSACode")).GetAttribute("value");
            string selTSACode = this._browser.FindElement(By.Name("selTSACode")).GetAttribute("value");
            string selDSAName = this._browser.FindElement(By.Name("selDSAName")).GetAttribute("value");
            string selTSAName = this._browser.FindElement(By.Name("selTSAName")).GetAttribute("value");

            //=======================

            //2. Demographic:
            IWebElement tabDemographic = this._browser.FindElement(By.PartialLinkText("Demographic"));
            tabDemographic.Click();
            //Click information user: //*[@id="formID184"]/table[5]/tbody/tr[3]/td[1]/a

            IWebElement userName = this._browser.FindElement(By.XPath("//*[@id='formID184']/table[5]/tbody/tr[3]/td[1]/a"));
            userName.Click();

            //2.1 Personal tab:
            IWebElement tabPersonal = this._browser.FindElement(By.PartialLinkText("Personal"));
            tabPersonal.Click();
            //            //Tables: //*[@id="formID188"]/table[7]
            //            IWebElement tablePersonal = this._browser.FindElement(By.XPath("//*[@id='formID188']/table[7]"));
            //            //tableOwnerDetail: //*[@id="formID188"]/table[10]
            //            IWebElement tableOwnerDetail = this._browser.FindElement(By.XPath("//*[@id='formID188']/table[10]"));
            //            //tableAddresssDetail: //*[@id="formID188"]/table[13]
            //            IWebElement tableAddresssDetail = this._browser.FindElement(By.XPath("//*[@id='formID188']/table[13]"));
            //Just get row by Name, don't need foreach all table.
            string txtTINNo = this._browser.FindElement(By.Name("txtTINNo")).GetAttribute("value");
            string txtAge = this._browser.FindElement(By.Name("txtAge")).GetAttribute("value");
            //string selSex = this._browser.FindElement(By.Name("selSex")).GetAttribute("value");
            IWebElement comboBox = _browser.FindElement(By.Name("selSex"));
            SelectElement selectedValue = new SelectElement(comboBox);
            string selSex = selectedValue.SelectedOption.Text;
            string selState = this._browser.FindElement(By.Name("selState")).GetAttribute("value");

            //2.2 Work Detail tab:
            IWebElement tabWork = this._browser.FindElement(By.PartialLinkText("Work Detail"));
            tabWork.Click();
            //Just get row by Name, don't need foreach all table.
            string companyName = this._browser.FindElement(By.Name("txtOtherEmpName")).GetAttribute("value");
            string street = this._browser.FindElement(By.Name("txtAddressThree")).GetAttribute("value");
            string country = this._browser.FindElement(By.Name("selCountry")).GetAttribute("value");
            string district = this._browser.FindElement(By.Name("txtCity")).GetAttribute("value");
            string ward = this._browser.FindElement(By.Name("txtAddressFour")).GetAttribute("value");
            string city = this._browser.FindElement(By.Name("selState")).GetAttribute("value");
            string homePhone = this._browser.FindElement(By.Name("txtPhoneOne")).GetAttribute("value");
            string mobile = this._browser.FindElement(By.Name("txtMobile")).GetAttribute("value");

            //2.3 Address: ???
            //2.4 Income/Liability tab:
            IWebElement tabIncome = this._browser.FindElement(By.PartialLinkText("Income/Liability"));
            tabIncome.Click();

            ReCord rec = new ReCord();
            rec.FullName = "";
            rec.Gender = selSex;
            rec.Age = txtAge;
            rec.ID_Card_Number = txtTINNo;
            rec.Phone = homePhone;
            rec.State = selState;
            rec.Stage = "";
            rec.Scheme = scheme;
            rec.Company = companyName;
            rec.Income = "";
            rec.DSA_Code = selDSACode;
            rec.DSA_Name = selDSAName;
            rec.TSA_Code = selTSACode;
            rec.TSA_Name = selTSAName;
            rec.SA_Phone_number = mobile;
            _browser.Close();
            //switch back to original window. Screen En
//            _browser.SwitchTo().Window(page1WindowHandle);

            return rec;
        }

        public void WaitForPageLoad()
        {
            if (page != null)
            {
                var waitForCurrentPageToStale = new WebDriverWait(this._browser, TimeSpan.FromSeconds(10));
                waitForCurrentPageToStale.Until(ExpectedConditions.StalenessOf(page));
            }

            var waitForDocumentReady = new WebDriverWait(this._browser, TimeSpan.FromSeconds(10));
            waitForDocumentReady.Until((wdriver) => (this._browser as IJavaScriptExecutor).ExecuteScript("return document.readyState").Equals("complete"));

            page = this._browser.FindElement(By.ClassName("BORDERATTRIBUTES"));

            System.Console.WriteLine("Page loaded");

        }

//        public static bool WaitUntilElementIsPresent(IWebDriver driver, By by, int timeout = 10)
//        {
//            for (var i = 0; i < timeout; i++)
//            {
//                if (driver.ElementIsPresent(by)) return true;
//                Thread.Sleep(1000);
//            }
//            return false;
//        }
    }
}
