﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AutoDataVPBank.core;
using Microsoft.Office.Interop.Excel;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.Extensions;
using OpenQA.Selenium.Support.UI;
using static AutoDataVPBank.Library;
using Application = System.Windows.Forms.Application;

namespace AutoDataVPBank.Pages.FecreditPage
{
    public class EnquiryScreenPage
    {
        //Number try get record from table before exit.
        private static int _nTry = Set.Fecredit.TryTimes.Value;
        //Stop search next page.
        private static bool _stopNav;
        private static string _enquiryScreenWindow;
        private static string _signform;
        private static string _signto;
        private static string _active;
        private static string _product;
        private static EnquiryScreenPageElementMap ScreenMap => new EnquiryScreenPageElementMap();
        private static EnquiryScreenPage _instance;
        public static EnquiryScreenPage GetInstance => _instance ?? (_instance = new EnquiryScreenPage());
        private static readonly List<string> Lheader = typeof(ReCord).GetProperties().Select(f => f.Name).ToList();

        public EnquiryScreenPage()
        {
            _signform = MForm.txtSignFo.Text;
            _signto = MForm.txtSignTo.Text;
            MForm.cboActive.Invoke((MethodInvoker)delegate
            {
                _active = MForm.cboActive.Text;
            });

            MForm.cboProDuct.Invoke((MethodInvoker)delegate
            {
                _product = (string)MForm.cboProDuct.SelectedValue;
            });
            
        }


