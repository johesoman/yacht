using System;
using System.IO;
using System.Text;

namespace Parser
{

  public class ParseError : Exception
  {
    public ParseError(string msg, int line, int column) : base(MakeMessage(msg, line, column))
    {
    }

    public static string MakeMessage(string msg, int line, int column)
    {
      return $"Parse error. {msg} on line {line} column {column}";
    }
  }

  public static class Utility
  {

    public static Program Parse()
    {
      Scanner l = new Scanner(Console.OpenStandardInput());
      Parser p = new Parser(l);
      p.Parse();
      return p.program;
    }

    public static Program Parse(string program)
    {
      byte[] data = Encoding.ASCII.GetBytes(program);
      MemoryStream stream = new MemoryStream(data, 0, data.Length);

      Scanner l = new Scanner(stream);
      Parser p = new Parser(l);
      p.Parse();
      return p.program;
    }
  }
}

