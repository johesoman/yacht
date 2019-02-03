using System;
using System.Collections.Generic;

namespace Parser
{
    public class Environment
    {
        public Environment()
        {
            Variables = new Stack<Dictionary<string, object>>();
            Functions = new Dictionary<string, FormalDeclaration>();
        }

        public Stack<Dictionary<string, object>> Variables { get; }
        public Dictionary<string, FormalDeclaration> Functions { get; }
    }

    internal class ProgramEvaluator : IProgramVisitor<object, Environment>
    {
        private ProgramEvaluator()
        {
        }

        public static ProgramEvaluator Instance { get; } = new ProgramEvaluator();

        public object Visit(Program p, Environment env)
        {
            foreach (var decl in p.Body) decl.Accept(DeclarationEvaluator.Instance, env);

            if (!env.Functions.ContainsKey("main"))
                throw new Exception("INTERPRETATION ERROR: could not find main function.");

            var main = env.Functions["main"];
            return ExpressionEvaluator.Instance.Visit(new FunctionCallExpression(main.Id, new List<Expression>()), env);
        }
    }

    internal class DeclarationEvaluator : IDeclarationVisitor<object, Environment>
    {
        private DeclarationEvaluator()
        {
        }

        public static DeclarationEvaluator Instance { get; } = new DeclarationEvaluator();

        public object Visit(FormalDeclaration d, Environment env)
        {
            var declarationName = d.Id;
            env.Functions.Add(declarationName, d);
            return null;
        }
    }

    internal class StatementEvaluator : IStatementVisitor<object, Environment>
    {
        private StatementEvaluator()
        {
        }

        public static StatementEvaluator Instance { get; } = new StatementEvaluator();

        public object Visit(BlockStatement s, Environment env)
        {
            env.Variables.Push(new Dictionary<string, object>());
            var num = env.Variables.Count;
            object value = null;
            foreach (var stmt in s.Body)
            {
                value = stmt.Accept(this, env);
                if (env.Variables.Count < num) break;
            }

            env.Variables.Pop();
            return value;
        }

        public object Visit(IfStatement s, Environment env)
        {
            var v = s.Condition.Accept(ExpressionEvaluator.Instance, env);
            object value;
            if ((bool) v)
                value = s.Consequent.Accept(this, env);
            else
                value = s.Alternate.Accept(this, env);

            return value;
        }

        public object Visit(WhileStatement s, Environment env)
        {
            var v = s.Condition.Accept(ExpressionEvaluator.Instance, env);
            object value = null;
            while (v is bool b && b)
            {
                value = s.Consequent.Accept(this, env);
                v = s.Condition.Accept(ExpressionEvaluator.Instance, env);
            }

            if (!(v is bool))
            {
                throw new Exception($"INTERPRETATION ERROR: line {s.Line} column {s.Column}, condition must be boolean.");
            }
            return value;
        }

        public object Visit(ReturnStatement s, Environment env)
        {
            var value = s.Expression?.Accept(ExpressionEvaluator.Instance, env);
            env.Variables.Pop();
            return value;
        }

        public object Visit(ExpressionStatement s, Environment env)
        {
            return s.Expression.Accept(ExpressionEvaluator.Instance, env);
        }

        public object Visit(FormalStatement s, Environment env)
        {
            env.Variables.Peek().Add(s.Formal.Id, s.Formal.Type == Formal.IdType.Int ? 0 : (object) false);
            return null;
        }
    }

    internal class ExpressionEvaluator : IExpressionVisitor<object, Environment>
    {
        private ExpressionEvaluator()
        {
        }

        public static ExpressionEvaluator Instance { get; } = new ExpressionEvaluator();

        public object Visit(IdentifierExpression e, Environment env)
        {
            var tempStack = new Stack<Dictionary<string, object>>();
            object value = null;
            while (env.Variables.Count > 0)
            {
                tempStack.Push(env.Variables.Pop());
                if (!tempStack.Peek().ContainsKey(e.Id)) continue;
                value = tempStack.Peek()[e.Id];
                break;
            }

            if (env.Variables.Count == 0 && value == null)
                throw new Exception(
                    $"INTERPRETATION ERROR: line {e.Line} column {e.Column}, variable {e.Id} doesn't contain a value.");

            while (tempStack.Count > 0) env.Variables.Push(tempStack.Pop());

            return value;
        }

        public object Visit(NumberExpression e, Environment env)
        {
            return e.Num;
        }

        public object Visit(BooleanExpression e, Environment env)
        {
            return e.Boolean;
        }

        public object Visit(AssignmentExpression e, Environment env)
        {
            var value = e.Expression.Accept(this, env);
            var tempStack = new Stack<Dictionary<string, object>>();
            while (env.Variables.Count > 0)
            {
                tempStack.Push(env.Variables.Pop());
                if (!tempStack.Peek().ContainsKey(e.Id)) continue;
                tempStack.Peek()[e.Id] = value;
                break;
            }

            if (env.Variables.Count == 0 && !tempStack.Peek().ContainsKey(e.Id))
                throw new Exception(
                    $"INTERPRETATION ERROR: line {e.Line} column {e.Column}, variable {e.Id} is not initialized.");

