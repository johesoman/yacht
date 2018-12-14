namespace Language



module PrettyPrint =



  open Types
  open PPrint.PPrint



  module Uop =
    let pretty =
      function
      | Uop.Not -> chr '!'
      | Uop.Sub -> chr '-'



    let prettyString =
      pretty
      >> render



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
      >> render



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
      >> render



    let prettyPrint =
      prettyString
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
      >> render



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
      >> render



    let prettyPrint =
      prettyString
      >> printfn "%s"



  module Param =
    let pretty (Param (t, n)) =
      sep [Type.pretty t; txt n]



    let prettyString =
      pretty
      >> render



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
      >> render



    let prettyPrint =
      prettyString
      >> printfn "%s"



  module Program =
    let pretty (Program ds) =
      List.map Def.pretty ds
      |> sepLines (empty)



    let prettyString =
      pretty
      >> render



    let prettyPrint =
      prettyString
      >> printfn "%s"
