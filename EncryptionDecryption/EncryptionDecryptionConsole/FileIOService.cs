using System.IO;
using System.Threading.Tasks;

namespace EncryptionDecryptionConsole
{
    public class FileIOService : IIOService
    {
        public Task<string> LoadAsync(string filename)
        {
            return File.ReadAllTextAsync(filename);
        }

        public Task SaveAsync(string filename, string text)
        {
            return File.WriteAllTextAsync(filename, text);
        }
    }
}
