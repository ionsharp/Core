using System;

namespace Imagin.Common.Extensions
{
    public static class SpecialFolderExtensions
    {
        public static string GetPath(this Environment.SpecialFolder Value)
        {
            return Environment.GetFolderPath(Value);
        }
    }
}
