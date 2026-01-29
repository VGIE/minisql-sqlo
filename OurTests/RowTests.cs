using Xunit;
namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row
        
        [Fact]
        public void Test1()
        {
            List<columnsDefinition> columns = new List<columnsDefinition>()
            {
                new ColumnDefinition(columnsDefinition.DataType.String, "name");
                new ColumnDefinition(columnsDefinition.DataType.String, "age");
            }
            List<string> rowValues = new List<string>()
            {
                "jacinto","37"
            };
            Row testRow = new Row(columns, rowValues);

            Assert.Equal("jacinto", testRow.GetValue("name"));
            Assert.Equal("37", testRow.GetValue("age"));

            testRow.SetValue("name", "maider");

            Assert.Equal("maider", testRow.GetValue("name"));
            Assert.Null(testRow.GetValue("nombre"));
        }
        
    }
}