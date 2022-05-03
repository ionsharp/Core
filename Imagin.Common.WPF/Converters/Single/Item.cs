using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System.IO;
using System.Windows.Data;

namespace Imagin.Common.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public class FileExtensionConverter : Converter<string, string>
    {
        public static FileExtensionConverter Default { get; private set; } = new FileExtensionConverter();
        FileExtensionConverter() { }

        protected override ConverterValue<string> ConvertTo(ConverterData<string> input)
        {
            string result = null;
            return !Try.Invoke(() => result = Path.GetExtension(input.Value))
                ? (ConverterValue<string>)Nothing.Do
                : input.Parameter == 0 ? result.Replace(".", string.Empty) : input.Parameter == 1 ? result : throw input.InvalidParameter;
        }

        protected override ConverterValue<string> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }

    [ValueConversion(typeof(object), typeof(string))]
    public class FileNameConverter : Converter<object, string>
    {
        public static FileNameConverter Default { get; private set; } = new FileNameConverter();
        FileNameConverter() { }

        protected override ConverterValue<string> ConvertTo(ConverterData<object> input)
        {
            var path = input.Value.ToString();

            if (path == StoragePath.Root)
                return StoragePath.RootName;

            if (path.EndsWith(@":\"))
            {
                foreach (var i in Computer.Drives)
                {
                    if (path.Equals(i.Name))
                        return $"{i.VolumeLabel} ({i.Name.Replace(@"\", string.Empty)})";
                }
                return path;
            }

            return Folder.Long.Exists(path) || input.Parameter == 1
                ? Path.GetFileName(path)
                : input.Parameter == 0
                    ? Path.GetFileNameWithoutExtension(path)
                    : throw input.InvalidParameter;
        }

        protected override ConverterValue<object> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }

    [ValueConversion(typeof(long), typeof(string))]
    public class FileSizeConverter : Converter<long, string>
    {
        public static FileSizeConverter Default { get; private set; } = new FileSizeConverter();
        public FileSizeConverter() : base() { }

        protected override ConverterValue<string> ConvertTo(ConverterData<long> input)
            => input.Value.FileSize(input.ActualParameter is FileSizeFormat i ? i : FileSizeFormat.BinaryUsingSI);

        protected override ConverterValue<long> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }

    [ValueConversion(typeof(double), typeof(string))]
    public class FileSpeedConverter : Converter<double, string>
    {
        public static FileSpeedConverter Default { get; private set; } = new FileSpeedConverter();
        FileSpeedConverter() { }

        protected override bool Is(object input) => input is double || input is string;

        protected override ConverterValue<string> ConvertTo(ConverterData<double> input)
        {
            long result = 0;

            if (input.ActualValue is double a)
                result = a.Int64();

            if (input.ActualValue is string b)
                result = b.Int64();

            return $"{result.FileSize(FileSizeFormat.BinaryUsingSI)}/s";
        }

        protected override ConverterValue<double> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }

    //...

    [ValueConversion(typeof(string), typeof(string))]
    public class ItemAccessedConverter : Converter<string, string>
    {
        public static ItemAccessedConverter Default { get; private set; } = new();
        public ItemAccessedConverter() : base() { }

        protected override ConverterValue<string> ConvertTo(ConverterData<string> input)
            => new FileInfo(input.Value).LastAccessTime.ToString();

        protected override ConverterValue<string> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(string))]
    public class ItemCreatedConverter : Converter<string, string>
    {
        public static ItemCreatedConverter Default { get; private set; } = new();
        public ItemCreatedConverter() : base() { }

        protected override ConverterValue<string> ConvertTo(ConverterData<string> input)
            => new FileInfo(input.Value).CreationTime.ToString();

        protected override ConverterValue<string> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(string))]
    public class ItemDescriptionConverter : Converter<string, string>
    {
        public static ItemDescriptionConverter Default { get; private set; } = new();
        ItemDescriptionConverter() { }

        protected override ConverterValue<string> ConvertTo(ConverterData<string> input) => Computer.FriendlyDescription(input.Value);

        protected override ConverterValue<string> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(string))]
    public class ItemModifiedConverter : Converter<string, string>
    {
        public static ItemModifiedConverter Default { get; private set; } = new();
        public ItemModifiedConverter() : base() { }

        protected override ConverterValue<string> ConvertTo(ConverterData<string> input)
            => new FileInfo(input.Value).LastWriteTime.ToString();

        protected override ConverterValue<string> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(string))]
    public class ItemSizeConverter : Converter<string, string>
    {
        public static ItemSizeConverter Default { get; private set; } = new();
        public ItemSizeConverter() : base() { }

        protected override ConverterValue<string> ConvertTo(ConverterData<string> input)
            => new FileInfo(input.Value).Length.FileSize(input.ActualParameter is FileSizeFormat i ? i : FileSizeFormat.BinaryUsingSI);

        protected override ConverterValue<string> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(ItemType))]
    public class ItemTypeConverter : Converter<string, ItemType>
    {
        public static ItemTypeConverter Default { get; private set; } = new();
        ItemTypeConverter() { }

        protected override ConverterValue<ItemType> ConvertTo(ConverterData<string> input) => Computer.GetType(input.Value);

        protected override ConverterValue<string> ConvertBack(ConverterData<ItemType> input) => Nothing.Do;
    }

    //...

    [ValueConversion(typeof(string), typeof(string))]
    public class ShortcutLocationConverter : Converter<string, string>
    {
        public static ShortcutLocationConverter Default { get; private set; } = new();
        ShortcutLocationConverter() { }

        protected override ConverterValue<string> ConvertTo(ConverterData<string> input) => Shortcut.TargetPath(input.Value);

        protected override ConverterValue<string> ConvertBack(ConverterData<string> input) => Nothing.Do;
    }

    //...

    [ValueConversion(typeof(string), typeof(double))]
    public class DriveAvailableSizeConverter : Converter<string, double>
    {
        public static DriveAvailableSizeConverter Default { get; private set; } = new();
        DriveAvailableSizeConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<string> input)
        {
            Try.Invoke(out double result, () =>
            {
                foreach (var i in Computer.Drives)
                {
                    if (i.Name.ToLower() == input.Value.ToLower())
                        return i.AvailableFreeSpace.Double();
                }
                return 0;
            });
            return result;
        }

        protected override ConverterValue<string> ConvertBack(ConverterData<double> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(bool))]
    public class DriveSizeLowConverter : Converter<string, bool>
    {
        public static DriveSizeLowConverter Default { get; private set; } = new();
        DriveSizeLowConverter() { }

        protected override ConverterValue<bool> ConvertTo(ConverterData<string> input)
        {
            Try.Invoke(out bool result, () =>
            {
                foreach (var i in Computer.Drives)
                {
                    if (i.Name.ToLower() == input.Value.ToLower())
                        return i.AvailableFreeSpace < 10000000000L;
                }
                return false;
            });
            return result;
        }

        protected override ConverterValue<string> ConvertBack(ConverterData<bool> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(double))]
    public class DriveTotalSizeConverter : Converter<string, double>
    {
        public static DriveTotalSizeConverter Default { get; private set; } = new();
        DriveTotalSizeConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<string> input)
        {
            Try.Invoke(out double result, () =>
            {
                foreach (var i in Computer.Drives)
                {
                    if (i.Name.ToLower() == input.Value.ToLower())
                        return i.TotalSize.Double();
                }
                return 0;
            });
            return result;
        }

        protected override ConverterValue<string> ConvertBack(ConverterData<double> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(double))]
    public class DriveUsedPercentConverter : Converter<string, double>
    {
        public static DriveUsedPercentConverter Default { get; private set; } = new();
        DriveUsedPercentConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<string> input)
        {
            Try.Invoke(out double result, () =>
            {
                foreach (var i in Computer.Drives)
                {
                    if (i.Name.ToLower() == input.Value.ToLower())
                        return i.TotalSize.Double() == 0 ? 0 : (i.TotalSize.Double() - i.AvailableFreeSpace.Double()) / i.TotalSize.Double();
                }
                return 0;
            });
            return result;
        }

        protected override ConverterValue<string> ConvertBack(ConverterData<double> input) => Nothing.Do;
    }

    [ValueConversion(typeof(string), typeof(double))]
    public class DriveUsedSizeConverter : Converter<string, double>
    {
        public static DriveUsedSizeConverter Default { get; private set; } = new();
        DriveUsedSizeConverter() { }

        protected override ConverterValue<double> ConvertTo(ConverterData<string> input)
        {
            Try.Invoke(out double result, () =>
            {
                foreach (var i in Computer.Drives)
                {
                    if (i.Name.ToLower() == input.Value.ToLower())
                        return i.TotalSize.Double() - i.AvailableFreeSpace.Double();
                }
                return 0;
            });
            return result;
        }

        protected override ConverterValue<string> ConvertBack(ConverterData<double> input) => Nothing.Do;
    }

}