using DbManager;

namespace OurTests
{
    public class InsertTests
    {
       [Fact]
       public void TestInsertConstructor()
        {
            string table= "Datos";
            List<string> values= new List<string>{"Manzana", "1.20", "5"};

            Insert insertQ= new Insert(table, values);

            Assert.Equal("Datos", insertQ.Table);
            Assert.Equal(3, insertQ.Values.Count);
            Assert.Equal("Manzana", insertQ.Values[0]);
        }

        [Fact]
        public void TestInsertExecuteSuccess()
        {

            Database db= Database.CreateTestDatabase();
            List<string> values= new List<string>{"NuevoObj", "2.55", "35"};

            Insert insertQ= new Insert(Table.TestTableName, values);

            string result=  insertQ.Execute(db);

            Assert.Equal(Constants.InsertSuccess, result);  
        }

        [Fact]
        public void TestInsertExecuteTableDoesNotExist()
        {
            Database db= Database.CreateTestDatabase();
            List<string> values= new List<string>{"Dato"};

            Insert insertQ= new Insert("TableInexistente", values);
            string result= insertQ.Execute(db);

            Assert.Equal(Constants.TableDoesNotExistError, result);
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);
            
        }

    }





}

        

        