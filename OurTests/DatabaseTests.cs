using DbManager;

namespace OurTests
{
    public class UnitTest1
    {
        //TODO DEADLINE 1B : Create your own tests for Database
        
        [Fact]
        public void AddTableAndTableByNameTest()
        {
            Database db=new Database("paco","1234");
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            db.AddTable(new Table("fechas",columns));
            Assert.NotNull(db.TableByName("fechas"));
            Assert.Null(db.TableByName("adios"));
        }
        
    }
}