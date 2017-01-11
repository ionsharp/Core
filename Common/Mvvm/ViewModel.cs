namespace Imagin.Common.Mvvm
{
    /// <summary>
    /// Base implementatation for defining view models.
    /// </summary>
    public class ViewModel : ViewModel<object>
    {
        /// <summary>
        /// Initializes a new instance of the ViewModel class.
        /// </summary>
        public ViewModel() : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ViewModel class.
        /// </summary>
        /// <param name="Object">The object to model.</param>
        public ViewModel(object Object) : base(Object)
        {
        }
    }
}
