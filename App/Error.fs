module Error



open System
open System.IO
open Extensions.Extensions



type Error =
  | CommandError of RunProcess.CommandError



module Error =
  let pretty =
    function
    | CommandError err ->
        let cmdArgs =
          Array.append [|err.command|] err.arguments
          |> String.concat " "

        sprintf "command '%s' failed with\n'%s'" cmdArgs err.message



  let write (sw : StreamWriter) err =
    let lines =
      pretty err
      |> String.splitOn '\n'

    sw.WriteLine ("ERROR: " + lines.[0])
    Array.iter (fun s -> sw.WriteLine ("       " + s)) lines.[1 ..]



  let print err =
    let old = System.Console.ForegroundColor

    try
      Console.ForegroundColor <- ConsoleColor.Red

      use sw = new StreamWriter(Console.OpenStandardOutput())
      write sw err

    finally
      Console.ForegroundColor <- old
