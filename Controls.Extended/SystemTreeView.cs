using Imagin.Common;
using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Extensions;
using Imagin.Controls.Common;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Controls.Extended
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckableSystemObject : TriCheckableObject
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Collapsed;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Expanded;

        bool IsCheckedChangeHandled = false;

        CheckableSystemObject Parent { get; set; } = default(CheckableSystemObject);

        ISystemProvider SystemProvider { get; set; } = default(ISystemProvider);

        ConcurrentCollection<CheckableSystemObject> children = new ConcurrentCollection<CheckableSystemObject>();
        /// <summary>
        /// 
        /// </summary>
        public ConcurrentCollection<CheckableSystemObject> Children
        {
            get
            {
                return children;
            }
            private set
            {
                SetValue(ref children, value, "Children");
            }
        }

        bool isExpanded = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                if (SetValue(ref isExpanded, value, "IsExpanded"))
                {
                    if (value)
                    {
                        OnExpanded();
                    }
                    else OnCollapsed();
                }
            }
        }

        bool isSelected = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                SetValue(ref isSelected, value, "IsSelected");
            }
        }

        string path = string.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                SetValue(ref path, value, "Path");
            }
        }

        bool queryOnExpanded = false;
        /// <summary>
        /// 
        /// </summary>
        public bool QueryOnExpanded
        {
            get
            {
                return queryOnExpanded;
            }
            set
            {
                SetValue(ref queryOnExpanded, value);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override bool? IsChecked
        {
            get
            {
                return isChecked;
            }
            set
            {
                if (SetValue(ref isChecked, value, "IsChecked") && value != null)
                {
                    if (value.Value)
                    {
                        OnChecked();
                    }
                    else OnUnchecked();
                }
            }
        }

        #endregion

        #region CheckableSystemObject

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="systemProvider"></param>
        /// <param name="isChecked"></param>
        public CheckableSystemObject(string path, ISystemProvider systemProvider, bool? isChecked = false) : base()
        {
            Path = path;
            SystemProvider = systemProvider;
            IsChecked = isChecked;
        }

        #endregion

        #region Methods

        void Determine()
        {
            //If it has a parent, determine it's state by enumerating all children, but current instance, which is already accounted for.
            if (Parent != null)
            {
                IsCheckedChangeHandled = true;
                var p = Parent;
                while (p != null)
                {
                    p.IsChecked = Determine(p);
                    p = p.Parent;
                }
                IsCheckedChangeHandled = false;
            }
        }

        bool? Determine(CheckableSystemObject Root)
        {
            //Whether or not all children and all children's children have the same value
            var Uniform = true;

            //If uniform, the value
            var Result = default(bool?);

            var j = false;
            foreach (var i in Root.Children)
            {
                //Get first child's state
                if (j == false)
                {
                    Result = i.IsChecked;
                    j = true;
                }
                //If the previous child's state is not equal to the current child's state, it is not uniform and we are done!
                else if (Result != i.IsChecked)
                {
                    Uniform = false;
                    break;
                }
            }

            return !Uniform ? null : Result;
        }

        void Query(ISystemProvider SystemProvider)
        {
            children.Clear();
            if (SystemProvider != null)
            {
                foreach (var i in SystemProvider.Query(path))
                {
                    children.Add(new CheckableSystemObject(i, SystemProvider, isChecked)
                    {
                        Parent = this
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnChecked()
        {
            base.OnChecked();

            if (!IsCheckedChangeHandled)
            {
                //By checking the root only, all children are checked automatically
                foreach (var i in children)
                    i.IsChecked = true;

                Determine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnUnchecked()
        {
            base.OnUnchecked();

            if (!IsCheckedChangeHandled)
            {
                //By unchecking the root only, all children are unchecked automatically
                foreach (var i in children)
                    i.IsChecked = false;

                Determine();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnCollapsed()
        {
            Collapsed?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnExpanded()
        {
            Expanded?.Invoke(this, new EventArgs());

            if (queryOnExpanded)
                BeginQuery(SystemProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SystemProvider"></param>
        public async void BeginQuery(ISystemProvider SystemProvider)
        {
            await Task.Run(() => Query(SystemProvider));
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemTreeView : TreeViewExt
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty QueryOnExpandedProperty = DependencyProperty.Register("QueryOnExpanded", typeof(bool), typeof(SystemTreeView), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnQueryOnExpandedChanged));
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
            d.As<SystemTreeView>().OnQueryOnExpandedChanged((bool)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty RootProperty = DependencyProperty.Register("Root", typeof(string), typeof(SystemTreeView), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnRootChanged));
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
            d.As<SystemTreeView>().OnRootChanged((string)e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        static DependencyProperty SystemObjectsProperty = DependencyProperty.Register("SystemObjects", typeof(ConcurrentCollection<CheckableSystemObject>), typeof(SystemTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        ConcurrentCollection<CheckableSystemObject> SystemObjects
        {
            get
            {
                return (ConcurrentCollection<CheckableSystemObject>)GetValue(SystemObjectsProperty);
            }
            set
            {
                SetValue(SystemObjectsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty SystemProviderProperty = DependencyProperty.Register("SystemProvider", typeof(ISystemProvider), typeof(SystemTreeView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSystemProviderChanged));
        /// <summary>
        /// 
        /// </summary>
        public ISystemProvider SystemProvider
        {
            get
            {
                return (ISystemProvider)GetValue(SystemProviderProperty);
            }
            set
            {
                SetValue(SystemProviderProperty, value);
            }
        }
        static void OnSystemProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.As<SystemTreeView>().OnSystemProviderChanged((ISystemProvider)e.NewValue);
        }

        #endregion

        #region SystemTreeView

        /// <summary>
        /// 
        /// </summary>
        public SystemTreeView() : base()
        {
            SetCurrentValue(SystemObjectsProperty, new ConcurrentCollection<CheckableSystemObject>());
            SetCurrentValue(SystemProviderProperty, new LocalSystemProvider());

            SetBinding(ItemsSourceProperty, new Binding()
            {
                Mode = BindingMode.OneWay,
                Path = new PropertyPath("SystemObjects"),
                Source = this
            });
        }

        #endregion

        #region Methods

        void OnQueryOnExpandedChanged(CheckableSystemObject Item, bool Value)
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
        /// <param name="Value"></param>
        protected virtual void OnRootChanged(string Value)
        {
            OnSystemProvided(SystemProvider, Value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Provider"></param>
        /// <param name="Root"></param>
        protected virtual void OnSystemProvided(ISystemProvider Provider, string Root)
        {
            SystemObjects.Clear();
            if (Provider != null)
            {
                foreach (var i in Provider.Query(Root))
                {
                    SystemObjects.Add(new CheckableSystemObject(i, SystemProvider)
                    {
                        QueryOnExpanded = QueryOnExpanded
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Value"></param>
        protected virtual void OnSystemProviderChanged(ISystemProvider Value)
        {
            OnSystemProvided(Value, Root);
        }

        #endregion
    }
}
