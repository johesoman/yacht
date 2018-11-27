using System;
using System.Collections.Generic;
using Generators;

namespace Parser
{

  public class BudgetExhaustedException : Exception
  {
  }

  public class GeneratorBudget
  {
    int budget;
    private GeneratorBudget()
    {
    }

    static GeneratorBudget instance = new GeneratorBudget();

    public static GeneratorBudget Instance { get { return instance; } }

    public int Budget { get { return budget; } set { budget = value; }}

    public void Spend()
    {
      budget--;
      if (budget < 0) throw new BudgetExhaustedException();
    }
  }

  public class BolzmannFailedException : Exception
  {
  }

  public class Bolztmann<T> : SizedGenerator<T>
  {
    Generator<T> gen;
    int threshold = 1000;

    public Bolztmann(Generator<T> gen, int target, float ep) : base(target, ep) {
      this.gen = gen;
    }


    public override T Generate(Random rnd)
    {
      int count = 0;
      do
      {
        try
        {
          GeneratorBudget.Instance.Budget = max;

          var candidate = gen.Generate(rnd);
          if (max - GeneratorBudget.Instance.Budget < min) continue;

        //	Console.WriteLine("Success after {0} steps. Size {1}", count, max - GeneratorBudget.Instance.Budget);
          return candidate;
        }
        catch (BudgetExhaustedException)
        {
        }
      } while (count++ < threshold);

      throw new BolzmannFailedException();
    }

  }


  /* Program generator
   *
   */

  public class ProgramGenerator : Generator<Program>
  {
    private ProgramGenerator()
    {
    }

    static ProgramGenerator instance = new ProgramGenerator();

    public static ProgramGenerator Instance
    {
      get { return instance; }
    }

    public override Program Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();

