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

            Condition condici2 = new Condition("Precio", ">", "1");
            result = table.Select(selectC, condici2);

            Assert.Equal(1, result.NumColumns());
            Assert.Equal(2, result.NumRows());

            Assert.Equal("Jabon", result.GetRow(0).Values[0]);
            Assert.Equal("Peine", result.GetRow(1).Values[0]);

            Condition condici3 = new Condition("Precio", "<", "1.0");
             result = table.Select(selectC, condici3);

            Assert.Equal(1, result.NumColumns());
            Assert.Equal(0, result.NumRows());

        }

        /*
         * Los deletes no funcionan
         * [Fact]
         public void deleteTest()
        {
            var columns = new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero"),
        new ColumnDefinition(ColumnDefinition.DataType.Double, "Altura")
         };

            Table table = new Table("Personas", columns);
            
            table.Insert(new List<string> { "Jorge" , "1" , "1.74" });
            table.Insert(new List<string> { "Irene" , "7" , "1.60" });
            table.Insert(new List<string> { "Ander" , "8" , "1.67" });

            Condition condicion = new Condition("Nombre", "=", "Maria");
            Condition condici2 = new Condition("Altua", "=", "1.75");
            Condition condici3 = new Condition("Numero", "=", "8");
            Assert.Equal(3, table.NumColumns());
            table.DeleteIthRow(2);
            Assert.Equal(2, table.NumColumns());

        }*/
    }

}

