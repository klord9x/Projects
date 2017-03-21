using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WebDriverCloudLoadTesting.Pages.AutomateThePlanet
{
    public partial class HomePage
    {
        public void AssertHeadline()
        {
            Assert.IsTrue(this.MainHeadline.Text.Contains("Taking Software Quality to"));
        }
    }
}