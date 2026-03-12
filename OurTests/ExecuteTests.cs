using DbManager;
using DbManager.Parser;
using Xunit;
namespace OurTests
{
    public class ExecuteTests
    {
        /*
        [Fact]
        public void DeleteTests()
        {
            
            
        }
        */
        /*
        [Fact]
        public void SelectTests()
        {
            Database database = Database.CreateTestDatabase();
            Select select = 
        }   
        */
        [Fact]
        public void CreateTableTest()
        {
            Database database = Database.CreateTestDatabase();
            List<ColumnDefinition> columns = new List<ColumnDefinition>
    {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad")
    };
            CreateTable createTable = new CreateTable("Tabla", columns);

            Assert.Equal(Constants.CreateTableSuccess, createTable.Execute(database));

            Assert.Equal(Constants.TableAlreadyExistsError, createTable.Execute(database));


        }
        /*
        [Fact]
        public void DropTableTest()
        {
            
        }
        */
    }
}