using System;
using System.Text;
using System.Collections.Generic;

namespace Parser
{
    public class Program1
    {
        public List<Declaration> Decls;

        public Program1(List<Declaration> Decls)
        {
            this.Decls = Decls;
        }
    }

    public class Locatable
    {
        public int line, column;

        public void SetLocation(QUT.Gppg.LexLocation loc) {
            line = loc.StartLine;
            column = loc.StartColumn;
        }
    }


    public class Declaration : Locatable
    {
    }

    public partial class TypeDeclaration : Declaration
    {
        public Type type;
        public string id;
        public List<Statement> list;
        public List<Statement> stmts;

        public enum Type { INT, BOOL, VOID};

        public TypeDeclaration(Type type, string id, List<Statement> list, List<Statement> stmts)
        {
            this.type = type;
            this.id = id;
            this.list = list;
            this.stmts = stmts;
        }
    }

    // statements

    public class Statement : Locatable
    {
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

    public partial class IfStatement : Statement
    {
        public Expression condition;
        public Statement conseqence;
        public Statement alternative;

        public IfStatement(Expression condition, Statement conseqence, Statement alternative)
        {
            this.condition = condition;
            this.conseqence = blockify(conseqence);
            this.alternative = blockify(alternative);
        }

        private Statement blockify(Statement s)
        {
            if (s is BlockStatement) return s;
            return new BlockStatement(s);
        }
    }


    public partial class WhileStatement : Statement
    {
        public Expression condition;
        public Statement conseqence;

        public WhileStatement(Expression condition, Statement conseqence) {
            this.condition = condition;
            this.conseqence = Blockify(conseqence);
        }

        private Statement Blockify(Statement s)
        {
            if (s is BlockStatement) return s;
            return new BlockStatement(s);
        }
    }

    public partial class ReturnStatement : Statement
    {
        public Expression retrunment;

        public ReturnStatement() {
            this.retrunment = new Expression();
        }

        public ReturnStatement(Expression retrunment) {
            this.retrunment = retrunment;
        }
    }

    public partial class BlockStatement : Statement
    {
        public List<Statement> Statements;

        public BlockStatement()
        {
            Statements = new List<Statement>();
        }

        public BlockStatement(Statement s)
        {
            Statements = new List<Statement>();
            Statements.Add(s);
        }

        public BlockStatement(List<Statement> Statements)
        {
            this.Statements = Statements;
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

    public class Expression : Locatable
    {
    }

    public partial class IdentifierExpression : Expression
    {
        public string id;
        public List<Expression> exprList;

        public IdentifierExpression(string id, List<Expression> exprList)
        {
            this.id = id;
            this.exprList = exprList;
        }

        public IdentifierExpression(String id) {
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

    public partial class BoolExpression : Expression
    {
        public bool b;

        public BoolExpression(string b)
        {

            this.b = b == "true";
            
        }
    }

    public partial class BinaryOperatorExpression : Expression
    {
        public Type type;
        public Expression left;
        public Expression right;

        public enum Type { OR, AND, EQ, NEQ, LEQ, GEQ, LT, GT, ADD, SUB, MUL, DIV };

        public BinaryOperatorExpression(Type type, Expression left, Expression right)
        {
            this.type = type;
            this.left = left;
            this.right = right;
        }
    }

    public partial class UnaryExpression : Expression
    {
        public Expression right;
        public Type t;

        public enum Type { NOT, NEG };

        public UnaryExpression(Type t, Expression right)
        {
            this.t = t;
            this.right = right;
        }
    }

    public partial class LetExpression : Expression
    {
        public Statement stmt;
        public Expression expr;

        public LetExpression(Statement stmt, Expression expr)
        {
            this.stmt = stmt;
            this.expr = expr;
        }
    }

    public partial class ExpressionList : Expression
    {
        public List<Expression> expr;

        public ExpressionList(List<Expression> expr)
        {
            this.expr = expr;
        }
    }

    public partial class FunctionExpression : Expression
    {
        public string id;
        public List<Expression> exprList;

        public FunctionExpression( string id, List<Expression> exprList)
        {
            this.id = id;
            this.exprList = exprList;
        }
    }

    public partial class TypeDef : Statement
    {
        public enum Type { BOOL, INT };

        public Type t;
        public string id;

        public TypeDef(Type t, string id)
        {
            this.t = t;
            this.id = id;
        }
    }

}