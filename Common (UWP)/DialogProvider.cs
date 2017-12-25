using System;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public static class DialogProvider
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="Text"></param>
        /// <param name="Buttons"></param>
        /// <returns></returns>
        public static async Task<int> ShowAsync(string Title, string Text, params DialogButton[] Buttons)
        {
            var Dialog = new MessageDialog(Text, Title);

            foreach (var i in Buttons)
            {
                Dialog.Commands.Add(new UICommand(i.Label)
                {
                    Id = i.Id
                });
            }

            return (int)(await Dialog.ShowAsync()).Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Title"></param>
        /// <param name="DefaultText"></param>
        /// <param name="PlaceholderText"></param>
        /// <param name="PrimaryButtonText"></param>
        /// <param name="SecondaryButtonText"></param>
        /// <returns></returns>
        public static async Task<string> ShowInputAsync(string Title, string DefaultText, string PlaceholderText, string PrimaryButtonText = "Ok", string SecondaryButtonText = "Cancel")
        {
            var Input = new TextBox()
            {
                Margin = new Thickness(0, 15, 0, 0),
                PlaceholderText = PlaceholderText,
                Text = DefaultText
            };

            var Dialog = new ContentDialog()
            {
                Content = Input,
                Title = Title,
                IsSecondaryButtonEnabled = true,
                PrimaryButtonText = PrimaryButtonText,
                SecondaryButtonText = SecondaryButtonText,
            };

            if (await Dialog.ShowAsync() == ContentDialogResult.Primary)
            {
                return Input.Text;
            }
            else return string.Empty;
        }
    }
}
