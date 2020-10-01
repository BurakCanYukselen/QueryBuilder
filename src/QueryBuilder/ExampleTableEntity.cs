using System;
using QueryBuilder.Queryable.Abstracts;

namespace QueryBuilder
{
    public class ExampleTableEntity : ISQLQueryable
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Ratio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTimeKind DateTimeKind { get; set; }
    }

    public class ExampleJoinTableEntity : ISQLQueryable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public decimal Ratio { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTimeKind DateTimeKind { get; set; }
    }

}