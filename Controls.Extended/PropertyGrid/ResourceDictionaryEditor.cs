using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imagin.Common.Extensions;
using Imagin.Controls.Extended;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
            var ResourceDictionary = Value as ResourceDictionary;

            if (Value is string && Value.ToString().FileExists())
            {
                using (var FileStream = File.OpenRead(Value.ToString()))
                {
                    FileStream.Seek(0, SeekOrigin.Begin);
                    ResourceDictionary = (ResourceDictionary)System.Windows.Markup.XamlReader.Load(FileStream);
                }
            }

            if (ResourceDictionary != null)
                await BeginFromResourceDictionary(ResourceDictionary);
        }

        /// <summary>
        /// Set properties by enumerating a resource dictionary.
        /// </summary>
        /// <param name="Dictionary">The dictionary to enumerate.</param>
        /// <param name="Callback">What to do afterwards.</param>
        public async Task BeginFromResourceDictionary(ResourceDictionary Dictionary, Action Callback = null)
        {
            if (Dictionary == null) return;
            await Task.Run(() =>
            {
                foreach (DictionaryEntry Entry in Dictionary)
                {
                    var Value = Entry.Value;
                    if (Value == null) continue;

                    var Type = Value.GetType();
                    if (Type.EqualsAny(typeof(LinearGradientBrush), typeof(SolidColorBrush)))
                    {
                        var t = PropertyModel.GetType(Type);
                        if (t == null) continue;

                        var Result = PropertyModel.New(t, Entry.Key.ToString(), Value, Type.Name.SplitCamelCase(), string.Empty, false, false);
                        Result.Object = Properties.Object;
                        Properties.Add(Result);
                    }
                }
            });
            if (Callback != null)
                Callback.Invoke();
        }

        #endregion
    }
}
