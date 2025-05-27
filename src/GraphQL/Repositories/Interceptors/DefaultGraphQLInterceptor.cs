using GraphQL.Client.Abstractions;
using Newtonsoft.Json.Linq;

namespace GraphQL.Respositories.Interceptors
{
    public sealed class DefaultGraphQLInterceptor : BaseGraphQLInterceptor<GraphQLResponse<JObject>>
    {
        private IGraphQLClient _Client;

        public DefaultGraphQLInterceptor(IGraphQLClient client)
        {
            _Client = client;
        }

        public override GraphQLResponse<JObject> SendQuery(string query, Dictionary<string, object> variables)
        {
            return Task.Run(async () => await _Client.SendQueryAsync<JObject>(
                new GraphQLRequest { Query = query, Variables = variables })).GetAwaiter().GetResult();
        }

        public override GraphQLResponse<JObject> SendMutation(string query, Dictionary<string, object> variables)
        {
            return Task.Run(async () => await _Client.SendMutationAsync<JObject>(
                new GraphQLRequest { Query = query, Variables = variables })).GetAwaiter().GetResult();
        }

        public override object? HandleResult(GraphQLResponse<JObject> result, Type returnType, string key)
        {
            if (result.Errors == null)
            {
                var data = result.Data;
                if (data != null && data.ContainsKey(key))
                {
                    return data[key]?.ToObject(returnType);
                }
            }
            else if (result.Errors.Length > 0)
            {
                // just throw the first error
                throw new Exception(result.Errors?.First().Message);
            }
            else
            {
                throw new Exception("An unexpected GraphQL.NET exception occurred.");
            }

            return null!;
        }
    }
}
