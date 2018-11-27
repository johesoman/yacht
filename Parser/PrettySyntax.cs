using System;
using System.Collections.Generic;

namespace Parser
{
  public partial class Program : Locatable, IPretty
  {
    public void Pretty(PrettyBuilder pb)
    {
      pb.Vertical(decls);
    }

    public string Pretty()
    {
      var pb = new PrettyBuilder();
      Pretty(pb);
      return pb.Layout();
    }
  }


  public partial class VariableDeclaration : Locatable, IPretty
  {
    public static Dictionary<Type, string> typeStrings =
      new Dictionary<Type, string>
      { { Type.INT, "int" }
      , { Type.BOOL, "bool" }
      };

    public void Pretty(PrettyBuilder pb)
    {
      pb.Append(typeStrings[type] + " " + name);
    }
  }

  public partial class FunctionDefinition : Locatable, IPretty
  {
    public static Dictionary<Type, string> typeStrings =
      new Dictionary<Type, string>
      { { Type.INT, "int" }
      , { Type.BOOL, "bool" }
      , { Type.VOID, "void" }
      };

    public void Pretty(PrettyBuilder pb)
    {
      pb.Append(typeStrings[returnType] + " " + name + "(");
      pb.Intersperse(formalParameters, ", ");
      pb.Append(")");

      pb.NewLine();
      pb.Append("{");

      pb.Indent();
      pb.Vertical(body);
      pb.Unindent();

      pb.NewLine();
      pb.Append("}");
    }
  }

  // statement

  public partial class Statement : Locatable, IPretty
  {
    public virtual void Pretty(PrettyBuilder pb)
    {
      pb.Append("Statement.Pretty: forgotten override ");
      pb.Append(this.GetType().ToString());
    }
  }

  public partial class BlockStatement : Statement
  {
    public override void Pretty(PrettyBuilder pb)
    {
      pb.Append("{");

      pb.Indent();
      pb.Vertical(body); // Vertical inserts NewLine if there are any statements, to avoid double newlines, from { and }
      pb.Unindent();

      pb.NewLine();
      pb.Append("}");
    }
  }

  public partial class IfStatement : Statement
  {
    public override void Pretty(PrettyBuilder pb)
    {
      pb.Append("if (");
      condition.Pretty(pb);
      pb.Append(")");

      if (consequent is BlockStatement) pb.NewLine();
      consequent.Pretty(pb);

      if (alternate != null)
      {
        pb.NewLine();
        pb.Append("else ");

        if (alternate is BlockStatement) pb.NewLine();
        alternate.Pretty(pb);
      }
    }
  }

  public partial class WhileStatement : Statement
  {
    public override void Pretty(PrettyBuilder pb)
    {
      pb.Append("while (");
      condition.Pretty(pb);
      pb.Append(")");

      if (body is BlockStatement) pb.NewLine();
      body.Pretty(pb);
    }
  }

  public partial class ReturnStatement : Statement
  {
    public override void Pretty(PrettyBuilder pb)
    {
      pb.Append("return ");
      if (expr != null) expr.Pretty(pb);
      pb.Append(";");
    }
  }


  public partial class ExpressionStatement : Statement
  {
    public override void Pretty(PrettyBuilder pb)
    {
      expr.Pretty(pb);
      pb.Append(";");
    }
  }

  public partial class VariableDeclarationStatement : Statement
  {
    public static Dictionary<Type, string> typeStrings =
      new Dictionary<Type, string>
      { { Type.INT, "int" }
      , { Type.BOOL, "bool" }
      };

    public override void Pretty(PrettyBuilder pb)
    {
      pb.Append(typeStrings[type] + " " + name + ";");
    }
  }


  // expressions

  public partial class Expression : Locatable, IPretty
  {
    public virtual void Pretty(PrettyBuilder pb)
    {
      pb.Append("Expression.Pretty: forgotten override ");
      pb.Append(this.GetType().ToString());
    }

    public virtual void Pretty(PrettyBuilder pb, int outerPrecedence, bool opposite)
    {
      pb.Append("Expression.Pretty: forgotten override ");
      pb.Append(this.GetType().ToString());
    }

  }

  public partial class VariableExpression : Expression
  {
    public override void Pretty(PrettyBuilder pb)
    {
      pb.Append(name);
    }

    public override void Pretty(PrettyBuilder pb, int outerPrecedence, bool opposite)
    {
      Pretty(pb);
    }
  }


  public partial class IntegerLiteralExpression : Expression
  {
    public override void Pretty(PrettyBuilder pb)
    {
      pb.Append(value.ToString());
    }

    public override void Pretty(PrettyBuilder pb, int outerPrecedence, bool opposite)
    {
      Pretty(pb);
    }
  }