        /// <summary>
        /// 2. Enquiry Screen Page, Search content.
        /// </summary>
        public void EnquiryScreen()
        {
            try
            {
                //Save Enquiry Screen.
                _enquiryScreenWindow = DriverFactory.Browser.CurrentWindowHandle;
                //TODO: Need Improve Unable to locate element: {"method":"name","selector":"selActivityId"}
                var selActivityId = new SelectElement(ScreenMap.SelectBoxSelActivityIdElement);
                var selProduct = new SelectElement(ScreenMap.SelectBoxSelProductElement);

                if (_active != "Select")
                {
                    selActivityId.SelectByText(_active);
                }
                selProduct.SelectByValue(_product);
                ScreenMap.TxtSignedToElement.Clear();
                ScreenMap.TxtSignedToElement.SendKeys(_signto);
                ScreenMap.TxtSingedElement.Clear();
                ScreenMap.TxtSingedElement.SendKeys(_signform);
                ScreenMap.BtnBtnSearchElement.ClickSafe(DriverFactory.Browser);
                FecreditLoginPage.Logout();
                DriverFactory.Browser.SwitchTo().Window(_enquiryScreenWindow);

                //TODO: Try find result if exist Maxtimeout = 5': 
                DriverFactory.Browser.WaitingPageRefreshed(By.CssSelector("#selPageIndex > option"));

                MainProcess();
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
            
        }

        private void MainProcess()
        {
            try
            {
                var pathExcelFile = Set.Fecredit.Paths.Data + @"\VPBank_" +
                                    new String(_signform.Where(Char.IsDigit).ToArray()) + "_" +
                                    new String(_signto.Where(Char.IsDigit).ToArray()) + ".xls";
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
                    MessageBox.Show(e.Message, @"Need Install Excel app!", MessageBoxButtons.OK,
                        MessageBoxIcon.Exclamation);
                    Logg.Error(e);
                    throw ;
                }


                //Get a new workbook.
                _Workbook oWb = oXl.Workbooks.Add("");
                var oSheet = (_Worksheet)oWb.ActiveSheet;
                //            Add table headers going cell by cell.

                for (var i = 0; i < Lheader.Count; i++)
                {
                    oSheet.Cells[1, i + 1] = Lheader[i];
                }

                //Format A1:D1 as bold, vertical alignment = center.
                oSheet.Range["A1", "U1"].Font.Bold = true;
                oSheet.Range["A1", "U1"].VerticalAlignment = XlVAlign.xlVAlignCenter;
                var indexRecord = 2;
                var lxxReCords = new List<ReCord>();

                //TODO Click next page or select next page:
                //*[@id="formID206"]/table[5]/tbody/tr/td/font/a[3]
                var n = 1;
                var page = 12;
                if (_active == "Reject Review")
                {
                    page = 1;
                }

                try
                {
                    var lastOption = DriverFactory.Browser.FindElement(By.CssSelector("#selPageIndex > option:last-child"));
                    Int32.TryParse(lastOption.GetAttributeSafe("value"), out n);
                }
                catch (Exception e)
                {
                    Logg.Error(e.Message);
                }
                if (n > page)
                {
                    n = n - page;
                }
                else
                {
                    return;
                }


                for (var i = n; i > 0; i--)
                {
                    if (CancelTask())
                    {
                        break;
                    }
                    //i = 450;
                    // select the drop down list
                    DriverFactory.Browser.ExecuteJavaScript("document.getElementById('customer').remove();");
                    var selectElement = new SelectElement(DriverFactory.Browser.FindElement(By.CssSelector("#selPageIndex")));
                    selectElement.SelectByValue(i.ToString());

                    //TODO: Fail for first chose option; or each select option (Firefox).
                    //Wait until have element.
                    DriverFactory.Browser.FindElementSafeV2(By.Id("customer"));
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
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
            
        }

        /// <summary>
        /// Get all detail need export.
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private static ReCord GetAllDetail(IWebElement e)
        {
            try
            {
                Logg.Error(@"Start Get detail for: " + e.Text);
                if (e.Displayed && e.Enabled)
                {
                    e.ClickSafe(DriverFactory.Browser);
                }

                //Switch to last window: detail 1
                DriverFactory.Browser.SwitchTo().Window(DriverFactory.Browser.WindowHandles.Last());
                Logg.Info("Current Browser:"+DriverFactory.Browser.Title);
                var element = ScreenMap.LinkQdeDetailElement;

                Logg.Error(@"Start Get detail");
                element.ClickSafe(DriverFactory.Browser);
                //TODO: Get all value of tab.

                //Handler data in detail2:
                //1. Sourcing: /html/body/form/table[5]; /html/body/form/table[6]
                //Handler table by column.
                var tabSourcing = ScreenMap.TabSourcingElement;
                tabSourcing.ClickSafe(DriverFactory.Browser);
                var scheme = DriverFactory.Browser.FindElementSafeV2(By.Name("selSchemeDesc")).GetAttributeSafe("value");
                var selDsaCode = DriverFactory.Browser.FindElementSafeV2(By.Name("selDSACode")).GetAttributeSafe("value");
                var selTsaCode = DriverFactory.Browser.FindElementSafeV2(By.Name("selTSACode")).GetAttributeSafe("value");
                var selDsaName = DriverFactory.Browser.FindElementSafeV2(By.Name("selDSAName")).GetAttributeSafe("value");
                var selTsaName = DriverFactory.Browser.FindElementSafeV2(By.Name("selTSAName")).GetAttributeSafe("value");
                var servicePhone = DriverFactory.Browser.FindElementSafeV2(By.Name("txtServicePhoneNumber")).GetAttributeSafe("value");
                var finAmountRequested = DriverFactory.Browser.FindElementSafeV2(By.Name("txtFinAmountRequested"))
                    .GetAttributeSafe("value");

                //=======================

                //2. Demographic: Error
                var tabDemographic = ScreenMap.TabDemographicElement;
                tabDemographic.ClickSafe(DriverFactory.Browser);
                //Click information user:
                var userName =
                    DriverFactory.Browser.FindElementSafeV2(By.XPath("//*[@id='formID184']/table[5]/tbody/tr[3]/td[1]/a"));
               
                if (userName == null || userName.Text != "")
                {
                    userName.ClickSafe(DriverFactory.Browser);
                }

                //2.1 Personal tab:
                var tabPersonal = ScreenMap.TabPersonalElement;
                tabPersonal.ClickSafe(DriverFactory.Browser);

                var txtTinNo = DriverFactory.Browser.FindElementSafeV2(By.Name("txtTINNo")).GetAttributeSafe("value");
                var txtAge = DriverFactory.Browser.FindElementSafeV2(By.Name("txtAge")).GetAttributeSafe("value");
                var selSex = "";
                var comboBox = DriverFactory.Browser.FindElementSafeV2(By.Name("selSex"));
                if (comboBox != null)
                {
                    var selectedValue = new SelectElement(comboBox);
                    selSex = selectedValue.SelectedOption.Text;
                }

                var selState = DriverFactory.Browser.FindElementSafeV2(By.Name("selState")).GetAttributeSafe("value");
                
                var mobile = DriverFactory.Browser.FindElementSafeV2(By.Name("txtMobile")).GetAttributeSafe("value");
                
                //2.2 Work Detail tab:
                var tabWork = DriverFactory.Browser.FindElementSafeV2(By.CssSelector("#apy_b1i2font"));
                
                tabWork.ClickSafe(DriverFactory.Browser);
                //Just get row by Name, don't need foreach all table.
                var txtCompnayName = DriverFactory.Browser.FindElementSafeV2(By.Name("txtCompnayName")).GetAttributeSafe("value");
                var companyName = txtCompnayName.Trim() != ""
                    ? txtCompnayName
                    : DriverFactory.Browser.FindElementSafeV2(By.Name("txtOtherEmpName")).GetAttributeSafe("value");
                

                //2.3 Address: ???
                var tabAddress = DriverFactory.Browser.FindElementSafeV2(By.CssSelector("#apy_b1i3font"));
                tabAddress.ClickSafe(DriverFactory.Browser);
                var listselStateThuongTru =
                    new List<IWebElement>(DriverFactory.Browser.FindElements(By.CssSelector("a[href^='javascript:updateFunc']")));
                var selStateThuongTru = "";
                foreach (var href in listselStateThuongTru)
                {
                    if (href.TextSafe() != "ĐỊA CHỈ THƯỜNG TRÚ") continue;
                    href.ClickSafe(DriverFactory.Browser);
                    selStateThuongTru = DriverFactory.Browser.FindElementSafeV2(By.Name("selState")).GetAttributeSafe("value");
                }
                //2.4 Income/Liability tab:
                var tabIncome = DriverFactory.Browser.FindElementSafeV2(By.CssSelector("#apy_b1i4font"));
                tabIncome.ClickSafe(DriverFactory.Browser);
                var inCome1 = DriverFactory.Browser.FindElementSafeV2(By.XPath("//*[@id='formID140']/table[7]/tbody/tr[2]/td[3]"))
                    .TextSafe();

                //3. References:
                var setReferences = "";
                if (_product == "PERSONAL")
                    setReferences = "#apy_b0i8font";
                if (_product == "AUTO")
                    setReferences = "#apy_b0i9font";
                var tabReferences = DriverFactory.Browser.FindElementSafeV2(By.CssSelector(setReferences));

                tabReferences.ClickSafe(DriverFactory.Browser);
                var strReferences = "";
                //Select all href link:
                var listRef =
                    new List<IWebElement>(
                        DriverFactory.Browser.FindElements(
                            By.CssSelector("a[href^='javascript:updateFunc']"))); //href="javascript:updateFunc

                foreach (var href in listRef)
                {
                    if (CancelTask())
                    {
                        break;
                    }
                    strReferences += href.TextSafe() + " :";
                    href.ClickSafe(DriverFactory.Browser);
                    strReferences += DriverFactory.Browser.FindElementSafeV2(By.Name("txtPhoneAreaCode")).GetAttributeSafe("value") + "; ";
                }

                //4. History: Get Select 2 last element with css
                var setHistory = "";
                if (_product == "PERSONAL")
                    setHistory = "#apy_b0i9font";
                if (_product == "AUTO")
                    setHistory = "#apy_b0i10font";

                var history = "";
                var tabHistory = DriverFactory.Browser.FindElementSafeV2(By.CssSelector(setHistory));
                tabHistory.ClickSafe(DriverFactory.Browser);
                try
                {
                    history += DriverFactory.Browser.FindElementSafeV2(
                            By.CssSelector(
                                "#formID9 > table:nth-child(84) > tbody > tr:nth-last-child(2) > td:nth-child(2) > font"))
                        .TextSafe();
                    if (history.Trim() != "")
                    {
                        history += "; ";
                    }
                    history += DriverFactory.Browser.FindElementSafeV2(
                            By.CssSelector(
                                "#formID9 > table:nth-child(84) > tbody > tr:nth-last-child(1) > td:nth-child(2) > font"))
                        .TextSafe();
                }
                catch (Exception exception)
                {
                    Logg.Error("History error:" + exception.Message);
                    throw;
                }

                var rec = new ReCord
                {
                    FullName = "",
                    Gender = selSex,
                    Age = txtAge,
                    IdCardNumber = txtTinNo,
                    Phone = mobile,
                    State = selState,
                    Stage = "",
                    Scheme = scheme,
                    Company = companyName,
                    Income = inCome1,
                    DsaCode = selDsaCode,
                    DsaName = selDsaName,
                    TsaCode = selTsaCode,
                    TsaName = selTsaName,
                    SaPhoneNumber = servicePhone,
                    ReferncesName = strReferences,
                    History = history,
                    FinAmountRequested = finAmountRequested,
                    StateThuongTru = selStateThuongTru
                };

                DriverFactory.Browser.Close();
                return rec;
            }
            catch (Exception exception)
            {
                Logg.Error(exception.Message);
                throw;
            }
        }

        /// <summary>
        /// Get data Table
        /// </summary>
        /// <param name="lxxReCords"></param>
        /// <param name="indexRecord"></param>
        /// <param name="oSheet"></param>
        private static void GetDataTable(List<ReCord> lxxReCords, ref int indexRecord, _Worksheet oSheet)
        {
            try
            {
                DriverFactory.Browser.WaitForPageLoad(Set.Fecredit.PageLoad.Value);
                var elemTable = DriverFactory.Browser.FindElementSafeV2(By.XPath("//*[@id='formID206']/table[4]"));
                // Fetch all Row of the table
                var lstTrElem = new List<IWebElement>(elemTable.FindElementsSafe(DriverFactory.Browser, By.TagName("tr")));
                var temp = 0;
                // Traverse each row
                var dateAsignFrom = DateTime.ParseExact(_signform, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var dateAsignTo = DateTime.ParseExact(_signto, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                foreach (var elemTr in lstTrElem)
                {
                    if (CancelTask())
                    {
                        break;
                    }
                    if (Equals(lstTrElem.First(), elemTr))
                    {
                        continue;
                    }
                    // Fetch the columns from a particuler row
                    var lstTdElem = new List<IWebElement>(elemTr.FindElementsSafe(DriverFactory.Browser, By.TagName("td")));
                    if (lstTdElem.Count > 0)
                    {
                        var detailHref = lstTdElem[0].TextSafe();
                        Logg.Error(@"Detail : " + detailHref);
                        //string applicationNo = 
                        var assigned = lstTdElem[1].TextSafe();
                        var fullNameRow = lstTdElem[2].TextSafe();
                        if (fullNameRow.Trim() == "") continue;
                        var dateAsignCurrent =
                            DateTime.ParseExact(assigned, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        if (dateAsignCurrent < dateAsignFrom || dateAsignCurrent >= dateAsignTo)
                        {
                            temp++;
                            continue;
                        }
                        //Click to detail1 Row:
                        //Or can be identified as href link 
                        //Check number
                        var isNumeric = detailHref.All(char.IsDigit);
                        if (!isNumeric)
                        {
                            continue;
                        }
                        Logg.Error(@"Get detail for: " + detailHref);
                        var element = DriverFactory.Browser.FindElementSafeV2(By.PartialLinkText(detailHref));

                        var recData = GetAllDetail(element);
                        Logg.Error(@"End Get detail for: " + detailHref);
                        if (recData == null)
                        {
                            recData = new ReCord();
                        }

                        // Create an array to multiple values at once.
                        //var arr = ((IEnumerable) recData).Cast<ReCord>()
                        //    .Select(x => x.ToString())
                        //    .ToArray();
                        var arrRecord = new List<string>();
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
                        arrRecord.Add(recData.FinAmountRequested);
                        arrRecord.Add(recData.StateThuongTru);
                        var indexRow = "A" + indexRecord;
                        var indexCol = "U" + indexRecord;
                        oSheet.Range[indexRow, indexCol].Value2 = arrRecord.ToArray();
                        //AutoFit columns A:D.
                        var oRng = oSheet.Range["A1", "V1"];
                        oRng.EntireColumn.AutoFit();
                        indexRecord++;
                    }
                    DriverFactory.Browser.SwitchTo().Window(_enquiryScreenWindow);
                } //End foreach

                //TODO: Stop when don't have record in duration.
                if (temp != lstTrElem.Count - 1) return;
                _nTry--;
                if (_nTry == 0)
                {
                    _stopNav = true;
                }
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }
    }
}
