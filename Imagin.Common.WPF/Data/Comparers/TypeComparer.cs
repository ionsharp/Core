using Imagin.Common.Linq;
using System;
using System.Collections;

namespace Imagin.Common.Data
{
    public class TypeComparer : IComparer
    {
        public static readonly TypeComparer Default = new();

        public TypeComparer() : base() { }

        int IComparer.Compare(object a, object b)
        {
            if (a is Type i)
            {
                if (b is Type j)
                {
                    int result;

                    var aCategory = i.GetAttribute<CategoryAttribute>()?.Category ?? "General";
                    var bCategory = j.GetAttribute<CategoryAttribute>()?.Category ?? "General";

                    result = aCategory?.CompareTo(bCategory) ?? 0;
                    if (result != 0) return result;

                    var aName = i.GetAttribute<DisplayNameAttribute>()?.DisplayName ?? i.Name;
                    var bName = j.GetAttribute<DisplayNameAttribute>()?.DisplayName ?? j.Name;

                    result = aName?.CompareTo(bName) ?? 0;
                    if (result != 0) return result;
                }
            }
            return 0;
        }
    }
}