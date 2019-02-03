using System;
using System.Text;
using System.Collections.Generic;

namespace Parser {

    

    public interface Locatable_interface
    {
        void SetLocation(QUT.Gppg.LexLocation loc);
        int Line { get; set; }
    }


    public class Locatable {
        private int column;
        private int line;

        public int Line { get => line; set => line = value; }
        public int Column { get => column; set => column = value; }

        public void SetLocation(QUT.Gppg.LexLocation loc) {
            line = loc.StartLine;
            column = loc.StartColumn;
        }
    }


    //ipretty
    public partial class Variable : IPretty {
        //bör kolla om en datatyper tillåts
        //type ID
        public string type;
        public string id;

        public Variable(string type, string id) {
            this.type = type;
            this.id = id;
        }
    }



    //ipretty
    public partial class ParsedProgram : Locatable, IPretty {
        //Program ::= Decl*
        public List<Declaration> declarations;

        public ParsedProgram(List<Declaration> declarations)
        {
            this.declarations = declarations;
        }
    }

    //ipretty
    public partial class Declaration : Locatable, IPretty {
        //Decl ::= Type id ( FormalList ) { Stmt* }
        //Decl ::= void id (FormalList )  { Stmt* }

        //technically not a Variable, it's return type and function name
        public string type;
        public string id;

        public List<Variable> formalList;
        public BlockStatement stmts;

        public Declaration(string type, string id, List<Variable> formalList, BlockStatement stmts) {
            this.type = type;
            this.id = id;
            //stringcopy?
            this.formalList = formalList;
            this.stmts = stmts;
        }
    }

    
    
    //ipretty
    public abstract class Statement : Locatable, IPretty {
        public abstract void Pretty(PrettyBuilder b);
    }

    public partial class BlockStatement : Statement {
        //Stmt ::= { Stmt* }
        public List<Statement> statements;

        public BlockStatement(List<Statement> statements) {
            this.statements = statements;
        }
    }

    public partial class IfStatement : Statement {
        //Stmt ::= if ( Expr ) { Stmts* }
        public Expression expr;
        public BlockStatement stmts;

        public IfStatement(Expression expr, BlockStatement stmts) {
            this.expr = expr;
            this.stmts = stmts;
        }
    }

    public partial class IfElseStatement : Statement {
        //Stmt ::= if ( Expr ) { Stmts1} else { Stmts2 }
        public Expression expr;
        public BlockStatement stmts1;
        public BlockStatement stmts2;

        public IfElseStatement(Expression expr, BlockStatement stmts1, BlockStatement stmts2) {
            this.expr = expr;
            this.stmts1 = stmts1;
            this.stmts2 = stmts2;
        }
    }

    public partial class WhileStatement : Statement {
        //Stmt ::= while ( Expr )  { Stmts* } ;
        public Expression expr;
        public BlockStatement stmts;

        public WhileStatement(Expression expr, BlockStatement stmts) {
            this.expr = expr;
            this.stmts = stmts;
        }
    }

    public partial class ReturnStatement : Statement {
        //Stmt ::= return Expr ;
        //Stmt ::= return ;
        public Expression expr;

        public ReturnStatement(Expression expr) {
            this.expr = expr;
        }

        public ReturnStatement() {
            this.expr = null;
        }
    }

    public partial class ExpressionStatement : Statement {
        //Stmt ::= Expr ;
        public Expression expr;

        public ExpressionStatement(Expression expr) {
            this.expr = expr;
        }
    }

    public partial class VariableStatement : Statement {
        //Stmt ::= type id ;
        public Variable variable;

        public VariableStatement(string type, string id) {
            this.variable = new Variable(type, id);
        }
    }


