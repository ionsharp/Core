using System;

namespace Imagin.Common.Storage
{
    public class RemoteValidator : IValidate
    {
        public virtual bool Validate(ItemType target, string path)
        {
            if (target == (ItemType.File | ItemType.Folder))
                return default;

            if (target == ItemType.File)
                return default;

            if (target == ItemType.Folder)
                return default;

            throw new InvalidOperationException();
        }
    }
}