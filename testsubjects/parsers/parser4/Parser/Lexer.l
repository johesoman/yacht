%option out:Generated/Lexer.cs
%namespace Parser
%using QUT.Gppg;

alpha   	[a-zA-Z_]
digit   	[0-9]
alphanum    {alpha}|{digit}
bool        true|false
type        int|bool

%%

" "|\r\n|\r|\n|\t	{ }
\/\/.*$         	{ }

"if"			{ return (int) Tokens.IF; }
"else"			{ return (int) Tokens.ELSE; }
"while"			{ return (int) Tokens.WHILE; }
"return"		{ return (int) Tokens.RETURN; }
"void"			{ return (int) Tokens.VOID; }
"int"			{ return (int) Tokens.INT; }
"bool"			{ return (int) Tokens.BOOLEAN; }

"{"				{ return (int) Tokens.LCBR; }
"}"				{ return (int) Tokens.RCBR; }
";"             { return (int) Tokens.SEMI; }
"("             { return (int) Tokens.LPAR; }
")"             { return (int) Tokens.RPAR; }
","             { return (int) Tokens.COMMA; }
"="				{ return (int) Tokens.ASGN; }

"||"			{ return (int) Tokens.OR; }
"&&"			{ return (int) Tokens.AND; }
"=="			{ return (int) Tokens.EQL; }
"!="			{ return (int) Tokens.NEQL; }
">"				{ return (int) Tokens.GRT; }
"<"				{ return (int) Tokens.LESS; }
">="			{ return (int) Tokens.GEQL; }
"<="			{ return (int) Tokens.LEQL; }
"+"             { return (int) Tokens.ADD; }
"-"             { return (int) Tokens.SUB; }
"*"             { return (int) Tokens.MUL; }
"/"             { return (int) Tokens.DIV; }
"!"				{ return (int) Tokens.NOT; }

{bool}					{ yylval.value = yytext;
                          return (int) Tokens.BOOL; 
                        }

{alpha}{alphanum}*      { yylval.value = yytext;
                          return (int) Tokens.ID; 
                        }
                        
{digit}+                { yylval.value = yytext;
                          return (int) Tokens.NUM; 
                        }
                        
.                       { yylval.value = yytext;
                          return (int) Tokens.ERR;
                        }
                        
%{
      yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol);
%}

%% 

override public void yyerror(string msg, object[] arg) {
  Console.WriteLine("{0} on line {1} column {2}", msg, yylloc.StartLine, yylloc.StartColumn);
}