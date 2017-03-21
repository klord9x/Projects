using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using NUnit.Framework;

namespace WebDriverExamples.Pages.ImprovedPageObjects
{
    public partial class DietMenuGeneratorPage
    {
        public void AssertHeadline()
        {
            Assert.AreEqual("Awesome Healthy Menu Generator", mainHeadline.Text, "");
        }
    }
}
