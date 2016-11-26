using System;

namespace Imagin.Common.Data
{
    [Serializable]
    public enum StringOperator
    {
        Contains,
        IsEqualTo,
        BeginsWith,
        EndsWith,
        MatchesRegex,
        DoesNotContain
    }
}
