module Program



open Argu
open Error
open System
open Extensions
open Application



type CliArgs =
    |              Tests of int:int
    | [<Mandatory>]Cmd   of string:string
  with
    interface IArgParserTemplate with
      member s.Usage =
        match s with
        | Tests _ ->
            "specify how many tests to run. this is an optional parameter. \
             the default value is 100."
        | Cmd _ ->
            String.concat ""
              [| "specify the command to run. \n"
               ; "examples:\n"
               ; "  \"dotnet mylexer/mylexer.dll\"\n"
               ; "  \"mono mylexer.exe\"\n"
               ; "  \"mylexer.exe\"\n"
              |]



module TestInfo =
  let ofCliArgs args =
    // If two separate values are given for the same argument,
    // reversing the list makes sure that we choose the second value.
    let args = List.rev args

    let command, arguments =
      List.pick (function Cmd s -> Some s | _ -> None) args
      |> String.splitOn ' '
      |> Array.removeAll " "
      |> function
         | [||] -> ""    , [||]
         | ss   -> ss.[0], ss.[1 ..]

    let tests =
      List.tryPick (function Tests n -> (Some n) | _ -> None) args
      |> function
         | Some n -> n
         | None   -> 100

    { numberOfTests = tests
    ; command       = command
    ; arguments     = arguments
    }



let checkStructure =
#if INTERACTIVE || DEBUG
    true
#else
    false
#endif



let parser =
  let colorizer =
    function
    | ErrorCode.HelpText -> None
    | _ -> Some ConsoleColor.Red

  ArgumentParser.Create<CliArgs> (
    programName    = "parsertester",
    checkStructure = checkStructure,
    errorHandler   = ProcessExiter(colorizer = colorizer))



[<EntryPoint>]
let main _ =
  let info =
    parser.Parse().GetAllResults()
    |> TestInfo.ofCliArgs
  0
  // match Application.run info with
  // | Error err -> Error.print err; 1
  // | Ok      _ -> 0


