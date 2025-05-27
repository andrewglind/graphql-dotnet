using GraphQL;
using GraphQL.Utilities;
using GraphQL.Client.Abstractions;
using GraphQL.Respositories.Interceptors;
using Castle.DynamicProxy;
using System.Reflection;

namespace GraphQL.Repositories
{
    public sealed class DefaultGraphQLRepositoryFactory : IGraphQLRepositoryFactory<T>
    {
        private IProxyGenerator _ProxyGenerator;
        private IInterceptor _Interceptor;

        public GraphQLRepositoryFactory(IGraphQLClient client) :
            this(client, new ProxyGenerator())
        {
        }

        public GraphQLRepositoryFactory(IGraphQLClient client, IProxyGenerator proxyGenerator) : 
            this(new DefaultGraphQLInterceptor(client), proxyGenerator)
        {
        }

        public GraphQLRepositoryFactory(IInterceptor interceptor, IProxyGenerator proxyGenerator)
        {
            _Interceptor = interceptor;
            _ProxyGenerator = proxyGenerator;
        }

        public override T CreateGraphQLRespository<T>() where T : class
        {
            var ttype = typeof(T);
            var hasAttribute = ttype.GetCustomAttribute<GraphQLRepositoryAttribute>() != null;
            if (!hasAttribute)
            {
                var attributeName = StringUtils.GetFriendlyAttributeName(typeof(GraphQLRepositoryAttribute).Name);
                throw new InvalidOperationException($"The type '{ttype.Name}', must be decorated with the [{attributeName}] attribute.");
            }

            return _ProxyGenerator.CreateInterfaceProxyWithoutTarget<T>(_Interceptor);
        }
    }
}
