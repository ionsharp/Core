using Imagin.Common.Linq;
using Imagin.Common.Storage;
using Imagin.Common.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    [Serializable]
    public enum ThumbnailView
    {
        Default,
        Preview
    }

    public class Thumbnail : Control
    {
        readonly CancelTask loadTask;

        static readonly DependencyPropertyKey IsLoadingKey = DependencyProperty.RegisterReadOnly(nameof(IsLoading), typeof(bool), typeof(Thumbnail), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsLoadingProperty = IsLoadingKey.DependencyProperty;
        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            private set => SetValue(IsLoadingKey, value);
        }

        static readonly DependencyPropertyKey IsSourceGifKey = DependencyProperty.RegisterReadOnly(nameof(IsSourceGif), typeof(bool), typeof(Thumbnail), new FrameworkPropertyMetadata(false));
        public static readonly DependencyProperty IsSourceGifProperty = IsSourceGifKey.DependencyProperty;
        public bool IsSourceGif
        {
            get => (bool)GetValue(IsSourceGifProperty);
            private set => SetValue(IsSourceGifKey, value);
        }

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register(nameof(Path), typeof(string), typeof(Thumbnail), new FrameworkPropertyMetadata(string.Empty, OnPathChanged));
        public string Path
        {
            get => (string)GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        static void OnPathChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => (sender as Thumbnail).OnPathChanged(new(e));

        static readonly DependencyPropertyKey PathTypeKey = DependencyProperty.RegisterReadOnly(nameof(PathType), typeof(ItemType), typeof(Thumbnail), new FrameworkPropertyMetadata(ItemType.Folder));
        public static readonly DependencyProperty PathTypeProperty = PathTypeKey.DependencyProperty;
        public ItemType PathType
        {
            get => (ItemType)GetValue(PathTypeProperty);
            private set => SetValue(PathTypeKey, value);
        }

        static readonly DependencyPropertyKey SourceKey = DependencyProperty.RegisterReadOnly(nameof(Source), typeof(ImageSource), typeof(Thumbnail), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty SourceProperty = SourceKey.DependencyProperty;
        public ImageSource Source
        {
            get => (ImageSource)GetValue(SourceProperty);
            private set => SetValue(SourceKey, value);
        }

        public static readonly DependencyProperty ViewProperty = DependencyProperty.Register(nameof(View), typeof(ThumbnailView), typeof(Thumbnail), new FrameworkPropertyMetadata(ThumbnailView.Preview, OnViewChanged));
        public ThumbnailView View
        {
            get => (ThumbnailView)GetValue(ViewProperty);
            private set => SetValue(ViewProperty, value);
        }
        static void OnViewChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e) => (sender as Thumbnail).OnViewChanged(new(e));

        public Thumbnail() : base() 
        {
            loadTask = new(Load, true);
            this.RegisterHandler(null, i => loadTask.Cancel());
        }

        async Task Load(CancellationToken token)
        {
            var path = Path; var type = Computer.GetType(path);
            PathType = type;

            switch (View)
            {
                case ThumbnailView.Default: break;
                case ThumbnailView.Preview:
                    IsLoading = true;

                    ImageSource result = null;
                    if (System.IO.Path.GetExtension(path) == ".gif")
                    {
                        IsSourceGif = true;
                        await Task.Run(() => Try.Invoke(() => result = (ImageSource)new ImageSourceConverter().ConvertFromString(path)));
                    }
                    else
                    {
                        IsSourceGif = false;
                        await Task.Run(() =>
                        {
                            Try.Invoke(() =>
                            {
                                result = type switch
                                {
                                    ItemType.File or ItemType.Shortcut => File.Long.Thumbnail(path) ?? Computer.Icon.GetLarge(path),
                                    ItemType.Root => Computer.Icon.GetLarge(string.Empty),
                                    _ => Computer.Icon.GetLarge(path),
                                };
                            });
                        });
                    }
                    Source = result;

                    IsLoading = false;
                    break;
            }
        }
        
        protected virtual void OnPathChanged(Value<string> input) => _ = loadTask.Start();

        protected virtual void OnViewChanged(Value<ThumbnailView> input) => _ = loadTask.Start();
    }
}