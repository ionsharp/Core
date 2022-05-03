using Imagin.Common.Controls;
using Imagin.Common.Numbers;
using System;
using System.Windows.Controls;

namespace Imagin.Common
{
    public class DialogReference : Base
    {
        public Button[] Buttons { get; private set; }

        public Uri Image { get; private set; } = DialogImage.Information;

        public DoubleSize ImageSize { get; private set; } = DialogWindow.DefaultImageSize;

        public object Message { get; private set; }

        public GetSetBoolean NeverShow { get; private set; }

        public int Result { get; set; }

        public string Title { get; private set; }

        public DialogReference(string title, object message, Uri image, Button[] buttons)
        {
            Title
                = title;
            Message
                = message;
            Image
                = image;
            Buttons
                = buttons;
        }
    }
}