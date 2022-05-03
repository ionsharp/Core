using System;
using System.Collections;

namespace Imagin.Common.Controls
{
    public class MemberSortComparer : IComparer
    {
        readonly PropertyGrid Control;

        public MemberSortComparer(PropertyGrid control) : base() => Control = control;

        int IComparer.Compare(object a, object b)
        {
            if (a is MemberModel i)
            {
                if (b is MemberModel j)
                {
                    int result = 0;
                    switch (Control.GroupName)
                    {
                        case MemberGroupName.Category:
                            result = i.Category?.CompareTo(j.Category) ?? 0;
                            if (result != 0) return result;
                            break;

                        case MemberGroupName.DeclaringType:
                            result = i.DeclaringType?.FullName.CompareTo(j.DeclaringType?.FullName) ?? 0;
                            if (result != 0) return result;
                            break;

                        case MemberGroupName.DisplayName:
                            result = i.DisplayName?.CompareTo(j.DisplayName) ?? 0;
                            if (result != 0) return result;
                            break;

                        case MemberGroupName.Type:
                            result = i.Type?.FullName.CompareTo(j.Type?.FullName) ?? 0;
                            if (result != 0) return result;
                            break;
                    }

                    result = i.Index.CompareTo(j.Index);
                    if (result != 0) return result;

                    switch (Control.SortName)
                    {
                        case MemberSortName.DisplayName:
                            result = i.DisplayName?.CompareTo(j.DisplayName) ?? 0;
                            if (result != 0) return result;
                            break;

                        case MemberSortName.Type:
                            result = i.Type?.FullName.CompareTo(j.Type?.FullName) ?? 0;
                            if (result != 0) return result;
                            break;
                    }
                }
            }
            return 0;
        }
    }
}