            while (tempStack.Count > 0) env.Variables.Push(tempStack.Pop());

            return value;
        }

        public object Visit(BinaryOperatorExpression e, Environment env)
        {
            var left = e.Left.Accept(this, env);
            if (left is bool b1)
            {
                switch (e.Typ)
                {
                    case BinaryOperatorExpression.Type.Or:
                    {
                        if (b1) return true;
                        var right = e.Right.Accept(this, env);
                        if (right is bool b2) return b2;
                        throw new Exception($"INTERPRETATION ERROR: line {e.Line} column {e.Column}, wrong variable types.");
                    }
                    case BinaryOperatorExpression.Type.And:
                    {
                        if (!b1) return false;
                        var right = e.Right.Accept(this, env);
                        if (right is bool b2) return b2;
                        throw new Exception($"INTERPRETATION ERROR: line {e.Line} column {e.Column}, wrong variable types.");
                    }
                    case BinaryOperatorExpression.Type.Eql:
                    {
                        var right = e.Right.Accept(this, env);
                        if (right is bool b2) return b1 == b2;
                        throw new Exception($"INTERPRETATION ERROR: line {e.Line} column {e.Column}, wrong variable types.");
                    }
                    case BinaryOperatorExpression.Type.NEql:
                    {
                        var right = e.Right.Accept(this, env);
                        if (right is bool b3) return b1 != b3;
                        throw new Exception($"INTERPRETATION ERROR: line {e.Line} column {e.Column}, wrong variable types.");
                    }
                }

                throw new Exception($"INTERPRETATION ERROR: line {e.Line} column {e.Column}, operation not suitable for booleans.");
            }
            var righty = e.Right.Accept(this, env);
            if (!(left is int i1) || !(righty is int i2))
                throw new Exception($"INTERPRETATION ERROR: line {e.Line} column {e.Column}, wrong variable types.");
            switch (e.Typ)
            {
                case BinaryOperatorExpression.Type.Eql:
                    return i1 == i2;
                case BinaryOperatorExpression.Type.NEql:
                    return i1 != i2;
                case BinaryOperatorExpression.Type.Add:
                    return i1 + i2;
                case BinaryOperatorExpression.Type.Sub:
                    return i1 - i2;
                case BinaryOperatorExpression.Type.Mul:
                    return i1 * i2;
                case BinaryOperatorExpression.Type.Div:
                    return i1 / i2;
                case BinaryOperatorExpression.Type.Grt:
                    return i1 > i2;
                case BinaryOperatorExpression.Type.Less:
                    return i1 < i2;
                case BinaryOperatorExpression.Type.GEql:
                    return i1 >= i2;
                case BinaryOperatorExpression.Type.LEql:
                    return i1 <= i2;
                default:
                    throw new Exception($"INTERPRETATION ERROR: line {e.Line} column {e.Column}, operation not suitable for integers.");
            }
        }

        public object Visit(UnaryOperatorExpression e, Environment env)
        {
            if (e.Typ == UnaryOperatorExpression.Type.Neg) return -(int) e.Expression.Accept(this, env);

            return !(bool) e.Expression.Accept(this, env);
        }

        public object Visit(FunctionCallExpression e, Environment env)
        {
            if (e.Id.Equals("print"))
            {
                foreach (var v in e.ListExpr)
                {
                    var val = v.Accept(this, env);
                    Console.Write($"{val} ");
                }

                Console.Write("\n");
                return null;
            }

            if (!env.Functions.ContainsKey(e.Id))
                throw new Exception(
                    $"INTERPRETATION ERROR: line {e.Line} column {e.Column}, function {e.Id} does not exist.");

            var dec = env.Functions[e.Id];
            if (dec.FormalList.Count != e.ListExpr.Count)
                throw new Exception($"INTERPRETATION ERROR: line {e.Line} column {e.Column}," +
                                    $" wrong number of parameters passed to function {e.Id} " +
                                    $"(passed {e.ListExpr.Count}, excpeted {dec.FormalList.Count}).");

            var dict = new Dictionary<string, object>();
            for (var i = 0; i < dec.FormalList.Count; i++)
                dict.Add(dec.FormalList[i].Id, e.ListExpr[i].Accept(this, env));
            env.Variables.Push(dict);
            var num = env.Variables.Count;

            object value = null;
            foreach (var stmt in dec.Statements)
            {
                value = stmt.Accept(StatementEvaluator.Instance, env);
                if (env.Variables.Count < num) break;
            }

            if (env.Variables.Count != num)
            {
                if (dec.Type == FormalDeclaration.IdType.Void && value != null)
                {
                    throw new Exception(
                        $"INTERPRETATION ERROR: line {e.Line} column {e.Column}, function {e.Id} should not return a value.");
                }

                if (dec.Type != FormalDeclaration.IdType.Void && value == null)
                {
                    throw new Exception(
                        $"INTERPRETATION ERROR: line {e.Line} column {e.Column}, function {e.Id} should return a value.");
                }
                return value;
            }
            if (dec.Type != FormalDeclaration.IdType.Void)
                throw new Exception(
                    $"INTERPRETATION ERROR: line {e.Line} column {e.Column}, function {e.Id} does not have return stetement.");
            env.Variables.Pop();
            return null;
        }
    }
}