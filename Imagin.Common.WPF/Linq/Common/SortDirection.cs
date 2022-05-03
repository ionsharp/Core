using Imagin.Common.Data;
using System;
using System.ComponentModel;

namespace Imagin.Common.Linq
{
    public static class XSortDirection
    {
        public static ListSortDirection Convert(this SortDirection input)
        {
            Enum.TryParse($"{input}", out ListSortDirection result);
            return result;
        }

        public static SortDirection Convert(this ListSortDirection input)
        {
            Enum.TryParse($"{input}", out SortDirection result);
            return result;
        }
    }
}