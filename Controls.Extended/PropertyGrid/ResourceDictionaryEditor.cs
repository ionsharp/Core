using Imagin.Common.Extensions;
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
            var Result = Value as ResourceDictionary;

            if (Value is string && Value.ToString().FileExists())
            {
                using (var FileStream = File.OpenRead(Value.ToString()))
                {
                    FileStream.Seek(0, SeekOrigin.Begin);
                    Result = (ResourceDictionary)System.Windows.Markup.XamlReader.Load(FileStream);
                }
            }

            if (Result != null)
                await Properties.BeginFromResourceDictionary(Result);
        }

        #endregion
    }
}
