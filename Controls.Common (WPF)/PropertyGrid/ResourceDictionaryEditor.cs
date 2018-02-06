using System.Threading.Tasks;
using System.Windows;

namespace Imagin.Controls.Common
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected override void FollowSource(ResourceDictionary source)
        {
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        protected override void Nest(object source)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void RewindNest()
        {
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
