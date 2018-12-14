namespace Generator



module PrettyPrintBadly =



  open FsCheck
  open PPrint.PPrint
  open GenExtensions
  open Language.Types
  open Language.PrettyPrint
  open Extensions.Extensions



  // +++++++++
  // + Maybe +
  // +++++++++



  type Maybe =
    { misspell : string -> string
    ; forget   : Doc -> Doc
    }



  module Maybe =
    let doWithFreq doFreq dontFreq f x =
      [dontFreq, Gen.constant false; doFreq, Gen.constant true]
      |> Gen.frequency
      |> Gen.sampleOne
      |> function
         | false -> x
         | true  -> f x



    let misspell xs =
      Gen.shuffle xs
      |> Gen.sampleOne
      |> String.ofCharArray



    let forget _ = empty



    let make doFreq dontFreq =
      { misspell = doWithFreq doFreq dontFreq misspell
      ; forget   = doWithFreq doFreq dontFreq forget
      }



  // +++++++++++++++
  // + bad helpers +
  // +++++++++++++++



  let parensIfBad maybe x p =
    if not x then p
    else
      cat [ chr '(' |> maybe.forget
          ; p
          ; chr ')' |> maybe.forget
          ]



  let parensBad maybe p =
    cat [ chr '(' |> maybe.forget
        ; p
        ; chr ')' |> maybe.forget
        ]



  let emptyBracesBad maybe =
    cat [ chr '{' |> maybe.forget
        ; chr '}' |> maybe.forget
        ]



  let sepListBad maybe ps =
    List.intersperse (chr ',') ps
    |> List.map (maybe.forget)
    |> List.chunkBySize 2
    |> List.map cat
    |> sep



  module Type =
    let rec prettyBad maybe =
      function
      | Type.Int  -> txt (maybe.misspell "int")
      | Type.Bool -> txt (maybe.misspell "bool")



    let prettyBadString doFreq dontFreq =
      prettyBad (Maybe.make doFreq dontFreq)
      >> render



    let prettyPrintBadly doFreq dontFreq =
      prettyBadString doFreq dontFreq
      >> printfn "%s"



  module Expr =
    let precedence =
      function
      | Call _           -> 8
      | Var  _           -> 8
      | Int  _           -> 8
      | Bool _           -> 8
      | Uop (Uop.Not, _) -> 7
      | Uop (Uop.Sub, _) -> 7
      | Bop (_, Mul, _)  -> 6
      | Bop (_, Div, _)  -> 6
      | Bop (_, Add, _)  -> 5
      | Bop (_, Sub, _)  -> 5
      | Bop (_, Lt , _)  -> 4
      | Bop (_, Le , _)  -> 4
      | Bop (_, Gt , _)  -> 4
      | Bop (_, Ge , _)  -> 4
      | Bop (_, Eq , _)  -> 3
      | Bop (_, Ne , _)  -> 3
      | Bop (_, And, _)  -> 2
      | Bop (_, Or , _)  -> 1
      | Bop (_, Asn, _)  -> 0



    // pwp = prettyWithPrecedence
    let rec pwp maybe p e =
      let p2 = precedence e

      parensIfBad maybe (p2 < p) <|
      match e with
      | Var  n -> maybe.forget (txt n)
      | Int  x -> sprintf "%d" x |> txt |> maybe.forget

      | Bool true  -> maybe.misspell "true"  |> txt |> maybe.forget
      | Bool false -> maybe.misspell "false" |> txt |> maybe.forget

      | Uop (op, e2) ->
          cat [ Uop.pretty op
              ; pwp maybe p2 e2]

      | Bop (e2, op, e3) ->
          sep [ pwp maybe p2 e2
              ; maybe.forget (Bop.pretty op)
              ; pwp maybe p2 e3
              ]

      | Call (n, es) ->
          cat [ maybe.forget (txt n)
              ; List.map (pwp maybe -1) es
                |> sepListBad maybe
                |> parensBad maybe
              ]



    let prettyBad maybe = pwp maybe -1



    let prettyBadString doFreq dontFreq =
      prettyBad (Maybe.make doFreq dontFreq)
      >> render



    let prettyPrintBadly doFreq dontFreq =
      prettyBadString doFreq dontFreq
      >> printfn "%s"



  module Stmt =
    let rec prettyBadManyNested maybe =
      function
      | [] -> empty
      | ss ->
          let f =
            function
            | If _    as s -> prettyBad maybe s
            | Block _ as s -> prettyBad maybe s
            | While _ as s -> prettyBad maybe s
            | s -> cat [prettyBad maybe s; maybe.forget (chr ';')]

          List.map f ss
          |> lines
          |> nest 2



    and prettyBad maybe =
      function
      | Expr e -> Expr.prettyBad maybe e

      | Decl (t, n) ->
          sep [ Type.prettyBad maybe t
              ; txt n
              ]

      | FuncRet e ->
          sep [ txt (maybe.misspell "return")
              ; Expr.prettyBad maybe e
              ]

      | ProcRet -> txt (maybe.misspell "return")

      | Block [] -> emptyBracesBad maybe
      | Block ss ->
          lines [ maybe.forget (chr '{')
                ; prettyBadManyNested maybe ss
                ; maybe.forget (chr '}')
                ]

      | While (e, ss) ->
          let cond =
            sep [ txt (maybe.misspell "while")
                ; parensBad maybe (Expr.prettyBad maybe e)]

          match ss with
          | [] -> sep [cond; emptyBracesBad maybe]
          | ss ->
              lines [ sep [cond; maybe.forget (chr '{')]
                    ; prettyBadManyNested maybe ss
                    ; maybe.forget (chr '}')
                    ]

      | If (e, ss, ss2) ->
          let cond =
            sep [ txt (maybe.misspell "if")
                ; parens (Expr.pretty e)]

          match ss, ss2 with
          | [], [] -> sep [cond; emptyBracesBad maybe]
          | ss, [] ->
              lines [ sep [cond; maybe.forget (chr '{')]
                    ; prettyBadManyNested maybe ss
                    ; maybe.forget (chr '}')
                    ]

          | ss, ss2 ->
              lines [ sep [cond; maybe.forget (chr '{')]
                    ; prettyBadManyNested maybe ss

                    ; sep [ maybe.forget (chr '}')
                          ; txt (maybe.misspell "else")
                          ; maybe.forget (chr '{')
                          ]

                    ; prettyBadManyNested maybe ss2
                    ; maybe.forget (chr '}')
                    ]



    let prettyBadString doFreq dontFreq =
      prettyBad (Maybe.make doFreq dontFreq)
      >> render



    let prettyPrintBadly doFreq dontFreq =
      prettyBadString doFreq dontFreq
      >> printfn "%s"



  module Param =
    let prettyBad maybe (Param (t, n)) =
      sep [Type.prettyBad maybe t; txt n]



    let prettyBadString doFreq dontFreq =
      prettyBad (Maybe.make doFreq dontFreq)
      >> render



    let PrettyPrintBadly doFreq dontFreq =
      prettyBadString doFreq dontFreq
      >> printfn "%s"



  module Def =
    let prettyBadNameAndParams maybe n ps =
      cat [ maybe.forget (txt n)
          ; List.map (Param.prettyBad maybe) ps
            |> sepListBad maybe
            |> parensBad maybe
          ]



    let prettyBadSigWith maybe ss signature =
          match ss with
          | [] -> sep [signature; emptyBracesBad maybe]
          | ss ->
              lines [ sep [signature; maybe.forget (chr '{')]
                    ; Stmt.prettyBadManyNested maybe ss
                    ; maybe.forget (chr '}')
                    ]



    let prettyBad maybe =
      function
      | Proc (n, ps, ss) ->
          [ maybe.misspell "void" |> txt |> maybe.forget
          ; prettyBadNameAndParams maybe n ps
          ]
          |> sep
          |> prettyBadSigWith maybe ss

      | Func (t, n, ps, ss) ->
          sep [Type.prettyBad maybe t; prettyBadNameAndParams maybe n ps ]
          |> prettyBadSigWith maybe ss



    let prettyBadString doFreq dontFreq =
      prettyBad (Maybe.make doFreq dontFreq)
      >> render



    let prettyPrintBadly doFreq dontFreq =
      prettyBadString doFreq dontFreq
      >> printfn "%s"



  module Program =
    let prettyBad maybe (Program ds) =
      List.map (Def.prettyBad maybe) ds
      |> sepLines (empty)



    let prettyBadString doFreq dontFreq =
      prettyBad (Maybe.make doFreq dontFreq)
      >> render



    let prettyPrintBadly doFreq dontFreq =
      prettyBadString doFreq dontFreq
      >> printfn "%s"




// +++++++++++
// + testing +
// +++++++++++
  open Generator.Generate



  let testExpr doFreq dontFreq n =
    Gen.sampleOne (Expr.generate n)
    |> Expr.prettyPrintBadly doFreq dontFreq



  let testStmt doFreq dontFreq n =
    Gen.sampleOne (Stmt.generate n)
    |> Stmt.prettyPrintBadly doFreq dontFreq



  let testDef doFreq dontFreq n =
    Gen.sampleOne (Def.generate n)
    |> Def.prettyPrintBadly doFreq dontFreq



  let testProgram doFreq dontFreq n =
    Gen.sampleOne (Program.generate n)
    |> Program.prettyPrintBadly doFreq dontFreq
