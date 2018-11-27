using System;
using System.Collections.Generic;

namespace Parser
{
  // TODO: in lab - decorate tree with line number information

  public class Locatable
  {
    public int line;
    public int column;

    public void SetLocation(QUT.Gppg.LexLocation loc)
    {
      this.line = loc.StartLine;
      this.column = loc.StartColumn;
    }
  }

  public partial class Program : Locatable, IPretty
  {
    public List<FunctionDefinition> decls;

    public Program(List<FunctionDefinition> decls)
    {
      this.decls = decls;
    }
  }

  public enum Type { INT, BOOL, VOID };

  // declarations

  // Not a top-level declaration
  public partial class VariableDeclaration : Locatable, IPretty
  {
    public string name;
    public Type type;

    public VariableDeclaration(Type type, string name)
    {
      this.name = name;
      this.type = type;
    }
  }
  public partial class FunctionDefinition : Locatable, IPretty
  {
    public string name;
    public Type returnType;
    public List<VariableDeclaration> formalParameters;
    public List<Statement> body;

    public FunctionDefinition(Type returnType, string name, List<VariableDeclaration> formalParameters, List<Statement> body)
    {
      this.name = name;
      this.returnType = returnType;
      this.formalParameters = formalParameters;
      this.body = body;
    }
  }

  // statements

  public partial class Statement : Locatable, IPretty
  {
    public Statement Blockify(Statement s)
    {
      if (s is BlockStatement) return s;
      if (s == null) return new BlockStatement(new List<Statement>());

      return new BlockStatement(new List<Statement> { s });
    }

  }

  public partial class BlockStatement : Statement
  {
    public List<Statement> body;

    public BlockStatement(List<Statement> body)
    {
      this.body = body;
    }
  }

  public partial class IfStatement : Statement
  {
    public Expression condition;
    public Statement consequent, alternate;

    public IfStatement(Expression condition, Statement consequent, Statement alternate)
    {
      this.condition = condition;
      this.consequent = Blockify(consequent);
      this.alternate = Blockify(alternate);
    }
  }

  public partial class WhileStatement : Statement
  {
    public Expression condition;
    public Statement body;

    public WhileStatement(Expression condition, Statement body)
    {
      this.condition = condition;
      this.body = Blockify(body);
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

  public partial class ExpressionStatement : Statement
  {
    public Expression expr;

    public ExpressionStatement(Expression expr)
    {
      this.expr = expr;
    }
  }

  public partial class VariableDeclarationStatement : Statement
  {
    public string name;
    public Type type;

    public VariableDeclarationStatement(Type type, string name)
    {
      this.name = name;
      this.type = type;
    }
  }

  // expressions

  public partial class Expression : Locatable, IPretty
  {
  }

  public partial class VariableExpression : Expression
  {
    public string name;

    public VariableExpression(string name)
    {
      this.name = name;
    }

  }

  public partial class IntegerLiteralExpression : Expression
  {
    public int value;

    public IntegerLiteralExpression(string value)
    {
      this.value = Convert.ToInt32(value);
    }

  }

  public partial class BooleanLiteralExpression : Expression
  {
    public bool value;

    public BooleanLiteralExpression(bool value)
    {
      this.value = value;
    }

    public BooleanLiteralExpression(string value)
    {
      this.value = Convert.ToBoolean(value);
    }
  }

  public partial class BinaryOperatorExpression : Expression
  {
    public OperatorType type;
    public Expression left;
    public Expression right;

    public enum OperatorType { OR, AND, EQ, NEQ, GT, LT, GEQ, LEQ, ADD, SUB, MUL, DIV }

    public BinaryOperatorExpression(OperatorType type, Expression left, Expression right)
    {
      this.type = type;
      this.left = left;
      this.right = right;
    }

    public static Expression CreateNotEqual(Expression left, Expression right)
    {
      var eq = new BinaryOperatorExpression(OperatorType.EQ, left, right);
      return new UnaryOperatorExpression(UnaryOperatorExpression.OperatorType.NOT, eq);
    }

    // A <= B == !(A > B)
    public static Expression CreateLessOrEqual(Expression left, Expression right)
    {
      var gt = new BinaryOperatorExpression(OperatorType.GT, left, right);
      return new UnaryOperatorExpression(UnaryOperatorExpression.OperatorType.NOT, gt);
    }

    // A < B == A <= B && A != B
    public static Expression CreateLessThan(Expression left, Expression right)
    {
      var leq = CreateLessOrEqual(left, right);
      var neq = CreateNotEqual(left, right);
      return new BinaryOperatorExpression(OperatorType.AND, leq, neq);
    }

    // A >= B == A > B || A == B
    public static Expression CreateGreaterOrEqual(Expression left, Expression right)
    {
      var gt = new BinaryOperatorExpression(OperatorType.GT, left, right);
      var eq = new BinaryOperatorExpression(OperatorType.EQ, left, right);
      return new BinaryOperatorExpression(OperatorType.OR, gt, eq);
    }


  }

  public partial class UnaryOperatorExpression : Expression
  {
    public OperatorType type;
    public Expression expr;

    public enum OperatorType { NEG, NOT };

    public UnaryOperatorExpression(OperatorType type, Expression expr)
    {
      this.type = type;
      this.expr = expr;
    }
  }

  public partial class AssignmentExpression : Expression
  {
    public string name;
    public Expression right;

    public AssignmentExpression(string name, Expression right)
    {
      this.name = name;
      this.right = right;
    }
  }

  public partial class FunctionCallExpression : Expression
  {
    public string name;
    public List<Expression> arguments;

    public FunctionCallExpression(string name, List<Expression> arguments)
    {
      this.name = name;
      this.arguments = arguments;
    }
  }

}
