using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imagin.Apps.Paint.Effects
{
    [DisplayName("Effects")]
    [Serializable]
    public class EffectCollection : ObservableCollection<ImageEffect>, ICloneable, ITypes
    {
        bool isVisible = true;
        public bool IsVisible
        {
            get => isVisible;
            set => this.Change(ref isVisible, value);
        }

        public static IEnumerable<Type> Types
        {
            get
            {
                var result = new List<Type>();
                Assembly.GetEntryAssembly().GetDerivedTypes(typeof(ImageEffect), App.DefaultName + $".{nameof(Effects)}", true, true).ForEach(i => result.Add(i));
                return result;
            }
        }

        public EffectCollection() : base() { }

        public EffectCollection(IEnumerable<ImageEffect> input) : base(input) { }

        public IEnumerable<Type> GetTypes() => Types;

        object ICloneable.Clone() => Clone();
        public EffectCollection Clone()
        {
            var result = new EffectCollection();
            this.ForEach(i => result.Add(i.Copy()));
            return result;
        }
    }
}