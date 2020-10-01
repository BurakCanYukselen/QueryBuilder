using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using QueryBuilder.Queryable.Models;

namespace QueryBuilder.Extensions
{
    public static class ParameterExpressionExtension
    {
        public static ExpressionProperty ToExpressionProperty(this ParameterExpression expression)
        {
            return new ExpressionProperty(expression.Name, expression.Type.Name);
        }

        public static ExpressionProperty ToExpressionProperty(this ParameterExpression expression, string alias)
        {
            return new ExpressionProperty(expression.Name, alias);
        }

        public static IEnumerable<ExpressionProperty> ToExpressionPropertyOfTuple(this ParameterExpression expression)
        {
            var fields = expression.Type.GetFields();
            var expressionProperties = new List<ExpressionProperty>();
            foreach (var field in fields)
            {
                if (field.FieldType.Name.Contains(typeof(ValueTuple).Name))
                    throw new InvalidExpressionException();

                expressionProperties.Add(new ExpressionProperty(
                    $"{expression}.{field.Name}",
                    field.FieldType.Name
                ));
            }

            return expressionProperties;
        }
        
        public static IEnumerable<ExpressionProperty> ToExpressionPropertyOfTuple(this ParameterExpression expression, string[] aliases)
        {
            var fields = expression.Type.GetFields();
            if(fields.Length != aliases.Length)
                throw new ArgumentOutOfRangeException(message: "Field and alias count not match", null);
            
            var fieldsWithAliases = fields.Join(aliases, p => Array.IndexOf(fields, p), s => Array.IndexOf(aliases, s), (p, s) => new {field = p, alias = s});

            var expressionProperties = new List<ExpressionProperty>();
            foreach (var fieldWithAlias in fieldsWithAliases)
            {
                if (fieldWithAlias.field.FieldType.Name.Contains(typeof(ValueTuple).Name))
                    throw new InvalidExpressionException();

                expressionProperties.Add(new ExpressionProperty(
                    $"{expression}.{fieldWithAlias.field.Name}",
                    fieldWithAlias.alias
                ));
            }

            return expressionProperties;
        }
    }
}