using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WebDriverExamples.Pages.ImprovedPageObjects
{
    public partial class DietMenuGeneratorPage
    {
        public IWebElement AdditionalSugarCheckbox
        {
            get
            {
                return this.wait.Until(ExpectedConditions.ElementExists(By.Id("ninja_forms_field_18")));
            }
         }
        
        public IWebElement BurgersSelectElement
        {
            get
            {
                return this.driver.FindElement(By.Id("ninja_forms_field_21"));
            }
        }

        public IWebElement FirstNameTextInput
        {
            get
            {
                return this.driver.FindElement(By.XPath("//*[@id='ninja_forms_field_23']"));
            }
        }

        public IWebElement MainHeadline
        {
            get
            {
                return this.driver.FindElement(By.TagName("h2"));
            }
        }

        public IWebElement VentiRadioButton
        {
            get
            {
                return this.driver.FindElement(By.Id("ninja_forms_field_19"));
            }
        }
    }
}
