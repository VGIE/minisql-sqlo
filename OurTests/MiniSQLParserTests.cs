using DbManager;
using DbManager.Parser;
using Xunit;
namespace OurTests
{
    public class MiniSQLParserTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row

        [Fact]
        public void DeleteTests()
        {
            
            Delete deleteTest = new Delete("table", new Condition("edad","=","1"));
            Delete deleteTest3 = new Delete("employee", new Condition("age", ">", "-5124.2456"));
            Delete deleteTest4 = new Delete("testTable", new Condition("column","=","string"));

            //For delete object created
            Assert.Equal(deleteTest,MiniSQLParser.Parse("DELETE FROM table WHERE edad='1'"));
            Assert.NotEqual(deleteTest, MiniSQLParser.Parse("DELETE FROM table"));
            Assert.Equal(deleteTest3, MiniSQLParser.Parse("DELETE FROM employee WHERE age>'-5124.2456'"));
            Assert.NotEqual(deleteTest3, MiniSQLParser.Parse("DELETE FROM employee WHERE employee>'-5124.2456'"));
            Assert.Equal(deleteTest4, MiniSQLParser.Parse("DELETE   FROM testTable     WHERE column='string'"));
            Assert.Equal(deleteTest4, MiniSQLParser.Parse("DELETE FROM testTable WHERE column='string'"));

            //For regex comprobation
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table one"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table"));
            Assert.Null(MiniSQLParser.Parse("Delete From table"));
            Assert.Null(MiniSQLParser.Parse(" DELETE FROM table1 WHERE name='Jacinto' "));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table WHERE age ='32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table WHERE age>= '-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table WHERE age>'-32.6123' "));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table WHERE age >'-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM  table WHERE age>='-32.6123'"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table  WHERE age>-32.6123"));
            Assert.Null(MiniSQLParser.Parse("DELETE FROM table '"));
            

            



        }

        [Fact]
        public void SelectTests()
        {

            //varias columnas y condición
            List<String> columns = new List<string> {"Column1","Column2" };
            Condition condition = new Condition("Column1", "=", "Hola");

            Assert.Equal(new Select("tabla", columns, condition), 
                MiniSQLParser.Parse("SELECT Column1,Column2 FROM tabla WHERE Column1='Hola'"));

            //UNA columna y SIN condición

            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, null),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla"));

            //Condicion con número entero
            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, new Condition("Column1", "=", "42")),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla WHERE Column1='42'"));

            //Condicion con número decimal
            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, new Condition("Column1", ">", "3.14")),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla WHERE Column1>'3.14'"));

            //Condicion con número negativo
            Assert.Equal(new Select("tabla", new List<string> { "Column1" }, new Condition("Column1", "<", "-5")),
                MiniSQLParser.Parse("SELECT Column1 FROM tabla WHERE Column1<'-5'"));

            // Querys malas
            Assert.Null(MiniSQLParser.Parse("SELECT FROM tabla"));
            Assert.Null(MiniSQLParser.Parse("SELECT col1 col2 FROM tabla"));
        }

        [Fact]

        public void CreateTableTest()
        {
            CreateTable table = (new CreateTable("Table", new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Altura")
            }));

            Assert.Equal(table, MiniSQLParser.Parse("CREATE TABLE Table (Nombre TEXT,Edad INT,Altura DOUBLE)"));

            Assert.Null(MiniSQLParser.Parse("CREATE TABLE table"));
            Assert.Null(MiniSQLParser.Parse("CREATE TABLE table (nota int)"));
            Assert.Null(MiniSQLParser.Parse("CREATE TABLE table (nota INT nombre TEXT"));

        }
        [Fact]
        public void DropTableTest()
        {
            Assert.Equal(new DropTable("Test1"), MiniSQLParser.Parse("DROP TABLE Test1"));
            Assert.Null(MiniSQLParser.Parse("DROP table Test2"));
            Assert.Null(MiniSQLParser.Parse("DROP TABLE Test2, Test 3"));
            Assert.Equal(new DropTable("Test4"),MiniSQLParser.Parse("DROP TABLE      Test4"));

        }
        [Fact]
        public void UpdateTests()
        {
            Assert.Equal(new Update("tabla", new List<SetValue>() { new SetValue("column1", "1"), new SetValue("column2", "2") }, new Condition("columna", "=", "valor")), MiniSQLParser.Parse("UPDATE tabla SET column1='1',column2='2' WHERE columna='valor'"));
            Assert.Equal(new Update("tabla", new List<SetValue>() { new SetValue("column1", "Hola"), new SetValue("column2", "2.5") }, new Condition("columna", ">", "3.4")), MiniSQLParser.Parse("UPDATE    tabla    SET    column1='Hola',column2='2.5'    WHERE     columna>'3.4'"));
        }

        [Fact]
        public void GrantTests()
        {
            Assert.Equal(new Grant("INSERT", "TestTable", "User"),MiniSQLParser.Parse("GRANT INSERT ON TestTable TO User"));
            Assert.Equal(new Grant("DELETE", "Table", "Admin"), MiniSQLParser.Parse("GRANT DELETE ON Table TO Admin"));
            Assert.Equal(new Grant("SELECT", "Test", "UserTest"), MiniSQLParser.Parse("GRANT SELECT ON Test TO UserTest"));
            Assert.Equal(new Grant("UPDATE", "TestTa", "User"), MiniSQLParser.Parse("GRANT UPDATE ON TestTa TO User"));
            Assert.Null(MiniSQLParser.Parse("GRANT insert ON T TO User"));
            Assert.Null(MiniSQLParser.Parse("GRANT SELECT ON Test TO UserTest57"));
            Assert.Null(MiniSQLParser.Parse("GRANT DELETE on Table TO Pedro"));
            Assert.Null(MiniSQLParser.Parse("GRANT DROP ON TO Pablo"));
        }

        [Fact]
        public void RevokeTests()
        {
            Assert.Equal(new Revoke("INSERT", "TestTable", "User"), MiniSQLParser.Parse("REVOKE INSERT ON TestTable TO User"));
            Assert.Equal(new Revoke("DELETE", "Table", "Admin"), MiniSQLParser.Parse("REVOKE DELETE ON Table TO Admin"));
            Assert.Equal(new Revoke("SELECT", "Test", "UserTest"), MiniSQLParser.Parse("REVOKE SELECT ON Test TO UserTest"));
            Assert.Equal(new Revoke("UPDATE", "TestTa", "User"), MiniSQLParser.Parse("REVOKE    UPDATE ON TestTa TO User"));
            Assert.Null(MiniSQLParser.Parse("REVOKE insert ON T TO User"));
            Assert.Null(MiniSQLParser.Parse("REVOKE SELECT ON Test TO UserTest57"));
            Assert.Null(MiniSQLParser.Parse("REVOKE DROP ON TO Pablo"));
        }

        [Fact]
        public void AddUserTests()
        {
            Assert.Equal(new AddUser("User", "Password", "Usuario"), MiniSQLParser.Parse("ADD USER (User,Password,Usuario)"));
            Assert.Equal(new AddUser("Pablo", "Contrasena", "Admin"), MiniSQLParser.Parse("ADD      USER (Pablo,Contrasena,Admin)"));
            Assert.Null(MiniSQLParser.Parse("ADD USER (Pablo123,password,mod)"));
            Assert.Null(MiniSQLParser.Parse("ADD USER (Pablo,password)"));
            Assert.Null(MiniSQLParser.Parse("ADD USER (Pablo,password,admin123)"));
        }

        [Fact]
        public void DeleteUserTests()
        {
            Assert.Equal(new DeleteUser("Pablo"), MiniSQLParser.Parse("DELETE USER Pablo"));
            Assert.Equal(new DeleteUser("Pablo"), MiniSQLParser.Parse("DELETE USER  Pablo"));
            Assert.Null(MiniSQLParser.Parse("DELETE USER Pablo123"));
            Assert.Null(MiniSQLParser.Parse("DELETE user Pablo123"));
        }

        [Fact]
        public void CreateSecurityProfileTests()
        {
            Assert.Equal(new CreateSecurityProfile("Moderador"), MiniSQLParser.Parse("CREATE SECURITY PROFILE Moderador"));
            Assert.Equal(new CreateSecurityProfile("Mod"), MiniSQLParser.Parse("CREATE   SECURITY       PROFILE    Mod"));
            Assert.Null(MiniSQLParser.Parse("CREATE SECURITY PROFILE Admin1"));
            Assert.Null(MiniSQLParser.Parse("CREATE security PROFILE Admin"));
        }

        [Fact]
        public void DropSecurityProfileTests()
        {
            Assert.Equal(new DropSecurityProfile("Moderador"), MiniSQLParser.Parse("DROP SECURITY PROFILE Moderador"));
            Assert.Equal(new DropSecurityProfile("Mod"), MiniSQLParser.Parse("DROP   SECURITY       PROFILE    Mod"));
            Assert.Null(MiniSQLParser.Parse("DROP SECURITY PROFILE Admin1"));
            Assert.Null(MiniSQLParser.Parse("DROP security PROFILE Admin"));
            
            
            
        }

        [Fact]
        public void InsertTests()
        {
            List<string> valores = new List<string>{"val1", "val2"};
            List<string> valores2 = new List<string>{"val3", "val4"};
            List<string> valoresEspacios = new List<string> {"val5 val5", "val6 val6 val6", "val8" };
            List<ColumnDefinition> columnas = new List<ColumnDefinition>();
            columnas.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "col1"));
            columnas.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "col2"));
            Table table = new Table("table1", columnas);
            Table table2 = new Table("table2", columnas);

            Insert insertTest1 = new Insert("table1",valores);
            Insert insertTest2 = new Insert("table1", valores);
            Insert insertTest3 = new Insert("table2", valores2);
            Insert insertTest4 = new Insert("table2", valoresEspacios);

            Assert.Equal(insertTest1, MiniSQLParser.Parse("INSERT INTO table1 VALUES ('val1','val2')"));
            Assert.Equal(insertTest2, MiniSQLParser.Parse("INSERT  INTO   table1   VALUES('val1','val2')"));
            Assert.Equal(insertTest3, MiniSQLParser.Parse("INSERT INTO table2 VALUES ('val3','val4')"));
            Assert.Equal(insertTest4, MiniSQLParser.Parse("INSERT     INTO    table2     VALUES   ('val5 val5','val6 val6 val6','val8')"));

            Assert.NotNull(MiniSQLParser.Parse("INSERT INTO table2 VALUES ('val3')"));
            Assert.NotNull(MiniSQLParser.Parse("INSERT INTO table2 VALUES ('val3','a','-53.543')"));
            Assert.NotNull(MiniSQLParser.Parse("INSERT INTO table2 VALUES ('null')"));


            //Regex Comprobation
            Assert.Null(MiniSQLParser.Parse("INSERT INTO table1 VALUES (val1, val2)"));
        }

    }
}