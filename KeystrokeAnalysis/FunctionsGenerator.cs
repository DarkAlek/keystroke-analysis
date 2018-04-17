using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeystrokeAnalysis
{
    class FunctionsGenerator
    {
        public long ManhattanDistance(List<long> l1, List<long> l2)
        {
            int len = l1.Count < l2.Count ? l1.Count : l2.Count;
            long distance = 0;

            for (int i = 0; i < len ; ++i )
            {
                distance = distance + Math.Abs(l1[i] - l2[i]);
            }

            return distance;
        }
    }
}
