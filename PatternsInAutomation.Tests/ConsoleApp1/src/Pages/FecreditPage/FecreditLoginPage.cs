using System;
using System.Linq;
using System.Windows.Forms;
using AutoDataVPBank.core;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using static AutoDataVPBank.core.DriverFactory;
using static AutoDataVPBank.Library;

namespace AutoDataVPBank.Pages.FecreditPage
{
    public class FecreditLoginPage:BaseWorker
    {
        private static FecreditLoginPage _instance;
        private static string _loginWindowHandle;
        private static FecreditLoginPageElementMap LoginMap => new FecreditLoginPageElementMap();
        public static FecreditLoginPage GetInstance => _instance ?? (_instance = new FecreditLoginPage());

        protected override void Process()
        {
            try
            {
                Logg.Info("Start Browser");
                MForm.cboActive.Invoke((MethodInvoker)delegate
                {
                    var type = MForm.cboBrowser.Text == @"Chrome" ? BrowserTypes.Chrome : BrowserTypes.Firefox;
                    StartBrowser(type);
                });
                
                Browser.NavigateSafe(Set.Fecredit.Urls.Login);
                Login(MForm.txtUser.Text, MForm.txtPass.Text);
                var message = CheckAlert();
                ProcessPageOne(message, MForm.radioButtonCAS.Checked);
                ProcessPageTwo(MForm.radioButtonCAS.Checked);
                Browser.SwitchTo().Window(Browser.WindowHandles.Last());
                //Go to get detail:
                EnquiryScreenPage.GetInstance.EnquiryScreen();
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
            finally
            {
                Stop();
            }
        }

        /// <summary>
        /// #1. Login Url.
        /// </summary>
        public static void Login(string user, string pass)
        {
            try
            {
                Browser.SwitchTo().Window(Browser.WindowHandles.First());
                LoginMap.TxtNameElement.Clear();
                LoginMap.TxtNameElement.SendKeys(user);
                LoginMap.TxtPasswordElement.Clear();
                LoginMap.TxtPasswordElement.SendKeys(pass);
                LoginMap.DataActionElement.Click();
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                MessageBox.Show(@"Đăng nhập không thành công!");
            }
        }

        private static void ProcessPageTwo(bool caSselect)
        {
            try
            {
                //switch to new window. Page 2
                Browser.SwitchTo().Window(Browser.WindowHandles.Last());
                Browser.WaitForPageLoad(Set.Fecredit.PageLoad.Value);
                Browser.SwitchTo().Frame("frameForwardToApp");
                Browser.SwitchTo().Frame("contents");
                //TODO: Improve, click fast not see button:
                if (caSselect)
                {
                    do
                    {
                        LoginMap.BtnPage2Click1Element.ClickSafe(Browser);
                    } while (LoginMap.BtnPage2CasClick2Element == null);
                    LoginMap.BtnPage2CasClick2Element.ClickSafe(Browser);
                }
                else
                {
                    do
                    {
                        LoginMap.BtnPage2Click1Element.ClickSafe(Browser);
                    } while (LoginMap.BtnPage2Click2Element == null);
                    LoginMap.BtnPage2Click2Element.ClickSafe(Browser);
                }

            }
            catch (WebDriverException e)
            {
                Logg.Error(e.Message);
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }

        private static void ProcessPageOne(string message, bool caSselect)
        {
            try
            {
                //switch to new window. Page 1
                Browser.SwitchTo().Window(Browser.WindowHandles.Last());
                _loginWindowHandle = Browser.CurrentWindowHandle;
                if (LoginMap.BtnPage1CasarchElement == null)
                {
                    MessageBox.Show(message, @"Oh No!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Logg.Error(@"Can't login");
                    throw new Exception(message);
                }

                if (caSselect)
                {
                    LoginMap.BtnPage1CasElement.ClickSafe(Browser);
                }
                else LoginMap.BtnPage1CasarchElement.ClickSafe(Browser);
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }

        private static string CheckAlert()
        {
            /*Check have alert:
             User ID is already logged in. Do you wish to create a new session.
             */
            try
            {
                var wait = new WebDriverWait(Browser, TimeSpan.FromSeconds(Set.Fecredit.Default.Value));
                wait.Until(ExpectedConditions.AlertIsPresent());
                var alert = Browser.SwitchTo().Alert();
                var message = alert.Text;
                Logg.Error(message);
                alert.Accept();
                return message;
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }

        public override void Teardown()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// After Click search on Enqiry Screen Page > Logout.
        /// </summary>
        public static void Logout()
        {
            try
            {
                Browser.SwitchTo().Window(_loginWindowHandle);
                LoginMap.BtnPage1ExitElement.Click();
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                throw;
            }
        }
    }
}
