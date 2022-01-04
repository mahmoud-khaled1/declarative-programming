using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Hello
    {
        private double x;
        private double y;

        public Hello(double x1,double y1 )
        {
            x = x1;
            y = y1;
        }
        public double GetZ(double v)
        {
            double z;
            if (x > 10)
                z = (x + y) / 2 + v;
            else
                z = (x - y) / 2 - v;

            return z;
        }

        public void UpdateX(double v)
        {
            x = y + 2 * v;
        }
        public void UpdateY(double v)
        {
            x = x - 2 * v;
        }
    }
}
