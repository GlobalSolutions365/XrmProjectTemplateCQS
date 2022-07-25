using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Xrm.Application.Interfaces;

namespace XmlHelper
{
    public class XmlHelper : IXmlHelper
    {
        public string Serialize<T>(T obj)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                //XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                //if (nameSpace != null)
                //{
                //    foreach (var item in nameSpace)
                //    { 
                //    ns.Add(item.Key, item.Value);
                //    }
                //}

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memStream, Encoding.UTF8);
                xmlTextWriter.Formatting = Formatting.Indented;
                serializer.Serialize(xmlTextWriter, obj);

                string xmlString = Encoding.UTF8.GetString(memStream.ToArray());
                return xmlString;
            }
        }

        public T Deserialize<T>(string xml)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                StreamWriter writer = new StreamWriter(memStream);
                writer.Write(xml);
                writer.Flush();

                memStream.Position = 0;

                T deserializedObj = (T)serializer.Deserialize(memStream);

                return deserializedObj;
            }
        }
    }
}
