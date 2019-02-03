using System.Collections.Generic;

namespace Parser
{
    public partial class Program
    {
        public void Pretty(PrettyBuilder b)
        {
            b.Vertical(Body);
        }
    }

    public partial class FormalDeclaration
    {
        private static readonly Dictionary<IdType, string> Var = new Dictionary<IdType, string>
        {
            {IdType.Int, "int "},
            {IdType.Bool, "bool "},
            {IdType.Void, "void "}
        };

        public override void Pretty(PrettyBuilder b)
        {
            b.Append(Var[Type]);
            b.Append(Id);
            b.Append("(");
            if (FormalList.Count > 0) b.Intersperse(FormalList, ", ");
            b.Append(") ");
            b.Append("{");
            if (Statements.Count > 0)
            {
                b.Indent();
                b.NewLine();
                b.Vertical(Statements);
                b.Unindent();
            }

            b.NewLine();
            b.Append("}");
            b.NewLine();
        }
    }

    public partial class Formal
    {
        private static readonly Dictionary<IdType, string> Var = new Dictionary<IdType, string>
        {
            {IdType.Int, "int "},
            {IdType.Bool, "bool "}
        };

        public void Pretty(PrettyBuilder b)
        {
            b.Append(Var[Type]);
            b.Append(Id);
        }
    }

    public partial class BlockStatement
    {
        public override void Pretty(PrettyBuilder b)
        {
            b.Append("{");
            if (Body.Count > 0)
            {
                b.Indent();
                b.NewLine();
                b.Vertical(Body);

                b.Unindent();
            }

            b.NewLine();
            b.Append("}");
        }
    }

    public partial class IfStatement
    {
        public override void Pretty(PrettyBuilder b)
        {
            b.Append("if (");
            Condition.Pretty(b);
            b.Append(") ");
            Consequent.Pretty(b);
            b.Append(" else ");
            Alternate.Pretty(b);
        }
    }

    public partial class WhileStatement
    {
        public override void Pretty(PrettyBuilder b)
        {
            b.Append("while (");
            Condition.Pretty(b);
            b.Append(") ");
            Consequent.Pretty(b);
        }
    }

    public partial class ReturnStatement
    {
        public override void Pretty(PrettyBuilder b)
        {
            b.Append("return ");
            Expression.Pretty(b);
            b.Append("; ");
        }
    }

    public partial class ExpressionStatement
    {
        public override void Pretty(PrettyBuilder b)
        {
            Expression.Pretty(b);
            b.Append("; ");
        }
    }

    public partial class FormalStatement
    {
        public override void Pretty(PrettyBuilder b)
        {
            Formal.Pretty(b);
            b.Append("; ");
        }
    }


    public abstract partial class Expression
    {
        public virtual void Pretty(PrettyBuilder b, int parentPrecedence, bool opposite)
        {
            Pretty(b);
        }

        public virtual void Pretty(PrettyBuilder b, int parentPrecedence)
        {
            Pretty(b);
        }
    }

    public partial class IdentifierExpression
    {
        public override void Pretty(PrettyBuilder b)
        {
            b.Append(Id);
        }
    }

    public partial class NumberExpression
    {
        public override void Pretty(PrettyBuilder b)
        {
            b.Append(Num.ToString());
        }
    }

    public partial class BooleanExpression
    {
        public override void Pretty(PrettyBuilder b)
        {
            b.Append(Boolean ? "true" : "false");
        }
    }

    public partial class AssignmentExpression
    {
        public override void Pretty(PrettyBuilder b)
        {
            Pretty(b, 0, false);
        }

        public override void Pretty(PrettyBuilder b, int parentPrecedence)
        {
            const int precedence = 1;
            const Associativity associativity = Associativity.Right;

            var parens = parentPrecedence > precedence;

            if (parens) b.Append("(");

            b.Append(Id);
            b.Append(" = ");
            Expression.Pretty(b, precedence, associativity == Associativity.Left);

            if (parens) b.Append(")");
        }

        public override void Pretty(PrettyBuilder b, int parentPrecedence, bool opposite)
        {
            const int precedence = 1;
            const Associativity associativity = Associativity.Right;

            var parens = parentPrecedence > precedence || parentPrecedence == precedence && opposite;

            if (parens) b.Append("(");

            b.Append(Id);
            b.Append(" = ");
            Expression.Pretty(b, precedence, associativity == Associativity.Left);

            if (parens) b.Append(")");
        }
    }

