%output = Generated\Parser.cs
%namespace Parser
//the value definition is added after groping around in the lexer file


%union {
  public string value;
  public Expression Expr;
  public Statement Stmt;
  public List<Expression> Exprs;
  public List<Statement> Stmts;
  public List<Variable> formalList;
  public Variable variable;
  public Declaration declaration;
  public List<Declaration> Decls;
  public ParsedProgram Program;
  public BlockStatement BlockStmt;
}

//ID and NUM are semantic, the rest are covered by rules in lexxer.l
%token <value> ID "id"
%token <value> NUM "num"
%token <value> ERR


%token <value> VOID "void"
%token LPAR "("
%token RPAR ")"
%token LCUR "{"
%token RCUR "}"
%token COM ","
%token <value> INT "int"
%token <value> BOOL "bool"
%token IF "if"
%token ELSE "else"
%token WHILE "while"
%token SEMI ";"
%token <value> TRUE "true"
%token <value> FALSE "false"
%token RETURN "return"
%token ASN "="

//kan ta bort value
%token <value> OR "||"
%token <value> AND "&&"
%token <value> EQ "=="
%token <value> NEQ "!="
%token <value> LT "<"
%token <value> GT ">"
%token <value> LTE "<="
%token <value> GTE ">="
%token <value> ADD "+"
%token <value> NEG "-"
%token <value> MUL "*"
%token <value> DIV "/"
%token <value> NOT "!"


//TODO: some kind of type declaration thingy
%type <Expr> Expr, EExpr //, FExpr, GExpr
%type <value> FirstID
%type <Exprs> ExprList, NotFirstExpr
%type <Stmts> Stmts
%type <Stmt> Stmt
%type <value> Type
%type <formalList> FormalList, NotFirstID
%type <variable> FirstID
%type <declaration> Decl
%type <Decls> Decls //ulf
%type <Program> Program
%type <BlockStmt> BlockStmt

//precedences

%nonassoc THEN
%nonassoc ELSE

%nonassoc NOBOP
%nonassoc BOP
/*
%nonassoc NONEG
%nonassoc NEG
*/

%right ASN
%left OR
%left AND
%left EQ NEQ
%left LT GT LTE GTE
%left ADD NEG
%left MUL DIV
%nonassoc NOT

%%
Program :  Decl Decls								{ $2.Insert(0, $1); Program = new ParsedProgram($2); }
		|											{ Program = new ParsedProgram(new List<Declaration>()); }	
        ;
		
Decls : Decl Decls									{ $2.Insert(0, $1); $$ = $2; }
	  |												{ $$ = new List<Declaration>(); }
	  ;

Decl : Type ID "(" FormalList ")" "{" Stmts "}"		{ $$ = new Declaration($1, $2, $4, ( new BlockStatement($7) )); } 
     | VOID ID "(" FormalList ")" "{" Stmts "}"		{ $$ = new Declaration("void", $2, $4, ( new BlockStatement($7) )); } 
	 ;
	 

	 
Stmts : Stmt Stmts							{ $$ = $2; $$.Insert(0, $1); } 
	  |										{ $$ = new List<Statement>();}
	  ;

FormalList : FirstID NotFirstID				{ $$ = $2; $$.Insert(0, $1);  } 
           |								{ $$ = new List<Variable>(); }
		   ;
			 
FirstID : Type ID							{ $$ = new Variable($1, $2); }
		;
			 
NotFirstID : "," FirstID NotFirstID			{ $$ = $3; $$.Insert(0, $2); } 
		   |								{ $$ = new List<Variable>(); }
		   ;
		   


Type : INT									{ $$ = "int"; }
     | BOOL									{ $$ = "bool"; }
	 ;



