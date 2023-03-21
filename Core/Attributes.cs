using Imagin.Core.Numerics;
using Imagin.Core.Reflection;
using Imagin.Core.Text;
using NLog.Time;
using System;
using System.ComponentModel;
using System.Reflection;

namespace Imagin.Core;

/// Objects | Members

#region [*]

[Obsolete(nameof(Incomplete))]
public class Incomplete : Attribute { }

#endregion

#region Class

#region XmlSerializer

[AttributeUsage(AttributeTargets.Class)]
public class XmlSerializerAttribute : Attribute
{
    public Type Serializer { get; set; }

    public XmlSerializerAttribute() : base() { }
}

#endregion

#endregion

#region Class | Enum | Event | Field | Method | Property | Struct

#region Description

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
public class DescriptionAttribute : LocalizableAttribute
{
    public string Description { get; set; }

    public DescriptionAttribute(string description = "", bool localize = true) : base(localize) => Description = description;
}

#endregion

#region Hide

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Struct)]
public class HideAttribute : Attribute
{
    public HideAttribute() : base() { }
}

#endregion

#region Image

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Struct)]
public class ImageAttribute : Attribute
{
    public readonly string LargeImage;

    public readonly string SmallImage;

    ///

    ImageAttribute() : base() { }

    public ImageAttribute(string smallImage, string largeImage = null) : this(AssemblyType.Current, smallImage, largeImage) { }

    public ImageAttribute(string smallImage, AssemblyType assembly) : this(assembly, smallImage) { }

    public ImageAttribute(AssemblyType assembly, string smallImage, string largeImage = null) : this()
    {
        var i = Resource.GetUri("/Images", assembly).OriginalString;
        SmallImage = $"{i}/{smallImage}"; LargeImage = $"{i}/{largeImage}";
    }

    public ImageAttribute(object smallImage) : this(AssemblyType.Core, $"{smallImage}.png", null) { }

    public ImageAttribute(object smallImage, object largeImage) : this(AssemblyType.Core, $"{smallImage}.png", $"{largeImage}.png") { }
}

#endregion

#region Name

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Struct)]
public class NameAttribute : LocalizableAttribute
{
    public string Name { get; set; }

    public NameAttribute(string name, bool localize = true) : base(localize) => Name = name;
}

#endregion

#region Show

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Struct)]
public class ShowAttribute : Attribute
{
    public ShowAttribute() : base() { }
}

#endregion

#endregion

#region Class | Event | Field | Method | Property | Struct

#region Category

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Struct)]
public class CategoryAttribute : Attribute
{
    public readonly object Category;

    public CategoryAttribute(object category) : base() => Category = category;
}

#endregion

#endregion

#region Class | Field | Method | Property | Struct

#region Options

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Struct)]
public class OptionsAttribute : Attribute
{
    public bool Show { get; set; }

    public OptionsAttribute() : base() { }
}

#endregion

#endregion

#region Class | Field | Property | Struct

#region Categorize

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct, Inherited = true)]
public class CategorizeAttribute : Attribute
{
    public bool Categorize { get; set; }

    public CategorizeAttribute(bool categorize = true) : base() => Categorize = categorize;
}

#endregion

#region Editable

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
public class EditableAttribute : Attribute
{
    public EditableAttribute() : base() { }
}

#endregion

#region HideName

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
public class HideNameAttribute : Attribute
{
    public HideNameAttribute() : base() { }
}

#endregion

#region Horizontal

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
public class HorizontalAttribute : Attribute
{
    public HorizontalAttribute() : base() { }
}

#endregion

#region Index

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
public class IndexAttribute : Attribute
{
    public int Index { get; set; }

    public IndexAttribute(int index = -1) : base() => Index = index;
}

#endregion

#region NonCloneable

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
public class NonCloneableAttribute : Attribute
{
    public NonCloneableAttribute() : base() { }
}

#endregion

