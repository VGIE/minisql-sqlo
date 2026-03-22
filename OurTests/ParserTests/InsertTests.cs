using DbManager;


namespace OurTests
{
    public class InsertTests
    {
        [Fact]
        public void TestInsertConstructor()
        {
            string table = "Datos";
            List<string> values = new List<string> { "Manzana", "1.20", "5" };


            Insert insertQ = new Insert(table, values);


            Assert.Equal("Datos", insertQ.Table);
            Assert.Equal(3, insertQ.Values.Count);
            Assert.Equal("Manzana", insertQ.Values[0]);
        }


        [Fact]
        public void TestInsertExecuteSuccess()
        {


            Database db = Database.CreateTestDatabase();
            List<string> values = new List<string> { "NuevoObj", "2.55", "35" };


            Insert insertQ = new Insert(Table.TestTableName, values);


            string result = insertQ.Execute(db);


            Assert.Equal(Constants.InsertSuccess, result);
        }


        [Fact]
        public void TestInsertExecuteTableDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();
            List<string> values = new List<string> { "Dato" };


            Insert insertQ = new Insert("TableInexistente", values);
            string result = insertQ.Execute(db);


            Assert.Equal(Constants.TableDoesNotExistError, result);
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);


        }


        [Fact]
        public void TestInsertSpaceInStringReturnsNull()
        {
            string query = "INSERT INTO Personas VALUES (10 00, 'Dato')";


            Insert result = MiniSQLParser.Parse(query) as Insert;


            Assert.Null(result);
        }


        [Fact]
        public void TestInsertUnbalancedQuotesReturnsNull()
        {
            string query = "INSERT INTO Personas VALUES ('Dato)";


            Insert result = MiniSQLParser.Parse(query) as Insert;


            Assert.Null(result);
        }


        [Fact]
        public void TestInsertMissingComaReturnsNull()
        {
            string query = "INSERT INTO Personas VALUES ('Dato1', 'Dato2')";


            Insert result = MiniSQLParser.Parse(query) as Insert;


            Assert.NotNull(result);
        }
        /*
        [Fact]
        public void TestInsertCommasAndSpaces()
        {
            string query1 = "INSERT INTO TestTable VALUES ('a','b')";
            Insert result1 = MiniSQLParser.Parse(query1) as Insert;
            Assert.NotNull(result1);

            string query2 = "INSERT INTO TestTable VALUES ('a', 'b')";
            Insert result2 = MiniSQLParser.Parse(query2) as Insert;
            Assert.NotNull(result2);

            string query3 = "INSERT INTO TestTable VALUES ('a' ,'b')";
            Insert result3 = MiniSQLParser.Parse(query3) as Insert;
            Assert.NotNull(result3);

            string query4 = "INSERT INTO TestTable VALUES ('a',    'b')";
            Insert result4 = MiniSQLParser.Parse(query4) as Insert;
            Assert.NotNull(result4);

            string query5 = "INSERT INTO TestTable VALUES ('a', 'b', 'c')";
            Insert result5 = MiniSQLParser.Parse(query5) as Insert;
            Assert.NotNull(result5);

            string query6 = "INSERT INTO TestTable VALUES ('a', 'b', 'c' )";
            Insert result6 = MiniSQLParser.Parse(query6) as Insert;
            Assert.NotNull(result6);

            string query7 = "INSERT INTO TestTable VALUES ('12', '3.45', 'Text')";
            Insert result7 = MiniSQLParser.Parse(query7) as Insert;
            Assert.NotNull(result7);



            string query8 = "INSERT INTO TestTable VALUES ('a' 'b')";
            Insert result8 = MiniSQLParser.Parse(query8) as Insert;
            Assert.Null(result8);

            string query9 = "INSERT INTO TestTable VALUES ('a', 'b' 'c')";
            Insert result9 = MiniSQLParser.Parse(query9) as Insert;
            Assert.Null(result9);

            string query10 = "INSERT INTO TestTable VALUES ('a','b',)";
            Insert result10 = MiniSQLParser.Parse(query10) as Insert;
            Assert.Null(result10);

            string query11 = "INSERT INTO TestTable VALUES ('a',,'b')";
            Insert result11 = MiniSQLParser.Parse(query11) as Insert;
            Assert.Null(result11);

            string query12 = "INSERT INTO TestTable VALUES ('a','b' , 'c";
            Insert result12= MiniSQLParser.Parse(query12) as Insert;
            Assert.Null(result12);
        }
        */
    }


}

