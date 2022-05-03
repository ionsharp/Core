using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class ClipBorder : Border
    {
        readonly RectangleGeometry clip = new();

        object oldClip = null;

        public override UIElement Child
        {
            get => base.Child;
            set
            {
                if (Child != value)
                {
                    if (Child != null)
                        Child.SetValue(ClipProperty, oldClip);

                    oldClip = value != null ? value.ReadLocalValue(ClipProperty) : null;
                    base.Child = value;
                }
            }
        }

        public ClipBorder() : base() { }

        protected override void OnRender(DrawingContext context)
        {
            ApplyClip();
            base.OnRender(context);
        }

        void ApplyClip()
        {
            if (Child != null)
            {
                clip.RadiusX = clip.RadiusY = Math.Max(0.0, CornerRadius.TopLeft - (BorderThickness.Left * 0.5));
                clip.Rect = new Rect(Child.RenderSize);
                Child.Clip = clip;
            }
        }
    }
}