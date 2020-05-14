namespace MockDelegates
{
    public class SimpleInterfaceCalculator
    {
        private readonly IAdder _adder;

        public SimpleInterfaceCalculator(IAdder adder)
        {
            _adder = adder;
        }

        public int Add(int a, int b)
        {
            return _adder.Add(a, b);
        }
    }
}
