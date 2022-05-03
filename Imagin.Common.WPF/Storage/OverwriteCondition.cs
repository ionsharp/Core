using System;

namespace Imagin.Common.Storage
{
    [Serializable]
    public enum OverwriteCondition
    {
        IfNewer,
        IfSizeDifferent,
        IfNewerOrSizeDifferent,
        Always
    }
}