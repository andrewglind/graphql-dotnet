namespace GraphQL
{
    [AttributeUsage(AttributeTargets.Interface)]
    public sealed class GraphQLRepositoryAttribute : Attribute
    {
        public string Description { get; set; }

        public GraphQLRepositoryAttribute(string description)
        {
            Description = description;
        }
    }
}
