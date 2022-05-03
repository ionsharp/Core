using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public abstract class MemberAttributeBinding : MultiBinding
    {
        protected string GetValue(object input)
        {
            if (input != null)
            {
                var result = $"{input}";
                if (!result.Empty())
                    return result;
            }
            return null;
        }
    }

    //...

    public class MemberDescriptionBinding : MemberAttributeBinding
    {
        public MemberDescriptionBinding() : base()
        {
            Converter = new MultiConverter<string>(i =>
            {
                string result = null;
                if (i.Values?.Length >= 1)
                {
                    if (i.Values[0] is MemberModel member)
                    {
                        //(1) Description of member value
                        result ??= GetValue(DescriptionConverter.Default.Convert(member.Value));
                        //(2) Description of member definition
                        result ??= GetValue(member.Description);
                        //(3) Description of actual type
                        result ??= GetValue(DescriptionConverter.Default.Convert(member.ActualType));
                        //(4) Description of member type
                        result ??= GetValue(DescriptionConverter.Default.Convert(member.Type));

                        if (member.Localize)
                            return result?.Translate();
                    }
                }
                return result;
            });
            Bindings.Add(new Binding() { Path = new(".") });
            Bindings.Add(new Binding() { Path = new(nameof(MemberModel.Value)) });
            Bindings.Add(new Binding() { Path = new(nameof(MemberModel.ActualType)) });
            Bindings.Add(new Binding() { Path = new(nameof(MemberModel.Type)) });
            Bindings.Add(new Data.Options(nameof(MainViewOptions.Language)));
        }
    }

    public class MemberNameBinding : MemberAttributeBinding
    {
        public MemberNameBinding() : base()
        {
            Converter = new MultiConverter<string>(i =>
            {
                string result = null;
                if (i.Values?.Length >= 1)
                {
                    if (i.Values[0] is MemberModel model)
                    {
                        //(1) Display name of the object
                        result ??= model.DisplayName;
                        //(2) To do: Display name of any base types?

                        if (model.Localize)
                            return result?.Translate();
                    }
                }
                return result;
            });
            Bindings.Add(new Binding() { Path = new(".") });
            Bindings.Add(new Data.Options(nameof(MainViewOptions.Language)));
        }

    }

    //...

    public class ObjectDescriptionBinding : MemberAttributeBinding
    {
        public ObjectDescriptionBinding() : this(".") { }

        public ObjectDescriptionBinding(string path) : base()
        {
            Converter = new MultiConverter<string>(i =>
            {
                string result = null;
                if (i.Values?.Length >= 1)
                {
                    if (i.Values[0] is object source)
                    {
                        //(1) Description of the object
                        result ??= GetValue(DescriptionConverter.Default.Convert(source));
                        //(2) To do: Description of any base types

                        if (source.GetType().GetAttribute<LocalizeAttribute>()?.Localize == true)
                            return result?.Translate();
                    }
                }
                return result;
            });
            Bindings.Add(new Binding() { Path = new(path) });
            Bindings.Add(new Data.Options(nameof(MainViewOptions.Language)));
        }
    }

    public class ObjectNameBinding : MemberAttributeBinding
    {
        public ObjectNameBinding() : this(".") { }

        public ObjectNameBinding(string path) : base()
        {
            Converter = new MultiConverter<string>(i =>
            {
                string result = null;
                if (i.Values?.Length >= 1)
                {
                    if (i.Values[0] is object source)
                    {
                        //(1) Display name of the object
                        result ??= GetValue(DisplayNameConverter.Default.Convert(source));
                        //(2) To do: Display name of any base types?

                        if (source.GetType().GetAttribute<LocalizeAttribute>()?.Localize == true)
                            return result?.Translate();
                    }
                }
                return result;
            });
            Bindings.Add(new Binding() { Path = new(path) });
            Bindings.Add(new Data.Options(nameof(MainViewOptions.Language)));
        }
    }
}