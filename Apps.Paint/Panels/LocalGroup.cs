using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public abstract class LocalGroupPanel<T> : GroupPanel<T> where T : new()
    {
        double previewSize = 64.0;
        [Featured(AboveBelow.Below)]
        [Format(RangeFormat.Slider)]
        [Label(false)]
        [Range(32.0, 512.0, 4.0)]
        [Tool]
        [Visible]
        public double PreviewSize
        {
            get => previewSize;
            set => this.Change(ref previewSize, value);
        }

        GroupView view = GroupView.Grid;
        [Featured(AboveBelow.Below), Label(false)]
        [Tool, Visible]
        public GroupView View
        {
            get => view;
            set => this.Change(ref view, value);
        }

        public LocalGroupPanel(GroupWriter<T> groups) : base()
        {
            Groups = groups;
            foreach (var i in GetDefault())
            {
                if (!Groups.Contains(j => j.GetType() == i.GetType()))
                    Groups.Insert(0, i);
            }
            if (SelectedGroupIndex == -1)
                SelectedGroupIndex = 0;
        }

        protected abstract IEnumerable<GroupCollection<T>> GetDefault();
    }
}