using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Imagin.Apps.Paint
{
    [Serializable]
    public class LayerCollection : ObservableCollection<Layer>
    {
        #region Events

        public event EventHandler<EventArgs> Changed;

        #endregion

        #region LayerCollection

        public LayerCollection() : base() { }

        #endregion

        #region Methods

        void OnChanged() 
            => Changed?.Invoke(this, EventArgs.Empty);

        void OnGroupLayerChanged(object sender, EventArgs e) 
            => OnChanged();

        //...

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    e.NewItems.ForEach<Layer>(i =>
                    {
                        if (i is GroupLayer j)
                            j.Layers.Changed += OnGroupLayerChanged;
                    });
                    break;

                case NotifyCollectionChangedAction.Move:
                    break;

                case NotifyCollectionChangedAction.Remove:
                    e.OldItems.ForEach<Layer>(i =>
                    {
                        if (i is GroupLayer j)
                            j.Layers.Changed -= OnGroupLayerChanged;
                    });
                    break;

                case NotifyCollectionChangedAction.Replace:
                    break;

                case NotifyCollectionChangedAction.Reset:
                    break;
            }
            OnChanged();
        }

        //...

        void Insert(Layer targetLayer, IEnumerable<Layer> addLayers, bool above)
        {
            var index = 0;
            LayerCollection layers = null; GroupLayer parent = null;
            if (targetLayer != null)
            {
                if (targetLayer is GroupLayer)
                {
                    index = 0;
                    layers = ((GroupLayer)targetLayer).Layers;
                    parent = (GroupLayer)targetLayer;
                }
                else
                {
                    layers
                    = targetLayer.Parent != null
                    ? targetLayer.Parent.Layers
                    : this;

                    index = layers.IndexOf(targetLayer);
                    parent = targetLayer.Parent;
                }
            }
            else layers = this;

            foreach (var i in addLayers)
            {
                i.Parent = parent;
                layers.Insert(above ? index : index + 1, i);
            }

            addLayers.Last().IsSelected = true;
        }

        public void InsertAbove(Layer targetLayer, Layer layer)
            => InsertAbove(targetLayer, Array<Layer>.New(layer));

        public void InsertBelow(Layer targetLayer, Layer layer)
            => InsertBelow(targetLayer, Array<Layer>.New(layer));

        public void InsertAbove(Layer targetLayer, IEnumerable<Layer> layers)
            => Insert(targetLayer, layers, true);

        public void InsertBelow(Layer targetLayer, IEnumerable<Layer> layers)
            => Insert(targetLayer, layers, false);

        //...

        public void ForAll<T>(Action<T> action, LayerCollection layers = null) where T : Layer
        {
            layers = layers ?? this;
            for (var i = layers.Count - 1; i >= 0; i--)
            {
                if (layers[i] is T)
                    action((T)layers[i]);

                if (layers[i] is GroupLayer)
                    ForAll(action, (layers[i] as GroupLayer).Layers);
            }
        }

        public void ForEach<T>(Action<T> action) where T : Layer
        {
            for (var i = Count - 1; i >= 0; i--)
            {
                if (this[i] is T)
                    action((T)this[i]);
            }
        }

        new public void Remove(Layer layer)
        {
            if (Count == 1)
            {
                Dialog.Show("Delete", "The document must specify at least one layer.", DialogImage.Error, Common.Controls.Buttons.Ok);
                return;
            }

            var layers = layer.Parent?.Layers ?? this;

            var index = layers.IndexOf(layer);
            if (index >= 0 && index < layers.Count)
                layers.RemoveAt(index);

            else Dialog.Show("Delete", "Something unexpected happen...", DialogImage.Error, Common.Controls.Buttons.Ok);
        }

        public void Remove<T>(Predicate<T> condition = null, LayerCollection layers = null) where T : Layer
        {
            layers = layers ?? this;
            for (var i = layers.Count - 1; i >= 0; i--)
            {
                if (layers[i] is GroupLayer groupLayer)
                    Remove(condition, groupLayer.Layers);

                if (layers[i] is T layer)
                {
                    if (condition?.Invoke(layer) != false)
                        RemoveAt(i);
                }
            }
        }

        public void UnselectAll() 
            => ForAll<Layer>(i => i.IsSelected = false);

        #endregion
    }
}