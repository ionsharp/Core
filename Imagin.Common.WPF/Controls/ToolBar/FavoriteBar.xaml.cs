using Imagin.Common.Input;
using Imagin.Common.Storage;
using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public partial class FavoriteBar : ToolBar
    {
        public event EventHandler<EventArgs<string>> Clicked;

        public FavoriteBar() => InitializeComponent();

        protected override void OnItemsSourceChanged(IEnumerable oldValue, IEnumerable newValue)
        {
            base.OnItemsSourceChanged(oldValue, newValue);
            if (newValue is not null)
            {
                if (newValue is not Favorites)
                {
                    if (newValue is ListCollectionView view)
                    {
                        if (view.SourceCollection is Favorites)
                            return;
                    }
                    throw new NotSupportedException($"'{newValue.GetType()}' is not a valid value for '{nameof(FavoriteBar)}.{nameof(ItemsSource)}' ({typeof(Favorites)}).");
                }
            }
        }

        protected void OnClicked(string folderPath) => Clicked?.Invoke(this, new EventArgs<string>(folderPath));

        ICommand clickCommand;
        public ICommand ClickCommand => clickCommand ??= new RelayCommand<string>(i => OnClicked(i), i => true);
    }
}