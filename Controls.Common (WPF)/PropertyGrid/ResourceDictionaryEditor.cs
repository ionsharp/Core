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
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override async Task GetPropertiesAsync(ResourceDictionary source)
        {
            await Properties.LoadAsync(source);
        }
    }
}
