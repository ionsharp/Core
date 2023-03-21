using System.Windows.Input;

namespace Imagin.Core.Linq;

public static class XCommand
{
    public static void Execute(this ICommand input) => input.Execute(null);
}