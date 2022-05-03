using Imagin.Common.Collections;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Linq;
using System;
using System.Linq;

namespace Imagin.Common.Storage
{
    [Serializable]
    public class Favorites : XmlWriter<Favorite>
    {
        public void Is(bool @is, string folderPath)
        {
            var found = this.FirstOrDefault(i => i.Path == folderPath);
            if (!@is)
            {
                found.If(i => i != null, i => Remove(i));
            }
            else if (found == null)
                Add(new Favorite(folderPath));
        }

        public Favorites(string folderPath, Limit limit = default) : base(nameof(Favorites), folderPath, nameof(Favorites), "xml", "favorite", limit, typeof(Favorite)) { }
    }
}