      // 0 to 3 functions in a program
      var program = FunctionDeclarartionGenerator.Instance.Generate(rnd, rnd.Next(0,4));
      return new Program(new List<FunctionDefinition>(program));
    }

  }


  /* type id
  *
  */

  public class VariableDeclarationGenerator : Generator<VariableDeclaration>
  {
    static IdentifierGenerator idGen = new IdentifierGenerator(1, 4);

    private VariableDeclarationGenerator()
    {
    }

    static VariableDeclarationGenerator instance = new VariableDeclarationGenerator();

    public static VariableDeclarationGenerator Instance
    {
      get { return instance; }
    }

    public override VariableDeclaration Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();

      var v = Enum.GetValues(typeof(Type));
      var type = (Type)v.GetValue(rnd.Next(v.Length - 1)); // exclude void

      var id = idGen.Generate(rnd);

      return new VariableDeclaration(type, id);
    }

  }

  /* type id ( param ) { stmt }
   *
   */

  public class FunctionDeclarartionGenerator : Generator<FunctionDefinition>
  {
    static IdentifierGenerator idGen = new IdentifierGenerator(1, 4);

    private FunctionDeclarartionGenerator()
    {
    }

    static FunctionDeclarartionGenerator instance = new FunctionDeclarartionGenerator();

    public static FunctionDeclarartionGenerator Instance
    {
      get { return instance; }
    }

    public override FunctionDefinition Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();
      var v = Enum.GetValues(typeof(Type));
      var type = (Type)v.GetValue(rnd.Next(v.Length));

      var id = idGen.Generate(rnd);

      // 0 to 3 arguments
      var parameters = VariableDeclarationGenerator.Instance.Generate(rnd, rnd.Next(4));

      // 0 to 3 statements in the body
      var body = StatementGenerator.Instance.Generate(rnd, rnd.Next(4));


      return new FunctionDefinition(type, id, new List<VariableDeclaration>(parameters), new List<Statement>(body));
    }

  }

  /* s
   *
   */

  public class StatementGenerator : Generator<Statement>
  {
    static Generator<Statement> stmtGen = new OneOf<Statement>(
      new List<Generator<Statement>>()
      {
      BlockStatementGenerator.Instance,
      IfStatementGenerator.Instance,
      WhileStatementGenerator.Instance,
      ReturnStatementGenerator.Instance,
      ExpressionStatementGenerator.Instance,
      VariableDeclarationStatementGenerator.Instance
      }
    );

    private StatementGenerator()
    {
    }

    static StatementGenerator instance = new StatementGenerator();

    public static StatementGenerator Instance
    {
      get { return instance; }
    }

    public override Statement Generate(Random rnd)
    {
      return stmtGen.Generate(rnd);
    }

  }



  /* { s1 s2 ... }
   *
   */

  public class BlockStatementGenerator : Generator<Statement>
  {
    private BlockStatementGenerator()
    {
    }

    static BlockStatementGenerator instance = new BlockStatementGenerator();

    public static BlockStatementGenerator Instance
    {
      get { return instance; }
    }

    public override Statement Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();
      var body = StatementGenerator.Instance.Generate(rnd, rnd.Next(4)); // 0 to 3 statements in a block
      return new BlockStatement(new List<Statement>(body));
    }

  }

  /* if (e) { stmts } (else { stmts })?
   *
   */

  public class IfStatementGenerator : Generator<Statement>
  {
    private IfStatementGenerator()
    {
    }

    static IfStatementGenerator instance = new IfStatementGenerator();

    public static IfStatementGenerator Instance
    {
      get { return instance; }
    }

    public override Statement Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();
      var expr = ExpressionGenerator.Instance.Generate(rnd);
      var consequent = BlockStatementGenerator.Instance.Generate(rnd); // only generate blocks

      if (rnd.Next(2) == 1) return new IfStatement(expr, consequent, null);

      var alternate = BlockStatementGenerator.Instance.Generate(rnd);

      return new IfStatement(expr, consequent, alternate);
    }

  }

  /* while (e) { stmts }
   *
   */

  public class WhileStatementGenerator : Generator<Statement>
  {
    private WhileStatementGenerator()
    {
    }

    static WhileStatementGenerator instance = new WhileStatementGenerator();

    public static WhileStatementGenerator Instance
    {
      get { return instance; }
    }

    public override Statement Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();
      var expr = ExpressionGenerator.Instance.Generate(rnd);
      var stmt = BlockStatementGenerator.Instance.Generate(rnd); // only generate blocks
      return new WhileStatement(expr, stmt);
    }

  }

  /* return e;
   *
   */

  public class ReturnStatementGenerator : Generator<Statement>
  {
    private ReturnStatementGenerator()
    {
    }

    static ReturnStatementGenerator instance = new ReturnStatementGenerator();

    public static ReturnStatementGenerator Instance
    {
      get { return instance; }
    }

    public override Statement Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();
      var expr = ExpressionGenerator.Instance.Generate(rnd);
      return new ReturnStatement(expr);
    }

  }

  /* e;
   *
   */

  public class ExpressionStatementGenerator : Generator<Statement>
  {
    private ExpressionStatementGenerator()
    {
    }

    static ExpressionStatementGenerator instance = new ExpressionStatementGenerator();

    public static ExpressionStatementGenerator Instance
    {
      get { return instance; }
    }

    public override Statement Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();
      var expr = ExpressionGenerator.Instance.Generate(rnd);
      return new ExpressionStatement(expr);
    }

  }

    /* type id;
   *
   */

  public class VariableDeclarationStatementGenerator : Generator<Statement>
  {
    static IdentifierGenerator idGen = new IdentifierGenerator(1, 4);

    private VariableDeclarationStatementGenerator()
    {
    }

    static VariableDeclarationStatementGenerator instance = new VariableDeclarationStatementGenerator();

    public static VariableDeclarationStatementGenerator Instance
    {
      get { return instance; }
    }

    public override Statement Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();

      var v = Enum.GetValues(typeof(Type));
      var type = (Type)v.GetValue(rnd.Next(v.Length - 1)); // exclude void

      var id = idGen.Generate(rnd);

      return new VariableDeclarationStatement(type, id);
    }

  }


  /* e
   *
   */

  public class ExpressionGenerator : Generator<Expression>
  {
    static Generator<Expression> exprGen = new OneOf<Expression>(
      new List<Generator<Expression>>()
      {
      VariableExpressionGenerator.Instance,
      IntegerLiteralExpressionGenerator.Instance,
      BooleanLiteralExpressionGenerator.Instance,
      BinaryOperatorExpressionGenerator.Instance,
      UnaryOperatorExpressionGenerator.Instance,
      AssignmentExpressionGenerator.Instance,
      FunctionCallExpressionGenerator.Instance
      }
    );

    private ExpressionGenerator()
    {
    }

    static ExpressionGenerator instance = new ExpressionGenerator();

    public static ExpressionGenerator Instance
    {
      get { return instance; }
    }

    public override Expression Generate(Random rnd)
    {
      return exprGen.Generate(rnd);
    }

  }

  /* id
   *
   */

  public class VariableExpressionGenerator : Generator<Expression>
  {

    static IdentifierGenerator idGen = new IdentifierGenerator(1,4);

    private VariableExpressionGenerator()
    {
    }

    static VariableExpressionGenerator instance = new VariableExpressionGenerator();

    public static VariableExpressionGenerator Instance
    {
      get { return instance; }
    }

    public override Expression Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();
      var id = idGen.Generate(rnd);
      return new VariableExpression(id);
    }
  }

  /* int
   *
   */

  public class IntegerLiteralExpressionGenerator : Generator<Expression>
  {

    static NumberGenerator numGen = new NumberGenerator(1, 4);

    private IntegerLiteralExpressionGenerator()
    {
    }

    static IntegerLiteralExpressionGenerator instance = new IntegerLiteralExpressionGenerator();

    public static IntegerLiteralExpressionGenerator Instance
    {
      get { return instance; }
    }

    public override Expression Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();
      var num = numGen.Generate(rnd);
      return new IntegerLiteralExpression(num);
    }
  }

  /* bool
   *
   */

  public class BooleanLiteralExpressionGenerator : Generator<Expression>
  {

    private BooleanLiteralExpressionGenerator()
    {
    }

    static BooleanLiteralExpressionGenerator instance = new BooleanLiteralExpressionGenerator();

    public static BooleanLiteralExpressionGenerator Instance
    {
      get { return instance; }
    }

    public override Expression Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();
      return new BooleanLiteralExpression(rnd.Next(2) == 1);
    }
  }

  /* e op e
   *
   */

  public class BinaryOperatorExpressionGenerator : Generator<Expression>
  {
    private BinaryOperatorExpressionGenerator()
    {
    }

    static BinaryOperatorExpressionGenerator instance = new BinaryOperatorExpressionGenerator();

    public static BinaryOperatorExpressionGenerator Instance
    {
      get { return instance; }
    }

    public override Expression Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();

      var v = Enum.GetValues(typeof(BinaryOperatorExpression.OperatorType));
      var op = (BinaryOperatorExpression.OperatorType) v.GetValue(rnd.Next(v.Length));

      var left = ExpressionGenerator.Instance.Generate(rnd);
      var right = ExpressionGenerator.Instance.Generate(rnd);

      return new BinaryOperatorExpression(op, left, right);
    }
  }

  /* op e
   *
   */

  public class UnaryOperatorExpressionGenerator : Generator<Expression>
  {
    private UnaryOperatorExpressionGenerator()
    {
    }

    static UnaryOperatorExpressionGenerator instance = new UnaryOperatorExpressionGenerator();

    public static UnaryOperatorExpressionGenerator Instance
    {
      get { return instance; }
    }

    public override Expression Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();

      var v = Enum.GetValues(typeof(UnaryOperatorExpression.OperatorType));
      var op = (UnaryOperatorExpression.OperatorType)v.GetValue(rnd.Next(v.Length));

      var expr = ExpressionGenerator.Instance.Generate(rnd);

      return new UnaryOperatorExpression(op, expr);
    }
  }

  /* id = e
   *
   */

  public class AssignmentExpressionGenerator : Generator<Expression>
  {
    static IdentifierGenerator idGen = new IdentifierGenerator(1, 4);

    private AssignmentExpressionGenerator()
    {
    }

    static AssignmentExpressionGenerator instance = new AssignmentExpressionGenerator();

    public static AssignmentExpressionGenerator Instance
    {
      get { return instance; }
    }

    public override Expression Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();

      var id = idGen.Generate(rnd);

      var expr = ExpressionGenerator.Instance.Generate(rnd);

      return new AssignmentExpression(id, expr);
    }
  }

  /* f(e1, e2, ...)
   *
   */

  public class FunctionCallExpressionGenerator : Generator<Expression>
  {
    static IdentifierGenerator idGen = new IdentifierGenerator(1, 4);

    private FunctionCallExpressionGenerator()
    {
    }

    static FunctionCallExpressionGenerator instance = new FunctionCallExpressionGenerator();

    public static FunctionCallExpressionGenerator Instance
    {
      get { return instance; }
    }

    public override Expression Generate(Random rnd)
    {
      GeneratorBudget.Instance.Spend();
      var id = idGen.Generate(rnd);

      // 0 to 3 arguments
      var arguments = ExpressionGenerator.Instance.Generate(rnd, rnd.Next(4));

      return new FunctionCallExpression(id, new List<Expression>(arguments));
    }
  }

}
