using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advanced_Automation_Course_Demos.TriangleChallenge
{
    [TestClass]
    public class MSTestTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        [DataSource("System.Data.OleDB",
@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=.\DataSheet.xlsx; Extended Properties='Excel 12.0;HDR=yes';",
                      "Sheet1$",
  DataAccessMethod.Sequential
)]
        [DeploymentItem(".\\DataSheet.xlsx")]
        public void DataDrivenTest()
        {
            var a = (double)_testContext.DataRow[0]; //(int)Column.UserId
            var b = (double)_testContext.DataRow[1];
            var c = (double)_testContext.DataRow[2];
            TriangleType type = TriangleTester.GetTriangleType(a, b, c);

            Assert.AreEqual(TriangleType.Equilateral, type);
        }
    }
}
