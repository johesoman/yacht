using System;
using System.Text;
using System.Collections.Generic;

namespace Parser
{
    
    public partial class Variable : IPretty
    {
        public void Pretty(PrettyBuilder b)
        {
            b.Append(type + " " + id);
        }
    }

   
    public partial class ParsedProgram : Locatable, IPretty
    {
        public void Pretty(PrettyBuilder b)
        {
            foreach (var decl in declarations){
                decl.Pretty(b);
            }
        }
    }
    
    public partial class Declaration : Locatable, IPretty
    {
        //Decl ::= Type id ( FormalList ) { Stmt* }
        //Decl ::= void id ( FormalList ) { Stmt* }

        public void Pretty(PrettyBuilder b)
        {
            b.Append(type + " " + id + "(");
            b.Intersperse(formalList, ", ");
            b.Append(") ");

            //prints { stuff }
            stmts.Pretty(b);
            b.NewLine();
        }
    }

    public partial class BlockStatement : Statement
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append("{");
            b.Indent();
            foreach (var statement in statements)
            {
                b.NewLine();
                statement.Pretty(b);
            }
            b.Unindent();
            b.NewLine();
            b.Append("}");
        }
    }

    public partial class IfStatement : Statement
    {
        //Stmt ::= if ( Expr ) { Stmt* }
        override public void Pretty(PrettyBuilder b)
        {
            b.Append("if (");
            expr.Pretty(b);
            b.Append(")");
            stmts.Pretty(b);
        }
    }

    public partial class IfElseStatement : Statement
    {
        //Stmt ::= if ( Expr ) { Stmt* }
        override public void Pretty(PrettyBuilder b)
        {
            b.Append("if (");
            expr.Pretty(b);
            b.Append(")");
            stmts1.Pretty(b);
            b.NewLine();
            b.Append("else");
            stmts2.Pretty(b);
        }
    }

    public partial class WhileStatement : Statement
    {
        //Stmt ::= while ( Expr ) { StmtS* }
        override public void Pretty(PrettyBuilder b)
        {
            b.Append("while (");
            expr.Pretty(b);
            b.Append(")");
            stmts.Pretty(b);            
        }
    }

    public partial class ReturnStatement : Statement
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append("return ");
            expr.Pretty(b);
            b.Append(";");
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

    public partial class VariableStatement : Statement
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(variable.type + " " + variable.id + ";");
        }
    }

    public partial class NumExpression : Expression
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(num);
        }
    }

    public partial class BooleanExpression : Expression
    {
        override public void Pretty(PrettyBuilder b)
        {
            //case sensitive
            if (boolean)
                b.Append("true");
            else
                b.Append("false");
        }
    }



    //TODO: i vissa fall ska parenteser tas bort
    //todo: det kan hända att den delen hamnar någon annan stans, typ parenthesis thingy
    //Vi behöver aldrig lägga till parenteser någonstans

    //principen är sund, men den måste genomföras på varje grunka som kan innehålla en BOP (Blockstatement, etc.)
    //När vi t.ex. står i ett statement som visar sig innehålla en expr så måste kollen köras innan vi går till expr'et i fråga
    //shit, det går inte
    //vi måte ta hand om parenteser där vi står, vi måste ha med oss ovanstående uttrycks operatorprecendens 

    public partial class BinaryOperatorExpression : Expression
    {
        override public void Pretty(PrettyBuilder b)
        {
            //bör ha förklaring varför vi valt dessa argument
            PrettySub(b, 1, false);
        }
        public void PrettySub(PrettyBuilder b, int outerPrecedence, bool opposite)
        {
            var precedence = OpToPrecedence(op);
            var needPar = ((opposite && (outerPrecedence == precedence)) || (outerPrecedence > precedence));

            if (needPar) 
                b.Append("(");

            if (left is BinaryOperatorExpression)
                ((BinaryOperatorExpression)left).PrettySub(b, precedence, IsOpposite(Direction.LEFT, op));
            else
                left.Pretty(b);

            b.Append(OpToString(op));

            if (right is BinaryOperatorExpression)
                ((BinaryOperatorExpression)right).PrettySub(b, precedence, IsOpposite(Direction.RIGHT, op));
            else
                right.Pretty(b);

            if(needPar)
                b.Append(")");
        }
    }

    public partial class IdentifierExpression : Expression
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(id);
        }
    }

    public partial class UnaryOperatorExpression : Expression
    {
        //Expr ::= uop Expr
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(OpToString(op));
            if (expr is BinaryOperatorExpression) {
                b.Append("(");
                expr.Pretty(b);
                b.Append(")");
            }
            else
                expr.Pretty(b);
        }
    }

    public partial class FunctionCallExpression : Expression
    {
        override public void Pretty(PrettyBuilder b)
        {
            b.Append(id+"(");
            b.Intersperse(exprList, ", ");
            b.Append(")");
        }
    }


 

}
