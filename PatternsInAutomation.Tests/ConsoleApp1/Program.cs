﻿//using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace AutoDataVPBank
{
    class Program
    {
        

        //static void Main(string[] args)
        //{
        //    Program a = new Program();
        //    a.SetupTest();
        //    a.LoginFinnOne();
        //}
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }


    }
}
