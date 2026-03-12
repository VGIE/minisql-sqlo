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
            
            Delete deleteTest = new Delete("table", new Condition("edad","=","1"));
            Delete deleteTest2 = new Delete("tableForDelete1", null);
            Delete deleteTest3 = new Delete("employee", new Condition("age", ">", "-5124.2456"));
            Delete deleteTest4 = new Delete("testTable", new Condition("column","=","string"));

            //For delete object created
            Assert.Equal(deleteTest,MiniSQLParser.Parse("DELETE FROM table WHERE edad='1'"));
            Assert.NotEqual(deleteTest, MiniSQLParser.Parse("DELETE FROM table"));
            Assert.Equal(deleteTest2, MiniSQLParser.Parse("DELETE FROM tableForDelete1"));
            Assert.NotEqual(deleteTest2, MiniSQLParser.Parse("Delete From tableForDelete1"));
            Assert.Equal(deleteTest3, MiniSQLParser.Parse("DELETE FROM employee WHERE age>'-5124.2456'"));
            Assert.NotEqual(deleteTest3, MiniSQLParser.Parse("DELETE FROM employee WHERE employee>'-5124.2456'"));
            Assert.Equal(deleteTest4, MiniSQLParser.Parse("DELETE   FROM testTable     WHERE column='string'"));
            Assert.Equal(deleteTest4, MiniSQLParser.Parse("DELETE FROM testTable WHERE column='string'"));

            //For regex comprobation
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table one"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table@"));
            Assert.Null(MiniSQLParser.Parse("Delete From table"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table1 WHERE name=Jacinto"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table WHERE age ='-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table WHERE age>= '-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table WHERE age>'-32.6123' "));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table WHERE age >'-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM  table WHERE age>='-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table  WHERE age>-32.6123"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table '"));




        }

        [Fact]
        public void SelectTests()
        {

            //varias columnas y condición
            List<String> columns = new List<string> {"Column1","Column2" };
            Condition condition = new Condition("Column1", "=", "Hola");

            Assert.Equal(new Select("tabla", columns, condition), 
                MiniSQLParser.Parse("SELECT Column1,Column2 FROM tabla WHERE Column1='Hola'"));

            //UNA columna y SIN condición

            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, null),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla"));

            //Condicion con número entero
            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, new Condition("Column1", "=", "42")),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla WHERE Column1='42'"));

            //Condicion con número decimal
            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, new Condition("Column1", ">", "3.14")),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla WHERE Column1>'3.14'"));

            //Condicion con número negativo
            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, new Condition("Column1", "<", "-5")),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla WHERE Column1<'-5'"));

            // Querys malas
            Assert.Null(MiniSQLParser.Parse("SELECT FROM tabla"));
            Assert.Null(MiniSQLParser.Parse("INSERT INTO tabla VALUES (1)"));
            Assert.Null(MiniSQLParser.Parse("SELECT col1 col2 FROM tabla"));
        }

        [Fact]

        public void CreateTableTest()
        {
            CreateTable table = (new CreateTable("Table", new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Altura")
            }));

            Assert.Equal(table, MiniSQLParser.Parse("CREATE TABLE Table (Nombre TEXT,Edad INT,Altura DOUBLE)"));

            Assert.Null(MiniSQLParser.Parse("CREATE TABLE table"));
            Assert.Null(MiniSQLParser.Parse("CREATE TABLE table (nota int)"));
            Assert.Null(MiniSQLParser.Parse("CREATE TABLE table (nota INT nombre TEXT"));

        }
        [Fact]
        public void DropTableTest()
        {
            Assert.Equal(new DropTable("Test1"), MiniSQLParser.Parse("DROP TABLE Test1"));
            Assert.Null(MiniSQLParser.Parse("DROP table Test2"));
            Assert.Null(MiniSQLParser.Parse("DROP TABLE Test2, Test 3"));
            Assert.Equal(new DropTable("Test4"),MiniSQLParser.Parse("DROP TABLE      Test4"));

        }

    }
}