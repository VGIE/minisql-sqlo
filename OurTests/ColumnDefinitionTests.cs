using DbManager;

namespace OurTests
{
    public class ColumnDefinitionsTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table

        [Fact]
        public void TestConstructor()
        {
            ColumnDefinition columnString = new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre");
            ColumnDefinition columnInt = new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero");
            ColumnDefinition columnDouble = new ColumnDefinition(ColumnDefinition.DataType.Double, "Precio");

            Assert.Equal(ColumnDefinition.DataType.String, columnString.Type);
            Assert.Equal("Nombre", columnString.Name);

            Assert.Equal(ColumnDefinition.DataType.Int, columnInt.Type);
            Assert.Equal("Numero", columnInt.Name);

            Assert.Equal(ColumnDefinition.DataType.Double, columnDouble.Type);
            Assert.Equal("Precio", columnDouble.Name);

        }


        [Fact]
        public void TestAsText()
        {
            ColumnDefinition columnString = new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre->Juan");
            ColumnDefinition columnInt = new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero->10");
            ColumnDefinition columnDouble = new ColumnDefinition(ColumnDefinition.DataType.Double, "Precio->5.2");

            string textString = columnString.AsText();
            string textInt = columnInt.AsText();
            string textDouble = columnDouble.AsText();

            Assert.Equal("Nombre[ARROW]Juan->String", textString);
            Assert.Equal("Numero[ARROW]10->Int", textInt);
            Assert.Equal("Precio[ARROW]5.2->Double", textDouble);

        }

        [Fact]
        public void TestParse()
        {
            ColumnDefinition columnString = new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre->Juan");
            ColumnDefinition columnInt = new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero->10");
            ColumnDefinition columnDouble = new ColumnDefinition(ColumnDefinition.DataType.Double, "Precio->5.2");

            string textString = columnString.AsText();
            string textInt = columnInt.AsText();
            string textDouble = columnDouble.AsText();

            ColumnDefinition parseString = ColumnDefinition.Parse(textString);
            ColumnDefinition parseInt = ColumnDefinition.Parse(textInt);
            ColumnDefinition parseDouble = ColumnDefinition.Parse(textDouble);

            Assert.Equal(ColumnDefinition.DataType.String, parseString.Type);
            Assert.Equal("Nombre->Juan", parseString.Name);

            Assert.Equal(ColumnDefinition.DataType.Int, parseInt.Type);
            Assert.Equal("Numero->10", parseInt.Name);

            Assert.Equal(ColumnDefinition.DataType.Double, parseDouble.Type);
            Assert.Equal("Precio->5.2", parseDouble.Name);

        }

    }

}