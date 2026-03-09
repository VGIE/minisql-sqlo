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
            List<SetValue> valuesToTest=new List<SetValue>();
            valuesToTest.Add(new SetValue("column1","1"));
            valuesToTest.Add(new SetValue("column2", "2"));
            Assert.Equal(new Update("tabla",valuesToTest,new Condition("columna", "=", "valor")),
               MiniSQLParser.Parse("UPDATE tabla SET column1=1,column2=2 WHERE columna=valor"));
        }

    }
}