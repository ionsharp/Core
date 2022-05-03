using Imagin.Common;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using Imagin.Common.Numbers;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Imagin.Apps.Paint
{
    #region Tool

    [Serializable]
    public abstract class Tool : Base, ISelect
    {
        #region Events

        [field: NonSerialized]
        public event SelectedEventHandler Selected;

        #endregion

        #region Fields

        public const string LayerNotSelected = "A layer must be selected.";

        public const string InvalidLayer = "Layer doesn't support this type of operation";

        //...

        [NonSerialized]
        protected Point? MouseDown;

        [Hidden]
        [NonSerialized]
        public Point? MouseDownAbsolute = null;

        [NonSerialized]
        protected Point? MouseMove;

        [Hidden]
        [NonSerialized]
        public Point? MouseMoveAbsolute = null;

        [NonSerialized]
        protected Point? MouseUp;

        [Hidden]
        [NonSerialized]
        public Point? MouseUpAbsolute;

        [Hidden]
        [NonSerialized]
        public ImageViewer ImageViewer = null;

        #endregion

        #region Properties

        protected double DifferenceX 
            => (MouseDown.Value.X - MouseMove.Value.X).Absolute().Round().Int32();

        protected double DifferenceY 
            => (MouseDown.Value.Y - MouseMove.Value.Y).Absolute().Round().Int32();

        protected IEnumerable<Document> Documents 
            => Get.Current<MainViewModel>().Documents.Select(i => i as Document);

        protected LayerCollection Layers 
            => Panel.Find<LayersPanel>().Layers;

        protected WriteableBitmap Pixels
            => (TargetLayer as PixelLayer).Pixels;

        protected Viewer Viewer 
            => Document.Viewer;

        //...

        [Hidden]
        public virtual Cursor Cursor { get; } = Cursors.None;

        [Hidden]
        public virtual Uri CursorIcon { get; } = null;

        [Hidden]
        public Document Document => Get.Current<MainViewModel>().ActiveDocument;

        [Hidden]
        public virtual bool Hidden => false;

        [Hidden]
        public abstract Uri Icon { get; }

        bool isSelected;
        [Hidden]
        public bool IsSelected
        {
            get => isSelected;
            set
            {
                this.Change(ref isSelected, value);
                if (value) OnSelected();
            }
        }

        [Hidden]
        public int Quadrant
        {
            get
            {
                if (MouseDown.Value.X > MouseMove.Value.X && MouseDown.Value.Y > MouseMove.Value.Y)
                    return 0;

                if (MouseDown.Value.X < MouseMove.Value.X && MouseDown.Value.Y > MouseMove.Value.Y)
                    return 1;

                if (MouseDown.Value.X > MouseMove.Value.X && MouseDown.Value.Y < MouseMove.Value.Y)
                    return 2;

                if (MouseDown.Value.X < MouseMove.Value.X && MouseDown.Value.Y < MouseMove.Value.Y)
                    return 3;

                return -1;
            }
        }

        bool showCompass = false;
        [Hidden]
        public bool ShowCompass
        {
            get => showCompass;
            set => this.Change(ref showCompass, value);
        }

        [Hidden]
        public Layer TargetLayer => Panel.Find<LayersPanel>().SelectedLayer;

        #endregion

        #region Tool

        public Tool() : base() { }

        #endregion

        #region Methods

        protected bool AssertLayer<T>() where T : Layer
        {
            if (TargetLayer == null)
            {
                Error(LayerNotSelected);
                return false;
            }
            else if (TargetLayer is T)
                return true;

            Error(InvalidLayer);
            return false;
        }

        protected void Error(string text) => Dialog.Show(ToString(), text, DialogImage.Error, Buttons.Ok);

        //...

        protected virtual bool AssertLayer() => true;

        protected virtual void OnDocumentChanged(Document document) { }

        protected virtual void OnSelected()
        {
            Selected?.Invoke(this, new SelectedEventArgs(null));
        }

        //...

        public virtual bool OnMouseDown(Point point)
        {
            var result = AssertLayer();
            if (result)
            {
                MouseUp = null;
                MouseDown = point;
            }
            return result;
        }

        public virtual void OnMouseDoubleClick(Point point) { }

        public virtual void OnMouseMove(Point point)
        {
            MouseMove = point;
        }

        public virtual void OnMouseUp(Point point)
        {
            MouseUp = point;
            MouseDown = null; MouseMove = null;
        }

        //...

        public virtual void OnPreviewRendered(System.Windows.Media.DrawingContext input, double zoom) { }

        //...

        public static Int32Region GetRegion(Point a, Point b)
        {
            double x = 0, y = 0, height = 0, width = 0;

            x = a.X < b.X ? a.X : b.X;
            y = a.Y < b.Y ? a.Y : b.Y;

            width = (a.X > b.X ? a.X - b.X : b.X - a.X).Absolute();
            height = (a.Y > b.Y ? a.Y - b.Y : b.Y - a.Y).Absolute();

            if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                var greater = height > width ? height : width;
                var smaller = height < width ? height : width;

                if (b.X > a.X)
                {
                    //Top right
                    if (b.Y < a.Y)
                    {
                        if (b.Y + greater > a.Y)
                        {
                            height = width = smaller;
                            y = a.Y - smaller;
                        }
                        else height = width = greater;
                    }
                    //Bottom right
                    else height = width = greater;
                }
                else
                {
                    //Top left
                    if (b.Y < a.Y)
                    {
                        height = width = smaller;
                        x = a.X - smaller;
                        y = a.Y - smaller;
                    }
                    //Bottom left
                    else
                    {
                        if (b.X + greater > a.X)
                        {
                            height = width = smaller;
                            x = a.X - smaller;
                        }
                        else height = width = greater;
                    }
                }
            }

            return new Int32Region(x.Int32(), y.Int32(), width.Int32(), height.Int32());
        }

        ICommand savePresetCommand;
        [DisplayName("Save preset")]
        [Icon(App.ImagePath + "SaveTool.png")]
        [Visible]
        public ICommand SavePresetCommand => savePresetCommand ??= new RelayCommand(() => Get.Current<Options>().ToolPresets.Add(new(GetType().Name, this.SmartClone())));

        #endregion
    }

    #endregion

    #region BlendTool

    [Serializable]
    public abstract class BlendTool : Tool
    {
        enum Category { Blend }

        BlendModes mode = BlendModes.Normal;
        [Category(Category.Blend)]
        [Index(0)]
        public virtual BlendModes Mode
        {
            get => mode;
            set => this.Change(ref mode, value);
        }

        double opacity = 1;
        [Category(Category.Blend)]
        [Format(Common.RangeFormat.UpDown)]
        [Index(1)]
        [Range(0.0, 1.0, 0.01)]
        [Width(64)]
        public double Opacity
        {
            get => opacity;
            set => this.Change(ref opacity, value);
        }
    }

    #endregion
}