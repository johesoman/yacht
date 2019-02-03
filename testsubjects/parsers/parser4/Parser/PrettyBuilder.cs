using System.Collections.Generic;
using System.Text;

namespace Parser
{
    public class PrettyBuilder
    {
        private readonly StringBuilder _builder;
        private int _indent;

        public PrettyBuilder()
        {
            _builder = new StringBuilder();
        }

        public void Indent()
        {
            _indent += 2;
        }

        public void Unindent()
        {
            _indent -= 2;
        }

        public void NewLine()
        {
            _builder.Append("\n");
            if (_indent > 0) _builder.Append(new string(' ', _indent));
        }

        public void Append(string s)
        {
            _builder.Append(s);
        }

        public override string ToString()
        {
            return _builder.ToString();
        }

        public void Intersperse(IEnumerable<IPretty> pretties, string separator)
        {
            var first = true;
            foreach (var p in pretties)
            {
                if (!first) _builder.Append(separator);

                p.Pretty(this);
                first = false;
            }
        }

        public void Vertical(IEnumerable<IPretty> pretties)
        {
            var first = true;
            foreach (var p in pretties)
            {
                if (!first) NewLine();

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