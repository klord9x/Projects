using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advanced_Automation_Course_Demos.TriangleChallenge
{
    public class TriangleTester
    {
        public static TriangleType GetTriangleType(double a, double b, double c)
        {
            if (a <= 0 || b <= 0 || c <= 0)
            {
                throw new ArgumentException("....");
            }
            //a + b > c
            //a + c > b
            //b + c > a
            if (a + b <= c)
            {
                throw new ArgumentException("....");
            }
            if (a + c <= b)
            {
                throw new ArgumentException("....");
            }
            if (b + c <= a)
            {
                throw new ArgumentException("....");
            }
            if (a == b && a == c) 
            {
                return TriangleType.Equilateral;
            }
            else if (a == b || a == c || b == c)
            {
                return TriangleType.Isosceles;
            }
            else
            {
                return TriangleType.Scalene;
            }
        }
    }

}
