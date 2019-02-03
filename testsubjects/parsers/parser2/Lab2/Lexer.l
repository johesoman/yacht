%option out:Generated/Lexer.cs
%namespace Parser
%using QUT.Gppg;

alpha [a-zA-Z] 
digit [0-9]
alphanum {alpha}|{digit}

%%

" "|\r|\n|\t      { }
\/\/.*$           { }

";"         { return (int) Tokens.SEMI; }
"="         { return (int) Tokens.ASN; }
"("         { return (int) Tokens.LPAR; }
")"         { return (int) Tokens.RPAR; }
","         { return (int) Tokens.COMMA; }
"{"         { return (int) Tokens.LBRA; }
"}"         { return (int) Tokens.RBRA; }
"int"       { return (int) Tokens.INT; }
"bool"      { return (int) Tokens.BOOL; }
"if"        { return (int) Tokens.IF; }
"else"      { return (int) Tokens.ELSE; }
"while"     { return (int) Tokens.WHILE; }
"return"    { return (int) Tokens.RETURN; }
"true"      { return (int) Tokens.TRUE; }
"false"     { return (int) Tokens.FALSE; }
"void"      { return (int) Tokens.VOID; }

"-"       { return (int) Tokens.SUB; }
"!"       { return (int) Tokens.NOT; }

"+"				{ return (int) Tokens.ADD; }
"/"				{ return (int) Tokens.DIV; }
"*"				{ return (int) Tokens.MUL; }
"||"			{ return (int) Tokens.OR; }
"&&"			{ return (int) Tokens.AND; }
"=="  		{ return (int) Tokens.EQ; }
"!="  		{ return (int) Tokens.NOT_EQ; }
"<"  			{ return (int) Tokens.LESS; }
">"  			{ return (int) Tokens.GREATER; }
"<="  		{ return (int) Tokens.LESS_OR_EQ; }
">="  		{ return (int) Tokens.GREATER_OR_EQ; }

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