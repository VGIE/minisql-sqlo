using DbManager;
using DbManager.Parser;
using Xunit;
namespace OurTests
{
    public class MiniSQLParserTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row

        /*[Fact]
        public void DeleteTests()
        {
            
            Assert.Equal(new Delete("tabla", new Condition("columna", "=", "valor")), 
                MiniSQLParser.Parse("DELETE FROM tabla WHERE columna = valor"));
            
        }*/

        [Fact]
        public void InsertTests()
        {
            List<string> valores = new List<string>{"valor1", "valor2"};
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "columna1"));
            columnas.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "columna2"));
            Table table = new Table("tabla", columnas);

            Insert resultado = (Insert)MiniSQLParser.Parse("INSERT INTO tabla VALUES (valor1, valor2)");

            Assert.Equal("tabla", resultado.Table);
            Assert.Equal(valores, resultado.Values);
        }

    }
}