using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Application = System.Windows.Forms.Application;

namespace AutoDataVPBank.Beginners.Pages.FecreditPage
{
   
    public class ReCord
    {
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string Age { get; set; }
        public string IdCardNumber { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string Stage { get; set; }
        public string Scheme { get; set; }
        public string Company { get; set; }
        public string Income { get; set; }
        public string DsaCode { get; set; }
        public string DsaName { get; set; }
        public string TsaCode { get; set; }
        public string TsaName { get; set; }
        public string SaPhoneNumber { get; set; }
        public string ApplicationNo { get; set; }
        public string Assign { get; set; }
        public string ReferncesName { get; set; }
        public string History { get; set; }

    }
    
    class FecreditLoginPage
    {
        private readonly IWebDriver _browser;
        //private const string Name = @"CC100278";
        //private const string Password = @"Khoinguyen@2";
        private const string Url = @"https://cps.fecredit.com.vn/finnsso/gateway/SSOGateway?requestID=7000003";

        //private const string Signed = "02/05/2016";
        //private const string SignedTo = "02/05/2016";

        public FecreditLoginPage(IWebDriver browser)
        {
            _browser = browser;
        }

        protected FecreditLoginPageElementMap LoginMap
        {
            get { return new FecreditLoginPageElementMap(_browser); }
        }

        protected EnquiryScreenPageElementMap ScreenMap
        {
            get { return new EnquiryScreenPageElementMap(_browser); }
        }

        public void Navigate()
        {
            _browser.Navigate().GoToUrl(Url);
        }

