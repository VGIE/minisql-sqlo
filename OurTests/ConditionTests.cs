using DbManager;

namespace OurTests
{
    public class ConditionTests
    {
        //TODO DEADLINE 1A : Create your own tests for Condition
        
        [Fact]
        public void IsTrue()
        {
        Condition cond1i= new Condition("age","<","20");
        Condition cond2i = new Condition("age", "=", "20");
        Condition cond3i = new Condition("age", ">", "20");

        Condition cond1s = new Condition("name", "<", "Paco");
        Condition cond2s = new Condition("name", "=", "Paco");
        Condition cond3s = new Condition("name", ">", "Paco");

        Condition cond1d = new Condition("age", "<", "20");
        Condition cond2d = new Condition("age", "=", "20");
        Condition cond3d = new Condition("age", ">", "20");


        Assert.True(cond1i.IsTrue("15", ColumnDefinition.DataType.Int));
        Assert.False(cond1i.IsTrue("55", ColumnDefinition.DataType.Int));
        Assert.True(cond2i.IsTrue("20", ColumnDefinition.DataType.Int));
        Assert.False(cond2i.IsTrue("25", ColumnDefinition.DataType.Int));
        Assert.True(cond3i.IsTrue("55", ColumnDefinition.DataType.Int));
        Assert.False(cond3i.IsTrue("15", ColumnDefinition.DataType.Int));

        Assert.True(cond1s.IsTrue("a", ColumnDefinition.DataType.String));
        Assert.False(cond1s.IsTrue("PPPP", ColumnDefinition.DataType.String));
        Assert.True(cond2s.IsTrue("Paco", ColumnDefinition.DataType.String));
        Assert.False(cond2s.IsTrue("Ohh que seráaa", ColumnDefinition.DataType.String));
        Assert.True(cond3s.IsTrue("PPPP", ColumnDefinition.DataType.String));
        Assert.False(cond3s.IsTrue("a", ColumnDefinition.DataType.String));

        Assert.True(cond1d.IsTrue("15.5", ColumnDefinition.DataType.Double));
        Assert.False(cond1d.IsTrue("55.5", ColumnDefinition.DataType.Double));
        Assert.True(cond2d.IsTrue("20.0", ColumnDefinition.DataType.Double));
        Assert.False(cond2d.IsTrue("25.53", ColumnDefinition.DataType.Double));
        Assert.True(cond3d.IsTrue("55.5", ColumnDefinition.DataType.Double));
        Assert.False(cond3d.IsTrue("15.5", ColumnDefinition.DataType.Double));
        }
        
    }
}