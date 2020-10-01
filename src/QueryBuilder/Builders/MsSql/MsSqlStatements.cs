using System;

namespace QueryBuilder.Builders.MsSql
{
    public class MsSqlStatements: IStatements
    {
        public string SelectStatement => $"SELECT {{0}} {Environment.NewLine}{{1}}{Environment.NewLine}FROM {{2}} {Environment.NewLine}{{3}}{Environment.NewLine}{{4}}";
        public string InsertStatement => $"INSERT INTO {{0}} {Environment.NewLine}{{1}}{Environment.NewLine} VALUES {Environment.NewLine}({{2}})";
        public string DeleteStatement => $"DELETE FROM {{0}}";
        public string UpdateStatement => "UPDATE {0} SET";
        public string WhereStatement => "WHERE {0}";
        public string LeftJoinStatement => "LEFT JOIN {0} ON {1}";
        public string InnerJoinStatement => "INNER JOIN {0} ON {1}";
        public string OuterJoinStatement => "OUTER JOIN {0} ON {1}";
        public string FullOuterJoinStatement => "FULL OUTER JOIN {0} ON {1}";
        public string TopStatement => "TOP {0}";
        public string OrderByStatement => "ORDER BY {0}";
        public string GroupByStatement => "GROUP BY {0}";
        public string Nolock => "{0} (NOLOCK)";
        public string Alias => "{0} AS {1}";
    }
}