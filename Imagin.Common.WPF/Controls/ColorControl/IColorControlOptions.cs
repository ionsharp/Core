using Imagin.Common.Collections.Serialization;

namespace Imagin.Common.Controls
{
    public interface IColorControlOptions
    {
        bool AutoSaveLayout { get; }

        IGroupWriter Colors { get; }

        //...

        Layouts Layouts { get; }

        //...

        void OnLoaded(ColorControl colorPicker);
    }
}