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
                StreamReader input;

                if (args[0] == "-t")
                {
                    input = new StreamReader(Console.OpenStandardInput());
                }
                else
                {
                    input = new StreamReader(args[0]);
                }

                Scanner lexer = new Scanner(input.BaseStream);
                var parser = new Parser(lexer);
                var ast = parser.Parse();
                Console.WriteLine(ast);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            /*
            var prg = "void foo(int a, int b) { int x; }";
            byte[] data = Encoding.ASCII.GetBytes(prg);
            MemoryStream stream = new MemoryStream(data, 0, data.Length);
            Scanner lexer = new Scanner(stream);
            Parser parser = new Parser(lexer);
            Console.WriteLine(parser.Parse());
            //Console.WriteLine(parser.Program);
            */
        }
    }
}