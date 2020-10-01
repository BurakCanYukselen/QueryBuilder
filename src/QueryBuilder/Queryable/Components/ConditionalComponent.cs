using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using QueryBuilder.Queryable.Abstracts;
using QueryBuilder.Queryable.Models;

namespace QueryBuilder.Queryable.Components
{
    public class ConditionalComponent : AbstractComponent
    {
        protected BinaryExpression Expression { get; set; }
        protected IEnumerable<ExpressionProperty> ExpressionProperties { get; set; }

        public ConditionalComponent(BinaryExpression expression, IEnumerable<ExpressionProperty> expressionProperties)
        {
            Expression = expression;
            ExpressionProperties = expressionProperties;
        }

        protected override string InternalBuild()
        {
            var claus = GetConditionals(Expression);
            return claus;
        }

        protected string GetConditionals(BinaryExpression expression)
        {
            var left = expression.Left;
            var right = expression.Right;

            var leftSideMemberName = ProcessExpression(left);
            var rightSideMemberName = ProcessExpression(right);
            var operationName = ProcessExpressionType(expression.NodeType);

            return $"({leftSideMemberName} {operationName} {rightSideMemberName})";
        }

        protected string ProcessExpression(Expression expression)
        {
            if (expression is BinaryExpression)
                return GetConditionals((BinaryExpression) expression);
            else if (expression is MemberExpression)
                return GetMemberExpressionName((MemberExpression) expression);
            else if (expression is ConstantExpression)
                return GetConstantExpressionValue((ConstantExpression) expression);
            else if (expression is UnaryExpression)
                return ProcessExpression(((UnaryExpression) expression).Operand);
            else
                throw new InvalidExpressionException();
        }

        protected string ProcessExpressionType(ExpressionType expressionType)
        {
            switch (expressionType)
            {
                case ExpressionType.AndAlso: return "AND";
                case ExpressionType.OrElse: return "OR";
                case ExpressionType.Equal: return "=";
                case ExpressionType.NotEqual: return "!=";
                case ExpressionType.GreaterThan: return ">";
                case ExpressionType.GreaterThanOrEqual: return ">=";
                case ExpressionType.LessThan: return "<";
                case ExpressionType.LessThanOrEqual: return "<=";
                default: throw new InvalidEnumArgumentException();
            }
        }

        protected string GetMemberExpressionName(MemberExpression expression)
        {
            if (expression.Member.DeclaringType == typeof(Tuple))
                return null;
                
            var propertyName = expression.Member.Name;
            var expressionProperty = ExpressionProperties.FirstOrDefault(p => p.HasMatch(expression.ToString(), propertyName));
            if (expressionProperty != null)
                return $"{expressionProperty.Alias}.{propertyName}";
            return $"@{propertyName}";
        }

        protected string GetConstantExpressionValue(ConstantExpression expression)
        {
            var expressionText = expression.ToString();
            var cuoteReplacement = expressionText.Replace('"', '\'');
            return cuoteReplacement;
        }
    }
}