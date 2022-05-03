using System;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public enum LayerMasks : int
    {
        None = 0,
        Clip = 1,
        DeepPunch = 2,
        ShallowPunch = 3
    }
}