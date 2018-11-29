namespace Language



module PrettyPrint =



  open Types
  open PPrint
  open System
  open Expecto
  open Extensions



  module Uop =
    let pretty =
      function
      | Not -> chr '!'



    let prettyString =
      pretty
      >> PPrint.render



    let prettyPrint =
      prettyString
      >> printfn "%s"



  module Bop =
    let pretty =
      function
      | Add -> chr '+'
      | Sub -> chr '-'
      | Mul -> chr '*'
      | Div -> chr '/'
      | Eq  -> txt "=="
      | Ne  -> txt "!="
      | Lt  -> chr '<'
      | Gt  -> chr '>'
      | Le  -> txt "<="
      | Ge  -> txt ">="
      | Or  -> txt "||"
      | And -> txt "&&"
      | Asn -> chr '='



    let prettyString =
      pretty
      >> PPrint.render



    let prettyPrint =
      prettyString
      >> printfn "%s"



  module Type =
    let rec pretty =
      function
      | Type.Int  -> txt "int"
      | Type.Bool -> txt "bool"



    let prettyString =
      pretty
      >> PPrint.render



    let prettyPrint =
      prettyString
      >> printfn "%s"



  module Expr =
    let precedence =
      function
      | Call _          -> 8
      | Var  _          -> 8
      | Int  _          -> 8
      | Bool _          -> 8
      | Uop (Not, _)    -> 7
      | Bop (_, Mul, _) -> 6
      | Bop (_, Div, _) -> 6
      | Bop (_, Add, _) -> 5
      | Bop (_, Sub, _) -> 5
      | Bop (_, Lt , _) -> 4
      | Bop (_, Le , _) -> 4
      | Bop (_, Gt , _) -> 4
      | Bop (_, Ge , _) -> 4
      | Bop (_, Eq , _) -> 3
      | Bop (_, Ne , _) -> 3
      | Bop (_, And, _) -> 2
      | Bop (_, Or , _) -> 1
      | Bop (_, Asn, _) -> 0



    // pwp = prettyWithPrecedence
    let rec pwp p e =
      let p2 = precedence e

      parensIf (p2 < p) <|
      match e with
      | Var  n     -> txt n
      | Bool true  -> txt "true"
      | Bool false -> txt "false"
      | Int  x     -> txt (sprintf "%d" x)

      | Uop (op, e2) ->
          cat [ Uop.pretty op
              ; pwp p2 e2]

      | Bop (e2, op, e3) ->
          sep [ pwp p2 e2
              ; Bop.pretty op
              ; pwp p2 e3
              ]

      | Call (n, es) ->
          cat [ txt n
              ; chr '('
              ; sepList (chr ',') (List.map (pwp -1) es)
              ; chr ')'
              ]



    let pretty = pwp -1



    let prettyString =
      pretty
      >> PPrint.render



    let prettyPrint =
      prettyString
      >> printfn "%s"



  module Stmt =
    let rec prettyManyNested =
      function
      | [] -> empty
      | ss ->
          let f =
            function
            | If _    as s -> pretty s
            | Block _ as s -> pretty s
            | While _ as s -> pretty s
            | s -> pretty s :: [chr ';'] |> cat

          List.map f ss
          |> lines
          |> nest 2



    and pretty =
      function
      | Expr e -> Expr.pretty e

      | Decl (t, n) ->
          sep [ Type.pretty t
              ; txt n
              ]

      | FuncRet e ->
          sep [ txt "return"
              ; Expr.pretty e
              ]

      | ProcRet -> txt "return"

      | Block [] -> txt "{}"
      | Block ss ->
          lines [ chr '{'
                ; prettyManyNested ss
                ; chr '}'
                ]

      | While (e, ss) ->
          let cond = sep [txt "while"; parens (Expr.pretty e)]

          match ss with
          | [] -> sep [cond; txt "{}"]
          | ss ->
              lines [ sep [cond; chr '{']
                    ; prettyManyNested ss
                    ; chr '}'
                    ]

      | If (e, ss, ss2) ->
          let cond = sep [txt "if"; parens (Expr.pretty e)]

          match ss, ss2 with
          | [], [] -> sep [cond; txt "{}"]
          | ss, [] ->
              lines [ sep [cond; chr '{']
                    ; prettyManyNested ss
                    ; chr '}'
                    ]

          | ss, ss2 ->
              lines [ sep [cond; chr '{']
                    ; prettyManyNested ss
                    ; txt "} else {"
                    ; prettyManyNested ss2
                    ; chr '}'
                    ]



    let prettyString =
      pretty
      >> PPrint.render



    let prettyPrint =
      prettyString
      >> printfn "%s"



  module Param =
    let pretty (Param (t, n)) =
      sep [Type.pretty t; txt n]



    let prettyString =
      pretty
      >> PPrint.render



    let prettyPrint =
      prettyString
      >> printfn "%s"



  module Def =
    let prettyNameAndParams n ps =
      cat [ txt n
          ; chr '('
          ; List.map Param.pretty ps |>  sepList (chr ',')
          ; chr ')'
          ]



    let prettySigWith ss signature =
          match ss with
          | [] -> sep [signature; txt "{}"]
          | ss ->
              lines [ sep [signature; chr '{']
                    ; Stmt.prettyManyNested ss
                    ; chr '}'
                    ]



    let pretty =
      function
      | Proc (n, ps, ss) ->
          sep [txt "void"; prettyNameAndParams n ps ]
          |> prettySigWith ss

      | Func (t, n, ps, ss) ->
          sep [Type.pretty t; prettyNameAndParams n ps ]
          |> prettySigWith ss



    let prettyString =
      pretty
      >> PPrint.render



    let prettyPrint =
      prettyString
      >> printfn "%s"



  module Program =
    let pretty (Program ds) =
      List.map Def.pretty ds
      |> sepLines (PPrint.empty)



    let prettyString =
      pretty
      >> PPrint.render



    let prettyPrint =
      prettyString
      >> printfn "%s"



