namespace OurTests;

using DbManager;
using DbManager.Parser;
using DbManager;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
public class DatabaseTests
{
    //TODO DEADLINE 1B : Create your own tests for Database
    public Database CreateTestDatabase1()
    {
        Database db = new("User", "User");
        ColumnDefinition name = new(ColumnDefinition.DataType.String, "Name");
        ColumnDefinition surname = new(ColumnDefinition.DataType.String, "Surname");
        ColumnDefinition age = new(ColumnDefinition.DataType.Int, "Age");
        List<ColumnDefinition> columns = new List<ColumnDefinition>();
        columns.Add(name);
        columns.Add(surname);
        columns.Add(age);
        db.CreateTable("Students", columns);


        List<String> row1 = new List<string>();
        string n1 = "John";
        row1.Add(n1);
        string a1 = "Doe";
        row1.Add(a1);
        string e1 = "20";
        row1.Add(e1);

        Row r1 = new(columns, row1);

        List<String> row2 = new List<string>();
        string n2 = "Mary";
        row2.Add(n2);
        string a2 = "Jane";
        row2.Add(a2);
        string e2 = "22";
        row2.Add(e2);

        Row r2 = new(columns, row2);
        db.TableByName("Students").AddRow(r1);
        db.TableByName("Students").AddRow(r2);
        return db;
    }
    [Fact]
    public void SelectTest()
    {
        Database db = CreateTestDatabase1();

        //Condition
        Condition c = new Condition("Surname", "=", "Doe");


        //List of Columns to search for in select
        List<String> columnsSearch = new List<String>();
        columnsSearch.Add("Surname");

        //Table to compare to
        ColumnDefinition surname = new(ColumnDefinition.DataType.String, "Surname");

        List<ColumnDefinition> columns = new List<ColumnDefinition>();
        columns.Add(surname);

        List<String> row1 = new List<string>();
        row1.Add("Doe");
        Row r = new(columns, row1);

        Table result = new Table("Students", columns);
        result.AddRow(r);

        //Correct functioning
        Assert.Equal(result.ToString(), db.Select("Students", columnsSearch, c).ToString());

        //Table doesn't exist
        Assert.Null(db.Select("Food", columnsSearch, c));

        //Columna doesn't exist
        columnsSearch.Clear();
        columnsSearch.Add("Job");
        Assert.Null(db.Select("Students", columnsSearch, c));
    }
    [Fact]
    public void DeleteWhereTest()
    {
        Database db = CreateTestDatabase1();

        //Condition to delete
        Condition c = new Condition("Surname", "=", "Doe");

        Assert.True(db.DeleteWhere("Students", c));
        Assert.False(db.DeleteWhere("NullTable", c));
        c = new Condition("NullColumn", "=", "Doe");
        Assert.False(db.DeleteWhere("Students", c));
    }
    [Fact]
    public void UpdateTest()
    {
        Database db = CreateTestDatabase1();
        List<String> columnsSearch = new List<String>();
        columnsSearch.Add("Surname");

        Condition c = new Condition("Surname", "=", "Doe");
        List<SetValue> newValues = new List<SetValue>();
        SetValue value1 = new SetValue("Name", "actualizado1");

        Assert.True(db.Update("Students", newValues, c));
        Assert.False(db.Update("NoExiste", newValues, c));
        newValues.Clear();
        value1 = new SetValue("ColumnaInexistente", "null");
        newValues.Add(value1);
        Assert.False(db.Update("Alumnos", newValues, c));
    }
    public Database CreateTestDatabase2()
    {
        Database db = new Database("paco", "1234");
        List<ColumnDefinition> columns = new List<ColumnDefinition>();
        ColumnDefinition c1 = new ColumnDefinition(ColumnDefinition.DataType.String, "nombre");
        ColumnDefinition c2 = new ColumnDefinition(ColumnDefinition.DataType.Int, "edad");
        ColumnDefinition c3 = new ColumnDefinition(ColumnDefinition.DataType.Double, "peso");
        columns.Add(c1);
        columns.Add(c2);
        columns.Add(c3);
        Table tabla = new Table("tabla", columns);
        db.AddTable(tabla);
        return db;
    }

    [Fact]
    public void CreateTableAndTableByNameTest()
    {
        Database db = CreateTestDatabase2();
        Assert.NotNull(db.TableByName("tabla"));
        Assert.Null(db.TableByName("adios"));
    }
    [Fact]
    public void CreateTableTest()
    {
        Database db = CreateTestDatabase2();
        List<ColumnDefinition> columnsVacio = new List<ColumnDefinition>();
        List<ColumnDefinition> columns = new List<ColumnDefinition>();
        ColumnDefinition c1 = new ColumnDefinition(ColumnDefinition.DataType.String, "nombre");
        ColumnDefinition c2 = new ColumnDefinition(ColumnDefinition.DataType.Int, "edad");
        ColumnDefinition c3 = new ColumnDefinition(ColumnDefinition.DataType.Double, "peso");
        columns.Add(c1);
        columns.Add(c2);
        columns.Add(c3);
        Assert.False(db.CreateTable("TablaFantasma", columnsVacio));
        Assert.Equal(Constants.DatabaseCreatedWithoutColumnsError, db.LastErrorMessage);
        Assert.True(db.CreateTable("TablaCorrecta", columns));
        Assert.Equal(Constants.CreateTableSuccess, db.LastErrorMessage);
        Assert.False(db.CreateTable("TablaCorrecta", columns));
        Assert.Equal(Constants.TableAlreadyExistsError, db.LastErrorMessage);
    }
    [Fact]
    public void DropTableTest()
    {
        Database db = CreateTestDatabase2();
        Assert.False(db.DropTable("LebronEsLaCabra"));
        Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);
        Assert.True(db.DropTable("tabla"));
        Assert.Equal(Constants.DropTableSuccess, db.LastErrorMessage);

    }
    [Fact]
    public void InsertTest()
    {
        Database db = CreateTestDatabase2();
        List<String> valores = null;
        Assert.False(db.Insert("maicol jordan", valores));
        Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);
        Assert.False(db.Insert("tabla", valores));
        valores = new List<string>();
        valores.Add("Luka");
        valores.Add("77");
        Assert.False(db.Insert("tabla", valores));
        valores.Add("200");
        Assert.True(db.Insert("tabla", valores));
        Assert.Equal(Constants.InsertSuccess, db.LastErrorMessage);
    }
}