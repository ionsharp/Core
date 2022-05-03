using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Xml.Serialization;

namespace Imagin.Apps.Desktop
{
    [DisplayName("Search")]
    [Serializable]
    public class SearchTile : Tile
    {
        [XmlIgnore]
        IList<SearchEngine> SearchEngines => Get.Current<Options>().SearchEngines;

        [XmlIgnore]
        string Url => $"{SearchEngines[searchEngine].Url}{text}";

        //...

        int searchEngine = 0;
        [Hidden]
        public int SearchEngine
        {
            get => searchEngine;
            set => this.Change(ref searchEngine, value);
        }

        string text;
        [Hidden]
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }

        //...

        public SearchTile() : base() { }

        //...

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(SearchEngine):
                case nameof(Text):
                    OnChanged();
                    break;
            }
        }

        [field: NonSerialized]
        ICommand searchCommand;
        [Hidden, XmlIgnore]
        public ICommand SearchCommand => searchCommand ??= new RelayCommand(() => Process.Start(new ProcessStartInfo(Url)), () => searchEngine >= 0 && searchEngine < SearchEngines.Count && !text.NullOrEmpty());
    }
}