#region ReadOnly

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
public class ReadOnlyAttribute : Attribute
{
    public ReadOnlyAttribute() : base() { }
}

#endregion

#region Vertical

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
public class VerticalAttribute : Attribute
{
    public VerticalAttribute() : base() { }
}

#endregion

#region View

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct, Inherited = false)]
public class ViewAttribute : Attribute
{
    public readonly MemberView View = MemberView.All;

    public readonly object ViewParameter = null;

    public ViewAttribute(MemberView view, object viewParameter = null) : base() { View = view; ViewParameter = viewParameter; }
}

#endregion

#region ViewSource

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Struct)]
public class ViewSourceAttribute : Attribute
{
    public bool ShowFilter { get; set; } = false;

    public bool ShowHeader { get; set; } = true;

    public bool ShowOptions { get; set; } = false;

    public bool ShowRoute { get; set; } = true;

    public bool ShowSearch { get; set; } = false;

    public ViewSourceAttribute() : base() { }
}

#endregion

#endregion

#region Class | Struct

#region Base

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = true)]
public class BaseAttribute : Attribute
{
    public readonly Type Type;

    public BaseAttribute(Type Type) : base() => this.Type = Type;
}

#endregion

#region Explicit

/// <summary>All members are hidden unless <see cref="ShowAttribute"/> is specified. If this attribute isn't specified, all members are automatically visible unless <see cref="HideAttribute"/> is specified.</summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false)]
public class ExplicitAttribute : Attribute
{
    public MemberTypes Types { get; set; }

    public ExplicitAttribute(MemberTypes types = MemberTypes.All) : base() => Types = types;
}

#endregion

#region Ignore

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class IgnoreAttribute : Attribute
{
    public readonly string[] Values;

    public IgnoreAttribute(params string[] values) : base() => Values = values;
}

#endregion

#endregion

/// Members

#region Field

#region Layout

public enum MemberLayout { Scroll, Stretch }

[AttributeUsage(AttributeTargets.Field)]
public class LayoutAttribute : Attribute
{
    public readonly MemberLayout Layout;

    public LayoutAttribute(MemberLayout Layout) : base() => this.Layout = Layout;
}

#endregion

#endregion

#region Field | Method | Property

#region Float

public enum Float
{
    Above,
    AboveLeft,
    AboveRight,

    Below,
    BelowLeft,
    BelowRight,
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
public class FloatAttribute : Attribute
{
    public readonly Float Float;

    public FloatAttribute(Float @float) : base() => Float = @float;
}

#endregion

#region Footer

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
public class FooterAttribute : Attribute
{
    public FooterAttribute() : base() { }
}

#endregion

#region Header

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
public class HeaderAttribute : Attribute
{
    public HeaderAttribute() : base() { }
}

#endregion

#region HeaderItem

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
public class HeaderItemAttribute : Attribute
{
    public HeaderItemAttribute() : base() { }
}

#endregion

#region HeaderOption

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
public class HeaderOptionAttribute : Attribute
{
    public HeaderOptionAttribute() : base() { }
}

#endregion

#region Pin

public enum Pin
{
    AboveOrLeft,
    BelowOrRight
}

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property)]
public class PinAttribute : Attribute
{
    public readonly Pin Pin;

    public PinAttribute(Pin pin) : base() => Pin = pin;
}

#endregion

#endregion

#region Field | Property

/// Appearance

#region Caption

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CaptionAttribute : Attribute
{
    public readonly string Caption;

    public CaptionAttribute(string Caption) : base() => this.Caption = Caption;
}

#endregion

#region CheckedImage

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class CheckedImageAttribute : ImageAttribute
{
    public CheckedImageAttribute(string image) : base(image, null) { }

    public CheckedImageAttribute(object image) : base(image) { }
}

#endregion

#region Color

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ColorAttribute : Attribute
{
    public readonly ByteVector4 Color;

    public ColorAttribute(string Color) : base() => this.Color = new(Color);
}

