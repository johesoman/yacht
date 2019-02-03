using System;
using System.Text;
using System.Collections.Generic;

namespace Parser
{

    public class Locatable
    {
        public int line, column;

        public void SetLocation(QUT.Gppg.LexLocation loc)
        {
            line = loc.StartLine;
            column = loc.StartColumn;
        }
    }

    public partial class Program
    {
        public List<Declaration> body;

        public Program(List<Declaration> body)
        {
            this.body = body;
        }

    }

    // DECL

    public partial class Declaration : Locatable
    {
        public Type_ type;
        public string id;
        public List<Parameter> parameters;
        public List<Statement> body;

        public Declaration(Type_ type, string id, List<Parameter> parameters, List<Statement> body)
        {
            this.type = type;
            this.id = id;
            this.parameters = parameters;
            this.body = body;
        }
    }

    public partial class Parameter : Locatable
    {
        public Type_ type;
        public string id;

        public Parameter(Type_ type, string id)
        {
            this.type = type;
            this.id = id;
        }
    }

    public enum Type_ { INT = 0, BOOL = 1, NULL = 2 }

    // Statements
    public abstract partial class Statement : Locatable
    {
    }

    public partial class BlockStatement : Statement
    {
        public List<Statement> s;

        public BlockStatement(List<Statement> s)
        {
            this.s = s;
        }
    }

    public partial class IfStatement : Statement
    {
        public Expression condition;
        public Statement consequence;
        public Statement alternative;

        public IfStatement(Expression condition, Statement consequence, Statement alternative)
        {
            this.condition = condition;
            this.consequence = MakeBlock(consequence);
            this.alternative = MakeBlock(alternative);
        }

        public Statement MakeBlock(Statement s)
        {
            if (s == null || s is BlockStatement)
                return s;

            return new BlockStatement(new List<Statement> { s });
        }
    }

    public partial class WhileStatement : Statement
    {
        public Expression condition;
        public Statement body;

        public WhileStatement(Expression condition, Statement body)
        {
            this.condition = condition;
            this.body = body;
        }
    }

    public partial class ReturnStatement : Statement
    {
        public Expression expression;

        public ReturnStatement(Expression expression)
        {
            this.expression = expression;
        }
    }

    public partial class ExpressionStatement : Statement
    {
        public Expression expression;

        public ExpressionStatement(Expression expression)
        {
            this.expression = expression;
        }
    }

    public partial class DeclareStatement : Statement
    {
        public Type_ type;
        public string id;

        public DeclareStatement(Type_ type, string id)
        {
            this.type = type;
            this.id = id;
        }
    }

    // expressions

    public abstract partial class Expression : Locatable
    {
    }

    public partial class BopExpression : Expression
    {
        public enum Operator { ADD, SUB, MUL, DIV, OR, AND, EQ, NOT_EQ, LESS, GREATER, LESS_OR_EQ, GREATER_OR_EQ }

        public Expression left;
        public Expression right;
        public Operator op;

        public BopExpression(Expression left, Operator op, Expression right)
        {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }

    public partial class NumberExpression : Expression
    {
        public int n;

        public NumberExpression(string s)
        {
            this.n = Convert.ToInt32(s);
        }
    }

    public partial class BoolExpression : Expression
    {
        public bool _bool;

        public BoolExpression(bool b)
        {
            this._bool = b;
        }
    }

    public partial class AssignmentExpression : Expression
    {
        public string id;
        public Expression expression;

        public AssignmentExpression(string id, Expression expression)
        {
            this.id = id;
            this.expression = expression;
        }
    }

    public partial class IdentifierExpression : Expression
    {
        public string id;

        public IdentifierExpression(string id)
        {
            this.id = id;
        }
    }

    public partial class UopExpression : Expression
    {
        public enum Operator { NEG, NOT }
        public Expression expression;
        public Operator uop;

        public UopExpression(Expression expression, Operator uop)
        {
            this.expression = expression;
            this.uop = uop;
        }
    }

    public partial class LetExpression : Expression
    {
        public string id;
        public List<Expression> eList;

        public LetExpression(string id, List<Expression> eList)
        {
            this.id = id;
            this.eList = eList;
        }
    }
}