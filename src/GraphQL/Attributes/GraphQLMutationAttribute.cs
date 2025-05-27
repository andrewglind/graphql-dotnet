namespace GraphQL
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class GraphQLMutationAttribute : Attribute
    {
        public string Mutation { get; set; }
        public string Key { get; set; }

        public GraphQLMutationAttribute(string mutation)
        {
            Mutation = mutation;
        }
    }
}
