using System.Threading.Tasks;

namespace Imagin.Common
{
    public delegate Task DialogClosedHandler(int result);

    public delegate Task<int> DialogOpenedHandler();
}