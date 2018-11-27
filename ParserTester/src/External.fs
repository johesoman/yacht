module External



type ParseError =
  { message : string }



let parse s =
  try
    Ok (Parser.Utility.Parse s)
  with
    | :? Parser.ParseError as ex ->
        Error {message = ex.Message}



let checkSyntax s =
  match parse s with
  | Ok _ -> None
  | Error err -> Some err



let tryPretty s =
  match parse s with
  | Ok p -> Some (p.Pretty())
  | Error _ -> None



let prettyOrEmpty s =
  match tryPretty s with
  | Some s2 -> s2
  | None    -> ""



let prettyPrint s =
  match tryPretty s with
  | Some s2 -> printfn "%s" s2
  | None -> ()

