using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Imagin.Common.Serialization
{
    public static class Serializer<T>
    {
        public static string ToXml(T value, params Type[] Types)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            StringBuilder stringBuilder = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true
            };

            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, settings))
            {
                serializer.Serialize(xmlWriter, value);
            }
            return stringBuilder.ToString();
        }

        public static T FromXml(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T value;
            using (StringReader stringReader = new StringReader(xml))
            {
                object deserialized = serializer.Deserialize(stringReader);
                value = (T)deserialized;
            }

            return value;
        }
    }
}
