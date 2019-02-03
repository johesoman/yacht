using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Parser
{
    internal class MainProgram
    {
        private static void Main(string[] args)
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

                // Lab 2.1 Lexical and Syntactical Analysis
                Console.WriteLine(success);

                if (!success) return;

                // Lab 2.2 Pretty prnting
                // var b = new PrettyBuilder();
                // parser.Program.Pretty(b);
                // Console.WriteLine(b.ToString());

                // Lab 2.2 Pretty printing, checking condition 1

                // var pretty = b.ToString();
                // var i_strip = Strip(prg);
                // var o_strip = Strip(pretty);
                // if (i_strip.Equals(o_strip)) Console.WriteLine(success);

                // Lab 2.2 Pretty printing, checking condition 2

                // var data2 = Encoding.ASCII.GetBytes(pretty);
                // var stream2 = new MemoryStream(data2, 0, data2.Length);
                // var lexer2 = new Scanner(stream2);
                // var parser2 = new Parser(lexer2);
                // var success2 = parser2.Parse();
                // if (!success2) return;
                // var b2 = new PrettyBuilder();
                // parser2.Program.Pretty(b2);
                // var pretty2 = b2.ToString();
                // if (pretty.Equals(pretty2)) Console.WriteLine(success);

                // Lab 2.3 Interpretation
                // var env = new Environment();
                // parser.Program.Accept(ProgramEvaluator.Instance, env);

                // Lab 2.4 Type checking
                // var tenv = new TypeEnvironment();
                // parser.Program.Accept(ProgramTypeChecker.Instance, tenv);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static readonly Regex StripWhitespace = new Regex(@"\s+");

        public static string Strip(string input, string replacement = "")
        {
            return StripWhitespace.Replace(input, replacement);
        }
    }
}
