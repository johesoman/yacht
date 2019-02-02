namespace Generator
open PPrint



module PrettyPrintBadly =



  open FsCheck
  open PPrint.PPrint
  open GenExtensions
  open Language.PrettyPrint
  open Extensions.Extensions



  module Doc =
    let tossCoin doFreq dontFreq =
      Gen.frequency
        [doFreq   , gen { return true }
        ; dontFreq, gen { return false }
        ]



    let swap (x : _ byref) (y : _ byref) =
      let z = x
      x <- y
      y <- z



    let fuzz doFreq dontFreq =
      let rec goFuzz xs = gen {
        let! ys =
          Array.map go xs
          |> Gen.sequenceToArr

        let n = Array.length ys

        for i = 0 to n - 1 do
          let! doForget = tossCoin doFreq dontFreq
          let! doSwap   = tossCoin doFreq dontFreq

          if doForget then ys.[i] <- PPrint.empty
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
    let prettyBad doFreq dontFreq d =
      Program.pretty d
      |> Doc.fuzz doFreq dontFreq



    let prettyBadString doFreq dontFreq =
      prettyBad doFreq dontFreq
      >> PPrint.render



    let prettyPrintBadly doFreq dontFreq =
      prettyBadString doFreq dontFreq
      >> printfn "%s"




// +++++++++++
// + testing +
// +++++++++++
  open Generator.Generate



  let testProgram doFreq dontFreq n =
    Gen.sampleOne (Program.generate n)
    |> Program.prettyPrintBadly doFreq dontFreq

