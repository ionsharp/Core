using System.Windows.Controls;
using System.Windows.Data;

namespace Imagin.Common.Controls
{
    public class TreeViewCell : ContentControl
    {
        public Binding Binding { get; set; }

        public TreeViewColumn Column { get; private set; }

        public TreeViewCell(TreeViewColumn column) : base() => Column = column;
    }
}