  public partial class BooleanLiteralExpression : Expression
  {
    public override void Pretty(PrettyBuilder pb)
    {
      pb.Append(value ? "true" : "false");
    }

    public override void Pretty(PrettyBuilder pb, int outerPrecedence, bool opposite)
    {
      Pretty(pb);
    }
  }


  public partial class BinaryOperatorExpression : Expression
  {
    static Dictionary<OperatorType, string> operators =
      new Dictionary<OperatorType, string>
      { { OperatorType.OR, "||" }
      , { OperatorType.AND, "&&" }
      , { OperatorType.EQ, "==" }
      , { OperatorType.NEQ, "!=" }
      , { OperatorType.GEQ, ">=" }
      , { OperatorType.LEQ, "<=" }
      , { OperatorType.GT, ">" }
      , { OperatorType.LT, "<" }
      , { OperatorType.ADD, "+" }
      , { OperatorType.SUB, "-" }
      , { OperatorType.MUL, "*" }
      , { OperatorType.DIV, "/" }
      };


    static Dictionary<OperatorType, int> precedences =
      new Dictionary<OperatorType, int>
      { // =, assignment 1
        { OperatorType.OR, 2 }
      , { OperatorType.AND, 3 }
      , { OperatorType.EQ, 4 }
      , { OperatorType.NEQ, 4 }
      , { OperatorType.GEQ, 5 }
      , { OperatorType.LEQ, 5 }
      , { OperatorType.GT,  5 }
      , { OperatorType.LT, 5 }
      , { OperatorType.ADD, 6 }
      , { OperatorType.SUB, 6 }
      , { OperatorType.MUL, 7 }
      , { OperatorType.DIV, 7 }
      // ! and unary -, 8
      // (), function call, 9
      };

    enum Associativity { LEFT, RIGHT, BOTH };

    static Dictionary<OperatorType, Associativity> associativities =
      new Dictionary<OperatorType, Associativity>
      { { OperatorType.OR, Associativity.BOTH }
      , { OperatorType.AND, Associativity.BOTH }
      , { OperatorType.EQ, Associativity.LEFT }
      , { OperatorType.NEQ, Associativity.LEFT }
      , { OperatorType.GEQ, Associativity.LEFT }
      , { OperatorType.LEQ, Associativity.LEFT }
      , { OperatorType.GT,  Associativity.LEFT }
      , { OperatorType.LT, Associativity.LEFT }
      , { OperatorType.ADD, Associativity.BOTH }
      , { OperatorType.SUB, Associativity.LEFT }
      , { OperatorType.MUL, Associativity.BOTH }
      , { OperatorType.DIV, Associativity.LEFT }
      };

    public override void Pretty(PrettyBuilder pb)
    {
      Pretty(pb, 0, false);
    }

    public override void Pretty(PrettyBuilder pb, int outerPrecedence, bool opposite)
    {
      var precedence = precedences[type];
      var associaticity = associativities[type];

      var needParenthesis = outerPrecedence > precedence || opposite && outerPrecedence == precedence;

      if (needParenthesis) pb.Append("(");

      left.Pretty(pb, precedence, associaticity == Associativity.RIGHT);
      pb.Append(operators[type]);
      right.Pretty(pb, precedence, associaticity == Associativity.LEFT);

      if (needParenthesis) pb.Append(")");
    }
  }


  public partial class UnaryOperatorExpression : Expression
  {
    public static Dictionary<OperatorType, string> operators =
      new Dictionary<OperatorType, string>
      { { OperatorType.NEG, "-" }
      , { OperatorType.NOT, "!" }
      };

    public override void Pretty(PrettyBuilder pb)
    {
      Pretty(pb, 0, false);
    }

    public override void Pretty(PrettyBuilder pb, int outerPrecedence, bool opposite)
    {
      pb.Append(operators[type]);
      expr.Pretty(pb, 8, false);
    }
  }

  public partial class AssignmentExpression : Expression
  {
    public override void Pretty(PrettyBuilder pb)
    {
      Pretty(pb, 0, false);
    }

    public override void Pretty(PrettyBuilder pb, int outerPrecedence, bool opposite)
    {
      var precedence = 1;
      var needParenthesis = outerPrecedence > precedence;

      if (needParenthesis) pb.Append("(");

      pb.Append(name + " = ");
      right.Pretty(pb, precedence, false); // right associative

      if (needParenthesis) pb.Append(")");
    }
  }

  public partial class FunctionCallExpression : Expression
  {
    public override void Pretty(PrettyBuilder pb)
    {
      pb.Append(name);
      pb.Append("(");
      pb.Intersperse(arguments, ", ");
      pb.Append(")");
    }

    public override void Pretty(PrettyBuilder pb, int outerPrecedence, bool opposite)
    {
      Pretty(pb);
    }
  }
}
