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

    public enum Type
    {
        INT, BOOL, VOID
    }

    // declarations
    public class Declaration : Locatable
    {

    }

    public partial class FunctionDeclaration : Declaration
    {
        public Type type;
        public string id;
        public List<Argument> args;
        public List<Statement> statements;

        public FunctionDeclaration(Type type, string id, List<Argument> args, List<Statement> statements)
        {
            this.type = type;
            this.id = id;
            this.args = args;
            this.statements = statements;
        }
    }


    // arguments
    public class Arg : Locatable
    {

    }

    public partial class Argument : Arg
    {
        public Type type;
        public string id;

        public Argument(Type type, string id)
        {
            this.type = type;
            this.id = id;
        }
    }

    public partial class ArgumentList : Arg
    {
        public List<Argument> args;

        public ArgumentList(List<Argument> args)
        {
            this.args = args;
        }
    }

    // statements

    public class Statement : Locatable
    {
    }

    public partial class BlockStatement : Statement
    {
        public List<Statement> body;

        public BlockStatement(List<Statement> body)
        {
            this.body = body;
        }
    }

    public partial class AssignmentStatement : Statement
    {
        public string id;
        public Expression expr;

        public AssignmentStatement(string id, Expression expr)
        {
            this.id = id;
            this.expr = expr;
        }
    }

    public partial class IfStatement : Statement
    {
        public Statement statement;
        public Statement elseStatement;
        public Expression expr;

        public IfStatement(Expression expr, Statement statement)
        {
            this.statement = statement;
            this.expr = expr;
        }

        public IfStatement(Expression expr, Statement statement, Statement elseStatement)
        {
            this.statement = statement;
            this.elseStatement = elseStatement;
            this.expr = expr;
        }
    }


    public partial class WhileStatement : Statement
    {
        public Statement statement;
        public Expression exrp;

        public WhileStatement(Expression expr, Statement statement)
        {
            this.statement = statement;
            this.exrp = expr;
        }
    }

    public partial class ReturnStatement : Statement
    {
        public Expression expr;

        public ReturnStatement(Expression expr)
        {
            this.expr = expr;
        }
    }

    public partial class VariableDeclaration : Statement
    {
        public Type type;
        public string id;

        public VariableDeclaration(Type type, string id)
        {
            this.type = type;
            this.id = id;
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

    // expressions

    public class Expression : Locatable
    {
    }

    public partial class IdentifierExpression : Expression
    {
        public string id;

        public IdentifierExpression(string id)
        {
            this.id = id;
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

    public partial class BooleanExpression : Expression
    {
        public bool boolean;

        public BooleanExpression(bool boolean)
        {
            this.boolean = boolean;
        }
    }

    public partial class BinaryOperatorExpression : Expression
    {
        public Type type;
        public Expression left;
        public Expression right;

        public enum Type { ADD, SUB, DIV, MUL, LESS, GREAT, LESSEQUAL, GREATEQUAL, EQUALS, NOTEQUALS, OR, AND }

        public BinaryOperatorExpression(BinaryOperatorExpression.Type type, Expression left, Expression right)
        {
            this.type = type;
            this.left = left;
            this.right = right;
        }
    }

    public partial class UnaryOperatorExpression : Expression
    {
        public Type type;
        public Expression expr;

        public enum Type { NOT, NEG }

        public UnaryOperatorExpression(UnaryOperatorExpression.Type type, Expression expr)
        {
            this.type = type;
            this.expr = expr;
        }
    }

    public partial class FunctionExpression : Expression
    {
        public string id;
        public List<Expression> lst_expr;

        public FunctionExpression(string id, List<Expression> lst_expr)
        {
            this.id = id;
            this.lst_expr = lst_expr;
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



}