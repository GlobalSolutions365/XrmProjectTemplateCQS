using System.IO;
using System.Linq;
using Xrm.Application.Interfaces;

namespace PhysicalFileSystem
{
    public class FileSystem : IFileSystem
    {
        public byte[] Read(string path) => File.ReadAllBytes(path);

        public string ReadUtf8(string path) => File.ReadAllText(path);

        public void Write(string path, byte[] bytes) => File.WriteAllBytes(path, bytes);

        public void WriteUtf8(string path, string text) => File.WriteAllText(path, text);

        public void Delete(string path) => File.Delete(path);

        public bool Exists(string path) => File.Exists(path);

        public string[] ListFiles(string folderPath) => new DirectoryInfo(folderPath).GetFiles().Select(f => f.Name).ToArray();

        public void MoveFile(string from, string to) => File.Move(from, to);
    }
}
