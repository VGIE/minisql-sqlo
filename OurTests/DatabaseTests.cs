using DbManager;
using System.ComponentModel.DataAnnotations;

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
        [Fact]
        public void CreateTableTest()
        {
            Database db = new Database("paco", "1234");
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            List<ColumnDefinition> columnsVacio = new List<ColumnDefinition>();
            ColumnDefinition c1 = new ColumnDefinition(ColumnDefinition.DataType.String,"nombre");
            ColumnDefinition c2 = new ColumnDefinition(ColumnDefinition.DataType.Int, "edad");
            ColumnDefinition c3 = new ColumnDefinition(ColumnDefinition.DataType.Double, "peso");
            columns.Add(c1);
            columns.Add(c2);
            columns.Add(c3);
            Assert.False(db);
        }
        
    }
}