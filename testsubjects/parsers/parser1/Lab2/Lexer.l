%option out:Generated/Lexer.cs
%namespace Parser
%using QUT.Gppg;

alpha   [a-zA-Z_]
digit   [0-9]
alphanum    {alpha}|{digit}

%%

" "|\r|\n       { }
//.*$           { }

"if"             { return (int) Tokens.IF; }
"else"             { return (int) Tokens.ELSE; }
"while"             { return (int) Tokens.WHILE; }
"return"             { return (int) Tokens.RETURN; }
"int"             { return (int) Tokens.INT; }
"bool"             { return (int) Tokens.BOOL; }
"void"             { return (int) Tokens.VOID; }
"{"             { return (int) Tokens.LCUR; }
"}"             { return (int) Tokens.RCUR; }
"("             { return (int) Tokens.LPAR; }
")"             { return (int) Tokens.RPAR; }
","             { return (int) Tokens.COMMA; }
";"             { return (int) Tokens.SEMI; }
"="				{ return (int) Tokens.ASN; }
"||"			{ return (int) Tokens.OR; }
"&&"			{ return (int) Tokens.AND; }
"!="			{ return (int) Tokens.NOTEQUAL; }
"=="			{ return (int) Tokens.EQUAL; }
"<"				{ return (int) Tokens.LESS; }
">"				{ return (int) Tokens.GREAT; }
"<="			{ return (int) Tokens.LESSEQUAL; }
">="			{ return (int) Tokens.GREATEQUAL; }
"!"				{ return (int) Tokens.NOT; }
"+"             { return (int) Tokens.ADD; }
"-"             { return (int) Tokens.SUB; }
"*"             { return (int) Tokens.MUL; }
"/"             { return (int) Tokens.DIV; }
"true"			{ return (int) Tokens.TRUE; }
"false"			{ return (int) Tokens.FALSE; }


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