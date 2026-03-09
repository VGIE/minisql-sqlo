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
            Delete deleteTest3 = new Delete("employee", new Condition("age", ">=", "'-5124.2456'"));

            //For delete object created
            Assert.Equal(deleteTest,MiniSQLParser.Parse("DELETE FROM table WHERE edad='1'"));
            Assert.NotEqual(deleteTest, MiniSQLParser.Parse("DELETE FROM table"));
            Assert.Equal(deleteTest2, MiniSQLParser.Parse("DELETE FROM tableForDelete1"));
            Assert.NotEqual(deleteTest2, MiniSQLParser.Parse("Delete From tableForDelete1"));
            Assert.Equal(deleteTest3, MiniSQLParser.Parse("DELETE FROM employee WHERE employee>='-5124.2456'"));
            Assert.NotEqual(deleteTest3, MiniSQLParser.Parse("DELETE FROM employee WHERE employee>'-5124.2456'"));

            //For regex comprobation
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table one"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table@"));
            Assert.Null(MiniSQLParser.Parse("Delete From table"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table1 WHERE name=Jacinto"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table WHERE age >='-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table WHERE age>= '-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table WHERE age>='-32.6123' "));
            Assert.Null(MiniSQLParser.Parse("DELETE  FROM table WHERE age >='-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM  table WHERE age>='-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table  WHERE age >='-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table '"));



        }

    }
}