using System.Linq.Expressions;
using DbManager;
using DbManager.Parser;

namespace OurTests
{
    public class UpdateTests
    {
        [Fact]
        public void TestUpdateInitialization()
        {
            string table = "Datos";
            var setValues = new List<SetValue>
        {
        new SetValue("Nombre", "Araitz"),
        new SetValue("Edad", "20")
        };
            Condition condicion = new Condition("Edad", "=", "22");
            Update updateQuery = new Update(table, setValues, condicion);

            Assert.Equal("Datos", updateQuery.Table);
            Assert.Equal(2, updateQuery.Columns.Count);
            Assert.Equal("Nombre", updateQuery.Columns[0].ColumnName);
            Assert.Equal("Araitz", updateQuery.Columns[0].Value);
            Assert.Equal("Edad", updateQuery.Where.ColumnName);
        }

        [Fact]
        public void TestUpdateExecuteSuccess()
        {
            Database db = Database.CreateTestDatabase();

            Condition condicion = new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            List<SetValue> nuevos = new List<SetValue>
            {
                new SetValue(Table.TestColumn2Name, "1.70")
            };

            Update updateQ = new Update(Table.TestTableName, nuevos, condicion);
            string result = updateQ.Execute(db);

            Assert.Equal(Constants.UpdateSuccess, result);

        }

        [Fact]
        public void UpdateInvalidCondition()
        {
            Database database = Database.CreateTestDatabase();

            Condition condition = new Condition(null, "=", null);

            List<SetValue> up = new List<SetValue>
            {
                new SetValue(Table.TestColumn2Name, "1.70"),
                new SetValue(Table.TestColumn3Name, "30")
            };

            Update updateQ = new Update(Table.TestTableName, up, condition);
            string result = updateQ.Execute(database);

            Assert.Equal(database.LastErrorMessage, result);
        }

        [Fact]
        public void UpdateTableDoesNotExist()
        {
            Database db = Database.CreateTestDatabase();

            Condition condition = new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            List<SetValue> values = new List<SetValue>
            {
                new SetValue(Table.TestColumn3Name, "30")
            };

            Update update = new Update("TablaInexistente", values, condition);

            string result = update.Execute(db);

            Assert.Equal(db.LastErrorMessage, result);
            Assert.NotEqual(Constants.UpdateSuccess, result);
        }

        [Fact]
        public void UpdateWithoutCondition()
        {
            Database db = Database.CreateTestDatabase();

            List<SetValue> values = new List<SetValue>
            {
                new SetValue(Table.TestColumn2Name, "1.80")
            };

            Update update = new Update(Table.TestTableName, values, null);

            string result = update.Execute(db);

            Assert.Equal(db.LastErrorMessage, result);
            Assert.NotEqual(Constants.UpdateSuccess, result);
        }

        [Fact]
        public void UpdateColumnNonExisting()
        {
            Database db = Database.CreateTestDatabase();

            Condition condition = new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            List<SetValue> values = new List<SetValue>
            {
                new SetValue("ColumnaInexistente", "10")
            };

            Update update = new Update(Table.TestTableName, values, condition);

            string result = update.Execute(db);

            Assert.Equal(db.LastErrorMessage, result);
        }

        [Fact]
        public void UpdateConditionFalse()
        {
            Database db = Database.CreateTestDatabase();

            Condition condition = new Condition(Table.TestColumn1Name, "=", "NoExiste");

            List<SetValue> values = new List<SetValue>
            {
                new SetValue(Table.TestColumn3Name, "40")
            };

            Update update = new Update(Table.TestTableName, values, condition);

            string result = update.Execute(db);

            Assert.Equal(Constants.UpdateSuccess, result);
        }

        [Fact]
        public void UpdateMultipleColumns()
        {
            Database db = Database.CreateTestDatabase();

            Condition condition = new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            List<SetValue> values = new List<SetValue>
            {
                new SetValue(Table.TestColumn2Name, "1.90"),
                new SetValue(Table.TestColumn3Name, "40")
            };

            Update update = new Update(Table.TestTableName, values, condition);

            string result = update.Execute(db);

            Assert.Equal(Constants.UpdateSuccess, result);
        }

        [Fact]
        public void UpdateWithoutColumns()
        {
            Database db = Database.CreateTestDatabase();

            Condition condition = new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            List<SetValue> values = new List<SetValue>();

            Update update = new Update(Table.TestTableName, values, condition);

            string result = update.Execute(db);

            Assert.Equal(db.LastErrorMessage, result);
        }

        [Fact]
        public void UpdateWrongType()
        {
            Database db = Database.CreateTestDatabase();

            Condition condition = new Condition(Table.TestColumn1Name, "=", Table.TestColumn1Row1);

            List<SetValue> values = new List<SetValue>
            {
                new SetValue(Table.TestColumn3Name, "text")
            };

            Update update = new Update(Table.TestTableName, values, condition);

            string result = update.Execute(db);

            Assert.Equal(db.LastErrorMessage, result);
        }
    }
}
