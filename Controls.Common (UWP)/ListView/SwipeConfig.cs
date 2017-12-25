using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Imagin.Controls.Common
{
    public class SwipeConfig
    {
        public EasingFunctionBase LeftEasingFunc { get; set; }

        public EasingFunctionBase RightEasingFunc { get; set; }

        public TranslateTransform MainTransform { get; set; }

        public ScaleTransform SwipeClipTransform { get; set; }

        public RectangleGeometry SwipeClipRectangle { get; set; }

        public int Duration { get; set; }

        public SwipeMode LeftSwipeMode { get; set; }

        public SwipeMode RightSwipeMode { get; set; }

        public SwipeDirection Direction { get; set; }

        public double LeftActionRateForSwipeLength { get; set; }

        public double RightActionRateForSwipeLength { get; set; }

        public double LeftSwipeLengthRate { get; set; }

        public double RightSwipeLengthRate { get; set; }

        public double ItemActualWidth { get; set; }

        public double ItemActualHeight { get; set; }

        public double CurrentSwipeWidth { get; set; }

        public Timeline LeftCustomStoreAnimation { get; set; }

        public Timeline LeftCustomTriggerAnimation { get; set; }

        public Timeline RightCustomStoreAnimation { get; set; }

        public Timeline RightCustomTriggerAnimation { get; set; }

        public bool EnableSwipeRight { get; set; }

        public bool EnableSwipeLeft { get; set; }


        public double LeftRateForActualWidth => LeftSwipeLengthRate * LeftActionRateForSwipeLength;

        public double RightRateForActualWidth => RightSwipeLengthRate * RightActionRateForSwipeLength;

        public bool CanSwipeLeft => Direction == SwipeDirection.Left && LeftSwipeMode != SwipeMode.None && EnableSwipeLeft;

        public bool CanSwipeRight => Direction == SwipeDirection.Right && RightSwipeMode != SwipeMode.None && EnableSwipeRight;

        public EasingFunctionBase EasingFunc => Direction == SwipeDirection.Left ? LeftEasingFunc : RightEasingFunc;

        public double SwipeLengthRate => Direction == SwipeDirection.Left ? LeftSwipeLengthRate : RightSwipeLengthRate;

        public double ActionRateForSwipeLength => Direction == SwipeDirection.Left ? LeftActionRateForSwipeLength : RightActionRateForSwipeLength;

        public SwipeMode GetSwipeMode(SwipeDirection swipeDirection)
        {
            return swipeDirection == SwipeDirection.Left ? LeftSwipeMode : RightSwipeMode;
        }

        public double CurrentSwipeRate => CurrentSwipeWidth / ItemActualWidth / SwipeLengthRate;

        public double TriggerActionTargetWidth => ItemActualWidth * SwipeLengthRate;

        public Timeline CustomStoreAnimation => Direction == SwipeDirection.Left ? LeftCustomStoreAnimation : RightCustomStoreAnimation;

        public Timeline CustomTriggerAnimation => Direction == SwipeDirection.Left ? LeftCustomTriggerAnimation : RightCustomTriggerAnimation;

        public void ResetSwipeClipCenterX()
        {
            SwipeClipTransform.CenterX = Direction == SwipeDirection.Left ? 0 : ItemActualWidth;
        }

        public void AdjustForSwipeFixCompleted(double targetWidth)
        {
            SwipeClipTransform.ScaleX = 1;
            if (Direction == SwipeDirection.Left)
                SwipeClipRectangle.Rect = new Rect(0, 0, targetWidth, ItemActualHeight);
            else
                SwipeClipRectangle.Rect = new Rect(ItemActualWidth - targetWidth, 0, targetWidth, ItemActualHeight);
        }

        public void AdjustForSwipeToFixStarted()
        {
            ResetSwipeClipCenterX();

            if (Direction == SwipeDirection.Left)
                SwipeClipRectangle.Rect = new Rect(0, 0, 1, ItemActualHeight);
            else
                SwipeClipRectangle.Rect = new Rect(ItemActualWidth - 1, 0, 1, ItemActualHeight);
        }

        public void AdjustForNotSwipeFixCompleted()
        {
            SwipeClipRectangle.Rect = new Rect(0, 0, 0, 0);
            SwipeClipTransform.ScaleX = 1;
        }
    }
}
