module Program



open App
open System
open Extensions
open RunProcess
open CommandLine



// Now, we are using CommandLineParser.FSharp. The current implementation
// is almost good. The main issues are:
//
//    * On the output, the application is referred to as "App" instead of
//      YACHT. The name seems to be inherited from project name, but maybe
//      there is a way to work around it (probably).
//
//    * I have not yet figured out how to change the version number.
//
// A possible solution is to just use Argu. It worked fine - it was simple and
// configurable. However, it does not currently work, because of some weird
// error. Basically, it throws an exception because it does something illegal
// with an array of System.String []. Since I did not want to spend too much
// time on that, I just switched to CommandLineParser.FSharp. However, the best
// long time solution would probably be to fix that error and use Argu. The
// old code using Argu can be found below main.



// CommandLineParser.FSharp requires proper enums.
type TestType =
  | parser = 0



type Options =
  {
    [<Option('n', "num", Default = 100
            , HelpText = "The number of tests to run."
    )>]
    num : int
  ;
    [<Option('c', "cmd"
            , Required = true
            , HelpText =
                "The command to run. If you have spaces in your path, " +
                "enclose it in quotation marks. " +
                "Examples: " +
                "dotnet path/app.dll, " +
                "dotnet \"path with spaces/app.dll\", " +
                "path/app.exe, " +
                "\"path with spaces/app.exe\"."
    )>]
    cmd : seq<string>
  ;
    [<Option('t', "type"
            , Required = true
            , HelpText = "The kind of test.")>]
    testType : TestType
  }



let runApp argv =
  let result = CommandLine.Parser.Default.ParseArguments<Options>(argv)
  match result with
  | :? Parsed<Options> as parsed ->
      let ss   = Array.ofSeq parsed.Value.cmd
      let cmd  = ss.[0]
      let args = Array.append ss.[1 ..] [|"-t"|]
      let num  = parsed.Value.num
      let tt   = parsed.Value.testType

      TestInfo.parserTestAll num
      |> App.runTest {Cmd.cmd = cmd; Cmd.args = args}

  | :? NotParsed<Options> -> ()
  | _ -> ()



let runTests path =
  let go (s : string) =
    let ss = s.Split ' '
    runApp ss

  let cmds = System.IO.File.ReadAllLines path

  go cmds.[0]



let testParsers () = runTests "testsubjects/parsers/cmds.txt"



[<EntryPoint>]
let main argv = runApp argv; 0





// open Argu



// type CliArgs =
//     |              Tests of int:int
//     | [<Mandatory>]Cmd   of string:string
//   with
//     interface IArgParserTemplate with
//       member s.Usage =
//         match s with
//         | Tests _ ->
//             "specify how many tests to run. this is an optional parameter. \
//              the default value is 100."
//         | Cmd _ ->
//             String.concat ""
//               [| "specify the command to run. \n"
//                ; "examples:\n"
//                ; "  \"dotnet mylexer/mylexer.dll\"\n"
//                ; "  \"mono mylexer.exe\"\n"
//                ; "  \"mylexer.exe\"\n"
//               |]



// let getCmdAndNum cliArgs =

//   let cmd, args =
//     List.pick (function Cmd ss -> Some ss | _ -> None) cliArgs
//     |> function
//        | [||] -> ""    , [||]
//        | ss   -> ss.[0], ss.[1 ..]

//   let num =
//     List.tryPick (function Num n -> Some n | _ -> None) cliArgs
//     |> function
//        | Some n -> n
//        | None   -> 100

//   { cmd = cmd; args = args }, TestInfo.parserTestAll num



// let checkStructure =
// #if INTERACTIVE || DEBUG
//     true
// #else
//     false
// #endif



// let parser =
//   let colorizer =
//     function
//     | ErrorCode.HelpText -> None
//     | _ -> Some ConsoleColor.Red

//   ArgumentParser.Create<CliArgs> (
//     programName    = "yacht",
//     checkStructure = checkStructure,
//     errorHandler   = ProcessExiter(colorizer = colorizer))



// [<EntryPoint>]
// let main _ =
//   let info =
//     parser.Parse().GetAllResults()
//     |> getCmdAndNum

//   match Application.run info with
//   | Error err -> Error.print err; 1
//   | Ok      _ -> 0


