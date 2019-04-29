using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuccaDevises
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Usage: LuccaDevises <path to file>");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }

            try
            {
                var convertor = new CurrencyConvertor(args[0]);
                convertor.Convert();
            }
            catch(Exception ex) //TODO: exception handling
            {
                Console.WriteLine("An error occured while reading file...");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
