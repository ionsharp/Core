using Imagin.Common.Configuration;
using Imagin.Common.Media;
using Imagin.Common.Models;
using Imagin.Common.Numbers;
using Imagin.Common.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public partial class PropertyGrid
    {
        #region DefaultTypes

        /// <summary>
        /// Types that have a default visual representation.
        /// </summary>
        public static List<Type> DefaultTypes = new()
        {
            typeof(Alignment),
            typeof(Array),
            typeof(bool),
            typeof(Bullets),
            typeof(byte),
            typeof(CardinalDirection),
            typeof(System.Drawing.Color),
            typeof(System.Windows.Media.Color),
            typeof(DateTime),
            typeof(decimal),
            typeof(double),
            typeof(DoubleMatrix),
            typeof(Enum),
            typeof(float),
            typeof(FontFamily),
            typeof(FontStyle),
            typeof(FontWeight),
            typeof(Gradient),
            typeof(GradientStepCollection),
            typeof(GraphicUnit),
            typeof(Guid),
            typeof(Hexadecimal),
            typeof(ICommand),
            typeof(IEnumerable),
            typeof(IList),
            typeof(int),
            typeof(Int32Pattern),
            typeof(ItemCollection),
            typeof(long),
            typeof(Layouts),
            typeof(LinearGradientBrush),
            typeof(ListCollectionView),
            typeof(NetworkCredential),
            typeof(object),
            typeof(PanelCollection),
            typeof(PointCollection),
            typeof(RadialGradientBrush),
            typeof(short),
            typeof(SolidColorBrush),
            typeof(string),
            typeof(StringColor),
            typeof(ApplicationResources),
            typeof(Thickness),
            typeof(TimeSpan),
            typeof(TimeZoneInfo),
            typeof(Type),
            typeof(UDouble),
            typeof(UIElementCollection),
            typeof(uint),
            typeof(ulong),
            typeof(Uri),
            typeof(ushort),
            typeof(Version),
            typeof(VisualCollection),
        };

        #endregion

        #region AssignableTypes

        /// <summary>
        /// Types that can be assigned other (derived) types.
        /// </summary>
        public static Dictionary<Type, Type[]> AssignableTypes = new()
        {
            { typeof(Brush), Array<Type>.New(typeof(LinearGradientBrush), typeof(RadialGradientBrush), typeof(SolidColorBrush)) }
        };

        #endregion

        #region ForbiddenTypes

        /// <summary>
        /// Types to avoid.
        /// </summary>
        public static List<Type> ForbiddenTypes = new()
        {
            typeof(ItemCollection),
            typeof(UIElementCollection),
            typeof(VisualCollection),
        };

        #endregion

        #region IndeterminableTypes

        /// <summary>
        /// Types where an indeterminable state can be represented visually.
        /// </summary>
        public static List<Type> IndeterminableTypes = new()
        {
            typeof(bool),
            typeof(byte),
            typeof(DateTime),
            typeof(decimal),
            typeof(double),
            typeof(Enum),
            typeof(FontFamily),
            typeof(FontStyle),
            typeof(FontWeight),
            typeof(Guid),
            typeof(ICommand),
            typeof(short),
            typeof(int),
            typeof(long),
            typeof(object),
            typeof(float),
            typeof(string),
            typeof(TimeSpan),
            typeof(TimeZoneInfo),
            typeof(Type),
            typeof(UDouble),
            typeof(ushort),
            typeof(uint),
            typeof(ulong),
            typeof(Uri),
            typeof(Version),
        };

        #endregion
    }
}