 using DbManager;
using DbManager.Parser;

namespace OurTests
{
 
 public class CreateTableTests
 {

        [Fact]
public void testCreateTable()
{      
            Database db = Database.CreateTestDatabase();

            string table = "Nueva";
           
            var columns= new List<ColumnDefinition>
            {
                 new ColumnDefinition(ColumnDefinition.DataType.Int, "Number")
            };
            

            CreateTable createTable = new CreateTable(table, columns);
            string result = createTable.Execute(db);

            Assert.Equal("Table created", result);
            
}

}
}