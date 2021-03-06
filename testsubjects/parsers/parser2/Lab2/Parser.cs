// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, John Gough, QUT 2005-2014
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.5.2
// Machine:  LAPTOP-B7BUAEVP
// DateTime: 2018-12-07 18:06:13
// UserName: Ray
// Input file <Parser.y - 2018-12-07 12:34:17>

// options: lines gplex

using System;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Text;
using QUT.Gppg;

namespace Parser
{
public enum Tokens {error=2,EOF=3,ID=4,NUM=5,ERR=6,
    SEMI=7,ASN=8,LPAR=9,RPAR=10,COMMA=11,LBRA=12,
    RBRA=13,INT=14,BOOL=15,IF=16,ELSE=17,WHILE=18,
    RETURN=19,TRUE=20,FALSE=21,VOID=22,BOP=23,UOP=24};

public struct ValueType
#line 4 "Parser.y"
       {
  public string value;

  public DeclarationList P;
  public Declaration DECL;
  public Statement S;
  public List<Statement> SQ;
  public List<Parameter> FL;
  public Type_ T;

  public Expression E;
  public List<Expression> EL;
}
#line default
// Abstract base class for GPLEX scanners
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

// Utility class for encapsulating token information
[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public class ScanObj {
  public int token;
  public ValueType yylval;
  public LexLocation yylloc;
  public ScanObj( int t, ValueType val, LexLocation loc ) {
    this.token = t; this.yylval = val; this.yylloc = loc;
  }
}

[GeneratedCodeAttribute( "Gardens Point Parser Generator", "1.5.2")]
public class Parser: ShiftReduceParser<ValueType, LexLocation>
{
#pragma warning disable 649
  private static Dictionary<int, string> aliases;
#pragma warning restore 649
  private static Rule[] rules = new Rule[37];
  private static State[] states = new State[80];
  private static string[] nonTerms = new string[] {
      "P", "Decl", "S", "Stmt", "Sq", "FormalList", "Fl", "Type", "Expr", "F", 
      "ExprList", "El", "Program", "$accept", };

  static Parser() {
    states[0] = new State(new int[]{14,63,15,64,22,72,3,-4},new int[]{-13,1,-1,3,-2,4,-8,6});
    states[1] = new State(new int[]{3,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{14,63,15,64,22,72,3,-4},new int[]{-1,5,-2,4,-8,6});
    states[5] = new State(-3);
    states[6] = new State(new int[]{4,7});
    states[7] = new State(new int[]{9,8});
    states[8] = new State(new int[]{14,63,15,64,10,-11},new int[]{-6,9,-8,65});
    states[9] = new State(new int[]{10,10});
    states[10] = new State(new int[]{12,11});
    states[11] = new State(new int[]{12,17,16,20,18,27,19,32,5,37,20,38,21,39,4,40,24,43,9,45,14,63,15,64,13,-9},new int[]{-3,12,-5,14,-4,15,-9,58,-10,48,-8,60});
    states[12] = new State(new int[]{13,13});
    states[13] = new State(-5);
    states[14] = new State(-7);
    states[15] = new State(new int[]{12,17,16,20,18,27,19,32,5,37,20,38,21,39,4,40,24,43,9,45,14,63,15,64,13,-9},new int[]{-5,16,-4,15,-9,58,-10,48,-8,60});
    states[16] = new State(-8);
    states[17] = new State(new int[]{12,17,16,20,18,27,19,32,5,37,20,38,21,39,4,40,24,43,9,45,14,63,15,64,13,-9},new int[]{-3,18,-5,14,-4,15,-9,58,-10,48,-8,60});
    states[18] = new State(new int[]{13,19});
    states[19] = new State(-16);
    states[20] = new State(new int[]{9,21});
    states[21] = new State(new int[]{5,37,20,38,21,39,4,40,24,43,9,45},new int[]{-9,22,-10,48});
    states[22] = new State(new int[]{10,23,23,35});
    states[23] = new State(new int[]{12,17,16,20,18,27,19,32,5,37,20,38,21,39,4,40,24,43,9,45,14,63,15,64},new int[]{-4,24,-9,58,-10,48,-8,60});
    states[24] = new State(new int[]{17,25,12,-17,16,-17,18,-17,19,-17,5,-17,20,-17,21,-17,4,-17,24,-17,9,-17,14,-17,15,-17,13,-17});
    states[25] = new State(new int[]{12,17,16,20,18,27,19,32,5,37,20,38,21,39,4,40,24,43,9,45,14,63,15,64},new int[]{-4,26,-9,58,-10,48,-8,60});
    states[26] = new State(-18);
    states[27] = new State(new int[]{9,28});
    states[28] = new State(new int[]{5,37,20,38,21,39,4,40,24,43,9,45},new int[]{-9,29,-10,48});
    states[29] = new State(new int[]{10,30,23,35});
    states[30] = new State(new int[]{12,17,16,20,18,27,19,32,5,37,20,38,21,39,4,40,24,43,9,45,14,63,15,64},new int[]{-4,31,-9,58,-10,48,-8,60});
    states[31] = new State(-19);
    states[32] = new State(new int[]{7,57,5,37,20,38,21,39,4,40,24,43,9,45},new int[]{-9,33,-10,48});
    states[33] = new State(new int[]{7,34,23,35});
    states[34] = new State(-20);
    states[35] = new State(new int[]{5,37,20,38,21,39,4,40,24,43,9,45},new int[]{-10,36});
    states[36] = new State(-25);
    states[37] = new State(-26);
    states[38] = new State(-27);
    states[39] = new State(-28);
    states[40] = new State(new int[]{8,41,9,49,7,-30,23,-30,10,-30,11,-30});
    states[41] = new State(new int[]{5,37,20,38,21,39,4,40,24,43,9,45},new int[]{-10,42});
    states[42] = new State(-29);
    states[43] = new State(new int[]{5,37,20,38,21,39,4,40,24,43,9,45},new int[]{-10,44});
    states[44] = new State(-31);
    states[45] = new State(new int[]{5,37,20,38,21,39,4,40,24,43,9,45},new int[]{-9,46,-10,48});
    states[46] = new State(new int[]{10,47,23,35});
    states[47] = new State(-33);
    states[48] = new State(-24);
    states[49] = new State(new int[]{5,37,20,38,21,39,4,40,24,43,9,45},new int[]{-11,50,-9,52,-10,48});
    states[50] = new State(new int[]{10,51});
    states[51] = new State(-32);
    states[52] = new State(new int[]{23,35,11,54,10,-36},new int[]{-12,53});
    states[53] = new State(-34);
    states[54] = new State(new int[]{5,37,20,38,21,39,4,40,24,43,9,45},new int[]{-9,55,-10,48});
    states[55] = new State(new int[]{23,35,11,54,10,-36},new int[]{-12,56});
    states[56] = new State(-35);
    states[57] = new State(-21);
    states[58] = new State(new int[]{7,59,23,35});
    states[59] = new State(-22);
    states[60] = new State(new int[]{4,61});
    states[61] = new State(new int[]{7,62});
    states[62] = new State(-23);
    states[63] = new State(-14);
    states[64] = new State(-15);
    states[65] = new State(new int[]{4,66});
    states[66] = new State(new int[]{11,68,10,-13},new int[]{-7,67});
    states[67] = new State(-10);
    states[68] = new State(new int[]{14,63,15,64},new int[]{-8,69});
    states[69] = new State(new int[]{4,70});
    states[70] = new State(new int[]{11,68,10,-13},new int[]{-7,71});
    states[71] = new State(-12);
    states[72] = new State(new int[]{4,73});
    states[73] = new State(new int[]{9,74});
    states[74] = new State(new int[]{14,63,15,64,10,-11},new int[]{-6,75,-8,65});
    states[75] = new State(new int[]{10,76});
    states[76] = new State(new int[]{12,77});
    states[77] = new State(new int[]{12,17,16,20,18,27,19,32,5,37,20,38,21,39,4,40,24,43,9,45,14,63,15,64,13,-9},new int[]{-3,78,-5,14,-4,15,-9,58,-10,48,-8,60});
    states[78] = new State(new int[]{13,79});
    states[79] = new State(-6);

    for (int sNo = 0; sNo < states.Length; sNo++) states[sNo].number = sNo;

    rules[1] = new Rule(-14, new int[]{-13,3});
    rules[2] = new Rule(-13, new int[]{-1});
    rules[3] = new Rule(-1, new int[]{-2,-1});
    rules[4] = new Rule(-1, new int[]{});
    rules[5] = new Rule(-2, new int[]{-8,4,9,-6,10,12,-3,13});
    rules[6] = new Rule(-2, new int[]{22,4,9,-6,10,12,-3,13});
    rules[7] = new Rule(-3, new int[]{-5});
    rules[8] = new Rule(-5, new int[]{-4,-5});
    rules[9] = new Rule(-5, new int[]{});
    rules[10] = new Rule(-6, new int[]{-8,4,-7});
    rules[11] = new Rule(-6, new int[]{});
    rules[12] = new Rule(-7, new int[]{11,-8,4,-7});
    rules[13] = new Rule(-7, new int[]{});
    rules[14] = new Rule(-8, new int[]{14});
    rules[15] = new Rule(-8, new int[]{15});
    rules[16] = new Rule(-4, new int[]{12,-3,13});
    rules[17] = new Rule(-4, new int[]{16,9,-9,10,-4});
    rules[18] = new Rule(-4, new int[]{16,9,-9,10,-4,17,-4});
    rules[19] = new Rule(-4, new int[]{18,9,-9,10,-4});
    rules[20] = new Rule(-4, new int[]{19,-9,7});
    rules[21] = new Rule(-4, new int[]{19,7});
    rules[22] = new Rule(-4, new int[]{-9,7});
    rules[23] = new Rule(-4, new int[]{-8,4,7});
    rules[24] = new Rule(-9, new int[]{-10});
    rules[25] = new Rule(-9, new int[]{-9,23,-10});
    rules[26] = new Rule(-10, new int[]{5});
    rules[27] = new Rule(-10, new int[]{20});
    rules[28] = new Rule(-10, new int[]{21});
    rules[29] = new Rule(-10, new int[]{4,8,-10});
    rules[30] = new Rule(-10, new int[]{4});
    rules[31] = new Rule(-10, new int[]{24,-10});
    rules[32] = new Rule(-10, new int[]{4,9,-11,10});
    rules[33] = new Rule(-10, new int[]{9,-9,10});
    rules[34] = new Rule(-11, new int[]{-9,-12});
    rules[35] = new Rule(-12, new int[]{11,-9,-12});
    rules[36] = new Rule(-12, new int[]{});

    aliases = new Dictionary<int, string>();
    aliases.Add(7, ";");
    aliases.Add(8, "=");
    aliases.Add(9, "(");
    aliases.Add(10, ")");
    aliases.Add(11, ",");
    aliases.Add(12, "{");
    aliases.Add(13, "}");
    aliases.Add(14, "int");
    aliases.Add(15, "bool");
    aliases.Add(16, "if");
    aliases.Add(17, "else");
    aliases.Add(18, "while");
    aliases.Add(19, "return");
    aliases.Add(20, "true");
    aliases.Add(21, "false");
    aliases.Add(22, "void");
    aliases.Add(23, "bop");
    aliases.Add(24, "uop");
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
#pragma warning disable 162, 1522
    switch (action)
    {
      case 2: // Program -> P
#line 52 "Parser.y"
            { Program = ValueStack[ValueStack.Depth-1].P; }
#line default
        break;
      case 3: // P -> Decl, P
#line 55 "Parser.y"
            { ValueStack[ValueStack.Depth-1].P.Add(ValueStack[ValueStack.Depth-2].DECL); CurrentSemanticValue.P = ValueStack[ValueStack.Depth-1].P; CurrentSemanticValue.P.SetLocation(LocationStack[LocationStack.Depth-2]);}
#line default
        break;
      case 4: // P -> /* empty */
#line 56 "Parser.y"
            { CurrentSemanticValue.P = new DeclarationList(); }
#line default
        break;
      case 5: // Decl -> Type, ID, "(", FormalList, ")", "{", S, "}"
#line 59 "Parser.y"
                                                  { CurrentSemanticValue.DECL = new Declaration(ValueStack[ValueStack.Depth-8].T, ValueStack[ValueStack.Depth-7].value, ValueStack[ValueStack.Depth-5].FL, ValueStack[ValueStack.Depth-2].S); CurrentSemanticValue.DECL.SetLocation(LocationStack[LocationStack.Depth-8]); }
#line default
        break;
      case 6: // Decl -> "void", ID, "(", FormalList, ")", "{", S, "}"
#line 60 "Parser.y"
                                                  { CurrentSemanticValue.DECL = new Declaration(Type_.NULL, ValueStack[ValueStack.Depth-7].value, ValueStack[ValueStack.Depth-5].FL, ValueStack[ValueStack.Depth-2].S); CurrentSemanticValue.DECL.SetLocation(LocationStack[LocationStack.Depth-8]); }
#line default
        break;
      case 7: // S -> Sq
#line 63 "Parser.y"
        { CurrentSemanticValue.S = new SequenceStatement(ValueStack[ValueStack.Depth-1].SQ); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-1]); }
#line default
        break;
      case 8: // Sq -> Stmt, Sq
#line 66 "Parser.y"
             { ValueStack[ValueStack.Depth-1].SQ.Add(ValueStack[ValueStack.Depth-2].S); CurrentSemanticValue.SQ = ValueStack[ValueStack.Depth-1].SQ;}
#line default
        break;
      case 9: // Sq -> /* empty */
#line 67 "Parser.y"
       { CurrentSemanticValue.SQ = new List<Statement>(); }
#line default
        break;
      case 10: // FormalList -> Type, ID, Fl
#line 70 "Parser.y"
                          { CurrentSemanticValue.FL = ValueStack[ValueStack.Depth-1].FL; CurrentSemanticValue.FL.Add(new Parameter(ValueStack[ValueStack.Depth-3].T, ValueStack[ValueStack.Depth-2].value)); }
#line default
        break;
      case 11: // FormalList -> /* empty */
#line 71 "Parser.y"
       { CurrentSemanticValue.FL = new List<Parameter>(); }
#line default
        break;
      case 12: // Fl -> ",", Type, ID, Fl
#line 74 "Parser.y"
                    { CurrentSemanticValue.FL = ValueStack[ValueStack.Depth-1].FL; CurrentSemanticValue.FL.Add(new Parameter(ValueStack[ValueStack.Depth-3].T, ValueStack[ValueStack.Depth-2].value));}
#line default
        break;
      case 13: // Fl -> /* empty */
#line 75 "Parser.y"
                    { CurrentSemanticValue.FL = new List<Parameter>(); }
#line default
        break;
      case 14: // Type -> "int"
#line 78 "Parser.y"
              { CurrentSemanticValue.T =  Type_.INT;  }
#line default
        break;
      case 15: // Type -> "bool"
#line 79 "Parser.y"
              { CurrentSemanticValue.T =  Type_.BOOL; }
#line default
        break;
      case 16: // Stmt -> "{", S, "}"
#line 82 "Parser.y"
                                                { CurrentSemanticValue.S = new ScopeStatement(ValueStack[ValueStack.Depth-2].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 17: // Stmt -> "if", "(", Expr, ")", Stmt
#line 83 "Parser.y"
                                                { CurrentSemanticValue.S = new IfStatement(ValueStack[ValueStack.Depth-3].E, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-5]); }
#line default
        break;
      case 18: // Stmt -> "if", "(", Expr, ")", Stmt, "else", Stmt
#line 84 "Parser.y"
                                                { CurrentSemanticValue.S = new IfElseStatement(ValueStack[ValueStack.Depth-5].E, ValueStack[ValueStack.Depth-3].S, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-7]); }
#line default
        break;
      case 19: // Stmt -> "while", "(", Expr, ")", Stmt
#line 85 "Parser.y"
                                                { CurrentSemanticValue.S = new WhileStatement(ValueStack[ValueStack.Depth-3].E, ValueStack[ValueStack.Depth-1].S); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-5]); }
#line default
        break;
      case 20: // Stmt -> "return", Expr, ";"
#line 86 "Parser.y"
                                                { CurrentSemanticValue.S = new ReturnStatement(ValueStack[ValueStack.Depth-2].E); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 21: // Stmt -> "return", ";"
#line 87 "Parser.y"
                                                { CurrentSemanticValue.S = new ReturnStatement(null); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-2]); }
#line default
        break;
      case 22: // Stmt -> Expr, ";"
#line 88 "Parser.y"
                                                { CurrentSemanticValue.S = new ExpressionStatement(ValueStack[ValueStack.Depth-2].E); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-2]); }
