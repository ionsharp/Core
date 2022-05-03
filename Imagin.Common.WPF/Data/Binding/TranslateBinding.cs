using Imagin.Common.Converters;
using Imagin.Common.Local.Engine;
using Imagin.Common.Local.Extensions;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    public class TranslateBinding : MultiBinding, IRemoteBinding
    {
        protected readonly Binding KeyBinding = null;

        public string Format { get; set; } = null;

        public bool Lower { get; set; }

        public string Prefix { get; set; }

        public RelativeSourceMode Relative { set => KeyBinding.RelativeSource = new(value); }

        public Type RelativeType { set => KeyBinding.RelativeSource = new(RelativeSourceMode.FindAncestor) { AncestorType = value }; }
        
        public RemoteBindingSource RemoteSource
        {
            set => KeyBinding.Source = this.GetSource(value);
        }

        public string Suffix { get; set; }

        public bool Upper { get; set; }

        public TranslateBinding() : this(".") { }

        public TranslateBinding(string path) : base()
        {
            Converter = new MultiConverter<string>(data =>
            {
                if (data.Values?.Length > 0)
                {
                    try
                    {
                        var culture = LocalizeDictionary.Instance.SpecificCulture;
                        var key = data.Values[0]?.ToString();

                        var value = LocExtension.GetLocalizedValue(data.TargetType, key, culture, null);
                        if (value == null)
                        {
                            var missingKeyEventResult = LocalizeDictionary.Instance.OnNewMissingKeyEvent(this, key);
                            if
                            (
                                LocalizeDictionary.Instance.OutputMissingKeys
                                &&
                                !string.IsNullOrEmpty(key)
                                && (data.TargetType == typeof(string)
                                || data.TargetType == typeof(object))
                            )
                            {
                                return missingKeyEventResult.MissingKeyResult != null
                                ? missingKeyEventResult.MissingKeyResult
                                : LocalizeDictionary.MissingKeyFormat.F(Prefix, Format?.F(key) ?? key, Suffix);
                            }
                            return null;
                        }
                        var result = Format?.F(value) ?? $"{value}";
                        result = $"{Prefix}{result}{Suffix}";
                        result = Lower ? result.ToLower() : Upper ? result.ToUpper() : result;
                        return result;
                    }
                    catch { }
                }
                return null;
            });

            KeyBinding = new Binding(path);
            Bindings.Add(KeyBinding);
            Bindings.Add(new Options() { Path = new PropertyPath("Language") });
        }

        public TranslateBinding(IValueConverter converter) : this(".", converter) { }

        public TranslateBinding(string path, IValueConverter converter) : this(path)
        {
            KeyBinding.Converter = converter;
        }
    }
}