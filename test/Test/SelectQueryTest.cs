using QueryBuilder;
using QueryBuilder.Extensions;
using QueryBuilder.Queryable.Models;
using Xunit;
using Xunit.Abstractions;

namespace Test
{
    public class SelectQueryTest
    {
        private ExampleTableEntity _exampleTableEntity = new ExampleTableEntity();
        private ExampleJoinTableEntity _exampleJoinTableEntity = new ExampleJoinTableEntity();
        private ITestOutputHelper _outputHelper;

        public SelectQueryTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public void Select_AllColumns_WithoutJoin_WithoutCondition()
        {
            var query = _exampleTableEntity
                .QueryTableAs("Table1")
                .Build();

            _outputHelper.WriteLine(query);
        }

        [Fact]
        public void Select_CustomColumns_WithoutJoin_WithoutCondition()
        {
            var query = _exampleTableEntity
                .QueryTableAs("Table1")
                .Columns<ExampleTableEntity>(p => p.Id, p => p.Amount, p => p.Ratio)
                .Build();

            _outputHelper.WriteLine(query);
        }

        [Fact]
        public void Select_AllColumns_WithOneJoin_WithoutCondition()
        {
            var query = _exampleTableEntity
                .QueryTableAs("Table1")
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Left, "Table2", (j, s) => j.Id == s.Id)
                .Build();

            _outputHelper.WriteLine(query);
        }

        [Fact]
        public void Select_AllColumns_WithOneJoinWithConstant_WithoutCondition()
        {
            var query = _exampleTableEntity
                .QueryTableAs("Table1")
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Left, "Table2", (j, s) => j.Id == s.Id && j.Amount > 5)
                .Build();

            _outputHelper.WriteLine(query);
        }

        [Fact]
        public void Select_AllColumns_WithOneJoinWithParameter_WithoutCondition()
        {
            var amount = 5;
            var query = _exampleTableEntity
                .QueryTableAs("Table1")
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Left, "Table2", (j, s) => j.Id == s.Id && j.Amount > amount)
                .Build();

            _outputHelper.WriteLine(query);
        }

        [Fact]
        public void Select_AllColumns_WithMultiJoin_WithoutCondition()
        {
            var query = _exampleTableEntity
                .QueryTableAs("Table1", "childTable1")
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Left, "Table2", "childTable2", "childTable1", (j, s) => j.Id == s.Id)
                .Join<ExampleTableEntity, ExampleTableEntity>(JoinTypes.Inner, "Table1", "parentTable1", "childTable1", (j, s) => j.Id == s.ParentId)
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Inner, "Table2", "parentTable2", "childTable2", (j, s) => j.Id == s.ParentId)
                .Build();

            _outputHelper.WriteLine(query);
        }

        [Fact]
        public void Select_OneTableColumns_WithMultiJoin_WithoutCondition()
        {
            var query = _exampleTableEntity
                .QueryTableAs("Table1", "childTable1")
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Left, "Table2", "childTable2", "childTable1", (j, s) => j.Id == s.Id)
                .Join<ExampleTableEntity, ExampleTableEntity>(JoinTypes.Inner, "Table1", "parentTable1", "childTable1", (j, s) => j.Id == s.ParentId)
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Inner, "Table2", "parentTable2", "childTable2", (j, s) => j.Id == s.ParentId)
                .Columns<ExampleTableEntity>("childTable1", p => p.Id, p => p.Amount)
                .Build();

            _outputHelper.WriteLine(query);
        }

        [Fact]
        public void Select_MultiTableColumns_WithMultiJoin_WithoutCondition()
        {
            var query = _exampleTableEntity
                .QueryTableAs("Table1", "childTable1")
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Left, "Table2", "childTable2", "childTable1", (j, s) => j.Id == s.Id)
                .Join<ExampleTableEntity, ExampleTableEntity>(JoinTypes.Inner, "Table1", "parentTable1", "childTable1", (j, s) => j.Id == s.ParentId)
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Inner, "Table2", "parentTable2", "childTable2", (j, s) => j.Id == s.ParentId)
                .Columns<ExampleTableEntity>("childTable1", p => p.Id, p => p.Amount)
                .Columns<ExampleJoinTableEntity>("childTable2", p => p.Id, p => p.CreatedAt)
                .Build();

            _outputHelper.WriteLine(query);
        }

        [Fact]
        public void Select_AllColumns_WithMultiJoin_WithOneTableCondition()
        {
            var query = _exampleTableEntity
                .QueryTableAs("Table1", "childTable1")
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Left, "Table2", "childTable2", "childTable1", (j, s) => j.Id == s.Id)
                .Join<ExampleTableEntity, ExampleTableEntity>(JoinTypes.Inner, "Table1", "parentTable1", "childTable1", (j, s) => j.Id == s.ParentId)
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Inner, "Table2", "parentTable2", "childTable2", (j, s) => j.Id == s.ParentId)
                .Where<ExampleTableEntity>("Table1", p => p.Amount > 5 && p.Name == "Test")
                .Build();

            _outputHelper.WriteLine(query);
        }

        [Fact]
        public void Select_AllColumns_WithMultiJoin_WithMultiTableCondition()
        {
            var query = _exampleTableEntity
                .QueryTableAs("Table1", "childTable1")
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Left, "Table2", "childTable2", "childTable1", (j, s) => j.Id == s.Id)
                .Join<ExampleTableEntity, ExampleTableEntity>(JoinTypes.Inner, "Table1", "parentTable1", "childTable1", (j, s) => j.Id == s.ParentId)
                .Join<ExampleJoinTableEntity, ExampleTableEntity>(JoinTypes.Inner, "Table2", "parentTable2", "childTable2", (j, s) => j.Id == s.ParentId)
                .Where<(ExampleTableEntity, ExampleJoinTableEntity)>(("childTable1","parentTable2"),p => p.Item1.Amount > 5 && p.Item2.Name == "Test")
                .Build();

            _outputHelper.WriteLine(query);
        }
    }
}