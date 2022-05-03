using Imagin.Common.Linq;
using System;
using System.Windows.Markup;

namespace Imagin.Common.Markup
{
    public sealed class AssemblyCopyright : MarkupExtension
    {
        public AssemblyCopyright() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => XAssembly.Copyright();
    }

    public sealed class AssemblyDescription : MarkupExtension
    {
        public AssemblyDescription() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => XAssembly.Description();
    }

    public sealed class AssemblyFileVersion : AssemblyVersion
    {
        public AssemblyFileVersion() : base() { }

        public AssemblyFileVersion(string assembly) : base(assembly) { }

        public override object ProvideValue(IServiceProvider serviceProvider) => XAssembly.FileVersion(Assembly);
    }

    public class AssemblyName : MarkupExtension
    {
        public string Assembly { get; set; } = null;

        public AssemblyName() : base() { }

        public AssemblyName(string assembly) : base()
        {
            Assembly = assembly;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => XAssembly.ShortName(Assembly);
    }

    public sealed class AssemblyIcon : Image
    {
        public AssemblyIcon(string relativePath) : base(relativePath) { }

        public AssemblyIcon(string assembly, string relativePath) : base(assembly, relativePath) { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Assembly == null)
                Assembly = XAssembly.ShortName();

            return base.ProvideValue(serviceProvider);
        }
    }

    public sealed class AssemblyProduct : MarkupExtension
    {
        public AssemblyProduct() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => XAssembly.Product();
    }

    public sealed class AssemblyTitle : MarkupExtension
    {
        public AssemblyTitle() : base() { }

        public override object ProvideValue(IServiceProvider serviceProvider) => XAssembly.Title();
    }

    /// <summary>
    /// This attribute is not discoverable for some reason. Use <see cref="AssemblyFileVersion"/> instead.
    /// </summary>
    public class AssemblyVersion : MarkupExtension
    {
        public string Assembly { get; set; } = null;

        public AssemblyVersion() : base() { }

        public AssemblyVersion(string assembly) : base()
        {
            Assembly = assembly;
        }

        public override object ProvideValue(IServiceProvider serviceProvider) => XAssembly.Version(Assembly);
    }

    public sealed class DefaultAssemblyName : AssemblyName
    {
        public DefaultAssemblyName() : base(InternalAssembly.Name) { }
    }
}