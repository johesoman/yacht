using System;
using System.Collections.Generic;

namespace Generators
{
  public class NumberGenerator : SizedGenerator<string>
  {
    static string digits = "0123456789";

    public NumberGenerator(int min, int max) : base(min, max) { }
    public NumberGenerator() : base(1, 4) { }

    override public string Generate(Random rnd)
    {
      var size = Within(rnd);
      var chars = new char[size];
      chars[0] = digits[rnd.Next(digits.Length - 2) + 1];

      for (int i = 1; i < size; i++)
      {
        chars[i] = digits[rnd.Next(digits.Length - 1)];
      }

      return new string(chars);
    }
  }


  public class IdentifierGenerator : SizedGenerator<string>
  {
    static string alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    static string alphanum = alpha + "0123456789";

    static HashSet<string> forbidden =
      new HashSet<string> { "if", "else", "while", "return", "int", "bool", "void", "true", "false"};

    public IdentifierGenerator(int min, int max) : base(min, max) { }
    public IdentifierGenerator() : base(1, 4) { }

    override public string Generate(Random rnd)
    {
      var size = Within(rnd);
      var chars = new char[size];

      while (true)
      {
        chars[0] = alpha[rnd.Next(alpha.Length - 1)];
        for (int i = 1; i < size; i++)
        {
          chars[i] = alphanum[rnd.Next(alphanum.Length - 1)];
        }

        var str = new string(chars);
        if (!forbidden.Contains(str)) return str;

      }
    }
  }
}
