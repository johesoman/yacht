namespace Generator



module PrettyPrintBadly =



  open PPrint.PPrint
  open Language.Types



  module Expr =
    let prettyBad =
      function
      | Var s -> txt s
      |     _ -> empty



    let prettyBadString =
      prettyBad
      >> render
