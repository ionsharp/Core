using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using System;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public class PathGroup : BaseNamable, ICloneable
    {
        [NonSerialized]
        bool isSelected = false;
        [Hidden]
        public bool IsSelected
        {
            get => isSelected;
            set => this.Change(ref isSelected, value);
        }

        ObservableCollection<Shape> paths = new();
        public ObservableCollection<Shape> Paths
        {
            get => paths;
            set => this.Change(ref paths, value);
        }

        public PathGroup() : this("") { }

        public PathGroup(string name) : base(name) { }

        object ICloneable.Clone()
        {
            var result = new PathGroup(Name);
            Paths.ForEach(i => result.Paths.Add(new() { Points = new(i.Points) }));
            return result;
        }
        public PathGroup Clone() => (this as ICloneable).Clone() as PathGroup;
    }
}