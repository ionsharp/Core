using Imagin.Common;
using Imagin.Common.Extensions;

namespace Imagin.Gadgets
{
    public class SearchProvider : NamedObject
    {
        string _base = string.Empty;
        public string Base
        {
            get
            {
                return _base;
            }
            set
            {
                _base = value;
                OnPropertyChanged("Base");
            }
        }

        public SearchProvider(string name, string _base) : base(name)
        {
            Base = _base;
        }

        public void Query(string Query)
        {
            if (!Base.IsNullOrEmpty())
                "{0}{1}".F(Base, Query).TryRun();
        }
    }
}
