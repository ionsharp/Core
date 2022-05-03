using Imagin.Common.Converters;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;

namespace Imagin.Common.Controls
{
    public class ItemVisibilityConverter : MultiConverter<Visibility>
    {
        public static ItemVisibilityConverter Default { get; private set; } = new ItemVisibilityConverter();
        ItemVisibilityConverter() { }

        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            //(0) Item
            //(1) File attributes
            //(2) File extensions
            //(3) Folder attributes
            //(4) View files
            if (values?.Length == 5)
            {
                if (values[0] is Item item)
                {
                    if (values[1] is Attributes fileAttributes)
                    {
                        if (values[3] is Attributes folderAttributes)
                        {
                            if (values[4] is bool viewFiles)
                            {
                                if (item is Storage.File file)
                                {
                                    if (viewFiles)
                                    {
                                        if (!fileAttributes.HasFlag(Attributes.Hidden))
                                        {
                                            if (file.IsHidden)
                                                return Visibility.Collapsed;
                                        }

                                        if (!fileAttributes.HasFlag(Attributes.ReadOnly))
                                        {
                                            if (file.IsReadOnly)
                                                return Visibility.Collapsed;
                                        }

                                        if (values[2] is string fileExtensions)
                                        {
                                            var e = fileExtensions.Split(Array<char>.New(';'), StringSplitOptions.RemoveEmptyEntries).Select(i => i.TrimExtension());
                                            if (!e.Any() || e.Contains(Path.GetExtension(file.Path).TrimExtension()))
                                                return Visibility.Visible;

                                            return Visibility.Collapsed;
                                        }

                                        return Visibility.Visible;
                                    }
                                }
                                else if (item is Folder folder)
                                {
                                    if (!folderAttributes.HasFlag(Attributes.Hidden))
                                    {
                                        if (folder.IsHidden)
                                            return Visibility.Collapsed;
                                    }

                                    if (!folderAttributes.HasFlag(Attributes.ReadOnly))
                                    {
                                        if (folder.IsReadOnly)
                                            return Visibility.Collapsed;
                                    }

                                    return Visibility.Visible;
                                }
                                else if (item is Drive)
                                {
                                    return Visibility.Visible;
                                }
                            }
                        }
                    }
                }
            }
            return Visibility.Collapsed;
        }
    }
}