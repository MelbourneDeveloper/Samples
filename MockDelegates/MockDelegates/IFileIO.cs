// ReSharper disable UnusedMember.Global
namespace MockDelegates
{
    public interface IFileIo
    {
        void WriteData(byte[] data);
        byte[] ReadData();
    }
}
