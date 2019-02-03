using System;
using System.Text;
using System.Collections.Generic;

namespace Parser
{
    public partial class Program : IPretty
    {
        public List<Declaration> body;

        public Program(List<Declaration> body)
        {
            this.body = body;
            body.Reverse();
        }

    }
    public class Locatable
    {
        public int line, column;

        public void SetLocation(QUT.Gppg.LexLocation loc)
        {
            line = loc.StartLine;
            column = loc.StartColumn;
        }
    }

    // statements

    public abstract partial class TypeParent : Locatable, IPretty
    {
        public abstract void Pretty(PrettyBuilder b);
    }

    public partial class Type : TypeParent
    {
        public Typ type;
        public enum Typ { INT, BOOL }


        public Type(Typ type)
        {
            this.type = type;
        }
    }

    public abstract class Declaration : Locatable, IPretty
    {

        public abstract void Pretty(PrettyBuilder b);
    }

    public abstract class Statement : Locatable, IPretty
    {
        public abstract void Pretty(PrettyBuilder b);
    }

    public abstract class FormalListParent : Locatable, IPretty
    {
        public abstract void Pretty(PrettyBuilder b);
    }

    public partial class FormalList : FormalListParent
    {
        Type type;
        string id;



        public FormalList(Type type, string id)
        {
            this.type = type;
            this.id = id;

        }
    }


    public partial class VoidDeclaration : Declaration
    {
        string id;
        List<Statement> Stmt;
        List<FormalListParent> fList;

        public VoidDeclaration(string id, List<FormalListParent> fList, List<Statement> Stmt)
        {
            this.id = id;
            this.Stmt = Stmt;
            this.fList = fList;
        }
    }

    public partial class TypeDeclaration : Declaration
    {
        Type type;
        string id;
        List<Statement> Stmt;
        List<FormalListParent> fList;

        public TypeDeclaration(Type type, string id, List<FormalListParent> fList, List<Statement> Stmt)
        {
            this.type = type;
            this.id = id;
            this.Stmt = Stmt;
            this.fList = fList;
        }
    }
    public partial class BlockStatement : Statement
    {
        public List<Statement> body;



        public BlockStatement(List<Statement> s)
        {
            body = s;
        }
    }




    public partial class InitializationStatement : Statement
    {
        Type type;
        string id;



        public InitializationStatement(Type type, string id)
        {
            this.type = type;
            this.id = id;

        }
    }



    public partial class IfStatement : Statement
    {
        public Expression condition;
        public Statement consequent, alternate;
        public IfStatement(Expression condition, Statement consequent, Statement alternate)
        {
            this.condition = condition;
            this.consequent = consequent;
            this.alternate = alternate;
        }

    }

    public partial class WhileStatement : Statement
    {
        public Expression condition;
        public Statement consequent;
        public WhileStatement(Expression condition, Statement consequent)
        {
            this.condition = condition;
            this.consequent = consequent;
        }
    }

    public partial class ReturnStatement : Statement
    {
        public Expression returnValue;
        public ReturnStatement(Expression returnValue)
        {
            this.returnValue = returnValue;
        }
    }

    public partial class ExpressionStatement : Statement
    {
        public Expression expr;
        public ExpressionStatement(Expression expr)
        {
            this.expr = expr;
        }
    }


    // expressions


    public abstract partial class Expression : Locatable, IPretty
    {
        public abstract void Pretty(PrettyBuilder b);
    }

    public partial class IdentifierExpression : Expression
    {
        public string id;

        public IdentifierExpression(string id)
        {
            this.id = id;
        }
    }

    public partial class AssignmentExpression : Expression
    {
        public string id;
        public Expression expr;

        public AssignmentExpression(string id, Expression expr)
        {
            this.id = id;
            this.expr = expr;
        }
    }

    public partial class NumberExpression : Expression
    {
        public int num;

        public NumberExpression(string num)
        {
            this.num = Convert.ToInt32(num);
        }
    }

    public partial class BinaryOperatorExpression : Expression
    {
        public Type type;
        public Expression left;
        public Expression right;

        public enum Type { ADD, SUB, MUL, DIV, GRTTHN, LESSTHN, EQUALS, LESSOREQ, GROREQ, NOTEQ, AND, OR, NOT }

        public BinaryOperatorExpression(Type type, Expression left, Expression right)
        {
            this.type = type;
            this.left = left;
            this.right = right;
        }
    }
    public partial class UniversalOperatorExpression : Expression
    {
        public Type type;
        public Expression expr;

        public enum Type { GRTTHN, LESSTHN, EQUALS, LESSOREQ, GROREQ, NOTEQ, AND, OR, NOT, ADD, SUB, MUL, DIV }

        public UniversalOperatorExpression(Type type, Expression expr)
        {
            this.type = type;
            this.expr = expr;
        }
    }




    public partial class BooleanExpression : Expression
    {
        public bool val;

        public BooleanExpression(bool val)
        {
            this.val = val;
        }
    }

    public partial class ListExpression : Expression
    {
        public string id;
        public List<ExprListParent> elist;

        public ListExpression(string id, List<ExprListParent> list)
        {
            this.id = id;
            elist = list;
        }
    }

    public partial class CastExpression : Expression
    {
        public Expression expr;

        public CastExpression(Expression e)
        {
            this.expr = e;

        }
    }


    public abstract partial class ExprListParent : Locatable, IPretty
    {
        public abstract void Pretty(PrettyBuilder b);
    }

    public partial class ExprList : ExprListParent
    {
        public Expression expr;

        public ExprList(Expression expr)
        {
            this.expr = expr;
        }
    }
}




