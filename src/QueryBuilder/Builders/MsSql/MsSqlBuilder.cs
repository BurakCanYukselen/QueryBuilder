using QueryBuilder.Queryable;
using QueryBuilder.Queryable.Abstracts;

namespace QueryBuilder.Builders.MsSql
{
    public class MsSqlBuilder: AbstractBuilder
    {
        protected override IStatements Statements { get; } = new MsSqlStatements();
        protected override AbstractComponent Component { get; }
        
        public override string Build()
        {
            return Component.Build();
        }
    }
}