using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Markup
{
    public class Read : Uri
    {
        public Read() : base() { }

        public Read(string relativePath) : base(relativePath) { }

        public Read(string assembly, string relativePath) : base(assembly, relativePath) { }

        public override object ProvideValue(IServiceProvider serviceProvider) => Resources.Read(Assembly, RelativePath);
    }

    public class ReadInternal : Read
    {
        public override string Assembly => InternalAssembly.Name;

        public ReadInternal(string relativePath) : base(relativePath) { }
    }

    public class ReadProject : Read
    {
        public override string Assembly => XAssembly.ShortName();

        public ReadProject(string relativePath) : base(relativePath) { }
    }
}