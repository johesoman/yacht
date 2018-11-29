namespace Generator


module Generate =



  open System
  open FsCheck
  open GenExtensions
  open Language.Types
  open Language.PrettyPrint



  module Uop =
    let generate =
      Gen.elements
        [ Not
        ]



  module Bop =
    let generate =
      Gen.elements
        [ Asn
        ; Or
        ; And
        ; Eq
        ; Ne
        ; Lt
        ; Le
        ; Gt
        ; Ge
        ; Add
        ; Sub
        ; Mul
        ; Div
        ]



  module Expr =
    let integer =
      // beware:
      //   Gen.choose (Int32.MinValue, Int32.MaxValue)
      //   will throw DivideByZeroException
      Gen.choose (-100, 100)
      |> Gen.map Int



    let boolean =
      Gen.elements [false; true]
      |> Gen.map Bool



    let var =
      Gen.identifier (1, 5)
      |> Gen.map Var



    let leafs =
      Gen.oneof
        [ integer
        ; boolean
        ; var
        ]



    let rec uops n =
      if n <= 0 then leafs
      else gen {
        let! e  = expr (n - 1)
        let! op = Uop.generate
        return Uop (op, e)
      }



    and bops n =
      if n <= 0 then leafs
      else gen {
        let! e  = expr (n / 2)
        let! op = Bop.generate
        let! e2 = expr (n / 2)
        return Bop (e, op, e2)
        }



    and call n =
      if n <= 0 then leafs
      else gen {
        let! m  = Gen.choose (1, 5)
        let! es = Gen.listOfLength m (expr (n / m))
        let! s  = Gen.identifier (1, 5)
        return Call (s, es)
        }



    and expr n =
      Gen.oneof
        [ var
        ; uops n
        ; bops n
        ; call n
        ; integer
        ; boolean
        ]



    let generate n = expr n



// +++++++++++
// + testing +
// +++++++++++



  let testExpr n =
    Gen.sampleOne (Expr.generate n)
    |> Expr.prettyPrint
