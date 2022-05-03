using Imagin.Common.Local.Extensions;
using System;
using System.Windows;
using System.Windows.Documents;

namespace Imagin.Common.Local.Engine
{
    /// <summary>
    /// An extension of <see cref="T:System.Windows.Documents.Run" /> for displaying localized enums.
    /// </summary>
    public class EnumRun : Run
    {
        /// <summary>
        /// Our own <see cref="LocExtension"/> instance.
        /// </summary>
        private LocExtension _ext;

        #region EnumValue property
        /// <summary>
        /// The EnumValue.
        /// </summary>
        public static readonly DependencyProperty EnumValueProperty = DependencyProperty.Register("EnumValue", typeof(Enum), typeof(EnumRun), new FrameworkPropertyMetadata(PropertiesChanged));

        /// <summary>
        /// The backing property for <see cref="EnumValueProperty"/>
        /// </summary>
        [Category("Common")]
        public Enum EnumValue
        {
            get => (Enum)GetValue(EnumValueProperty);
            set => SetValue(EnumValueProperty, value);
        }
        #endregion

        #region PrependType property
        /// <summary>
        /// This flag determines, if the type should be added using the given separator.
        /// </summary>
        public static readonly DependencyProperty PrependTypeProperty = DependencyProperty.Register("PrependType", typeof(bool), typeof(EnumRun), new FrameworkPropertyMetadata(false, PropertiesChanged));

        /// <summary>
        /// The backing property for <see cref="LocProxy.PrependTypeProperty"/>
        /// </summary>
        [Category("Common")]
        public bool PrependType
        {
            get => (bool)GetValue(PrependTypeProperty);
            set => SetValue(PrependTypeProperty, value);
        }
        #endregion

        #region Separator property
        /// <summary>
        /// The Separator.
        /// </summary>
        public static readonly DependencyProperty SeparatorProperty = DependencyProperty.Register("Separator", typeof(string), typeof(EnumRun), new FrameworkPropertyMetadata("_", PropertiesChanged));

        /// <summary>
        /// The backing property for <see cref="LocProxy.SeparatorProperty"/>
        /// </summary>
        [Category("Common")]
        public string Separator
        {
            get => (string)GetValue(SeparatorProperty);
            set => SetValue(SeparatorProperty, value);
        }
        #endregion

        #region Prefix property
        /// <summary>
        /// The Prefix.
        /// </summary>
        public static readonly DependencyProperty PrefixProperty = DependencyProperty.Register("Prefix", typeof(string), typeof(EnumRun), new FrameworkPropertyMetadata(null, PropertiesChanged));

        /// <summary>
        /// The backing property for <see cref="LocProxy.PrefixProperty"/>
        /// </summary>
        [Category("Common")]
        public string Prefix
        {
            get => (string)GetValue(PrefixProperty);
            set => SetValue(PrefixProperty, value);
        }
        #endregion

        #region PropertiesChanged
        /// <summary>
        /// A notification handler for changed properties.
        /// </summary>
        /// <param name="d">The object.</param>
        /// <param name="e">The event arguments.</param>
        private static void PropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EnumRun run)
            {
                var value = run.EnumValue;
                if (value != null)
                {
                    var key = value.ToString();

                    if (run.PrependType)
                        key = value.GetType().Name + run.Separator + key;
                    if (!string.IsNullOrEmpty(run.Prefix))
                        key = run.Prefix + run.Separator + key;

                    if (run._ext == null)
                    {
                        run._ext = new LocExtension { Key = key };
                        run._ext.SetBinding(run, run.GetType().GetProperty("Text"));
                    }
                    else
                        run._ext.Key = key;
                }
            }
        }
        #endregion
    }
}
