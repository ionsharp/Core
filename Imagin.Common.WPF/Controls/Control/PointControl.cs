using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class PointControl : Control
    {
        #region (class) PointModel

        public class PointModel : Base
        {
            public readonly PointControl Source;

            double x = 0;
            public double X
            {
                get => x;
                set => this.Change(ref x, value);
            }

            double y = 0;
            public double Y
            {
                get => y;
                set => this.Change(ref y, value);
            }

            public PointModel(PointControl source, double x, double y) : base()
            {
                Source = source;
                X = x; Y = y;
            }

            public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                base.OnPropertyChanged(propertyName);
                Source.UpdateSource();
            }

            public override string ToString() => $"({x}, {y})";
        }

        #endregion

        #region Properties

        internal readonly Handle Handle = false;

        private System.Windows.Shapes.Ellipse target;

        private PointModel targetPoint;

        public static readonly DependencyProperty PointsProperty = DependencyProperty.Register(nameof(Points), typeof(PointCollection), typeof(PointControl), new FrameworkPropertyMetadata(null, OnPointsChanged));
        public PointCollection Points
        {
            get => (PointCollection)GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }
        static void OnPointsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<PointControl>().OnPointsChanged(e);

        static readonly DependencyPropertyKey MovablePointsKey = DependencyProperty.RegisterReadOnly(nameof(MovablePoints), typeof(ObservableCollection<PointModel>), typeof(PointControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty MovablePointsProperty = MovablePointsKey.DependencyProperty;
        public ObservableCollection<PointModel> MovablePoints
        {
            get => (ObservableCollection<PointModel>)GetValue(MovablePointsProperty);
            private set => SetValue(MovablePointsKey, value);
        }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(PointControl), new FrameworkPropertyMetadata(1d));
        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        #endregion

        #region PointControl

        public PointControl() : base() 
        {
            MovablePoints = new();
        }

        #endregion

        #region Methods

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.OriginalSource is System.Windows.Shapes.Ellipse ellipse)
            {
                target 
                    = ellipse;
                targetPoint 
                    = ellipse.DataContext as PointModel;

                target.CaptureMouse();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (targetPoint != null)
            {
                var point = e.GetPosition(this);
                targetPoint.X = (point.X / Zoom).Coerce(1); targetPoint.Y = (point.Y / Zoom).Coerce(1);
            }
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            target?.ReleaseMouseCapture();
            target = null; targetPoint = null;
        }

        protected virtual void OnPointsChanged(Value<PointCollection> input)
        {
            Handle.SafeInvoke(() =>
            {
                MovablePoints.Clear();
                input.New?.ForEach(i => MovablePoints.Add(new(this, i.X, i.Y)));
            });
        }

        internal void UpdateSource() 
            => Handle.Invoke(() => SetCurrentValue(PointsProperty, new PointCollection(MovablePoints.Select(i => new Point(i.X, i.Y)))));

        #endregion

        #region Commands

        ICommand insertAfterCommand;
        public ICommand InsertAfterCommand => insertAfterCommand ??= new RelayCommand<PointModel>(i =>
        {
            var result = new PointModel(this, 0, 0);

            var index = MovablePoints.IndexOf(i) + 1;
            if (index >= MovablePoints.Count)
                MovablePoints.Add(result);

            else MovablePoints.Insert(index, result);
            UpdateSource();
        }, i => i != null);

        ICommand insertBeforeCommand;
        public ICommand InsertBeforeCommand => insertBeforeCommand ??= new RelayCommand<PointModel>(i =>
        {
            MovablePoints.Insert(MovablePoints.IndexOf(i), new(this, 0, 0));
            UpdateSource();

        }, i => i != null);

        ICommand removeCommand;
        public ICommand RemoveCommand => removeCommand ??= new RelayCommand<PointModel>(i =>
        {
            MovablePoints.Remove(i);
            UpdateSource();
        }, i => i != null);

        #endregion
    }
}