using DbManager;

namespace OurTests
{
    public class ColumnDefinitionsTests
    {
        //TODO DEADLINE 1A : Create your own tests for ColumnDefinition
        
        [Fact]
        public void Test1()
        {
            ColumnDefinition column = new ColumnDefinition(ColumnDefinition.DataType.String, "Name");
            string expected = "Name->String";
            string result = column.AsText();
            Assert.Equal(expected, result);

            column = new ColumnDefinition(ColumnDefinition.DataType.Double, "Total->Price");
            expected = "Total[ARROW]Price->Double";
            result = column.AsText();
            Assert.Equal(expected, result);

            
            string input = "Salary->Double";
            ColumnDefinition result1 = ColumnDefinition.Parse(input);
            Assert.Equal("Salary", result1.Name);
            Assert.Equal(ColumnDefinition.DataType.Double, result1.Type);
        }
        
    }
}