using NUnit.Framework;
using System;
using Demos = Advanced_Automation_Course_Demos.TriangleChallenge;

namespace TriangleTester.Tests.NUnit
{
    [TestFixture]
    public class TriangeTesterTests
    {
        [Test]
        [TestCase(4, 4, 4, ExpectedResult = 3)]
        [TestCase(4, 6, 4, ExpectedResult = 2)]
        public int TestTrianglesTester(int a, int b, int c)
        {
            return (int)Demos.TriangleTester.GetTriangleType(a, b, c);
        }

        [Test]
        public void ThrowNewArgumentException_When_NegativeSides()
        {
            Assert.Throws<DllNotFoundException>(() => Demos.TriangleTester.GetTriangleType(0, 0, 0));
        }
    }
}
