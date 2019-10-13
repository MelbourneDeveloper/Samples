using EncryptionDecryptionConsole;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EncryptionDecryptionTests
{
    public class EncryptedPersisterTests : AESEncryptionTestsBase
    {
        #region Fields
        private const string filename = "test";
        private const string Username = "Bob";
        private const string Password = "Password123";
        private const string EncryptedText = "f4e9a35aa90ba645474e53271ee6cff4e0cb1379f10ae6a784eb5f00ce6666d06d9c0562dabb3758975006fd8aa7b7e3";
        #endregion

        [Test]
        public async Task TestSaveAsync()
        {
            var ioServiceMock = new Mock<IIOService>();

            var encryptedPersister = new EncryptedPersister(_symmetricAlgorithmCryptographyService, ioServiceMock.Object);

            await encryptedPersister.SaveAsync(filename, new Credentials { Username = Username, Password = Password });

            ioServiceMock.Verify(s => s.SaveAsync(filename, EncryptedText), Times.Once);

            ioServiceMock.VerifyNoOtherCalls();
        }

        [Test]
        public async Task TestLoadAsync()
        {
            var ioServiceMock = new Mock<IIOService>();

            ioServiceMock.Setup(s => s.LoadAsync(filename)).Returns(Task.FromResult(EncryptedText));

            var encryptedPersister = new EncryptedPersister(_symmetricAlgorithmCryptographyService, ioServiceMock.Object);

            var credentials = await encryptedPersister.LoadAsync<Credentials>(filename);

            ioServiceMock.Verify(s => s.LoadAsync(filename), Times.Once);

            ioServiceMock.VerifyNoOtherCalls();

            Assert.AreEqual(Username, credentials.Username);
            Assert.AreEqual(Password, credentials.Password);
        }
    }
}
