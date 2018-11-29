namespace Generator


module Generate =



  open FsCheck
  open Language.Types



  module Gen =
    let alphaChar =
      ['a' .. 'z'] @ ['A' .. 'Z']
      |> Gen.elements



    let numChar =
      ['0' .. '9']
      |> Gen.elements



    let alphaNumChar =
      Gen.frequency
        [ 2, alphaChar
        ; 1, numChar
        ]



    let alphaNumString lo hi = gen {
        let! n  = Gen.choose (lo, hi)
        let! c  = alphaChar
        let! cs = Gen.arrayOfLength n alphaNumChar
        return new string(Array.append [|c|] cs)
      }