#endregion

#region Content

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ContentAttribute : Attribute<object>
{
    public ContentAttribute(object input) : base(input) { }
}

#endregion

#region Format

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class FormatAttribute : Attribute
{
    public readonly object Format;

    public FormatAttribute(object Format) : base() => this.Format = Format;
}

#endregion

#region Height

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class HeightAttribute : Attribute
{
    public double Maximum { get; set; }

    public double Minimum { get; set; }

    public double Value { get; set; }

    public HeightAttribute(double value, double minimum = double.NaN, double maximum = double.NaN)
    {
        Value
            = value;
        Minimum
            = minimum;
        Maximum
            = maximum;
    }
}

#endregion

#region LeftText

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class LeftTextAttribute : Attribute
{
    public readonly string Text;

    public LeftTextAttribute(string text) : base() => Text = text;
}

#endregion

#region Placeholder

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class PlaceholderAttribute : Attribute<string>
{
    public PlaceholderAttribute(string input) : base(input) { }
}

#endregion

#region RightText

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class RightTextAttribute : Attribute
{
    public readonly string Text;

    public RightTextAttribute(string text) : base() => Text = text;
}

#endregion

#region StringFormat

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class StringFormatAttribute : Attribute
{
    public readonly string Format;

    public StringFormatAttribute(string format)
        : base() => Format = format;
}

#endregion

#region ToolTip

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ToolTipAttribute : TemplateAttribute
{
    public ToolTipAttribute(string propertyName, Type templateKey, string templateKeyName) : base(propertyName, templateKey, templateKeyName) { }
}

#endregion

#region Width

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class WidthAttribute : Attribute
{
    public double Maximum { get; set; }

    public double Minimum { get; set; }

    public double Value { get; set; }

    public WidthAttribute(double value, double minimum = double.NaN, double maximum = double.NaN)
    {
        Value
            = value;
        Minimum
            = minimum;
        Maximum
            = maximum;
    }
}

#endregion

/// Behavior

#region Assign

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AssignAttribute : Attribute
{
    public readonly string Values = null;

    public readonly Type[] Types = null;

    public AssignAttribute(params Type[] Types) : base() => this.Types = Types;

    public AssignAttribute(string Values) : base() => this.Values = Values;
}

#endregion

#region HideIfNull

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class HideIfNullAttribute : Attribute
{
    public HideIfNullAttribute() : base() { }
}

#endregion

#region Lock

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class LockAttribute : Attribute
{
    public readonly bool Locked;

    public LockAttribute() : this(true) { }

    public LockAttribute(bool locked) : base() => Locked = locked;
}

#endregion

#region NameFromValue

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class NameFromValueAttribute : Attribute
{
    public NameFromValueAttribute() : base() { }
}

#endregion

#region Nullable

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class NullableAttribute : Attribute
{
    public NullableAttribute() { }
}

#endregion

#region Require

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class RequireAttribute : Attribute
{
    public RequireAttribute() { }
}

#endregion

#region Reserve

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ReserveAttribute : Attribute
{
    public ReserveAttribute() : base() { }
}

#endregion

#region Update

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class UpdateAttribute : Attribute
{
    public readonly double Seconds;

    public UpdateAttribute(double seconds) : base() => Seconds = seconds;
}

#endregion

#region Validate

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ValidateAttribute : Attribute
{
    public readonly Type Type;

    public ValidateAttribute(Type type) : base() => Type = type;
}

#endregion

/// Filter

#region Option

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class OptionAttribute : Attribute
{
    public OptionAttribute() { }
}

#endregion

#region Page

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class PageAttribute : Attribute
{
    public string Name { get; set; }

    public PageAttribute() : base() { }

    public PageAttribute(object name) : this() => Name = name?.ToString();
}

#endregion

#region Tab

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class TabAttribute : Attribute
{
    public readonly string Name;

    public TabAttribute(object Name) : base() => this.Name = $"{Name}";
}

