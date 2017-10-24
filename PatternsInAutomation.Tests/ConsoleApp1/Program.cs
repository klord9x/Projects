//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Firefox;
//using OpenQA.Selenium.Support.UI;
//using PatternsInAutomation.Tests.Beginners;
//using PatternsInAutomation.Tests.Beginners.Selenium.Bing.Pages;
//using System;
//using ConsoleApp1.Beginners.Pages.FecreditLogin;
//using POP = PatternsInAutomation.Tests.Beginners.Pages.BingMainPage;
//using OpenQA.Selenium.Chrome;
//using System.IO;

using System;
using System.Windows.Forms;
namespace AutoDataVPBank
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                Application.Run(MainForm.GetInstance);
            }
            catch (StackOverflowException e)
            {
                Console.WriteLine(@"Error caught: {0}", e);
            }
            
        }


    }
}
