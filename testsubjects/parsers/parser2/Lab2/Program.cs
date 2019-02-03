using System;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace Parser
{
    class ProgramMain
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
                StreamReader input;

                if (args[0] == "-t")
                    input = new StreamReader(Console.OpenStandardInput());
                else
                    input = new StreamReader(args[0]);

                string program = input.ReadToEnd();
                byte[] data = Encoding.ASCII.GetBytes(program);
                MemoryStream stream = new MemoryStream(data, 0, data.Length);
                Scanner scanner = new Scanner(stream);
                Parser parser = new Parser(scanner);

                if (parser.Parse())
                    Console.WriteLine("True"); //Console.WriteLine(parser.Program.ToString());
                else
                    Console.WriteLine("False");

                Console.WriteLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine();
            }
        }
    }
}