#endregion

/// Other

#region Abbreviation

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class AbbreviationAttribute : Attribute
{
    public readonly string Abbreviation;

    public AbbreviationAttribute(string abbreviation) : base() => Abbreviation = abbreviation;
}

#endregion

/// Type-specific

#region Range

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class RangeAttribute : Attribute
{
    public readonly object Increment;

    public readonly object Maximum;

    public readonly object Minimum;

    public RangeStyle Style { get; set; } = RangeStyle.Default;

    RangeAttribute(object minimum, object maximum, object increment) : base() { Minimum = minimum; Maximum = maximum; Increment = increment; }

    public RangeAttribute(Byte minimum, Byte maximum, Byte increment)
        : this((object)minimum, maximum, increment) { }

    public RangeAttribute(Decimal minimum, Decimal maximum, Decimal increment)
        : this((object)minimum, maximum, increment) { }

    public RangeAttribute(Double minimum, Double maximum, Double increment)
        : this((object)minimum, maximum, increment) { }

    public RangeAttribute(Int16 minimum, Int16 maximum, Int16 increment)
        : this((object)minimum, maximum, increment) { }

    public RangeAttribute(Int32 minimum, Int32 maximum, Int32 increment)
        : this((object)minimum, maximum, increment) { }

    public RangeAttribute(Int64 minimum, Int64 maximum, Int64 increment)
        : this((object)minimum, maximum, increment) { }

    public RangeAttribute(Single minimum, Single maximum, Single increment)
        : this((object)minimum, maximum, increment) { }

    public RangeAttribute(UDouble minimum, UDouble maximum, UDouble increment)
        : this((object)minimum, maximum, increment) { }

    public RangeAttribute(UInt16 minimum, UInt16 maximum, UInt16 increment)
        : this((object)minimum, maximum, increment) { }

    public RangeAttribute(UInt32 minimum, UInt32 maximum, UInt32 increment)
        : this((object)minimum, maximum, increment) { }

    public RangeAttribute(UInt64 minimum, UInt64 maximum, UInt64 increment)
        : this((object)minimum, maximum, increment) { }

    public RangeAttribute(USingle minimum, USingle maximum, USingle increment)
        : this((object)minimum, maximum, increment) { }
}

#endregion

#region RangeGradient

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class RangeGradientAttribute : Attribute
{
    public readonly string[] Colors;

    public RangeGradientAttribute(params string[] colors) : base() => Colors = colors;
}

#endregion

#endregion

#region Property

#region EnableTrigger

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class EnableTriggerAttribute : OperatorTriggerAttribute
{
    public EnableTriggerAttribute(string Name, Operators Operator = Operators.Equal, object Value = null) : base(Name, Operator, Value) { }
}

#endregion

#region Modify

[AttributeUsage(AttributeTargets.Property)]
public class ModifyAttribute : Attribute
{
    public ModifyAttribute() : base() { }
}

#endregion

#region NonSerializable

/// <summary>Specifies something can or can't be serialized. Alternative to <see cref="SerializableAttribute"/>, which doesn't support properties!</summary>
[AttributeUsage(AttributeTargets.Property)]
public class NonSerializableAttribute : Attribute
{
    public NonSerializableAttribute() : base() { }
}

#endregion

#region OperatorTrigger

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public abstract class OperatorTriggerAttribute : Attribute
{
    public string Name { get; set; }

    public Operators Operator { get; set; }

    public object Value { get; set; }

    public OperatorTriggerAttribute() : base() { }

    public OperatorTriggerAttribute(string name, Operators @operator = Operators.Equal, object value = null) : this()
    {
        Name = name; Operator = @operator; Value = value;
    }
}

#endregion

