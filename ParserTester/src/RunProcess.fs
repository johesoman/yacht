module RunProcess



open System
open Extensions
open System.Diagnostics
open System.ComponentModel
open System.Collections.Generic



type CommandError =
  { command   : string
  ; arguments : string []
  ; message   : string
  }



let runProcess cmd args write =
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
          FileName               = cmd,
          Arguments              = String.concat " " args
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
      | "" -> Result.Ok (String.concatNonNull "" outputs)
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
  |> Result.mapError (fun s -> {command = cmd; arguments = args; message = s})
