using System.Collections.Generic;

namespace Xrm.Application.Interfaces
{
    public interface IXmlHelper
    {
        T Deserialize<T>(string xml);
        string Serialize<T>(T obj);
    }
}
