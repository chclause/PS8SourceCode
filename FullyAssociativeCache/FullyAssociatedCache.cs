using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caches
{
    class FullyAssociatedCache
    {
        public class Row
        {
            public string BinaryNum { get; set; }
            public Row(string binaryNum)
            {
                BinaryNum = binaryNum;
            }

            public string Tag
            {
                get { return GetTag(); }
            }

            private string GetTag()
            {
                string tag = "";
                char c;
                for (int i =0; i<13; i++)
                {
                    c = BinaryNum[i];
                    tag = tag + c;
                }
                return tag;
            }
        }



        string[] Data;
        // Contains all the Rows that make up the current cache
        Row[] AssociativeCache;

        // Immediately makes a string[] of binary numbers that represent the memory addresses
        public FullyAssociatedCache(int[] data)
        {
            Data = DataToBinaryStrings(data);
            AssociativeCache = new Row[8];
            SetInitialValues();
        }

        // Initialize the cache so no errors are thrown
        private void SetInitialValues()
        {
            for (int i=0; i<AssociativeCache.Length;i++)
            {
                AssociativeCache[i] = new Row(Data[i]);
            }
        }



        // The brains of it all
        public void SimulateCacheLoop()
        {
            // Loop through each data value multiple times to simulate the program loop
            for (int i=0; i<2; i++)
            {
                int hitCount = 0;
                int missCount = 0;
                Console.WriteLine("Start Loop: ");
                bool hit = false;
                int dec;
                
                // Loop through all the binary numbers that represent the memory addresses
                foreach (string address in Data)
                {
                    hit = false;
                    dec = Convert.ToInt32(address, 2);
                    Console.WriteLine("Accessing: " + dec + " (tag " + Convert.ToInt32(GetTag(address), 2)+")");
                    Console.WriteLine();
                    // Check each row in the catch for the tag match
                    foreach (Row row in AssociativeCache)
                        {
                            // The data is in the cache, it is a hit because their 13 bit tags match
                            if (row.Tag.Equals(GetTag(address)))
                            {
                                Console.WriteLine("HIT using tag of " + Convert.ToInt32(row.BinaryNum, 2));    
                                Console.WriteLine("");
                                hit = true;
                                if (i > 0)
                                {
                                    hitCount++;
                                }
                                break;
                            }
                        }

                    // No tags in the cache matched
                    if (!hit)
                    {
                        Console.WriteLine("Miss, Replacing LRU (address): " + Convert.ToInt32(AssociativeCache[0].BinaryNum, 2));
                        AssociativeCache = UpdateCache(AssociativeCache, address);
                        Console.WriteLine();
                        if (i > 0)
                        {
                            missCount++;
                        }
                    }
                }
                Console.WriteLine("");
                Console.WriteLine("End Loop");
                Console.WriteLine("");
                // Print out the end values of the cache
                Console.WriteLine("Ending addresses cached: ");
                foreach(Row row in AssociativeCache)
                {
                    Console.WriteLine("Address: " + Convert.ToInt32(row.BinaryNum,2) + "    Tag: " + Convert.ToInt32(GetTag(row.BinaryNum), 2));
                }
                Console.WriteLine();
                Console.WriteLine("Hits: " + hitCount);
                Console.WriteLine("Misses: " + missCount);
                // This is (miss penalty*miss count + hit penalty*hit count) / Number of instructions
                float CPI = (((18 + 3 * 8) * missCount) + hitCount) / Data.Length;
                Console.WriteLine("CPI: " + CPI.ToString());
            }
        }


        // Delete the LRU, which is the cache[0] and shift everything down one, add the new item.
        public Row[] UpdateCache(Row[] cache, string newItem)
        {
            Row[] newCache = new Row[cache.Length];
            for (int i=1; i<cache.Length; i++)
            {
                newCache[i - 1] = cache[i];
            }
            Row newRow = new Caches.FullyAssociatedCache.Row(newItem);
            newCache[newCache.Length-1] = newRow;
            return newCache;
        }


        // Convert all memory items to a binary string representation
        public string[] DataToBinaryStrings(int[] data)
        {
            string[] dataStrings = new string[data.Length];
            string currentBinaryNum;
            for (int i=0; i<data.Length; i++)
            {
                // Convert it to a binary string
                currentBinaryNum = Convert.ToString(data[i], 2);
                int length = currentBinaryNum.Length;
                // Prepend 0's
                if (currentBinaryNum.Length < 16)
                {
                    for (int j = 0; j<(16-length); j++)
                    {
                        currentBinaryNum = "0" + currentBinaryNum;
                    }
                }
                dataStrings[i] = currentBinaryNum;
            }
            return dataStrings;
        }

        // Get a tag from first most significant 13 bits of a 16 bit address
        public string GetTag(string data)
        {
            string tag = "";
            char c;            
            for (int i = 0; i < 13; i++)
            {
                c = data[i];
                tag = tag + c;
            }
            return tag;
        }
    }
}
