using System.Text;

namespace MockDelegates
{
    public class StringConcatenatorWithDependencies 
    {
        IFileIO _fileIO;

        public StringConcatenatorWithDependencies(IFileIO fileIO)
        {
            _fileIO = fileIO;
        }

        public string ConcatenateString(string a, string b)
        {
            var returnValue = a + b;
            var data = Encoding.UTF8.GetBytes(returnValue);
            _fileIO.WriteData(data);
            return returnValue;
        }
    }
}