    //Expressions
    public enum Operator { ASN, OR, AND, EQ, NEQ, LT, GT, LTE, GTE, ADD, NEG, MUL, DIV, NOT };
    public abstract class Expression : Locatable, IPretty {
        public abstract void Pretty(PrettyBuilder b);
        public static string OpToString(Operator op)
        {
            switch (op)
            {
                case (Operator.ADD):
                    return "+";
                case (Operator.AND):
                    return "&&";
                case (Operator.DIV):
                    return "/";
                case (Operator.EQ):
                    return "==";
                case (Operator.GT):
                    return ">";
                case (Operator.GTE):
                    return ">=";
                case (Operator.LT):
                    return "<";
                case (Operator.LTE):
                    return "<=";
                case (Operator.MUL):
                    return "*";
                case (Operator.NEG):
                    return "-";
                case (Operator.NEQ):
                    return "!=";
                case (Operator.OR):
                    return "||";
                case (Operator.ASN):
                    return "=";
                case (Operator.NOT):
                    return "!";
                default:
                    throw new NotImplementedException("Failiure in op totostring");
            }
        }
    }
    


    public partial class NumExpression : Expression {
        //Expr ::= int
        public string num;

        public NumExpression(string num) {
            this.num = num;
        }
    }

    public partial class BooleanExpression : Expression {
        // Expr ::= true | false
        public Boolean boolean;

        public BooleanExpression(Boolean boolean) {
            //this.boolean = Convert.ToBoolean(boolean);
            this.boolean = boolean;
        }
    }

    public partial class BinaryOperatorExpression : Expression {
        //Expr ::= Expr bop Expr
        public Operator op;
        public Expression left;
        public Expression right;

        
        
        public static int OpToPrecedence(Operator op)
        {
            switch (op)
            {
                case (Operator.ASN):
                    return 1;
                case (Operator.OR):
                    return 2;
                case (Operator.AND):
                    return 3;
                case (Operator.EQ):
                    return 4;
                case (Operator.NEQ):
                    return 4;
                case (Operator.LT):
                    return 5;
                case (Operator.GT):
                    return 5;
                case (Operator.LTE):
                    return 5;
                case (Operator.GTE):
                    return 5;
                case (Operator.ADD):
                    return 6;
                case (Operator.NEG):
                    return 6;
                case (Operator.MUL):
                    return 7;
                case (Operator.DIV):
                    return 7;
                default:
                    throw new NotImplementedException("Fail in OpToPrecedence");
            }
        }
        public enum Direction { LEFT, RIGHT, BOTH };
        private static Dictionary<Operator, Direction> asdf = new Dictionary<Operator, Direction> {
            {Operator.ASN, Direction.RIGHT},
            {Operator.OR, Direction.LEFT},
            {Operator.AND, Direction.LEFT},
            {Operator.EQ, Direction.LEFT},
            {Operator.NEQ, Direction.LEFT},
            {Operator.LT, Direction.LEFT},
            {Operator.GT, Direction.LEFT},
            {Operator.LTE, Direction.LEFT},
            {Operator.GTE, Direction.LEFT},
            {Operator.ADD, Direction.BOTH},
            {Operator.NEG, Direction.LEFT},
            {Operator.MUL, Direction.BOTH},
            {Operator.DIV, Direction.LEFT}
        };

        public static bool IsOpposite(Direction childDirection, Operator parentOperator) {
            if (asdf.TryGetValue(parentOperator, out Direction dir))
            {
                if (dir != childDirection)
                    return true;
                return false;
            }
            else
                throw new NotImplementedException("AbstractSyntax.cs, IsOpposite: Unexpected behaviour.\n");
        }

        public BinaryOperatorExpression(Expression left, Operator op, Expression right) {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }

    //not partial, because it inherits from BopExpression
    public class AssignmentExpression : BinaryOperatorExpression
    {
        //Expr ::= id = Expr
        
        public AssignmentExpression(string id, Expression expr) 
            : base(new IdentifierExpression(id), Operator.ASN, expr)
        {}
    }

    public partial class IdentifierExpression : Expression {
        //Expr ::= id
        public string id;

        public IdentifierExpression(string id) {
            this.id = id;
        }
    }

    public partial class UnaryOperatorExpression : Expression {
        //Expr ::= uop Expr
        public Operator op;
        public Expression expr;

    public UnaryOperatorExpression(Operator type, Expression expr) {
            this.op = type;
            this.expr = expr;
        }
    }

    public partial class FunctionCallExpression : Expression {
        //Expr ::= id(ExprList )
        public string id; //function name

        //public ExprList exprList;
        public List<Expression> exprList; //arguments

        public FunctionCallExpression(string id, List<Expression> exprList) {
            this.id = id;
            this.exprList = exprList;
        }
    }
}
