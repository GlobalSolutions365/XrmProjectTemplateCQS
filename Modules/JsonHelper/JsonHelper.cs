using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Xrm.Application.Interfaces;

namespace JsonHelper
{
    public class JsonHelper : IJsonHelper
    {
        public string Serialize<T>(T obj)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T), null, int.MaxValue, true, null, false);
                serializer.WriteObject(memStream, obj);

                string jsonString = Encoding.UTF8.GetString(memStream.ToArray());

                return jsonString;
            }
        }

        public T Deserialize<T>(string json)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));

                StreamWriter writer = new StreamWriter(memStream);
                writer.Write(json);
                writer.Flush();

                memStream.Position = 0;

                T deserializedObj = (T)serializer.ReadObject(memStream);

                return deserializedObj;
            }
        }
    }
}
