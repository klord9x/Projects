using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;
using Microsoft.Office.Interop.Excel;
using Application = System.Windows.Forms.Application;
using System.Diagnostics;
using System.Globalization;
using OpenQA.Selenium.Support.Extensions;

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
        //Number try get record from table before exit.
        private int _nTry = 10;
        //Stop search next page.
        private bool _stopNav;

        private string _enquiryScreenWindow;
        private string _page1WindowHandle;
        private readonly string _signform;
        private readonly string _signto;
        private readonly string _active;

        private readonly string _product = "PERSONAL";
        private readonly List<string> _lheader = new List<string>() { "Full Name", "Gender", "Age", "ID Card Number", "Phone", "State", "Stage", "Scheme", "Company", "Income", "DSA Code", "DSA Name", "TSA Code", "TSA Name", "SA Phone number", "ApplicationNo", "Assign", "References", "History" };
        //private const string Name = @"CC100278";
        //private const string Password = @"Khoinguyen@2";
        private const string Url = @"https://cps.fecredit.com.vn/finnsso/gateway/SSOGateway?requestID=7000003";

        //private const string Signed = "02/05/2016";
        //private const string SignedTo = "02/05/2016";

        public FecreditLoginPage(IWebDriver browser, string signform, string signto, string active)
        {
            _browser = browser;
            _signform = signform;
            _signto = signto;
            _active = active;
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
        public void Login(string user, string pass, bool caSselect)
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
            _page1WindowHandle = _browser.CurrentWindowHandle;
            if (LoginMap.BtnPage1CasarchElement == null)
            {
                _browser.Quit();
                MessageBox.Show(message, @"Oh No!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Console.WriteLine(@"Can't login");
                return;
            }
            //TODO: Fail here
            if (caSselect)
            {
                LoginMap.BtnPage1CasElement.ClickSafe(_browser);
            }
            else LoginMap.BtnPage1CasarchElement.ClickSafe(_browser);
//            LoginMap.BtnPage1CasarchElement.ClickSafe(_browser);
            

            //switch to new window. Page 2
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
//            _browser.WaitForLoad();
            _browser.WaitForPageLoad(15);
            try
            {
                _browser.SwitchTo().Frame("frameForwardToApp");
                _browser.SwitchTo().Frame("contents");
                LoginMap.BtnPage2Click1Element.ClickSafe(_browser);
                if (caSselect)
                {
                    LoginMap.BtnPage2CasClick2Element.ClickSafe(_browser);
                }
                else LoginMap.BtnPage2Click2Element.ClickSafe(_browser);
                
            }
            catch (WebDriverException e)
            {
                return;
//                Console.WriteLine(e);
//                throw;
            }

            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
//            _browser.WaitForLoad();
            EnquiryScreen();
        }

        /// <summary>
        /// 2. Enquiry Screen Page, Search content.
        /// </summary>
        public void EnquiryScreen ()
        {
            _enquiryScreenWindow = _browser.CurrentWindowHandle; //Save Enquiry Screen.
//            Boolean stopNav = false;
            SelectElement selActivityId = new SelectElement(ScreenMap.SelectBoxSelActivityIdElement);
            SelectElement selProduct = new SelectElement(ScreenMap.SelectBoxSelProductElement);

            if (_active == "Select")
            {
                selActivityId.SelectByText(_active);
            }
            else
            {
                selActivityId.SelectByText(_active);
            }
            selProduct.SelectByValue(_product);

            ScreenMap.TxtSignedToElement.Clear();
            ScreenMap.TxtSignedToElement.SendKeys(_signto);

            ScreenMap.TxtSingedElement.Clear();
            ScreenMap.TxtSingedElement.SendKeys(_signform);

            try
            {
                ScreenMap.BtnBtnSearchElement.Click();
            }
            catch (Exception e)
            {
//                Thread.Sleep(5000);
                Console.WriteLine(e);
            }

            //switch back to original window. Page 1. Logout
            _browser.SwitchTo().Window(_page1WindowHandle);
            LoginMap.BtnPage1ExitElement.Click();
            _browser.SwitchTo().Window(_enquiryScreenWindow);
            
            //TODO: Try find result if exist Maxtimeout = 5': 
            _browser.WaitingPageRefreshed(By.CssSelector("#selPageIndex > option"));

            ////////////////////////////////////////////////////////////////////////////////////
            object misvalue = System.Reflection.Missing.Value;
            var pathExcelFile = Application.StartupPath + @"\AutoDataVPBank.xls";
            //check File exits
            if (File.Exists(pathExcelFile))
            {
                File.Delete(pathExcelFile);
            }
            //Start Excel and get Application object.
            Microsoft.Office.Interop.Excel.Application oXl;
            try
            {
                oXl = new Microsoft.Office.Interop.Excel.Application { Visible = true };
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, @"Need Install Excel app!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Console.WriteLine(e);
                return;
            }
            

            //Get a new workbook.
            _Workbook oWb = oXl.Workbooks.Add("");
            var oSheet = (_Worksheet)oWb.ActiveSheet;
//            Add table headers going cell by cell.

            for (int i = 0; i < _lheader.Count; i++)
            {
                oSheet.Cells[1, i + 1] = _lheader[i];
            }

            //Format A1:D1 as bold, vertical alignment = center.
            oSheet.Range["A1", "S1"].Font.Bold = true;
            oSheet.Range["A1", "S1"].VerticalAlignment = XlVAlign.xlVAlignCenter;
            int indexRecord = 2;
            List < ReCord >lxxReCords= new List<ReCord>();

            //TODO Click next page or select next page:
            //*[@id="formID206"]/table[5]/tbody/tr/td/font/a[3]
            int n = 1;
            int page = 12;
            if (_active == "Reject Review")
            {
                page = 1;
            }
            // select the drop down list
//            SelectElement selectElement = new SelectElement(_browser.FindElement(By.CssSelector("#selPageIndex")));
            //create select element object 
//            var selectElement = new SelectElement(education);

           
            
            try
            {
                IWebElement lastOption = _browser.FindElement(By.CssSelector("#selPageIndex > option:last-child"));
                Int32.TryParse(lastOption.GetAttributeSafe("value"), out n);
            }
            catch (Exception e)
            {
//                Thread.Sleep(5000);
                Console.WriteLine(e);
            }
            if (n > page)
            {
                n = n - page;
            }
            else
            {
                return;
            }
//            n = 428;


            for (int i = n; i > 0; i--)
            {
                //i = 450;
                // select the drop down list
                _browser.ExecuteJavaScript("document.getElementById('customer').remove();");
                SelectElement selectElement = new SelectElement(_browser.FindElement(By.CssSelector("#selPageIndex")));
                selectElement.SelectByValue(i.ToString());

                //TODO: Fail for first chose option; or each select option (Firefox).
                //Try remove element and waiting element exit when page refreshed:
//                _browser.ExecuteJavaScript("document.getElementById('selPageIndex').remove();");
//                _browser.ExecuteJavaScript("document.getElementsByName('btnReset').remove();");
                _browser.WaitingPageRefreshed(By.Id("customer"));
                if (!_stopNav)
                {
                    GetDataTable(lxxReCords, ref indexRecord, oSheet);
                }
                else
                {
                    break;
                }
                
            }

            oXl.Visible = false;
            oXl.UserControl = false;
            oWb.SaveAs(pathExcelFile, XlFileFormat.xlWorkbookDefault, Type.Missing, Type.Missing,
                false, false, XlSaveAsAccessMode.xlNoChange,
                Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);

            oWb.Close();
            //oXl.Workbooks.Open(pathExcelFile);
            Process.Start(pathExcelFile);
        }

        /// <summary>
        /// Get all detail need export.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public ReCord GetAllDetail(IWebElement e)
        {
            Console.WriteLine(@"Start Get detail for: " + e.Text);
            //            ReCord rec = new ReCord { };
            if (e.Displayed && e.Enabled)
            {
                e.ClickSafe(_browser);
            }

            //Switch to last window: detail 1
            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            //TODO: firefox error.
            //            IWebElement element = null;
            IWebElement element = null;
            while (true)
            {
                element = _browser.FindElementSafe(By.PartialLinkText("QDE"));//body > a:nth-child(1)
                if (element != null && element.Displayed && element.Enabled)
                {
                    break;
                }
            }
            
//            IWebElement element = _browser.FindElementSafe(By.CssSelector("a[href^='Activity']"));//_browser.FindElements(By.CssSelector("a[href^='Activity']")
            Console.WriteLine(@"Get detail");
            //Switch to last window: detail 2
            element.ClickSafe(_browser);
//            _browser.SwitchTo().Window(_browser.WindowHandles.Last());
            //TODO: Get all value of tab.

            //Handler data in detail2:
            //1. Sourcing: /html/body/form/table[5]; /html/body/form/table[6]
            //Handler table by column.
            IWebElement tabSourcing = _browser.FindElementSafe(By.CssSelector("#apy_b0i1font"));
            tabSourcing.ClickSafe(_browser);
            string scheme = _browser.FindElementSafe(By.Name("selSchemeDesc")).GetAttributeSafe("value");
            string selDSACode = _browser.FindElementSafe(By.Name("selDSACode")).GetAttributeSafe("value");
            string selTSACode = _browser.FindElementSafe(By.Name("selTSACode")).GetAttributeSafe("value");
            string selDSAName = _browser.FindElementSafe(By.Name("selDSAName")).GetAttributeSafe("value");
            string selTSAName = _browser.FindElementSafe(By.Name("selTSAName")).GetAttributeSafe("value");
            string servicePhone = _browser.FindElementSafe(By.Name("txtServicePhoneNumber")).GetAttributeSafe("value");

            //=======================

            //2. Demographic: Error
            IWebElement tabDemographic = _browser.FindElementSafe(By.CssSelector("#apy_b0i2font"));
//            if (tabDemographic == null)
//            {
//                return null;
//            }
            tabDemographic.ClickSafe(_browser);
            //Click information user:
            IWebElement userName = _browser.FindElementSafe(By.XPath("//*[@id='formID184']/table[5]/tbody/tr[3]/td[1]/a"));
            //            if (userName == null)
            //            {
            //                return rec;
            //            }
            if (userName == null || userName.Text != "")
            {
                userName.ClickSafe(_browser);
            }

            //2.1 Personal tab:
            IWebElement tabPersonal = _browser.FindElementSafe(By.CssSelector("#apy_b1i1font"));
//            if (tabPersonal == null)
//            {
//                return rec;
//            }
//            else
//            {
            tabPersonal.ClickSafe(_browser);
            string txtTINNo = _browser.FindElementSafe(By.Name("txtTINNo")).GetAttributeSafe("value");
            string txtAge = _browser.FindElementSafe(By.Name("txtAge")).GetAttributeSafe("value");
            string selSex = "";
            IWebElement comboBox = _browser.FindElementSafe(By.Name("selSex"));
            if (comboBox != null)
            {
                SelectElement selectedValue = new SelectElement(comboBox);
                selSex = selectedValue.SelectedOption.Text;

            }

            string selState = _browser.FindElementSafe(By.Name("selState")).GetAttributeSafe("value");
//            rec.Gender = selSex;
//            rec.Age = txtAge;
//            rec.IdCardNumber = txtTINNo;
//            rec.State = selState;
            string mobile = _browser.FindElementSafe(By.Name("txtMobile")).GetAttributeSafe("value");
//            rec.Phone = mobile;
//            }
            

            //2.2 Work Detail tab:
            IWebElement tabWork = _browser.FindElementSafe(By.CssSelector("#apy_b1i2font"));
//            if (tabWork == null)
//            {
//                return rec;
//            }
//            else
//            {
            tabWork.ClickSafe(_browser);
            //Just get row by Name, don't need foreach all table.
            string companyName = _browser.FindElementSafe(By.Name("txtOtherEmpName")).GetAttributeSafe("value");
//            rec.Company = companyName;
//            }
          

            //2.3 Address: ???
            //2.4 Income/Liability tab:
            IWebElement tabIncome = _browser.FindElementSafe(By.CssSelector("#apy_b1i4font"));
            tabIncome.ClickSafe(_browser);
            string inCome1 = _browser.FindElementSafe(By.XPath("//*[@id='formID140']/table[7]/tbody/tr[2]/td[3]")).TextSafe();
//            string inCome2 = _browser.FindElementSafe(By.XPath("//*[@id='formID140']/table[7]/tbody/tr[3]/td[3]")).TextSafe();

            //3. References:
            IWebElement tabReferences = _browser.FindElementSafe(By.CssSelector("#apy_b0i8font"));
            string strReferences = "";
            tabReferences.ClickSafe(_browser);
            //Select all href link:
            List<IWebElement> listRef = new List<IWebElement> (_browser.FindElements(By.CssSelector("a[href^='javascript:updateFunc']")));
           
            foreach (var href in listRef)
            {
                strReferences += href.TextSafe() + " :";
                href.ClickSafe(_browser);
                strReferences += _browser.FindElementSafe(By.Name("txtPhoneAreaCode")).GetAttributeSafe("value") + "; ";
            }

            //4. History: Get Select 2 last element with css
            string history = "";
            IWebElement tabHistory = _browser.FindElementSafe(By.CssSelector("#apy_b0i9font"));
            tabHistory.ClickSafe(_browser);
            history += _browser.FindElementSafe(
                    By.CssSelector(
                        "#formID9 > table:nth-child(83) > tbody > tr:nth-last-child(2) > td:nth-child(2) > font"))
                .TextSafe();
            if (history.Trim() != "")
            {
                history += "; ";
            }
            history += _browser.FindElementSafe(
                    By.CssSelector(
                        "#formID9 > table:nth-child(83) > tbody > tr:nth-last-child(1) > td:nth-child(2) > font"))
                .TextSafe();

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

//        /// <summary>
//        /// Get value of Input element in Tag.
//        /// </summary>
//        /// <param name="tabName"></param>
//        /// <param name="txtNameList"></param>
//        /// <param name="listRecord"></param>
//        public List<KeyValuePair<string, string>> GetTxtValuesInTag(String tabName, String txtNameList, ref List<KeyValuePair<string, string>> listRecord)
//        {
//            string[] arrName = txtNameList.Split(',');
//            //Click to tab:
//            IWebElement tab = _browser.FindElementSafe(By.PartialLinkText(tabName));
//            tab.ClickSafe(_browser);
//            foreach (var txtName in arrName)
//            {
//                IWebElement txtElement = _browser.FindElementSafe(By.Name(txtName));
//                if (txtElement != null)
//                {
//                    listRecord.Add(new KeyValuePair<string, string>(txtName, txtElement.GetAttributeSafe("value")));
//                }
//            }
//
//            return listRecord;
//        }

        /// <summary>
        /// Get data Table
        /// </summary>
        /// <param name="lxxReCords"></param>
        /// <param name="indexRecord"></param>
        /// <param name="oSheet"></param>
        public void GetDataTable(List<ReCord> lxxReCords, ref int indexRecord, _Worksheet oSheet)
        {
//            _browser.WaitForLoad();
//            _browser.WaitForPageLoad(60);
            var elemTable = _browser.FindElementSafe(By.XPath("//*[@id='formID206']/table[4]"));
//            var elemTable = _browser.FindElementSafe(By.CssSelector("#formID206 > table:nth-child(23)"));
            // Fetch all Row of the table
            List<IWebElement> lstTrElem = new List<IWebElement>(elemTable.FindElementsSafe(_browser, By.TagName("tr")));
            int temp = 0;
//            int ntry = 10;
            //            String detailHref = "";//Click <a href="javascript:updateFunc('0')" tabindex="0">2734182</a>
            //            String assigned = "";
            // Traverse each row
            DateTime dateAsignFrom = DateTime.ParseExact(_signform, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime dateAsignTo = DateTime.ParseExact(_signto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            foreach (var elemTr in lstTrElem)
            {
                if (lstTrElem.First() == elemTr)
                {
                    continue;
                }
                // Fetch the columns from a particuler row
                List<IWebElement> lstTdElem = new List<IWebElement>(elemTr.FindElementsSafe(_browser, By.TagName("td")));
                if (lstTdElem.Count > 0)
                {
                    var detailHref = lstTdElem[0].TextSafe();
                    Console.WriteLine(@"Detail : " + detailHref);
                    //string applicationNo = 
                    var assigned = lstTdElem[1].TextSafe();
                    var fullNameRow = lstTdElem[2].TextSafe();
                    DateTime dateAsignCurrent = DateTime.ParseExact(assigned, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    if (dateAsignCurrent < dateAsignFrom || dateAsignCurrent >= dateAsignTo)
                    {
                        temp++;
                        continue;
//                        break;
                    }
                    //Click to detail1 Row:
                    //Or can be identified as href link 
                    //Check number
                    bool isNumeric = detailHref.All(char.IsDigit);
                    if (!isNumeric)
                    {
                        continue;
                    }
                    Console.WriteLine(@"Get detail for: " + detailHref);
                    IWebElement element = _browser.FindElementSafe(By.PartialLinkText(detailHref));

                    ReCord recData = GetAllDetail(element);
                    Console.WriteLine(@"End Get detail for: " + detailHref);
                    if (recData == null)
                    {
                        recData = new ReCord();
                    }

                    // Create an array to multiple values at once.
                    List<string> arrRecord = new List<string>() { };
                    lxxReCords.Add(recData);
                    arrRecord.Add(fullNameRow);
                    arrRecord.Add(recData.Gender);
                    arrRecord.Add(recData.Age);
                    arrRecord.Add(recData.IdCardNumber);
                    arrRecord.Add(recData.Phone);
                    arrRecord.Add(recData.State);
                    arrRecord.Add(_active);
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
                    var indexRow = "A" + indexRecord;
                    var indexCol = "S" + indexRecord;
                    oSheet.Range[indexRow, indexCol].Value2 = arrRecord.ToArray();
                    //AutoFit columns A:D.
                    var oRng = oSheet.Range["A1", "T1"];
                    oRng.EntireColumn.AutoFit();
                    indexRecord++;
                }
                _browser.SwitchTo().Window(_enquiryScreenWindow);
            }//End foreach

            //TODO: Stop when don't have record in duration.
            if (temp == lstTrElem.Count - 1)
            {
                _nTry--;
                if (_nTry == 0)
                {
                    _stopNav = true;
                }
            }
        }
    }
}
