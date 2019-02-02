namespace ParserTester



module Positive =



  open System
  open FsCheck
  open Extensions
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



  module Type =
    let generate =
      Gen.elements
        [ Type.Int
        ; Type.Bool
        ]



  module Param =
    let generate = gen {
      let! t = Type.generate
      let! s = Gen.identifier (1, 5)

      return Param (t, s)
      }



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




  module Stmt =
    type Size =
      { initial : int
      ; current : int
      }



    let leafs = Gen.map Expr Expr.leafs



    let expr size =
      Expr.generate size.initial
      |> Gen.map Expr



    let procRet = gen {return ProcRet}



    let funcRet size =
      Expr.generate size.initial
      |> Gen.map FuncRet



    let decl = gen {
      let! t = Type.generate
      let! s = Gen.identifier (1, 5)

      return Decl (t, s)
      }



    let rec gIf size =
      if size.current <= 0 then leafs
      else gen {
        let! m     = Gen.choose (0, 5)
        let! m2    = Gen.choose (0, 5 - m)
        let  size2 = { size with current = size.current / max 1 (m + m2) }

        let! e   = Expr.generate size2.initial
        let! ss  = Gen.listOfLength m  (stmt size2)
        let! ss2 = Gen.listOfLength m2 (stmt size2)

        return If (e, ss, ss2)
        }

    and gWhile size =
      if size.current <= 0 then leafs
      else gen {
        let! m     = Gen.choose (0, 5)
        let  size2 = {size with current = size.current / max 1 m}

        let! e  = Expr.generate size2.initial
        let! ss = Gen.listOfLength m (stmt size2)

        return While (e, ss)
        }



    and block size =
      if size.current <= 0 then leafs
      else gen {
        let! m     = Gen.choose (0, 5)
        let  size2 = {size with current = size.current / max 1 m}
        let! ss    = Gen.listOfLength m (stmt size2)

        return Block ss
        }



    and stmt n =
      Gen.oneof
        [ decl
        ; gIf n
        ; expr n
        ; procRet
        ; block n
        ; gWhile n
        ; funcRet n
        ]



    let generate n = stmt {initial = n; current = n}



  module Def =
    let nameAndParams = gen {
      let! s  = Gen.identifier (1, 5)
      let! m  = Gen.choose (1, 5)
      let! ps = Gen.listOfLength m Param.generate

      return s, ps
      }



    let body n = gen {
      let! m  = Gen.choose (1, 5)
      return! Gen.listOfLength m (Stmt.generate (n / m))
    }



    let generate n = gen {
      let! s, ps = nameAndParams
      let! ss    = body n

      return!
        Gen.oneof
          [ gen { return Proc (s, ps, ss) }

          ; gen {
                let! t = Type.generate
                return Func (t, s, ps, ss)
              }
          ]
      }



  module Program =
    let generate n = gen {
      let  n2 = max 1 (n |> double |> log |> int)
      let! m  = Gen.choose (1, n2)
      let! ds = Gen.listOfLength m (Def.generate (n / m))
      return Program ds
      }



  // +++++++++++
  // + testing +
  // +++++++++++


  let testExpr n =
    Gen.sampleOne (Expr.generate n)
    |> Expr.prettyPrint



  let testStmt n =
    Gen.sampleOne (Stmt.generate n)
    |> Stmt.prettyPrint



  let testDef n =
    Gen.sampleOne (Def.generate n)
    |> Def.prettyPrint



  let testProgram n =
    Gen.sampleOne (Program.generate n)
    |> Program.prettyPrint
