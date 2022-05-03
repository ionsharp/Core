using Imagin.Common;
using System;
using System.Collections.Generic;

namespace Imagin.Apps.Paint
{
    [DisplayName("Clone layer")]
    [Serializable]
    public class CloneLayerAction : RegionalLayerAction
    {
        public CloneLayerAction(Layer layer, LayerCollection layers, int index) : base(layer, layers, index) { }

        public override BaseAction Clone()
        {
            return default;
        }

        public override void Execute()
        {
            Layers.Insert(Index, Target);
        }

        public override void Reverse()
        {
            Layers.Remove(Target);
        }
    }

    [DisplayName("Delete layer")]
    [Serializable]
    public class DeleteLayerAction : RegionalLayerAction
    {
        public DeleteLayerAction(Layer layer, LayerCollection layers, int index) : base(layer, layers, index) { }

        public override BaseAction Clone()
        {
            return default;
        }

        public override void Execute()
        {
            Layers.Remove(Target);
        }

        public override void Reverse()
        {
            Layers.Insert(Index, Target);
        }
    }

    [DisplayName("Move layer")]
    [Serializable]
    public class MoveLayerAction : BaseAction
    {
        IEnumerable<Layer> Layers
        {
            get; set;
        }

        LayerCollection Source
        {
            get; set;
        }

        public MoveLayerAction(LayerCollection source, IEnumerable<Layer> layers) : base()
        {
            Source = source;
            Layers = layers;
        }

        public override BaseAction Clone()
        {
            return default;
        }

        public override void Execute()
        {

        }

        public override void Reverse()
        {
        }
    }

    [DisplayName("New layer")]
    [Serializable]
    public class NewLayerAction : RegionalLayerAction
    {
        public NewLayerAction(Layer layer, LayerCollection layers, int index) : base(layer, layers, index) { }

        public override BaseAction Clone()
        {
            return default;
        }

        public override void Execute()
        {
            Layers.Insert(Index, Target);
        }

        public override void Reverse()
        {
            Layers.Remove(Target);
        }
    }

    //...

    [DisplayName("Merge layer")]
    [Serializable]
    public class MergeLayerAction : BaseAction
    {
        public MergeLayerAction() : base() { }

        public override BaseAction Clone()
        {
            return default;
        }

        public override void Execute()
        {
        }

        public override void Reverse()
        {
        }
    }

    [DisplayName("Paste layer style")]
    [Serializable]
    public class PasteLayerStyleAction : LayerAction
    {
        LayerStyle NewStyle
        {
            get; set;
        }

        LayerStyle OldStyle
        {
            get; set;
        }

        public PasteLayerStyleAction(Layer layer, LayerStyle oldStyle, LayerStyle newStyle) : base(layer)
        {
            OldStyle = oldStyle;
            NewStyle = newStyle;
        }

        public override BaseAction Clone()
        {
            return default;
        }

        public override void Execute()
        {
            //Target.Style = NewStyle;
        }

        public override void Reverse()
        {
            //Target.Style = OldStyle;
        }
    }

    [DisplayName("Rasterize layer")]
    [Serializable]
    public class RasterizeLayerAction : BaseAction
    {
        public RasterizeLayerAction() : base() { }

        public override BaseAction Clone()
        {
            return default;
        }

        public override void Execute()
        {
        }

        public override void Reverse()
        {
        }
    }
}