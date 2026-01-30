using DbManager;
using Xunit;
namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row

        [Fact]
        public void GetValueTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "employee_name"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "age"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "years_Worked"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "salary")
            };
            
            List<string> rowValues = new List<string>()
            {
                "jacinto","37","5","36859.23"
            };
            Row testRow = new Row(columns, rowValues);

            Assert.Equal("jacinto", testRow.GetValue("employee_name"));
            Assert.Equal("37", testRow.GetValue("age"));
            Assert.Equal("5", testRow.GetValue("years_Worked"));
            Assert.Equal("36859.23", testRow.GetValue("salary"));

            Assert.NotEqual("maider", testRow.GetValue("employee_name"));
            Assert.NotEqual("37.5", testRow.GetValue("age"));
            Assert.NotEqual("hola", testRow.GetValue("years_Worked"));
            Assert.NotEqual("46859.23", testRow.GetValue("salary"));

        }
        [Fact]
        public void SetValue()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "name"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "age"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "years_Worked"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "salary")
            };

            List<string> rowValues = new List<string>()
            {
                "jacinto","37","5","36859.23"
            };
            Row testRow = new Row(columns, rowValues);


            testRow.SetValue("name", "maider");
            testRow.SetValue("age", "25");
            testRow.SetValue("years_Worked", "10");
            testRow.SetValue("salary", "25350.00");

            Assert.Equal("maider", testRow.GetValue("name"));
            Assert.Equal("25", testRow.GetValue("age"));
            Assert.Equal("10", testRow.GetValue("years_Worked"));
            Assert.Equal("25350.00", testRow.GetValue("salary"));

            Assert.NotEqual("Maider", testRow.GetValue("name"));
            Assert.NotEqual("37", testRow.GetValue("age"));
            Assert.NotEqual("maider", testRow.GetValue("years_Worked"));
            Assert.NotEqual("36859.23", testRow.GetValue("salary"));
        }

    }
}