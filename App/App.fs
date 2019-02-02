module App



open System
open Extensions



type TestInfo =
  { numberOfTests : int
  ; command       : string
  ; arguments     : string []
  }



let run generate action check n =
  let rec go i =
    if n <= i then ()
    else
      let x = generate ()

      match action (i + 1) x with
      | None -> ()
      | Some result ->
          if check result x
            then go (i + 1)
            else ()

  go 0



let check s x =
  match Console.ReadLine() with
  | "error" -> false
  | _       ->
      printfn "%s" s
      true



let generate () =
  let n   = 10
  let pos = lazy (ParserTester.Generate.positive        n)
  let neg = lazy (ParserTester.Generate.negative (1,10) n)

  Rand.choose (1, pos) (1, neg)
  |> Lazy.force



let action i x =
  printfn "This is test number %A" i

  Some ""


let test () =
  run generate action check 5



// let run (info : TestInfo) : Result<unit, Error> =
//   let writer (input : string) (sw : StreamWriter) = sw.Write input

//   let rec go i =
//     if info.numberOfTests < i then Ok ()
//     else
//       if i = 1 then printfn "Running %d tests!" info.numberOfTests

//       let elems = SourceElement.generate (min (i * 5) 100)
//       let input = SourceElement.prettyMany elems

//       let result =
//         RunProcess.runProcess
//           info.command
//           info.arguments
//           (writer input)

//       match result with
//       | Error cmdErr -> Error (CommandError cmdErr)
//       | Ok    output ->
//           match tryFindDifference elems output with
//           | None ->
//               printf "."
//               go (i + 1)
//           | Some (input, expected, actual) ->
//               printfn "\n"
//               printfn "+++++++++++++++++++"
//               printfn "+      Input       "
//               printfn "+++++++++++++++++++"
//               printfn "%s" input
//               printfn "+++++++++++++++++++"
//               printfn "+ Expected output +"
//               printfn "+++++++++++++++++++"
//               printfn "%s" expected
//               printfn "+++++++++++++++++++"
//               printfn "+  Actual output  +"
//               printfn "+++++++++++++++++++"
//               printfn "%s" actual
//               Ok ()

//   if info.numberOfTests < 1
//     then Ok ()
//     else go 1



// let test () =
//   run
//     { numberOfTests = 100
//     ; command = "dotnet"
//     ; arguments = [|"/Users/j/Desktop/lextest/test/sam/app/sam.dll"|]
//     }
//   |> Result.map (printfn "%A")
//   |> ignore


