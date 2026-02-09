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
        Condition c = new Condition("Apellido", "=", "Cortinsen");
        List<String> columnsTest = new List<String>();
        columnsTest.Add("Apellido");
        

        ColumnDefinition nombre = new(ColumnDefinition.DataType.String, "Nombre");
        ColumnDefinition apellido = new(ColumnDefinition.DataType.String, "Apellido");
        ColumnDefinition edad = new(ColumnDefinition.DataType.Int, "Edad");
        List<ColumnDefinition> columns = new List<ColumnDefinition>();
        columns.Add(nombre);
        columns.Add(apellido);
        columns.Add(edad);
        Table resultado = new Table("Alumnos", columns);
        List<String> fila = new List<string>();
        string n = "Tijicius";
        fila.Add(n);
        string a = "Cortinsen";
        fila.Add(a);
        string e = "22";
        fila.Add(e);
        Row r = new(columns, fila);
        Assert.Equal(resultado,db.Select("Alumnos", columnsTest, c));

    }
    [Fact]
    public void DeleteWhere()
    {
        Database db = CrearDb();
    }
    [Fact]
    public void UpdateTest()
    {
        Database db = CrearDb();
    }
}