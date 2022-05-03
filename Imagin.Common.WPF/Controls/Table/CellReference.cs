using Imagin.Common.Linq;
using System.Collections;
using System.Linq;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class CellReference : Freezable
    {
        IDictionary Cells => Source as IDictionary;

        static readonly DependencyPropertyKey CellKey = DependencyProperty.RegisterReadOnly(nameof(Cell), typeof(object), typeof(CellReference), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty CellProperty = CellKey.DependencyProperty;
        public object Cell
        {
            get => GetValue(CellProperty);
            private set => SetValue(CellKey, value);
        }

        public static readonly DependencyProperty ColumnProperty = DependencyProperty.Register(nameof(Column), typeof(string), typeof(CellReference), new FrameworkPropertyMetadata(string.Empty, OnColumnChanged));
        public string Column
        {
            get => (string)GetValue(ColumnProperty);
            set => SetValue(ColumnProperty, value);
        }
        static void OnColumnChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<CellReference>().OnColumnChanged((string)e.NewValue);

        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(nameof(Source), typeof(object), typeof(CellReference), new FrameworkPropertyMetadata(null, OnSourceChanged));
        public object Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }
        static void OnSourceChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => i.As<CellReference>().OnSourceChanged(e.NewValue);

        protected override Freezable CreateInstanceCore() => new CellReference();

        public CellReference() : base() { }

        object GetCell()
            => Cells?.Keys.Contains(Column) == true ? Cells[Column] : null;

        void SetCell(object input)
        {
            if (Cells?.Keys.Contains(Column) == true)
                Cells[Column] = input;
        }

        protected virtual void OnColumnChanged(string input)
            => Cell = GetCell();

        protected virtual void OnSourceChanged(object input)
            => Cell = GetCell();
    }
}