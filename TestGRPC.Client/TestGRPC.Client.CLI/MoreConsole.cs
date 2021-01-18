using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestGRPC.Client.CLI
{
    public static class MoreConsole
    {
        public static int ReadInt()
        {
            while (true)
            {
                string line = Console.ReadLine();
                if (int.TryParse(line, out int result))
                    return result;
                else
                    Console.WriteLine("Please enter a valid number:");
            }
        }
    }
}
