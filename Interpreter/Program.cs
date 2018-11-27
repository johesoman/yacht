using System;
using Parser;

namespace Interpreter
{
  class MainClass
  {
    public static void Main(string[] args)
    {

      if (args.Length > 1 && args[0] == "-t")
      {
        var ast = Parser.Utility.Parse();
        Utility.Interpret(ast);
      }
    }
  }
}
