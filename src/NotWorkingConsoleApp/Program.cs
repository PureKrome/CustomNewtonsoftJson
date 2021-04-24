using Newtonsoft.Json;
using System;

namespace NotWorkingConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Proof -> my custom Newtonsoft.Json dll is being used.
            Console.WriteLine(JsonConvert.HelloWorldTest());
        }
    }
}
