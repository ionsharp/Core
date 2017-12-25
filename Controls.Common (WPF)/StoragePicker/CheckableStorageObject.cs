using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.IO;
using Imagin.Common.Linq;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckableStorageObject : CheckableObject, IExpandable, ISelectable
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

        /// <summary>
        /// 
        /// </summary>
        public event SelectedEventHandler Selected;

        bool StateChangeHandled = false;

        CheckableStorageObject Parent { get; set; } = default(CheckableStorageObject);

        ISystemObjectProvider SystemObjectProvider { get; set; } = default(ISystemObjectProvider);

        CheckableStorageCollection children = new CheckableStorageCollection();
        /// <summary>
        /// 
        /// </summary>
        public CheckableStorageCollection Children
        {
            get
            {
                return children;
            }
            private set
            {
                SetValue(ref children, value, () => Children);
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
                if (SetValue(ref isExpanded, value, () => IsExpanded))
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
                if (SetValue(ref isSelected, value, () => IsSelected) && value)
                    OnSelected(new SelectedEventArgs(null));
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
                SetValue(ref path, value, () => Path);
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
                SetValue(ref queryOnExpanded, value, () => QueryOnExpanded);
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
                if (SetValue(ref isChecked, value, () => IsChecked) && value != null)
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

        #region CheckableStorageObject

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="systemObjectProvider"></param>
        /// <param name="isChecked"></param>
        public CheckableStorageObject(string path, ISystemObjectProvider systemObjectProvider, bool? isChecked = false) : base()
        {
            Path = path;
            SystemObjectProvider = systemObjectProvider;
            IsChecked = isChecked;
        }

        #endregion

        #region Methods

        void Determine()
        {
            //If it has a parent, determine it's state by enumerating all children, but current instance, which is already accounted for.
            if (Parent != null)
            {
                StateChangeHandled = true;
                var p = Parent;
                while (p != null)
                {
                    p.IsChecked = Determine(p);
                    p = p.Parent;
                }
                StateChangeHandled = false;
            }
        }

        bool? Determine(CheckableStorageObject Root)
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

        void Query(ISystemObjectProvider SystemProvider)
        {
            children.Clear();
            if (SystemProvider != null)
            {
                foreach (var i in SystemProvider.Query(path))
                {
                    children.Add(new CheckableStorageObject(i, SystemProvider, isChecked)
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

            if (!StateChangeHandled)
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
        /// <param name="State"></param>
        protected override void OnStateChanged(bool? State)
        {
            base.OnStateChanged(State);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnUnchecked()
        {
            base.OnUnchecked();

            if (!StateChangeHandled)
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

            if (!children.Any<CheckableStorageObject>() || queryOnExpanded)
                BeginQuery(SystemObjectProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSelected(SelectedEventArgs e)
        {
            Selected?.Invoke(this, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SystemProvider"></param>
        public async void BeginQuery(ISystemObjectProvider SystemProvider)
        {
            await Task.Run(() => Query(SystemProvider));
        }

        #endregion
    }
}
