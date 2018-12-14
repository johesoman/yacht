module Extensions



open System



module Pair =
  let rightLeft x y = y, x



  let leftRight x y = x, y



  let map f (x, y) = f x, f y



  let mapLeft f (x, y) = f x, y



  let mapRight f (x, y) = x, f y



module List =
  let intersperse x xs =
    let rec go =
      function
      | [] -> []
      | [y] -> [y]
      | y :: ys -> y :: x :: go ys

    go xs



  let intercalate xs ys =
    let rec go =
      function
      | [] -> []
      | [z] -> [z]
      | z :: zs -> z :: xs @ go zs

    go ys



  let snoc x xs = xs @ [x]



  let tryUnsnoc =
    let rec go =
      function
      | []  -> failwith "Error @ List.tryUnsnoc"
      | [x] -> [], x
      | x :: xs ->
          match go xs with
          | ys, y -> x :: ys, y

    function
    | [] -> None
    | xs -> Some (go xs)



module Map =
  let singleton (k : 'Key) (v : 'Value) =
    Map.add k v Map.empty



  let tryFindByKey f =
    let g k v =
      if f k
        then Some (k, v)
        else None

    Map.tryPick g



  let tryFindByValue f =
    let g k v =
      if f v
        then Some (k, v)
        else None

    Map.tryPick g



  let tryFindByBoth f =
    let g k v =
      if f v
        then Some (k, v)
        else None

    Map.tryPick g



  let containsValue x =
    tryFindByValue ((=) x)
    >> Option.isSome



  // Union by key. It resolves collisions with onCollision.
  let unionWith onCollision m =
    let f acc (x, y) =
      match Map.tryFind x acc with
      | None   -> Map.add x y acc
      | Some z -> Map.add x (onCollision z y) acc

    Map.toSeq >> Seq.fold f m



  // Union by key, left-biased.
  // It resolves collisions by always choosing the value from m1.
  let union (m1 : Map<_, _>) (m2 : Map<_, _>) =
    unionWith (fun a _ -> a) m1 m2



  let mapValues f = Map.map (fun k v -> f v)



  let keys m =
    Map.toSeq m
    |> Seq.map fst



  let values m =
    Map.toSeq m
    |> Seq.map snd



  let findWithDefault k v1 m =
    match Map.tryFind k m with
    | Some v2 -> v2
    | None    -> v1



module Seq =
  let group (xs : _ []) = Seq.groupBy id xs



  let removeBy f = Seq.filter (f >> not)



  let removeAll x xs = Seq.filter ((<>) x) xs



  // returns the first duplicate
  let tryFindDuplicate (xs : _ seq) =
      let s = System.Collections.Generic.HashSet()
      let e = xs.GetEnumerator()
      let mutable found  = false
      let mutable result = None

      while e.MoveNext() && not found do
        let x = e.Current
        if s.Contains x
          then
            result <- Some x
            found  <- true
          else
            ignore (s.Add x)

      result



  // returns the first x in xs not present in ys
  let tryFindMissing (xs : _ seq) (ys : _ seq) =
    let s = Seq.fold (fun acc k -> Set.add k acc) Set.empty ys
    let e = xs.GetEnumerator()
    let mutable found = false
    let mutable ret   = None

    while e.MoveNext() && not found do
      let x = e.Current
      if Set.contains x s
        then ()
        else
          ret   <- Some x
          found <- true

    ret



