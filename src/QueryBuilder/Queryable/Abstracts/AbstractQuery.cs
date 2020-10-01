using QueryBuilder.Builders;
using QueryBuilder.Builders.MsSql;

namespace QueryBuilder.Queryable.Abstracts
{
    public abstract class AbstractQuery
    {
        internal IStatements Statements { get; set; } = new MsSqlStatements();
        public abstract string Build();
    }
}