#region Trigger

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class TriggerAttribute : Attribute
{
    /// <summary>The property to get a value from on the defining object (this can be anything).</summary>
    public readonly string SourceName;

    /// <summary>The property to set a value to (any property defined by <see cref="Controls.MemberModel"/>).</summary>
    public readonly string TargetName;

    public TriggerAttribute(string targetName, string sourceName) : base()
    {
        TargetName = targetName; SourceName = sourceName;
    }
}

#endregion

#region StyleTriggerAttribute

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class StyleTriggerAttribute : Attribute
{
    public string TargetName { get; set; }

    public string LocalName { get; set; }

    public StyleTriggerAttribute(string targetName, string localName) : base() { TargetName = targetName; LocalName = localName; }
}

#endregion

#region VisibilityTrigger

[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public class VisibilityTriggerAttribute : OperatorTriggerAttribute
{
    public VisibilityTriggerAttribute(string Name, Operators Operator, object Value) : base(Name, Operator, Value) { }

    public VisibilityTriggerAttribute(string Name, object Value, Operators Operator = Operators.Equal) : base(Name, Operator, Value) { }
}

#endregion

#endregion

/// Menu

#region Menu

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Enum | AttributeTargets.Struct, Inherited = false)]
public class MenuAttribute : Attribute
{
    /// <summary>Indicates everything in the specified type should be placed in a new item titled "Other" when using <see cref="System.Windows.Controls.Menu"/>.</summary>
    public bool Float { get; set; } = false;

    public object Parent { get; set; } = null;

    public readonly Type[] Types;

    public MenuAttribute(params Type[] types) : base() => Types = types;
}

#endregion

#region MenuItem

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method)]
public class MenuItemAttribute : Attribute
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public object Category { get; set; } = null;

    public int CategoryIndex { get; set; } = 0;

    public int SubCategory { get; set; } = 0;

    ///

    public string Header { get; set; }

    public bool HideIfDisabled { get; set; }

    ///

    public object Icon { get; set; }

    public string IconPath { get; set; }

    public Type IconTemplateSource { get; set; }

    public string IconTemplateKey { get; set; }

    ///

    public int Index { get; set; } = 0;

    ///

    public string InputGestureText { get; set; }

    public string InputGestureTextPath { get; set; }

    public Type InputGestureTextSource { get; set; }

    public string InputGestureTextKey { get; set; }

    ///

    public object Parent { get; set; } = null;

    ///

    public bool CanSlide { get; set; } = false;

    public string SlidePath { get; set; }

    public double SlideMaximum { get; set; }

    public double SlideMinimum { get; set; }

    public double SlideTick { get; set; }

    public string SlideHeader { get; set; }

    ///

    public MenuItemAttribute() : this(null) { }

    public MenuItemAttribute(object parent) : base() => Parent = parent;
}

#endregion

#region MenuItemCollection

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class MenuItemCollectionAttribute : MenuItemAttribute
{
    public ListSortDirection GroupDirection { get; set; }

    public string GroupDirectionPath { get; set; }

    public string GroupName { get; set; }

    public string GroupNamePath { get; set; }

    public string GroupSource { get; set; }

    ///

    public bool IsInline { get; set; } = true;

    ///

    public bool ItemCheckable { get; set; } = false;

    public object ItemCheckableMode { get; set; }

    public string ItemCheckablePath { get; set; }

    ///

    public string ItemCommandName { get; set; }

    public string ItemCommandParameterPath { get; set; }

    ///

    public Type ItemHeaderBinding { get; set; }

    public Type ItemHeaderConverter { get; set; }

    public string ItemHeaderPath { get; set; }

    ///

    public object ItemIcon { get; set; }

    public string ItemIconPath { get; set; }

    public Type ItemIconTemplateSource { get; set; }

    public string ItemIconTemplateKey { get; set; }

    ///

    public Type ItemInputGestureTextConverter { get; set; }

    public string ItemInputGestureTextPath { get; set; }

    public string ItemInputGestureTextTemplateKey { get; set; }

    public Type ItemInputGestureTextTemplateSource { get; set; }

    ///

    public string ItemToolTipPath { get; set; }

    public Type ItemToolTipTemplateSource { get; set; }

    public string ItemToolTipTemplateKey { get; set; }

    ///

    public Type ItemType { get; set; }

    ///

    public ListSortDirection SortDirection { get; set; }

    public string SortDirectionPath { get; set; }

    public string SortName { get; set; }

    public string SortNamePath { get; set; }

    public string SortSource { get; set; }

    ///

    public MenuItemCollectionAttribute() : this(null) { }

    public MenuItemCollectionAttribute(object parent) : base(parent) { }

    public MenuItemCollectionAttribute(object parent, Type itemType, string itemHeaderPath = null, string itemIconPath = null) : this(parent) { ItemType = itemType; ItemHeaderPath = itemHeaderPath; ItemIconPath = itemIconPath; }
}

