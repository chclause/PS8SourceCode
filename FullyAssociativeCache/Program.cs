using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caches
{
    class Program
    {
        static void Main(string[] args)
        {
            // Location in memory in decimal
            int[] Data = new int[] { 4, 8, 20, 24, 28, 36, 44, 20, 24, 28, 36, 40, 44, 68, 72,
                92, 96, 100, 104, 108, 112, 100, 112, 116, 120, 128, 140 };

            FullyAssociatedCache fullAssociativeCache = new FullyAssociatedCache(Data);
            fullAssociativeCache.SimulateCacheLoop();
            DirectMappedCache directMappedCache = new DirectMappedCache(Data);
            directMappedCache.SimulateDirectMappedCache();




            Console.ReadKey();
        }
    }
}
