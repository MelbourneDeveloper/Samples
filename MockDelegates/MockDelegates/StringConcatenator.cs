namespace MockDelegates
{
    public class StringConcatenator
    {
        Add<string> _add;

        public StringConcatenator(Add<string> add)
        {
            _add = add;
        }

        public string ConcatenateString(string a, string b)
        {
            return _add(a, b);
        }
    }
}
