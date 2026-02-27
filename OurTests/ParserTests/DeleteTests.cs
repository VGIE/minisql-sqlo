using DbManager;
using DbManager.Parser;

namespace OurTests
{
    public class DeleteTests
    {
        
        [Fact]
        public void TestDeleteConstructor()
        {
            string table= "Datos";
            Condition condicion= new Condition("Nombre", "=", "Araitz");

            Delete deleteQ= new Delete(table,condicion);

            Assert.Equal("Datos", deleteQ.Table);
            Assert.Equal(condicion, deleteQ.Where);
            Assert.Equal("Nombre", deleteQ.Where.ColumnName);
            
        }

        [Fact]
        public void TestDeleteExecuteSuccess()
        {
            Database db= Database.CreateTestDatabase();

            Condition condicion= new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            Delete deleteQ= new Delete(Table.TestTableName, condicion);

            string result= deleteQ.Execute(db);

            Assert.Equal(Constants.DeleteSuccess, result);
            
        }

        [Fact]
        public void TestDeleteExecuteTableNotExist()
        {
            Database db= Database.CreateTestDatabase();
            Condition condicion= new Condition("Id", "=", "1");

            Delete deleteQ= new Delete("TablaInexistente", condicion);
            string result= deleteQ.Execute(db);

            Assert.Equal(Constants.TableDoesNotExistError, result);
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);


            
        }
    }
}      