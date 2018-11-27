namespace Generator



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



  module Type =
    let rec pretty =
      function
      | Type.Int  -> txt "int"
      | Type.Bool -> txt "bool"



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
      | Var (s)    -> txt s
      | Bool false -> txt "false"
      | Bool true  -> txt "true"
      | Int x      -> txt (sprintf "%d" x)

      | Uop (op, e2) ->
          cat [ Uop.pretty op
              ; pwp p2 e2]

      | Bop (e2, op, e3) ->
          sep [ pwp p2 e2
              ; Bop.pretty op
              ; pwp p2 e3
              ]

      | Call (s, es) ->
          cat [ txt s
              ; chr '('
              ; sepList (chr ',') (List.map (pwp -1) es)
              ; chr ')'
              ]



    let pretty = pwp -1



    let prettyString =
      pretty
      >> PPrint.render



// ++++++++
// + test +
// ++++++++



  let testExpr =
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



  let allTests =
    testList "pretty"
      [
        testExpr
      ]



  let test () = runTests defaultConfig allTests
