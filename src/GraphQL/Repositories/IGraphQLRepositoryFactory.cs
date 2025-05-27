namespace GraphQL.Repositories;

public interface IGraphQLRepositoryFactory<T>
{
    public T CreateGraphQLRespository();
}
