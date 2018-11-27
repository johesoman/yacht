using System;
using System.Collections.Generic;
using Parser;

namespace Interpreter
{
  // environments
  public class Environment
  {
    Stack<VariableEnvironment> venvs = new Stack<VariableEnvironment>();

    public VariableEnvironment VEnv
    {
      get
      {
        return venvs.Peek();
      }
    }

    FunctionEnvironment fenv = new FunctionEnvironment();

    public FunctionEnvironment FEnv
    {
      get
      {
        return fenv;
      }
    }

    public Environment()
    {
      venvs.Push(new VariableEnvironment());
    }

    public void EnterFunction()
    {
      venvs.Push(new VariableEnvironment());
    }

    public void EnterFunction(List<VariableDeclaration> formalParameters, List<Value> parameters)
    {
      var venv = new VariableEnvironment();

      for (var i = 0; i < formalParameters.Count; i++)
      {
        venv.Define(formalParameters[i].name, parameters[i]);
      }

      venvs.Push(venv);
    }

    public void ExitFunction()
    {
      venvs.Pop();
    }
  }


  public class FunctionEnvironment
  {
    Dictionary<string, FunctionDefinition> functions = new Dictionary<string, FunctionDefinition>();

    public FunctionEnvironment()
    {
    }

    public FunctionDefinition Lookup(string x)
    {
      return functions[x];
    }

    public void Define(string x, FunctionDefinition d)
    {
      functions[x] = d;
    }
  }

  public class VariableEnvironment
  {
    Stack<Dictionary<string, Value>> stack = new Stack<Dictionary<string, Value>>();

    public VariableEnvironment()
    {
      stack.Push(new Dictionary<string, Value>());
    }

    public void EnterBlock()
    {
      stack.Push(new Dictionary<string, Value>());
    }

    public void ExitBlock()
    {
      stack.Pop();
    }

    public bool IsDefined(string x)
    {
      foreach (var ve in stack)
      {
        if (ve.ContainsKey(x)) return true;
      }

      return false;
    }

    public Value Lookup(string x)
    {
      foreach (var ve in stack)
      {
        if (ve.ContainsKey(x)) return ve[x];
      }

      throw new ExecutionError();
    }

    public void Update(string x, Value v)
    {
      foreach (var ve in stack)
      {
        if (ve.ContainsKey(x))
        {
          ve[x] = v;
        }
      }

      throw new ExecutionError();
    }

    public void Define(string x, Value v)
    {
      stack.Peek()[x] = v;
    }
  }

}
