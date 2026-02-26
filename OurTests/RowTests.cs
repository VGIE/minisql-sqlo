using DbManager;
using Xunit;
namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row

        [Fact]
        public void GetValueTest()
        {
            Row nullRow = new Row(null,null);

            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "employee_name"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "age"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "years_Worked"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "salary")
            };

            List<string> rowValues = new List<string>()
            {
                "jacinto","37","5","36859.23"
            };
            Row testRow = new Row(columns, rowValues);

            Assert.Equal("jacinto", testRow.GetValue("employee_name"));
            Assert.Equal("37", testRow.GetValue("age"));
            Assert.Equal("5", testRow.GetValue("years_Worked"));
            Assert.Equal("36859.23", testRow.GetValue("salary"));
            Assert.Null(testRow.GetValue(""));

            Assert.NotEqual("maider", testRow.GetValue("employee_name"));
            Assert.NotEqual("37.5", testRow.GetValue("age"));
            Assert.NotEqual("hola", testRow.GetValue("years_Worked"));
            Assert.NotEqual("46859.23", testRow.GetValue("salary"));

            Assert.Null(nullRow.GetValue("test"));
            Assert.Null(nullRow.GetValue(""));

        }
        [Fact]
        public void SetValueTest()
        {
            Row nullRow = new Row(null, null);
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "name"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "age"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "years_Worked"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "salary")
            };

            List<string> rowValues = new List<string>()
            {
                "jacinto","37","5","36859.23"
            };
            Row testRow = new Row(columns, rowValues);


            testRow.SetValue("name", "maider");
            testRow.SetValue("age", "25");
            testRow.SetValue("years_Worked", "10");
            testRow.SetValue("salary", "25350.00");

            Assert.Equal("maider", testRow.GetValue("name"));
            Assert.Equal("25", testRow.GetValue("age"));
            Assert.Equal("10", testRow.GetValue("years_Worked"));
            Assert.Equal("25350.00", testRow.GetValue("salary"));

            Assert.NotEqual("Maider", testRow.GetValue("name"));
            Assert.NotEqual("37", testRow.GetValue("age"));
            Assert.NotEqual("maider", testRow.GetValue("years_Worked"));
            Assert.NotEqual("36859.23", testRow.GetValue("salary"));

            nullRow.SetValue("hola", "adios");
            Assert.Null(nullRow.GetValue("hola"));

        }
        [Fact]
        public void SetValueDifferentSizes()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "name"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "age"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "years_Worked"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "salary")
            };

            List<string> rowValues = new List<string>()
            {
                "jacinto","37"
            };

            Row testRow = new Row(columns, rowValues);

            testRow.SetValue("years_Worked","5");
            Assert.Equal("5", testRow.GetValue("years_Worked"));

            rowValues = new List<string>()
            {
                "jacinto","37"
            };

            Row testRow2 = new Row(columns, rowValues);
            Assert.Equal("37", testRow2.GetValue("age"));
            testRow2.SetValue("salary", "10000");
            Assert.Equal("10000", testRow2.GetValue("salary"));
            
        }
        [Fact]
        public void IsTrueTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "age"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "years_Worked"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "salary")
            };

            List<string> rowValues = new List<string>()
            {
                "jacinto","37","5","36859.23"
            };
            Row testRow = new Row(columns, rowValues);

            Assert.True(testRow.IsTrue(new Condition("name", "=", "jacinto")));
            Assert.True(testRow.IsTrue(new Condition("age", "<", "65")));
            Assert.True(testRow.IsTrue(new Condition("years_Worked", "=", "5")));
            Assert.True(testRow.IsTrue(new Condition("salary", ">", "30000")));

            Assert.False(testRow.IsTrue(new Condition("name", "=", "Jacinto")));
            Assert.False(testRow.IsTrue(new Condition("age", ">", "65")));
            Assert.False(testRow.IsTrue(new Condition("years_Worked", "=", "500")));
            Assert.False(testRow.IsTrue(new Condition("salary", "<", "30000")));

            Assert.False(testRow.IsTrue(null));
        }

        [Fact]
        public void AsTextTest()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "age"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "years_Worked"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "salary")
            };

            List<string> rowValues = new List<string>()
            {
                "jacin:to","37",":","36859.23"
            };
            List<ColumnDefinition> columns2 = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "name"),

            };

            List<string> rowValues2 = new List<string>()
            {
                "m:aid:er"
            };
            Row testRow = new Row(columns, rowValues);
            Row testRow2 = new Row(columns2, rowValues2);

            Assert.Equal("jacin[SEPARATOR]to:37:[SEPARATOR]:36859.23", testRow.AsText());
            Assert.Equal("m[SEPARATOR]aid[SEPARATOR]er", testRow2.AsText());

            Assert.NotEqual("jacin:to[SEPARATOR]37[SEPARATOR]:[SEPARATOR]36859.23", testRow.AsText());
            Assert.NotEqual("maider[SEPARATOR]", testRow2.AsText());

            Row nullRow = new Row(null, null);
            Assert.Null(nullRow.AsText());

        }
        [Fact]
        public void AsTextWithNulls()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "age"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "years_Worked"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "salary")
            };

            List<string> rowValues = new List<string>()
            {
                "jacin:to",":"
            };
            Row testRow = new Row(columns, rowValues);

            Assert.Equal("jacin[SEPARATOR]to:[SEPARATOR]", testRow.AsText());

            testRow.SetValue("salary", "10000");

            Assert.Equal("jacin[SEPARATOR]to:[SEPARATOR]::10000", testRow.AsText());
        }
        [Fact]
        public void ParseTest()
        {

            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "age"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "years_Worked"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "salary")
            };

            List<string> rowValues = new List<string>()
            {
                "jacinto","37","5","36859.23"
            };
            Row testRow = new Row(columns, rowValues);

            Assert.Equal(testRow.AsText(), Row.Parse(columns, testRow.AsText()).AsText());
        }

    }
}