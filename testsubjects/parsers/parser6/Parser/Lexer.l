%option out:Generated/Lexer.cs
%namespace Parser
%using QUT.Gppg;

alpha   [a-zA-Z_]
digit   [0-9]


alphanum    {alpha}|{digit}
//uOperators		{NOT}



%%

" "|\r|\n|\r\n|\t	{ }
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

"\+"			{ return (int) Tokens.ADD; }
"\-"			{ return (int) Tokens.NEG; }
"\*"			{ return (int) Tokens.MUL; }
"\/"			{ return (int) Tokens.DIV; }
"\|\|"			{ return (int) Tokens.OR; }
"&&"			{ return (int) Tokens.AND; }
"=="			{ return (int) Tokens.EQ; }
"!="			{ return (int) Tokens.NEQ; }
"<="			{ return (int) Tokens.LTE; }
">="			{ return (int) Tokens.GTE; }
"<"				{ return (int) Tokens.LT; }
">"				{ return (int) Tokens.GT; }
"="				{ return (int) Tokens.ASN; }
"!"				{ return (int) Tokens.NOT; }



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
