using Imagin.Common.Converters;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class DockRootControl : ContentControl
    {
        #region Keys

        public static readonly ResourceKey<GridSplitter> GridSplitterStyleKey
            = new();

        public static readonly ReferenceKey<ImageElement> PrimaryMarkerKey 
            = new();

        public static readonly ReferenceKey<ImageElement> SecondaryMarkerKey 
            = new();

        #endregion

        #region (IValueConverter) CenterMarkerVisibilityConverter

        static IValueConverter centerMarkerVisibilityConverter;
        public static IValueConverter CenterMarkerVisibilityConverter => centerMarkerVisibilityConverter ??= new SimpleConverter<IDockControl, Visibility>(i =>
        {
            if (i.DockControl.Dragging)
            {
                if (i.DockControl.Drag.Content?.FirstOrDefault<Content>() is Document && i.DockControl.Drag.MouseOver is DockDocumentControl)
                    return Visibility.Visible;

                if (i.DockControl.Drag.Content?.FirstOrDefault<Content>() is Models.Panel)
                {
                    if (i.DockControl.Drag.MouseOver is DockDocumentControl documentControl)
                        return Visibility.Visible;

                    else if (i.DockControl.Drag.MouseOver is DockPanelControl panelControl)
                    {
                        if (!i.DockControl.Drag.Content.Contains<Models.Panel>(k => !k.CanShare) && !panelControl.Source.Contains<Models.Panel>(k => !k.CanShare))
                            return Visibility.Visible;
                    }
                }
            }
            return Visibility.Collapsed;
        },
        j => null);

        #endregion

        #region (IMultiValueConverter) MarkerVisibilityConverter

        public static readonly IMultiValueConverter MarkerVisibilityConverter = new MultiConverter<Visibility>(i =>
        {
            if (i.Values?.Length == 2)
            {
                if (i.Values[0] is IDockControl a)
                {
                    if (i.Values[1] is DockRootControl b)
                        return a.Root.Equals(b).Visibility();
                }
            }
            return Visibility.Collapsed;
        });

        #endregion

        #region (IValueConverter) SecondaryMarkerVisibilityConverter

        public static readonly IValueConverter SecondaryMarkerVisibilityConverter = new SimpleConverter<IDockControl, Visibility>(i =>
        {
            if (i is DockPanelControl j)
            {
                if (j.Collapse)
                    return Visibility.Collapsed;
            }
            return Visibility.Visible;
        });

        #endregion

        #region (IValueConverter) TertiaryMarkerVisibilityConverter

        public static readonly IValueConverter TertiaryMarkerVisibilityConverter = new SimpleConverter<IDockControl, Visibility>(i =>
        {
            if (i is DockDocumentControl)
                return Visibility.Visible;

            return Visibility.Collapsed;
        });

        #endregion
        
        #region Fields

        public readonly DockWindow Window;

        #endregion

        #region Properties

        public IDockControl Child
        {
            get => Content as IDockControl;
            set => Content = value as UIElement;
        }

        public DockControl DockControl { get; private set; }

        //...

        #region ActiveContent

        static readonly DependencyPropertyKey ActiveContentKey = DependencyProperty.RegisterReadOnly(nameof(ActiveContent), typeof(Content), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty ActiveContentProperty = ActiveContentKey.DependencyProperty;
        public Content ActiveContent
        {
            get => (Content)GetValue(ActiveContentProperty);
            private set => SetValue(ActiveContentKey, value);
        }

        #endregion

        #region BottomPanel

        static readonly DependencyPropertyKey BottomPanelKey = DependencyProperty.RegisterReadOnly(nameof(BottomPanel), typeof(Models.Panel), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty BottomPanelProperty = BottomPanelKey.DependencyProperty;
        public Models.Panel BottomPanel
        {
            get => (Models.Panel)GetValue(BottomPanelProperty);
            private set => SetValue(BottomPanelKey, value);
        }

        #endregion

        #region BottomPanels

        static readonly DependencyPropertyKey BottomPanelsKey = DependencyProperty.RegisterReadOnly(nameof(BottomPanels), typeof(DockAnchorPanelCollection), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty BottomPanelsProperty = BottomPanelsKey.DependencyProperty;
        public DockAnchorPanelCollection BottomPanels
        {
            get => (DockAnchorPanelCollection)GetValue(BottomPanelsProperty);
            private set => SetValue(BottomPanelsKey, value);
        }

        #endregion

        #region IsBottomOpen

        static readonly DependencyPropertyKey IsBottomOpenKey = DependencyProperty.RegisterReadOnly(nameof(IsBottomOpen), typeof(bool), typeof(DockRootControl), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsBottomOpenProperty = IsBottomOpenKey.DependencyProperty;
        public bool IsBottomOpen
        {
            get => (bool)GetValue(IsBottomOpenProperty);
            private set => SetValue(IsBottomOpenKey, value);
        }

        #endregion

        #region IsEmpty

        public bool IsEmpty
        {
            get
            {
                var a = Minimized.Count == 0;
                var b = !AllPins().Contains(i => i.Count > 0);
                var c = !DockControl.DocumentControls.Contains(i => ReferenceEquals(i.Root, this) && i.Source.Count > 0);
                var d = !DockControl.PanelControls.Contains(i => ReferenceEquals(i.Root, this));
                return a && b && c && d;
            }
        }

        #endregion

        #region IsLeftOpen

        static readonly DependencyPropertyKey IsLeftOpenKey = DependencyProperty.RegisterReadOnly(nameof(IsLeftOpen), typeof(bool), typeof(DockRootControl), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsLeftOpenProperty = IsLeftOpenKey.DependencyProperty;
        public bool IsLeftOpen
        {
            get => (bool)GetValue(IsLeftOpenProperty);
            private set => SetValue(IsLeftOpenKey, value);
        }

        #endregion

        #region IsRightOpen

        static readonly DependencyPropertyKey IsRightOpenKey = DependencyProperty.RegisterReadOnly(nameof(IsRightOpen), typeof(bool), typeof(DockRootControl), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsRightOpenProperty = IsRightOpenKey.DependencyProperty;
        public bool IsRightOpen
        {
            get => (bool)GetValue(IsRightOpenProperty);
            private set => SetValue(IsRightOpenKey, value);
        }

        #endregion

        #region IsTopOpen

        static readonly DependencyPropertyKey IsTopOpenKey = DependencyProperty.RegisterReadOnly(nameof(IsTopOpen), typeof(bool), typeof(DockRootControl), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsTopOpenProperty = IsTopOpenKey.DependencyProperty;
        public bool IsTopOpen
        {
            get => (bool)GetValue(IsTopOpenProperty);
            private set => SetValue(IsTopOpenKey, value);
        }

        #endregion

        #region LeftPanel

        static readonly DependencyPropertyKey LeftPanelKey = DependencyProperty.RegisterReadOnly(nameof(LeftPanel), typeof(Models.Panel), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty LeftPanelProperty = LeftPanelKey.DependencyProperty;
        public Models.Panel LeftPanel
        {
            get => (Models.Panel)GetValue(LeftPanelProperty);
            private set => SetValue(LeftPanelKey, value);
        }

        #endregion

        #region LeftPanels

        static readonly DependencyPropertyKey LeftPanelsKey = DependencyProperty.RegisterReadOnly(nameof(LeftPanels), typeof(DockAnchorPanelCollection), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty LeftPanelsProperty = LeftPanelsKey.DependencyProperty;
        public DockAnchorPanelCollection LeftPanels
        {
            get => (DockAnchorPanelCollection)GetValue(LeftPanelsProperty);
            private set => SetValue(LeftPanelsKey, value);
        }

        #endregion

        #region Minimized

        static readonly DependencyPropertyKey MinimizedKey = DependencyProperty.RegisterReadOnly(nameof(Minimized), typeof(DockAnchorDocumentCollection), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty MinimizedProperty = MinimizedKey.DependencyProperty;
        public DockAnchorDocumentCollection Minimized
        {
            get => (DockAnchorDocumentCollection)GetValue(MinimizedProperty);
            private set => SetValue(MinimizedKey, value);
        }

        #endregion

        #region PopupTransition

        public static readonly DependencyProperty PopupTransitionProperty = DependencyProperty.Register(nameof(PopupTransition), typeof(Transitions), typeof(DockRootControl), new FrameworkPropertyMetadata(Transitions.Default));
        public Transitions PopupTransition
        {
            get => (Transitions)GetValue(PopupTransitionProperty);
            set => SetValue(PopupTransitionProperty, value);
        }

        #endregion

        #region RightPanel

        static readonly DependencyPropertyKey RightPanelKey = DependencyProperty.RegisterReadOnly(nameof(RightPanel), typeof(Models.Panel), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty RightPanelProperty = RightPanelKey.DependencyProperty;
        public Models.Panel RightPanel
        {
            get => (Models.Panel)GetValue(RightPanelProperty);
            private set => SetValue(RightPanelKey, value);
        }

        #endregion

        #region RightPanels

        static readonly DependencyPropertyKey RightPanelsKey = DependencyProperty.RegisterReadOnly(nameof(RightPanels), typeof(DockAnchorPanelCollection), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty RightPanelsProperty = RightPanelsKey.DependencyProperty;
        public DockAnchorPanelCollection RightPanels
        {
            get => (DockAnchorPanelCollection)GetValue(RightPanelsProperty);
            private set => SetValue(RightPanelsKey, value);
        }

        #endregion

        #region SecondaryMarkerPosition

        static readonly DependencyPropertyKey SecondaryMarkerPositionKey = DependencyProperty.RegisterReadOnly(nameof(SecondaryMarkerPosition), typeof(Point2D), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SecondaryMarkerPositionProperty = SecondaryMarkerPositionKey.DependencyProperty;
        public Point2D SecondaryMarkerPosition
        {
            get => (Point2D)GetValue(SecondaryMarkerPositionProperty);
            private set => SetValue(SecondaryMarkerPositionKey, value);
        }

        #endregion

        #region SelectionPoints

        static readonly DependencyPropertyKey SelectionPointsKey = DependencyProperty.RegisterReadOnly(nameof(SelectionPoints), typeof(System.Windows.Media.PointCollection), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SelectionPointsProperty = SelectionPointsKey.DependencyProperty;
        public System.Windows.Media.PointCollection SelectionPoints
        {
            get => (System.Windows.Media.PointCollection)GetValue(SelectionPointsProperty);
            private set => SetValue(SelectionPointsKey, value);
        }

        #endregion

        #region TopPanel

        static readonly DependencyPropertyKey TopPanelKey = DependencyProperty.RegisterReadOnly(nameof(TopPanel), typeof(Models.Panel), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty TopPanelProperty = TopPanelKey.DependencyProperty;
        public Models.Panel TopPanel
        {
            get => (Models.Panel)GetValue(TopPanelProperty);
            private set => SetValue(TopPanelKey, value);
        }

        #endregion

        #region TopPanels

        static readonly DependencyPropertyKey TopPanelsKey = DependencyProperty.RegisterReadOnly(nameof(TopPanels), typeof(DockAnchorPanelCollection), typeof(DockRootControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty TopPanelsProperty = TopPanelsKey.DependencyProperty;
        public DockAnchorPanelCollection TopPanels
        {
            get => (DockAnchorPanelCollection)GetValue(TopPanelsProperty);
            private set => SetValue(TopPanelsKey, value);
        }

        #endregion

        #endregion

        #region DockRootControl

        public DockRootControl(DockControl control, DockWindow window) : base()
        {
            DockControl
                = control;

            BottomPanels
                = new(this);
            LeftPanels
                = new(this);
            RightPanels
                = new(this);
            TopPanels
                = new(this);

            Minimized
                = new(this);
            SecondaryMarkerPosition
                = new();
            SelectionPoints
                = new();
            Window
                = window;

            this.RegisterHandler(OnLoaded, OnUnloaded);
        }

        //...

        internal void Activate(DockContentControl input)
        {
            ActiveContent = input.SelectedItem as Content;
            DockControl.Activate(this);
        }

        internal void Deactivate(DockContentControl input)
        {
            ActiveContent = null;
            DockControl.Deactivate(this);
        }
        
        #endregion

        #region Methods

        void OnLoaded()
        {
            BottomPanels.CollectionChanged 
                += OnPinsChanged;
            LeftPanels.CollectionChanged
                += OnPinsChanged;
            RightPanels.CollectionChanged
                += OnPinsChanged;
            TopPanels.CollectionChanged
                += OnPinsChanged;

            foreach (ImageElement i in this.GetChildren(PrimaryMarkerKey))
            {
                i.MouseEnter
                    += OnPrimaryMarkerMouseEnter;
                i.MouseLeave
                    += OnPrimaryMarkerMouseLeave;
                i.MouseLeftButtonUp
                    += OnPrimaryMarkerMouseLeftButtonUp;
            }
            foreach (ImageElement i in this.GetChildren(SecondaryMarkerKey))
            {
                i.MouseEnter
                    += OnSecondaryMarkerMouseEnter;
                i.MouseLeave
                    += OnSecondaryMarkerMouseLeave;
                i.MouseLeftButtonUp
                    += OnSecondaryMarkerMouseLeftButtonUp;
            }
        }

        void OnUnloaded()
        {
            BottomPanels.CollectionChanged
                -= OnPinsChanged;
            LeftPanels.CollectionChanged
                -= OnPinsChanged;
            RightPanels.CollectionChanged
                -= OnPinsChanged;
            TopPanels.CollectionChanged
                -= OnPinsChanged;

            foreach (ImageElement i in this.GetChildren(PrimaryMarkerKey))
            {
                i.MouseEnter
                    -= OnPrimaryMarkerMouseEnter;
                i.MouseLeave
                    -= OnPrimaryMarkerMouseLeave;
                i.MouseLeftButtonUp
                    -= OnPrimaryMarkerMouseLeftButtonUp;
            }
            foreach (ImageElement i in this.GetChildren(SecondaryMarkerKey))
            {
                i.MouseEnter
                    -= OnSecondaryMarkerMouseEnter;
                i.MouseLeave
                    -= OnSecondaryMarkerMouseLeave;
                i.MouseLeftButtonUp
                    -= OnSecondaryMarkerMouseLeftButtonUp;
            }
        }

        //...

        void OnPinsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    IsBottomOpen = IsLeftOpen = IsRightOpen = IsTopOpen = false;
                    break;
            }
        }

        //...

        void OnPrimaryMarkerMouseEnter(object sender, MouseEventArgs e)
        {
            if (DockControl.Dragging)
            {
                if (sender is FrameworkElement element)
                {
                    if (element.Tag is SecondaryDocks a)
                    {
                        switch (a)
                        {
                            case SecondaryDocks.Left:
                                Select(0, 0, ActualHeight, ActualWidth / 2);
                                break;
                            case SecondaryDocks.Top:
                                Select(0, 0, ActualHeight / 2, ActualWidth);
                                break;
                            case SecondaryDocks.Right:
                                Select(ActualWidth / 2, 0, ActualHeight, ActualWidth / 2);
                                break;
                            case SecondaryDocks.Bottom:
                                Select(0, ActualHeight / 2, ActualHeight / 2, ActualWidth);
                                break;
                        }
                    }
                }
            }
        }

        void OnPrimaryMarkerMouseLeave(object sender, MouseEventArgs e) => Select(0, 0, 0, 0);

        void OnPrimaryMarkerMouseLeftButtonUp(object sender, MouseButtonEventArgs e) 
            => DockControl.DockPrimary((SecondaryDocks)(sender as FrameworkElement).Tag);

        //...

        void OnSecondaryMarkerMouseEnter(object sender, MouseEventArgs e)
        {
            if (DockControl.Dragging)
            {
                if (sender is FrameworkElement element)
                {
                    if (element.Tag is SecondaryDocks a)
                    {
                        switch (a)
                        {
                            case SecondaryDocks.Left:
                                Select
                                    (DockControl.Drag.MousePosition.X - (DockControl.Drag.MouseOver.ActualWidth / 2),
                                    DockControl.Drag.MousePosition.Y - (DockControl.Drag.MouseOver.ActualHeight / 2),
                                    DockControl.Drag.MouseOver.ActualHeight,
                                    DockControl.Drag.MouseOver.ActualWidth / 2 / 2);
                                break;
                            case SecondaryDocks.Top:
                                Select
                                    (DockControl.Drag.MousePosition.X - (DockControl.Drag.MouseOver.ActualWidth / 2),
                                    DockControl.Drag.MousePosition.Y - (DockControl.Drag.MouseOver.ActualHeight / 2),
                                    DockControl.Drag.MouseOver.ActualHeight / 2 / 2,
                                    DockControl.Drag.MouseOver.ActualWidth);
                                break;
                            case SecondaryDocks.Right:
                                Select
                                    (DockControl.Drag.MousePosition.X + (DockControl.Drag.MouseOver.ActualWidth / 2 / 2),
                                    DockControl.Drag.MousePosition.Y - (DockControl.Drag.MouseOver.ActualHeight / 2),
                                    DockControl.Drag.MouseOver.ActualHeight,
                                    DockControl.Drag.MouseOver.ActualWidth / 2 / 2);
                                break;
                            case SecondaryDocks.Bottom:
                                Select
                                    (DockControl.Drag.MousePosition.X - (DockControl.Drag.MouseOver.ActualWidth / 2),
                                    DockControl.Drag.MousePosition.Y + (DockControl.Drag.MouseOver.ActualHeight / 2 / 2),
                                    DockControl.Drag.MouseOver.ActualHeight / 2 / 2,
                                    DockControl.Drag.MouseOver.ActualWidth);
                                break;
                            case SecondaryDocks.Center:
                                Select(DockControl.Drag.MousePosition.X - (DockControl.Drag.MouseOver.ActualWidth / 2), DockControl.Drag.MousePosition.Y - (DockControl.Drag.MouseOver.ActualHeight / 2), DockControl.Drag.MouseOver.ActualHeight, DockControl.Drag.MouseOver.ActualWidth);
                                break;
                        }
                    }
                    else if (element.Tag is TertiaryDocks b)
                    {
                        switch (b)
                        {
                            case TertiaryDocks.Left:
                                Select
                                    (DockControl.Drag.MousePosition.X - (DockControl.Drag.MouseOver.ActualWidth / 2), 
                                    DockControl.Drag.MousePosition.Y - (DockControl.Drag.MouseOver.ActualHeight / 2), 
                                    DockControl.Drag.MouseOver.ActualHeight, 
                                    DockControl.Drag.MouseOver.ActualWidth / 2);
                                break;
                            case TertiaryDocks.Top:
                                Select
                                    (DockControl.Drag.MousePosition.X - (DockControl.Drag.MouseOver.ActualWidth / 2), 
                                    DockControl.Drag.MousePosition.Y - (DockControl.Drag.MouseOver.ActualHeight / 2), 
                                    DockControl.Drag.MouseOver.ActualHeight / 2, 
                                    DockControl.Drag.MouseOver.ActualWidth);
                                break;
                            case TertiaryDocks.Right:
                                Select
                                    (DockControl.Drag.MousePosition.X, 
                                    DockControl.Drag.MousePosition.Y - (DockControl.Drag.MouseOver.ActualHeight / 2), 
                                    DockControl.Drag.MouseOver.ActualHeight, 
                                    DockControl.Drag.MouseOver.ActualWidth / 2);
                                break;
                            case TertiaryDocks.Bottom:
                                Select
                                    (DockControl.Drag.MousePosition.X - (DockControl.Drag.MouseOver.ActualWidth / 2), 
                                    DockControl.Drag.MousePosition.Y, 
                                    DockControl.Drag.MouseOver.ActualHeight / 2, 
                                    DockControl.Drag.MouseOver.ActualWidth);
                                break;
                        }
                    }
                }
            }
        }

        void OnSecondaryMarkerMouseLeave(object sender, MouseEventArgs e) => Select(0, 0, 0, 0);

        void OnSecondaryMarkerMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element)
            {
                if (element.Tag is SecondaryDocks a)
                    DockControl.DockSecondary(a);
                else if (element.Tag is TertiaryDocks b)
                    DockControl.DockTertiary(b);
            }
        }

        //...

        internal void MarkSecondary(double x, double y) { SecondaryMarkerPosition.X = x; SecondaryMarkerPosition.Y = y; }

        //...

        void Preview(DockPanelButton i)
        {
            if (i.IsChecked == false)
            {
                IsBottomOpen = IsLeftOpen = IsRightOpen = IsTopOpen = false;
                return;
            }

            var control
                = i.FindParent<DockPanelBar>();
            var panel
                = i.DataContext as Models.Panel;

            foreach (var j in control.FindVisualChildren<CheckBox>())
            {
                if (j != i)
                    j.IsChecked = false;
            }

            if (control.ItemsSource == BottomPanels)
            {
                BottomPanel = panel;
                IsBottomOpen = true;
            }
            if (control.ItemsSource == LeftPanels)
            {
                LeftPanel = panel;
                IsLeftOpen = true;
            }
            if (control.ItemsSource == RightPanels)
            {
                RightPanel = panel;
                IsRightOpen = true;
            }
            if (control.ItemsSource == TopPanels)
            {
                TopPanel = panel;
                IsTopOpen = true;
            }
        }

        void Select(DockPanelButton i)
        {
            var start = i.TranslatePoint(new Point(0, 0), this);

            var j = i.FindParent<DockPanelBar>();
            if (ReferenceEquals(j.Source, j.Root.LeftPanels))
            {
                SelectionPoints = new System.Windows.Media.PointCollection()
                {
                    start,
                    new Point(start.X + i.ActualHeight, start.Y),
                    new Point(start.X + i.ActualHeight, start.Y + i.ActualWidth),
                    new Point(start.X, start.Y + i.ActualWidth)
                };
            }
            else if (ReferenceEquals(j.Source, j.Root.RightPanels))
            {
                SelectionPoints = new System.Windows.Media.PointCollection()
                {
                    start,
                    new Point(start.X - i.ActualHeight, start.Y),
                    new Point(start.X - i.ActualHeight, start.Y + i.ActualWidth),
                    new Point(start.X, start.Y + i.ActualWidth)
                };
            }
            else
            {
                SelectionPoints = new System.Windows.Media.PointCollection()
                {
                    start,
                    new Point(start.X, start.Y + i.ActualHeight),
                    new Point(start.X + i.ActualWidth, start.Y + i.ActualHeight),
                    new Point(start.X + i.ActualWidth, start.Y)
                };
            }
        }

        void Select(FrameworkElement i)
        {
            if (i is DockPanelButton j)
                Select(j);

            else if (i?.FindParent<TabItem>() is TabItem k)
            {
                var parent = k.FindParent<TabControl>();
                if (parent == null)
                    return;

                var start0 = k.TranslatePoint(new Point(0, 0), this);
                if (parent is DockPanelControl panelControl)
                {
                    if (panelControl.Collapse)
                    {
                        SelectionPoints = new System.Windows.Media.PointCollection()
                        {
                            start0,
                            new Point(start0.X, start0.Y + k.ActualHeight),
                            new Point(start0.X + k.ActualWidth, start0.Y + k.ActualHeight),
                            new Point(start0.X + k.ActualWidth, start0.Y)
                        };
                        return;
                    }
                }

                var start1 = parent.TranslatePoint(new Point(0, 0), this);
                switch (parent.TabStripPlacement)
                {
                    case Dock.Bottom:
                        SelectionPoints = new System.Windows.Media.PointCollection()
                        {
                            start1,
                            new Point(start1.X, start1.Y + parent.ActualHeight - k.ActualHeight),
                            start0,
                            new Point(start0.X, start0.Y + k.ActualHeight),
                            new Point(start0.X + k.ActualWidth, start0.Y + k.ActualHeight),
                            new Point(start0.X + k.ActualWidth, start0.Y),
                            new Point(start1.X + parent.ActualWidth, start1.Y + parent.ActualHeight - k.ActualHeight),
                            new Point(start1.X + parent.ActualWidth, start1.Y)
                        };
                        break;

                    case Dock.Top:
                        SelectionPoints = new System.Windows.Media.PointCollection()
                        {
                            new Point(start1.X, start1.Y + parent.ActualHeight),
                            new Point(start1.X + parent.ActualWidth, start1.Y + parent.ActualHeight),
                            new Point(start1.X + parent.ActualWidth, start1.Y + k.ActualHeight),
                            new Point(start0.X + k.ActualWidth, start0.Y + k.ActualHeight),
                            new Point(start0.X + k.ActualWidth, start0.Y),
                            start0,
                            new Point(start0.X, start0.Y + k.ActualHeight),
                            new Point(start1.X, start1.Y + k.ActualHeight),
                        };
                        break;
                }
            }
        }

        //...

        public IEnumerable<DockAnchorPanelCollection> AllPins()
        {
            yield return BottomPanels; yield return LeftPanels; yield return RightPanels; yield return TopPanels;
        }

        //...

        public void EachPin(Predicate<DockAnchorPanelCollection> action)
        {
            foreach (var i in AllPins())
            {
                if (!action(i))
                    break;
            }
        }

        public void EachPin(Predicate<Models.Panel> action)
        {
            foreach (var i in AllPins())
            {
                foreach (var j in i)
                {
                    if (!action(j))
                        break;
                }
            }
        }

        //...

        public void Select(double x, double y, double height, double width)
        {
            SelectionPoints = new System.Windows.Media.PointCollection()
            {
                new Point(x, y),
                new Point(x, y + height),
                new Point(x + width, y + height),
                new Point(x + width, y)
            };
        }

        #endregion

        #region Commands

        ICommand previewCommand;
        public ICommand PreviewCommand 
            => previewCommand ??= new RelayCommand<DockPanelButton>(Preview);
        
        ICommand selectCommand;
        public ICommand SelectCommand 
            => selectCommand ??= new RelayCommand<FrameworkElement>(Select);

        ICommand unselectCommand;
        public ICommand UnselectCommand 
            => unselectCommand ??= new RelayCommand(() => Select(0, 0, 0, 0), () => true);

        #endregion
    }
}