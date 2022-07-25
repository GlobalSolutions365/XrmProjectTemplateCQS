namespace Xrm.Application.Interfaces
{
    public interface IJsonHelper
    {
        T Deserialize<T>(string json);
        string Serialize<T>(T obj);
    }
}
