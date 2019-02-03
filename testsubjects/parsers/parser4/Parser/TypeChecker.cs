using System;
using System.Collections.Generic;

namespace Parser
{
    public enum Type
    {
        Void,
        Int,
        Bool
    }

    public class TypeEnvironment
    {
        public TypeEnvironment()
        {
            Variables = new Stack<Dictionary<string, Type>>();
            Functions = new Dictionary<string, FormalDeclaration>();
            ReturnedType = Type.Void;
            ExcpetedType = Type.Void;
        }

        public Stack<Dictionary<string, Type>> Variables { get; }
        public Dictionary<string, FormalDeclaration> Functions { get; }
        public Type ReturnedType { get; set; }
        public Type ExcpetedType { get; set; }
    }

    internal class ProgramTypeChecker : IProgramVisitor<object, TypeEnvironment>
    {
        private ProgramTypeChecker()
        {
        }

        public static ProgramTypeChecker Instance { get; } = new ProgramTypeChecker();

        public object Visit(Program p, TypeEnvironment env)
        {
            foreach (var decl in p.Body) decl.Accept(DeclarationTypeChecker.Instance, env);
            foreach (var function in env.Functions.Keys)
            {
                env.ReturnedType = Type.Void;
                switch (env.Functions[function].Type)
                {
                    case FormalDeclaration.IdType.Bool:
                        env.ExcpetedType = Type.Bool;
                        break;
                    case FormalDeclaration.IdType.Int:
                        env.ExcpetedType = Type.Int;
                        break;
                    case FormalDeclaration.IdType.Void:
                        env.ExcpetedType = Type.Void;
                        break;
                }

                var dict = new Dictionary<string, Type>();
                foreach (var formal in env.Functions[function].FormalList)
                    dict.Add(formal.Id, formal.Type == Formal.IdType.Bool ? Type.Bool : Type.Int);
                env.Variables.Push(dict);
                foreach (var stmt in env.Functions[function].Statements)
                    stmt.Accept(StatementTypeChecker.Instance, env);

                if (env.ExcpetedType != env.ReturnedType)
                    throw new Exception($"fail {env.Functions[function].Line} {env.Functions[function].Column}\n" +
                                        $"function expected to return {env.ExcpetedType} but returned {env.ReturnedType}.");
                env.Variables.Pop();
            }

            Console.WriteLine("pass");
            return null;
        }
    }

    internal class DeclarationTypeChecker : IDeclarationVisitor<object, TypeEnvironment>
    {
        private DeclarationTypeChecker()
        {
        }

        public static DeclarationTypeChecker Instance { get; } = new DeclarationTypeChecker();

        public object Visit(FormalDeclaration d, TypeEnvironment env)
        {
            var declarationName = d.Id;
            env.Functions.Add(declarationName, d);
            return null;
        }
    }

    internal class StatementTypeChecker : IStatementVisitor<Type, TypeEnvironment>
    {
        private StatementTypeChecker()
        {
        }

        public static StatementTypeChecker Instance { get; } = new StatementTypeChecker();

        public Type Visit(BlockStatement s, TypeEnvironment env)
        {
            env.Variables.Push(new Dictionary<string, Type>());
            var num = env.Variables.Count;
            var value = Type.Void;
            foreach (var stmt in s.Body)
            {
                value = stmt.Accept(this, env);
                if (env.Variables.Count < num) break;
            }

            if (env.Variables.Count == num) env.Variables.Pop();
            return value;
        }

        public Type Visit(IfStatement s, TypeEnvironment env)
        {
            var v = s.Condition.Accept(ExpressionTypeChecker.Instance, env);
            if (v != Type.Bool) throw new Exception($"fail {s.Line} {s.Column}\ncondition must of type bool.");
            s.Consequent.Accept(this, env);
            s.Alternate.Accept(this, env);

            return Type.Void;
        }

