using DbManager;

namespace OurTests
{
    public class SelectTests
    {
       [Fact]
       public void TestSelectConstructor()
        {
            string table= "Datos";
            List<string> columns= new List<string>{"Nombre", "Edad"};
            Condition condicion= new Condition("Edad", ">", "20");

            Select selectQ= new Select(table, columns, condicion);

            Assert.Equal("Datos", selectQ.Table);
            Assert.Equal(2, selectQ.Columns.Count);
            Assert.Equal("Nombre", selectQ.Columns[0]);
            Assert.Equal(condicion, selectQ.Where);
        } 

       [Fact]
       public void TestSelectExecuteSuccess()
        {
            Database db= Database.CreateTestDatabase();
            List<string> columns= new List<string>{Table.TestColumn1Name};
            Select selectQ= new Select(Table.TestTableName, columns, null);
            string result= selectQ.Execute(db);

            Assert.NotNull(result);
            Assert.Contains(Table.TestColumn1Row1, result);
            Assert.DoesNotContain(Constants.UpdateSuccess, result);
            
        }

        [Fact]
        public void TestSelectWithCondition()
        {
            Database db= Database.CreateTestDatabase();
            List<string> columns= new List<string>{Table.TestColumn1Name};
            Condition condicion= new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            Select selectQ= new Select(Table.TestTableName, columns, condicion);
            string result= selectQ.Execute(db);

            Assert.Contains(Table.TestColumn1Row1, result);
            Assert.DoesNotContain(Table.TestColumn1Row2, result);
            
        }

        [Fact]

        public void TestSelectWithSpacesInLiteralWithoutQuotesNull()
        {
            string query= "SELECT Nombre FROM Usuarios WHERE Ciudad = Lu ";

            var result= MiniSQLParser.Parse(query);
            Assert.Null(result);

        }

        [Fact]

        public void TestSelectWhereValueWIthSpacesWithQuotesNotNull()
        {
            string query= "SELECT Nombre FROM Personas WHERE Name = 'Lupe'";
            Select result = MiniSQLParser.Parse(query) as Select;

            Assert.NotNull(result);
            Assert.Equal("Lupe", result.Where.LiteralValue);
        }



    }
    
}
