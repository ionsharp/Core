using System;

namespace Imagin.Common.Storage
{
    public class LocalValidator : IValidate
    {
        public virtual bool Validate(ItemType target, string path)
        {
            if (path == StoragePath.Root)
                return false;

            try
            {
                if (target == (ItemType.File | ItemType.Folder))
                    return File.Long.Exists(path) ? true : Folder.Long.Exists(path);

                if (target == ItemType.File)
                    return File.Long.Exists(path);

                if (target == ItemType.Folder)
                    return Folder.Long.Exists(path);

                throw new InvalidOperationException();
            }
            catch { return false; }
        }
    }
}