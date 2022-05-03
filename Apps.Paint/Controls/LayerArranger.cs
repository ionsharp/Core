using Imagin.Apps.Paint.Effects;
using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common;
using Imagin.Common.Media;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace Imagin.Apps.Paint
{
    public class LayerArranger : Control
    {
        #region Properties

        public static readonly DependencyProperty LayersProperty = DependencyProperty.Register(nameof(Layers), typeof(LayerCollection), typeof(LayerArranger), new FrameworkPropertyMetadata(null, OnLayersChanged));
        public LayerCollection Layers
        {
            get => (LayerCollection)GetValue(LayersProperty);
            set => SetValue(LayersProperty, value);
        }
        static void OnLayersChanged(object sender, DependencyPropertyChangedEventArgs e) => sender.As<LayerArranger>().OnLayersChanged(e);

        static readonly DependencyPropertyKey LayoutKey = DependencyProperty.RegisterReadOnly(nameof(Layout), typeof(object), typeof(LayerArranger), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty LayoutProperty = LayoutKey.DependencyProperty;
        public object Layout
        {
            get => GetValue(LayoutProperty);
            private set => SetValue(LayoutKey, value);
        }

        public static readonly DependencyProperty ZoomProperty = DependencyProperty.Register(nameof(Zoom), typeof(double), typeof(LayerArranger), new FrameworkPropertyMetadata(1d));
        public double Zoom
        {
            get => (double)GetValue(ZoomProperty);
            set => SetValue(ZoomProperty, value);
        }

        #endregion

        #region LayerArranger

        public LayerArranger() { }

        #endregion

        #region Methods

        void OnLayersChanged(Value<LayerCollection> input)
        {
            if (input.Old is LayerCollection a)
                a.Changed -= OnLayersChanged;

            if (input.New is LayerCollection b)
                b.Changed += OnLayersChanged;

            Update();
        }

        void OnLayersChanged(object sender, EventArgs e) 
            => Update();

        //...

        BlendEffect GetBlendEffect(StyleLayer bLayer)
        {
            var result = new BlendEffect();
            result.Bind(BlendEffect.ActualBlendModeProperty, nameof(StyleLayer.BlendMode), bLayer);

            var bParent = bLayer.Parent?.Layers ?? Layers;
            if (bParent != null)
            {
                var bIndex = bParent.IndexOf(bLayer);
                var aIndex = bIndex + 1;

                if (aIndex < bParent.Count)
                {
                    var aLayer = bParent[aIndex];
                    if (aLayer is VisualLayer aVisual)
                    {
                        result.Bind(BlendEffect.ActualMaskProperty, nameof(LayerStyle.Mask), bLayer.Style);
                        result.CInput = new ImageBrush { ImageSource = aVisual.Pixels };
                    }
                }
            }

            return result;
        }

        ChannelsEffect GetChannelEffect(StyleLayer layer)
        {
            var result = new ChannelsEffect();
            result.Bind(ChannelsEffect.RedProperty, $"{nameof(StyleLayer.Style)}.{nameof(LayerStyle.Channels)}", layer, BindingMode.OneWay, EnumFlagsToBooleanConverter.Default, RedGreenBlue.Red);
            result.Bind(ChannelsEffect.GreenProperty, $"{nameof(StyleLayer.Style)}.{nameof(LayerStyle.Channels)}", layer, BindingMode.OneWay, EnumFlagsToBooleanConverter.Default, RedGreenBlue.Green);
            result.Bind(ChannelsEffect.BlueProperty, $"{nameof(StyleLayer.Style)}.{nameof(LayerStyle.Channels)}", layer, BindingMode.OneWay, EnumFlagsToBooleanConverter.Default, RedGreenBlue.Blue);
            return result;
        }

        //...

        ShaderEffect GetEffect(GroupLayer layer, UIElement oldVisual)
        {
            var newVisual = GetVisual(layer, oldVisual);

            var result = GetBlendEffect(layer);
            result.BInput = new VisualBrush { Visual = newVisual };
            return result;
        }

        ShaderEffect GetEffect(VisualLayer layer)
        {
            var result = GetBlendEffect(layer);

            var brush = new VisualBrush { Stretch = Stretch.Fill, TileMode = TileMode.None, Visual = GetVisual(layer) };
            if (layer is PixelLayer)
            {
                brush.ViewboxUnits = BrushMappingMode.Absolute;
                brush.MultiBind(VisualBrush.ViewboxProperty, new MultiConverter<Rect>(i =>
                {
                    if (i.Values?.Length == 4)
                    {
                        if (i.Values[0] is int x && i.Values[1] is int y && i.Values[2] is double width && i.Values[3] is double height)
                            return new Rect(-x, -y, width, height);
                    }
                    return new Rect(0, 0, 0, 0);
                }),
                null, new Binding("X") { Source = layer }, new Binding("Y") { Source = layer }, new Binding(nameof(Width)) { Source = this }, new Binding(nameof(Height)) { Source = this });
            }
            result.BInput = brush;
            return result;
        }

        //...

        UIElement GetVisual(GroupLayer layer, UIElement input)
        {
            var result = new Border();
            result.MultiBind(Border.OpacityProperty, new MultiConverter<double>(i => i.Values?.Length == 2 && i.Values[0] is bool j && j && i.Values[1] is double k ? k : 0), null, new Binding(nameof(Layer.IsVisible)) { Source = layer }, new Binding($"{nameof(StyleLayer.Style)}.{nameof(LayerStyle.Opacity)}") { Source = layer });

            result.Child = input;
            result.Effect = GetChannelEffect(layer);
            return result;
        }

        UIElement GetVisual(VisualLayer layer) => new LayerView() { Layer = layer };

        //...

        /// <summary>
        /// (Recursive) Arranges layers starting with last and ending with first. The last layer in a <see cref="GroupLayer"/> is blended 
        /// with the layer immediately below the <see cref="GroupLayer"/> (if that layer is also a <see cref="GroupLayer"/>,
        /// it is blended with the first layer of the second <see cref="GroupLayer"/>).
        /// </summary>
        /// <remarks>To do: Blend <see cref="GroupLayer"/> first, THEN blend that with whatever is below it (recursively). Adjustments, filters, and components also need applied.</remarks>
        Border Arrange(LayerCollection layers)
        {
            if (layers == null)
                return null;

            Border a = new();
            Border b = null;

            Layer lastLayer = null;

            layers.ForEach<StackableLayer>(i =>
            {
                if (lastLayer == null)
                {
                    if (i is GroupLayer groupLayer)
                        a.Child = Arrange(groupLayer.Layers);

                    else if (i is VisualLayer visualLayer)
                        a.Child = GetVisual(visualLayer);

                    else return;

                    lastLayer = i;
                    return;
                }

                b = new Border() { SnapsToDevicePixels = true };
                b.Child = a;

                ShaderEffect effect = null;
                if (i is EffectLayer effectLayer)
                {
                    effect = effectLayer.Effect;
                    a.Bind(UIElement.EffectProperty, nameof(ImageEffect.IsVisible), effect, BindingMode.OneWay, new ComplexConverter<bool, ImageEffect>(i => i.Value ? (ImageEffect)i.ActualParameter : null), effect);
                }
                else
                {
                    if (i is GroupLayer groupLayer)
                        effect = GetEffect(groupLayer, Arrange(groupLayer.Layers));

                    else if (i is VisualLayer visualLayer)
                        effect = GetEffect(visualLayer);

                    a.Effect = effect;
                }

                a = b;
                lastLayer = i;
            });
            return a;
        }

        //...

        public void Update()
        {
            Layout = null;
            Layout = Arrange(Layers);
        }

        #endregion
    }
}