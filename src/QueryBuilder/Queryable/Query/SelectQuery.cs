using System;
using System.Collections.Generic;
using System.Linq;
using QueryBuilder.Extensions;
using QueryBuilder.Queryable.Abstracts;

namespace QueryBuilder.Queryable.Query
{
    public class SelectQuery : AbstractQuery
    {
        public SelectQuery(string tableName, string tableAlias)
        {
            TableName = tableName;
            TableAlias = tableAlias;
            Columns = new List<AbstractComponent>();
            Joins = new List<AbstractComponent>();
        }

        internal int? TopN { get; set; }
        internal List<AbstractComponent> Columns { get; set; }
        internal List<AbstractComponent> Joins { get; set; }
        internal AbstractComponent Condition { get; set; }

        protected string TableName { get; set; }
        protected string TableAlias { get; set; }
        
        public override string Build()
        {
            var topN = GetTopN();
            var columns = GetColumns();
            var joins = GetJoins();
            var conditions = GetConditions();

            var query = GetQuery(topN, columns, joins, conditions);
            return query;
        }

        protected string GetQuery(string topN, string columns, string joins, string conditions)
        {
            var tableNameWithAlias = string.Format(base.Statements.Alias, TableName, TableAlias);
            var statement = string.Format(base.Statements.SelectStatement,
                topN,
                columns,
                tableNameWithAlias,
                joins,
                conditions);
            return statement;
        }

        protected string GetTopN()
        {
            if (TopN != null)
                return string.Join(base.Statements.TopStatement, TopN);
            else
                return null;
        }

        protected string GetConditions()
        {
            if (Condition != null)
                return Condition.Build();
            return null;
        }

        protected string GetColumns()
        {
            if (!Columns.HasChild())
                return "*";
            else
                return string.Join($",{Environment.NewLine}", Columns.Select(p => p.Build()));
        }

        protected string GetJoins()
        {
            string joins = null;
            if (Joins.HasChild())
                joins = string.Join($"{Environment.NewLine}", Joins.Select(p => p.Build()));
            return joins;
        }
    }
}