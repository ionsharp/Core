using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Imagin.Common.Serialization
{
    public class XmlCallbackSerializer : XmlSerializer
    {
        public XmlCallbackSerializer(Type type) : base(type) { }

        public XmlCallbackSerializer(XmlTypeMapping xmlTypeMapping) : base(xmlTypeMapping) { }

        public XmlCallbackSerializer(Type type, string defaultNamespace) : base(type, defaultNamespace) { }

        public XmlCallbackSerializer(Type type, Type[] extraTypes) : base(type, extraTypes) { }

        public XmlCallbackSerializer(Type type, XmlAttributeOverrides overrides) : base(type, overrides) { }

        public XmlCallbackSerializer(Type type, XmlRootAttribute root) : base(type, root) { }

        public XmlCallbackSerializer(Type type, XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace) : base(type, overrides, extraTypes, root, defaultNamespace) { }

        public XmlCallbackSerializer(Type type, XmlAttributeOverrides overrides, Type[] extraTypes, XmlRootAttribute root, string defaultNamespace, string location) : base(type, overrides, extraTypes, root, defaultNamespace, location) { }

        //...

        public new object Deserialize(Stream stream)
        {
            var result = base.Deserialize(stream);
            EachWith<OnDeserializedAttribute>(result);
            return result;
        }

        public new object Deserialize(TextReader textReader)
        {
            var result = base.Deserialize(textReader);
            EachWith<OnDeserializedAttribute>(result);
            return result;
        }

        public new object Deserialize(XmlReader xmlReader)
        {
            var result = base.Deserialize(xmlReader);
            EachWith<OnDeserializedAttribute>(result);
            return result;
        }

        public new object Deserialize(XmlSerializationReader reader)
        {
            var result = base.Deserialize(reader);
            EachWith<OnDeserializedAttribute>(result);
            return result;
        }

        public new object Deserialize(XmlReader xmlReader, string encodingStyle)
        {
            var result = base.Deserialize(xmlReader, encodingStyle);
            EachWith<OnDeserializedAttribute>(result);
            return result;
        }

        public new object Deserialize(XmlReader xmlReader, XmlDeserializationEvents events)
        {
            var result = base.Deserialize(xmlReader, events);
            EachWith<OnDeserializedAttribute>(result);
            return result;
        }

        public new object Deserialize(XmlReader xmlReader, string encodingStyle, XmlDeserializationEvents events)
        {
            var result = base.Deserialize(xmlReader, encodingStyle, events);
            EachWith<OnDeserializedAttribute>(result);
            return result;
        }

        //...

        public new void Serialize(object o, XmlSerializationWriter xmlSerializationWriter)
        {
            EachWith<OnSerializingAttribute>(o);
            base.Serialize(o, xmlSerializationWriter);
            EachWith<OnSerializedAttribute>(o);
        }

        public new void Serialize(Stream stream, object o)
        {
            EachWith<OnSerializingAttribute>(o);
            base.Serialize(stream, o);
            EachWith<OnSerializedAttribute>(o);
        }

        public new void Serialize(TextWriter textWriter, object o)
        {
            EachWith<OnSerializingAttribute>(o);
            base.Serialize(textWriter, o);
            EachWith<OnSerializedAttribute>(o);
        }

        public new void Serialize(XmlWriter xmlWriter, object o)
        {
            EachWith<OnSerializingAttribute>(o);
            base.Serialize(xmlWriter, o);
            EachWith<OnSerializedAttribute>(o);
        }

        public new void Serialize(Stream stream, object o, XmlSerializerNamespaces namespaces)
        {
            EachWith<OnSerializingAttribute>(o);
            base.Serialize(stream, o, namespaces);
            EachWith<OnSerializedAttribute>(o);
        }

        public new void Serialize(TextWriter textWriter, object o, XmlSerializerNamespaces namespaces)
        {
            EachWith<OnSerializingAttribute>(o);
            base.Serialize(textWriter, o, namespaces);
            EachWith<OnSerializedAttribute>(o);
        }

        public new void Serialize(XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces)
        {
            EachWith<OnSerializingAttribute>(o);
            base.Serialize(xmlWriter, o, namespaces);
            EachWith<OnSerializedAttribute>(o);
        }

        public new void Serialize(XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces, string encodingStyle)
        {
            EachWith<OnSerializingAttribute>(o);
            base.Serialize(xmlWriter, o, namespaces, encodingStyle);
            EachWith<OnSerializedAttribute>(o);
        }

        public new void Serialize(XmlWriter xmlWriter, object o, XmlSerializerNamespaces namespaces, string encodingStyle, string id)
        {
            EachWith<OnSerializingAttribute>(o);
            base.Serialize(xmlWriter, o, namespaces, encodingStyle, id);
            EachWith<OnSerializedAttribute>(o);
        }

        //...

        public static bool HasAttribute<T>(MemberInfo input)
        {
            foreach (var i in input.GetCustomAttributes(true))
            {
                if (i is T)
                    return true;
            }
            return false;
        }

        //...

        bool IsValidType(Type i)
            => i is not null 
            && !i.Equals(typeof(string)) 
            && !i.IsArray 
            && i.IsClass 
            && !i.IsEnum 
            && !i.IsImport 
            && !i.IsInterface 
            && !i.IsPrimitive 
            && i.IsPublic 
            && i.IsSerializable 
            && !i.IsValueType 
            && i.IsVisible;

        //...

        void Each(object input, Action<object> action) 
        {
            var type = input?.GetType();
            if (IsValidType(type))
            {
                action(input);
                if (input is IEnumerable j)
                {
                    foreach (var i in j)
                        Each(i, action);
                }

                foreach (var i in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
                {
                    if (IsValidType(i.PropertyType))
                    {
                        object temp = default;
                        Try.Invoke(() => temp = i.GetValue(input, null));
                        if (temp != null)
                            Each(temp, action);
                    }
                }
            }
        }

        void EachWith<T>(object input) where T : Attribute
        {
            Each(input, i =>
            {
                var methods = i.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                if (methods?.Length > 0)
                {
                    foreach (var j in methods)
                    {
                        if (HasAttribute<T>(j))
                            j.Invoke(i, new object[] { new StreamingContext(StreamingContextStates.Other) });
                    }
                }
            });
        }
    }
}