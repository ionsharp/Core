using GongSolutions.Wpf.DragDrop;
using Imagin.Common.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Imagin.Common
{
    public abstract class DropHandler<Element, Data> : IDropTarget where Element : UIElement
    {
        public readonly Element Parent;

        //...

        public DropHandler(Element parent) => Parent = parent;

        //...

        protected abstract IEnumerable<Data> From(object source);

        protected abstract IEnumerable<Data> From(DataObject dataObject);

        //...

        protected virtual bool Droppable(UIElement input) => input is Element;

        //...

        public void DragOver(IDropInfo info)
        {
            var source = info.Data is DataObject i ? From(i) : From(info.Data);
            if (source.Any())
            {
                if (Droppable(info.VisualTarget))
                {
                    var result = DragOver(source, info.VisualTarget);
                    info.Effects = result;
                    return;
                }
            }
            info.Effects = DragDropEffects.None;
        }

        public void Drop(IDropInfo info)
        {
            var source = info.Data is DataObject i ? From(i) : From(info.Data);
            if (source.Any())
            {
                if (Droppable(info.VisualTarget))
                    Drop(source, info.VisualTarget);
            }
        }

        //...

        protected abstract DragDropEffects DragOver(IEnumerable<Data> source, UIElement target);

        protected abstract void Drop(IEnumerable<Data> source, UIElement target);
    }

    public class DefaultDropHandler : DropHandler<FrameworkElement, object>
    {
        public DefaultDropHandler(FrameworkElement parent) : base(parent) { }

        protected override DragDropEffects DragOver(IEnumerable<object> source, UIElement target) => DragDropEffects.Move;

        protected override void Drop(IEnumerable<object> source, UIElement target) { }

        protected override IEnumerable<object> From(object source)
        {
            yield return source;
        }

        protected override IEnumerable<object> From(DataObject dataObject)
        {
            if (dataObject.ContainsAudio())
                yield return dataObject.GetAudioStream();

            if (dataObject.ContainsFileDropList())
            {
                foreach (var i in dataObject.GetFileDropList())
                    yield return i;
            }
            if (dataObject.ContainsImage())
                yield return dataObject.GetImage();

            if (dataObject.ContainsText())
                yield return dataObject.GetText();
        }
    }
}