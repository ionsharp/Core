using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Imagin.Controls.Common
{
    public delegate void AnimationCallback(EasingFunctionBase easingFunc, double itemToX, double duration);

    public class SwipeReleaseAnimationConstructor
    {
        private SwipeConfig _config = new SwipeConfig();

        public SwipeConfig Config
        {
            get { return _config; }
            set { _config = value; }
        }

        public static SwipeReleaseAnimationConstructor Create(SwipeConfig config)
        {
            SwipeReleaseAnimationConstructor constructor = new SwipeReleaseAnimationConstructor();
            constructor.Config = config;
            return constructor;
        }

        public void DisplaySwipeAnimation(SwipeDirection direction, AnimationCallback beginTriggerCallback, Action triggerCompleteCallback, AnimationCallback beginRestoreCallback, Action restoreCompleteCallback)
        {
            var swipeAnimator = GetSwipeAnimator(_config.GetSwipeMode(direction));

            if (swipeAnimator == null)
                return;

            Config.ResetSwipeClipCenterX();

            if (swipeAnimator.ShouldTriggerAction(Config))
            {
                swipeAnimator.ActionTrigger(direction, Config, beginTriggerCallback, triggerCompleteCallback);
            }
            else
            {
                swipeAnimator.Restore(Config, beginRestoreCallback, restoreCompleteCallback);
            }
        }

        public ISwipeAnimator GetSwipeAnimator(SwipeMode mode)
        {
            switch (mode)
            {
                case SwipeMode.Collapse:
                    return CollapseSwipeAnimator.Instance;
                case SwipeMode.Fix:
                    return FixedSwipeAnimator.Instance;
                case SwipeMode.Expand:
                    return ExpandSwipeAnimator.Instance;
                case SwipeMode.None:
                    return null;
                default:
                    throw new NotSupportedException("not supported swipe mode");
            }
        }
    }

    public interface ISwipeAnimator
    {
        void Restore(SwipeConfig config, AnimationCallback beginRestoreCallback, Action restoreCompleteCallback);
        void ActionTrigger(SwipeDirection direction, SwipeConfig config, AnimationCallback beginTriggerCallback, Action triggerCompleteCallback);
        bool ShouldTriggerAction(SwipeConfig config);
    }

    public abstract class BaseSwipeAnimator : ISwipeAnimator
    {
        public abstract void ActionTrigger(SwipeDirection direction, SwipeConfig config, AnimationCallback beginTriggerCallback, Action triggerCompleteCallback);

        public virtual bool ShouldTriggerAction(SwipeConfig config)
        {
            return config.ActionRateForSwipeLength <= config.CurrentSwipeRate;
        }

        public void Restore(SwipeConfig config, AnimationCallback beginRestoreCallback, Action restoreCompleteCallback)
        {
            beginRestoreCallback?.Invoke(config.EasingFunc, 0, config.Duration);

            DisplayAnimation(config, 0, 0, ()=>
            {
                config.SwipeClipRectangle.Rect = new Rect(0, 0, 0, config.ItemActualHeight);
                config.SwipeClipTransform.ScaleX = 1;

                restoreCompleteCallback?.Invoke();
            });
        }

        protected void DisplayAnimation(SwipeConfig config, double itemTo, double clipTo, Action complete)
        {
            DisplayAnimation(config, "X", itemTo, "ScaleX", clipTo, complete);
        }

        protected void DisplayAnimation(SwipeConfig config, string itemProperty, double itemTo, string clipProperty, double clipTo, Action complete)
        {
            Storyboard animStory = new Storyboard();
            animStory.Children.Add(Utils.CreateDoubleAnimation(config.MainTransform, itemProperty, config.EasingFunc, itemTo, config.Duration));
            animStory.Children.Add(Utils.CreateDoubleAnimation(config.SwipeClipTransform, clipProperty, config.EasingFunc, clipTo, config.Duration));

            animStory.Completed += (sender, e) =>
            {
                complete?.Invoke();
            };

            animStory.Begin();
        }
    }

    public class CollapseSwipeAnimator : BaseSwipeAnimator
    {
        public readonly static ISwipeAnimator Instance = new CollapseSwipeAnimator();

        public override void ActionTrigger(SwipeDirection direction, SwipeConfig config, AnimationCallback beginTriggerCallback, Action triggerCompleteCallback)
        {
            beginTriggerCallback?.Invoke(config.EasingFunc, 0, config.Duration);

            DisplayAnimation(config, 0, 0, () =>
            {
                triggerCompleteCallback?.Invoke();
                config.AdjustForNotSwipeFixCompleted();
            });
        }
    }

    public class FixedSwipeAnimator : BaseSwipeAnimator
    {
        public static readonly FixedSwipeAnimator Instance = new FixedSwipeAnimator();

        public override void ActionTrigger(SwipeDirection direction, SwipeConfig config, AnimationCallback beginTriggerCallback, Action triggerCompleteCallback)
        {
            var targetWidth = config.TriggerActionTargetWidth;
            var targetX = config.Direction == SwipeDirection.Left ? targetWidth : -targetWidth;
            var clipScaleX = targetWidth / config.CurrentSwipeWidth;

            beginTriggerCallback?.Invoke(config.EasingFunc, targetX, config.Duration);

            DisplayAnimation(config, targetX, clipScaleX, ()=>
            {
                triggerCompleteCallback?.Invoke();
                config.AdjustForSwipeFixCompleted(targetWidth);
            });
        }

        public void SwipeTo(SwipeDirection direction, SwipeConfig config, bool animated)
        {
            var targetWidth = config.TriggerActionTargetWidth;
            var targetX = config.Direction == SwipeDirection.Left ? targetWidth : -targetWidth;
            config.AdjustForSwipeToFixStarted();
            var clipScaleX = targetWidth;

            if (animated)
            {
                DisplayAnimation(config, targetX, clipScaleX, () =>
                {
                    config.AdjustForSwipeFixCompleted(targetWidth);
                });
            }
            else
            {
                config.MainTransform.X = targetX;
                config.AdjustForSwipeFixCompleted(targetWidth);
            }
        }
    }

    public class ExpandSwipeAnimator : BaseSwipeAnimator
    {
        public readonly static ISwipeAnimator Instance = new ExpandSwipeAnimator();

        public override void ActionTrigger(SwipeDirection direction, SwipeConfig config, AnimationCallback beginTriggerCallback, Action triggerCompleteCallback)
        {
            var targetX = config.Direction == SwipeDirection.Left ? config.ItemActualWidth : -config.ItemActualWidth;
            var clipScaleX = config.ItemActualWidth / config.CurrentSwipeWidth;

            beginTriggerCallback?.Invoke(config.EasingFunc, targetX, config.Duration);

            DisplayAnimation(config, targetX, clipScaleX, ()=>
            {
                triggerCompleteCallback?.Invoke();
                config.AdjustForNotSwipeFixCompleted();
            });
        }
    }
}