    public partial class BinaryOperatorExpression
    {
        private static readonly Dictionary<Type, int> Precedences = new Dictionary<Type, int>
        {
            {Type.Or, 2},
            {Type.And, 3},
            {Type.Eql, 4},
            {Type.NEql, 4},
            {Type.Grt, 5},
            {Type.Less, 5},
            {Type.GEql, 5},
            {Type.LEql, 5},
            {Type.Add, 6},
            {Type.Sub, 6},
            {Type.Mul, 7},
            {Type.Div, 7}
        };

        private static readonly Dictionary<Type, Associativity> Associativities = new Dictionary<Type, Associativity>
        {
            {Type.Or, Associativity.Left},
            {Type.And, Associativity.Left},
            {Type.Eql, Associativity.Left},
            {Type.NEql, Associativity.Left},
            {Type.Grt, Associativity.Left},
            {Type.Less, Associativity.Left},
            {Type.GEql, Associativity.Left},
            {Type.LEql, Associativity.Left},
            {Type.Add, Associativity.Left},
            {Type.Sub, Associativity.Left},
            {Type.Mul, Associativity.Left},
            {Type.Div, Associativity.Left}
        };


        private static readonly Dictionary<Type, string> Operators = new Dictionary<Type, string>
        {
            {Type.Or, " || "},
            {Type.And, " && "},
            {Type.Eql, " == "},
            {Type.NEql, " != "},
            {Type.Grt, " > "},
            {Type.Less, " < "},
            {Type.GEql, " >= "},
            {Type.LEql, " <= "},
            {Type.Add, " + "},
            {Type.Sub, " - "},
            {Type.Mul, " * "},
            {Type.Div, " / "}
        };

        public override void Pretty(PrettyBuilder b)
        {
            Pretty(b, 0, false);
        }

        public override void Pretty(PrettyBuilder b, int parentPrecedence)
        {
            var precedence = Precedences[Typ];
            var associativity = Associativities[Typ];

            var parens = parentPrecedence > precedence;

            if (parens) b.Append("(");

            Left.Pretty(b, precedence, associativity == Associativity.Right);
            b.Append(Operators[Typ]);
            Right.Pretty(b, precedence, associativity == Associativity.Left);

            if (parens) b.Append(")");
        }

        public override void Pretty(PrettyBuilder b, int parentPrecedence, bool opposite)
        {
            var precedence = Precedences[Typ];
            var associativity = Associativities[Typ];

            var parens = parentPrecedence > precedence || parentPrecedence == precedence && opposite;

            if (parens) b.Append("(");

            Left.Pretty(b, precedence, associativity == Associativity.Right);
            b.Append(Operators[Typ]);
            Right.Pretty(b, precedence, associativity == Associativity.Left);

            if (parens) b.Append(")");
        }
    }

    public partial class UnaryOperatorExpression
    {
        private static readonly Dictionary<Type, string> Operators = new Dictionary<Type, string>
        {
            {Type.Neg, "-"},
            {Type.Not, "!"}
        };

        public override void Pretty(PrettyBuilder b)
        {
            Pretty(b, 0);
        }

        public override void Pretty(PrettyBuilder b, int parentPrecedence)
        {
            const int precedence = 8;

            var parens = parentPrecedence > precedence;

            if (parens) b.Append("(");
            b.Append(Operators[Typ]);
            Expression.Pretty(b, precedence);
            if (parens) b.Append(")");
        }
    }

    public partial class FunctionCallExpression
    {
        public override void Pretty(PrettyBuilder b)
        {
            Pretty(b, 0);
        }

        public override void Pretty(PrettyBuilder b, int parentPrecedence)
        {
            const int precedence = 9;

            var parens = parentPrecedence > precedence;

            if (parens) b.Append("(");
            b.Append(Id);
            b.Append("(");
            if (ListExpr.Count > 0) b.Intersperse(ListExpr, ", ");
            b.Append(")");
            if (parens) b.Append(")");
        }
    }

    internal enum Associativity
    {
        Left,
        Right
    }
}