module Program



open App
open System
open Extensions
open RunProcess
open CommandLine
open CommandLine.Text



type Options =
  {
    [<Option('n', "num", Default = "100"
            , HelpText = "The number of tests to run."
    )>]
    num : string
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
  }



[<EntryPoint>]
let main argv =
  let result = CommandLine.Parser.Default.ParseArguments<Options>(argv)
  match result with
  | :? Parsed<Options> as parsed -> printfn "%A" parsed
  | :? NotParsed<Options> -> ()
  | _ -> ()

  0
