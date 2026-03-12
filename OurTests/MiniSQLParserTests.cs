using DbManager;
using DbManager.Parser;
using Xunit;
namespace OurTests
{
    public class MiniSQLParserTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row

        [Fact]
        public void DeleteTests()
        {
            /*
            Assert.Equal(new Delete("tabla", new Condition("columna", "=", "valor")), 
                MiniSQLParser.Parse("DELETE FROM tabla WHERE columna = valor"));
            */
        }
        [Fact]
        public void UpdateTests()
        {
            Update result = (Update)MiniSQLParser.Parse("UPDATE tabla SET column1=1,column2=2 WHERE columna=valor");
            Assert.Equal("tabla",result.Table);


            Assert.Equal("columna", result.Where.ColumnName);
            Assert.Equal("=", result.Where.Operator);
            Assert.Equal("valor", result.Where.LiteralValue);

 
            Assert.Equal(2, result.Columns.Count);
            Assert.Equal("column1", result.Columns[0].ColumnName);
            Assert.Equal("1", result.Columns[0].Value);
            Assert.Equal("column2", result.Columns[1].ColumnName);
            Assert.Equal("2", result.Columns[1].Value);
        }

    }
}