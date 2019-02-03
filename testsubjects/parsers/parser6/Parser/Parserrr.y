%token ID "id"
%token VOID "void"
%token LPAR "("
%token RPAR ")"
%token LCUR "{"
%token RCUR "}"
%token COM ","
%token INT "int"
%token BOOL "bool"
%token IF "if"
%token ELSE "else"
%token WHILE "while"
%token SEMI ";"
%token BOP "bop"
%token UOP "uop"
%token TRUE "true"
%token FALSE "false"
%token RETURN "return"
%token CLEE "*"
%token EQ "="
%%
Program : Decl CLEE
        ;

Decl : Type ID LPAR FormalList RPAR LCUR Stmt CLEE RCUR
     | VOID ID LPAR FormalList RPAR LCUR Stmt CLEE RCUR
	 ;

FormalList : Type ID LPAR COM Type ID RPAR CLEE 
             |
			 ;

Type : INT 
     | BOOL
	 ;

Stmt : LCUR Stmt CLEE RCUR
     |  IF LPAR Expr RPAR Stmt
     |  IF LPAR Expr RPAR Stmt ELSE Stmt
     |  WHILE LPAR Expr RPAR Stmt
     |  RETURN Expr SEMI
     |  RETURN SEMI
     |  Expr SEMI
     |  Type ID SEMI
	 ;

Expr : INT
     |  TRUE
     |  FALSE
     |  Expr BOP Expr
     |  ID EQ Expr
     |  ID
     |  UOP Expr
     |  ID LPAR ExprList RPAR
     |  LPAR Expr RPAR
	 ;

ExprList : Expr LPAR COM Expr RPAR CLEE 
         |
		 ;
%%
