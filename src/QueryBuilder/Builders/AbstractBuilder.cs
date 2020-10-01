using QueryBuilder.Queryable;
using QueryBuilder.Queryable.Abstracts;

namespace QueryBuilder.Builders
{
    public abstract class AbstractBuilder : IBuilder
    {
        protected abstract IStatements Statements { get; }
        protected abstract AbstractComponent Component { get; }

        public abstract string Build();
    }
}