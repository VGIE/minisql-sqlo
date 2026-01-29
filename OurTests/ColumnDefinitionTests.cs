using DbManager;

namespace OurTests
{
    public class ColumnDefinitionsTests
    {
        //TODO DEADLINE 1A : Create your own tests for ColumnDefinition
        
        [Fact]
        public void Test1()
        {
            ColumnDefinition cdInt = new ColumnDefinition(ColumnDefinition.DataType.Int, "Age");
            ColumnDefinition cdString = new ColumnDefinition(ColumnDefinition.DataType.String, "Name");
            ColumnDefinition cdDouble = new ColumnDefinition(ColumnDefinition.DataType.Double, "Salary");
            String testEncode = "HolaCarambola";
            Assert.Equal("", Encode(testEncode));
        }
        
    }
}