namespace Generator



module Types =
  type Type =
    | Int
    | Bool



  type Name =
    | Name of string



  type Param =
    | Param of Type * Name



  type Uop =
    | Not



  type Bop =
    | Asn
    | Or
    | And
    | Eq
    | Ne
    | Lt
    | Le
    | Gt
    | Ge
    | Add
    | Sub
    | Mul
    | Div



  type Expr =
    | Int  of int
    | Bool of bool
    | Var  of Name
    | Uop  of Uop * Expr
    | Call of Name * Expr []
    | Bop  of Expr * Bop * Expr



  type Stmt =
    | ProcRet
    | Expr    of Expr
    | FuncRet of Expr
    | Decl    of Type * Name
    | While   of Expr * Stmt []
    | If      of Expr * Stmt [] * Stmt []



  type Def =
    | Func of Type * Name * Param [] * Stmt []
    | Proc of Type * Name * Param [] * Stmt []



  type Program =
    | Program of Def []
