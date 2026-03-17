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
            List<string> valores2 = new List<string>{"valor3", "valor4"};
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "columna1"));
            columnas.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "columna2"));
            Table table = new Table("tabla", columnas);
            Table table2 = new Table("alumnos", columnas);

            Insert resultado = (Insert)MiniSQLParser.Parse("INSERT INTO tabla VALUES (valor1, valor2)");
            Insert resultado2 = (Insert)MiniSQLParser.Parse("INSERT  INTO   tabla   VALUES  (  valor1  ,  valor2   )");
            Insert resultado3 = (Insert)MiniSQLParser.Parse("INSERT INTO alumnos VALUES ('valor3', 'valor4')");

            Assert.Equal("tabla", resultado.Table);
            Assert.Equal(valores, resultado.Values);
            Assert.Equal("tabla", resultado2.Table);
            Assert.Equal(valores, resultado2.Values);
            Assert.Equal("alumnos", resultado3.Table);
            Assert.Equal(valores2, resultado3.Values);
        }

    }
}