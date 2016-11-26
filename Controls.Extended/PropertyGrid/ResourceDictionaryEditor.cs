using Imagin.Common.Extensions;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;

namespace Imagin.Controls.Extended
{
    public class ResourceDictionaryEditor : PropertyGrid
    {
        #region ResourceDictionaryEditor

        public ResourceDictionaryEditor() : base()
        {
        }

        #endregion

        #region Methods

        protected override async Task SetObject(object Value)
        {
            if (Value != null)
            {
                var ResourceDictionary = Value as ResourceDictionary;
                if (Value is string && Value.ToString().FileExists())
                {
                    using (FileStream FileStream = File.OpenRead(Value.ToString()))
                    {
                        FileStream.Seek(0, SeekOrigin.Begin);
                        ResourceDictionary = (ResourceDictionary)System.Windows.Markup.XamlReader.Load(FileStream);
                    }
                }

                if (ResourceDictionary != null)
                    await this.Properties.BeginFromResourceDictionary(ResourceDictionary);
            }
        }

        #endregion
    }
}
