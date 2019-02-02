namespace ParserTester



module Generate =


  open FsCheck
  open Extensions
  open Language.PrettyPrint



  let positive n =
    Positive.Program.generate n
    |> Gen.sampleOne
    |> Program.prettyString



  let negative freq n =
    Positive.Program.generate n
    |> Gen.sampleOne
    |> Negative.Program.prettyString freq




