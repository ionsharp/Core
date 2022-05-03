using Imagin.Common.Data;
using Imagin.Common.Linq;
using System;

namespace Imagin.Common.Storage
{
    [DisplayName("Extensions")]
    [Serializable]
    public class FilterExtensions : Base
    {
        IncludeExclude filter = IncludeExclude.Include;
        public IncludeExclude Filter
        {
            get => filter;
            set => this.Change(ref filter, value);
        }

        string value = string.Empty;
        [DisplayName("Extensions")]
        [Style(StringStyle.Tokens)]
        public string Value
        {
            get => value;
            set
            {
                value = value.Replace(".", string.Empty);
                this.Change(ref this.value, value);
            }
        }

        public FilterExtensions() : base() { }

        public override string ToString() => value.NullOrEmpty() ? "All" : $"{filter} .{value.Replace(";", ", .").TrimEnd('.').TrimEnd(' ').TrimEnd(',')}";
    }
}