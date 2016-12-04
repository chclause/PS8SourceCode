using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caches
{
    class DirectMappedCache
    {

        // Maps a row to a tag
        Dictionary<string, string> DMappedCache;
        string[] IsCached;
        string[] Data;

        public DirectMappedCache(int[] data)
        {
            DMappedCache = new Dictionary<string, string>();
            Data = DataToBinaryStrings(data);
            IsCached = new string[8];
        }


        // Convert the addresses to binary strings
        public string[] DataToBinaryStrings(int[] data)
        {
            string[] dataStrings = new string[data.Length];
            string currentBinaryNum;
            for (int i = 0; i < data.Length; i++)
            {
                // Convert it to a binary string
                currentBinaryNum = Convert.ToString(data[i], 2);
                int length = currentBinaryNum.Length;
                // Prepend 0's
                if (currentBinaryNum.Length < 16)
                {
                    for (int j = 0; j < (16 - length); j++)
                    {
                        currentBinaryNum = "0" + currentBinaryNum;
                    }
                }
                dataStrings[i] = currentBinaryNum;
            }
            return dataStrings;
        }


        // Get the 15-6 bits (10 bits)
        public string GetTag(string data)
        {
            string tag = "";
            char c;
            for (int i = 0; i < 10; i++)
            {
                c = data[i];
                tag = tag + c;
            }
            return tag;
        }

        // Get the row number
        public string GetRow(string data)
        {
            string row = "";
            char c;
            for (int i=10; i<13; i++)
            {
                c = data[i];
                row = row + c;
            }
            return row;
        }


        // The brains of the operation 
        public void SimulateDirectMappedCache()
        {
            string currentTag;
            string currentRow;
            int hitCount = 0;
            int missCount = 0;
            // Run the program loop a few times
            for (int i=0; i<2; i++)
            {
                hitCount = 0;
                missCount = 0;
                // Loop through the addresses
                for (int j=0; j<Data.Length; j++)
                {
                    currentTag = GetTag(Data[j]);
                    currentRow = GetRow(Data[j]);
                    Console.WriteLine("Accessing " + Convert.ToInt32(Data[j], 2) + " (tag " + Convert.ToInt32(currentTag, 2) + ")");
                     // If the row has something in it
                     if (DMappedCache.ContainsKey(currentRow))
                     {
                        if (DMappedCache[currentRow].Equals(currentTag))
                        {
                            Console.WriteLine("Hit from row " + Convert.ToInt32(currentRow, 2));
                            if (i>0)
                            {
                                hitCount++;
                            }
                        }
                        else
                        {
                            
                            Console.WriteLine("Miss, replacing row " + Convert.ToInt32(currentRow, 2));
                            DMappedCache.Remove(currentRow);
                            DMappedCache.Add(currentRow, currentTag);
                            IsCached[Convert.ToInt32(currentRow, 2)] = Data[j];
                            if (i>0)
                            {
                                missCount++;
                            }
                        }
                    }
                     else
                    {
                        DMappedCache[currentRow] = currentTag;
                        IsCached[Convert.ToInt32(currentRow, 2)] = Data[j];
                    }
                }
                // Print the contents of the cache
                int count = 0;
                foreach (KeyValuePair<string, string> row in DMappedCache)
                {
                    Console.WriteLine("Address: " + Convert.ToInt32(IsCached[count], 2) + "     Row: " + Convert.ToInt32(row.Key, 2) + "     Tag: " + 
                        Convert.ToInt32(row.Value, 2));
                    count++;
                }
                Console.WriteLine("Hits: " + hitCount + "     Misses: " + missCount);

                // This is (miss penalty*miss count + hit penalty*hit count) / Number of instructions
                float CPI = (((18 + 3 * 8) * missCount) + hitCount) / Data.Length;
                Console.WriteLine("CPI: " + CPI.ToString());
            }
        }
    }
}
