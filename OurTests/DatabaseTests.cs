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
        string a1 = "Farlopo";
        int e1 = 2;

        Row r1 = new(columns, fila1);

        List<String> fila2 = new List<string>();
        string n2 = "Tijicius";
        string a2 = "Cortinsen";
        int e2 = 22;

        Row r2 = new(columns, fila2);
        db.TableByName("Alumnos").AddRow(r1);
        db.TableByName("Alumnos").AddRow(r2);
        return db;
    }
    [Fact]
    public void SelectTest()
    {
        Database db = CrearDb();
        
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