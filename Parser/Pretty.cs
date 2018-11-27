using System;
using System.Collections.Generic;
using System.Text;

namespace Parser
{

  public class PrettyBuilder
  {
    public int indent;
    public StringBuilder builder;

    public PrettyBuilder()
    {
      builder = new StringBuilder();
    }

    public void Indent()
    {
      indent += 2;
    }

    public void Unindent()
    {
      indent -= 2;
    }

    public void Append(string s)
    {
      builder.Append(s);
    }

    public void NewLine()
    {
      builder.Append("\n");
      if (indent > 0) builder.Append(new string(' ', indent));
    }

    public void Intersperse(IEnumerable<IPretty> pretties, string separator)
    {
      var first = true;
      foreach (var p in pretties)
      {
        if (!first) Append(separator);
        first = false;
        p.Pretty(this);
      }
    }

    public void Vertical(IEnumerable<IPretty> pretties)
    {
      foreach (var p in pretties)
      {
        NewLine();
        p.Pretty(this);
      }
    }


    public string Layout()
    {
      return builder.ToString();
    }
  }



  public interface IPretty
  {
    void Pretty(PrettyBuilder pb);
  }



}
