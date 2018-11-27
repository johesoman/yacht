namespace Generator


module Generate =



  open FsCheck



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



    let alphaNumString = gen {
        let! n  = Gen.choose (0, 4)
        let! c  = alphaChar
        let! cs = Gen.arrayOfLength n alphaNumChar
        return new string(Array.append [|c|] cs)
      }






