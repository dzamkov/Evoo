using System;
using System.Collections.Generic;
using System.Text;

namespace Evoo
{
    class Util
    {
        public static double RealTime()
        {
            return DateTime.Now.Ticks / 10000000.0;
        }
    }
}
