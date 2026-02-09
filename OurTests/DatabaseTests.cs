using DbManager;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;

namespace OurTests
{
    public class DatabaseTests
    {
        //TODO DEADLINE 1B : Create your own tests for Database
        public Database CreateTestDatabase()
        {
            Database db = new Database("paco", "1234");
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            ColumnDefinition c1 = new ColumnDefinition(ColumnDefinition.DataType.String, "nombre");
            ColumnDefinition c2 = new ColumnDefinition(ColumnDefinition.DataType.Int, "edad");
            ColumnDefinition c3 = new ColumnDefinition(ColumnDefinition.DataType.Double, "peso");
            columns.Add(c1);
            columns.Add(c2);
            columns.Add(c3);
            Table tabla = new Table("tabla",columns);
            return db;
        }
        
        [Fact]
        public void AddTableAndTableByNameTest()
        {
            Database db=new Database("paco","1234");
            Assert.NotNull(db.TableByName("tabla"));
            Assert.Null(db.TableByName("adios"));
        }
        [Fact]
        public void CreateTableTest()
        {
            Database db=CreateTestDatabase();
            List<ColumnDefinition> columnsVacio = new List<ColumnDefinition>();
            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            ColumnDefinition c1 = new ColumnDefinition(ColumnDefinition.DataType.String, "nombre");
            ColumnDefinition c2 = new ColumnDefinition(ColumnDefinition.DataType.Int, "edad");
            ColumnDefinition c3 = new ColumnDefinition(ColumnDefinition.DataType.Double, "peso");
            columns.Add (c1);
            columns.Add(c2);
            columns.Add(c3);
            Assert.False(db.CreateTable("TablaFantasma",columnsVacio));
            Assert.Equal(Constants.DatabaseCreatedWithoutColumnsError, db.LastErrorMessage);
            Assert.True(db.CreateTable("TablaCorrecta", columns));
            Assert.Equal(Constants.CreateTableSuccess,db.LastErrorMessage);
            Assert.False(db.CreateTable("TablaCorrecta", columns));
            Assert.Equal(Constants.TableAlreadyExistsError, db.LastErrorMessage);
        }
        [Fact]
        public void DropTableTest()
        {
            Database db=CreateTestDatabase();
            Assert.False(db.DropTable("LebronEsLaCabra"));
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);
            Assert.True(db.DropTable("tabla"));
            Assert.Equal(Constants.DropTableSuccess, db.LastErrorMessage);

        }

    }
}