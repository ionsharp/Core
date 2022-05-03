using ImageMagick;
using Imagin.Common;
using Imagin.Common.Analytics;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Numbers;
using Imagin.Common.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    #region Document

    [Serializable]
    public abstract class Document : Common.Models.FileDocument, ICloneable, IFrameworkReference
    {
        public static readonly ReferenceKey<Viewer> ViewerKey = new();

        enum Category { Size }

        #region Properties

        /// <summary>
        /// The default name for untitled documents.
        /// </summary>
        public const string DefaultName = "Untitled";

        /// <summary>
        /// The default file extension for templates.
        /// </summary>
        public const string TemplateExtension = "template";

        //...

        double angle = 0;
        [Hidden]
        public double Angle
        {
            get => angle;
            set => this.Change(ref angle, value);
        }

        [NonSerialized]
        double cropHeight = new();
        [Hidden]
        public double CropHeight
        {
            get => cropHeight;
            set => this.Change(ref cropHeight, value);
        }

        [NonSerialized]
        double cropWidth = new();
        [Hidden]
        public double CropWidth
        {
            get => cropWidth;
            set => this.Change(ref cropWidth, value);
        }

        [NonSerialized]
        double cropX = new();
        [Hidden]
        public double CropX
        {
            get => cropX;
            set => this.Change(ref cropX, value);
        }

        [NonSerialized]
        double cropY = new();
        [Hidden]
        public double CropY
        {
            get => cropY;
            set => this.Change(ref cropY, value);
        }

        [Hidden]
        public override string Extension => "image";

        [Hidden]
        public override string[] Extensions => ImageFormats.Writable.Select(i => i.Extension).ToArray();

        int height;
        [Category(Category.Size)]
        [ReadOnly]
        [StringFormat(NumberFormat.Default)]
        public virtual int Height
        {
            get => height;
            set
            {
                this.Change(ref height, value);
                this.Changed(() => MegaPixels);
            }
        }

        History history = new();
        [Hidden]
        public History History
        {
            get => history;
            set => this.Change(ref history, value);
        }

        LayerCollection layers;
        [Hidden]
        public LayerCollection Layers
        {
            get => layers;
            set => this.Change(ref layers, value);
        }

        [DisplayName("Mega pixels")]
        [ReadOnly]
        public double MegaPixels
        {
            get => (Width * Height) / 1.0.Shift(6);
        }

        string password = string.Empty;
        [Style(StringStyle.Password)]
        public string Password
        {
            get => password;
            set => this.Change(ref password, value);
        }

        [Hidden]
        public Int32Region Region => new(0, 0, Width, Height);

        float resolution = 72;
        [Status]
        public float Resolution
        {
            get => resolution;
            set => this.Change(ref resolution, value);
        }

        ObservableCollection<Shape> selections = new();
        [Hidden]
        public ObservableCollection<Shape> Selections
        {
            get => selections;
            set => this.Change(ref selections, value);
        }

        DocumentStatus status = null;
        [Hidden]
        public DocumentStatus Status
        {
            get => status;
            set => this.Change(ref status, value);
        }

        [Hidden]
        public override string Title
        {
            get
            {
                var result = Name;

                if (IsModified)
                    result += "*";

                return result;
            }
        }

        [Hidden]
        public override object ToolTip => Title;

        [Hidden]
        public Viewer Viewer { get; private set; }

        int width;
        [Category(Category.Size)]
        [ReadOnly]
        [StringFormat(NumberFormat.Default)]
        public virtual int Width
        {
            get => width;
            set
            {
                this.Change(ref width, value);
                this.Changed(() => MegaPixels);
            }
        }

        double zoom = 1;
        [Format(Common.RangeFormat.Both)]
        [Range(0.05, 10.0, 0.01)]
        [StringFormat("p2")]
        public double Zoom
        {
            get => zoom;
            set => this.Change(ref zoom, value);
        }

        #endregion

        #region Document

        public Document() : base()
        {
            Layers = new LayerCollection();
        }

        void IFrameworkReference.SetReference(IFrameworkKey key, FrameworkElement element)
        {
            if (key == ViewerKey)
            {
                Viewer = element as Viewer;
                Viewer.Unloaded += OnViewerUnloaded;
            }
        }

        void OnViewerUnloaded(object sender, RoutedEventArgs e)
        {
            Viewer.Unloaded -= OnViewerUnloaded;
            Viewer = null;
        }

        #endregion

        #region Methods

        public void FitScreen() 
            => Viewer.If(i => i.FitScreen());

        public void FillScreen() 
            => Viewer.If(i => i.FillScreen());

        //...

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Height):
                    CropHeight = height;
                    break;
                case nameof(Width):
                    CropWidth = width;
                    break;
            }
        }

        async protected sealed override Task<bool> SaveAsync(string filePath)
        {
            try
            {
                switch (System.IO.Path.GetExtension(filePath).TrimExtension())
                {
                    //case Extension:
                    case TemplateExtension:
                        BinarySerializer.Serialize(filePath, this);
                        break;

                    case "ico":
                        System.Drawing.Bitmap result1 = null; //await Render();
                        using (var iconFileStream = new System.IO.FileStream(filePath, System.IO.FileMode.Create))
                            result1.Convert().Save(iconFileStream);

                        break;

                    default:
                        System.Drawing.Bitmap result2 = await RenderAsync();
                        var image = new MagickImage(result2);
                        image.Write(filePath);
                        break;
                }
                return true;
            }
            catch (Exception e)
            {
                Dispatch.Invoke(() => Log.Write<Document>(e));
                return false;
            }
        }

        public void SaveAsTemplate()
        {
            var inputWindow = new InputWindow
            {
                Placeholder = "Name",
                Title = "Save as template..."
            };
            inputWindow.ShowDialog();
            if (!inputWindow.Input.NullOrEmpty())
            {
                //Save($@"{Get.Current<App>().Properties.GetFolderPath("Templates")}\{inputWindow.Input}.{TemplateExtension}");
                return;
            }
            Dialog.Show("Save as template...", "Name can't be empty!", DialogImage.Error, Buttons.Ok);
        }

        //...

        public abstract Document Clone();
        object ICloneable.Clone() => Clone();

        public virtual void Copy() { }

        public virtual void CopyMerged() { }

        public virtual void Cut() { }

        public virtual void CutMerged() { }

        public virtual void Paste() { }

        public virtual void PasteNewLayer() { }

        //...

        public abstract System.Drawing.Bitmap Render();

        async public Task<System.Drawing.Bitmap> RenderAsync() => await Task.Run(() => Render());

        public abstract IEnumerable<Layer> NewLayers();

        //...

        public override void Subscribe()
        {
            History.Subscribe();
        }

        public override void Unsubscribe()
        {
            History.Unsubscribe();
        }

        //...

        public static void AssertInvalid(string filePath, string title = "Open", string message = "The file '{0}' is invalid or corrupt.") 
            => Dialog.Show(title, message.F(filePath), DialogImage.Error, Buttons.Ok);

        public static Document New(Type i)
        {
            Document result = null;
            if (i == typeof(StackDocument))
                result = new StackDocument() { IsModified = true };

            else if (i == typeof(LayoutDocument))
                result = new LayoutDocument() { IsModified = true };

            else if (i == typeof(ImageDocument))
            {
                var window = new NewWindow();
                window.ShowDialog();

                if (XWindow.GetResult(window) == 1)
                    result = new ImageDocument(window.DocumentPreset) { IsModified = true };
            }
            return result;
        }

        public static ImageDocument NewFromTemplate(string i)
        {
            BinarySerializer.Deserialize(i, out ImageDocument template);
            if (template == null)
            {
                AssertInvalid(i);
                return null;
            }

            return new ImageDocument(template) { IsModified = true };
        }

        public static Result SaveAs(System.Drawing.Bitmap bitmap)
        {
            var path = string.Empty;
            if (StorageWindow.Show(out path, "Save as...", StorageWindowModes.SaveFile, ImageFormats.Writable.Select(i => i.Extension)))
            {
                if (path?.Length > 0)
                {
                    try
                    {
                        using (var image = new ImageMagick.MagickImage(bitmap))
                            image.Write(path);

                        return true;
                    }
                    catch (Exception e)
                    {
                        Log.Write<MainViewModel>(e);
                        return new Error(e);
                    }
                }
            }
            return null;
        }

        #endregion

        #region Commands

        [field: NonSerialized]
        ICommand saveAsTemplateCommand;
        [Hidden]
        public ICommand SaveAsTemplateCommand => saveAsTemplateCommand ??= new RelayCommand(() => SaveAsTemplate());

        #endregion
    }

    #endregion

    #region DocumentStatus

    public class DocumentStatus : Base
    {
        enum Category { Size }

        public readonly Document Document;

        [Category(Category.Size)]
        [ReadOnly]
        [StringFormat(NumberFormat.Default)]
        [Status]
        public virtual int Height
        {
            get => Document.Height;
            set => Document.Height = value;
        }

        [DisplayName("Mega pixels")]
        [ReadOnly]
        [Status]
        public double MegaPixels => (Width * Height) / 1.0.Shift(6);

        [Status]
        public float Resolution
        {
            get => Document.Resolution;
            set => Document.Resolution = value;
        }

        [Category(Category.Size)]
        [ReadOnly]
        [StringFormat(NumberFormat.Default)]
        [Status]
        public virtual int Width
        {
            get => Document.Width;
            set => Document.Width = value;
        }

        [Featured(AboveBelow.Below)]
        [Format(Common.RangeFormat.Both)]
        [Index(int.MaxValue)]
        [Range(0.05, 10.0, 0.01)]
        [Status]
        [StringFormat("p2")]
        [Width(double.NaN, 128)]
        public double Zoom
        {
            get => Document.Zoom;
            set => Document.Zoom = value;
        }

        public DocumentStatus(Document document) : base() => Document = document;
    }

    #endregion

    #region ArrangeDocument

    [Serializable]
    public abstract class ArrangeDocument : Document
    {
        public sealed override IEnumerable<Layer> NewLayers()
        {
            var result = Get.Current<MainViewModel>().OpenFiles();
            if (result.Count() > 0)
            {
                foreach (var i in result)
                {
                    var j = new StaticLayer(i) { Name = Layer.DefaultName };
                    yield return j;
                }
            }
        }

        public override void Paste()
        {
            Layers.Add(new StaticLayer(Clipboard.GetImage().Bitmap().WriteableBitmap()));
        }
    }

    #endregion
}