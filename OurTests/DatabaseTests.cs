namespace OurTests;

using DbManager;
public class DatabaseTests
{
    //TODO DEADLINE 1B : Create your own tests for Database
    public Database CrearDb()
    {
        Database db = new("farlop", "farlop");
        ColumnDefinition nombre = new(ColumnDefinition.DataType.String, "Nombre");
        ColumnDefinition apellido = new(ColumnDefinition.DataType.String, "Apellido");
        ColumnDefinition edad = new(ColumnDefinition.DataType.Int, "Edad");
        List<ColumnDefinition> columns = new List<ColumnDefinition>();
        columns.Add(nombre);
        columns.Add(apellido);
        columns.Add(edad);
        db.CreateTable("Alumnos", columns);


        List<String> fila1 = new List<string>();
        string n1 = "Naigel";
        fila1.Add(n1);
        string a1 = "Farlopo";
        fila1.Add(a1);
        string e1 = "2";
        fila1.Add(e1);

        Row r1 = new(columns, fila1);

        List<String> fila2 = new List<string>();
        string n2 = "Tijicius";
        fila2.Add(n2);
        string a2 = "Cortinsen";
        fila2.Add(a2);
        string e2 = "22";
        fila2.Add(e2);

        Row r2 = new(columns, fila2);
        db.TableByName("Alumnos").AddRow(r1);
        db.TableByName("Alumnos").AddRow(r2);
        return db;
    }
    [Fact]
    public void SelectTest()
    {
        Database db = CrearDb();

        //Condition
        Condition c = new Condition("Apellido", "=", "Cortinsen");


        //List of Columns to search for in select
        List<String> columnsSearch = new List<String>();
        columnsSearch.Add("Apellido");
        
        //Table to compare to
        ColumnDefinition apellido = new(ColumnDefinition.DataType.String, "Apellido");
        
        List<ColumnDefinition> columns = new List<ColumnDefinition>();
        columns.Add(apellido);
        
        List<String> row1 = new List<string>();
        row1.Add("Cortinsen");
        Row r = new(columns, row1);

        Table resultado = new Table("Alumnos", columns);
        resultado.AddRow(r);
        
        //Correct functioning
        Assert.Equal(resultado.ToString(),db.Select("Alumnos", columnsSearch, c).ToString());

        //Table doesn't exist
        Assert.Null(db.Select("Comidas", columnsSearch, c));

        //Columna doesn't exist
        columnsSearch.Clear();
        columnsSearch.Add("Empleo");
        Assert.Null(db.Select("Alumnos", columnsSearch, c));



    }
    [Fact]
    public void DeleteWhere()
    {
        Database db = CrearDb();

        //Condition to delete
        Condition c = new Condition("Apellido", "=", "Cortinsen");

        Assert.True(db.DeleteWhere("Alumnos", c));
        Assert.False(db.DeleteWhere("NoExiste", c));

        c = new Condition("ColumnaNoExiste", "=", "Cortinsen");
        Assert.False(db.DeleteWhere("Alumnos", c));
    }
    /*
    [Fact]
    public void UpdateTest()
    {
        Database db = CrearDb();
    }
*/    
}