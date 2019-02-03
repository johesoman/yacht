%option out:Generated/Lexer.cs
%namespace Parser
%using QUT.Gppg;

alpha   [a-zA-Z]
digit   [0-9]
alphanum    {alpha}|{digit}

%%

" "|\r|\n       { }
//.*$           { }

"if"         	{ return (int) Tokens.IF; }
"else"          { return (int) Tokens.ELSE; }
"while"         { return (int) Tokens.WHILE; }
"return"        { return (int) Tokens.RETURN; }
"int"           { return (int) Tokens.INTEGER; }
"bool"          { return (int) Tokens.BOOLEAN; }
"void"          { return (int) Tokens.VOID; }
"true"          { return (int) Tokens.TRUE; }
"false"         { return (int) Tokens.FALSE; }

";"             { return (int) Tokens.SEMI; }
"("             { return (int) Tokens.LPAR; }
")"             { return (int) Tokens.RPAR; }
","             { return (int) Tokens.COMMA; }
"{"             { return (int) Tokens.LCUR; }
"}"             { return (int) Tokens.RCUR; }


"/"				{ return (int) Tokens.DIV; }
"-"             { return (int) Tokens.SUB; }
"*"             { return (int) Tokens.MUL; }
"+"             { return (int) Tokens.PLUS; }
"="             { return (int) Tokens.ASN; }

"=="            { return (int) Tokens.EQUALS; }
"&&"            { return (int) Tokens.AND; }
"!="            { return (int) Tokens.NOTEQ; }
"<"             { return (int) Tokens.LESSTHN; }
">"             { return (int) Tokens.GRTTHN; }
"<="            { return (int) Tokens.LSOREQ; }
">="            { return (int) Tokens.GROREQ; }
"!"             { return (int) Tokens.NOT; }
"||"             { return (int) Tokens.OR; }


{alpha}{alphanum}*      { yylval.value = yytext;
                          return (int) Tokens.ID; 
                        }
                        
{digit}+                { yylval.value = yytext;
                          return (int) Tokens.INT; 
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