using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace Imagin.Common.Controls
{
    [Serializable]
    public class Row : Base, IUnsubscribe
    {
        [field: NonSerialized]
        public event EventHandler<EventArgs> Changed;

        [field: NonSerialized]
        internal string[] columns;

        //...

        [XmlElement("Cell")]
        public List<Cell> ActualCells { get; private set; } = new List<Cell>();

        //...

        bool @checked = false;
        public bool Checked
        {
            get => @checked;
            set => this.Change(ref @checked, value);
        }

        [field: NonSerialized]
        CellDictionary cells = new();
        [XmlIgnore]
        public CellDictionary Cells
        {
            get => cells;
            set => this.Change(ref cells, value);
        }

        //...

        public Row() : base() 
        {
            Cells.Changed += OnCellsChanged;
        }

        //...

        void OnCellsChanged(object sender, EventArgs e) => OnCellsChanged();

        //...

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            OnCellsChanged();
        }

        protected virtual void OnCellsChanged() => Changed?.Invoke(this, EventArgs.Empty);

        //...

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext input)
        {
            Cells = new();
            if (columns?.Length > 0)
            {
                for (var i = 0; i < columns.Length; i++)
                    Cells.Add(columns[i], ActualCells[i]);
            }
            Cells.Changed += OnCellsChanged;
        }

        [OnSerializing]
        protected void OnSerializing(StreamingContext input)
        {
            ActualCells.Clear();
            foreach (var i in cells)
                ActualCells.Add(i.Value);
        }

        //...

        public void Unsubscribe()
        {
            if (Cells != null)
            {
                Cells.Changed -= OnCellsChanged;
                Cells.Unsubscribe();
            }
        }
    }
}