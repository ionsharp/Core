using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public class CountValue : Base, IPoint2D, ISize, ISelect
    {
        [field: NonSerialized]
        public event SelectedEventHandler Selected;

        [field: NonSerialized]
        bool isSelected = false;
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                this.Change(ref isSelected, value);
                if (value)
                {
                    OnSelected();
                }
            }
        }

        Point2D position = new(0, 0);
        public Point2D Position
        {
            get => position;
            set => this.Change(ref position, value);
        }

        DoubleSize size = new(250, 250);
        public DoubleSize Size
        {
            get => size;
            set => this.Change(ref size, value);
        }

        int value = 0;
        public int Value
        {
            get => value;
            set => this.Change(ref this.value, value);
        }

        public CountValue(int value, Point point)
        {
            Value = value;
            Position = point;
        }

        protected virtual void OnSelected()
        {
            Selected?.Invoke(this, new SelectedEventArgs(this));
        }
    }

    [Serializable]
    public class Count : BaseNamable
    {
        string fontColor = $"255,0,0,0";
        public SolidColorBrush FontColor
        {
            get
            {
                var result = fontColor.Split(',');
                return new SolidColorBrush(Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte()));
            }
            set => this.Change(ref fontColor, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

        double fontSize = 16;
        public double FontSize
        {
            get => fontSize;
            set => this.Change(ref fontSize, value);
        }

        string markerColor = $"255,0,0,0";
        public SolidColorBrush MarkerColor
        {
            get
            {
                var result = markerColor.Split(',');
                return new SolidColorBrush(Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte()));
            }
            set => this.Change(ref markerColor, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }
        
        double markerSize = 16;
        public double MarkerSize
        {
            get => markerSize;
            set
            {
                this.Change(ref markerSize, value);
                this.Changed(() => MarkerSizeHalf);
            }
        }

        [Hidden]
        public double MarkerSizeHalf => MarkerSize / 2.0;

        System.Collections.ObjectModel.ObservableCollection<CountValue> values = new();
        public System.Collections.ObjectModel.ObservableCollection<CountValue> Values
        {
            get => values;
            set => this.Change(ref values, value);
        }

        public Count(string name) : base(name) { }
    }

    [DisplayName("Count")]
    [Icon(App.ImagePath + "Count.png")]
    [Serializable]
    public class CountTool : Tool
    {
        Count count = null;
        public Count Count
        {
            get => count;
            set => this.Change(ref count, value);
        }

        [Hidden]
        public Common.Collections.Generic.ObservableCollection<Count> Counts => (Document as ImageDocument)?.Counts;

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Count.png");

        public override bool OnMouseDown(Point point)
        {
            if (Count != null)
                Count.Values.Add(new CountValue(Count.Values.Count, new Point(point.X - (Count.MarkerSize / 2.0), point.Y - (Count.MarkerSize / 2.0))));

            return base.OnMouseDown(point);
        }

        [field: NonSerialized]
        ICommand addCommand;
        public ICommand AddCommand => addCommand ??= new RelayCommand(() => Counts.Add(new Count("Untitled")), () => Counts != null);

        [field: NonSerialized]
        ICommand clearCommand;
        public ICommand ClearCommand => clearCommand ??= new RelayCommand(() => Count.Values.Clear(), () => Count != null);
    }
}