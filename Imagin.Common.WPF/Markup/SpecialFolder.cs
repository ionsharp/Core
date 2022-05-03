using Imagin.Common.Linq;
using System;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public class SpecialFolder : MarkupExtension
    {
        public Environment.SpecialFolder Folder
        {
            get; set;
        }

        public SpecialFolder() : base() { }

        public SpecialFolder(Environment.SpecialFolder folder) : this()
        {
            Folder = folder;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => Folder.Path();
    }
}