using System.Windows;

namespace Imagin.Common
{
    public interface IFrameworkReference
    {
        void SetReference(IFrameworkKey key, FrameworkElement element);
    }
}