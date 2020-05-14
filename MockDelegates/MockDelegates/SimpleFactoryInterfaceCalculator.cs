namespace MockDelegates
{
    public class SimpleFactoryInterfaceCalculator
    {
        IAdder _adder;

        public SimpleFactoryInterfaceCalculator(CreateInstance<IAdder> createAdder)
        {
            _adder = createAdder("simple");
        }

        public int Add(int a, int b)
        {
            return _adder.Add(a, b);
        }
    }
}
