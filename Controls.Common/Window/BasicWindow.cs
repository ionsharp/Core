using Imagin.Common.Collections;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class BasicWindow : WindowBase
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ButtonsProperty = DependencyProperty.Register("Buttons", typeof(FrameworkElementCollection), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ButtonsPanelProperty = DependencyProperty.Register("ButtonsPanel", typeof(ItemsPanelTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ButtonStyleProperty = DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Style ButtonStyle
        {
            get
            {
                return (Style)GetValue(ButtonStyleProperty);
            }
            set
            {
                SetValue(ButtonStyleProperty, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FooterProperty = DependencyProperty.Register("Footer", typeof(WindowFooter), typeof(BasicWindow), new FrameworkPropertyMetadata(default(WindowFooter), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public WindowFooter Footer
        {
            get
            {
                return (WindowFooter)GetValue(FooterProperty);
            }
            set
            {
                SetValue(FooterProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty IconTemplateProperty = DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
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
        
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ResizeGripTemplateProperty = DependencyProperty.Register("ResizeGripTemplate", typeof(DataTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TitleTemplateProperty = DependencyProperty.Register("TitleTemplate", typeof(DataTemplate), typeof(BasicWindow), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty TypeProperty = DependencyProperty.Register("Type", typeof(WindowType), typeof(BasicWindow), new FrameworkPropertyMetadata(WindowType.Window, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
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

        /// <summary>
        /// 
        /// </summary>
        public BasicWindow() : base()
        {
            DefaultStyleKey = typeof(BasicWindow);
            Buttons = new FrameworkElementCollection();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OnClosed"></param>
        public BasicWindow(Action OnClosed) : this()
        {
            if (OnClosed != null)
                Closed += (s, e) => OnClosed();
        }

        #endregion
    }
}
