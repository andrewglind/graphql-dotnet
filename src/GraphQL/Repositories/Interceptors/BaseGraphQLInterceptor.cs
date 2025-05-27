using GraphQL;
using GraphQL.Utilities;
using GraphQL.Client.Abstractions.Utilities;
using Castle.DynamicProxy;
using System.Reflection;

namespace GraphQL.Respositories.Interceptors
{
    public abstract class BaseGraphQLInterceptor<T> : IInterceptor
    {
        public abstract T SendQuery(string query, Dictionary<string, object> variables);

        public abstract T SendMutation(string mutation, Dictionary<string, object> variables);

        public abstract object? HandleResult(T result, Type returnType, string key);

        public void Intercept(IInvocation invocation)
        {
            if (invocation != null)
            {
                MethodInfo methodInfo = invocation.Method;
                var returnType = methodInfo.ReturnType;
                var variables = GetVariables(methodInfo.GetParameters(), invocation.Arguments);

                var queryAttribute = methodInfo.GetCustomAttribute<GraphQLQueryAttribute>();
                if (queryAttribute != null)
                {
                    var query = StringUtils.AllowShorthandQuery(queryAttribute.Query);
                    var key = queryAttribute.Key ?? returnType.Name.ToLower();
                    var result = SendQuery(query, variables);
                    invocation.ReturnValue = HandleResult(result, returnType, key);
                }

                var mutationAttribute = methodInfo.GetCustomAttribute<GraphQLMutationAttribute>();
                if (mutationAttribute != null)
                {
                    var mutation = StringUtils.AllowShorthandMutation(mutationAttribute.Mutation);
                    var key = mutationAttribute.Key ?? methodInfo.Name.ToLowerFirst();
                    var result = SendMutation(mutation, variables);
                    invocation.ReturnValue = HandleResult(result, returnType, key);
                }
            }
        }

        private Dictionary<string, object> GetVariables(ParameterInfo[] parameters, object?[] arguments)
        {
            var variables = new Dictionary<string, object>();

            if (parameters != null && parameters.Length > 0 && arguments != null && arguments.Length > 0)
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    var parameter = parameters[i];
                    var parameterName = parameter.Name!;
                    var argument = arguments[i]!;

                    var queryParameterAttribute = parameter.GetCustomAttribute<QueryParameterAttribute>();
                    if (queryParameterAttribute != null)
                    {
                        parameterName = queryParameterAttribute.Name;
                    }

                    variables.Add(parameterName, argument);
                }
            }

            return variables;
        }
    }
}
