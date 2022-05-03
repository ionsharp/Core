using System.Collections.Generic;

namespace Imagin.Common.Configuration
{
    public interface ISingleApplication
    {
        void OnReopened(IList<string> Arguments);
    }
}