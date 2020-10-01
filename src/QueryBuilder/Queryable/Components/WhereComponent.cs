using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using QueryBuilder.Extensions;
using QueryBuilder.Queryable.Abstracts;
using QueryBuilder.Queryable.Models;

namespace QueryBuilder.Queryable.Components
{
    public class WhereComponent<TEntity> : AbstractComponent
    {
        protected ConditionalComponent ConditionalComponent { get; set; }

        public WhereComponent(Expression<Func<TEntity, bool>> selector)
        {
            IEnumerable<ExpressionProperty> expressionProperties;
            if (typeof(ITuple).IsAssignableFrom(typeof(TEntity)))
                expressionProperties = selector.Parameters[0].ToExpressionPropertyOfTuple();
            else
                expressionProperties = new[] {selector.Parameters[0].ToExpressionProperty()};
            ConditionalComponent = new ConditionalComponent((BinaryExpression) selector.Body, expressionProperties);
        }

        public WhereComponent(string alias, Expression<Func<TEntity, bool>> selector)
        {
            if (typeof(ITuple).IsAssignableFrom(typeof(TEntity)))
                throw new InvalidOperationException("Use TupleType Aliases with TupleType Expression");

            var expressionProperties = new[] {selector.Parameters[0].ToExpressionProperty(alias)};
            ConditionalComponent = new ConditionalComponent((BinaryExpression) selector.Body, expressionProperties);
        }

        public WhereComponent(ITuple alias, Expression<Func<TEntity, bool>> selector)
        {
            var aliases = alias.GetType().GetFields();
            var aliasesValues = aliases.Select(p => (string) p.GetValue(alias)).ToArray();

            IEnumerable<ExpressionProperty> expressionProperties;
            if (typeof(ITuple).IsAssignableFrom(typeof(TEntity)))
                expressionProperties = selector.Parameters[0].ToExpressionPropertyOfTuple(aliasesValues);
            else
                expressionProperties = new[] {selector.Parameters[0].ToExpressionProperty()};
            ConditionalComponent = new ConditionalComponent((BinaryExpression) selector.Body, expressionProperties);
        }

        protected override string InternalBuild()
        {
            var conditionals = ConditionalComponent.Build();
            var claus = string.Format(base.Statements.WhereStatement, conditionals);
            return claus;
        }
    }
}