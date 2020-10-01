using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using QueryBuilder.Queryable.Abstracts;

namespace QueryBuilder.Queryable.Components
{
    public class ColumnComponent<TEntity> : AbstractComponent
    {
        public string Alias { get; set; }
        public Expression<Func<TEntity, object>>[] Selectors { get; set; }

        public ColumnComponent(Expression<Func<TEntity, object>>[] selectors)
        {
            Alias = typeof(TEntity).Name;
            Selectors = selectors;
        }
        
        public ColumnComponent(string alias,Expression<Func<TEntity, object>>[] selectors)
        {
            Alias = alias;
            Selectors = selectors;
        }

        protected override string InternalBuild()
        {
            var columns = MergeColumns(Selectors);
            return columns;
        }

        protected string MergeColumns(IEnumerable<Expression> selectors)
        {
            var columnsList = selectors.Select(p => $"{Alias}.{ProcessExpression(((LambdaExpression)p).Body)}");
            var mergedColumns = string.Join(",", columnsList);
            return mergedColumns;
        }

        protected string ProcessExpression(Expression expression)
        {
            if (expression is UnaryExpression)
                return ProcessExpression(((UnaryExpression) expression).Operand);
            else if (expression is MemberExpression)
                return GetMemberName((MemberExpression) expression);
            else
                throw new InvalidExpressionException();
        }

        protected string GetMemberName(MemberExpression expression)
        {
            var memberName = expression.Member.Name;
            return memberName;
        }
    }
}