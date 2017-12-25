using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Input;
using Imagin.Common.IO;
using Imagin.Common.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class StoragePicker : TreeView
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        static DependencyProperty SystemObjectsProperty = DependencyProperty.Register("SystemObjects", typeof(CheckableStorageCollection), typeof(StoragePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        CheckableStorageCollection SystemObjects
        {
            get
            {
                return (CheckableStorageCollection)GetValue(SystemObjectsProperty);
            }
            set
            {
                SetValue(SystemObjectsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty CheckedPathsProperty = DependencyProperty.Register("CheckedPaths", typeof(IList<string>), typeof(StoragePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public IList<string> CheckedPaths
        {
            get
            {
                return (IList<string>)GetValue(CheckedPathsProperty);
            }
            private set
            {
                SetValue(CheckedPathsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FileStyleProperty = DependencyProperty.Register("FileStyle", typeof(Style), typeof(StoragePicker), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Style FileStyle
        {
            get
            {
                return (Style)GetValue(FileStyleProperty);
            }
            private set
            {
                SetValue(FileStyleProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FileTemplateProperty = DependencyProperty.Register("FileTemplate", typeof(DataTemplate), typeof(StoragePicker), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate FileTemplate
        {
            get
            {
                return (DataTemplate)GetValue(FileTemplateProperty);
            }
            private set
            {
                SetValue(FileTemplateProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FolderStyleProperty = DependencyProperty.Register("FolderStyle", typeof(Style), typeof(StoragePicker), new FrameworkPropertyMetadata(default(Style), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public Style FolderStyle
        {
            get
            {
                return (Style)GetValue(FolderStyleProperty);
            }
            private set
            {
                SetValue(FolderStyleProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FolderTemplateProperty = DependencyProperty.Register("FolderTemplate", typeof(DataTemplate), typeof(StoragePicker), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate FolderTemplate
        {
            get
            {
                return (DataTemplate)GetValue(FolderTemplateProperty);
            }
            private set
            {
                SetValue(FolderTemplateProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty QueryOnExpandedProperty = DependencyProperty.Register("QueryOnExpanded", typeof(bool), typeof(StoragePicker), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnQueryOnExpandedChanged));
        /// <summary>
        /// 
        /// </summary>
        public bool QueryOnExpanded
        {
            get
            {
                return (bool)GetValue(QueryOnExpandedProperty);
            }
            set
            {
                SetValue(QueryOnExpandedProperty, value);
            }
        }
        static void OnQueryOnExpandedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<StoragePicker>().OnQueryOnExpandedChanged((bool)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty RootProperty = DependencyProperty.Register("Root", typeof(string), typeof(StoragePicker), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRootChanged));
        /// <summary>
        /// 
        /// </summary>
        public string Root
        {
            get
            {
                return (string)GetValue(RootProperty);
            }
            set
            {
                SetValue(RootProperty, value);
            }
        }
        static void OnRootChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<StoragePicker>().OnRootChanged((string)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SystemObjectProviderProperty = DependencyProperty.Register("SystemObjectProvider", typeof(ISystemObjectProvider), typeof(StoragePicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSystemObjectProviderChanged));
        /// <summary>
        /// 
        /// </summary>
        public ISystemObjectProvider SystemObjectProvider
        {
            get
            {
                return (ISystemObjectProvider)GetValue(SystemObjectProviderProperty);
            }
            set
            {
                SetValue(SystemObjectProviderProperty, value);
            }
        }
        static void OnSystemObjectProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<StoragePicker>().OnSystemObjectProviderChanged((ISystemObjectProvider)e.NewValue);
        }

        #endregion

        #region StoragePicker

        /// <summary>
        /// 
        /// </summary>
        public StoragePicker() : base()
        {
            SetCurrentValue(CheckedPathsProperty, new ConcurrentCollection<string>());
            SetCurrentValue(SystemObjectsProperty, new CheckableStorageCollection());
            SetCurrentValue(SystemObjectProviderProperty, new LocalSystemObjectProvider());

            SetBinding(ItemsSourceProperty, new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("SystemObjects"),
                Source = this
            });

            SystemObjects.ItemStateChanged += (s, e) => OnSystemObjectStateChanged(s, e);
        }

        #endregion

        #region Methods

        void OnQueryOnExpandedChanged(CheckableStorageObject Item, bool Value)
        {
            foreach (var i in Item.Children)
            {
                i.QueryOnExpanded = Value;
                OnQueryOnExpandedChanged(i, Value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnQueryOnExpandedChanged(bool Value)
        {
            foreach (var i in SystemObjects)
                OnQueryOnExpandedChanged(i, Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Provider"></param>
        /// <param name="Root"></param>
        protected virtual void OnRefreshed(ISystemObjectProvider Provider, string Root)
        {
            SystemObjects.Clear();
            if (Provider != null)
            {
                foreach (var i in Provider.Query(Root))
                {
                    var j = new CheckableStorageObject(i, SystemObjectProvider)
                    {
                        QueryOnExpanded = QueryOnExpanded
                    };
                    SystemObjects.Add(j);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnRootChanged(string Value)
        {
            OnRefreshed(SystemObjectProvider, Value);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSystemObjectProviderChanged(ISystemObjectProvider Value)
        {
            OnRefreshed(Value, Root);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnSystemObjectStateChanged(object sender, CheckedEventArgs e)
        {
            var i = (sender as CheckableStorageObject);
            switch (e.State)
            {
                case true:
                    CheckedPaths.Add(i.Path);
                    break;
                case false:
                case null:
                    CheckedPaths.Remove(i.Path);
                    break;
            }
        }

        #endregion
    }
}
