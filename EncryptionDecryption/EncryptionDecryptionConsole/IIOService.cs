using System.Threading.Tasks;

namespace EncryptionDecryptionConsole
{
    public interface IIOService
    {
        Task SaveAsync(string filename, string text);
        Task<string> LoadAsync(string filename);
    }
}