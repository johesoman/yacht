using System;
namespace Interpreter
{
  // values
  public interface Value
  {
    int ToInt();
    bool ToBool();
  }

  public class IntegerValue : Value, IEquatable<IntegerValue>
  {
    public int value;

    public IntegerValue(int value)
    {
      this.value = value;
    }

    public int ToInt()
    {
      return value;
    }

    public bool ToBool()
    {
      throw new ExecutionError();
    }

    public override string ToString()
    {
      return value.ToString();
    }

    public bool Equals(IntegerValue other)
    {
      return value == other.value;
    }
  }

  public class BooleanValue : Value, IEquatable<BooleanValue>
  {
    public bool value;

    public BooleanValue(bool value)
    {
      this.value = value;
    }

    public int ToInt()
    {
      throw new ExecutionError();
    }

    public bool ToBool()
    {
      return value;
    }

    public override string ToString()
    {
      return value.ToString();
    }

    public bool Equals(BooleanValue other)
    {
      return value == other.value;
    }
  }

  // void
  public class Void
  {
    private Void()
    {
    }

    private static Void instance = new Void();
    public static Void Instance
    {
      get
      {
        return instance;
      }
    }
  }

  public interface ExecutionStatus
  {
    Value ReturnValue();
  }

  public class Continue : ExecutionStatus
  {
    private Continue()
    {
    }

    private static Continue instance = new Continue();
    public static Continue Instance
    {
      get
      {
        return instance;
      }
    }

    public Value ReturnValue()
    {
      throw new ExecutionError();
    }
  }

  public class Return : ExecutionStatus
  {
    Value value;

    public Return(Value value)
    {
      this.value = value;
    }

    public Value ReturnValue()
    {
      return value;
    }
  }
}
