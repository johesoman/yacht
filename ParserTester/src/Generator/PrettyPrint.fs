namespace Generator



module PrettyPrint =



  open Types


  module Expr =
    let precedence =
      function
      | Call _ -> 8
      | Var  _ -> 8
      | Int  _ -> 8
      | Bool _ -> 8
      | Uop (Not, _)    -> 7
      | Bop (_, Mul, _) -> 6
      | Bop (_, Div, _) -> 6
      | Bop (_, Add, _) -> 5
      | Bop (_, Sub, _) -> 5
      | Bop (_, Lt , _) -> 4
      | Bop (_, Leq, _) -> 4
      | Bop (_, Gt , _) -> 4
      | Bop (_, Geq, _) -> 4
      | Bop (_, Eq , _) -> 3
      | Bop (_, Neq, _) -> 3
      | Bop (_, And, _) -> 2
      | Bop (_, Or , _) -> 1
      | Asn  _          -> 0


