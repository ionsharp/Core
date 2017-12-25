using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Input;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CheckableStorageCollection : ConcurrentCollection<CheckableStorageObject>
    {
        /// <summary>
        /// Occurs whenever an item's state changes.
        /// </summary>
        public event CheckedEventHandler ItemStateChanged;

        /// <summary>
        /// 
        /// </summary>
        public CheckableStorageCollection() : base()
        {
        }

        void OnItemStateChanged(object sender, CheckedEventArgs e)
        {
            ItemStateChanged?.Invoke(sender as CheckableStorageObject, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        protected override void OnItemAdded(CheckableStorageObject i)
        {
            base.OnItemAdded(i);
            i.StateChanged += OnItemStateChanged;
            i.Children.ItemStateChanged += OnItemStateChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        protected override void OnItemRemoved(CheckableStorageObject i)
        {
            base.OnItemRemoved(i);
            i.StateChanged -= OnItemStateChanged;
            i.Children.ItemStateChanged -= OnItemStateChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnPreviewItemsCleared()
        {
            base.OnPreviewItemsCleared();
            foreach (var i in this)
            {
                i.StateChanged -= OnItemStateChanged;
                i.Children.ItemStateChanged -= OnItemStateChanged;
            }
        }
    }
}