        public Type Visit(WhileStatement s, TypeEnvironment env)
        {
            var v = s.Condition.Accept(ExpressionTypeChecker.Instance, env);
            if (v != Type.Bool) throw new Exception($"fail {s.Line} {s.Column}\n condition must of type bool.");

            var value = s.Consequent.Accept(this, env);
            return value;
        }

        public Type Visit(ReturnStatement s, TypeEnvironment env)
        {
            env.ReturnedType = s.Expression?.Accept(ExpressionTypeChecker.Instance, env) ?? Type.Void;
            if (env.ExcpetedType != env.ReturnedType)
                throw new Exception($"fail {s.Expression?.Line ?? s.Line} {s.Expression?.Column ?? s.Column}\n" +
                                    $"function is expected to return {env.ExcpetedType}, but is returning {env.ReturnedType} instead.");

            return env.ReturnedType;
        }

        public Type Visit(ExpressionStatement s, TypeEnvironment env)
        {
            return s.Expression.Accept(ExpressionTypeChecker.Instance, env);
        }

        public Type Visit(FormalStatement s, TypeEnvironment env)
        {
            foreach (var variable in env.Variables)
                if (variable.ContainsKey(s.Formal.Id))
                    throw new Exception($"fail {s.Line} {s.Column}\nvariable already initialized.");

            env.Variables.Peek().Add(s.Formal.Id, s.Formal.Type == Formal.IdType.Int ? Type.Int : Type.Bool);
            return s.Formal.Type == Formal.IdType.Int ? Type.Int : Type.Bool;
        }
    }

    internal class ExpressionTypeChecker : IExpressionVisitor<Type, TypeEnvironment>
    {
        private ExpressionTypeChecker()
        {
        }

        public static ExpressionTypeChecker Instance { get; } = new ExpressionTypeChecker();

        public Type Visit(IdentifierExpression e, TypeEnvironment env)
        {
            var tempStack = new Stack<Dictionary<string, Type>>();
            var value = Type.Void;
            while (env.Variables.Count > 0)
            {
                tempStack.Push(env.Variables.Pop());
                if (!tempStack.Peek().ContainsKey(e.Id)) continue;
                value = tempStack.Peek()[e.Id];
                break;
            }

            if (env.Variables.Count == 0 && value == Type.Void)
                throw new Exception(
                    $"fail {e.Line} {e.Column}\n variable {e.Id} is not initialized.");

            while (tempStack.Count > 0) env.Variables.Push(tempStack.Pop());

            return value;
        }

        public Type Visit(NumberExpression e, TypeEnvironment env)
        {
            return Type.Int;
        }

        public Type Visit(BooleanExpression e, TypeEnvironment env)
        {
            return Type.Bool;
        }

        public Type Visit(AssignmentExpression e, TypeEnvironment env)
        {
            var value = e.Expression.Accept(this, env);
            var tempStack = new Stack<Dictionary<string, Type>>();
            while (env.Variables.Count > 0)
            {
                tempStack.Push(env.Variables.Pop());
                if (!tempStack.Peek().ContainsKey(e.Id)) continue;
                if (tempStack.Peek()[e.Id] != value)
                    throw new Exception(
                        $"fail {e.Line} {e.Column}\n type {value} cannot be assigned to variable {e.Id} of type {tempStack.Peek()[e.Id]}.");
                break;
            }

            if (env.Variables.Count == 0 && !tempStack.Peek().ContainsKey(e.Id))
                throw new Exception(
                    $"fail {e.Line} {e.Column}\n variable {e.Id} is not initialized.");

            while (tempStack.Count > 0) env.Variables.Push(tempStack.Pop());

            return value;
        }

