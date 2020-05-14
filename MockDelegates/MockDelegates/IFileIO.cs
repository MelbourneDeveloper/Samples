namespace MockDelegates
{
    public interface IFileIO
    {
        void WriteData(byte[] data);
        byte[] ReaderData();
    }
}
