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
            ColumnDefinition columnString = ColumnDefinition.CreateTestColumnString();
            ColumnDefinition columnInt = ColumnDefinition.CreateTestColumnInt();
            ColumnDefinition columnDouble = ColumnDefinition.CreateTestColumnDouble();

            Assert.Equal(columnString, testTable.ColumnByName("Name"));
            Assert.Equal(columnDouble, testTable.ColumnByName("Height"));
            Assert.Equal(columnInt, testTable.ColumnByName("Age"));
            Assert.Null(testTable.ColumnByName(null));
            Assert.Equal(0, testTable.ColumnIndexByName("Name"));
            Assert.Equal(1, testTable.ColumnIndexByName("Height"));
            Assert.Equal(2, testTable.ColumnIndexByName("Age"));
        }

        [Fact]
        public void tableToString()
        {
            Table testTable = Table.CreateTestTable();

            List<ColumnDefinition> emptyColumns = new List<ColumnDefinition>();
            Table testTable2 = new Table("emptyTable", emptyColumns);


            List<ColumnDefinition> emptyValues = new List<ColumnDefinition>()
            {
                ColumnDefinition.CreateTestColumnString(),
                ColumnDefinition.CreateTestColumnDouble(),
                ColumnDefinition.CreateTestColumnInt()
            };
            Table testTable3 = new Table("emptyValues", emptyValues);


            Assert.Equal("['Name','Height','Age']{'Rodolfo','1.62','25'}{'Maider','1.67','67'}{'Pepe','1.55','51'}", testTable.ToString());
            Assert.Equal("", testTable2.ToString());
            Assert.Equal("['Name','Height','Age']", testTable3.ToString());
        }

        [Fact]
        public void deleteIthRowTest()
        {
            Table testTable = Table.CreateTestTable();
            Table testTable2 = Table.CreateTestTable2Rows();
            Table testTable3 = Table.CreateTestTable1Row();
            testTable.DeleteIthRow(1);
            Assert.Equal(testTable2.ToString(), testTable.ToString());
            testTable.DeleteIthRow(1);
            Assert.Equal(testTable3.ToString(), testTable.ToString());
        }

        [Fact]
        public void RowIndicesWhereConditionIsTrueTest()
        {
            Table testTable = Table.CreateTestTable();

            Condition condition = new Condition("Name", "=", "Rodolfo");
            Condition condition2 = new Condition("Height", "<", "1.56");
            Condition condition3 = new Condition("Age", ">", "50");

            List<int> index = new List<int>();
            index.Add(0);
            List<int> index2 = new List<int>();
            index2.Add(2);
            List<int> index3 = new List<int>();
            index3.Add(1);
            index3.Add(2);

            Assert.Equal(index, testTable.RowIndicesWhereConditionIsTrue(condition));
            Assert.Equal(index2, testTable.RowIndicesWhereConditionIsTrue(condition2));
            Assert.Equal(index3, testTable.RowIndicesWhereConditionIsTrue(condition3));

        }

        [Fact]
        public void DeleteWhereTest()
        {
            Table testTable = Table.CreateTestTable();
            Table testTable2 = Table.CreateTestTable();

            Condition condition = new Condition("Name", "=", "Rodolfo");
            Condition condition2 = new Condition("Height", "<", "1.56");
            Condition condition3 = new Condition("Age", ">", "50");

            testTable.DeleteWhere(condition);
            testTable2.DeleteIthRow(0);
            Assert.Equal(testTable2.ToString(), testTable.ToString());

            testTable = Table.CreateTestTable();
            testTable2 = Table.CreateTestTable();
            testTable.DeleteWhere(condition2);
            testTable2.DeleteIthRow(2);
            Assert.Equal(testTable2.ToString(), testTable.ToString());

            testTable = Table.CreateTestTable();
            testTable2 = Table.CreateTestTable();
            testTable.DeleteWhere(condition3);
            testTable2.DeleteIthRow(1);
            testTable2.DeleteIthRow(1);
            Assert.Equal(testTable2.ToString(), testTable.ToString());
        }

        [Fact]
        public void selectTest()
        {
            Table testTable = Table.CreateTestTable();


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