        public Type Visit(BinaryOperatorExpression e, TypeEnvironment env)
        {
            var left = e.Left.Accept(this, env);
            var right = e.Right.Accept(this, env);

            switch (e.Typ)
            {
                case BinaryOperatorExpression.Type.Add:
                    if (left != Type.Int || right != Type.Int)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of type int for operator +.");
                    return Type.Int;
                case BinaryOperatorExpression.Type.Sub:
                    if (left != Type.Int || right != Type.Int)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of type int for operator -.");
                    return Type.Int;
                case BinaryOperatorExpression.Type.Mul:
                    if (left != Type.Int || right != Type.Int)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of type int for operator *.");
                    return Type.Int;
                case BinaryOperatorExpression.Type.Div:
                    if (left != Type.Int || right != Type.Int)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of type int for operator /.");
                    return Type.Int;
                case BinaryOperatorExpression.Type.And:
                    if (left != Type.Bool || right != Type.Bool)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of type bool for operator &&.");
                    return Type.Bool;
                case BinaryOperatorExpression.Type.Or:
                    if (left != Type.Bool || right != Type.Bool)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of type bool for operator ||.");
                    return Type.Bool;
                case BinaryOperatorExpression.Type.Eql:
                    if (left != right)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of equal type for operator ==.");
                    return Type.Bool;
                case BinaryOperatorExpression.Type.NEql:
                    if (left != right)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of equal type for operator !=.");
                    return Type.Bool;
                case BinaryOperatorExpression.Type.Grt:
                    if (left != Type.Int || right != Type.Int)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of type int for operator >.");
                    return Type.Bool;
                case BinaryOperatorExpression.Type.GEql:
                    if (left != Type.Int || right != Type.Int)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of type int for operator >=.");
                    return Type.Bool;
                case BinaryOperatorExpression.Type.Less:
                    if (left != Type.Int || right != Type.Int)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of type int for operator <.");
                    return Type.Bool;
                case BinaryOperatorExpression.Type.LEql:
                    if (left != Type.Int || right != Type.Int)
                        throw new Exception(
                            $"fail {e.Line} {e.Column}\nopperands should be of type int for operator <=.");
                    return Type.Bool;
                default:
                    throw new Exception($"fail {e.Line} {e.Column}\nsomething went wrong.");
            }
        }

        public Type Visit(UnaryOperatorExpression e, TypeEnvironment env)
        {
            var type = e.Expression.Accept(this, env);
            switch (e.Typ)
            {
                case UnaryOperatorExpression.Type.Neg when type != Type.Int:
                    throw new Exception($"fail {e.Line} {e.Column}\nopperand should be of type int for operator -.");
                case UnaryOperatorExpression.Type.Not when type != Type.Bool:
                    throw new Exception($"fail {e.Line} {e.Column}\nopperand should be of type bool for operator !.");
                default:
                    return type;
            }
        }

        public Type Visit(FunctionCallExpression e, TypeEnvironment env)
        {
            if (e.Id.Equals("print"))
            {
                foreach (var v in e.ListExpr) v.Accept(this, env);
                return Type.Void;
            }

            if (!env.Functions.ContainsKey(e.Id))
                throw new Exception(
                    $"fail {e.Line} {e.Column}\nfunction {e.Id} is not declared.");

            var dec = env.Functions[e.Id];
            if (dec.FormalList.Count != e.ListExpr.Count)
                throw new Exception($"fail {e.Line} {e.Column}\n" +
                                    $"wrong number of parameters passed to function {e.Id} " +
                                    $"(passed {e.ListExpr.Count}, excpeted {dec.FormalList.Count}).");

            for (var i = 0; i < e.ListExpr.Count; i++)
            {
                var typeE = e.ListExpr[i].Accept(this, env);
                var typeF = dec.FormalList[i].Type == Formal.IdType.Bool ? Type.Bool : Type.Int;
                if (typeE != typeF)
                    throw new Exception($"fail {e.Line} {e.Column}\n" +
                                        $"wrong type of variable passed to parameter {dec.FormalList[i].Id} " +
                                        $"(passed {typeE}, excpeted {typeF}).");
            }

            switch (dec.Type)
            {
                case FormalDeclaration.IdType.Void:
                    return Type.Void;
                case FormalDeclaration.IdType.Int:
                    return Type.Int;
                case FormalDeclaration.IdType.Bool:
                    return Type.Bool;
                default:
                    throw new Exception($"fail {e.Line} {e.Column}\nsomething went wrong.");
            }
        }
    }
}