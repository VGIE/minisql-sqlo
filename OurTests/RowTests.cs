using DbManager;

namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row
        /*
        [Fact]
        public void Test1()
        {
        }
        */
        [Fact]
        public void asTextTest() 
        {
           var columns = new List<ColumnDefinition>
           {
            new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
            new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero"),
            new ColumnDefinition(ColumnDefinition.DataType.Double, "Decimal")
           };
            List<String> valores = new List<String> { "Noa", "1", "0.5" };
            Row r=new Row(columns, valores);
            string prue = r.AsText();
            string resultado = "Noa:1:0.5";
            Assert.Equal(resultado, prue);
        }

        [Fact]
        public void asTextDelimTest() 
        {
            var columns = new List<ColumnDefinition>
           {
            new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
            new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero"),
            new ColumnDefinition(ColumnDefinition.DataType.Double, "Decimal")
           };
            List<String> valores = new List<String> { "Noa:", "1", "0.5" };
            Row r = new Row(columns, valores);
            string prue = r.AsText();
            string resultado = "Noa[SEPARATOR]:1:0.5";
            Assert.Equal(resultado, prue);
        }

        [Fact]
        public void parseTest() 
        {
            var columns = new List<ColumnDefinition>
           {
            new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
            new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero"),
            new ColumnDefinition(ColumnDefinition.DataType.Double, "Decimal")
           };

           
            List<String> valores = new List<String> { "Noa", "1", "0.5" };
            Row resultado = new Row(columns, valores);
            string values = resultado.AsText();
            Row prue=Row.Parse(columns,values);

            Assert.Equal(resultado.Values, prue.Values);
        }

        [Fact]
        public void parseDelimTest() 
        {
            var columns = new List<ColumnDefinition>
           {
            new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
            new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero"),
            new ColumnDefinition(ColumnDefinition.DataType.Double, "Decimal")
           };


            List<String> valores = new List<String> { "Noa:", "1", "0.5" };
            
            Row resultado = new Row(columns, valores);
            string values ="Noa[SEPARATOR]:1:0.5";
            Console.WriteLine(values);
            Row prue = Row.Parse(columns, values);

            Assert.Equal(resultado.Values, prue.Values);
        }

        [Fact]
        public void getSetTest() {
            var columns = new List<ColumnDefinition>
           {
            new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
            new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero"),
            new ColumnDefinition(ColumnDefinition.DataType.Double, "Decimal")
           };


            List<String> valores = new List<String> { "Noa", "1", "0.5" };
            Row r=new Row(columns, valores);
            string prue = r.GetValue("Nombre");
            Assert.Equal("Noa",prue);
            r.SetValue("Nombre", "Wiwi");
            prue = r.GetValue("Nombre");
            Assert.Equal("Wiwi", prue);
        }
    }
}