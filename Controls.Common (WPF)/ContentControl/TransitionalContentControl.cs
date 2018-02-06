using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// A <see cref="ContentControl"/> that animates content as it loads and unloads.
    /// </summary>
    public class TransitionalContentControl : ContentControlBase
    {
        #region Constants

        internal const string PresentationGroup = "PresentationStates";

        internal const string NormalState = "Normal";

        internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";

        internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";

        /// <summary>
        /// 
        /// </summary>
        public const TransitionType DefaultTransitionState = TransitionType.Default;

        #endregion

        #region Fields

        ContentPresenter currentContentPresentationSite;

        ContentPresenter previousContentPresentationSite;

        bool allowIsTransitioningPropertyWrite;

        #endregion

        #region Events

        /// <summary>
        /// 
        /// </summary>
        public event RoutedEventHandler TransitionCompleted;

        #endregion

        #region Properties

        Storyboard currentTransition;
        Storyboard CurrentTransition
        {
            get
            {
                return currentTransition;
            }
            set
            {
                // decouple event
                if (currentTransition != null)
                    currentTransition.Completed -= OnTransitionCompleted;

                currentTransition = value;

                if (currentTransition != null)
                    currentTransition.Completed += OnTransitionCompleted;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsTransitioningProperty = DependencyProperty.Register("IsTransitioning", typeof(bool), typeof(TransitionalContentControl), new PropertyMetadata(OnIsTransitioningPropertyChanged));
        /// <summary>
        /// Gets/sets if the content is transitioning.
        /// </summary>
        public bool IsTransitioning
        {
            get
            {
                return (bool)GetValue(IsTransitioningProperty);
            }
            private set
            {
                allowIsTransitioningPropertyWrite = true;
                SetValue(IsTransitioningProperty, value);
                allowIsTransitioningPropertyWrite = false;
            }
        }
        static void OnIsTransitioningPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (TransitionalContentControl)d;

            if (!source.allowIsTransitioningPropertyWrite)
            {
                source.IsTransitioning = (bool)e.OldValue;
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty TransitionProperty = DependencyProperty.Register("Transition", typeof(TransitionType), typeof(TransitionalContentControl), new FrameworkPropertyMetadata(TransitionType.Default, FrameworkPropertyMetadataOptions.AffectsArrange | FrameworkPropertyMetadataOptions.Inherits, OnTransitionPropertyChanged));
        /// <summary>
        /// 
        /// </summary>
        public TransitionType Transition
        {
            get
            {
                return (TransitionType)GetValue(TransitionProperty);
            }
            set
            {
                SetValue(TransitionProperty, value);
            }
        }
        static void OnTransitionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var source = (TransitionalContentControl)d;
            var oldTransition = (TransitionType)e.OldValue;
            var newTransition = (TransitionType)e.NewValue;

            if (source.IsTransitioning)
            {
                source.AbortTransition();
            }

            // find new transition
            Storyboard newStoryboard = source.GetStoryboard(newTransition);

            // unable to find the transition.
            if (newStoryboard == null)
            {
                // could be during initialization of xaml that presentationgroups was not yet defined
                if (VisualStates.TryGetVisualStateGroup(source, PresentationGroup) == null)
                {
                    // will delay check
                    source.CurrentTransition = null;
                }
                else
                {
                    // revert to old value
                    source.SetValue(TransitionProperty, oldTransition);

                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Temporary removed exception message", newTransition));
                }
            }
            else
            {
                source.CurrentTransition = newStoryboard;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RestartTransitionOnContentChangeProperty = DependencyProperty.Register("RestartTransitionOnContentChange", typeof(bool), typeof(TransitionalContentControl), new PropertyMetadata(false, OnRestartTransitionOnContentChangePropertyChanged));
        /// <summary>
        /// 
        /// </summary>
        public bool RestartTransitionOnContentChange
        {
            get
            {
                return (bool)GetValue(RestartTransitionOnContentChangeProperty);
            }
            set
            {
                SetValue(RestartTransitionOnContentChangeProperty, value);
            }
        }
        static void OnRestartTransitionOnContentChangePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((TransitionalContentControl)d).OnRestartTransitionOnContentChangeChanged((bool)e.OldValue, (bool)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CustomVisualStatesProperty = DependencyProperty.Register("CustomVisualStates", typeof(ObservableCollection<VisualState>), typeof(TransitionalContentControl), new PropertyMetadata(null));
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<VisualState> CustomVisualStates
        {
            get
            {
                return (ObservableCollection<VisualState>)GetValue(CustomVisualStatesProperty);
            }
            set
            {
                SetValue(CustomVisualStatesProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty CustomVisualStatesNameProperty = DependencyProperty.Register("CustomVisualStatesName", typeof(string), typeof(TransitionalContentControl), new PropertyMetadata("CustomTransition"));
        /// <summary>
        /// Gets or sets the name of the custom transition visual state.
        /// </summary>
        public string CustomVisualStatesName
        {
            get
            {
                return (string)GetValue(CustomVisualStatesNameProperty);
            }
            set
            {
                SetValue(CustomVisualStatesNameProperty, value);
            }
        }

        #endregion

        #region TransitionalContentControl

        /// <summary>
        /// 
        /// </summary>
        public TransitionalContentControl()
        {
            CustomVisualStates = new ObservableCollection<VisualState>();
            DefaultStyleKey = typeof(TransitionalContentControl);
        }

        #endregion

        #region Methods

        Storyboard GetStoryboard(TransitionType newTransition)
        {
            VisualStateGroup presentationGroup = VisualStates.TryGetVisualStateGroup(this, PresentationGroup);
            Storyboard newStoryboard = null;
            if (presentationGroup != null)
            {
                var transitionName = GetTransitionName(newTransition);
                newStoryboard = presentationGroup.States
                                                 .OfType<VisualState>()
                                                 .Where(state => state.Name == transitionName)
                                                 .Select(state => state.Storyboard)
                                                 .FirstOrDefault();
            }
            return newStoryboard;
        }

        string GetTransitionName(TransitionType transition)
        {
            switch (transition)
            {
                default:
                case TransitionType.Default:
                    return "DefaultTransition";
                case TransitionType.Normal:
                    return "Normal";
                case TransitionType.Up:
                    return "UpTransition";
                case TransitionType.Down:
                    return "DownTransition";
                case TransitionType.Right:
                    return "RightTransition";
                case TransitionType.RightReplace:
                    return "RightReplaceTransition";
                case TransitionType.Left:
                    return "LeftTransition";
                case TransitionType.LeftReplace:
                    return "LeftReplaceTransition";
                case TransitionType.Custom:
                    return CustomVisualStatesName;
            }
        }

        [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "newContent", Justification = "Should be used in the future.")]
        void StartTransition(object oldContent, object newContent)
        {
            // both presenters must be available, otherwise a transition is useless.
            if (currentContentPresentationSite != null && previousContentPresentationSite != null)
            {
                if (RestartTransitionOnContentChange)
                {
                    CurrentTransition.Completed -= OnTransitionCompleted;
                }

                if (ContentTemplateSelector != null)
                {
                    previousContentPresentationSite.ContentTemplate = ContentTemplateSelector.SelectTemplate(oldContent, this);
                    currentContentPresentationSite.ContentTemplate = ContentTemplateSelector.SelectTemplate(newContent, this);
                }
                else
                {
                    previousContentPresentationSite.ContentTemplate = ContentTemplate;
                    currentContentPresentationSite.ContentTemplate = ContentTemplate;
                }
                currentContentPresentationSite.Content = newContent;
                previousContentPresentationSite.Content = oldContent;

                // and start a new transition
                if (!IsTransitioning || RestartTransitionOnContentChange)
                {
                    if (RestartTransitionOnContentChange)
                    {
                        CurrentTransition.Completed += OnTransitionCompleted;
                    }
                    IsTransitioning = true;
                    VisualStateManager.GoToState(this, NormalState, false);
                    VisualStateManager.GoToState(this, GetTransitionName(Transition), true);
                }
            }
        }

        void OnTransitionCompleted(object sender, EventArgs e)
        {
            AbortTransition();
            TransitionCompleted?.Invoke(this, new RoutedEventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldContent"></param>
        /// <param name="newContent"></param>
        protected override void OnContentChanged(object oldContent, object newContent)
        {
            base.OnContentChanged(oldContent, newContent);

            StartTransition(oldContent, newContent);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnApplyTemplate()
        {
            if (IsTransitioning)
            {
                AbortTransition();
            }

            if (CustomVisualStates != null && CustomVisualStates.Any())
            {
                var presentationGroup = VisualStates.TryGetVisualStateGroup(this, PresentationGroup);
                if (presentationGroup != null)
                {
                    foreach (var state in CustomVisualStates)
                    {
                        presentationGroup.States.Add(state);
                    }
                }
            }

            base.OnApplyTemplate();

            previousContentPresentationSite = GetTemplateChild(PreviousContentPresentationSitePartName) as ContentPresenter;
            currentContentPresentationSite = GetTemplateChild(CurrentContentPresentationSitePartName) as ContentPresenter;

            if (currentContentPresentationSite != null)
            {
                if (ContentTemplateSelector != null)
                {
                    currentContentPresentationSite.ContentTemplate = ContentTemplateSelector.SelectTemplate(Content, this);
                }
                else
                {
                    currentContentPresentationSite.ContentTemplate = ContentTemplate;
                }
                currentContentPresentationSite.Content = Content;
            }

            // hookup currenttransition
            Storyboard transition = GetStoryboard(Transition);
            CurrentTransition = transition;
            if (transition == null)
            {
                var invalidTransition = Transition;
                // revert to default
                Transition = DefaultTransitionState;

                throw new ArgumentException(string.Format("'{0}' Transition could not be found!", invalidTransition), "Transition");
            }
            VisualStateManager.GoToState(this, NormalState, false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        protected virtual void OnRestartTransitionOnContentChangeChanged(bool oldValue, bool newValue)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void AbortTransition()
        {
            // go to normal state and release our hold on the old content.
            VisualStateManager.GoToState(this, NormalState, false);
            IsTransitioning = false;
            if (previousContentPresentationSite != null)
            {
                previousContentPresentationSite.ContentTemplate = null;
                previousContentPresentationSite.Content = null;
            }
        }

        /// <summary>
        /// Reload the current transition if the content is the same.
        /// </summary>
        public void ReloadTransition()
        {
            // both presenters must be available, otherwise a transition is useless.
            if (currentContentPresentationSite != null && previousContentPresentationSite != null)
            {
                if (RestartTransitionOnContentChange)
                {
                    CurrentTransition.Completed -= OnTransitionCompleted;
                }
                if (!IsTransitioning || RestartTransitionOnContentChange)
                {
                    if (RestartTransitionOnContentChange)
                    {
                        CurrentTransition.Completed += OnTransitionCompleted;
                    }
                    IsTransitioning = true;
                    VisualStateManager.GoToState(this, NormalState, false);
                    VisualStateManager.GoToState(this, GetTransitionName(Transition), true);
                }
            }
        }

        #endregion
    }
}