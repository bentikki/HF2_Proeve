using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZooCleaning
{
    static class Printer
    {
        public static void Print(string s)
        {
            string output = Thread.CurrentThread.ManagedThreadId.ToString("#00");
            Console.WriteLine($"Thread[{output}] {DateTime.Now.ToString("hh:mm:ss.fff")}: {s}");
        }
    }
}
