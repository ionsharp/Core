using Imagin.Common;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Imagin.Apps.Paint
{
    [DisplayName("Animation")]
    [Icon(App.ImagePath + "DocumentAnimation.png")]
    [Serializable]
    public class AnimationDocument : Document
    {
        public AnimationDocument() : base() { }

        public override Document Clone() => new AnimationDocument();

        public override Bitmap Render() => default;

        public override IEnumerable<Layer> NewLayers() => default;
    }
}