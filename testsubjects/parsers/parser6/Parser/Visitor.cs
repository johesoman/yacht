using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parser
{
    interface Expr
    {
        R Accept<R, A>(ExprVisitor<R, A> v, A arg);
    }

    interface ExprVisitor<R, A>
    {
        R Visit(NumExpr e, A arg);
        R Visit(VarExpr e, A arg);
        R Visit(NegExpr e, A arg);
    }


    class EvalVisitor : ExprVisitor<int, Dictionary<string, int>>
    {
        public int Visit(NumExpr e, Dictionary<string, int> d)
        {
            return e.value;
        }

        public int Visit(VarExpr e, Dictionary<string, int> d)
        {
            return d[e.id];
        }

        public int Visit(NegExpr e, Dictionary<string, int> d)
        {
            return -e.expr.Accept(this, d);
        }
    }


    class VarExpr : Expr
    {
        public string id;

        public VarExpr(string id)
        {
            this.id = id;
        }

        public T Accept<T, A>(ExprVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }

    }

    class NumExpr : Expr
    {
        public int value;

        public NumExpr(int n)
        {
            value = n;
        }

        public T Accept<T, A>(ExprVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }


    }

    class NegExpr : Expr
    {
        public Expr expr;

        public NegExpr(Expr expr)
        {
            this.expr = expr;
        }

        public T Accept<T, A>(ExprVisitor<T, A> v, A arg)
        {
            return v.Visit(this, arg);
        }


    }
}
