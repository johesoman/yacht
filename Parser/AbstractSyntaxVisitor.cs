using System;
namespace Parser
{
  // program
  public interface IProgramVisitor<R,A>
  {
    R Visit(Program p, A arg);
  }

  public partial class Program
  {
    public R Accept<R,A>(IProgramVisitor<R,A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  // function declaration
  public interface IFunctionDeclarationVisitor<R, A>
  {
    R Visit(FunctionDefinition d, A arg);
  }

  public partial class FunctionDefinition
  {
    public R Accept<R,A>(IFunctionDeclarationVisitor<R,A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  // statements

  public interface IStatementVisitor<R, A>
  {
    R Visit(BlockStatement s, A arg);
    R Visit(IfStatement s, A arg);
    R Visit(WhileStatement s, A arg);
    R Visit(ReturnStatement s, A arg);
    R Visit(ExpressionStatement s, A arg);
    R Visit(VariableDeclarationStatement s, A arg);
  }

  public abstract partial class Statement
  {
    public abstract R Accept<R, A>(IStatementVisitor<R, A> v, A arg);
  }

    public partial class BlockStatement
  {
    override public R Accept<R, A>(IStatementVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  public partial class IfStatement
  {
    override public R Accept<R, A>(IStatementVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  public partial class WhileStatement
  {
    override public R Accept<R, A>(IStatementVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  public partial class ReturnStatement
  {
    override public R Accept<R, A>(IStatementVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  public partial class ExpressionStatement
  {
    override public R Accept<R, A>(IStatementVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  public partial class VariableDeclarationStatement
  {
    override public R Accept<R, A>(IStatementVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  // expressions

  public interface IExpressionVisitor<R, A>
  {
    R Visit(VariableExpression e, A arg);
    R Visit(IntegerLiteralExpression e, A arg);
    R Visit(BooleanLiteralExpression e, A arg);
    R Visit(BinaryOperatorExpression e, A arg);
    R Visit(UnaryOperatorExpression e, A arg);
    R Visit(AssignmentExpression e, A arg);
    R Visit(FunctionCallExpression e, A arg);
  }

  public abstract partial class Expression
  {
    public abstract R Accept<R, A>(IExpressionVisitor<R, A> v, A arg);
  }

  public partial class VariableExpression
  {
    override public R Accept<R, A>(IExpressionVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  public partial class IntegerLiteralExpression
  {
    override public R Accept<R, A>(IExpressionVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  public partial class BooleanLiteralExpression
  {
    override public R Accept<R, A>(IExpressionVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  public partial class BinaryOperatorExpression
  {
    override public R Accept<R, A>(IExpressionVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  public partial class UnaryOperatorExpression
  {
    override public R Accept<R, A>(IExpressionVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  public partial class AssignmentExpression
  {
    override public R Accept<R, A>(IExpressionVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

  public partial class FunctionCallExpression
  {
    override public R Accept<R, A>(IExpressionVisitor<R, A> v, A arg)
    {
      return v.Visit(this, arg);
    }
  }

}