// ++++++++
// + test +
// ++++++++



  let strip = String.removeBy Char.IsWhiteSpace



  let testDefAndProgram () =
    let pr  = Def.prettyString
    let prs = pr >> strip

    let ps = [Param (Type.Int, "x"); Param (Type.Int, "y")]
    let ss = [ Decl (Type.Int, "z")
             ; Expr (Bop (Var "z", Asn, Bop (Var "x", Add, Var "y")))
             ]

    let d  = Func (Type.Int, "f", ps, ss @ [FuncRet (Var "z")])
    let d2 = Proc ("g", ps, ss @ [Expr (Call ("print", [Var "z"])); ProcRet])

    let defs =
      testList "defs"
        [ test "1" {Expect.equal (prs d )
                      "intf(intx,inty){intz;z=x+y;returnz;}" ""}
        ; test "2" {Expect.equal (prs d2)
                      "voidg(intx,inty){intz;z=x+y;print(z);return;}" ""}
        ]

    let p = Program.prettyString (Program [d; d2])

    let programs =
      testList "programs"
        [ test "1" {Expect.equal (strip p )
                      "intf(intx,inty){intz;z=x+y;returnz;}\
                      voidg(intx,inty){intz;z=x+y;print(z);return;}" ""}
        ]

    testList "all"
      [ defs
      ; programs
      ]



  let testStmt () =
    let pr  = Stmt.prettyString
    let prs = pr >> strip

    let b  = pr <| Block [Expr (Int 1); Block [Block []]; Expr (Int 2)]
    let b2 = pr <| Block [Block [Block [Block [Block []]]]]
    let b3 = pr <| Block [Block []; (Expr (Var "x")); Block []; ProcRet]
    let b4 = pr <| Block [Block []; Block []; Block []; FuncRet (Int 1)]

    let blocks =
      testList "blocks"
        [ test "1" {Expect.equal (strip b ) "{1;{{}}2;}" ""}
        ; test "2" {Expect.equal (strip b2) "{{{{{}}}}}" ""}
        ; test "3" {Expect.equal (strip b3) "{{}x;{}return;}" ""}
        ; test "4" {Expect.equal (strip b4) "{{}{}{}return1;}" ""}
        ]

    let fi  = pr <| If (Int 1, [Block []], [Block []])
    let fi2 = pr <| If (Int 1, [Block []], [])
    let fi3 = pr <| If (Int 1, [], [Block []])
    let fi4 = pr <| If (Int 1, [], [])
    let fi5 = pr <| If (Int 1, [Expr (Int 1); Expr (Int 2)], [])
    let fi6 = pr <| If (Int 1, [], [Expr (Int 1); Expr (Int 2)])
    let fi7 = pr <| If (Int 1, [Block [Expr (Int 1); Expr (Int 2)]], [])

    let ifs =
      testList "ifs"
        [ test "1" {Expect.equal (strip fi ) "if(1){{}}else{{}}" ""}
        ; test "2" {Expect.equal (strip fi2) "if(1){{}}" ""}
        ; test "3" {Expect.equal (strip fi3) "if(1){}else{{}}" ""}
        ; test "4" {Expect.equal (strip fi4) "if(1){}" ""}
        ; test "5" {Expect.equal (strip fi5) "if(1){1;2;}" ""}
        ; test "6" {Expect.equal (strip fi6) "if(1){}else{1;2;}" ""}
        ; test "7" {Expect.equal (strip fi7) "if(1){{1;2;}}" ""}
        ]

    let wh  = pr <| While (Int 1, [])
    let wh2 = pr <| While (Int 1, [Block [Block []]])
    let wh3 = pr <| While (Int 1, [Expr (Int 1); Block []; ProcRet])
    let wh4 = pr <| While (Int 1, [If (Int 1, [ProcRet], [Block [ProcRet]])])

    let whiles =
      testList "whiles"
        [ test "1" {Expect.equal (strip wh ) "while(1){}" ""}
        ; test "2" {Expect.equal (strip wh2) "while(1){{{}}}" ""}
        ; test "3" {Expect.equal (strip wh3) "while(1){1;{}return;}" ""}
        ; test "4" {Expect.equal (strip wh4)
                      "while(1){if(1){return;}else{{return;}}}" ""}
        ]

    let other =
      testList "other"
        [ test "1" {Expect.equal (pr ProcRet) "return" ""}
        ; test "2" {Expect.equal (pr (FuncRet (Int 1))) "return 1" ""}
        ; test "3" {Expect.equal (pr (Expr (Int 1))) "1" ""}
        ; test "4" {Expect.equal (pr (Decl (Type.Int, "x"))) "int x" ""}
        ; test "5" {Expect.equal (pr (Decl (Type.Bool, "y"))) "bool y" ""}
        ]

    testList "stmt" [blocks; ifs; whiles; other]



  let testExpr () =
    let pr       = Expr.prettyString
    let bpr op   = pr (Bop (Int 1, op, Int 2))
    let cpr s es = pr (Call (s, es))

    let leafs =
      testList "leafs"
        [ test "1" {Expect.equal (pr (Var "x")) "x" ""}
        ; test "2" {Expect.equal (pr (Int 1337)) "1337" ""}
        ; test "3" {Expect.equal (pr (Bool false)) "false" ""}
        ; test "4" {Expect.equal (pr (Bool true)) "true" ""}
        ; test "5" {Expect.equal (pr (Bool true)) "true" ""}
        ]

    let bops =
      testList "bops"
        [ test "1"  {Expect.equal (bpr Mul) "1 * 2" ""}
        ; test "2"  {Expect.equal (bpr Div) "1 / 2" ""}
        ; test "3"  {Expect.equal (bpr Add) "1 + 2" ""}
        ; test "4"  {Expect.equal (bpr Sub) "1 - 2" ""}
        ; test "5"  {Expect.equal (bpr Lt)  "1 < 2" ""}
        ; test "6"  {Expect.equal (bpr Le)  "1 <= 2" ""}
        ; test "7"  {Expect.equal (bpr Gt)  "1 > 2" ""}
        ; test "8"  {Expect.equal (bpr Ge)  "1 >= 2" ""}
        ; test "9"  {Expect.equal (bpr Eq)  "1 == 2" ""}
        ; test "10" {Expect.equal (bpr Ne)  "1 != 2" ""}
        ; test "11" {Expect.equal (bpr And) "1 && 2" ""}
        ; test "12" {Expect.equal (bpr Or)  "1 || 2" ""}
        ; test "13" {Expect.equal (bpr Asn) "1 = 2" ""}
        ; test "14" {
                      Expect.equal
                        (Bop (Bop (Int 1, Asn, Int 2),
                              Mul,
                              Bop (Bop (Int 3, Add, Int 4), Div, Int 5))
                         |> pr)
                        "(1 = 2) * (3 + 4) / 5" ""
                    }
        ; test "15" {
                      Expect.equal
                        (Bop (Bop (Bop (Int 1, Asn, Int 2), Add, Int 3),
                              Mul,
                              Int 4)
                         |> pr)
                        "((1 = 2) + 3) * 4" ""
                    }
        ; test "16" {
                      Expect.equal
                        (Bop (Int 1,
                              Mul,
                              Bop (Bop (Int 2, Asn, Int 3), Add, Int 4))
                         |> pr)
                        "1 * ((2 = 3) + 4)" ""
                    }
        ]

    let other =
      testList "other"
        [ test "1" {
                     Expect.equal
                       (cpr "f" [Int 3; Bop (Int 1, Add, Int 2) ])
                        "f(3, 1 + 2)" ""
                   }
        ; test "2" {
                     Expect.equal
                       (cpr "f" [Int 3; Call ("g", [Bop (Int 1, Add, Int 2)])])
                        "f(3, g(1 + 2))" ""
                   }
        ]

    testList "expr" [leafs; bops; other]



  let allTests () =
    testList "pretty"
      [ testExpr ()
      ; testStmt ()
      ; testDefAndProgram ()
      ]



  let test () = runTests defaultConfig (allTests ())
