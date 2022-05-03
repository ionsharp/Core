namespace Imagin.Common
{
    public static class InternalAssembly
    {
        public const string AbsoluteImagePath 
            = "pack://application:,,,/" + Name + ";component/Images/";

        public const string AbsolutePath 
            = "pack://application:,,,/" + Name + ";component/";

        public const string Name = "Imagin.Common.WPF";

        public static class Space
        {
            public const string Common
                = "Imagin.Common";

            public const string Analytics
                = Common + ".Analytics";

            public const string Behavior
                = Common + ".Behavior";

            public const string Collections
                = Common + ".Collections";

            public const string CollectionsConcurrent
                = Common + ".Collections.Concurrent";

            public const string CollectionsObjectModel
                = Common + ".Collections.ObjectModel";

            public const string CollectionsSerialization
                = Common + ".Collections.Serialization";

            public const string Colors
                = Common + ".Colors";

            public const string Configuration
                = Common + ".Configuration";

            public const string Controls
                = Common + ".Controls";

            public const string Converters
                = Common + ".Converters";

            public const string Data
                = Common + ".Data";

            public const string Effects
                = Common + ".Effects";

            public const string Input
                = Common + ".Input";

            public const string Linq
                = Common + ".Linq";
            
            public const string Local
                = Common + ".Local";

            public const string LocalEngine
                = Common + ".Local.Engine";

            public const string LocalExtensions
                = Common + ".Local.Extensions";

            public const string LocalProviders
                = Common + ".Local.Providers";

            public const string Markup
                = Common + ".Markup";

            public const string Media
                = Common + ".Media";

            public const string MediaAnimation
                = Common + ".Media.Animation";

            public const string Models
                = Common + ".Models";

            public const string Numbers
                = Common + ".Numbers";

            public const string Storage
                = Common + ".Storage";

            public const string Text
                = Common + ".Text";

            public const string Time
                = Common + ".Time";
        }

        public const string Xml = "http://imagin.tech/imagin/wpf";
    }
}