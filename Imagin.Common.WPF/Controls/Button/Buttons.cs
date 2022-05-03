using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Controls
{
    public class Buttons : ObservableCollection<Button>
    {
        static Button New(string label, int i, bool isDefault = false, bool isCancel = false)
        {
            var result = new Button() { Content = label, IsDefault = isDefault, IsCancel = isCancel };
            XButton.SetResult(result, i);
            return result;
        }

        //...

        public static Button[] AbortRetryIgnore 
            = Array<Button>.New(New("Abort", 0), New("Retry", 1, true), New("Ignore", 2, false, true));

        public static Button[] Cancel 
            = Array<Button>.New(New("Cancel", 0, false, true));

        public static Button[] Continue 
            = Array<Button>.New(New("Continue", 0, true));

        public static Button[] ContinueCancel 
            = Array<Button>.New(New("Continue", 0, true), New("Cancel", 1, false, true));

        public static Button[] Done 
            = Array<Button>.New(New("Done", 0, true));

        public static Button[] Ok 
            = Array<Button>.New(New("Ok", 0, true));

        public static Button[] OkCancel 
            = Array<Button>.New(New("Ok", 0, true), New("Cancel", 1, false, true));

        public static Button[] SaveCancel 
            = Array<Button>.New(New("Save", 0, true), New("Cancel", 1, false, true));

        public static Button[] YesCancel 
            = Array<Button>.New(New("Yes", 0, true), New("Cancel", 1, false, true));

        public static Button[] YesNo 
            = Array<Button>.New(New("Yes", 0, true), New("No", 1, false, true));

        public static Button[] YesNoCancel 
            = Array<Button>.New(New("Yes", 0, true), New("No", 1), New("Cancel", 2, false, true));

        //...

        public Buttons() : base() { }

        public Buttons(Button[] input) : base(input) { }

        public Buttons(Window window, params Button[] input) : base(input) 
        {
            foreach (var i in this)
            {
                i.Command
                    = XWindow.CloseCommand;
                i.CommandParameter
                    = XButton.GetResult(i);
                i.CommandTarget
                    = window;
            }
        }
    }
}