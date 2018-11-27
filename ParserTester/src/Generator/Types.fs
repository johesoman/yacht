namespace Generator



module Types =
  type Type =
    | Int  of int
    | Bool of bool



  type Name =
    | Name of string



  type Param =
    | Param of Type * Name



  type Bop =
    | Or
    | And
    | Not
    | Eq
    | Neq
    | Lt
    | Leq
    | Gt
    | Geq
    | Add
    | Sub
    | Mul
    | Div



  type Uop =
    | Not
    | Add
    | Sub



  type Expr =
    | Int  of int
    | Bool of bool
    | Var  of Name
    | Expr of Expr
    | Uop  of Uop * Expr
    | Call of Name * Expr []
    | Bop  of Bop * Expr * Expr



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