module Array =
  let pairwiseMap f =
    Array.pairwise
    >> Array.map (fun (a, b) -> f a b)



  let pairwiseFold init f xs =
    let n = Array.length xs

    let rec go i acc =
      if n <= i
        then acc
        else
          f acc xs.[i - 1] xs.[i]
          |> go (i + 1)

    go 1 init



  let pairwiseForall f xs =
    let n = Array.length xs

    let rec go i =
      if n <= i
        then true
        else
          if f xs.[i - 1] xs.[i]
            then go (i + 1)
            else false

    go 1



  let pairwiseExists f xs =
    let n = Array.length xs

    let rec go i =
      if n <= i
        then false
        else
          if f xs.[i - 1] xs.[i]
            then true
            else go (i + 1)

    go 1



  let pairwiseExactlyOne f xs =
    let n = Array.length xs

    let rec go i flag =
      if n <= i
        then flag
        else
          let  res = f xs.[i - 1] xs.[i]

          if   res && flag then false
          elif res         then go (i + 1) true
          else                  go (i + 1) flag

    go 1 false



  let windowedMap n f =
    Array.windowed n
    >> Array.map f



  let initial (xs : _ []) =
    match Array.length xs with
    | n when n < 2 -> [||]
    | n            -> xs.[.. n - 2]



  let tail =
    function
    | [||] -> [||]
    | xs   -> xs.[1 ..]



  let update i x =
    Array.mapi (fun j y -> if i = j then x else y)



  let group (xs : _ []) = Array.groupBy id xs



  let removeBy f = Array.filter (f >> not)



  let removeAll x xs = Array.filter ((<>) x) xs



  let collectOption f = Array.collect (f >> Option.defaultValue [||])



  let toArrayOption =
    function
    | [||] -> None
    | xs   -> Some xs



  let ofArrayOption =
    function
    | Some xs -> xs
    | None    -> [||]



  let mapOption f =
     Array.collect (f >> Option.toArray)
     >> toArrayOption



  let foldOption f init xs =
    let         n    = Array.length xs
    let mutable i    = 0
    let mutable ret  = Some init
    let mutable loop = true

    while i < n && loop do
      match ret with
      | Some acc -> ret  <- f acc xs.[i]; i <- i + 1
      | None     -> loop <- false

    ret



  let collectArrayOption f =
    let g acc x = Option.map (Array.append acc) (f x)
    foldOption g ([||])



  let sequenceOption (xs : _ option []) : _ [] option =
    mapOption id xs



  let intersperse sep xs =
    match Array.length xs with
    | n when n < 2 -> xs

    | n ->
        let ys : _ [] = Array.zeroCreate (n + n - 1)

        ys.[0] <- xs.[0]

        let mutable i = 1
        let mutable j = 1

        while i < n do
          ys.[j]     <- sep
          ys.[j + 1] <- xs.[i]

          i <- i + 1
          j <- j + 2

        ys



  let first xs = Array.get xs 0



  let cons x xs = Array.append [|x|] xs



  let tryUncons =
    function
    | [| |] -> None
    | [|x|] -> Some (x, [||])
    | xs    -> Some ( xs.[0]
                    , xs.[1 ..]
                    )



  let snoc xs x = Array.append xs [|x|]



  let tryUnsnoc =
    function
    | [| |] -> None
    | [|x|] -> Some ([||], x)
    | xs    -> Some ( xs.[.. Array.length xs - 2]
                    , Array.last xs
                    )



  let mapAccum f init xs =
    let g (ys, acc) x =
      let y, acc2 = f acc x
      snoc ys y, acc2

    Array.fold g ([||], init) xs



  let intercalate sep = intersperse sep >> Array.concat



module String =
  let concatNonNull sep ss =
    Seq.filter (String.IsNullOrEmpty >> not) ss
    |> String.concat sep



  let splitOnMany cs (s : string) = s.Split cs



  let splitOn c s = splitOnMany [|c|] s



  let toLines (s : string) = s.Split '\n'



  let fromLines ss = String.concat "\n" ss



  let ofCharArray (xs : char []) = new string(xs)



  let toCharArray (s: string) = s.ToCharArray()



  let hofBase f =
    toCharArray
    >> f
    >> ofCharArray



  let removeAll c = hofBase (Array.removeAll c)



  let removeBy f = hofBase (Array.removeBy f)



  let filter f = hofBase (Array.filter f)



  let map f = hofBase (Array.map f)



  let takeNOrLess n s =
    match n, s with
    | n, _  when n < 1 -> ""
    | _, "" -> ""
    | _, _ ->
        if String.length s < n
          then ""
          else s.[.. n - 1]




  let endsWith s s2 =
    match String.length s, String.length s2 with
    | _, 0 -> true
    | n, m when m < n -> false
    | n, m -> s = s2.[m - n ..]


