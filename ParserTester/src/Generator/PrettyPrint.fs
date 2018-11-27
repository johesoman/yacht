namespace Generator



module PrettyPrint =



  open Types
  open PPrint
  open System
  open Expecto



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
      | Var (Name s) -> txt s
      | Bool false   -> txt "false"
      | Bool true    -> txt "true"
      | Int x        -> txt (sprintf "%d" x)



    let pretty = pwp -1



    let prettyString =
      pretty
      >> PPrint.render



  let testExpr =
    let pr = Expr.prettyString

    testList "expr"
      [ test "1" {Expect.equal (pr (Var (Name "x"))) "x" ""}
      ; test "2" {Expect.equal (pr (Int 1337)) "1337" ""}
      ; test "3" {Expect.equal (pr (Bool false)) "false" ""}
      ; test "4" {Expect.equal (pr (Bool true)) "true" ""}
      ]



  let allTests =
    testList "pretty"
      [
        testExpr
      ]



  let test () = runTests defaultConfig allTests
