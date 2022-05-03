using Imagin.Common.Linq;
using System;
using System.Collections.Generic;

namespace Imagin.Common.Controls
{
    public class CellDictionary : Dictionary<string, Cell>, IUnsubscribe
    {
        public event EventHandler<EventArgs> Changed;

        new public void Add(string a, Cell b)
        {
            base.Add(a, b);
            b.PropertyChanged += OnCellChanged;
        }

        void OnCellChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e) => OnCellChanged();

        protected virtual void OnCellChanged() => Changed?.Invoke(this, EventArgs.Empty);

        public void Unsubscribe() => this.ForEach(i => i.Value.PropertyChanged -= OnCellChanged);
    }
}