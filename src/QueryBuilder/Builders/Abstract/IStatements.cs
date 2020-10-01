namespace QueryBuilder.Builders
{
    public interface IStatements
    {
        public string SelectStatement { get; }
        public string InsertStatement { get; }
        public string DeleteStatement { get; }
        public string UpdateStatement { get; }
        public string WhereStatement { get; }
        public string LeftJoinStatement { get; }
        public string InnerJoinStatement { get; }
        public string OuterJoinStatement { get; }
        public string FullOuterJoinStatement { get; }
        public string TopStatement { get; }
        public string OrderByStatement { get; }
        public string GroupByStatement { get; }
        public string Nolock { get; }
        public string Alias { get; }
    }
}