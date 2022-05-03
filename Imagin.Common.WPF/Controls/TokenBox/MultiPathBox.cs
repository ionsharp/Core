using Imagin.Common.Converters;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public class MultiPathBox : TokenBox
    {
        #region Properties

        readonly MultiPathBoxDropHandler DropHandler;

        public static readonly IValidate DefaultValidator = new LocalValidator();

        //...

        IValidate validator => ValidateHandler ?? DefaultValidator;

        //...

        public static readonly DependencyProperty BrowseButtonTemplateProperty = DependencyProperty.Register(nameof(BrowseButtonTemplate), typeof(DataTemplate), typeof(MultiPathBox), new FrameworkPropertyMetadata(default(DataTemplate)));
        public DataTemplate BrowseButtonTemplate
        {
            get => (DataTemplate)GetValue(BrowseButtonTemplateProperty);
            set => SetValue(BrowseButtonTemplateProperty, value);
        }

        public static readonly DependencyProperty BrowseButtonToolTipProperty = DependencyProperty.Register(nameof(BrowseButtonToolTip), typeof(string), typeof(MultiPathBox), new FrameworkPropertyMetadata(default(string)));
        public string BrowseButtonToolTip
        {
            get => (string)GetValue(BrowseButtonToolTipProperty);
            set => SetValue(BrowseButtonToolTipProperty, value);
        }

        public static readonly DependencyProperty BrowseButtonVisibilityProperty = DependencyProperty.Register(nameof(BrowseButtonVisibility), typeof(Visibility), typeof(MultiPathBox), new FrameworkPropertyMetadata(Visibility.Visible));
        public Visibility BrowseButtonVisibility
        {
            get => (Visibility)GetValue(BrowseButtonVisibilityProperty);
            set => SetValue(BrowseButtonVisibilityProperty, value);
        }

        public static readonly DependencyProperty BrowseFileExtensionsProperty = DependencyProperty.Register(nameof(BrowseFileExtensions), typeof(Extensions), typeof(MultiPathBox), new FrameworkPropertyMetadata(Extensions.Empty));
        [TypeConverter(typeof(ExtensionsTypeConverter))]
        public Extensions BrowseFileExtensions
        {
            get => (Extensions)GetValue(BrowseFileExtensionsProperty);
            set => SetValue(BrowseFileExtensionsProperty, value);
        }

        public static readonly DependencyProperty BrowseFilterModeProperty = DependencyProperty.Register(nameof(BrowseFilterMode), typeof(StorageWindowFilterModes), typeof(MultiPathBox), new FrameworkPropertyMetadata(StorageWindowFilterModes.Single));
        public StorageWindowFilterModes BrowseFilterMode
        {
            get => (StorageWindowFilterModes)GetValue(BrowseFilterModeProperty);
            set => SetValue(BrowseFilterModeProperty, value);
        }

        public static readonly DependencyProperty BrowseModeProperty = DependencyProperty.Register(nameof(BrowseMode), typeof(StorageWindowModes), typeof(MultiPathBox), new FrameworkPropertyMetadata(StorageWindowModes.OpenFolder, null, OnBrowseModeCoerced));
        public StorageWindowModes BrowseMode
        {
            get => (StorageWindowModes)GetValue(BrowseModeProperty);
            set => SetValue(BrowseModeProperty, value);
        }
        static object OnBrowseModeCoerced(DependencyObject i, object input) => input is StorageWindowModes mode && mode != StorageWindowModes.SaveFile ? input : throw new NotSupportedException();

        public static readonly DependencyProperty BrowseTitleProperty = DependencyProperty.Register(nameof(BrowseTitle), typeof(string), typeof(MultiPathBox), new FrameworkPropertyMetadata(default(string)));
        public string BrowseTitle
        {
            get => (string)GetValue(BrowseTitleProperty);
            set => SetValue(BrowseTitleProperty, value);
        }

        public static readonly DependencyProperty BrowseTypeProperty = DependencyProperty.Register(nameof(BrowseType), typeof(StorageWindowTypes), typeof(MultiPathBox), new FrameworkPropertyMetadata(StorageWindowTypes.Explorer));
        public StorageWindowTypes BrowseType
        {
            get => (StorageWindowTypes)GetValue(BrowseTypeProperty);
            set => SetValue(BrowseTypeProperty, value);
        }

        public static readonly DependencyProperty CanBrowseProperty = DependencyProperty.Register(nameof(CanBrowse), typeof(bool), typeof(MultiPathBox), new FrameworkPropertyMetadata(true));
        public bool CanBrowse
        {
            get => (bool)GetValue(CanBrowseProperty);
            set => SetValue(CanBrowseProperty, value);
        }

        public static readonly DependencyProperty CanValidateProperty = DependencyProperty.Register(nameof(CanValidate), typeof(bool), typeof(MultiPathBox), new FrameworkPropertyMetadata(true));
        public bool CanValidate
        {
            get => (bool)GetValue(CanValidateProperty);
            set => SetValue(CanValidateProperty, value);
        }

        public static readonly DependencyProperty IconVisibilityProperty = DependencyProperty.Register(nameof(IconVisibility), typeof(Visibility), typeof(MultiPathBox), new FrameworkPropertyMetadata(Visibility.Visible));
        public Visibility IconVisibility
        {
            get => (Visibility)GetValue(IconVisibilityProperty);
            set => SetValue(IconVisibilityProperty, value);
        }

        public static readonly DependencyProperty IsValidProperty = DependencyProperty.Register(nameof(IsValid), typeof(bool), typeof(MultiPathBox), new FrameworkPropertyMetadata(false));
        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidProperty, value);
        }

        public static readonly DependencyProperty ValidateHandlerProperty = DependencyProperty.Register(nameof(ValidateHandler), typeof(IValidate), typeof(MultiPathBox), new FrameworkPropertyMetadata(null));
        public IValidate ValidateHandler
        {
            get => (IValidate)GetValue(ValidateHandlerProperty);
            set => SetValue(ValidateHandlerProperty, value);
        }

        public static readonly DependencyProperty ValidateTemplateProperty = DependencyProperty.Register(nameof(ValidateTemplate), typeof(DataTemplate), typeof(MultiPathBox), new FrameworkPropertyMetadata(default(DataTemplate)));
        public DataTemplate ValidateTemplate
        {
            get => (DataTemplate)GetValue(ValidateTemplateProperty);
            set => SetValue(ValidateTemplateProperty, value);
        }

        public static readonly DependencyProperty ValidateToolTipProperty = DependencyProperty.Register(nameof(ValidateToolTip), typeof(string), typeof(MultiPathBox), new FrameworkPropertyMetadata(default(string)));
        public string ValidateToolTip
        {
            get => (string)GetValue(ValidateToolTipProperty);
            set => SetValue(ValidateToolTipProperty, value);
        }

        #endregion

        #region MultiPathBox

        public MultiPathBox() : base()
        {
            DropHandler = new(this);
            GongSolutions.Wpf.DragDrop.DragDrop.SetDropHandler(this, DropHandler);
        }

        #endregion

        #region Methods

        public void Browse()
        {
            Focus();
            if (StorageWindow.Show(out string[] paths, BrowseTitle, BrowseMode, BrowseFileExtensions.Values, Source.Split(';')?.Last<string>(), BrowseFilterMode, BrowseType))
                SetCurrentValue(SourceProperty, $"{Source};{paths.ToString<string>(";")}");

            MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        ICommand browseCommand;
        public ICommand BrowseCommand => browseCommand ??= new RelayCommand(Browse, () => CanBrowse);

        #endregion
    }
}