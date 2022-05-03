using Imagin.Common.Analytics;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Imagin.Common.Controls
{
    public class MatrixControl : Control
    {
        #region (class) ValueModel

        public class ValueModel : Base
        {
            public readonly MatrixControl Control;

            double value = 0;
            public double Value
            {
                get => value;
                set => this.Change(ref this.value, value);
            }

            public ValueModel(MatrixControl control, double value) : base()
            {
                Control = control; this.value = value;
            }

            public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                base.OnPropertyChanged(propertyName);
                Log.Write<MatrixControl>($"ValueModel.OnPropertyChanged: ({value})");
                Control.UpdateSource();
            }
        }

        #endregion

        #region Properties

        internal readonly Handle Handle = false;

        static readonly DependencyPropertyKey ColumnsKey = DependencyProperty.RegisterReadOnly(nameof(Columns), typeof(int), typeof(MatrixControl), new FrameworkPropertyMetadata(1));
        public static readonly DependencyProperty ColumnsProperty = ColumnsKey.DependencyProperty;
        public int Columns
        {
            get => (int)GetValue(ColumnsProperty);
            private set => SetValue(ColumnsKey, value);
        }

        static readonly DependencyPropertyKey EditableMatrixKey = DependencyProperty.RegisterReadOnly(nameof(EditableMatrix), typeof(ObservableCollection<ValueModel>), typeof(MatrixControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty EditableMatrixProperty = EditableMatrixKey.DependencyProperty;
        public ObservableCollection<ValueModel> EditableMatrix
        {
            get => (ObservableCollection<ValueModel>)GetValue(EditableMatrixProperty);
            private set => SetValue(EditableMatrixKey, value);
        }

        public static readonly DependencyProperty IsEditableProperty = DependencyProperty.Register(nameof(IsEditable), typeof(bool), typeof(MatrixControl), new FrameworkPropertyMetadata(true));
        public bool IsEditable
        {
            get => (bool)GetValue(IsEditableProperty);
            set => SetValue(IsEditableProperty, value);
        }

        public static readonly DependencyProperty LabelVisibilityProperty = DependencyProperty.Register(nameof(LabelVisibility), typeof(Visibility), typeof(MatrixControl), new FrameworkPropertyMetadata(Visibility.Visible));
        public Visibility LabelVisibility
        {
            get => (Visibility)GetValue(LabelVisibilityProperty);
            set => SetValue(LabelVisibilityProperty, value);
        }
        
        public static readonly DependencyProperty MatrixProperty = DependencyProperty.Register(nameof(Matrix), typeof(DoubleMatrix), typeof(MatrixControl), new FrameworkPropertyMetadata(null, OnMatrixChanged));
        public DoubleMatrix Matrix
        {
            get => (DoubleMatrix)GetValue(MatrixProperty);
            set => SetValue(MatrixProperty, value);
        }
        static void OnMatrixChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => sender.As<MatrixControl>().OnMatrixChanged(e);

        static readonly DependencyPropertyKey RowsKey = DependencyProperty.RegisterReadOnly(nameof(Rows), typeof(int), typeof(MatrixControl), new FrameworkPropertyMetadata(1));
        public static readonly DependencyProperty RowsProperty = RowsKey.DependencyProperty;
        public int Rows
        {
            get => (int)GetValue(RowsProperty);
            private set => SetValue(RowsKey, value);
        }

        public static readonly DependencyProperty WeightBrushProperty = DependencyProperty.Register(nameof(WeightBrush), typeof(System.Windows.Media.Brush), typeof(MatrixControl), new FrameworkPropertyMetadata(Brushes.Black));
        public System.Windows.Media.Brush WeightBrush
        {
            get => (System.Windows.Media.Brush)GetValue(WeightBrushProperty);
            set => SetValue(WeightBrushProperty, value);
        }

        public static readonly DependencyProperty WeightVisibilityProperty = DependencyProperty.Register(nameof(WeightVisibility), typeof(Visibility), typeof(MatrixControl), new FrameworkPropertyMetadata(Visibility.Visible));
        public Visibility WeightVisibility
        {
            get => (Visibility)GetValue(WeightVisibilityProperty);
            set => SetValue(WeightVisibilityProperty, value);
        }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(MatrixControl), new FrameworkPropertyMetadata(1d));
        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        #endregion

        #region MatrixControl

        public MatrixControl() : base()
        {
            EditableMatrix 
                = new();
        }

        #endregion

        #region Methods

        internal void UpdateSource() => Handle.Invoke(() =>
        {
            var result = new DoubleMatrix((uint)Rows, (uint)Columns);

            uint x = 0;
            uint y = 0;
            EditableMatrix.ForEach(i =>
            {
                result[y, x] = i.Value;
                if (x == Columns - 1)
                {
                    x = 0;
                    y++;
                }
                else x++;
            });

            SetCurrentValue(MatrixProperty, result);
        });

        protected virtual void OnMatrixChanged(Value<DoubleMatrix> input)
        {
            Handle.SafeInvoke(() =>
            {
                Columns
                    = (int)input.New.Columns;
                Rows
                    = (int)input.New.Rows;

                EditableMatrix.Clear();
                input.New.Each(i =>
                {
                    EditableMatrix.Add(new(this, i));
                    return i;
                });
            });
        }

        #endregion
    }
}