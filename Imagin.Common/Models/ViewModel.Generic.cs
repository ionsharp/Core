using System;

namespace Imagin.Common.Models
{
    /// <inheritdoc/>
    [Serializable]
    public class ViewModel<T> : ViewModel
    {
        T view = default;
        public virtual T View
        {
            get => view;
            set => this.Change(ref view, value, () => View);
        }

        protected ViewModel() : base() { }

        public ViewModel(T view) : this() => View = view;
    }
}