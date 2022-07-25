namespace Xrm.Application.Interfaces
{
    public interface IFileSystem
    {
        void Write(string path, byte[] bytes);

        void WriteUtf8(string path, string text);

        byte[] Read(string path);

        string ReadUtf8(string path);

        void Delete(string path);

        bool Exists(string path);

        string[] ListFiles(string folderPath);

        void MoveFile(string from, string to);
    }
}
