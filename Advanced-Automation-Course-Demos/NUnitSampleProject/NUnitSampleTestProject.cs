using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnitSampleProject
{
    [TestFixture]
    public class NUnitSampleTestProject
    {
        [Test, Combinatorial]
        public void CombinatorialTest([Values(1, 2, 3)] int x, [Values("A", "B")] string s)
        {
            Console.WriteLine("x = {0}, s = {1}", x, s);
        }


        [Test]
        public void SampleNunitTest()
        {
        }
    }
}
