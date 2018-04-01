using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyGrid : PropertyGridBase<object>
    {
        /// <summary>
        /// 
        /// </summary>
        public PropertyGrid() : base()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSourceLocked(object sender, EventArgs e)
        {
            OnSourceLocked();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSourceUnlocked(object sender, EventArgs e)
        {
            OnSourceUnlocked();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnSourcePropertyChanged(e.PropertyName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSourceDeleted(object sender, EventArgs e)
        {
            OnSourceDeleted((IList)sender);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected override void FollowSource(object source)
        {
            if (source is IDeletable)
                source.As<IDeletable>().Deleted += OnSourceDeleted;

            if (source is INotifyPropertyChanged)
                source.As<INotifyPropertyChanged>().PropertyChanged += OnSourcePropertyChanged;

            if (source is ILockable)
            {
                SetCurrentValue(IsSourceEnabledProperty, !source.As<ILockable>().IsLocked);

                source.As<ILockable>().Locked += OnSourceLocked;
                source.As<ILockable>().Unlocked += OnSourceUnlocked;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected override void IgnoreSource(object source)
        {
            SetCurrentValue(IsSourceEnabledProperty, true);

            if (source is IDeletable)
                source.As<IDeletable>().Deleted -= OnSourceDeleted;

            if (source is INotifyPropertyChanged)
                source.As<INotifyPropertyChanged>().PropertyChanged -= OnSourcePropertyChanged;

            if (source is ILockable)
            {
                source.As<ILockable>().Locked -= OnSourceLocked;
                source.As<ILockable>().Unlocked -= OnSourceUnlocked;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override object GetSource()
        {
            return Source;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected override void Nest(object source)
        {
            nest.Push(Source);
            isNesting = true;
            SetCurrentValue(SourceProperty.Property, source);
            isNesting = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void RewindNest()
        {
            isNesting = true;
            SetCurrentValue(SourceProperty.Property, nest.Pop());
            isNesting = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSourceLocked()
        {
            SetCurrentValue(IsSourceEnabledProperty, false);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSourceUnlocked()
        {
            SetCurrentValue(IsSourceEnabledProperty, true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void OnSourcePropertyChanged(string propertyName)
        {
            foreach (var i in Properties)
            {
                if (i.Name == propertyName && !i.HostPropertyChangeHandled)
                {
                    i.ValueChangeHandled = true;
                    i.RefreshValue();
                    i.ValueChangeHandled = false;
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected virtual void OnSourceDeleted(object source)
        {
            IgnoreSource(source);
            Properties.Reset(null);
        }

        /// <summary>
        /// Loads a collection of properties served by the given <see cref="object"/>.
        /// </summary>
        /// <param name="source">The source in which properties are served.</param>
        public override async Task LoadPropertiesAsync(object source)
        {
            await Properties.LoadAsync(source);
        }
    }
}
