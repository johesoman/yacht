%output=Generated/Parser.cs
%namespace Parser

%union {
  public string value;
  
  public Declaration D;
  public Expression E;
  public Statement S;
  public Argument A;
  public List<Declaration> Dd;
  public List<Expression> L;
  public List<Statement> Ss;
  public List<Argument> Aa;
  public bool B;
  public Type T;
}

%token <value> ID
%token <value> NUM
%token <value> ERR

%token IF "if"
%token ELSE "else"
%token VOID "void"
%token WHILE "while"
%token RETURN "return"
%token INT "int"
%token BOOL "bool"
%token LPAR "("
%token RPAR ")"
%token LCUR "{"
%token RCUR "}"
%token SEMI ";"
%token COMMA ","
%token ASN "="
%token OR "||"
%token AND "&&"
%token NOTEQUAL "!="
%token EQUAL "=="
%token LESS "<"
%token GREAT ">"
%token LESSEQUAL "<="
%token GREATEQUAL ">="
%token NOT "!"
%token ADD "+"
%token SUB "-"
%token MUL "*"
%token DIV "/"
%token TRUE "true"
%token FALSE "false"

%type <Dd> Declarations
%type <D> Declaration
%type <Ss> Statements
%type <S> Statement
%type <E> Expression F FunctionCall Assignment Operation Logical Equality Comparison Addition Multiplication Unary
%type <L> ExpressionList Expressions
%type <Aa> Arguments ArgumentList
%type <A> Argument
%type <B> BooleanLiteral
%type <T> Type

%%

Program : Declarations EOF                                                      { Program = $1; }
        ;

Declaration : Type ID "(" ArgumentList ")" "{" Statements "}"                   { $$ = new FunctionDeclaration($1, $2, $4, $7); $$.SetLocation(@1); }
            | "void" ID "(" ArgumentList ")" "{" Statements "}"                 { $$ = new FunctionDeclaration(Type.VOID, $2, $4, $7); $$.SetLocation(@1);}
            ;

Declarations : Declaration Declarations                                         { $$ = $2; $$.Add($1); }
             |                                                                  { $$ = new List<Declaration>();}
             ;

ArgumentList : Argument Arguments                                               { $$ = $2; $$.Add($1); }                                             
             |
             ;

Arguments  : "," Argument Arguments                                             { $$ = $3; $$.Add($2); }
           |                                                                    { $$ = new List<Argument>(); }
           ;

Argument : Type ID                                                              { $$ = new Argument($1, $2); $$.SetLocation(@1); }
         ;

Type : "int"                                                                    { $$ = Type.INT; }
     | "bool"                                                                   { $$ = Type.BOOL; }
     ;


Operation : Logical                                                             { $$ = $1; }
    ;
    
Logical : Equality "||" Logical                                                 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.OR, $1, $3); $$.SetLocation(@1); }
     | Equality "&&" Logical                                                    { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.AND, $1, $3); $$.SetLocation(@1); }
     | Equality                                                                 { $$ = $1; }
     ;

Equality : Comparison "==" Equality                                             { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.EQUALS, $1, $3); $$.SetLocation(@1); }
     | Comparison "!=" Equality                                                 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.NOTEQUALS, $1, $3); $$.SetLocation(@1); }
     | Comparison                                                               { $$ = $1; }
     ;
     
Comparison : Addition "<" Comparison                                            { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.LESS, $1, $3); $$.SetLocation(@1); }
     | Addition ">" Comparison                                                  { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.GREAT, $1, $3); $$.SetLocation(@1);}
     | Addition "<=" Comparison                                                 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.LESSEQUAL, $1, $3); $$.SetLocation(@1);}
     | Addition ">=" Comparison                                                 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.GREATEQUAL, $1, $3); $$.SetLocation(@1); }
     | Addition                                                                 { $$ = $1; }
     ;
     
Addition : Multiplication "+" Addition                                          { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.ADD, $1, $3); $$.SetLocation(@1); }
     | Multiplication "-" Addition                                              { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.SUB, $1, $3); $$.SetLocation(@1); }
     | Multiplication                                                           { $$ = $1; }
     ;
     
Multiplication : Unary "*" Multiplication                                       { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.MUL, $1, $3); $$.SetLocation(@1); }
     | Unary "/" Multiplication                                                 { $$ = new BinaryOperatorExpression(BinaryOperatorExpression.Type.DIV, $1, $3); $$.SetLocation(@1); }
     | Unary                                                                    { $$ = $1; }
     ;

Unary : "!" Unary                                                               { $$ = new UnaryOperatorExpression(UnaryOperatorExpression.Type.NOT, $2); $$.SetLocation(@1); }
    | "-" Unary                                                                 { $$ = new UnaryOperatorExpression(UnaryOperatorExpression.Type.NEG, $2); $$.SetLocation(@1); }
    | F                                                                         { $$ = $1; }
    ;

Statement : "{" Statements "}"                                                  { $$ = new BlockStatement($2); $$.SetLocation(@1); }
          | "if" "(" Expression ")" Statement                                   { $$ = new IfStatement($3, $5); $$.SetLocation(@1); }
          | "if" "(" Expression ")" Statement "else" Statement                  { $$ = new IfStatement($3, $5, $7); $$.SetLocation(@1); }
          | "while" "(" Expression ")" Statement                                { $$ = new WhileStatement($3, $5); $$.SetLocation(@1); }
          | "return" Expression ";"                                             { $$ = new ReturnStatement($2); $$.SetLocation(@1); }
          | "return" ";"                                                        { $$ = new ReturnStatement(null); $$.SetLocation(@1); }
          | Expression ";"                                                      { $$ = new ExpressionStatement($1); $$.SetLocation(@1); }
          | Type ID ";"                                                         { $$ = new VariableDeclaration($1, $2); $$.SetLocation(@1); }
          ;

Statements : Statement Statements                                               { $$ = $2; $$.Add($1); }
           |                                                                    { $$ = new List<Statement>();}
           ;

ExpressionList : Expression Expressions                                         { $$ = $2; $$.Add($1); }
               |                                                                
               ;

Expressions : "," Expression Expressions                                        { $$ = $3; $$.Add($2); }
            |                                                                   { $$ = new List<Expression>(); }
            ;

Expression : Operation                                                          { $$ = $1; }
           | Assignment                                                         { $$ = $1; }
       ;

F : NUM                                                                         { $$ = new NumberExpression($1); $$.SetLocation(@1); }
  | ID                                                                          { $$ = new IdentifierExpression($1); $$.SetLocation(@1); }
  | BooleanLiteral                                                              { $$ = new BooleanExpression($1); $$.SetLocation(@1); }
  | "(" Expression ")"                                                          { $$ = $2; }
  | FunctionCall                                                                { $$ = $1; }
  ;

FunctionCall : ID "(" ExpressionList ")"                                        { $$ = new FunctionExpression($1, $3); $$.SetLocation(@1); }
             ;

Assignment : ID "=" Expression                                                  { $$ = new AssignmentExpression($1, $3); $$.SetLocation(@1); }
           ;

BooleanLiteral : "true"                                                         { $$ = true; }
               | "false"                                                        { $$ = false; }
               ;


%%
public Parser(Scanner s) : base(s) { }
public List<Declaration> Program; 