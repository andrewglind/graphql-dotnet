namespace GraphQL
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GraphQLQueryAttribute : Attribute
    {
        public string Query { get; set; }
        public string Key { get; set; }

        public GraphQLQueryAttribute(string query)
        {
            Query = query;
        }
    }
}
