using System;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;


namespace Parser {
    
    
    class Program {

        static string strip (string s)
        {
            
            s = s.Trim();

            //remove all consecutive spaces, keep single space
            s = Regex.Replace(s, @"\s+", " ");

            s = s.Replace('\n', ' ');
            s = s.Replace('\r', ' ');
            s = s.Replace("\t", "");
            
            
           
            return s;
        }

        static string pretty(Parser p)
        {
            var b = new PrettyBuilder();
            p.Program.Pretty(b);
            return b.ToString();
        }

        static Parser parseFile(string program)
        {
            byte[] data = Encoding.ASCII.GetBytes(program);
            MemoryStream stream = new MemoryStream(data, 0, data.Length);
            Scanner lexer = new Scanner(stream);
            Parser parser = new Parser(lexer);
            parser.Parse();
            
            return parser;
        }

        static void Main(string[] args) {
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
                try
                {
                    Parser parser = parseFile(program);

                    var strip1 = strip(pretty(parser));
                    //Console.WriteLine(strip1);
                    var strip2 = strip(pretty(parseFile(strip1)));
                    //Console.WriteLine(strip2);

                    if (strip1 != strip2)
                        throw new NotImplementedException("PrettyPrint1 is not equal to PrettyPrint2.");

                    Console.WriteLine(true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(false);
                }
                

                

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.Read();
            Console.Read();


        }
    }
}
