namespace QueryBuilder.Queryable.Models
{
    public class ExpressionProperty
    {
        public string Name { get; set; }
        public string Alias { get; set; }

        public ExpressionProperty(string name, string alias)
        {
            Name = name;
            Alias = alias;
        }

        public bool HasMatch(string expression, string propertyName) => expression == $"{Name}.{propertyName}";
    }
}