using System;
using System.IO;
using System.Text;

using Generators;

namespace Parser
{
  class MainClass
  {
    public static void Main(string[] args)
    {
      if (args.Length < 1)
      {
        var seeder = new Random(100);

        for (int outer = 0; outer <= 10; outer++)
        {
          var seed = seeder.Next();
          Console.WriteLine("Seed {0}", seed);
          var rnd = new Random(seed);

          for (int i = 0; i <= 100; i++)
          {
            var bolzmann = new Bolztmann<Program>(ProgramGenerator.Instance, 500, 0.3f);
            var ast = bolzmann.Generate(rnd);
            var original = ast.Pretty();

            byte[] data = Encoding.ASCII.GetBytes(original);
            MemoryStream stream = new MemoryStream(data, 0, data.Length);
            Scanner l = new Scanner(stream);
            Parser p = new Parser(l);

            if (!p.Parse())
            {
              Console.WriteLine(original);
              return;
            }

            var parsed = p.program.Pretty();

            if (parsed != original)
            {
              Console.WriteLine("Original program and parsed program not the same");
              Console.WriteLine("Original:");
              Console.WriteLine("-------------------------------------------");
              Console.WriteLine(original);
              Console.WriteLine("Parsed:");
              Console.WriteLine("-------------------------------------------");
              Console.WriteLine(parsed);
              return;
            }
            Console.Write(".");

          }
          Console.Write("\n");
        }
      }

      if (args.Length >= 1 && args[0] == "-t")
      {
        Scanner l = new Scanner(Console.OpenStandardInput());
        Parser p = new Parser(l);
                if(p.Parse()) {
                    Console.WriteLine(p.program.Pretty());
                }

                //Console.WriteLine(p.Parse());
      }

      /*
      string str = "t";
      byte[] data = Encoding.ASCII.GetBytes(str);
      MemoryStream stream = new MemoryStream(data, 0, data.Length);

      Scanner l = new Scanner(stream);
      Console.WriteLine((Tokens)l.yylex());
      Console.WriteLine((Tokens)l.yylex());
      Console.WriteLine((Tokens)l.yylex());
*/

  //		Parser p = new Parser(l);
  //		Console.WriteLine(p.Parse());

    }
  }
}
