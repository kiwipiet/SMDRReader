using System;

namespace SMDRReader
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var service = new SMDRReaderService())
            {
                service.Start();
                Console.ReadKey();
            }
        }
    }
}
