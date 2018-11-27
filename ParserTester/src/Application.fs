module Application



// dotnet
open System
open System.IO
open System.Text

// generator
// open Generator.Types
// open Generator.Generate
// open Generator.PrettyPrint

// other
open Error
open RunProcess



type TestInfo =
  { numberOfTests : int
  ; command       : string
  ; arguments     : string []
  }



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


