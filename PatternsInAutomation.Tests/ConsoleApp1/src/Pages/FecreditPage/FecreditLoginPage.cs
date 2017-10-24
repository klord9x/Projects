using System;
using System.Linq;
using System.Windows.Forms;
using AutoDataVPBank.core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutoDataVPBank.Pages.FecreditPage
{
    public class FecreditLoginPage:BaseWorker
    {
        //private const string Name = @"CC100278";
        //private const string Password = @"Khoinguyen@2";
        private const string Url = @"https://cps.fecredit.com.vn/finnsso/gateway/SSOGateway?requestID=7000003";
        private static FecreditLoginPage _instance;
        //private const string Signed = "02/05/2016";
        //private const string SignedTo = "02/05/2016";

        public static FecreditLoginPageElementMap LoginMap => new FecreditLoginPageElementMap();
        public static FecreditLoginPage GetInstance => _instance ?? (_instance = new FecreditLoginPage());

        protected override void Process()
        {
            try
            {
                Library.Logg.Info("Start Browser");
                DriverFactory.StartBrowser();
                Navigate();
                Login(Library.MainForm.txtUser.Text, Library.MainForm.txtPass.Text, Library.MainForm.radioButtonCAS.Checked);
            }
            catch (Exception e)
            {
                Library.Logg.Error(e.Message);
                throw;
            }
        }


        public static void Navigate()
        {
            DriverFactory.Browser.NavigateSafe(Url);
        }

        /// <summary>
        /// #1. Login Url.
        /// </summary>
        public static void Login(string user, string pass, bool caSselect)
        {
            try
            {
                LoginMap.TxtNameElement.Clear();
                LoginMap.TxtNameElement.SendKeys(user);
                LoginMap.TxtPasswordElement.Clear();
                LoginMap.TxtPasswordElement.SendKeys(pass);
                LoginMap.DataActionElement.Click();
            }
            catch (Exception e)
            {
                Library.Logg.Error(e.Message);
                MessageBox.Show(@"Đăng nhập không thành công!");
            }
            var message = CheckAlert();
            ProcessPageOne(message, caSselect);
            ProcessPageTwo(caSselect);
            DriverFactory.Browser.SwitchTo().Window(DriverFactory.Browser.WindowHandles.Last());
            //Go to get detail:
            var enqui = new EnquiryScreenPage();
            enqui.EnquiryScreen();
        }

        private static void ProcessPageTwo(bool caSselect)
        {
            //switch to new window. Page 2
            DriverFactory.Browser.SwitchTo().Window(DriverFactory.Browser.WindowHandles.Last());
            DriverFactory.Browser.WaitForPageLoad(15);
            try
            {
                DriverFactory.Browser.SwitchTo().Frame("frameForwardToApp");
                DriverFactory.Browser.SwitchTo().Frame("contents");
                LoginMap.BtnPage2Click1Element.ClickSafe(DriverFactory.Browser);
                if (caSselect)
                {
                    LoginMap.BtnPage2CasClick2Element.ClickSafe(DriverFactory.Browser);
                }
                else LoginMap.BtnPage2Click2Element.ClickSafe(DriverFactory.Browser);

            }
            catch (WebDriverException e)
            {
                Library.Logg.Error(e.Message);
            }
            catch (Exception e)
            {
                Library.Logg.Error(e.Message);
                throw;
            }
        }

        private static void ProcessPageOne(string message, bool caSselect)
        {
            //switch to new window. Page 1
            DriverFactory.Browser.SwitchTo().Window(DriverFactory.Browser.WindowHandles.Last());
            EnquiryScreenPage.Page1WindowHandle = DriverFactory.Browser.CurrentWindowHandle;
            if (LoginMap.BtnPage1CasarchElement == null)
            {
                DriverFactory.StopBrowser();
                MessageBox.Show(message, @"Oh No!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Library.Logg.Error(@"Can't login");
                Application.Exit();
                return;
            }
            //TODO: Fail here
            if (caSselect)
            {
                LoginMap.BtnPage1CasElement.ClickSafe(DriverFactory.Browser);
            }
            else LoginMap.BtnPage1CasarchElement.ClickSafe(DriverFactory.Browser);
        }

        private static string CheckAlert()
        {
            /*Check have alert:
             User ID is already logged in. Do you wish to create a new session.
             */
            try
            {
                var wait = new WebDriverWait(DriverFactory.Browser, TimeSpan.FromSeconds(15));
                wait.Until(ExpectedConditions.AlertIsPresent());
                var alert = DriverFactory.Browser.SwitchTo().Alert();
                var message = alert.Text;
                Library.Logg.Error(message);
                alert.Accept();
                return message;
            }
            catch (Exception e)
            {
                Library.Logg.Error(e.Message);
                throw;
            }
        }

        public override void Teardown()
        {
            throw new NotImplementedException();
        }
    }
}
