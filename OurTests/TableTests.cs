using DbManager;

namespace OurTests
{
    public class TableTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table

        [Fact]
        public void constructorTest1()
        {
            var columns = new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero")
         };

            Table table = new Table("Personas", columns);

            Assert.Equal(2, columns.Count);
            Assert.Equal("Nombre", columns[0].Name);
            Assert.Equal(ColumnDefinition.DataType.String, columns[0].Type);
            Assert.Equal("Numero", columns[1].Name);
            Assert.Equal(ColumnDefinition.DataType.Int, columns[1].Type);

        }

        [Fact]
        public void TableSelectWithoutConditionTest() 
        {
            var columns = new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero")
         };

            Table table = new Table("Personas", columns);
            table.Select(columns, );
        }
    }

}
