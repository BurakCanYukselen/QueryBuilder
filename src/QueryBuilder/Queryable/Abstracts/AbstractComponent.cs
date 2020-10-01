using QueryBuilder.Builders;
using QueryBuilder.Builders.MsSql;

namespace QueryBuilder.Queryable.Abstracts
{
    public abstract class AbstractComponent
    {
        public IStatements Statements { get; set; } = new MsSqlStatements();

        protected abstract string InternalBuild();

        public string Build()
        {
            var _statement = InternalBuild();
            return _statement;
        }
    }
}