#endregion

/// Style

#region Style

/// <summary>Applies to (<see cref="Boolean"/>).</summary>
public enum BooleanStyle
{
    Button,
    ToggleButton,
}

/// <summary>Applies to (<see cref="System.Windows.Input.ICommand"/>, <see cref="System.Reflection.MethodInfo"/>).</summary>
public enum ButtonStyle
{
    Normal,
    Default,
    Cancel
}

/// <summary>Applies to (<see cref="System.Collections.IEnumerable"/>, <see cref="System.Collections.IList"/>, <see cref="System.Collections.Specialized.INotifyCollectionChanged"/>).</summary>
public enum CollectionStyle
{
    ToggleButton,
}

/// <summary>Applies to (<see cref="ByteVector4"/>, <see cref="System.Windows.Media.Color"/>, <see cref="System.Windows.Media.SolidColorBrush"/>).</summary>
public enum ColorStyle
{
    Model,
    String
}

/// <summary>Applies to (<see cref="DateTime"/>).</summary>
public enum DateTimeStyle
{
    Default,
    Relative,
    Remaining,
}

/// <summary>Applies to (<see cref="Enum"/>).</summary>
public enum EnumStyle
{
    Flags
}

/// <summary>Applies to (<see cref="Int32"/>).</summary>
public enum Int32Style
{
    Index
}

/// <summary>Applies to (<see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="Single"/>, <see cref="UDouble"/>, <see cref="UInt16"/>, <see cref="UInt32"/>, <see cref="UInt64"/>, <see cref="USingle"/>).</summary>
public enum NumberStyle
{
    Angle,
    Progress,
    ProgressRound,
    Unit
}

/// <summary>Applies to (<see cref="Object"/>).</summary>
public enum ObjectStyle
{
    Button
}

/// <summary>Applies to (<see cref="String"/>); <see cref="StringStyle.ReadOnly"/> applies to (anything)</summary>
public enum StringStyle
{
    Default,
    FilePath,
    FolderPath,
    MultiLine,
    Password,
    Path,
    ReadOnly,
    Resource,
    Search,
    Thumbnail,
    Tokens
}

///

/// <summary>Applies to (<see cref="Byte"/>, <see cref="DateTime"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="Single"/>, <see cref="TimeSpan"/>, <see cref="UDouble"/>, <see cref="UInt16"/>, <see cref="UInt32"/>, <see cref="UInt64"/>, <see cref="USingle"/>).</summary>
[Serializable]
public enum RangeStyle
{
    Default,
    Slider,
    UpDown,
    Both
}

///

