using System;
using System.Drawing;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using static AutoDataVPBank.Beginners.Pages.FecreditPage.Library;
using static AutoDataVPBank.Beginners.Pages.FecreditPage.RandomNumber;

namespace AutoDataVPBank.Beginners.Pages.FecreditPage
{
    public static class DriverFactory
    {
        private static WebDriverWait _browserWait;

        private static IWebDriver _browser;

        public static IWebDriver Browser
        {
            get
            {
                if (_browser == null)
                    throw new NullReferenceException(
                        "The WebDriver browser instance was not initialized. You should first call the method Start.");
                return _browser;
            }
            private set => _browser = value;
        }

        public static WebDriverWait BrowserWait
        {
            get
            {
                if (_browserWait == null || _browser == null)
                    throw new NullReferenceException(
                        "The WebDriver browser wait instance was not initialized. You should first call the method Start.");
                return _browserWait;
            }
            private set => _browserWait = value;
        }

        public static void StartBrowser(BrowserTypes browserType = BrowserTypes.Firefox, int defaultTimeOut = 69)
        {
            switch (browserType)
            {
                case BrowserTypes.Firefox:
                    FireFox();
                    break;
                case BrowserTypes.InternetExplorer:
                    break;
                case BrowserTypes.Chrome:
                    Chrome();
                    break;
                case BrowserTypes.NotSet:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(browserType), browserType, null);
            }

            BrowserWait = new WebDriverWait(Browser, TimeSpan.FromSeconds(defaultTimeOut));
            Browser.Manage().Timeouts().PageLoad = TimeOuts;
            Browser.Manage().Timeouts().AsynchronousJavaScript = TimeOuts;

            var size = Browser.Manage().Window.Size;
            size.Width = size.Width - Between(200, 600);
            size.Height = size.Height + Between(200, 400);
            Browser.Manage().Window.Size = size;
            Browser.Manage().Window.Position = new Point(0, 0);
        }

        public static void StopBrowser()
        {
            Browser.Quit();
            Browser = null;
            BrowserWait = null;
        }

        private static void FireFox()
        {
            try
            {
                var service = FirefoxDriverService.CreateDefaultService();
                var options = new FirefoxOptions();
                var profile = new FirefoxProfile();
                //service.HideCommandPromptWindow = true;
                profile.Clean();
                profile.DeleteAfterUse = true;
                profile.SetPreference("browser.privatebrowsing.autostart", false);
                //Fix can't upload file
                profile.SetPreference("dom.file.createInChild", true);
                //profile.SetPreference("general.useragent.override", userAgent);
                options.Profile = profile;

                Browser = new FirefoxDriver(service, options, TimeOuts);
            }
            catch (WebDriverException e)
            {
                Logg.Error(e.Message);
                KillProcess();
                throw;
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                KillProcess();
                throw;
            }
        }

        private static void Chrome(string userAgent = null)
        {
            try
            {
                var chromeDriverService = ChromeDriverService.CreateDefaultService();
                var options = new ChromeOptions();
                //var mobi = new ChromeMobileEmulationDeviceSettings(_acc.UserAgent);
                chromeDriverService.HideCommandPromptWindow = true;
                if (!string.IsNullOrEmpty(userAgent))
                {
                    options.AddArgument("--user-agent=" + userAgent);
                }
                
                // add parameter which will disable the extension
                options.AddArgument("--disable-extensions");
                options.AddArgument("disable-infobars");
                Browser = new ChromeDriver(chromeDriverService, options, TimeOuts);
            }
            catch (WebDriverException e)
            {
                Logg.Error(e.Message);
                KillProcess();
                throw;
            }
            catch (Exception e)
            {
                Logg.Error(e.Message);
                KillProcess();
                throw;
            }
        }
    }
}