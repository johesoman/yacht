namespace Language



module Types =
  type Type =
    | Int
    | Bool



  type Name = string



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
    | Call of Name * Expr list
    | Bop  of Expr * Bop * Expr



  type Stmt =
    | ProcRet
    | Expr    of Expr
    | FuncRet of Expr
    | Block   of Stmt list
    | Decl    of Type * Name
    | While   of Expr * Stmt list
    | If      of Expr * Stmt list * Stmt list



  type Def =
    | Proc of Name * Param list * Stmt list
    | Func of Type * Name * Param list * Stmt list



  type Program =
    | Program of Def list
