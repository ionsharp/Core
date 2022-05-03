using Imagin.Common;
using System;

namespace Imagin.Apps.Desktop
{
    [Serializable]
    public class SearchEngine : BaseNamable
    {
        string url = null;
        [DisplayName("Url")]
        public string Url
        {
            get => url;
            set => this.Change(ref url, value);
        }

        public SearchEngine() : this("Untitled", "") { }

        public SearchEngine(string name, string url) : base(name) => Url = url;
    }
}