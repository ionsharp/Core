using System;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    [Serializable]
    public abstract class ControlOptions<T> : Base, IControlOptions where T : Control
    {
        public ControlOptions() : base() { }
    }
}