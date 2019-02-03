namespace Parser
{
    public interface IProgramVisitor<R, A>
    {
        R Visit(Program p, A arg);
    }

    public interface IDeclarationVisitor<R, A>
    {
        R Visit(FormalDeclaration d, A arg);
    }

    public interface IStatementVisitor<R, A>
    {
        R Visit(BlockStatement s, A arg);
        R Visit(IfStatement s, A arg);
        R Visit(WhileStatement s, A arg);
        R Visit(ReturnStatement s, A arg);
        R Visit(ExpressionStatement s, A arg);
        R Visit(FormalStatement s, A arg);
    }

    public interface IExpressionVisitor<R, A>
    {
        R Visit(IdentifierExpression e, A arg);
        R Visit(NumberExpression e, A arg);
        R Visit(BooleanExpression e, A arg);
        R Visit(AssignmentExpression e, A arg);
        R Visit(BinaryOperatorExpression e, A arg);
        R Visit(UnaryOperatorExpression e, A arg);
        R Visit(FunctionCallExpression e, A arg);
    }
}
