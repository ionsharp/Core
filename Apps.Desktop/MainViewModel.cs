using Imagin.Common;
using Imagin.Common.Analytics;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Controls;
using Imagin.Common.Converters;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Media;
using Imagin.Common.Models;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Apps.Desktop
{
    public class MainViewModel : MainViewModel<MainWindow>
    {
        public static readonly ResourceKey<ContextMenu> TileMenuKey = new();

        public static readonly ResourceKey<FrameworkElement> TileStyleKey = new();

        public static readonly Common.ResourceKey TileTemplateKey = new();

        public static readonly Common.ResourceKey TileHeaderTemplateSelectorKey = new();

        public static readonly Common.ResourceKey TileContentTemplateSelectorKey = new();
        
        //...

        public static Screen DefaultScreen => new(new NoteTile()
        {
            Title = "This is a tile",
            Text = "Feel free to move me around...",
            Size = new(256, 256), Position = new(SystemParameters.FullPrimaryScreenWidth / 2d, SystemParameters.FullPrimaryScreenHeight / 2d)
        });

        //...

        bool drawing = false;
        public bool Drawing
        {
            get => drawing;
            set => this.Change(ref drawing, value);
        }

        Screen screen = null;
        public Screen Screen
        {
            get => screen;
            set
            {
                this.Change(ref screen, value);
                this.Changed(() => TaskbarItemDescription);
            }
        }

        public XmlWriter<Screen> Screens => Get.Current<Options>().Screens;

        public string TaskbarItemDescription => Screen != null && Screens != null ? $"{Screens.IndexOf(Screen) + 1} / {Screens.Count}" : string.Empty;

        //...

        public MainViewModel() : base()
        {
            Screens.CollectionChanged += OnScreensChanged;

            if (Screens.Count == 0)
                Screens.Add(DefaultScreen);

            Screen = Screens.ElementAtOrDefault(Get.Current<Options>().Screen)
                ?? Enumerable.FirstOrDefault(Screens);

            OnThemeChanged();
            Get.Current<Options>().ThemeChanged += OnThemeChanged;
        }

        //...

        void OnScreensChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (Screen == e.OldItems[0])
                        Screen = Enumerable.FirstOrDefault(Screens);
                    break;
            }
        }

        void OnThemeChanged(object sender, EventArgs<string> e) => OnThemeChanged();

        //...

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Drawing):
                    if (Drawing)
                        View.Activate();

                    break;

                case nameof(Screen):
                    Get.Current<Options>().Screen = Screen.Index - 1;
                    break;
            }
        }

        //...

        protected virtual void OnThemeChanged()
        {
            //This doesn't change automatically when the theme does...
            View?.TaskbarIcon?.ContextMenu?.UpdateDefaultStyle();
        }

        //...

        IValueConverter tileNameConverter
            => new SimpleConverter<Type, string>(i => i.GetAttribute<DisplayNameAttribute>().DisplayName);

        public void Draw(DoubleRegion selection)
        {
            var tilePosition
                = new Point2D(selection.X.NearestFactor(Get.Current<Options>().TileSnap), selection.Y.NearestFactor(Get.Current<Options>().TileSnap));
            var tileSize
                = new DoubleSize(selection.Height.NearestFactor(Get.Current<Options>().TileSnap), selection.Width.NearestFactor(Get.Current<Options>().TileSnap));

            var tileTypes 
                = XAssembly.GetAssembly(nameof(Desktop)).GetDerivedTypes(typeof(Tile), "Imagin.Apps.Desktop", true, true);

            var window = new ComboWindow("NewTile".Translate(), "Pick a tile...", new ObservableCollection<Type>(tileTypes), tileNameConverter, Common.Controls.SelectionMode.Single, Buttons.ContinueCancel);
            window.ShowDialog();

            if (XWindow.GetResult(window) == 0)
            {
                if (window.SelectedItem is Type tileType)
                {
                    var tile = Activator.CreateInstance(tileType) as Tile;
                    tile.Position
                        = tilePosition;
                    tile.Size
                        = tileSize;

                    Screen.Add(tile);

                    if (tile is ImageTile imageTile)
                    {
                        StorageWindow.Show(out string path, "Browse file or folder...", StorageWindowModes.Open, ImageFormats.Readable.Select(i => i.Extension));
                        imageTile.Path = path;
                    }
                }
                else XWindow.GetNotifications(View).Add(new Notification("NewTile".Translate(), new Warning("Nothing was selected..."), 3.Seconds()));
            }

            selection.X = selection.Y = selection.Height = selection.Width = 0;
            Drawing = false;
        }

        //...

        ICommand addScreenCommand;
        public ICommand AddScreenCommand => addScreenCommand ??= new RelayCommand(() =>
        {
            var result = new Screen();
            Screens.Add(result);
            Screen = result;
        });

        ICommand cancelCommand;
        public ICommand CancelCommand => cancelCommand ??= new RelayCommand(() => Drawing = false, () => Drawing);

        ICommand deleteScreenCommand;
        public ICommand DeleteScreenCommand => deleteScreenCommand ??= new RelayCommand<Screen>(i =>
        {
            var result = Dialog.Show("Delete screen", $"Are you sure you want to delete 'Screen {i.Index}'?", DialogImage.Warning, Buttons.YesNo);
            if (result == 0)
                Screens.Remove(i);
        },
        i => i is Screen);

        ICommand drawCommand;
        public ICommand DrawCommand => drawCommand ??= new RelayCommand(() => Drawing = true, () => !Drawing && Screen != null);

        ICommand leftScreenCommand;
        public ICommand LeftScreenCommand => leftScreenCommand ??= new RelayCommand(() =>
        {
            if (Screen != null && Screens.Any<Screen>() && Screens.IndexOf(Screen) > 0)
            {
                var index = Screens.IndexOf(Screen);
                index--;

                if (index < 0)
                    return;

                Screen = Screens[index];
            }
        }, () =>
        {
            return true;
            if (Screen != null)
            {
                var index = Screens?.IndexOf(Screen) ?? -1;
                if (index > 0 && index <= Screens.Count - 1)
                    return true;
            }
            return false;
        });

        ICommand removeTileCommand;
        public ICommand RemoveTileCommand => removeTileCommand ??= new RelayCommand<Tile>(i => Screen.Remove(i), i => i is Tile);

        ICommand rightScreenCommand;
        public ICommand RightScreenCommand => rightScreenCommand ??= new RelayCommand(() =>
        {
            if (Screen != null && Screens.Any<Screen>() && Screens.IndexOf(Screen) < Screens.Count - 1)
            {
                var index = Screens.IndexOf(Screen);
                index++;

                if (index > Screens.Count - 1)
                    return;

                Screen = Screens[index];
            }
        }, () =>
        {
            return true;
            if (Screen != null)
            {
                var index = Screens?.IndexOf(Screen) ?? -1;
                if (index >= 0 && index < Screens.Count - 1)
                    return true;
            }
            return false;
        });
        
        ICommand selectScreenCommand;
        public ICommand SelectScreenCommand 
            => selectScreenCommand ??= new RelayCommand<Screen>(i => Screen = i, i => i is Screen);
        
        ICommand showLogWindowCommand;
        public ICommand ShowLogWindowCommand 
            => showLogWindowCommand ??= new RelayCommand(() => new LogWindow(Get.Current<App>().Log, new LogPanel(Get.Current<App>().Log)).Show());

        ICommand showTileOptionsCommand;
        public ICommand ShowTileOptionsCommand 
            => showTileOptionsCommand ??= new RelayCommand<Tile>(i => PropertyWindow.ShowDialog("Tile", i), i => i != null);
    }
}