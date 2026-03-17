using DbManager;
using DbManager.Parser;
using Xunit;
namespace OurTests
{
    public class ExecuteTests
    {
        
        [Fact]
        public void DeleteTests()
        {
            Assert.Equal("DeleteSuccess",Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("DELETE FROM TestTable WHERE Age='25'"));
            Assert.NotEqual("DeleteSuccess", Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("DELETE FROM TestTable"));
            Assert.Equal("DeleteSuccess", Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("DELETE      FROM    TestTable    WHERE Age<'25'"));
            Assert.Equal("DeleteSuccess", Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("DELETE FROM TestTable WHERE Name='Maider'"));
        }
        

        [Fact]
        public void SelectTests()
        {
            Database database = Database.CreateTestDatabase();
            Select select = new Select("TestTable", new List<string>()
            {
                "Name","Height"
            });
            Assert.Equal(database.Select("TestTable", new List<string>()
            {
                "Name","Height"
            },null).ToString(),select.Execute(database));


            select = new Select("TablaMal", new List<string>() { });
            string result = select.Execute(database);
            Assert.Equal(Constants.TableDoesNotExistError, database.LastErrorMessage);

            select = new Select("TestTable", new List<string>() { "NoExiste" });
            result = select.Execute(database);
            Assert.Equal(Constants.ColumnDoesNotExistError, database.LastErrorMessage);
        }   
        
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
        
        [Fact]
        public void DropTableTest()
        {
            Database database = Database.CreateTestDatabase();

            DropTable dropTable = new DropTable("TestTable");
            Assert.Equal(Constants.DropTableSuccess, dropTable.Execute(database));

            string result = dropTable.Execute(database);
            Assert.Equal(Constants.TableDoesNotExistError, dropTable.Execute(database));
        }
        
        [Fact]
        public void InsertTest()
        {
            Database database = Database.CreateTestDatabase();
            Insert insert = new Insert("TestTable", new List<string>() { "Igor", "21", "1.80" });
            Assert.Equal(Constants.InsertSuccess, insert.Execute(database));

            insert = new Insert("TablaMal", new List<string>() { "Igor", "21", "1.80" });
            Assert.Equal(Constants.TableDoesNotExistError, insert.Execute(database));

            insert = new Insert("TestTable", new List<string>() { "Igor", "21" });
            Assert.Equal(Constants.ColumnCountsDontMatch, insert.Execute(database));
        }
    }
}