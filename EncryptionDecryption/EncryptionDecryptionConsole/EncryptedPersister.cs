using EncryptionDecryption;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace EncryptionDecryptionConsole
{
    public class EncryptedPersister
    {
        #region Fields
        private readonly ICryptographyService _cryptographyService;
        private readonly IIOService _ioService;
        #endregion

        #region Constructor
        public EncryptedPersister(ICryptographyService cryptographyService, IIOService ioService)
        {
            _cryptographyService = cryptographyService;
            _ioService = ioService;
        }
        #endregion

        #region Public Methods
        public async Task SaveAsync<T>(string filename, T model)
        {
            var json = JsonConvert.SerializeObject(model);

            var encryptedJson = _cryptographyService.Encrypt(json);

            await _ioService.SaveAsync(filename, encryptedJson);
        }

        public async Task<T> LoadAsync<T>(string filename)
        {
            var encryptedJson = await _ioService.LoadAsync(filename);

            var plainTextJson = _cryptographyService.Decrypt(encryptedJson);

            var model = JsonConvert.DeserializeObject<T>(plainTextJson);

            return model;
        }
        #endregion
    }
}
