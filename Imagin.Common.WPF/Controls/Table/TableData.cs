using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class TableData : NotifableCollection<Row>, IUnsubscribe
    {
        enum Category
        {
            Columns
        }

        string columns = "A;B";
        [XmlAttribute]
        public string Columns
        {
            get => columns;
            set => this.Change(ref columns, value);
        }

        public TableData() : base() { }

        //...

        void OnRowChanged(object sender, EventArgs e) => OnChanged();

        //...

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext input)
        {
            var j = Table.ParseColumns(columns);
            this.ForEach(i => i.columns = j);
        }

        //...

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems[0].As<Row>().Changed += OnRowChanged;
                    break;

                case NotifyCollectionChangedAction.Remove:
                    Unsubscribe(e.OldItems[0] as Row);
                    break;
            }
        }

        //...

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Columns):

                    var columns = Table.ParseColumns(Columns);
                    foreach (var i in this)
                    {
                        Dictionary<string, Cell> oldValues = new();
                        foreach (var j in i.Cells)
                            oldValues.Add(j.Key, j.Value);

                        i.Cells.Clear();
                        foreach (var j in columns)
                            i.Cells.Add(j, !oldValues.ContainsKey(j) ? new() : oldValues[j]);
                    }
                    OnChanged();
                    break;
            }
        }

        //...

        void Unsubscribe(Row i)
        {
            i.Changed -= OnRowChanged;
            i.Unsubscribe();
        }

        public void Unsubscribe() => this.ForEach(i => Unsubscribe(i));
    }
}