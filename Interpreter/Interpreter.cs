using System;
using System.Collections.Generic;
using Parser;

namespace Interpreter
{

  public class ExecutionError : Exception
  {
    public ExecutionError()
    {
    }
  }

  public static class Utility
  {
    public static void Interpret(Program p)
    {
      var env = new Environment();
      var programInterpreter = new ProgramInterpreter();

      p.Accept(programInterpreter, env);
    }
  }

  // program
  public class ProgramInterpreter : IProgramVisitor<Void, Environment>
  {
    readonly FunctionInterpreter functionInterpreter = new FunctionInterpreter();

    public Void Visit(Program p, Environment env)
    {
      foreach (var d in p.decls)
      {
        env.FEnv.Define(d.name, d);
      }

      var main = env.FEnv.Lookup("main");
      main.Accept(functionInterpreter, env);

      return Void.Instance;
    }
  }

  // function
  public class FunctionInterpreter : IFunctionDeclarationVisitor<Value, Environment>
  {
    readonly StatementInterpreter statementInterpreter = new StatementInterpreter();

    public Value Visit(FunctionDefinition f, Environment env)
    {
      ExecutionStatus status = Continue.Instance;

      foreach (var s in f.body)
      {
        status = s.Accept(statementInterpreter, env);
        if (!(status is Continue)) break;
      }

      // null is used to represent no return value
      if (status is Return) return status.ReturnValue();

      // no return statement at all
      return null;
    }
  }


  // statement interpreter
  public class StatementInterpreter : IStatementVisitor<ExecutionStatus, Environment>
  {
    ExpressionInterpreter expressionInterpreter = new ExpressionInterpreter();

    public ExecutionStatus Visit(BlockStatement bs, Environment env)
    {
      env.VEnv.EnterBlock();
      ExecutionStatus status = Continue.Instance;

      foreach (var s in bs.body)
      {
        status = s.Accept(this, env);
        if (!(status is Continue)) break;
      }

      env.VEnv.ExitBlock();
      return status;
    }

    public ExecutionStatus Visit(IfStatement s, Environment env)
    {
      ExecutionStatus status = Continue.Instance;

      var v = s.condition.Accept(expressionInterpreter, env);

      if (v.ToBool()) status = s.consequent.Accept(this, env);
      else status = s.alternate.Accept(this, env);

      return status;
    }

    public ExecutionStatus Visit(WhileStatement s, Environment env)
    {
      ExecutionStatus status = Continue.Instance;

      var v = s.condition.Accept(expressionInterpreter, env);

      while (v.ToBool())
      {
        status = s.body.Accept(this, env);
        if (!(status is Continue)) break;
        v = s.condition.Accept(expressionInterpreter, env);
      }

      return status;
    }

    public ExecutionStatus Visit(ExpressionStatement s, Environment env)
    {
      s.expr.Accept(expressionInterpreter, env);
      return Continue.Instance;
    }

    public ExecutionStatus Visit(VariableDeclarationStatement s, Environment env)
    {
      switch (s.type)
      {
        case Parser.Type.INT:
          env.VEnv.Define(s.name, new IntegerValue(0));
          break;

        case Parser.Type.BOOL:
          env.VEnv.Define(s.name, new BooleanValue(false));
          break;

        case Parser.Type.VOID:
          throw new ExecutionError();
      }

      return Continue.Instance;
    }

    public ExecutionStatus Visit(ReturnStatement s, Environment env)
    {
      return Continue.Instance;
    }
  }

  // expression interpreter
  public class ExpressionInterpreter : IExpressionVisitor<Value, Environment>
  {
    readonly FunctionInterpreter functionInterpreter = new FunctionInterpreter();

    public Value Visit(IntegerLiteralExpression e, Environment env)
    {
      return new IntegerValue(e.value);
    }

    public Value Visit(BooleanLiteralExpression e, Environment env)
    {
      return new BooleanValue(e.value);
    }

    public Value Visit(VariableExpression e, Environment env)
    {
      return env.VEnv.Lookup(e.name);
    }

    public Value Visit(AssignmentExpression e, Environment env)
    {
      var v = e.right.Accept(this, env);
      env.VEnv.Update(e.name, v);
      return v;
    }

    public Value Visit(BinaryOperatorExpression e, Environment env)
    {
      // OR, AND, EQ, NEQ, GT, LT, GEQ, LEQ, ADD, SUB, MUL, DIV
      var v1 = e.left.Accept(this, env);

      Value result;
      switch (e.type)
      {
        case BinaryOperatorExpression.OperatorType.OR:
          if (v1.ToBool()) result = v1; else result = e.right.Accept(this, env);
          return result;

        case BinaryOperatorExpression.OperatorType.AND:
          if (v1.ToBool()) result = e.right.Accept(this, env); else result = v1;
          return result;
      }

      var v2 = e.right.Accept(this, env);

      switch (e.type)
      {
        case BinaryOperatorExpression.OperatorType.EQ:
          result = new BooleanValue(v1.Equals(v2));
          break;

        case BinaryOperatorExpression.OperatorType.NEQ:
          result = new BooleanValue(!v1.Equals(v2));
          break;

        case BinaryOperatorExpression.OperatorType.GT:
          result = new BooleanValue(v1.ToInt() > v2.ToInt());
          break;

        case BinaryOperatorExpression.OperatorType.LT:
          result = new BooleanValue(v1.ToInt() < v2.ToInt());
          break;

        case BinaryOperatorExpression.OperatorType.GEQ:
          result = new BooleanValue(v1.ToInt() >= v2.ToInt());
          break;

        case BinaryOperatorExpression.OperatorType.LEQ:
          result = new BooleanValue(v1.ToInt() <= v2.ToInt());
          break;

        case BinaryOperatorExpression.OperatorType.ADD:
          result = new IntegerValue(v1.ToInt() + v2.ToInt());
          break;

        case BinaryOperatorExpression.OperatorType.SUB:
          result = new IntegerValue(v1.ToInt() + v2.ToInt());
          break;

        case BinaryOperatorExpression.OperatorType.MUL:
          result = new IntegerValue(v1.ToInt() + v2.ToInt());
          break;

        case BinaryOperatorExpression.OperatorType.DIV:
          result = new IntegerValue(v1.ToInt() + v2.ToInt());
          break;

        default:
          throw new ExecutionError();
      }

      return result;
    }

    public Value Visit(UnaryOperatorExpression e, Environment env)
    {
      var v = e.expr.Accept(this, env);

      switch (e.type)
      {
        case UnaryOperatorExpression.OperatorType.NEG:
          return new IntegerValue(-v.ToInt());

        case UnaryOperatorExpression.OperatorType.NOT:
          return new BooleanValue(!v.ToBool());
      }

      throw new ExecutionError();
    }

    public Value Visit(FunctionCallExpression e, Environment env)
    {
      if (e.name == "print")
      {
        var first = true;
        foreach (var arg in e.arguments)
        {
          var v = arg.Accept(this, env);
          if (!first) Console.Write(" ");
          first = false;
          Console.Write(v);
        }

        Console.WriteLine();
      }

      var fdef = env.FEnv.Lookup(e.name);
      var parmCount = fdef.formalParameters.Count;
      var parameters = new List<Value>(parmCount);

      foreach (var arg in e.arguments)
      {
        parameters.Add(arg.Accept(this, env));
      }

      env.EnterFunction(fdef.formalParameters, parameters);

      var result = fdef.Accept(functionInterpreter, env);

      env.ExitFunction();
      return result;
    }

  }

}
