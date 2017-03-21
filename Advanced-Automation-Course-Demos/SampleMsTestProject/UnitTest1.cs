using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleMsTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext testContext)
        {
            //.....
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            //.....
        }

        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod]
        [TestCategory("Nightly")]
        [TestCategory("CI")]
        [TestCategory("Admin")]
        [Description("test....")]
        public void GetNameFromDb_When_Condition()
        {
            // Arrange
            var firstPerson = new Person()
            {
                FirstName = "Anton",
                LastName = "Angelov"
            };

            // Act
            var dbPerson = new Person()
            {
                FirstName = "Ivan",
                LastName = "Angelov"
            };

            // Assert
            Assert.AreEqual(firstPerson.FirstName, dbPerson.FirstName, "The first name was not as expected.");
        }
    }
}
