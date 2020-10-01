using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using QueryBuilder.Extensions;
using QueryBuilder.Queryable.Abstracts;
using QueryBuilder.Queryable.Models;

namespace QueryBuilder.Queryable.Components
{
    public class JoinComponent<TJoin, TSelect> : AbstractComponent
    {
        protected string TableName { get; set; }
        protected string JoinAlias { get; set; }
        protected ConditionalComponent ConditionalComponent { get; set; }
        protected JoinTypes JoinTypes { get; set; }

        public JoinComponent(JoinTypes joinTypes, string tableName, Expression<Func<TJoin, TSelect, bool>> selectors)
        {
            JoinAlias = typeof(TJoin).Name;
            TableName = tableName;
            JoinTypes = joinTypes;

            var expressionJoinObjectProperties = selectors.Parameters[0].ToExpressionProperty();
            var expressionSelectObjectProperties = selectors.Parameters[1].ToExpressionProperty();
            ConditionalComponent = new ConditionalComponent((BinaryExpression) selectors.Body, new[] {expressionJoinObjectProperties, expressionSelectObjectProperties});
        }

        public JoinComponent(JoinTypes joinTypes, string tableName, string joinAlias, Expression<Func<TJoin, TSelect, bool>> selectors)
        {
            JoinAlias = joinAlias;
            TableName = tableName;
            JoinTypes = joinTypes;

            var expressionJoinObjectProperties = selectors.Parameters[0].ToExpressionProperty(joinAlias);
            var expressionSelectObjectProperties = selectors.Parameters[1].ToExpressionProperty();
            ConditionalComponent = new ConditionalComponent((BinaryExpression) selectors.Body, new[] {expressionJoinObjectProperties, expressionSelectObjectProperties});
        }

        public JoinComponent(JoinTypes joinTypes, string tableName, string joinAlias, string selectAlias, Expression<Func<TJoin, TSelect, bool>> selectors)
        {
            JoinAlias = joinAlias;
            TableName = tableName;
            JoinTypes = joinTypes;

            var expressionJoinObjectProperties = selectors.Parameters[0].ToExpressionProperty(joinAlias);
            var expressionSelectObjectProperties = selectors.Parameters[1].ToExpressionProperty(selectAlias);
            ConditionalComponent = new ConditionalComponent((BinaryExpression) selectors.Body, new[] {expressionJoinObjectProperties, expressionSelectObjectProperties});
        }

        protected override string InternalBuild()
        {
            var conditionals = ConditionalComponent.Build();
            var joinTableWithAlias = string.Format(base.Statements.Alias, TableName, JoinAlias);
            var claus = string.Format(GetJoinStatement(), joinTableWithAlias, conditionals);
            return claus;
        }

        protected string GetJoinStatement()
        {
            switch (JoinTypes)
            {
                case JoinTypes.Left: return base.Statements.LeftJoinStatement;
                case JoinTypes.Outer: return base.Statements.OuterJoinStatement;
                case JoinTypes.FullOuter: return base.Statements.FullOuterJoinStatement;
                case JoinTypes.Inner: return base.Statements.InnerJoinStatement;
                default: throw new InvalidEnumArgumentException();
            }
        }
    }
}