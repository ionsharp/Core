using Imagin.Common.Storage;
using System.Runtime.CompilerServices;

namespace Imagin.Common.Controls
{
    public abstract class NavigatorGroup : Base
    {
        bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set => this.Change(ref isSelected, value);
        }
    }

    public class FavoriteGroup : NavigatorGroup
    {
        Favorites favorites = null;
        public Favorites Favorites
        {
            get => favorites;
            set => this.Change(ref favorites, value);
        }

        public FavoriteGroup(Favorites favorites) => Favorites = favorites;
    }

    public class FolderGroup : NavigatorGroup
    {
        public ItemCollection Items { get; private set; } = new ItemCollection(string.Empty, new Filter());

        string path;
        public string Path
        {
            get => path;
            set => this.Change(ref path, value);
        }

        public FolderGroup(string path) => Path = path;

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Path):
                    _ = Items.RefreshAsync(path);
                    break;
            }
        }
    }
}