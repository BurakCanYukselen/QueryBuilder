using System;
using System.IO;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using QueryBuilder.Queryable.Abstracts;
using QueryBuilder.Queryable.Components;
using QueryBuilder.Queryable.Models;
using QueryBuilder.Queryable.Query;

namespace QueryBuilder.Extensions
{
    public static class SelectQueryExtensions
    {
        public static SelectQuery QueryTableAs(this ISQLQueryable entity, string tableName)
        {
            var query = new SelectQuery(tableName, entity.GetType().Name);
            return query;
        }

        public static SelectQuery QueryTableAs(this ISQLQueryable entity, string tableName, string alias)
        {
            var query = new SelectQuery(tableName, alias);
            return query;
        }

        public static SelectQuery Columns<TEntity>(this SelectQuery query, params Expression<Func<TEntity, object>>[] selectors)
        {
            var columnComponent = new ColumnComponent<TEntity>(selectors);
            query.Columns.Add(columnComponent);
            return query;
        }

        public static SelectQuery Columns<TEntity>(this SelectQuery query, string alias, params Expression<Func<TEntity, object>>[] selectors)
        {
            var columnComponent = new ColumnComponent<TEntity>(alias, selectors);
            query.Columns.Add(columnComponent);
            return query;
        }

        public static SelectQuery Join<TJoin, TSelect>(this SelectQuery query, JoinTypes joinTypes, string tableName, Expression<Func<TJoin, TSelect, bool>> selectors)
        {
            var joinComponent = new JoinComponent<TJoin, TSelect>(joinTypes, tableName, selectors);
            query.Joins.Add(joinComponent);
            return query;
        }

        public static SelectQuery Join<TJoin, TSelect>(this SelectQuery query, JoinTypes joinTypes, string tableName, string joinAlias, Expression<Func<TJoin, TSelect, bool>> selectors)
        {
            var joinComponent = new JoinComponent<TJoin, TSelect>(joinTypes, tableName, joinAlias, selectors);
            query.Joins.Add(joinComponent);
            return query;
        }

        public static SelectQuery Join<TJoin, TSelect>(this SelectQuery query, JoinTypes joinTypes, string tableName, string joinAlias, string selectAlias, Expression<Func<TJoin, TSelect, bool>> selectors)
        {
            var joinComponent = new JoinComponent<TJoin, TSelect>(joinTypes, tableName, joinAlias, selectAlias, selectors);
            query.Joins.Add(joinComponent);
            return query;
        }

        public static SelectQuery Where<TEntity>(this SelectQuery query, Expression<Func<TEntity, bool>> selector)
        {
            query.Condition = new WhereComponent<TEntity>(selector);
            return query;
        }

        public static SelectQuery Where<TEntity>(this SelectQuery query, string alias, Expression<Func<TEntity, bool>> selector)
        {
            if (typeof(ITuple).IsAssignableFrom(typeof(TEntity)))
                throw new InvalidDataException("");

            query.Condition = new WhereComponent<TEntity>(alias, selector);
            return query;
        }

        public static SelectQuery Where<TEntity>(this SelectQuery query, ITuple aliases, Expression<Func<TEntity, bool>> selector)
        {
            query.Condition = new WhereComponent<TEntity>(aliases, selector);
            return query;
        }
    }
}