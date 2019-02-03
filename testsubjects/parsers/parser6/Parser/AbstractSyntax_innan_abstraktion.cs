using System;
using System.Text;
using System.Collections.Generic;

namespace Parser {


    public interface Locatablee
    {
        void SetLocation(QUT.Gppg.LexLocation loc);
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


    public class Variable {
        //bör kolla om en datatyper tillåts
        public string type;
        public string id;

        public Variable(string type, string id) {
            this.type = type;
            this.id = id;
        }
    }




    public class ParsedProgram : Locatable {
        //Program ::= Decl*
        public List<Declaration> declarations;
        public ParsedProgram(List<Declaration> declarations)
        {
            this.declarations = declarations;
        }
    }

    public class Declaration : Locatable {
        //Decl ::= Type id ( FormalList ) { Stmt* }
        //Decl ::= void id (FormalList )  { Stmt* }

        //technically not a variable, it's return type and function name
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

    
    

    public class Statement : Locatable {

    }

    public partial class BlockStatement : Statement {
        //Stmt ::= { Stmt* }
        public List<Statement> statements;

        public BlockStatement(List<Statement> statements) {
            this.statements = statements;
        }
    }

    public partial class IfStatement : Statement {
        //Stmt ::= if ( Expr ) Stmt
        public Expression expr;
        public Statement stmt;

        public IfStatement(Expression expr, Statement stmt) {
            this.expr = expr;
            this.stmt = stmt;
        }
    }

    public partial class IfElseStatement : Statement {
        //Stmt ::= if ( Expr ) Stmt1 else Stmt2
        public Expression expr;
        public Statement stmt1;
        public Statement stmt2;

        public IfElseStatement(Expression expr, Statement stmt1, Statement stmt2) {
            this.expr = expr;
            this.stmt1 = stmt1;
            this.stmt1 = stmt2;
        }
    }

    public partial class WhileStatement : Statement {
        //Stmt ::= while ( Expr ) Stmt ;
        public Expression expr;
        public Statement stmt;

        public WhileStatement(Expression expr, Statement stmt) {
            this.expr = expr;
            this.stmt = stmt;
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

    public class Expression : Locatable {
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
        public string boolean;

        public BooleanExpression(string boolean) {
            //this.boolean = Convert.ToBoolean(boolean);
            this.boolean = boolean;
        }
    }

    public partial class BinaryOperatorExpression : Expression {
        //Expr ::= Expr bop Expr
        public string op;
        public Expression left;
        public Expression right;

        public BinaryOperatorExpression(Expression left, String op, Expression right) {
            this.left = left;
            this.op = op;
            this.right = right;
        }
    }

    public partial class AssignmentExpression : Expression {
        //Expr ::= id = Expr
        public string id;
        public Expression expr;

        public AssignmentExpression(string id, Expression expr) {
            this.id = id;
            this.expr = expr;
        }
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
        public string type;
        public Expression expr;

        public UnaryOperatorExpression(string type, Expression expr) {
            this.type = type;
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

    public partial class ParenthesisExpression : Expression {
        //Expr ::= ( Expr )
        public Expression expr;

        public ParenthesisExpression(Expression expr) {
            this.expr = expr;
        }
    }


}
