using System.Collections.Generic;


namespace Parser
{

    public partial class Program : IPretty
    {

        public void Pretty(PrettyBuilder b)
        {
            b.Vertical(body);
        }
    }

    public partial class FormalList : FormalListParent
    {
        override public void Pretty(PrettyBuilder b)
        {
            type.Pretty(b);
            b.Append(" ");
            b.Append(id);
        }
    }

    public partial class VoidDeclaration : Declaration
    {

        override public void Pretty(PrettyBuilder b)
        {
            b.Append("void ");
            b.Append(id);
            b.Append("(");
            if (fList != null)
                b.Intersperse(fList, ",");

            b.Append(")");

            b.Append("{");
            if (Stmt.Count > 0)
            {
                b.Indent();
                b.NewLine();
                b.Vertical(Stmt);

                b.Unindent();
            }
            b.NewLine();
            b.Append("}");
        }
    }

    public partial class TypeDeclaration : Declaration
    {
        override public void Pretty(PrettyBuilder b)
        {
            type.Pretty(b);
            b.Append(" ");
            b.Append(id);
            b.Append("(");
            if (fList != null)
            {
                b.Intersperse(fList, ",");
            }

            b.Append(")");

            b.Append("{");
            if (Stmt.Count > 0)
            {
                b.Indent();
                b.NewLine();
                b.Vertical(Stmt);

                b.Unindent();
            }
            b.NewLine();
            b.Append("}");
        }
    }

    public partial class Type : TypeParent
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(type.ToString().ToLower());
        }
    }



    public partial class InitializationStatement : Statement
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(type.type.ToString().ToLower());
            b.Append(" ");
            b.Append(id);
            b.Append(";");

        }
    }

    public partial class WhileStatement : Statement
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append("while");
            b.Append("(");
            condition.Pretty(b);
            b.Append(")");
            consequent.Pretty(b);
        }
    }

    public partial class ReturnStatement : Statement
    {
        override public void Pretty(PrettyBuilder b)
        {
            if (returnValue != null)
            {
                b.Append("return ");
                returnValue.Pretty(b);
                b.Append(";");
            }
            else
            {
                b.Append("return;");
            }
        }
    }

    public partial class ExpressionStatement : Statement
    {
        override public void Pretty(PrettyBuilder b)
        {
            expr.Pretty(b);
            b.Append(";");
        }
    }





    public partial class BlockStatement : Statement
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append("{");
            if (body.Count > 0)
            {
                b.Indent();
                b.NewLine();
                b.Vertical(body);

                b.Unindent();
            }

            b.NewLine();
            b.Append("}");
        }
    }


    public partial class AssignmentExpression : Expression
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(id);
            b.Append("=");
            expr.Pretty(b);
        }

    }

    public partial class IfStatement : Statement
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append("if(");
            condition.Pretty(b);
            b.Append(")");
            consequent.Pretty(b);
            b.Append("else");
            alternate.Pretty(b);
        }
    }


    public abstract partial class Expression : Locatable, IPretty
    {
        public virtual void Pretty(PrettyBuilder b, int parentPrecedence, bool opposite)
        {
            Pretty(b);
        }
    }

    public partial class IdentifierExpression : Expression
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(id);

        }

    }


    public partial class NumberExpression : Expression
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(num.ToString());
        }
    }

    public partial class UniversalOperatorExpression : Expression
    {
        static Dictionary<Type, int> precedences = new Dictionary<Type, int>
        { {Type.NOT, 8}, {Type.SUB, 8}  };
        static Dictionary<Type, string> operators = new Dictionary<Type, string>
        {
        {Type.NOT, "!"},
         {Type.SUB, "-"},
             };

        override public void Pretty(PrettyBuilder b)
        {
            Pretty(b, 0, false);
        }

        override public void Pretty(PrettyBuilder b, int parentPrecedence, bool opposite)
        {
            var precedence = precedences[type];


            var parens = parentPrecedence > precedence || parentPrecedence == precedence && opposite;

            if (parens)
            {
                b.Append("(");
            }
            b.Append(operators[type]);
            expr.Pretty(b);


            if (parens)
            {
                b.Append(")");
            }
        }
    }

    public partial class BooleanExpression : Expression
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(val.ToString().ToLower());
        }
    }

    public partial class ListExpression : Expression
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(id);
            b.Append("(");
            if (elist != null)
            {
                b.Intersperse(elist, ",");
            }
            b.Append(")");
        }
    }

    public partial class CastExpression : Expression
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append("(");
            expr.Pretty(b);
            b.Append(")");
        }
    }

    public partial class ExprList : ExprListParent
    {
        override public void Pretty(PrettyBuilder b)
        {
            expr.Pretty(b);


        }
    }
    public partial class BinaryOperatorExpression : Expression
    {

        static Dictionary<Type, int> precedences = new Dictionary<Type, int>
        { {Type.ADD, 6}, {Type.SUB, 6}, {Type.MUL, 7}, {Type.DIV, 7} , {Type.AND, 3}, {Type.OR, 2}, {Type.EQUALS, 4}, {Type.GROREQ, 5}, {Type.GRTTHN, 5}, {Type.LESSTHN, 5}, {Type.LESSOREQ, 5}, {Type.NOTEQ, 4} };

        enum Associativity { LEFT, RIGHT, BOTH };

        static Dictionary<Type, Associativity> associativities = new Dictionary<Type, Associativity>
        {
        {Type.ADD, Associativity.LEFT},
         {Type.SUB, Associativity.LEFT},
             {Type.MUL, Associativity.LEFT},
            {Type.DIV, Associativity.LEFT},
            {Type.OR, Associativity.LEFT},
            {Type.AND, Associativity.LEFT},
            {Type.EQUALS, Associativity.LEFT},
            {Type.GROREQ, Associativity.LEFT},
            {Type.LESSTHN, Associativity.LEFT},
            {Type.GRTTHN, Associativity.LEFT},
            {Type.LESSOREQ, Associativity.LEFT},
           {Type.NOTEQ, Associativity.LEFT},
             };


        static Dictionary<Type, string> operators = new Dictionary<Type, string>
        {
        {Type.ADD, "+"},
         {Type.SUB, "-"},
             {Type.MUL, "*"},
            {Type.DIV,"/"},
            {Type.LESSTHN, "<"},
         {Type.GRTTHN, ">"},
             {Type.LESSOREQ, "<="},
            {Type.GROREQ,">="},
            {Type.EQUALS, "=="},
         {Type.NOTEQ, "!="},
             {Type.AND, "&&"},
            {Type.OR,"||"}
             };

        override public void Pretty(PrettyBuilder b)
        {
            Pretty(b, 0, false);
        }

        override public void Pretty(PrettyBuilder b, int parentPrecedence, bool opposite)
        {
            var precedence = precedences[type];
            var associativity = associativities[type];

            var parens = parentPrecedence > precedence || parentPrecedence == precedence && opposite;

            if (parens)
            {
                b.Append("(");
            }

            left.Pretty(b, precedence, associativity == Associativity.RIGHT);
            b.Append(operators[type]);
            right.Pretty(b, precedence, associativity == Associativity.LEFT);

            if (parens)
            {
                b.Append(")");
            }

        }
    }


}


