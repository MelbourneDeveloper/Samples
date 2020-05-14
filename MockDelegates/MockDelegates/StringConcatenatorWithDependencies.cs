using System.Text;

namespace MockDelegates
{
    public class StringConcatenatorWithDependencies 
    {
        private readonly IFileIo _fileIo;

        public StringConcatenatorWithDependencies(IFileIo fileIo)
        {
            _fileIo = fileIo;
        }

        public string ConcatenateString(string a, string b)
        {
            var returnValue = a + b;
            var data = Encoding.UTF8.GetBytes(returnValue);
            _fileIo.WriteData(data);
            return returnValue;
        }
    }
}