[AttributeUsage(AttributeTargets.Event | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
public class StyleAttribute : Attribute
{
    public Enum Style { get; set; }

    ///

    protected StyleAttribute() : base() { }

    ///

    public StyleAttribute(ButtonStyle style) : base() => Style = style;

    public StyleAttribute(BooleanStyle style) : base() => Style = style;

    public StyleAttribute(CollectionStyle style) : base() => Style = style;

    public StyleAttribute(DateTimeStyle style) : base() => Style = style;

    public StyleAttribute(EnumStyle style) : base() => Style = style;

    public StyleAttribute(ObjectStyle style) : base() => Style = style;
}

#endregion

#region CollectionStyle

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class CollectionStyleAttribute : StyleAttribute
{
    public string AddCommand { get; set; }

    public string AddItems { get; set; }

    public Type AddType { get; set; }

    public Type[] AddTypes { get; set; }

    ///

    public Bullets Bullet { get; set; } = Bullets.Square;

    ///

    public bool CanAdd { get; set; } = true;

    public bool CanClear { get; set; } = true;

    public bool CanMove { get; set; } = true;

    public bool CanRemove { get; set; } = true;

    public bool CanSelect { get; set; } = true;

    ///

    public string ItemCommand { get; set; }

    public string ItemPath { get; set; } = ".";

    public object ItemSource { get; set; }

    public Enum ItemStyle { get; set; }

    ///

    public int SelectedIndex { get; set; }

    public object SelectedItem { get; set; }

    public string SelectedItemPropertyName { get; set; }

    ///

    new public readonly CollectionStyle Style;

    ///

    public CollectionStyleAttribute() : base() { }

    public CollectionStyleAttribute(CollectionStyle style) : this() => Style = style;
}

#endregion

#region ColorStyle

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class ColorStyleAttribute : StyleAttribute
{
    public readonly bool Alpha;

    public readonly Type Model;

    public readonly bool Normalize;

    public readonly int Precision;

    ///

    new public readonly ColorStyle Style;

    ///

    public ColorStyleAttribute(ColorStyle style, bool alpha = false) : base()
    {
        Style = style;
        Alpha = alpha;
    }

    public ColorStyleAttribute(ColorStyle style, Type model, bool normalize, int precision) : base()
    {
        Style = style;
        Model = model; Normalize = normalize; Precision = precision;
    }
}

#endregion

#region Int32Style

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class Int32StyleAttribute : StyleAttribute
{
    public readonly string ItemName;

    public readonly string ItemPath;

    public readonly string ItemSource;

    ///

    new public readonly Int32Style Style;

    ///

    public Int32StyleAttribute(Int32Style style, string itemSource, string itemPath = ".", string itemName = null) : base()
    {
        Style = style;
        ItemSource = itemSource; ItemPath = itemPath; ItemName = itemName;
    }
}

#endregion

#region NumberStyle

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class NumberStyleAttribute : StyleAttribute
{
    public readonly AngleUnit AngleUnit;

    public readonly Unit Unit;

    ///

    new public readonly NumberStyle Style;

    ///

    public NumberStyleAttribute(NumberStyle style) : base() => Style = style;

    public NumberStyleAttribute(NumberStyle style, AngleUnit a) : this(style) => AngleUnit = a;

    public NumberStyleAttribute(NumberStyle style, Unit a) : this(style) => Unit = a;
}

#endregion

#region StringStyle

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
public class StringStyleAttribute : StyleAttribute
{
    public Type Converter { get; set; }

    public object ConverterParameter { get; set; }

    public string Path { get; set; }

    public string StringFormat { get; set; }

    public bool CanClear { get; set; } = false;

    public char Delimiter { get; set; } = ';';

    public string EnterCommand { get; set; }

    public Formats Format { get; set; } = Formats.Raw;

    public int Length { get; set; } = 0;

    public bool Select { get; set; } = false;

    public string Suggestions { get; set; }

    public string SuggestionCommand { get; set; }

    public Trimming Trim { get; set; } = Trimming.None;

    public StringType Type { get; set; } = StringType.Any;

    public Wrapping Wrap { get; set; } = Wrapping.None;

    ///

    new public readonly StringStyle Style;

    ///

    public StringStyleAttribute() : base() { }

    public StringStyleAttribute(StringStyle style, char delimiter = ';') : this() { Style = style; Delimiter = delimiter; }
}

#endregion