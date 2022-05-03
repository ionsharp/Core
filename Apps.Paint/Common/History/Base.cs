using Imagin.Common;
using Imagin.Common.Input;
using System;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public abstract class BaseAction : Base, ICloneable
    {
        public virtual bool CanRepeat => false;

        protected BaseAction() : base()
        {
        }

        public abstract void Execute();

        public abstract void Reverse();

        object ICloneable.Clone()
        {
            return Clone();
        }
        public abstract BaseAction Clone();

        ICommand executeCommand;
        public ICommand ExecuteCommand => executeCommand ??= new RelayCommand(Execute);
    }

    [Serializable]
    public abstract class LayerAction : BaseAction
    {
        protected Layer Target { get; set; }

        protected LayerAction(Layer target) : base() => Target = target;
    }

    [Serializable]
    public abstract class RegionalLayerAction : LayerAction
    {
        /// <summary>
        /// The index of the layer relative to the collection.
        /// </summary>
        protected int Index { get; set; }

        /// <summary>
        /// The collection of layers in which the layer is stored.
        /// </summary>
        protected LayerCollection Layers { get; set; }

        protected RegionalLayerAction(Layer layer, LayerCollection layers, int index) : base(layer)
        {
            Layers = layers; Index = index;
        }
    }
}