%option out:Generated/Lexer.cs
%namespace Parser
%using QUT.Gppg;

alpha   [a-zA-Z_]
digit   [0-9]

PLUS	(\+)
MINUS	(\-)
MULT	(\*)
DIV		(\/)
OR		(\|\|)
AND		(&&)
EQ		(==)
NOTEQ	(!=)
LT		(<)
GT		(>)
LTEQ	(<=)
GTEQ	(>=)    //Nää, skriv in dem tillsammans med strängarna nedan, det finns ingen vvinst med att köra macro på dem.
//NOT		[!]


alphanum    {alpha}|{digit}



%%

" "|\r|\n|\r\n	{ }
//.*$			{ }



"void"			{ return (int) Tokens.VOID; }
"("				{ return (int) Tokens.LPAR; }
")"				{ return (int) Tokens.RPAR; }
"{"				{ return (int) Tokens.LCUR; }
"}"				{ return (int) Tokens.RCUR; }
","				{ return (int) Tokens.COM; }
"int"			{ return (int) Tokens.INT; }
"bool"			{ return (int) Tokens.BOOL; }
"if"			{ return (int) Tokens.IF; }
"else"			{ return (int) Tokens.ELSE; }
"while"			{ return (int) Tokens.WHILE; }
";"				{ return (int) Tokens.SEMI; }
"true"			{ return (int) Tokens.TRUE; }
"false"			{ return (int) Tokens.FALSE; }
"return"		{ return (int) Tokens.RETURN; }
"="				{ return (int) Tokens.ASN; }
"!"				{ return (int) Tokens.UOP; }
//TODO: put them in the correct order -ORDER MATTERS!! (precedence)
"\+"	{ return (int) Tokens.BOP; }
"\-"	{ return (int) Tokens.BOP; }
"\*"	{ return (int) Tokens.BOP; }
"\/"	{ return (int) Tokens.BOP; }
"\|\|"	{ return (int) Tokens.BOP; }
"&&"	{ return (int) Tokens.BOP; }
"=="	{ return (int) Tokens.BOP; }
"!="	{ return (int) Tokens.BOP; }
"<"		{ return (int) Tokens.BOP; }
">"		{ return (int) Tokens.BOP; }
"<="	{ return (int) Tokens.BOP; }
">="	{ return (int) Tokens.BOP; }


{alpha}{alphanum}*      { yylval.value = yytext;
                          return (int) Tokens.ID; 
                        }
                        
{digit}+                { yylval.value = yytext;
                          return (int) Tokens.NUM; 
                        }


                        
.                       { yylval.value = yytext;
                          return (int) Tokens.ERR;
                        }
                        
    {
      yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol);
    }
	

%% 

override public void yyerror(string msg, object[] arg) {
  Console.WriteLine("{0} on line {1} column {2}", msg, yylloc.StartLine, yylloc.StartColumn);
}