#line default
        break;
      case 23: // Stmt -> Type, ID, ";"
#line 89 "Parser.y"
                                                { CurrentSemanticValue.S = new DeclareStatement(ValueStack[ValueStack.Depth-3].T, ValueStack[ValueStack.Depth-2].value); CurrentSemanticValue.S.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 24: // Expr -> F
#line 92 "Parser.y"
                { CurrentSemanticValue.E = ValueStack[ValueStack.Depth-1].E; CurrentSemanticValue.E.SetLocation(LocationStack[LocationStack.Depth-1]); }
#line default
        break;
      case 25: // Expr -> Expr, "bop", F
#line 93 "Parser.y"
                         { CurrentSemanticValue.E = new BopExpression(ValueStack[ValueStack.Depth-3].E, ValueStack[ValueStack.Depth-1].E); CurrentSemanticValue.E.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 26: // F -> NUM
#line 96 "Parser.y"
             { CurrentSemanticValue.E = new NumberExpression(ValueStack[ValueStack.Depth-1].value); CurrentSemanticValue.E.SetLocation(LocationStack[LocationStack.Depth-1]); }
#line default
        break;
      case 27: // F -> "true"
#line 97 "Parser.y"
               { CurrentSemanticValue.E = new BoolExpression(true); CurrentSemanticValue.E.SetLocation(LocationStack[LocationStack.Depth-1]); }
#line default
        break;
      case 28: // F -> "false"
#line 98 "Parser.y"
                { CurrentSemanticValue.E = new BoolExpression(false); CurrentSemanticValue.E.SetLocation(LocationStack[LocationStack.Depth-1]); }
#line default
        break;
      case 29: // F -> ID, "=", F
#line 99 "Parser.y"
                { CurrentSemanticValue.E = new AssignmentExpression(ValueStack[ValueStack.Depth-3].value, ValueStack[ValueStack.Depth-1].E); CurrentSemanticValue.E.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 30: // F -> ID
#line 100 "Parser.y"
            { CurrentSemanticValue.E = new IdentifierExpression(ValueStack[ValueStack.Depth-1].value); CurrentSemanticValue.E.SetLocation(LocationStack[LocationStack.Depth-1]); }
#line default
        break;
      case 31: // F -> "uop", F
#line 101 "Parser.y"
                { CurrentSemanticValue.E = new UopExpression(ValueStack[ValueStack.Depth-1].E); CurrentSemanticValue.E.SetLocation(LocationStack[LocationStack.Depth-2]); }
#line default
        break;
      case 32: // F -> ID, "(", ExprList, ")"
#line 102 "Parser.y"
                         { CurrentSemanticValue.E = new LetExpression(ValueStack[ValueStack.Depth-4].value, ValueStack[ValueStack.Depth-2].EL); CurrentSemanticValue.E.SetLocation(LocationStack[LocationStack.Depth-4]); }
#line default
        break;
      case 33: // F -> "(", Expr, ")"
#line 103 "Parser.y"
                   { CurrentSemanticValue.E = new PsExpression(ValueStack[ValueStack.Depth-2].E); CurrentSemanticValue.E.SetLocation(LocationStack[LocationStack.Depth-3]); }
#line default
        break;
      case 34: // ExprList -> Expr, El
#line 106 "Parser.y"
                     { ValueStack[ValueStack.Depth-1].EL.Add(ValueStack[ValueStack.Depth-2].E); CurrentSemanticValue.EL = ValueStack[ValueStack.Depth-1].EL; }
#line default
        break;
      case 35: // El -> ",", Expr, El
#line 109 "Parser.y"
                   { CurrentSemanticValue.EL = ValueStack[ValueStack.Depth-1].EL; CurrentSemanticValue.EL.Add(ValueStack[ValueStack.Depth-2].E); }
#line default
        break;
      case 36: // El -> /* empty */
#line 110 "Parser.y"
                 { CurrentSemanticValue.EL = new List<Expression>(); }
#line default
        break;
    }
#pragma warning restore 162, 1522
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliases != null && aliases.ContainsKey(terminal))
        return aliases[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }

#line 114 "Parser.y"

public Parser( Scanner s ) : base( s ) { }
public DeclarationList Program; 
#line default
}
}
