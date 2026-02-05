namespace OurTests;

using DbManager;
{
    public class DatabaseTests
{
    //TODO DEADLINE 1B : Create your own tests for Database

    [Fact]
    public void SelectTest()
    {
        Database db = new("farlop", "farlop");
        ColumnDefinition nombre = new(ColumnDefinition.DataType.String, "Nombre");
        ColumnDefinition apellido = new(ColumnDefinition.DataType.String, "Apellido");
        ColumnDefinition edad = new(ColumnDefinition.DataType.Int, "Edad");
        List<ColumnDefinition> columns = new List<ColumnDefinition>();
        columns.Add(nombre);
        columns.Add(apellido);
        columns.Add(edad);
        Table t = new("Alumnos", columns);

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
        
        t.AddRow(r1);
        t.AddRow(r2);
        db.AddTable(t);
    }
    public void DeleteWhere()
    {

    }
    public void UpdateTest()
    {

    }

}
}