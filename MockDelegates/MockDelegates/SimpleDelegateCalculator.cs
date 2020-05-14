namespace MockDelegates
{
    public class SimpleDelegateCalculator
    {
        private readonly Add _add;

        public SimpleDelegateCalculator(Add add)
        {
            _add = add;
        }

        public int Add(int a, int b)
        {
            return _add(a, b);
        }
    }
}