Stmt :  BlockStmt									{ $$ = $1;  $$.SetLocation(@1); }
	 |  IF "(" Expr ")" BlockStmt  %prec THEN		{ $$ = new IfStatement($3, $5); $$.SetLocation(@1);  }
     |  IF "(" Expr ")" BlockStmt "else" BlockStmt	{ $$ = new IfElseStatement($3, $5, $7); $$.SetLocation(@1);  }
     |  WHILE "(" Expr ")" BlockStmt				{ $$ = new WhileStatement($3, $5); $$.SetLocation(@1);  }
     |  RETURN Expr SEMI							{ $$ = new ReturnStatement($2); $$.SetLocation(@1);  }
     |  RETURN SEMI									{ $$ = new ReturnStatement(); $$.SetLocation(@1);  }
     |  Expr SEMI									{ $$ = new ExpressionStatement($1); $$.SetLocation(@1);  }
     |  Type ID SEMI								{ $$ = new VariableStatement($1, $2); $$.SetLocation(@1);  }
	 ;

BlockStmt : "{" Stmts "}"							{ $$ = new BlockStatement($2); $$.SetLocation(@1); }
		  ;



Expr : NUM									{ $$ = new NumExpression($1); $$.SetLocation(@1); }
      |  TRUE								{ $$ = new BooleanExpression(true); $$.SetLocation(@1); }
      |  FALSE								{ $$ = new BooleanExpression(false); $$.SetLocation(@1); }
      |  ID "=" Expr						{ $$ = new AssignmentExpression($1, $3); $$.SetLocation(@1); }
      |  ID									{ $$ = new IdentifierExpression($1); $$.SetLocation(@1); }
      |  ID "(" ExprList ")"				{ $$ = new FunctionCallExpression($1, $3); $$.SetLocation(@1); }
      |  "(" Expr ")"						{ $$ = $2; $$.SetLocation(@1); }
	  |  EExpr
	  ;


EExpr : Expr "+" Expr						{$$ = new BinaryOperatorExpression($1, Operator.ADD, $3); $$.SetLocation(@1);}
	  | Expr "||" Expr						{$$ = new BinaryOperatorExpression($1, Operator.OR, $3); $$.SetLocation(@1);}
	  | Expr "&&" Expr						{$$ = new BinaryOperatorExpression($1, Operator.AND, $3); $$.SetLocation(@1);}
	  | Expr "==" Expr						{$$ = new BinaryOperatorExpression($1, Operator.EQ, $3); $$.SetLocation(@1);}
	  | Expr "!=" Expr						{$$ = new BinaryOperatorExpression($1, Operator.NEQ, $3); $$.SetLocation(@1);}
	  | Expr "<" Expr						{$$ = new BinaryOperatorExpression($1, Operator.LT, $3); $$.SetLocation(@1);}
	  | Expr ">" Expr						{$$ = new BinaryOperatorExpression($1, Operator.GT, $3); $$.SetLocation(@1);}
	  | Expr "<=" Expr						{$$ = new BinaryOperatorExpression($1, Operator.LTE, $3); $$.SetLocation(@1);}
	  | Expr ">=" Expr						{$$ = new BinaryOperatorExpression($1, Operator.GTE, $3); $$.SetLocation(@1);}
	  | Expr "-" Expr						{$$ = new BinaryOperatorExpression($1, Operator.NEG, $3); $$.SetLocation(@1);}
	  | Expr "*" Expr						{$$ = new BinaryOperatorExpression($1, Operator.MUL, $3); $$.SetLocation(@1);}
	  | Expr "/" Expr						{$$ = new BinaryOperatorExpression($1, Operator.DIV, $3); $$.SetLocation(@1);}
	  | "-" Expr %prec MUL					{$$ = new UnaryOperatorExpression(Operator.NEG, $2); $$.SetLocation(@1);}
	  | "!" Expr							{$$ = new UnaryOperatorExpression(Operator.NOT, $2); $$.SetLocation(@1);}
	  ;
	  
	 

ExprList : Expr NotFirstExpr				{ $$ = $2; $$.Insert(0, $1); }//new List<Expression>($2); $$.Insert(0, $1); } 
         |									{ $$ = new List<Expression>(); }
		 ;
		 
NotFirstExpr : "," Expr NotFirstExpr		{ $$ = $3; $$.Insert(0, $2); }
             |								{ $$ = new List<Expression>(); }
		     ;
		  

%%

public Parser( Scanner s ) : base( s ) { }
public ParsedProgram Program;
