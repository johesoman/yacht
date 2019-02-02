namespace ParserTester



module Negative =



  open FsCheck
  open Extensions
  open PPrint.PPrint
  open Language.PrettyPrint



  module Doc =
    let swap (x : _ byref) (y : _ byref) =
      let z = x
      x <- y
      y <- z



    let fuzz (doFreq, dontFreq) =
      let rec goFuzz xs = gen {
        let! ys =
          Array.map go xs
          |> Gen.sequenceToArr

        let n = Array.length ys

        for i = 0 to n - 1 do
          let! doForget = Gen.tossCoin doFreq dontFreq
          let! doSwap   = Gen.tossCoin doFreq dontFreq

          if doForget then ys.[i] <- empty
          if doSwap   then
            let! j = Gen.choose (0, n - 1)
            swap &ys.[i] &ys.[j]

        return ys
        }

      and go =
        function
        | Chr c  -> gen { return Chr c }
        | Cat xs -> Gen.map Cat (goFuzz xs)
        | Sep xs -> Gen.map Sep (goFuzz xs)

        | Lines xs ->
            // If you want to fuzz lines, replace the
            // implementation below with the following
            // snippet:
            //   Gen.map Lines (goFuzz xs)
            Array.map go xs
            |> Gen.sequenceToArr
            |> Gen.map Lines

        | Nest (x, d) -> gen {
            let! d2 = go d
            return Nest (x, d2)
            }

      go >> Gen.sampleOne



  module Program =
    let pretty freq d =
      Program.pretty d
      |> Doc.fuzz freq



    let prettyString freq =
      pretty freq
      >> render



    let prettyPrint freq =
      prettyString freq
      >> printfn "%s"



  // +++++++++++
  // + testing +
  // +++++++++++



  open ParserTester.Positive



  let testProgram freq n =
    Gen.sampleOne (Program.generate n)
    |> Program.prettyPrint freq

