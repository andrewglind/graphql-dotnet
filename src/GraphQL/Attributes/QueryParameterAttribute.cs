namespace GraphQL
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public sealed class QueryParameterAttribute : Attribute
    {
        public readonly string Name;

        public QueryParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
