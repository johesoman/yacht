%output=Generated/Parser.cs
%namespace Parser

%union {
  public string value;
  
  public Declaration D;
  public List<Declaration> Ld;
  public Formal F;
  public List<Formal> Lf;
  public List<Statement> Ls;
  public Statement S;
  public Expression E;
  public List<Expression> Le;
}

%token IF "if"
%token ELSE "else"
%token WHILE "while"
%token RETURN "return"
%token VOID "void"
%token INT "int"
%token BOOLEAN "bool"

%token LCBR "{"
%token RCBR "}"
%token SEMI ";"
%token LPAR "("
%token RPAR ")"
%token COMMA ","
%token ASGN "="

%token OR "||"
%token AND "&&"
%token EQL "=="
%token NEQL "!="
%token GRT ">"
%token LESS "<"
%token GEQL ">="
%token LEQL "<="
%token ADD "+"
%token SUB "-"
%token MUL "*"
%token DIV "/"
%token NOT "!"

%token <value> BOOL
%token <value> ID
%token <value> NUM
%token <value> ERR


%type <D> D
%type <Ld> M
%type <F> F
%type <Lf> J 
%type <S> S
%type <Ls> B
%type <E> E, Z, G, C, H, I, O, R, K
%type <Le> L

%%

P : M EOF							{  Program = new Program($1); } 
  ;

M : M D								{ $$ = $1; $$.Add($2); }
  |									{ $$ = new List<Declaration>(); }
  ;
  
D : "int" ID "(" J ")" "{" B "}"	{ $$ = new FormalDeclaration(FormalDeclaration.IdType.Int, $2, $4, $7); $$.SetLocation(@1); }
  | "bool" ID "(" J ")" "{" B "}"	{ $$ = new FormalDeclaration(FormalDeclaration.IdType.Bool, $2, $4, $7); $$.SetLocation(@1); }
  | "void" ID "(" J ")" "{" B "}"	{ $$ = new FormalDeclaration(FormalDeclaration.IdType.Void, $2, $4, $7); $$.SetLocation(@1); }
  ;

B : B S								{ $$ = $1; $$.Add($2); }
  |									{ $$ = new List<Statement>(); }
  ;

J : F								{ $$ = new List<Formal>(); $$.Add($1); }
  | J "," F							{ $$ = $1; $$.Add($3); }
  |  								{ $$ = new List<Formal>(); }
  ;

F : "int" ID						{ $$ = new Formal(Formal.IdType.Int, $2); $$.SetLocation(@1); }
  | "bool" ID						{ $$ = new Formal(Formal.IdType.Bool, $2); $$.SetLocation(@1); }
  ;

S : "{" B "}"						{ $$ = new BlockStatement($2); $$.SetLocation(@1); }
  | IF "(" E ")" S ELSE S			{ $$ = new IfStatement($3, $5, $7); $$.SetLocation(@1); }
  | IF "(" E ")" S					{ $$ = new IfStatement($3, $5); $$.SetLocation(@1); }
  | WHILE "(" E ")" S				{ $$ = new WhileStatement($3, $5); $$.SetLocation(@1); }
  | RETURN E ";"					{ $$ = new ReturnStatement($2); $$.SetLocation(@1); }
  | RETURN ";"						{ $$ = new ReturnStatement(); $$.SetLocation(@1); }
  | E ";"							{ $$ = new ExpressionStatement($1); $$.SetLocation(@1); }
  | F ";"							{ $$ = new FormalStatement($1); $$.SetLocation(@1); }
  ;

E : ID "=" E 						{ $$ = new AssignmentExpression($1, $3); $$.SetLocation(@1); }
  | Z								{ $$ = $1; }
  ;
  
Z : Z "||" G						{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.Or, $1, $3); $$.SetLocation(@1); }
  | G								{ $$ = $1; }
  ;
  
G : G "&&" C						{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.And, $1, $3); $$.SetLocation(@1); }
  | C								{ $$ = $1; }
  ;
  
C : C "==" H						{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.Eql, $1, $3); $$.SetLocation(@1); }
  | C "!=" H						{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.NEql, $1, $3); $$.SetLocation(@1); }
  | H								{ $$ = $1; }
  ;

H : H "<" I							{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.Less, $1, $3); $$.SetLocation(@1); }
  | H ">" I							{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.Grt, $1, $3); $$.SetLocation(@1); }
  | H "<=" I						{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.LEql, $1, $3); $$.SetLocation(@1); }
  | H ">=" I						{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.GEql, $1, $3); $$.SetLocation(@1); }
  | I								{ $$ = $1; }
  ;

I : I "+" O 						{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.Add, $1, $3); $$.SetLocation(@1); }
  | I "-" O							{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.Sub, $1, $3); $$.SetLocation(@1); }
  | O								{ $$ = $1; }
  ;

O : O "*" R							{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.Mul, $1, $3); $$.SetLocation(@1); }
  | O "/" R							{ $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.Div, $1, $3); $$.SetLocation(@1); }
  | R								{ $$ = $1; }
  ;

R : "!" R							{ $$ = new UnaryOperatorExpression(UnaryOperatorExpression.Type.Not, $2); $$.SetLocation(@1); }
  | "-" R							{ $$ = new UnaryOperatorExpression(UnaryOperatorExpression.Type.Neg, $2); $$.SetLocation(@1); }
  | K								{ $$ = $1; }
  ;
  
K : NUM								{ $$ = new NumberExpression($1); $$.SetLocation(@1); }
  | BOOL							{ $$ = new BooleanExpression($1); $$.SetLocation(@1); }
  | ID "(" L ")"					{ $$ = new FunctionCallExpression($1, $3); $$.SetLocation(@1); }
  | ID								{ $$ = new IdentifierExpression($1); $$.SetLocation(@1); }
  | "(" E ")"						{ $$ = $2; }
  ;
  
L : E								{ $$ = new List<Expression>(); $$.Add($1); }
  | L "," E							{ $$ = $1; $$.Add($3); }
  |									{ $$ = new List<Expression>(); }
  ;
  
%%

public Parser(Scanner s) : base(s) { }
public Program Program; 
