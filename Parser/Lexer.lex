%namespace Parser
%using QUT.Gppg; 

%option out:Generated/Lexer.cs

alpha [a-zA-Z]
digit [0-9]
alphanum {alpha}|{digit}

%%

" "|\r|\n	        { }
//.*$				{ }
\"[^\"\n]*\"		{ }

"if"  			    { return (int) Tokens.IF; }
"else"              { return (int) Tokens.ELSE; }
"while"             { return (int) Tokens.WHILE; }
"return"            { return (int) Tokens.RETURN; }

"int"               { return (int) Tokens.INTTYPE; }
"bool"              { return (int) Tokens.BOOLTYPE; }
"void"              { return (int) Tokens.VOIDTYPE; }

"("					{ return (int) Tokens.LPAR; }
")"					{ return (int) Tokens.RPAR; }
"{"					{ return (int) Tokens.LCUR; }
"}"					{ return (int) Tokens.RCUR; }
";"					{ return (int) Tokens.SEMI; }
","					{ return (int) Tokens.COMMA; }

"||"				{ return (int) Tokens.OR; }
"&&"				{ return (int) Tokens.AND; }

"=="				{ return (int) Tokens.EQ; }
"!="				{ return (int) Tokens.NEQ; }
">="				{ return (int) Tokens.GEQ; }
"<="				{ return (int) Tokens.LEQ; }

"<"		 	    	{ return (int) Tokens.LT; }
">"					{ return (int) Tokens.GT; }

"="					{ return (int) Tokens.ASN; }

"+"					{ return (int) Tokens.ADD; }
"-"					{ return (int) Tokens.SUB; }
"*"					{ return (int) Tokens.MUL; }
"/"					{ return (int) Tokens.DIV; }

"!"					{ return (int) Tokens.NOT; }


"true"				{ yylval.value = yytext;
                      return (int) Tokens.BOOL; }

"false"				{ yylval.value = yytext;
                      return (int) Tokens.BOOL; }

{alpha}{alphanum}*  { yylval.value = yytext;
                      return (int) Tokens.ID; }

{digit}+            { yylval.value = yytext;
                      return (int) Tokens.INT; }

.                   { yylval.value = yytext;
                      return (int) Tokens.ERR; }

%{
yylloc = new LexLocation(tokLin,tokCol,tokELin,tokECol); 
%}

%%

override public void yyerror(string msg, object[] args) { 
  throw new ParseError(msg, yylloc.StartLine, yylloc.StartColumn);
}
