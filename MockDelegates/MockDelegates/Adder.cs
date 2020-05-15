namespace MockDelegates
{
    public partial class DelegateTests
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
