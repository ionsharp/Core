using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System;
using System.Windows;
using System.Windows.Media;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public class Note : Base, IPoint2D, ISize, ISelect
    {
        [field: NonSerialized]
        public event SelectedEventHandler Selected;

        string color = $"255,0,0,0";
        public virtual SolidColorBrush Color
        {
            get
            {
                var result = color.Split(',');
                var actualResult = default(SolidColorBrush);
                Dispatch.Invoke(() => actualResult = new SolidColorBrush(System.Windows.Media.Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte())));
                return actualResult;
            }
            set => this.Change(ref color, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

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

        string text = string.Empty;
        public string Text
        {
            get => text;
            set => this.Change(ref text, value);
        }

        public Note(SolidColorBrush color, Point point)
        {
            Color = color;
            Position = point;
        }

        protected virtual void OnSelected()
        {
            Selected?.Invoke(this, new SelectedEventArgs(this));
        }
    }

    [DisplayName("Note")]
    [Icon(App.ImagePath + "Note.png")]
    [Serializable]
    public class NoteTool : Tool
    {
        public ObservableCollection<Note> Notes => Document.As<ImageDocument>()?.Notes;

        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Note.png");

        string color = $"255,0,0,0";
        public virtual SolidColorBrush Color
        {
            get
            {
                var result = color.Split(',');
                var actualResult = default(SolidColorBrush);
                Dispatch.Invoke(() => actualResult = new SolidColorBrush(System.Windows.Media.Color.FromArgb(result[0].Byte(), result[1].Byte(), result[2].Byte(), result[3].Byte())));
                return actualResult;
            }
            set => this.Change(ref color, $"{value.Color.A},{value.Color.R},{value.Color.G},{value.Color.B}");
        }

        public override bool OnMouseDown(Point point)
        {
            var note = new Note(Color, point)
            {
                IsSelected = true
            };
            Notes.Add(note);

            return base.OnMouseDown(point);
        }
    }
}