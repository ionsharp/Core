using System;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Attributes
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DynamicPropertyAttribute : Attribute
    {
        public BindingMode BindingMode
        {
            get; private set;
        }

        public Type ConverterType
        {
            get; private set;
        }
        
        public string Name
        {
            get; private set;
        }

        public string PropertyPath
        {
            get; private set;
        }

        public Type Type
        {
            get; private set;
        }

        public DynamicPropertyAttribute(string Name, Type Type, string PropertyPath, Type ConverterType, BindingMode BindingMode)
        {
            this.Name = Name;
            this.Type = Type;
            this.PropertyPath = PropertyPath;
            this.ConverterType = ConverterType;
            this.BindingMode = BindingMode;
        }
    }
}
