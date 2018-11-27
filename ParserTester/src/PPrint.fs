module PPrint



open System
open Extensions



type Doc =
  | Text  of char []
  | Sep   of Doc  []
  | Cat   of Doc  []
  | Lines of Doc  []
  | Nest  of int * Doc



let txt : string -> Doc = String.toCharArray >> Text

let chr : char -> Doc = Array.singleton >> Text

let sep : _ seq -> Doc = Seq.toArray >> Sep

let cat : _ seq -> Doc = Seq.toArray >> Cat

let lines : _ seq -> Doc = Seq.toArray >> Lines

let nest (n : int) (p : Doc) : Doc = Nest (n, p)



let isEmpty =
  function
  | Text  [||] -> true
  | Sep   [||] -> true
  | Cat   [||] -> true
  | Lines [||] -> true
  | _          -> false



let render (p : Doc) : string =
  let rec join (sep : char []) =
    let f acc d =
      match Array.tryUnsnoc acc, Array.tryUncons (go d) with
      | Some (xs, x), Some (y, ys) ->
          let n  = Array.length x + Array.length sep
          let cs = Array.replicate n ' '

          Array.concat
            [ xs
            ; [|Array.concat [x; sep; y]|]
            ; Array.map (Array.append cs) ys
            ]

      | _ , Some (y, ys) -> Array.cons y ys
      | _                -> acc

    Array.fold f [||]

  and go : Doc -> char [] [] =
    function
    | Text cs -> [|cs|]

    | Sep ds ->
        Array.removeBy isEmpty ds
        |> join [|' '|]

    | Cat ds ->
        Array.removeBy isEmpty ds
        |> join [||]

    | Lines ds ->
        Array.removeBy isEmpty ds
        |> Array.collect go

    | Nest (n, p) ->
        let cs = Array.replicate n ' '
        Array.map (Array.append cs) (go p)

  let charGrid = go p

  let sb = Text.StringBuilder 64

  for i = 0 to Array.length charGrid - 2 do
    sb.Append charGrid.[i] |> ignore
    sb.Append '\n'         |> ignore

  sb.Append (Array.last charGrid) |> ignore
  sb.ToString ()



let empty = Text [||]



let surround d1 d2 = cat [d1; d2; d1]



let between d1 d2 d3 = cat [d1; d3; d2]



let sepList d =
  Array.ofSeq
  >> Array.intersperse (cat [d; chr ' '])
  >> cat



let brackets = between (chr '[') (chr ']')



let parens = between (chr '(') (chr ')')



let braces = between (chr '{') (chr '}')



let parensIf =
  function
  | false -> id
  | true  -> parens



let dquotes = surround (chr '"')
