namespace Generator



module PrettyPrintBadly =
  open PPrint
  open Language.Types



  module Expr =
    let prettyBad =
      function
      | Var _ -> PPrint.empty
      |     _ -> PPrint.empty



    let prettyBadString =
      prettyBad
      >> PPrint.render
