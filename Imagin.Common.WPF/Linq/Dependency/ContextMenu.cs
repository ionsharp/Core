using System;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common.Linq
{
    [Extends(typeof(ContextMenu))]
    public static class XContextMenu
    {
        #region DataType

        public static readonly DependencyProperty DataTypeProperty = DependencyProperty.RegisterAttached("DataType", typeof(Type), typeof(XContextMenu), new FrameworkPropertyMetadata(null));
        public static Type GetDataType(ContextMenu i) => (Type)i.GetValue(DataTypeProperty);
        public static void SetDataType(ContextMenu i, Type input) => i.SetValue(DataTypeProperty, input);

        #endregion

        #region DataKey

        public static readonly DependencyProperty DataKeyProperty = DependencyProperty.RegisterAttached("DataKey", typeof(object), typeof(XContextMenu), new FrameworkPropertyMetadata(null));
        public static object GetDataKey(ContextMenu i) => i.GetValue(DataKeyProperty);
        public static void SetDataKey(ContextMenu i, object input) => i.SetValue(DataKeyProperty, input);

        #endregion
    }
}