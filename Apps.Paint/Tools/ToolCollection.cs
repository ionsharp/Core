using Imagin.Common.Collections.Generic;
using System;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public class ToolCollection : ObservableCollection<ToolGroup>
    {
        public void ForAll(Action<Tool> action)
        {
            foreach (var i in this)
            {
                foreach (var j in i.Tools)
                    action(j);
            }
        }

        public T Get<T>() where T: Tool
        {
            foreach (var i in this)
            {
                foreach (var j in i.Tools)
                {
                    if (j is T k)
                        return k;
                }
            }
            return default;
        }
    }
}