using DbManager;
using DbManager.Parser;
using Xunit;

namespace OurTests
{
    public class TableTests
    {
        [Fact]
        public void getRowColumnTest()
        {
            Table testTable = Table.CreateTestTable();
            string TestColumn1Name = "Name";
            string TestColumn2Name = "Height";
            string TestColumn3Name = "Age";
            string TestColumn1Row1 = "Rodolfo";
            string TestColumn2Row1 = "1.62";
            string TestColumn3Row1 = "25";
            ColumnDefinition.DataType TestColumn1Type = ColumnDefinition.DataType.String;
            ColumnDefinition.DataType TestColumn2Type = ColumnDefinition.DataType.Double;
            ColumnDefinition.DataType TestColumn3Type = ColumnDefinition.DataType.Int;
            List<ColumnDefinition> ExpectedTypes = new List<ColumnDefinition>()
            {
                new ColumnDefinition(TestColumn1Type, TestColumn1Name),
                new ColumnDefinition(TestColumn2Type, TestColumn2Name),
                new ColumnDefinition(TestColumn3Type, TestColumn3Name)
            };
            List<string> ExpectedValues = new List<string>() { TestColumn1Row1, TestColumn2Row1, TestColumn3Row1 };
            Row ExpectedRow = new Row(ExpectedTypes, ExpectedValues);

            Assert.True(ExpectedRow.Equals(testTable.GetRow(0)));
            Assert.Null(testTable.GetRow(5));
            Assert.True((new ColumnDefinition(TestColumn1Type, TestColumn1Name)).Equals(testTable.GetColumn(0)));
            Assert.Null(testTable.GetColumn(5));
        }

        [Fact]
        public void numRowsColumnsTest()
        {
            Table testTable = Table.CreateTestTable();
            Assert.Equal(3, testTable.NumRows());
            Assert.Equal(3, testTable.NumColumns());
        }

        [Fact]
        public void columnByName_columnIndexByNameTest()
        {
            Table testTable = Table.CreateTestTable();


            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            ColumnDefinition columna2 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna2");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            columnas.Add(columna2);
            Table tabla1 = new Table("tabla1", columnas);

            Assert.Equal(columna1, tabla1.ColumnByName("columna1"));
            Assert.Equal(columna2, tabla1.ColumnByName("columna2"));
            Assert.Null(tabla1.ColumnByName(null));
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
            List<ColumnDefinition> columnas2 = new List<ColumnDefinition>();
            Table tabla2 = new Table("tabla2", columnas2);
            List<ColumnDefinition> columnas3 = new List<ColumnDefinition>();
            columnas3.Add(columna1);
            columnas3.Add(columna2);
            Table tabla3 = new Table("tabla3", columnas3);

            Assert.Equal("['columna1','columna2']{'c1','c2'}{'c1','c2'}", tabla1.ToString());
            Assert.Equal("", tabla2.ToString());
            Assert.Equal("['columna1','columna2']", tabla3.ToString());
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
            List<string> valores2 = new List<string>();
            valores2.Add("1");
            valores2.Add("2");
            Row fila = new Row(columnas, valores);
            Row fila2 = new Row(columnas, valores2);
            tabla1.AddRow(fila);
            tabla1.AddRow(fila2);
            Condition condicion = new Condition("columna1", "=", "c1");
            Condition condicion2 = new Condition("columna2", "=", "2");
            List<int> index = new List<int>();
            for (int i = 0; i < tabla1.NumRows(); i++)
            {
                if (tabla1.GetRow(i).IsTrue(condicion))
                {
                    index.Add(i);
                }
            }

            List<int> index2 = new List<int>();
            for (int i = 0; i < tabla1.NumRows(); i++)
            {
                if (tabla1.GetRow(i).IsTrue(condicion2))
                {
                    index2.Add(i);
                }
            }
            Assert.Equal(index, tabla1.RowIndicesWhereConditionIsTrue(condicion));
            Assert.Equal(index2, tabla1.RowIndicesWhereConditionIsTrue(condicion2));
        }

        [Fact]
        public void DeleteWhereTest()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            ColumnDefinition columna2 = new ColumnDefinition(ColumnDefinition.DataType.Int, "columna2");
            ColumnDefinition columna3 = new ColumnDefinition(ColumnDefinition.DataType.Double, "columna3");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            columnas.Add(columna2);
            columnas.Add(columna3);
            Table tabla1 = new Table("tabla1", columnas);
            List<string> valoresF1 = new List<string>();
            valoresF1.Add("c1-1");
            valoresF1.Add("1");
            valoresF1.Add("1.1");
            List<string> valoresF2 = new List<string>();
            valoresF2.Add("c1-2");
            valoresF2.Add("2");
            valoresF2.Add("2.2");
            List<string> valoresF3 = new List<string>();
            valoresF3.Add("c1-3");
            valoresF3.Add("2");
            valoresF3.Add("3.3");
            Row fila = new Row(columnas, valoresF1);
            Row fila2 = new Row(columnas, valoresF2);
            Row fila3 = new Row(columnas, valoresF3);
            tabla1.AddRow(fila);
            tabla1.AddRow(fila2);
            tabla1.AddRow(fila3);
            Condition condicion = new Condition("columna1", "=", "c1-1");
            Condition condicion2 = new Condition("columna2", "=", "2");
            Condition condicion3 = new Condition("columna3", ">", "3");

            Table tablaResultado = new Table("tablaResultado", columnas);
            tablaResultado.AddRow(fila2);
            tablaResultado.AddRow(fila3);
            tabla1.DeleteWhere(condicion);

            Assert.Equal(tablaResultado.ToString(), tabla1.ToString());

            tablaResultado = new Table("tablaResultado", columnas);
            tabla1.DeleteWhere(condicion2);

            Assert.Equal(tablaResultado.ToString(), tabla1.ToString());

            tablaResultado = new Table("tablaResultado", columnas);
            tabla1.AddRow(fila3);
            tabla1.DeleteWhere(condicion3);
            Assert.Equal(tablaResultado.ToString(), tabla1.ToString());
        }

        [Fact]
        public void selectTest()
        {
            ColumnDefinition columna1 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna1");
            ColumnDefinition columna2 = new ColumnDefinition(ColumnDefinition.DataType.String, "columna2");
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(columna1);
            columnas.Add(columna2);
            List<ColumnDefinition> columnasDes = new List<ColumnDefinition>();
            columnasDes.Add(columna2);
            columnasDes.Add(columna1);
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
            List<string> columnasDesordenadas = new List<string>();
            columnasDesordenadas.Add("columna2");
            columnasDesordenadas.Add("columna1");
            Table tablaResultado2 = new Table("tablaResultado2", columnasDes);
            tablaResultado2.AddRow(fila);
            tablaResultado2.AddRow(fila2);

            Assert.Equal(tablaResultado.ToString(), tabla1.Select(columnasNombres, condicion).ToString());
            Assert.Equal(tablaResultado2.ToString(), tabla1.Select(columnasDesordenadas, null).ToString());
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
            Assert.True(tabla1.Insert(valores));
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

            Assert.False(tabla1.Update(nuevosValores, null));
            Assert.Equal(tablaResultado.ToString(), tabla1.ToString());
        }
    }
}