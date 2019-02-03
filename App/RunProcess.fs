module RunProcess



open System
open Extensions
open System.Diagnostics
open System.ComponentModel
open System.Collections.Generic



type Cmd =
  { cmd  : string
  ; args : string []
  }



type CmdOk =
  { cmd      : Cmd
  ; output   : string
  ; exitCode : int
  }



type CmdErr =
  { cmd  : Cmd
  ; msg  : string
  }



module CmdErr =
  let prettyPrint ({cmd = cmd; msg = msg} : CmdErr) =
    let old = System.Console.ForegroundColor

    try
      Console.ForegroundColor <- ConsoleColor.Red

      let s =
        Array.append [|cmd.cmd|] cmd.args
        |> String.concat " "

      let lines =
        sprintf "command '%s' failed with\n'%s'" s msg
        |> String.splitOn '\n'

      printfn "%s" ("ERROR: " + lines.[0])
      Array.iter (fun s -> printfn "%s" ("       " + s)) lines.[1 ..]

    finally
      Console.ForegroundColor <- old



let runProcessWithInput (cmd : Cmd) write =
  let go () =
    // setup process
    use p =
      let info =
        ProcessStartInfo(
          RedirectStandardOutput = true,
          RedirectStandardInput  = true,
          RedirectStandardError  = true,
          // UseShellExecute has to be false when we use Redirect*
          UseShellExecute        = false,
          FileName               = cmd.cmd,
          Arguments              = String.concat " " cmd.args
        )
      new Process(StartInfo = info)

    // setup stdout
    let outputs = List<string>()
    DataReceivedEventHandler (fun _ ev -> outputs.Add ev.Data)
    |> p.OutputDataReceived.AddHandler


    // setup stderror
    let errors = List<string>()
    DataReceivedEventHandler (fun _ ev -> errors.Add ev.Data)
    |> p.ErrorDataReceived.AddHandler


    // session
    try
      p.Start() |> ignore
      p.BeginOutputReadLine()
      p.BeginErrorReadLine()

      write p.StandardInput

      p.StandardInput.Close()

      p.WaitForExit()

      match String.concatNonNull "" errors with
      | "" -> Result.Ok (p.ExitCode, String.concatNonNull "" outputs)
      | s  -> Result.Error s

    // exception handling
    with
    | :? InvalidOperationException ->
        Error "No file was specified"

    | :? Win32Exception as ex ->
        match ex.NativeErrorCode with
        | 2 -> Error "Cannot find the specified file"
        | _ ->
            sprintf "%s, NativeErrorCode: %d" ex.Message ex.NativeErrorCode
            |> Error

    | ex -> Error ex.Message

  go ()
  |> function
     | Error s -> Error {CmdErr.cmd = cmd; CmdErr.msg = s}

     | Ok (x, s) -> Ok { CmdOk.cmd      = cmd
                       ; CmdOk.exitCode = x
                       ; CmdOk.output   = s
                       }



let runProcess cmd = runProcessWithInput cmd (fun _ -> ())
