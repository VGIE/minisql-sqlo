using System.Linq.Expressions;
using DbManager;
using DbManager.Parser;

namespace OurTests
{
    public class UpdateTests
    {
        [Fact]
        public void TestUpdateInitialization()
        {
            string table= "Datos";
        var setValues = new List<SetValue>
        {
        new SetValue("Nombre", "Araitz"),
        new SetValue("Edad", "20")
        };
        Condition condicion= new Condition("Edad", "=", "22");
        Update updateQuery= new Update(table, setValues, condicion);

        Assert.Equal("Datos", updateQuery.Table);
        Assert.Equal(2, updateQuery.Columns.Count);
        Assert.Equal("Nombre", updateQuery.Columns[0].ColumnName);
        Assert.Equal("Araitz", updateQuery.Columns[0].Value);
        Assert.Equal("Edad", updateQuery.Where.ColumnName);
        }

         [Fact]

         public void TestUpdateExecuteSuccess()
        {
            Database db= Database.CreateTestDatabase();
            Table table= db.TableByName(Table.TestTableName);

            Condition condicion= new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            List<SetValue> nuevos= new List<SetValue>
            {
                new SetValue(Table.TestColumn2Name, "1.70"),
                new SetValue(Table.TestColumn3Name, "30")
            };

            Update updateQ= new Update(Table.TestTableName, nuevos, condicion);
            string result= updateQ.Execute(db);

            Assert.Equal(Constants.UpdateSuccess, result);

        }

        [Fact]
        public void TestUpdateExecuteError()
        {
            Database db= Database.CreateTestDatabase();

            Update updateQ= new Update("TablaInexistente", new List<SetValue>(), null);
            string result= updateQ.Execute(db);

            Assert.Equal(db.LastErrorMessage, result);
            Assert.NotEqual(Constants.UpdateSuccess, result);

        }




    }
    
    
    
    }
