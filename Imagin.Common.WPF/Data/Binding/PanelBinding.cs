using Imagin.Common.Models;
using System;
using System.Linq;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    public class PanelBinding : Binding
    {
        public Type Type { set => SetType(value); }

        public PanelBinding() : base() { }

        public PanelBinding(Type type) : this(".", type) { }

        public PanelBinding(string path, Type type) : base(path)
        {
            SetType(type);
            UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
        }

        void SetType(Type input)
        {
            Source = Get.Where<IDockViewModel>()?.Panels.FirstOrDefault(i => i.GetType().Equals(input));
        }
    }
}