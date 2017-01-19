using Imagin.Common.Collections.ObjectModel;
using Imagin.Common.Input;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Imagin.Common.Extensions;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicWindow : WindowBase
    {
        #region Properties

        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(FrameworkElementCollection), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public FrameworkElementCollection Buttons
        {
            get
            {
                return (FrameworkElementCollection)GetValue(ButtonsProperty);
            }
            set
            {
                SetValue(ButtonsProperty, value);
            }
        }

        public static readonly DependencyProperty ButtonsPanelProperty = DependencyProperty.Register("ButtonsPanel", typeof(ItemsPanelTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public ItemsPanelTemplate ButtonsPanel
        {
            get
            {
                return (ItemsPanelTemplate)GetValue(ButtonsPanelProperty);
            }
            set
            {
                SetValue(ButtonsPanelProperty, value);
            }
        }

        public static readonly DependencyProperty ButtonsStyleProperty = DependencyProperty.Register("ButtonsStyle", typeof(Style), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public Style ButtonsStyle
        {
            get
            {
                return (Style)GetValue(ButtonsStyleProperty);
            }
            set
            {
                SetValue(ButtonsStyleProperty, value);
            }
        }
        
        public static DependencyProperty ContentBorderBrushProperty = DependencyProperty.Register("ContentBorderBrush", typeof(Brush), typeof(BasicWindow), new FrameworkPropertyMetadata(default(Brush), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the border brush of the content.
        /// </summary>
        public Brush ContentBorderBrush
        {
            get
            {
                return (Brush)GetValue(ContentBorderBrushProperty);
            }
            set
            {
                SetValue(ContentBorderBrushProperty, value);
            }
        }

        public static DependencyProperty ContentBorderThicknessProperty = DependencyProperty.Register("ContentBorderThickness", typeof(Thickness), typeof(BasicWindow), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the border thickness of the content.
        /// </summary>
        public Thickness ContentBorderThickness
        {
            get
            {
                return (Thickness)GetValue(ContentBorderThicknessProperty);
            }
            set
            {
                SetValue(ContentBorderThicknessProperty, value);
            }
        }

        public static DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(BasicWindow), new FrameworkPropertyMetadata(default(Thickness), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets the outer margin of the content.
        /// </summary>
        public Thickness ContentMargin
        {
            get
            {
                return (Thickness)GetValue(ContentMarginProperty);
            }
            set
            {
                SetValue(ContentMarginProperty, value);
            }
        }

        public static DependencyProperty FooterProperty = DependencyProperty.Register("Footer", typeof(object), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public object Footer
        {
            get
            {
                return GetValue(FooterProperty);
            }
            set
            {
                SetValue(FooterProperty, value);
            }
        }

        public static DependencyProperty FooterTemplateProperty = DependencyProperty.Register("FooterTemplate", typeof(DataTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate FooterTemplate
        {
            get
            {
                return (DataTemplate)GetValue(FooterTemplateProperty);
            }
            set
            {
                SetValue(FooterTemplateProperty, value);
            }
        }

        public static DependencyProperty FooterTemplateSelectorProperty = DependencyProperty.Register("FooterTemplateSelector", typeof(DataTemplateSelector), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplateSelector FooterTemplateSelector
        {
            get
            {
                return (DataTemplateSelector)GetValue(FooterTemplateSelectorProperty);
            }
            set
            {
                SetValue(FooterTemplateSelectorProperty, value);
            }
        }

        public static DependencyProperty IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate IconTemplate
        {
            get
            {
                return (DataTemplate)GetValue(IconTemplateProperty);
            }
            set
            {
                SetValue(IconTemplateProperty, value);
            }
        }

        public static readonly DependencyProperty OverlayProperty = DependencyProperty.Register("Overlay", typeof(object), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Element to place on top of everything else; element covers entire window.
        /// </summary>
        public object Overlay
        {
            get
            {
                return GetValue(OverlayProperty);
            }
            set
            {
                SetValue(OverlayProperty, value);
            }
        }

        public static readonly DependencyProperty OverlayVisibilityProperty = DependencyProperty.Register("OverlayVisibility", typeof(Visibility), typeof(BasicWindow), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// The visibility of the overlay element.
        /// </summary>
        public Visibility OverlayVisibility
        {
            get
            {
                return (Visibility)GetValue(OverlayVisibilityProperty);
            }
            set
            {
                SetValue(OverlayVisibilityProperty, value);
            }
        }
        
        public static DependencyProperty ResizeGripTemplateProperty = DependencyProperty.Register("ResizeGripTemplate", typeof(DataTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate ResizeGripTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ResizeGripTemplateProperty);
            }
            set
            {
                SetValue(ResizeGripTemplateProperty, value);
            }
        }

        public static DependencyProperty TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public DataTemplate TitleTemplate
        {
            get
            {
                return (DataTemplate)GetValue(TitleTemplateProperty);
            }
            set
            {
                SetValue(TitleTemplateProperty, value);
            }
        }

        public static DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(WindowType), typeof(BasicWindow), new FrameworkPropertyMetadata(WindowType.Window, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public WindowType Type
        {
            get
            {
                return (WindowType)GetValue(TypeProperty);
            }
            set
            {
                SetValue(TypeProperty, value);
            }
        }

        #endregion

        #region BasicWindow

        public BasicWindow() : base()
        {
            DefaultStyleKey = typeof(BasicWindow);
            Buttons = new FrameworkElementCollection();
        }

        public BasicWindow(Action OnClosed) : this()
        {
            if (OnClosed != null)
                Closed += (s, e) => OnClosed();
        }

        #endregion
    }
}
