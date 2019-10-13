namespace EncryptionDecryption
{
    public interface ICryptographyService : IOneWayEncryptionService
    {
        string Decrypt(string text);
    }
}