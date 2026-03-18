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
             var columns = new List<ColumnDefinition>
            {
                 new ColumnDefinition(ColumnDefinition.DataType.Int, "Number")
            };


            CreateTable createTable = new CreateTable(table, columns);
            string result = createTable.Execute(db);

            Assert.Equal("Table created", result);

         }

         [Fact]
         public void testCreateTableValidRegexQuery()
         {
              string query= "CREATE TABLE Productos (ID INT, Precio DOUBLE, Descripcion TEXT)";


              CreateTable result = MiniSQLParser.Parse(query) as CreateTable;
              Assert.NotNull(result);


         }


         [Fact]
         public void testCreateTableInvalidRegexQuery()
         {
              string query= "CREATE TABLE Personas (Activo BOOLEAN)";


              CreateTable result = MiniSQLParser.Parse(query) as CreateTable;
              Assert.Null(result);
             
         }


         [Fact]
         public void testLoweCaseTypeReturnsNull()
         {
              string query= "CREATE TABLE Personas (ID int)";


              CreateTable result = MiniSQLParser.Parse(query) as CreateTable;
              Assert.Null(result);
         }


          [Fact]
         public void testMissingColumnTypeReturnsNull()
         {
              string query= "CREATE TABLE Personas (ID INT, Nombre)";


              CreateTable result = MiniSQLParser.Parse(query) as CreateTable;
              Assert.Null(result);
         }


         [Fact]
         public void testSpaceAtTheStartReturnsNull()
         {
              string query= " CREATE TABLE Personas (ID INT)";


              CreateTable result = MiniSQLParser.Parse(query) as CreateTable;
              Assert.Null(result);
         }


         [Fact]
         public void TestCreateTableExecuteSuccess()
         {
              Database db= Database.CreateTestDatabase();
              List<ColumnDefinition> columns= new List<ColumnDefinition>()
              {
                   new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad")
              };
              CreateTable createTableQ= new CreateTable("Edades", columns);
              string result= createTableQ.Execute(db);


              Assert.Equal(Constants.CreateTableSuccess, result);
         }


         [Fact]
         public void TestCreateTableINTReturnsNotNull()
         {
              string query= "CREATE TABLE Personas (ID INT)";


              CreateTable result = MiniSQLParser.Parse(query) as CreateTable;


              Assert.NotNull(result);
         }


         [Fact]
         public void TestCreateTableDOUBLEReturnsNotNull()
         {
              string query= "CREATE TABLE Personas (Precios DOUBLE)";


              CreateTable result = MiniSQLParser.Parse(query) as CreateTable;


              Assert.NotNull(result);
         }


         [Fact]
         public void TestCreateTableTEXTReturnsNotNull()
         {
              string query= "CREATE TABLE Personas (Descripcion TEXT)";


              CreateTable result = MiniSQLParser.Parse(query) as CreateTable;


              Assert.NotNull(result);
         }


         [Fact]
         public void TestCreateTableStringReturnsNotNull()
         {
              string query= "CREATE TABLE Personas (ID String)";


              CreateTable result = MiniSQLParser.Parse(query) as CreateTable;


              Assert.Null(result);
         }

         [Fact]
         public void TestExecuteCreate()
          {
               string table= "A";
            Database database = Database.CreateTestDatabase();
            var columns = new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "N"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero"),
        
         };

            CreateTable createQ= new CreateTable(table,columns);
            string result= createQ.Execute(database);
            Assert.Equal("Table created", result);
          }


          /*[Fact]
         public void TestCreateTableInvalid()
         {
              string query= "CREATE TABLE Personas (ID INT, Name TEXT )";


              CreateTable result = MiniSQLParser.Parse(query) as CreateTable;


              Assert.Null(result);
         }*/


         /*[Fact]
         public void TestCreateTableEmptyTable()
         {
              string query= "CREATE TABLE Personas ()";


              CreateTable result = MiniSQLParser.Parse(query) as CreateTable;


              Assert.NotNull(result);
         }*/
    }
}
