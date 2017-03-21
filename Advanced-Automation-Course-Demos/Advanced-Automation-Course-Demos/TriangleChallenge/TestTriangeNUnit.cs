using NUnit.Framework;

namespace Advanced_Automation_Course_Demos.TriangleChallenge
{
    [TestFixture]
    class TestTriangeNUnit
    {
        [TestCase(4, 4, 4, ExpectedResult = TriangleType.Equilateral)]
        [TestCase(4, 6, 6, ExpectedResult = TriangleType.Isosceles)]
        public TriangleType TestTriangleNUnit(double a, double b, double c)
        {
            return TriangleTester.GetTriangleType(a, b, c);
        }
    }
}