namespace EncryptionDecryption
{
    public interface ICryptographyService
    {
        string Encrypt(string text);
        string Decrypt(string text);
    }
}