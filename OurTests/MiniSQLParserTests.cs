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
        public void SelectTests()
        {

            //varias columnas y condición
            List<String> columns = new List<string> {"Column1","Column2" };
            Condition condition = new Condition("Column1", "=", "Hola");

            Assert.Equal(new Select("tabla", columns, condition), 
                MiniSQLParser.Parse("SELECT Column1,Column2 FROM tabla WHERE Column1 = 'Hola'"));

            //UNA columna y SIN condición

            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, null),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla"));

            //Condicion con número entero
            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, new Condition("Column1", "=", "42")),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla WHERE Column1 = '42'"));

            //Condicion con número decimal
            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, new Condition("Column1", ">", "3.14")),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla WHERE Column1 > '3.14'"));

            //Condicion con número negativo
            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, new Condition("Column1", "<", "-5")),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla WHERE Column1 < '-5'"));

            // Querys malas
            Assert.Null(MiniSQLParser.Parse("SELECT FROM tabla"));
            Assert.Null(MiniSQLParser.Parse("INSERT INTO tabla VALUES (1)"));
            Assert.Null(MiniSQLParser.Parse("SELECT col1 col2 FROM tabla"));
        }


    }
}