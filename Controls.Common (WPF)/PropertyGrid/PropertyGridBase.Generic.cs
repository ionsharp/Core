using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public abstract class PropertyGridBase<TSource> : PropertyGridBase, IPropertyGrid<TSource>
    {
        #region Properties

        /// <summary>
        /// Occurs when the selected object changes.
        /// </summary>
        public event EventHandler<EventArgs<TSource>> SourceChanged;

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty<TSource, PropertyGridBase<TSource>> SourceProperty = new DependencyProperty<TSource, PropertyGridBase<TSource>>("Source", new FrameworkPropertyMetadata(default(TSource), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSourceChanged));
        /// <summary>
        /// 
        /// </summary>
        public TSource Source
        {
            get
            {
                return SourceProperty.Get(this);
            }
            set
            {
                SourceProperty.Set(this, value);
            }
        }
        static async void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            await d.As<PropertyGridBase<TSource>>().OnSourceChanged((TSource)e.OldValue, (TSource)e.NewValue);
        }

        #endregion

        #region PropertyGridBase

        /// <summary>
        /// 
        /// </summary>
        public PropertyGridBase() : base()
        {
        }

        #endregion

        #region Methods

        ICommand disconnectSourceCommand;
        /// <summary>
        /// 
        /// </summary>
        public override ICommand DisconnectSourceCommand
        {
            get
            {
                disconnectSourceCommand = disconnectSourceCommand ?? new RelayCommand<object>(p => SetCurrentValue(SourceProperty.Property, default(TSource)), p => Source != null);
                return disconnectSourceCommand;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnUnloaded(RoutedEventArgs e)
        {
            base.OnUnloaded(e);
            IgnoreSource(Source);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected abstract void FollowSource(TSource source);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public abstract Task GetPropertiesAsync(TSource source);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected abstract void IgnoreSource(TSource source);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oldSource"></param>
        /// <param name="newSource"></param>
        /// <returns></returns>
        protected virtual async Task OnSourceChanged(TSource oldSource, TSource newSource)
        {
            if (!isNesting)
                nest.Clear();

            IgnoreSource(oldSource);

            if (newSource != null)
            {
                Properties.Reset(newSource);

                IsLoading = true;
                await GetPropertiesAsync(newSource);
                IsLoading = false;

                FollowSource(newSource);
            }
            else if (AcceptsNullObjects)
                Properties.Reset(null);

            SourceChanged?.Invoke(this, new EventArgs<TSource>(newSource));
        }

        #endregion
    }
}
