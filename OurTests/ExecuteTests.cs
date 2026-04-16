
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
            Assert.Equal(Constants.DeleteSuccess,Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("DELETE FROM TestTable WHERE Age='25'"));
            Assert.NotEqual(Constants.DeleteSuccess, Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("DELETE FROM TestTable"));
            Assert.Equal(Constants.DeleteSuccess, Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("DELETE      FROM    TestTable    WHERE Age<'25'"));
            Assert.Equal(Constants.DeleteSuccess, Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("DELETE FROM TestTable WHERE Name='Maider'"));
            Assert.NotEqual(Constants.DeleteSuccess, Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("DELETE FROM TestTable1 WHERE Wage='-52.85'"));
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

            Assert.Equal(Constants.InsertSuccess, Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("INSERT INTO TestTable VALUES ('Izan','20','-5.9')"));
            Assert.Equal(Constants.InsertSuccess, Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("INSERT INTO TestTable VALUES ('Izan Ascasso','20','-5.9')"));
            Assert.NotNull(Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("INSERT INTO    table1 VALUES('val5 val5','val6 val6 val6')"));
            
        }
        [Fact]
        public void UpdateTableTest() 
        {
            Assert.Equal(Constants.UpdateSuccess, Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("UPDATE TestTable SET Height='1.56',Age='52' WHERE Name='Pepe'"));
            Assert.NotEqual("UpdateSuccess", Database.CreateTestDatabase().
                ExecuteMiniSQLQuery("UPDATE tabla SET column1=1,column2=2 WHERE columna=valor"));
        }
    }
}