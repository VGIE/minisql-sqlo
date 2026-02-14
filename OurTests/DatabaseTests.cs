using DbManager;
using DbManager.Parser;

namespace OurTests
{
    public class UnitTest1
    {
        //TODO DEADLINE 1B : Create your own tests for Database

        [Fact]
        public void testAddTable()
        {
            Database database = Database.CreateTestDatabase();
            Table table = Table.CreateTestTable("testTable");
            bool add = database.AddTable(table);

            Assert.True(add);
        }

        [Fact]
        public void testTableByNameOk()
        {
            Database database = Database.CreateTestDatabase();
            Table table = database.TableByName(Table.TestTableName);

            Assert.NotNull(table);
            Assert.Equal(Table.TestTableName, table.Name);
        }

        [Fact]
        public void testTableByNameNull()
        {
            Database database = Database.CreateTestDatabase();
            Table table = database.TableByName("newTable");

            Assert.Null(table);
        }

        [Fact]
        public void testCreateTableOk()
        {
            Database database = Database.CreateTestDatabase();
            bool create = database.CreateTable("new table", new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero"),
        new ColumnDefinition(ColumnDefinition.DataType.Double, "Precio")
         });

            Assert.True(create);
            Assert.Equal(Constants.CreateTableSuccess, database.LastErrorMessage);
        }

        [Fact]
        public void testCreateTableExists()
        {
            Database database = Database.CreateTestDatabase();
            bool create = database.CreateTable(Table.TestTableName, new List<ColumnDefinition>
                  {
        new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
        new ColumnDefinition(ColumnDefinition.DataType.Int, "Numero"),
        new ColumnDefinition(ColumnDefinition.DataType.Double, "Precio")
         });

            Assert.False(create);
            Assert.Equal(Constants.TableAlreadyExistsError, database.LastErrorMessage);
        }

        [Fact]
        public void testCreateTableWithoutColumns()
        {
            Database database = Database.CreateTestDatabase();
            bool create = database.CreateTable("new table", new List<ColumnDefinition>
            { });

            Assert.False(create);
            Assert.Equal(Constants.DatabaseCreatedWithoutColumnsError, database.LastErrorMessage);
        }

        [Fact]
        public void testDropTable()
        {
            Database database = Database.CreateTestDatabase();
            bool drop = database.DropTable(Table.TestTableName);

            Assert.Null(database.TableByName(Table.TestTableName));
            Assert.True(drop);
        }

        [Fact]
        public void testInsertOk()
        {
            Database database = Database.CreateTestDatabase();
            bool insert = database.Insert(Table.TestTableName, new List<String>
                  { "Noa", "1", "0.5"});

            Assert.True(insert);
            Assert.Equal(Constants.InsertSuccess, database.LastErrorMessage);
        }

        [Fact]
        public void testInsertTableNotExist()
        {
            Database database = Database.CreateTestDatabase();
            bool insert = database.Insert("new table", new List<String>
                  { "Noa", "1", "0.5"});

            Assert.False(insert);
            Assert.Equal(Constants.TableDoesNotExistError, database.LastErrorMessage);
        }

        [Fact]
        public void testInsertColumnDontMatch()
        {
            Database database = Database.CreateTestDatabase();
            Table table = database.TableByName(Table.TestTableName);
            bool insertMenos = database.Insert(Table.TestTableName, new List<String>
                  { "Noa"});

            Assert.False(insertMenos);
            Assert.Equal(Constants.ColumnCountsDontMatch, database.LastErrorMessage);

            bool insertMas = database.Insert(Table.TestTableName, new List<String>
                  { "Noa", "1.80", "30", "Dato1"});

            Assert.False(insertMas);
            Assert.Equal(Constants.ColumnCountsDontMatch, database.LastErrorMessage);
        }

        [Fact]
        public void testSelectOk()
        {
            Database database = Database.CreateTestDatabase();
            Condition condition = new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            Table select = database.Select(Table.TestTableName, new List<String>
                  { Table.TestColumn1Name}, condition);

            Assert.Equal(Table.TestColumn1Row1, select.GetRow(0).Values[0]);
        }

        [Fact]
        public void testSelectNull()
        {
            Database database = Database.CreateTestDatabase();
            Condition condition = new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            Table select = database.Select(Table.TestTableName, new List<String>
                  {"nada"}, condition);

            Assert.Null(select);
            Assert.Equal(Constants.ColumnDoesNotExistError, database.LastErrorMessage);
        }

        [Fact]
        public void testDeletWhereOk()
        {
            Database database = Database.CreateTestDatabase();
            Table table = database.TableByName(Table.TestTableName);
            Condition condition = new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            bool delet = database.DeleteWhere(Table.TestTableName, condition);

            Assert.True(delet);
            Assert.Equal(Constants.DeleteSuccess, database.LastErrorMessage);

            for (int i = 0; i < table.NumRows(); i++)
            {
                Row row = table.GetRow(i);
                Assert.NotEqual(Table.TestColumn1Row1, row.GetValue(Table.TestColumn1Name));
            }
        }

        [Fact]
        public void testColumnConditionNotExist()
        {
            Database database = Database.CreateTestDatabase();
            Condition condition = new Condition("noColumn", "=", "noValor");

            bool delet = database.DeleteWhere(Table.TestTableName, condition);

            Assert.False(delet);
            Assert.Equal(Constants.ColumnDoesNotExistError, database.LastErrorMessage);

        }

        [Fact]
        public void testUpdateOk()
        {
            Database database = Database.CreateTestDatabase();
            Table table = database.TableByName(Table.TestTableName);
            Condition condition = new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            List<SetValue> up = new List<SetValue>
            {
                new SetValue(Table.TestColumn2Name, "1.70"),
                new SetValue(Table.TestColumn3Name, "30")
            };

            bool update = database.Update(Table.TestTableName, up, condition);

            Assert.True(update);
            Assert.Equal(Constants.UpdateSuccess, database.LastErrorMessage);

            for (int i = 0; i < table.NumRows(); i++)
            {
                Row row = table.GetRow(i);

                if (row.GetValue(Table.TestColumn1Name) == Table.TestColumn1Row1)
                {
                    Assert.Equal("1.70", row.GetValue(Table.TestColumn2Name));
                    Assert.Equal("30", row.GetValue(Table.TestColumn3Name));
                }
                else
                {
                    Assert.NotEqual("1.70", row.GetValue(Table.TestColumn2Name));
                    Assert.NotEqual("30", row.GetValue(Table.TestColumn3Name));
                }
            }
        }

        [Fact]
        public void testSaveAndLoad()
        {
            Database database = Database.CreateTestDatabase();

            bool save = database.Save(Table.TestTableName);
            Assert.True(save);

            Database load = Database.Load(Table.TestTableName, Database.AdminUsername, Database.AdminPassword);
            Assert.NotNull(load);

            Table table = load.TableByName(Table.TestTableName);
            Assert.NotNull(table);

            Assert.Equal(3, table.NumColumns());
            Assert.Equal(3, table.NumRows());
        }
    }
}