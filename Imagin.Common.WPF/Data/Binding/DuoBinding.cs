using Imagin.Common.Converters;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    public sealed class DuoBinding : MultiBinding
    {
        public DuoBinding() : base()
        {
            Converter = new MultiConverter<object[]>(i => i.Values);
        }
        
        public DuoBinding(string path1, object source1) : this()
        {
            Bindings.Add(new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new(path1),
                Source = source1
            });
            Bindings.Add(new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new(".")
            });
        }

        public DuoBinding(string path1, object source1, string path2, object source2) : this()
        {
            Bindings.Add(new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new(path1),
                Source = source1
            });
            Bindings.Add(new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new(path2),
                Source = source2,
            });
        }
    }
}