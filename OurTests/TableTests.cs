using DbManager;
using Xunit;

namespace OurTests
{
    public class TableTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table
        /*
        [Fact]
        public void Test1()
        {

        }
        */
        [Fact]
        public void getRowColumnTest()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            Table tabla1 = new Table("tabla1", columnas);
            List<string> valores = new List<string>();
            valores.Add("fila1");
            Row fila = new Row(columnas, valores);
            tabla1.AddRow(fila);

            Assert.Equal(fila, tabla1.GetRow(1));
            Assert.Equal(columna1, tabla1.GetColumn(1));
        }

        [Fact]
        public void numRowsColumnsTest()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            Table tabla1 = new Table("tabla1", columnas);
            List<string> valores = new List<string>();
            valores.Add("fila1");
            List<string> valores2 = new List<string>();
            valores.Add("fila2");
            Row fila = new Row(columnas, valores);
            Row fila2 = new Row(columnas, valores2);
            tabla1.AddRow(fila);
            tabla1.AddRow(fila2);

            Assert.Equal(2, tabla1.NumRows());
            Assert.Equal(1, tabla1.NumColumns());
        }

        [Fact]
        public void columnByName_columnIndexByNameTest()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            ColumnDefinition columna2 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna2");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            columnas.Add(columna2);
            Table tabla1 = new Table("tabla1", columnas);

            Assert.Equal(columna1, tabla1.ColumnByName("columna1"));
            Assert.Equal(columna2, tabla1.ColumnByName("columna2"));
            Assert.Equal(1, tabla1.ColumnIndexByName("columna1"));
            Assert.Equal(2, tabla1.ColumnIndexByName("columna1"));
        }



         
    }
}