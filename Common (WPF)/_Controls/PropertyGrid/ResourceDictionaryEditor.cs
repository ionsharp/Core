using Imagin.Common.Globalization;
using System.Threading.Tasks;
using System.Windows;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceDictionaryEditor : PropertyGridBase<ResourceDictionary>
    {
        /// <summary>
        /// 
        /// </summary>
        public ResourceDictionaryEditor() : base()
        {
            SetCurrentValue(NameColumnHeaderProperty, Localizer.GetValue<string>("Key", "Imagin.Common.WPF"));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected override void FollowSource(ResourceDictionary source)
        {
            //Do nothing!
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
        protected override void IgnoreSource(ResourceDictionary source)
        {
            //Do nothing!
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected override void Nest(object source)
        {
            //Do nothing!
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void RewindNest()
        {
            //Do nothing!
        }

        /// <summary>
        /// Loads a collection of properties served by the given <see cref="ResourceDictionary"/>.
        /// </summary>
        /// <param name="source">The source in which properties are served.</param>
        public override async Task LoadPropertiesAsync(ResourceDictionary source)
        {
            await Properties.LoadAsync(source);
        }
    }
}
