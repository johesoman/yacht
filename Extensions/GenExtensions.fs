namespace Extensions



module Gen =



  open System
  open FsCheck



  let tossCoin trueFreq falseFreq =
    Gen.frequency
      [ trueFreq , gen { return true }
      ; falseFreq, gen { return false }
      ]



  let sampleOne g =
    Gen.sample 0 1 g
    |> List.head



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
    let! cs = Gen.arrayOfLength n alphaNumChar
    return new string(cs)
    }


  let identifier (lenLo, lenHi) = gen {
    let! c  = alphaChar
    let! cs = alphaNumString lenLo (lenHi - 1)
    return string c + cs
    }



module Rand =



  open FsCheck



  let choose (xFreq, x) (yFreq, y) =
    let g = gen {
      let! first = Gen.tossCoin xFreq yFreq

      if first
        then return x
        else return y
      }

    Gen.sampleOne g
