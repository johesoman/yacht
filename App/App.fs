module App



open System
open System.IO
open Extensions
open RunProcess



type TestInfo =
  { num : int
  ; gen : int -> string
  ; chk : CmdOk -> string -> bool
  }



let testRunner generate action check n =
  let rec go i =
    if n <= i then printfn ""
    else
      let x = generate (i + 1)

      match action x with
      | None -> ()
      | Some result ->
          if check result x
            then
              if i % 50 = 0 then printfn ""
              printf "."

              go (i + 1)
            else ()

  go 0



let processRunner cmd (s : string) =
  let write (sw : StreamWriter) = sw.Write s

  match RunProcess.runProcessWithInput cmd write with
  | Ok cmdOk     -> Some cmdOk
  | Error cmdErr ->
      CmdErr.prettyPrint cmdErr
      None



let runTest cmd (info : TestInfo) =
  testRunner info.gen (processRunner cmd) info.chk info.num



module TestInfo =
  let scaledGen gen i = gen (5 * i)



  let scaledGenParserPos = scaledGen ParserTester.Generate.positive
  let scaledGenParserNeg = scaledGen (ParserTester.Generate.negative (1, 100))



  let parserTestChk {cmd = _; output = output; exitCode = exitCode} input =
    true



  let parserTestPos n =
    { num = n
    ; gen = scaledGenParserPos
    ; chk = parserTestChk
    }



  let parserTestNeg n =
    { num = n
    ; gen = scaledGenParserNeg
    ; chk = parserTestChk
    }



  let parserTestAll n =
    let pos = scaledGenParserPos
    let neg = scaledGenParserNeg

    { num = n
    ; gen = fun i -> (Rand.choose (1, pos) (1, neg)) i
    ; chk = parserTestChk
    }



let test () =
  let gen  _ =
    let n   = 10
    let pos = lazy (ParserTester.Generate.positive        n)
    let neg = lazy (ParserTester.Generate.negative (1,10) n)

    Rand.choose (1, pos) (1, neg)
    |> Lazy.force

  let act x = Some ""

  let chk s x =
    match Console.ReadLine() with
    | "error" -> false
    | _       ->
        printfn "%s" x
        true

  testRunner gen act chk 5
