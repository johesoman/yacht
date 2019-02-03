using System;
using System.Collections.Generic;
using QUT.Gppg;

namespace Parser
{
    public class Locatable
    {
        public int Line, Column;

        public void SetLocation(LexLocation loc)
        {
            Line = loc.StartLine;
            Column = loc.StartColumn;
        }
    }

    public partial class Program : IPretty
    {
        public List<Declaration> Body;

        public Program(List<Declaration> body)
        {
            Body = body;
        }

        public R Accept<R, A>(IProgramVisitor<R, A> v, A arg)
        {
            return v.Visit(this, arg);
        }

    }

    // declarations

    public abstract class Declaration : Locatable, IPretty
    {
        public abstract void Pretty(PrettyBuilder b);
        public abstract R Accept<R, A>(IDeclarationVisitor<R, A> v, A arg);
    }

    public partial class FormalDeclaration : Declaration
    {
        public enum IdType
        {
            Int,
            Bool,
            Void
        }

        public IdType Type;
        public string Id;
        public List<Formal> FormalList;
        public List<Statement> Statements;

        public FormalDeclaration(string id, List<Formal> formalList, List<Statement> statements = null)
        {
            Id = id;
            FormalList = formalList;
            Statements = statements;
        }

        public FormalDeclaration(IdType type, string id, List<Formal> formalList, List<Statement> statements = null)
        {
            Type = type;
            Id = id;
            FormalList = formalList;
            Statements = statements;
        }

        public override T Accept<T, A>(IDeclarationVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }

    }

    // formal

    public partial class Formal : Locatable, IPretty
    {
        public enum IdType
        {
            Int,
            Bool
        }

        public IdType Type;
        public string Id;

        public Formal(IdType type, string id)
        {
            Type = type;
            Id = id;
        }
    }

    // statements

    public abstract class Statement : Locatable , IPretty
    {
        public abstract void Pretty(PrettyBuilder b);
        public abstract R Accept<R, A>(IStatementVisitor<R, A> v, A arg);
    }

    public partial class BlockStatement : Statement
    {
        public List<Statement> Body;

        public BlockStatement()
        {
            Body = new List<Statement>();
        }

        public BlockStatement(Statement s)
        {
            Body = new List<Statement> {s};
        }

        public BlockStatement(List<Statement> body)
        {
            Body = body;
        }

        public override T Accept<T, A>(IStatementVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }

    }

    public partial class IfStatement : Statement
    {
        public Expression Condition;
        public Statement Consequent;
        public Statement Alternate;

        public IfStatement(Expression condition, Statement consequent)
        {
            Condition = condition;
            Consequent = Blockify(consequent);
            Alternate = new BlockStatement();

        }

        public IfStatement(Expression condition, Statement consequent, Statement alternate)
        {
            Condition = condition;
            Consequent = Blockify(consequent);
            Alternate = Blockify(alternate);
        }

        public Statement Blockify(Statement s)
        {
            return s is BlockStatement ? s : new BlockStatement(s);
        }

        public override T Accept<T, A>(IStatementVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }

    public partial class WhileStatement : Statement
    {
        public Expression Condition;
        public Statement Consequent;

        public WhileStatement(Expression condition, Statement consequent)
        {
            Condition = condition;
            Consequent = Blockify(consequent);
        }

        public Statement Blockify(Statement s)
        {
            return s is BlockStatement ? s : new BlockStatement(s);
        }

        public override T Accept<T, A>(IStatementVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }

    public partial class ReturnStatement : Statement
    {
        public Expression Expression;

        public ReturnStatement(Expression expression = null)
        {
            Expression = expression;
        }

        public override T Accept<T, A>(IStatementVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }

    public partial class ExpressionStatement : Statement
    {
        public Expression Expression;

        public ExpressionStatement(Expression expression)
        {
            Expression = expression;
        }

        public override T Accept<T, A>(IStatementVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }

    public partial class FormalStatement : Statement
    {
        public Formal Formal;

        public FormalStatement(Formal formal)
        {
            Formal = formal;
        }

        public override T Accept<T, A>(IStatementVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }

    // expressions

    public abstract partial class Expression : Locatable, IPretty
    {
        public abstract void Pretty(PrettyBuilder b);
        public abstract R Accept<R, A>(IExpressionVisitor<R, A> v, A arg);

    }

    public partial class IdentifierExpression : Expression
    {
        public string Id;

        public IdentifierExpression(string id)
        {
            Id = id;
        }

        public override T Accept<T, A>(IExpressionVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }

    public partial class NumberExpression : Expression
    {
        public int Num;

        public NumberExpression(string num)
        {
            Num = Convert.ToInt32(num);
        }

        public override T Accept<T, A>(IExpressionVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }

    public partial class BooleanExpression : Expression
    {
        public bool Boolean;

        public BooleanExpression(string boolean)
        {
            Boolean = boolean == "true";
        }

        public override T Accept<T, A>(IExpressionVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }

    public partial class AssignmentExpression : Expression
    {
        public Expression Expression;
        public string Id;

        public AssignmentExpression(string id, Expression expr)
        {
            Id = id;
            Expression = expr;
        }

        public override T Accept<T, A>(IExpressionVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }

    public partial class BinaryOperatorExpression : Expression
    {
        public enum Type
        {
            Add,
            Sub,
            Mul,
            Div,
            And,
            Or,
            Eql,
            NEql,
            Grt,
            GEql,
            Less,
            LEql
        }

        public Expression Left;
        public Expression Right;
        public Type Typ;

        public BinaryOperatorExpression(Type type, Expression left, Expression right)
        {
            Typ = type;
            Left = left;
            Right = right;
        }

        public override T Accept<T, A>(IExpressionVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }

    public partial class UnaryOperatorExpression : Expression
    {
        public enum Type
        {
            Neg,
            Not
        }
        
        public Expression Expression;
        public Type Typ;
        
        public UnaryOperatorExpression(Type type, Expression expression)
        {
            Typ = type;
            Expression = expression;
        }

        public override T Accept<T, A>(IExpressionVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }

    public partial class FunctionCallExpression : Expression
    {
        public string Id;
        public List<Expression> ListExpr;

        public FunctionCallExpression(string id, List<Expression> listExpr)
        {
            Id = id;
            ListExpr = listExpr;
        }

        public override T Accept<T, A>(IExpressionVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }
    }
}