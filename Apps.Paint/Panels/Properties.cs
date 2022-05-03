using Imagin.Common;
using Imagin.Common.Linq;
using Imagin.Common.Media;
using Imagin.Common.Models;
using Imagin.Common.Numbers;
using System;
using System.Linq;

namespace Imagin.Apps.Paint
{
    public class PropertiesPanel : Common.Models.PropertiesPanel
    {
        public static readonly Type[] Types = new Type[]
        {
            typeof(Brush),
            typeof(Curve),
            typeof(Document),
            typeof(Namable<DoubleMatrix>),
            typeof(Gradient),
            typeof(Layer),
            typeof(LayerStyle),
            typeof(Path),
            typeof(Selection),
            typeof(Shape),
        };

        public PropertiesPanel() : base()
        {
            Get.Current<MainViewModel>().ActiveContentChanged 
                += OnActiveContentChanged;
            Get.Current<MainViewModel>().DocumentRemoved 
                += OnDocumentRemoved;
        }

        void OnActiveContentChanged(object sender, Common.Input.EventArgs<Content> e)
        {
            var content = e.Value;
            if (content is Document)
                Select(content);

            content.If<IGroupPanel>
                (i => i.SelectedItem != null, i => Select(i.SelectedItem));
            content.If<LayersPanel>
                (i => i.SelectedLayer != null, i => Select(i.SelectedLayer));
        }

        void OnDocumentRemoved(object sender, Common.Input.EventArgs<Document> e)
        {
            for (var i = 0; i < Sources.Count; i++)
            {
                if (ReferenceEquals(Sources[i], e.Value))
                {
                    Sources.RemoveAt(i);
                    SelectedIndex = i;
                    return;
                }
            }
        }

        public void Select(object input)
        {
            if (input != null)
            {
                var baseType = Types.FirstOrDefault(i => input.GetType().Inherits(i, true));
                if (baseType != null)
                {
                    for (var i = 0; i < Sources.Count; i++)
                    {
                        if (Sources[i].GetType().Inherits(baseType, true))
                        {
                            if (!ReferenceEquals(Sources[i], input))
                            {
                                Sources[i] = input;
                                SelectedIndex = i;
                            }
                            return;
                        }
                    }
                    Sources.Add(input);
                    SelectedIndex = Sources.Count - 1;
                }
            }
        }
    }
}