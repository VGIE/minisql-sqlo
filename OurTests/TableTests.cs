using DbManager;
using DbManager.Parser;
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

            Assert.Equal(fila, tabla1.GetRow(0));
            Assert.Equal(columna1, tabla1.GetColumn(0));
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
            Assert.Equal(0, tabla1.ColumnIndexByName("columna1"));
            Assert.Equal(1, tabla1.ColumnIndexByName("columna2"));
        }

        [Fact]
        public void tableToString()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            ColumnDefinition columna2 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna2");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            columnas.Add(columna2);
            Table tabla1 = new Table("tabla1", columnas);

            Assert.Equal("['columna1','columna2']", tabla1.ToString());

            List<string> valores = new List<string>();
            valores.Add("c1");
            valores.Add("c2");
            Row fila = new Row(columnas, valores);
            Row fila2 = new Row(columnas, valores);
            tabla1.AddRow(fila);
            tabla1.AddRow(fila2);

            Assert.Equal("['columna1','columna2']{'c1','c2'}{'c1','c2'}", tabla1.ToString());
        }

        [Fact]
        public void deleteIthRowTest()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            ColumnDefinition columna2 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna2");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            columnas.Add(columna2);
            Table tabla1 = new Table("tabla1", columnas);
            List<string> valores = new List<string>();
            valores.Add("c1");
            valores.Add("c2");
            Row fila = new Row(columnas, valores);
            Row fila2 = new Row(columnas, valores);
            tabla1.AddRow(fila);
            tabla1.AddRow(fila2);
            tabla1.DeleteIthRow(0);

            Assert.Equal(fila2, tabla1.GetRow(0));
        }

        [Fact]
        public void RowIndicesWhereConditionIsTrueTest()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            ColumnDefinition columna2 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna2");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            columnas.Add(columna2);
            Table tabla1 = new Table("tabla1", columnas);
            List<string> valores = new List<string>();
            valores.Add("c1");
            valores.Add("c2");
            Row fila = new Row(columnas, valores);
            Row fila2 = new Row(columnas, valores);
            tabla1.AddRow(fila);
            tabla1.AddRow(fila2);
            Condition condicion = new Condition("columna1", "=", "c1");
            List<int> index = new List<int>();
            for (int i = 0; i < tabla1.NumRows(); i++)
            {
                if (tabla1.GetRow(i).IsTrue(condicion))
                {
                    index.Add(i);
                }
            }
            Assert.Equal(index, tabla1.RowIndicesWhereConditionIsTrue(condicion));
        }

        [Fact]
        public void selectTest()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            ColumnDefinition columna2 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna2");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            columnas.Add(columna2);
            Table tabla1 = new Table("tabla1", columnas);
            List<string> valores = new List<string>();
            valores.Add("c1");
            valores.Add("c2");
            List<string> valores2 = new List<string>();
            valores2.Add("c3");
            valores2.Add("c4");
            Row fila = new Row(columnas, valores);
            Row fila2 = new Row(columnas, valores2);
            tabla1.AddRow(fila);
            tabla1.AddRow(fila2);
            Condition condicion = new Condition("columna1", "=", "c1");
            Table tablaResultado = new Table("tablaResultado", columnas);
            tablaResultado.AddRow(fila);
            List<string> columnasNombres = new List<string>();
            columnasNombres.Add("columna1");
            columnasNombres.Add("columna2");
            Table tablaResultado2 = new Table("tablaResultado2", columnas);
            tablaResultado2.AddRow(fila);
            tablaResultado2.AddRow(fila2);

            Assert.Equal(tablaResultado.ToString(), tabla1.Select(columnasNombres, condicion).ToString());
            Assert.Equal(tablaResultado2.ToString(), tabla1.Select(columnasNombres, null).ToString());
        }

        [Fact]
        public void insertTest()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            ColumnDefinition columna2 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna2");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            columnas.Add(columna2);
            Table tabla1 = new Table("tabla1", columnas);
            List<string> valores = new List<string>();
            valores.Add("c1");
            valores.Add("c2");
            tabla1.Insert(valores);
            Table tablaResultado = new Table("tablaResultado", columnas);
            Row fila = new Row(columnas, valores);
            tablaResultado.AddRow(fila);

            List<string> valoresMal = new List<string>();
            valoresMal.Add("mal1");
            valoresMal.Add("mal2");
            valoresMal.Add("mal3");

            Assert.Equal(tablaResultado.ToString(), tabla1.ToString());
            Assert.False(tabla1.Insert(valoresMal));
        }

        [Fact]
        public void updateTest()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            ColumnDefinition columna2 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna2");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            columnas.Add(columna2);
            Table tabla1 = new Table("tabla1", columnas);
            List<string> valores = new List<string>();
            valores.Add("c1");
            valores.Add("c2");
            tabla1.Insert(valores);
            List<SetValue> nuevosValores = new List<SetValue>();
            SetValue valor1 = new SetValue("columna1", "actualizado1");
            SetValue valor2 = new SetValue("columna2", "actualizado2");
            nuevosValores.Add(valor1);
            nuevosValores.Add(valor2);
            Condition condicion = new Condition("columna2", "=", "c2");
            tabla1.Update(nuevosValores, condicion);
            Table tablaResultado = new Table("tablaResultado", columnas);
            List<string> valores2 = new List<string>();
            valores2.Add("actualizado1");
            valores2.Add("actualizado2");
            tablaResultado.Insert(valores2);

            Assert.Equal(tablaResultado.ToString(), tabla1.ToString());
        }
    }
}