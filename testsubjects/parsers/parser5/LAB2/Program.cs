using System;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Parser
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Usage; {0} [-t | <filename>]", Process.GetCurrentProcess().ProcessName);
                return;
            }

            try
            {
                var input = args[0] == "-t" ? new StreamReader(Console.OpenStandardInput()) : new StreamReader(args[0]);

                var prg = input.ReadToEnd();
                var data = Encoding.ASCII.GetBytes(prg);
                var stream = new MemoryStream(data, 0, data.Length);
                var lexer = new Scanner(stream);
                var parser = new Parser(lexer);
                var success = parser.Parse();

                Console.WriteLine(success);

                if (!success) return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
