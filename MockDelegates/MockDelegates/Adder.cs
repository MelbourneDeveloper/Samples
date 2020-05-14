namespace MockDelegates
{
    public partial class Tests
    {
        public class Adder : IAdder
        {
            public int Add(int a, int b)
            {
                return a + b;
            }
        }
    }
}
