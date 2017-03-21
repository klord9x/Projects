using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebDriverCloudLoadTesting.Pages.AutomateThePlanet
{
    public partial class BlogPage
    {
        public void AssertTitle()
        {
            Assert.AreEqual(this.driver.Title, "Blog - Automate The Planet");
        }
    }
}
