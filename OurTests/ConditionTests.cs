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


        Assert.True();
        }
        
    }
}