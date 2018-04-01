using Imagin.Common.Collections.Generic;
using Imagin.Colour.Controls.Models;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Imagin.Colour.Controls.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ColorModelCollection : TCollection<ColorModel>
    {
        /// <summary>
        /// 
        /// </summary>
        public event SelectedEventHandler Selected;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        protected override void OnItemAdded(ColorModel Item)
        {
            base.OnItemAdded(Item);
            Item.Selected -= OnSelected;
            Item.Selected += OnSelected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Item"></param>
        protected override void OnItemRemoved(ColorModel Item)
        {
            base.OnItemRemoved(Item);
            Item.Selected -= OnSelected;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void OnItemsCleared()
        {
            base.OnItemsCleared();
            this.ForEach(i => i.Selected -= OnSelected);
        }

        void OnSelected(object sender, SelectedEventArgs e) => Selected?.Invoke(this, new SelectedEventArgs(sender, e.Value));
    }
}
