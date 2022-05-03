using Imagin.Common.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Imagin.Common.Storage
{
    public sealed class Filter
    {
        public static Filter Default = new(ItemType.All);

        public readonly IEnumerable<string> Extensions;

        public readonly ItemType Types;

        public Filter(ItemType types = ItemType.All, params string[] extensions)
        {
            Types = types;
            Extensions = extensions?.Length > 0 ? extensions.Select(i => i.ToLower()) : null;
        }

        public bool Evaluate(string path, ItemType type)
        {
            if (type == ItemType.File)
            {
                if (Extensions == null)
                    return true;

                var fileExtension = Path.GetExtension(path).TrimExtension();
                //Arbitrarily exclude files with no extensions when at least one extension is specified
                if (Extensions.Any())
                {
                    if (fileExtension.TrimWhitespace().NullOrEmpty())
                        return false;
                }

                return Extensions.Contains(fileExtension) && Types.HasFlag(ItemType.File);
            }
            return Types.HasFlag(type);
        }
    }
}