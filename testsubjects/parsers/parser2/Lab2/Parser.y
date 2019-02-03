%output=Generated/Parser.cs
%namespace Parser

%union {
  public string value;

  public List<Declaration> P;
  public Declaration DECL;
  public List<Statement> S;
  public Statement STMT;
  public List<Parameter> FL;
  public Type_ T;

  public BopExpression.Operator B;

  public Expression E;
  public List<Expression> EL;
}

%token <value> ID
%token <value> NUM
%token <value> ERR

%token SEMI ";"
%token ASN "="
%token LPAR "("
%token RPAR ")"
%token COMMA ","
%token LBRA "{"
%token RBRA "}"
%token INT "int"
%token BOOL "bool"
%token IF "if"
%token ELSE "else"
%token WHILE "while"
%token RETURN "return"
%token TRUE "true"
%token FALSE "false"
%token VOID "void"
%token NOT "!"  
%token SUB "-"
%token ADD "+"
%token DIV "/"
%token MUL "*"
%token OR "||"
%token AND "&&"
%token EQ "=="  
%token NOT_EQ "!="  
%token LESS "<"  
%token GREATER ">"  
%token LESS_OR_EQ "<="  
%token GREATER_OR_EQ ">="  

%type <P> P
%type <DECL> Decl
%type <S> S
%type <STMT> Stmt
%type <FL> FormalList, Fl
%type <T> Type
%type <E> Expr, F, Eone, Etwo, Ethree, Efour, Efive, Esix
%type <EL> ExprList

%%

Start : P { program = new Program($1); }
        ;    

P : P Decl  { $$ = $1; $$.Add($2); }
  |         { $$ = new List<Declaration>(); }
  ;                                      

Decl : Type ID "(" FormalList ")" "{" S "}"       { $$ = new Declaration($1, $2, $4, $7); $$.SetLocation(@1); }
     | "void" ID "(" FormalList ")" "{" S "}"     { $$ = new Declaration(Type_.NULL, $2, $4, $7); $$.SetLocation(@1); }
	   ;

S : S Stmt    { $$ = $1; $$.Add($2); }
  |           { $$ = new List<Statement>(); }
  ;   

FormalList : Type ID Fl   { $$ = $3; $$.Insert(0, new Parameter($1, $2)); }
		       |              { $$ = new List<Parameter>(); }
           ; 

Fl : "," Type ID Fl { $$ = $4; $$.Insert(0, new Parameter($2, $3));} 
   |                { $$ = new List<Parameter>(); }
   ; 

Type : "int"  { $$ =  Type_.INT;  }
     | "bool" { $$ =  Type_.BOOL; }
     ; 

Stmt  :  "{" S "}"                              { $$ = new BlockStatement($2); $$.SetLocation(@1); }
      |  "if" "(" Expr ")" Stmt                 { $$ = new IfStatement($3, $5, null); $$.SetLocation(@1); }
      |  "if" "(" Expr ")" Stmt "else" Stmt     { $$ = new IfStatement($3, $5, $7); $$.SetLocation(@1); }
      |  "while" "(" Expr ")" Stmt              { $$ = new WhileStatement($3, $5); $$.SetLocation(@1); }
      |  "return" Expr ";"                      { $$ = new ReturnStatement($2); $$.SetLocation(@1); }
      |  "return" ";"                           { $$ = new ReturnStatement(null); $$.SetLocation(@1); }
      |  Expr ";"                               { $$ = new ExpressionStatement($1); $$.SetLocation(@1); }
      |  Type ID ";"                            { $$ = new DeclareStatement($1, $2); $$.SetLocation(@1); }
	    ;

Expr : ID "=" Expr				{ $$ = new AssignmentExpression($1, $3); $$.SetLocation(@1); }								
     | Eone
     ;

Eone : Eone "||" Etwo { $$ = new BopExpression($1, BopExpression.Operator.OR, $3); }
     | Etwo
     ;
     

Etwo : Etwo "&&" Ethree { $$ = new BopExpression($1, BopExpression.Operator.AND, $3); }					
		 | Ethree
		 ;


Ethree : Ethree "==" Efour { $$ = new BopExpression($1, BopExpression.Operator.EQ, $3); }
			 | Ethree "!=" Efour { $$ = new BopExpression($1, BopExpression.Operator.NOT_EQ, $3); }
			 | Efour
			 ;

Efour : Efour "<"  Efive { $$ = new BopExpression($1, BopExpression.Operator.LESS, $3); }							
		  | Efour ">"  Efive { $$ = new BopExpression($1, BopExpression.Operator.GREATER, $3); }					
		  | Efour "<=" Efive { $$ = new BopExpression($1, BopExpression.Operator.LESS_OR_EQ, $3); }		
		  | Efour ">=" Efive { $$ = new BopExpression($1, BopExpression.Operator.GREATER_OR_EQ, $3); }	
		  | Efive
			;

Efive : Efive "+" Esix	{ $$ = new BopExpression($1, BopExpression.Operator.ADD, $3); }
      | Efive "-" Esix	{ $$ = new BopExpression($1, BopExpression.Operator.SUB, $3); }
		  | Esix
		  ;

Esix : Esix "/" F  { $$ = new BopExpression($1, BopExpression.Operator.DIV, $3); }							
		 | Esix "*" F  { $$ = new BopExpression($1, BopExpression.Operator.MUL, $3); }		
     | F
		 ;

F : NUM						  { $$ = new NumberExpression($1); $$.SetLocation(@1); }
  | "true"					{ $$ = new BoolExpression(true); $$.SetLocation(@1); }
  | "false"					{ $$ = new BoolExpression(false); $$.SetLocation(@1); }
  | ID						  { $$ = new IdentifierExpression($1); $$.SetLocation(@1); }
  | "!" F					        { $$ = new UopExpression($2, UopExpression.Operator.NOT); $$.SetLocation(@1); }
  | "-" F					{ $$ = new UopExpression($2, UopExpression.Operator.NEG); $$.SetLocation(@1); }
  | ID "(" ExprList ")"		{ $$ = new LetExpression($1, $3); $$.SetLocation(@1); }
  | "(" Expr ")"			{ $$ = $2; $$.SetLocation(@1); }
  ;

ExprList : Expr                 { $$ = new List<Expression>(); $$.Add($1); }
         | ExprList "," Expr		{ $$ = $1; $$.Add($3); }
         |                      { $$ = new List<Expression>(); }
         ;

%%

public Parser( Scanner s ) : base( s ) { }
public Program program; 