        /// <summary>
        /// #1. Login Url.
        /// </summary>
        public void Login(string user, string pass, string signform, string signto, string active, string stage)
        {
            string message = "";
            LoginMap.TxtNameElement.Clear();
            LoginMap.TxtNameElement.SendKeys(user);

            LoginMap.TxtPasswordElement.Clear();
            LoginMap.TxtPasswordElement.SendKeys(pass);

            try
            {
                LoginMap.DataActionElement.Click();
            }
            catch (Exception e)
            {
//                Console.WriteLine(e);
//                throw;
            }

            
            /*Check have alert:
             User ID is already logged in. Do you wish to create a new session.
             */
            try
            {
                WebDriverWait wait = new WebDriverWait(_browser, TimeSpan.FromSeconds(5));
                wait.Until(ExpectedConditions.AlertIsPresent());
                IAlert alert = _browser.SwitchTo().Alert();
                message = alert.Text;
                Console.WriteLine(message);
                alert.Accept();
            }
            catch (Exception e)
            {
                //exception handling
            }

            //switch to new window. Page 1
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            string enquiryScreenWindow = _browser.CurrentWindowHandle;
            if (LoginMap.BtnPage1CasarchElement == null)
            {
                _browser.Quit();
                MessageBox.Show(message, @"Oh No!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Console.WriteLine(@"Can't login");
                return;
            }
            LoginMap.BtnPage1CasarchElement?.Click();

            //switch to new window. Page 2
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            try
            {
                _browser.SwitchTo().Frame("frameForwardToApp");
                _browser.SwitchTo().Frame("contents");
                LoginMap.BtnPage2Click1Element.Click();
                LoginMap.BtnPage2Click2Element.Click();
            }
            catch (WebDriverException e)
            {
//                Console.WriteLine(e);
//                throw;
            }
            

            //switch back to original window. Page 1
            _browser.SwitchTo().Window(enquiryScreenWindow);
            LoginMap.BtnPage1ExitElement.Click();

            //switch to Enquiry Screen.
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            EnquiryScreen(signform, signto, active, stage);
        }

        /// <summary>
        /// 2. Enquiry Screen Page, Search content.
        /// </summary>
        public void EnquiryScreen (string signform, string signto, string active, string stage)
        {
            string enquiryScreenWindow = _browser.CurrentWindowHandle; //Save Enquiry Screen.
            SelectElement selActivityId = new SelectElement(ScreenMap.SelectBoxSelActivityIdElement);
            SelectElement selProduct = new SelectElement(ScreenMap.SelectBoxSelProductElement);

            selActivityId.SelectByText(active);
            selProduct.SelectByValue("PERSONAL");

            ScreenMap.TxtSignedToElement.Clear();
            ScreenMap.TxtSignedToElement.SendKeys(signto);

            ScreenMap.TxtSingedElement.Clear();
            ScreenMap.TxtSingedElement.SendKeys(signform);

            try
            {
                ScreenMap.BtnBtnSearchElement.Click();
            }
            catch (Exception e)
            {
                Thread.Sleep(5000);
                Console.WriteLine(e);
            }
            try
            {
                //Try find result if exist: 
                var employeeLabel = _browser.FindElement(By.CssSelector("#selPageIndex > option"), 10);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
//                throw;
            }
            


            ////////////////////////////////////////////////////////////////////////////////////
//            object misvalue = System.Reflection.Missing.Value;
//            var pathExcelFile = Application.StartupPath + @"\AutoDataVPBank.xls";
//            //check File exits
//            if (File.Exists(pathExcelFile))
//            {
//                File.Delete(pathExcelFile);
//            }
            //Start Excel and get Application object.
            var oXl = new Microsoft.Office.Interop.Excel.Application {Visible = true};

            //Get a new workbook.
            _Workbook oWb = oXl.Workbooks.Add("");
            var oSheet = (_Worksheet)oWb.ActiveSheet;
            //Add table headers going cell by cell.
            List<string> lheader = new List<string>() { "Full Name", "Gender", "Age", "ID Card Number", "Phone", "State", "Stage", "Scheme", "Company", "Income", "DSA Code", "DSA Name", "TSA Code", "TSA Name", "SA Phone number", "ApplicationNo", "Assign", "References", "History"};

            for (int i = 0; i < lheader.Count; i++)
            {
                oSheet.Cells[1, i + 1] = lheader[i];
            }

            //Format A1:D1 as bold, vertical alignment = center.
            oSheet.Range["A1", "S1"].Font.Bold = true;
            oSheet.Range["A1", "S1"].VerticalAlignment = XlVAlign.xlVAlignCenter;
            int indexRecord = 2;
            List < ReCord >lxxReCords= new List<ReCord>();

            //TODO Call function Fetch all Row of the table 
            GetDataTable(lxxReCords, ref indexRecord, oSheet, enquiryScreenWindow, stage);

            //TODO Click next page or select next page:
            //*[@id="formID206"]/table[5]/tbody/tr/td/font/a[3]
            int n = 1;
            int page = 1;
            try
            {
                IWebElement lastOption = _browser.FindElement(By.CssSelector("#selPageIndex > option:last-child"));
                Int32.TryParse(lastOption.GetAttribute("value"), out n);
            }
            catch (Exception e)
            {
                Thread.Sleep(5000);
                Console.WriteLine(e);
            }

            for (int i = 1; i < n; i++)
            {
                IWebElement nextPage = _browser.FindElement(By.XPath("//*[@id='formID206']/table[5]/tbody/tr/td/font/a[3]"));
                try
                {
                    nextPage.Click();
                }
                catch (Exception e)
                {
                    page = i;
                    Thread.Sleep(5000);
                    Console.WriteLine(e);
                }

                GetDataTable(lxxReCords, ref indexRecord, oSheet, enquiryScreenWindow,stage);
            }

            //AutoFit columns A:D.
            var oRng = oSheet.Range["A1", "S1"];
            oRng.EntireColumn.AutoFit();

            //Create file to save:
            var folder = ("\\" + signform + "_" + signto).Replace("/", "");
            var pathExcelFile = Application.StartupPath + folder + @"\AutoDataVPBank" + page + ".xls";
            //check File exits
            if (File.Exists(pathExcelFile))
            {
                File.Delete(pathExcelFile);
            }

            oXl.Visible = false;
            oXl.UserControl = false;
            oWb.SaveAs(pathExcelFile, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            oWb.Close();
            //oXL.Workbooks.Open(pathExcelFile);
            System.Diagnostics.Process.Start(pathExcelFile);
        }

        /// <summary>
        /// Get all detail need export.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="stage"></param>
        /// <returns></returns>
        public ReCord GetAllDetail(IWebElement e, string stage)
        {
            e.Click();
            //Switch to last window: detail 1
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            IWebElement element = _browser.FindElementSafe(By.PartialLinkText("QDE"));
            Console.WriteLine(@"Get detail");
            //Switch to last window: detail 2
            element.Click();
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            //TODO: Get all value of tab.

            //Handler data in detail2:
            //1. Sourcing: /html/body/form/table[5]; /html/body/form/table[6]
            //Handler table by column.
            IWebElement tabSourcing = _browser.FindElementSafe(By.PartialLinkText("Sourcing"));
            tabSourcing.Click();
            string scheme = _browser.FindElementSafe(By.Name("selSchemeDesc")).GetAttribute("value");
            string selDSACode = _browser.FindElementSafe(By.Name("selDSACode")).GetAttribute("value");
            string selTSACode = _browser.FindElementSafe(By.Name("selTSACode")).GetAttribute("value");
            string selDSAName = _browser.FindElementSafe(By.Name("selDSAName")).GetAttribute("value");
            string selTSAName = _browser.FindElementSafe(By.Name("selTSAName")).GetAttribute("value");
            string servicePhone = _browser.FindElementSafe(By.Name("txtServicePhoneNumber")).GetAttribute("value");

            //=======================

            //2. Demographic:
            IWebElement tabDemographic = _browser.FindElementSafe(By.PartialLinkText("Demographic"));
            tabDemographic.Click();
            //Click information user:
            IWebElement userName = _browser.FindElementSafe(By.XPath("//*[@id='formID184']/table[5]/tbody/tr[3]/td[1]/a"));
            userName.Click();

            //2.1 Personal tab:
            IWebElement tabPersonal = _browser.FindElementSafe(By.PartialLinkText("Personal"));
            tabPersonal.Click();
            string txtTINNo = _browser.FindElementSafe(By.Name("txtTINNo")).GetAttribute("value");
            string txtAge = _browser.FindElementSafe(By.Name("txtAge")).GetAttribute("value");
            IWebElement comboBox = _browser.FindElementSafe(By.Name("selSex"));
            SelectElement selectedValue = new SelectElement(comboBox);
            string selSex = selectedValue.SelectedOption.Text;
            string selState = _browser.FindElementSafe(By.Name("selState")).GetAttribute("value");

            //2.2 Work Detail tab:
            IWebElement tabWork = _browser.FindElementSafe(By.PartialLinkText("Work Detail"));
            tabWork.Click();
            //Just get row by Name, don't need foreach all table.
            string companyName = _browser.FindElementSafe(By.Name("txtOtherEmpName")).GetAttribute("value");
            string mobile = _browser.FindElementSafe(By.Name("txtMobile")).GetAttribute("value");

            //2.3 Address: ???
            //2.4 Income/Liability tab:
            IWebElement tabIncome = _browser.FindElementSafe(By.PartialLinkText("Income/Liability"));
            tabIncome.Click();
            string inCome1 = _browser.FindElementSafe(By.XPath("//*[@id='formID140']/table[7]/tbody/tr[2]/td[3]")).Text;
            string inCome2 = _browser.FindElementSafe(By.XPath("//*[@id='formID140']/table[7]/tbody/tr[3]/td[3]")).Text;

            //3. References:
            IWebElement tabReferences = _browser.FindElementSafe(By.PartialLinkText("References"));
            string strReferences = "";
            tabReferences.Click();
            //Select all href link:
            List<IWebElement> listRef = new List<IWebElement> (_browser.FindElements(By.CssSelector("a[href^='javascript:updateFunc']")));
           
            foreach (var href in listRef)
            {
                strReferences += href.Text +" :";
                href.Click();
                strReferences += _browser.FindElementSafe(By.Name("txtPhoneAreaCode")).GetAttribute("value") + "; ";
            }

            //4. History: Get Select second last element with css
            string history = "";
            if (stage == "Reject Review")
            {
                IWebElement tabHistory = _browser.FindElementSafe(By.PartialLinkText("Application History"));
                tabHistory.Click();
                history = _browser.FindElementSafe(By.CssSelector("#formID9 > table:nth-child(83) > tbody > tr:nth-last-child(2) > td:nth-child(2) > font")).Text;
            }
           


            ReCord rec = new ReCord
            {
                FullName = "",
                Gender = selSex,
                Age = txtAge,
                IdCardNumber = txtTINNo,
                Phone = mobile,
                State = selState,
                Stage = "",
                Scheme = scheme,
                Company = companyName,
                Income = inCome1,
                DsaCode = selDSACode,
                DsaName = selDSAName,
                TsaCode = selTSACode,
                TsaName = selTSAName,
                SaPhoneNumber = servicePhone,
                ReferncesName = strReferences,
                History = history,
            };
            _browser.Close();

            return rec;
        }

        /// <summary>
        /// Get value of Input element in Tag.
        /// </summary>
        /// <param name="tabName"></param>
        /// <param name="txtNameList"></param>
        /// <param name="listRecord"></param>
        public List<KeyValuePair<string, string>> GetTxtValuesInTag(String tabName, String txtNameList, ref List<KeyValuePair<string, string>> listRecord)
        {
            string[] arrName = txtNameList.Split(',');
            //Click to tab:
            IWebElement tab = _browser.FindElementSafe(By.PartialLinkText(tabName));
            tab.Click();
            foreach (var txtName in arrName)
            {
                IWebElement txtElement = _browser.FindElementSafe(By.Name(txtName));
                if (txtElement != null)
                {
                    listRecord.Add(new KeyValuePair<string, string>(txtName, txtElement.GetAttribute("value")));
                }
            }

            return listRecord;
        }

        /// <summary>
        /// Get data Table
        /// </summary>
        /// <param name="lxxReCords"></param>
        /// <param name="indexRecord"></param>
        /// <param name="oSheet"></param>
        /// <param name="enquiryScreenWindow"></param>
        public void GetDataTable(List<ReCord> lxxReCords, ref int indexRecord, _Worksheet oSheet, string enquiryScreenWindow, string stage)
        {
            var elemTable = _browser.FindElementSafe(By.XPath("//*[@id='formID206']/table[4]"));
            // Fetch all Row of the table
            List<IWebElement> lstTrElem = new List<IWebElement>(elemTable.FindElements(By.TagName("tr")));
//            String detailHref = "";//Click <a href="javascript:updateFunc('0')" tabindex="0">2734182</a>
//            String assigned = "";
            // Traverse each row
            foreach (var elemTr in lstTrElem)
            {
                if (lstTrElem.First() == elemTr)
                {
                    continue;
                }
                // Fetch the columns from a particuler row
                List<IWebElement> lstTdElem = new List<IWebElement>(elemTr.FindElements(By.TagName("td")));
                if (lstTdElem.Count > 0)
                {
                    var detailHref = lstTdElem[0].Text;
                    //string applicationNo = 
                    var assigned = lstTdElem[1].Text;
                    var fullNameRow = lstTdElem[2].Text;
                    // Traverse each column
//                    foreach (var elemTd in lstTdElem)
//                    {
//                        //Get Text value of first td.
//                        if (lstTdElem.First() == elemTd)
//                        {
//                            detailHref = elemTd.Text;
//                        }
//
//                    }

                    //Click to detail1 Row:
                    //Or can be identified as href link 
                    //Check number
                    bool isNumeric = detailHref.All(char.IsDigit);
                    if (!isNumeric)
                    {
                        continue;
                    }
                    IWebElement element = _browser.FindElementSafe(By.PartialLinkText(detailHref));

                    ReCord recData = GetAllDetail(element, stage);

                    // Create an array to multiple values at once.
                    List<string> arrRecord = new List<string>() { };
                    lxxReCords.Add(recData);
                    arrRecord.Add(fullNameRow);
                    arrRecord.Add(recData.Gender);
                    arrRecord.Add(recData.Age);
                    arrRecord.Add(recData.IdCardNumber);
                    arrRecord.Add(recData.Phone);
                    arrRecord.Add(recData.State);
                    arrRecord.Add(stage);
                    arrRecord.Add(recData.Scheme);
                    arrRecord.Add(recData.Company);
                    arrRecord.Add(recData.Income);
                    arrRecord.Add(recData.DsaCode);
                    arrRecord.Add(recData.DsaName);
                    arrRecord.Add(recData.TsaCode);
                    arrRecord.Add(recData.TsaName);
                    arrRecord.Add(recData.SaPhoneNumber);
                    arrRecord.Add(detailHref);
                    arrRecord.Add(assigned);
                    arrRecord.Add(recData.ReferncesName);
                    arrRecord.Add(recData.History);
                    string indexRow = "A" + indexRecord;
                    string indexCol = "S" + indexRecord;
                    oSheet.Range[indexRow, indexCol].Value2 = arrRecord.ToArray();
                    indexRecord++;
                }
                _browser.SwitchTo().Window(enquiryScreenWindow);
            }//End foreach
        }
    }
}
