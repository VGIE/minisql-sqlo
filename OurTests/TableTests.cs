using DbManager;

namespace OurTests
{
    public class TableTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table

        [Fact]
        public void constructorTest1()
        {
            var columns = new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero")
         };

            Table table = new Table("Personas", columns);

            Assert.Equal(2, columns.Count);
            Assert.Equal("Nombre", columns[0].Name);
            Assert.Equal(ColumnDefinition.DataType.String, columns[0].Type);
            Assert.Equal("Numero", columns[1].Name);
            Assert.Equal(ColumnDefinition.DataType.Int, columns[1].Type);

        }

        [Fact]
        public void TableSelectWithoutConditionTest()
        {
            var columns = new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero")
         };

            Table table = new Table("Personas", columns);

            table.Insert(new List<string> { "Ana", "3" });
            table.Insert(new List<string> { "Marcos", "5" });
            List<string> selectC = new List<string>();
            selectC.Add("Nombre");
            Table result = table.Select(selectC, null);

            Assert.Equal("Ana", result.GetRow(0).Values[0]);
            Assert.Equal("Marcos", result.GetRow(1).Values[0]);
            // select sobre una tabla nombre inexistente
            Table t = new Table("Persona", null);
            Table result2 = t.Select(null, null);
            Assert.Null(result2);
            Table result3 = t.Select(selectC, null);
            
           
        }
        [Fact]
        public void TableSelectWithConditionTest()
        {
            var columns = new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero")
         };

            Table table = new Table("Personas", columns);
            table.Insert(new List<string> { "Ana", "3" });
            table.Insert(new List<string> { "Marcos", "5" });
            List<string> selectC = new List<string>();
            selectC.Add("Nombre");

            Condition condicion = new Condition("Numero", "=", "3");

            Table result = table.Select(selectC, condicion);

            Assert.Equal(1, result.NumColumns());
            Assert.Equal(1, result.NumRows());

            Assert.Equal("Ana", result.GetRow(0).Values[0]);

            Condition condici2 = new Condition("Numero", "=", "1");
            result = table.Select(selectC, condici2);

            Assert.Equal(1, result.NumColumns());
            Assert.Equal(0, result.NumRows());

            Condition condici3 = new Condition("Numero", ">", "1");
            result = table.Select(selectC, condici3);

            Assert.Equal(1, result.NumColumns());
            Assert.Equal(2, result.NumRows());

            Assert.Equal("Ana", result.GetRow(0).Values[0]);
            Assert.Equal("Marcos", result.GetRow(1).Values[0]);
        }
        [Fact]
        public void conditionWithDoubleTest()
        {
            var columns = new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Double, "Precio")
         };

            Table table = new Table("Producto", columns);
            table.Insert(new List<string> { "Jabon", "12.02" });
            table.Insert(new List<string> { "Peine", "3.5" });

            List<string> selectC = new List<string>();
            selectC.Add("Nombre");

            Condition condicion = new Condition("Precio", "=", "3.5");

            Table result = table.Select(selectC, condicion);

            Assert.Equal(1, result.NumColumns());
            Assert.Equal(1, result.NumRows());

            Assert.Equal("Peine", result.GetRow(0).Values[0]);

            Condition condici2 = new Condition("Precio", ">", "3.4999999999");
            result = table.Select(selectC, condici2);

            Assert.Equal(1, result.NumColumns());
            Assert.Equal(2, result.NumRows());

            Assert.Equal("Jabon", result.GetRow(0).Values[0]);
            Assert.Equal("Peine", result.GetRow(1).Values[0]);

            Condition condici3 = new Condition("Precio", "<", "3.5");
            result = table.Select(selectC, condici3);

            Assert.Equal(1, result.NumColumns());
            Assert.Equal(0, result.NumRows());

        }

        [Fact]
        public void deleteTest()
        {
            var columns = new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero"),
        new ColumnDefinition(ColumnDefinition.DataType.Double, "Altura")
         };

            Table table = new Table("Personas", columns);

            table.Insert(new List<string> { "Jorge", "1", "1.74" });
            table.Insert(new List<string> { "Irene", "7", "1.60" });
            table.Insert(new List<string> { "Ander", "8", "1.67" });

            Condition condicion = new Condition("Nombre", "=", "Jorge");
            Condition condici2 = new Condition("Altura", "<", "1.75");

            Assert.Equal(3, table.NumColumns());
            table.DeleteWhere(condicion);
            Assert.Equal(2, table.NumRows());

            table.DeleteWhere(condici2);
            Assert.Equal(0, table.NumRows());

        }

        [Fact]
        public void RowSetAndGetValueWithNotEnoughValues()
        {
            var columns = new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero"),
        new ColumnDefinition(ColumnDefinition.DataType.Double, "Altura")
         };

            Table table = new Table("Personas", columns);

            Assert.False(table.Insert(new List<string> { "Jorge", "1" }));

            Assert.False(table.Insert(new List<string> { }));

            Row row = new Row(columns, new List<string>() { });
            row.SetValue("Nombre", "Juan");
            Assert.Equal("Juan", row.GetValue("Nombre"));
        }

        [Fact]
        public void TableSelectWithoutConditionAndDisorderedColumns()
        {
            Table table = Table.CreateTestTable();

            Table select = table.Select(new List<string>
            {
                Table.TestColumn2Name, Table.TestColumn3Name, Table.TestColumn1Name
            }, null);

            Assert.Equal(Table.TestColumn2Name, select.GetColumn(0).Name);
            Assert.Equal(Table.TestColumn3Name, select.GetColumn(1).Name);
            Assert.Equal(Table.TestColumn1Name, select.GetColumn(2).Name);

            Assert.Equal(Table.TestColumn2Row1, select.GetRow(0).Values[0]);
            Assert.Equal(Table.TestColumn3Row1, select.GetRow(0).Values[1]);
            Assert.Equal(Table.TestColumn1Row1, select.GetRow(0).Values[2]);

            Assert.Equal(Table.TestColumn2Row2, select.GetRow(1).Values[0]);
            Assert.Equal(Table.TestColumn3Row2, select.GetRow(1).Values[1]);
            Assert.Equal(Table.TestColumn1Row2, select.GetRow(1).Values[2]);
        }
    }

}

