using Imagin.Common.Linq;
using Imagin.Common.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class GradientControl : Control
    {
        #region Properties

        readonly System.Random random = new();

        public static readonly DependencyProperty GradientProperty = DependencyProperty.Register(nameof(Gradient), typeof(GradientStepCollection), typeof(GradientControl), new FrameworkPropertyMetadata(null, OnGradientChanged, new CoerceValueCallback(OnGradientCoerced)));
        public GradientStepCollection Gradient
        {
            get => (GradientStepCollection)GetValue(GradientProperty);
            set => SetValue(GradientProperty, value);
        }
        static void OnGradientChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((GradientControl)i).OnGradientChanged(new Value<GradientStepCollection>(e));
        static object OnGradientCoerced(DependencyObject i, object value) => ((GradientControl)i).OnGradientCoerced(value as GradientStepCollection);

        public static readonly DependencyProperty PreviewBorderStyleProperty = DependencyProperty.Register(nameof(PreviewBorderStyle), typeof(Style), typeof(GradientControl), new FrameworkPropertyMetadata(null));
        public Style PreviewBorderStyle
        {
            get => (Style)GetValue(PreviewBorderStyleProperty);
            set => SetValue(PreviewBorderStyleProperty, value);
        }

        public static readonly DependencyProperty SelectedStepProperty = DependencyProperty.Register(nameof(SelectedStep), typeof(GradientStep), typeof(GradientControl), new FrameworkPropertyMetadata(null));
        public GradientStep SelectedStep
        {
            get => (GradientStep)GetValue(SelectedStepProperty);
            set => SetValue(SelectedStepProperty, value);
        }
        
        public static readonly DependencyProperty SelectedStepIndexProperty = DependencyProperty.Register(nameof(SelectedStepIndex), typeof(int), typeof(GradientControl), new FrameworkPropertyMetadata(-1, new PropertyChangedCallback(OnSelectedStepIndexChanged)));
        public int SelectedStepIndex
        {
            get => (int)GetValue(SelectedStepIndexProperty);
            set => SetValue(SelectedStepIndexProperty, value);
        }
        static void OnSelectedStepIndexChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((GradientControl)i).OnSelectedStepIndexChanged(new Value<int>(e));

        public static readonly DependencyProperty StepsProperty = DependencyProperty.Register(nameof(Steps), typeof(int), typeof(GradientControl), new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnStepsChanged)));
        public int Steps
        {
            get => (int)GetValue(StepsProperty);
            set => SetValue(StepsProperty, value);
        }
        static void OnStepsChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => ((GradientControl)i).OnStepsChanged(new Value<int>(e));

        #endregion

        #region GradientControl

        public GradientControl() : base() => SetCurrentValue(GradientProperty, Media.Gradient.Default.Steps);

        #endregion

        #region Methods

        void Shift(bool ShiftAll)
        {
            for (var i = 0; i < (ShiftAll ? Gradient.Count : Gradient.Count - 1); i++)
            {
                Gradient[i].Offset
                = i == 0
                ? 0
                : Gradient[i - 1].Offset
                    + 1d.Divide(Gradient.Count.Double() - 1d)
                    .Round(2);
            }
        }

        //...

        protected virtual void OnStepsChanged(Value<int> input)
        {
            if (input.New > Gradient.Count)
            {
                //Number of bands to add
                var newValue = input.New - Gradient.Count;
                for (var i = 0; i < newValue; i++)
                {
                    var randomColor = Color.FromRgb(random.Next(0, 255).Byte(), random.Next(0, 255).Byte(), random.Next(0, 255).Byte());

                    //Add new band with random color
                    Gradient.Add(new GradientStep(1d, randomColor));

                    //If next to last band index is valid
                    if (Gradient.Count > 1)
                        Shift(false);
                }
            }
            else if (input.New < Gradient.Count)
            {
                for (var i = Gradient.Count - 1; i >= input.New; i--)
                    Gradient.Remove(Gradient[i]);

                if (SelectedStepIndex > Gradient.Count - 1)
                    SetCurrentValue(SelectedStepIndexProperty, Gradient.Count - 1);

                Shift(true);
            }
        }

        //...

        protected virtual void OnGradientChanged(Value<GradientStepCollection> input)
        {
            if (input.New != null)
            {
                SetCurrentValue(StepsProperty, input.New.Count);

                //Setting -1 first is important!
                SetCurrentValue(SelectedStepIndexProperty, -1);
                SetCurrentValue(SelectedStepIndexProperty, 0);
            }
        }

        protected virtual GradientStepCollection OnGradientCoerced(GradientStepCollection input)
        {
            if (input == null || input.Count == 0)
                return Media.Gradient.Default.Steps;

            return input;
        }

        //...

        protected virtual void OnSelectedStepIndexChanged(Value<int> input)
        {
            SetCurrentValue(SelectedStepProperty, Gradient?.Count > input.New && input.New >= 0 ? Gradient[input.New] : null);
        }

        #endregion
    }
}