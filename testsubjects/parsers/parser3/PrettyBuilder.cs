using System;
using System.Text;
using System.Collections.Generic;

namespace Parser
{
    public class PrettyBuilder
    {

        StringBuilder builder;
        int indent = 0;

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

        public void NewLine()
        {
            builder.Append("\n");
            if (indent > 0)
            {
                builder.Append(new string(' ', indent));
            }
        }

        public void Append(string s)
        {
            builder.Append(s);
        }

        public override string ToString()
        {
            return builder.ToString();
        }

        public void Intersperse(IEnumerable<IPretty> pretties, string separator)
        {
            var first = true;
            foreach (var p in pretties)
            {
                if (!first)
                {
                    builder.Append(separator);
                }

                p.Pretty(this);
                first = false;
            }
        }

        public void Vertical(IEnumerable<IPretty> pretties)
        {
            var first = true;
            foreach (var p in pretties)
            {
                if (!first)
                {
                    NewLine();
                }

                p.Pretty(this);
                first = false;
            }
        }
    }

    public interface IPretty
    {
    void Pretty(PrettyBuilder b);
    }
}
