using Imagin.Common.Analytics;
using Imagin.Common.Converters;
using Imagin.Common.Data;
using Imagin.Common.Linq;
using Imagin.Common.Reflection;
using Imagin.Common.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Controls
{
    public abstract class MemberModel : BaseNamable, IComparable
    {
        readonly internal Handle Handle = false;

        System.Timers.Timer timer;

        #region Fields

        public readonly MemberCollection Collection;

        #endregion

        #region Properties

        public MemberAttributes Attributes { get; private set; }

        public MemberSource Source { get; private set; }

        //...

        public abstract bool CanWrite { get; }

        //...

        /// <summary>
        /// To do: Use <see cref="Imagin.Common.Converters.NullConverter"/> to prevent changing <see cref="UpDown{T}.Increment"/> if <see langword="null"/>. It does not currently work as intended...
        /// </summary>
        object DefaultIncrement
        {
            get
            {
                if (Type == typeof(byte))
                    return (byte)1;

                if (Type == typeof(decimal))
                    return (decimal)1;

                if (Type == typeof(double))
                    return (double)1;

                if (Type == typeof(short))
                    return (short)1;

                if (Type == typeof(int))
                    return (int)1;

                if (Type == typeof(long))
                    return (long)1;

                if (Type == typeof(float))
                    return (float)1;

                if (Type == typeof(TimeSpan))
                    return 1.Seconds();

                if (Type == typeof(UDouble))
                    return (UDouble)1;

                if (Type == typeof(ushort))
                    return (ushort)1;

                if (Type == typeof(uint))
                    return (uint)1;

                if (Type == typeof(ulong))
                    return (ulong)1;

                return null;
            }
        }

        /// <summary>
        /// To do: Use <see cref="Imagin.Common.Converters.NullConverter"/> to prevent changing <see cref="UpDown{T}.Maximum"/> if <see langword="null"/>. It does not currently work as intended...
        /// </summary>
        object DefaultMaximum
        {
            get
            {
                if (Type == typeof(byte))
                    return byte.MaxValue;

                if (Type == typeof(decimal))
                    return decimal.MaxValue;

                if (Type == typeof(double))
                    return double.MaxValue;

                if (Type == typeof(short))
                    return short.MaxValue;

                if (Type == typeof(int))
                    return int.MaxValue;

                if (Type == typeof(long))
                    return long.MaxValue;

                if (Type == typeof(float))
                    return float.MaxValue;

                if (Type == typeof(TimeSpan))
                    return TimeSpan.MaxValue;

                if (Type == typeof(UDouble))
                    return UDouble.MaxValue;

                if (Type == typeof(ushort))
                    return ushort.MaxValue;

                if (Type == typeof(uint))
                    return uint.MaxValue;

                if (Type == typeof(ulong))
                    return ulong.MaxValue;

                return null;
            }
        }

        /// <summary>
        /// To do: Use <see cref="Imagin.Common.Converters.NullConverter"/> to prevent changing <see cref="UpDown{T}.Minimum"/> if <see langword="null"/>. It does not currently work as intended...
        /// </summary>
        object DefaultMinimum
        {
            get
            {
                if (Type == typeof(byte))
                    return byte.MinValue;

                if (Type == typeof(decimal))
                    return decimal.MinValue;

                if (Type == typeof(double))
                    return double.MinValue;

                if (Type == typeof(short))
                    return short.MinValue;

                if (Type == typeof(int))
                    return int.MinValue;

                if (Type == typeof(long))
                    return long.MinValue;

                if (Type == typeof(float))
                    return float.MinValue;

                if (Type == typeof(TimeSpan))
                    return TimeSpan.MinValue;

                if (Type == typeof(UDouble))
                    return UDouble.MinValue;

                if (Type == typeof(ushort))
                    return ushort.MinValue;

                if (Type == typeof(uint))
                    return uint.MinValue;

                if (Type == typeof(ulong))
                    return ulong.MinValue;

                return null;
            }
        }

        //...

        Type actualType;
        public Type ActualType
        {
            get => actualType;
            private set => this.Change(ref actualType, value);
        }

        public Type BaseType 
            => Type?.BaseType;

        public virtual Type DeclaringType 
            => Member?.DeclaringType;

        public Type TemplateType => ActualType is Type i ? GetTemplateType(i) : null;

        public abstract Type Type { get; }

        //...

        ObservableCollection<Type> assignableTypes = null;
        public ObservableCollection<Type> AssignableTypes
        {
            get => assignableTypes;
            private set => this.Change(ref assignableTypes, value);
        }

        string category = null;
        public string Category
        {
            get => category;
            set => this.Change(ref category, value);
        }

        ICommand command = null;
        public ICommand Command
        {
            get => command;
            private set => this.Change(ref command, value);
        }

        object content = null;
        public object Content
        {
            get => content;
            private set => this.Change(ref content, value);
        }

        IValueConverter converter = null;
        public IValueConverter Converter
        {
            get => converter;
            private set => this.Change(ref converter, value);
        }

        object converterParameter = null;
        public object ConverterParameter
        {
            get => converterParameter;
            private set => this.Change(ref converterParameter, value);
        }

        char delimiter = ';';
        public char Delimiter
        {
            get => delimiter;
            set => this.Change(ref delimiter, value);
        }

        int depth = 0;
        public int Depth
        {
            get => depth;
            set => this.Change(ref depth, value);
        }

        string description = null;
        public string Description
        {
            get => description;
            private set => this.Change(ref description, value);
        }

        string displayName = null;
        public virtual string DisplayName
        {
            get => displayName;
            set => this.Change(ref displayName, value);
        }

        FileSizeFormat fileSizeFormat = FileSizeFormat.BinaryUsingSI;
        public FileSizeFormat FileSizeFormat
        {
            get => fileSizeFormat;
            set => this.Change(ref fileSizeFormat, value);
        }

        object format = default;
        public virtual object Format
        {
            get => format;
            private set => this.Change(ref format, value);
        }

        double height = double.NaN;
        public double Height
        {
            get => height;
            private set => this.Change(ref height, value);
        }

        string icon = null;
        public string Icon
        {
            get => icon;
            private set => this.Change(ref icon, value);
        }

        string iconColor = null;
        public string IconColor
        {
            get => iconColor;
            private set => this.Change(ref iconColor, value);
        }

        dynamic increment = default;
        public object Increment
        {
            get => increment;
            set => this.Change(ref increment, value);
        }

        int index = 0;
        public int Index
        {
            get => index;
            private set => this.Change(ref index, value);
        }

        bool isEnabled = true;
        public virtual bool IsEnabled
        {
            get => isEnabled;
            private set => this.Change(ref isEnabled, value);
        }

        bool isFeatured = false;
        public bool IsFeatured
        {
            get => isFeatured;
            private set => this.Change(ref isFeatured, value);
        }
        
        bool isIndeterminate = false;
        public bool IsIndeterminate
        {
            get => isIndeterminate;
            private set => this.Change(ref isIndeterminate, value);
        }

        public bool IsIndeterminable 
            => PropertyGrid.IndeterminableTypes.Contains(TemplateType);

        bool isLockable = false;
        public bool IsLockable
        {
            get => isLockable;
            private set => this.Change(ref isLockable, value);
        }

        bool isLocked = false;
        public bool IsLocked
        {
            get => isLocked;
            internal set => this.Change(ref isLocked, value);
        }

        bool isReadOnly = false;
        public virtual bool IsReadOnly
        {
            get => isReadOnly;
            private set => this.Change(ref isReadOnly, value);
        }

        bool isTool = false;
        public bool IsTool
        {
            get => isTool;
            private set => this.Change(ref isTool, value);
        }

        string itemPath = ".";
        public string ItemPath
        {
            get => itemPath;
            private set => this.Change(ref itemPath, value);
        }

        object itemSource = null;
        public object ItemSource
        {
            get => itemSource;
            private set => this.Change(ref itemSource, value);
        }

        Enum itemStyle = null;
        public Enum ItemStyle
        {
            get => itemStyle;
            private set => this.Change(ref itemStyle, value);
        }

        Type itemType = null;
        public Type ItemType
        {
            get => itemType;
            private set => this.Change(ref itemType, value);
        }

        ListCollectionView itemTypes = null;
        public ListCollectionView ItemTypes
        {
            get => itemTypes;
            private set => this.Change(ref itemTypes, value);
        }

        bool label = true;
        public bool Label
        {
            get => label;
            private set => this.Change(ref label, value);
        }
        
        bool localize = true;
        public bool Localize
        {
            get => localize;
            private set => this.Change(ref localize, value);
        }

        dynamic maximum = default;
        public object Maximum
        {
            get => maximum;
            set => this.Change(ref maximum, value);
        }

        double maximumHeight = double.NaN;
        public double MaximumHeight
        {
            get => maximumHeight;
            private set => this.Change(ref maximumHeight, value);
        }

        double maximumWidth = double.NaN;
        public double MaximumWidth
        {
            get => maximumWidth;
            private set => this.Change(ref maximumWidth, value);
        }

        MemberInfo member;
        public virtual MemberInfo Member
        {
            get => member;
            private set
            {
                this.Change(ref member, value);
                MemberType = member?.MemberType ?? MemberTypes.Custom;
                Name = member?.Name;
            }
        }

        MemberCollection members = null;
        public MemberCollection Members
        {
            get => members;
            set => this.Change(ref members, value);
        }

        MemberTypes memberType;
        public MemberTypes MemberType
        {
            get => memberType;
            private set => this.Change(ref memberType, value);
        }

        dynamic minimum = default;
        public object Minimum
        {
            get => minimum;
            set => this.Change(ref minimum, value);
        }

        double minimumHeight = double.NaN;
        public double MinimumHeight
        {
            get => minimumHeight;
            private set => this.Change(ref minimumHeight, value);
        }

        double minimumWidth = double.NaN;
        public double MinimumWidth
        {
            get => minimumWidth;
            private set => this.Change(ref minimumWidth, value);
        }

        string placeholder = null;
        public string Placeholder
        {
            get => placeholder;
            private set => this.Change(ref placeholder, value);
        }

        string stringFormat = null;
        public string StringFormat
        {
            get => stringFormat;
            private set => this.Change(ref stringFormat, value);
        }

        object style = null;
        public virtual object Style
        {
            get => style;
            private set => this.Change(ref style, value);
        }

        object suggestions = null;
        public object Suggestions
        {
            get => suggestions;
            private set => this.Change(ref suggestions, value);
        }

        ICommand suggestionCommand = null;
        public ICommand SuggestionCommand
        {
            get => suggestionCommand;
            private set => this.Change(ref suggestionCommand, value);
        }

        UpdateSourceTrigger updateSourceTrigger = UpdateSourceTrigger.Default;
        public UpdateSourceTrigger UpdateSourceTrigger
        {
            get => updateSourceTrigger;
            private set => this.Change(ref updateSourceTrigger, value);
        }

        IValidate validateHandler = null;
        public IValidate ValidateHandler
        {
            get => validateHandler;
            set => this.Change(ref validateHandler, value);
        }

        dynamic value = default;
        public object Value
        {
            get => value;
            set => OnValueChanging(this.value, value);
        }

        double width = double.NaN;
        public double Width
        {
            get => width;
            private set => this.Change(ref width, value);
        }

        //...

        #endregion

        #region MemberModel

        public MemberModel(MemberData data) : base()
        {
            Collection
                = data.Collection;
            Source
                = data.Source;
            Member 
                = data.Member;

            Attributes = data.Attributes;
            Apply(Attributes);
        }

        #endregion

        #region Methods

        void OnContentTrigger(object sender, PropertyChangedEventArgs e)
        {
            var contentTrigger = Attributes.Get<ContentTriggerAttribute>();
            if (e.PropertyName == contentTrigger.PropertyName)
            {
                Try.Invoke(() =>
                {
                    var result = sender.GetPropertyValue(e.PropertyName);
                    Content = contentTrigger.Format?.F(result) ?? result;
                });
            }
        }

        void OnPropertyTrigger(object sender, PropertyChangedEventArgs e)
        {
            var propertyTrigger = Attributes.Get<PropertyTriggerAttribute>();
            if (e.PropertyName == propertyTrigger.SourceName)
            {
                Try.Invoke(() =>
                {
                    var result = sender.GetPropertyValue(e.PropertyName);
                    this.SetPropertyValue(propertyTrigger.TargetName, result);
                },
                e => Log.Write<MemberModel>(e));
            }
        }
        
        void OnEnableTrigger(object sender, PropertyChangedEventArgs e)
        {
            var enableTrigger = Attributes.Get<EnableTriggerAttribute>();
            if (e.PropertyName == enableTrigger.Property)
            {
                Try.Invoke(() =>
                {
                    var result = sender.GetPropertyValue(e.PropertyName);
                    IsEnabled = result?.Equals(enableTrigger.Value) == true;
                });
            }
        }

        void OnItemSourceChanged(object sender, PropertyChangedEventArgs e)
        {
            var source = Attributes.Get<SourceAttribute>();
            if (e.PropertyName == source.ItemSource)
                Try.Invoke(() => ItemSource = sender.GetPropertyValue(e.PropertyName));
        }

        void OnUpdate(object sender, System.Timers.ElapsedEventArgs e)
        {
            UpdateValue();
        }

        //...

        int IComparable.CompareTo(object a)
        {
            if (a is MemberModel b)
                return Index.CompareTo(b.Index);

            return 0;
        }

        //...

        protected T Info<T>() where T : MemberInfo => (T)Member;

        protected virtual void Apply(MemberAttributes input)
        {
            if (Type != null)
            {
                AssignableTypes
                   = input?.Get<AssignableAttribute>()?.Types is Type[] assignableTypes && assignableTypes.Length > 0
                   ? new(assignableTypes)
                   : PropertyGrid.AssignableTypes.ContainsKey(Type)
                       ? new(PropertyGrid.AssignableTypes[Type])
                       : new();
            }

            Category
                = input.Get<CategoryAttribute>()?.Category
                ?? input.Get<System.ComponentModel.CategoryAttribute>()?.Category;

            if (input.Get<CommandAttribute>()?.CommandName is string commandName)
                Try.Invoke(() => Command = Source.First.GetPropertyValue(commandName) as ICommand, e => Log.Write<MemberModel>(e));

            Content = input.Get<ContentAttribute>()?.Content;

            var contentTrigger = input.Get<ContentTriggerAttribute>();
            if (contentTrigger?.PropertyName != null)
                OnContentTrigger(Source.First, new(contentTrigger.PropertyName));

            Try.Invoke(() => Converter = input.Get<ConvertAttribute>()?.Converter.Create<IValueConverter>(), e => Log.Write<MemberModel>(e));
            ConverterParameter
                = input.Get<ConvertAttribute>()?.ConverterParameter;

            Delimiter
                = input.Get<DelimitAttribute>()?.Character ?? Delimiter;

            Description
                = input.Get<DescriptionAttribute>()?.Description
                ?? input.Get<System.ComponentModel.DescriptionAttribute>()?.Description;

            DisplayName
                = input.Get<DisplayNameAttribute>()?.DisplayName
                ?? input.Get<System.ComponentModel.DisplayNameAttribute>()?.DisplayName
                ?? Name;

            IsFeatured
                = input.Get<FeaturedAttribute>()?.Featured ?? IsFeatured;

            IsTool
                = input.Get<ToolAttribute>() != null;

            Format
                = input.Get<FormatAttribute>()?.Format ?? Format;

            var enableTrigger = input.Get<EnableTriggerAttribute>();
            if (enableTrigger?.Property != null)
                OnEnableTrigger(Source.First, new(enableTrigger.Property));

            Height
                = input.Get<HeightAttribute>()?.Height ?? Height;

            Icon
                = input.Get<IconAttribute>()?.Icon;
            IconColor
                = input.Get<IconAttribute>()?.Color ?? IconColor;

            Increment
                = input.Get<RangeAttribute>()?.Increment ?? DefaultIncrement;

            Index
                = input.Get<IndexAttribute>()?.Index ?? Index;

            IsReadOnly
                = input.Get<ReadOnlyAttribute>()?.ReadOnly == true
                || input.Get<System.ComponentModel.ReadOnlyAttribute>()?.IsReadOnly == true
                || !CanWrite;

            ItemPath
                = input.Get<SourceAttribute>()?.ItemPath;

            var sourceAttribute = input.Get<SourceAttribute>();
            if (sourceAttribute?.ItemSource != null)
                Try.Invoke(() => ItemSource = Source.First.GetPropertyValue(sourceAttribute.ItemSource.ToString()), e => Log.Write<MemberModel>(e));

            ItemStyle
                = input.Get<ItemStyleAttribute>()?.Style ?? ItemStyle;

            if (input.Get<ItemTypeAttribute>()?.ItemType is Type itemType)
                ItemType = itemType;

            if (GetValue() is ITypes iTypes)
            {
                ItemTypes = new(new ObservableCollection<Type>(iTypes.GetTypes()));
                ItemTypes.CustomSort = TypeComparer.Default;
                ItemTypes.GroupDescriptions.Add(new PropertyGroupDescription() { Converter = CategoryConverter.Default });
            }

            Label
                = input.Get<LabelAttribute>()?.Label ?? Label;

            Localize
                = input.Get<LocalizeAttribute>()?.Localize ?? Localize;

            IsLockable
                = input.Get<LockedAttribute>()?.Locked ?? IsLockable;

            Maximum
                = input.Get<RangeAttribute>()?.Maximum ?? DefaultMaximum;

            MaximumHeight
                = input.Get<HeightAttribute>()?.MaximumHeight ?? MaximumHeight;

            MaximumWidth
                = input.Get<WidthAttribute>()?.MaximumWidth ?? MaximumWidth;

            Minimum
                = input.Get<RangeAttribute>()?.Minimum ?? DefaultMinimum;

            MinimumHeight
                = input.Get<HeightAttribute>()?.MinimumHeight ?? MinimumHeight;

            MinimumWidth
                = input.Get<WidthAttribute>()?.MinimumWidth ?? MinimumWidth;

            Placeholder
                = input.Get<PlaceholderAttribute>()?.Placeholder;

            StringFormat
                = input.Get<StringFormatAttribute>()?.Format;

            Style
                = input.Get<StyleAttribute>()?.Style;

            if (input.Get<SuggestionsAttribute>()?.SourceName is string suggestions)
            {
                Suggestions = Source.First.GetPropertyValue(suggestions);
                if (Suggestions != null)
                {
                    if (input.Get<SuggestionsAttribute>().CommandName is string suggestionCommand)
                        SuggestionCommand = Source.First.GetPropertyValue(suggestionCommand) as ICommand;
                }
            }

            UpdateSourceTrigger
                = input.Get<UpdateSourceTriggerAttribute>()?.UpdateSourceTrigger ?? UpdateSourceTrigger;

            IValidate validateHandler = null;
            Try.Invoke(() => validateHandler = input.Get<ValidateAttribute>()?.Type.Create<IValidate>(), e => Log.Write<MemberModel>(e));
            ValidateHandler = validateHandler;

            Width
                = input.Get<WidthAttribute>()?.Width ?? Width;

            //... Property triggers

            var propertyTrigger = input.Get<PropertyTriggerAttribute>();
            if (propertyTrigger != null)
                OnPropertyTrigger(Source.First, new(propertyTrigger.SourceName));
        }

        //...

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case nameof(Value):
                    ActualType = value?.GetType();
                    this.Changed(() => TemplateType);
                    break;
            }
        }

        //...

        void OnValueChanging(object oldValue, object newValue)
        {
            if (!isReadOnly)
            {
                Unsubscribe();
                SetValue(newValue);

                UpdateValue(newValue);
                switch (Source.DataType)
                {
                    case Types.Value:

                        if (Source.Parent?.Name?.Length > 0)
                        {
                            var member = Source.Parent.Value.GetType().GetMember(Source.Parent.Name).First();
                            if (member != null)
                            {
                                if (member is FieldInfo a)
                                    a.SetValue(Source.Parent.Value, Source.First);

                                if (member is PropertyInfo b)
                                    b.SetValue(Source.Parent.Value, Source.First);
                            }
                        }
                        break;
                }

                OnValueChanged(newValue);
                Subscribe();
            }
        }

        protected virtual void OnValueChanged(object input) { }

        //...

        public object GetValue()
        {
            object result = null;
            Dispatch.Invoke(() =>
            {
                Try.Invoke(() =>
                {
                    IsIndeterminate = false;
                    result = GetValue(Source.First);
                    for (var i = 1; i < Source.Count; i++)
                    {
                        var next = GetValue(Source[i]);
                        if (result?.Equals(next) == false)
                        {
                            IsIndeterminate = true;
                            result = null;
                            break;
                        }
                    }
                });
            });
            return result;
        }

        protected abstract object GetValue(object input);

        public IEnumerable<object> GetValues()
        {
            foreach (var i in Source)
                yield return i;

            yield break;
        }

        //...

        protected void SetValue(object value) => Handle.Invoke(() =>
        {
            foreach (var i in Source)
                SetValue(i, value);
        });

        protected abstract void SetValue(object source, object value);

        //...

        internal void UpdateValue() => UpdateValue(GetValue());

        internal void UpdateValue(object input) => this.Change(ref value, input, () => Value);

        //...

        internal void UpdateSource(MemberSource input)
        {
            Unsubscribe();
            Source = input;

            UpdateValue();
            Subscribe();
        }

        //...

        public virtual void Subscribe()
        {
            var a = TemplateType == typeof(object) || GetTemplateType(Type) == typeof(object);
            var b = Style?.Equals(ObjectStyle.Expander) == true;

            var value = Value;
            if (a && b && value != null)
            {
                Members ??= new(Collection.ParentControl, this, Depth + 1);
                _ = Members.Reload(MemberCollection.LoadType.Recreate, new(new MemberRouteSource(value), null), MemberFilter.Field | MemberFilter.Property, null, false, null);
            }

            foreach (var i in Source)
            {
                if (i is INotifyPropertyChanged j)
                {
                    if (Attributes.Get<ContentTriggerAttribute>() != null)
                        j.PropertyChanged += OnContentTrigger;

                    if (Attributes.Get<EnableTriggerAttribute>() != null)
                        j.PropertyChanged += OnEnableTrigger;

                    if (Attributes.Get<PropertyTriggerAttribute>() != null)
                        j.PropertyChanged += OnPropertyTrigger;
                    
                    if (Attributes.Get<SourceAttribute>() != null)
                        j.PropertyChanged += OnItemSourceChanged;
                }
            }

            if (Attributes.Get<UpdateAttribute>() is UpdateAttribute updateAttribute)
            {
                timer = new() { Interval = updateAttribute.Seconds * 1000 };
                timer.Elapsed += OnUpdate;
                timer.Start();
            }
        }

        public virtual void Unsubscribe()
        {
            foreach (var i in Source)
            {
                if (i is INotifyPropertyChanged j)
                {
                    if (Attributes.Get<ContentTriggerAttribute>() != null)
                        j.PropertyChanged -= OnContentTrigger;

                    if (Attributes.Get<EnableTriggerAttribute>() != null)
                        j.PropertyChanged -= OnEnableTrigger;

                    if (Attributes.Get<PropertyTriggerAttribute>() != null)
                        j.PropertyChanged -= OnPropertyTrigger;

                    if (Attributes.Get<SourceAttribute>() != null)
                        j.PropertyChanged -= OnItemSourceChanged;
                }
            }

            if (timer != null)
            {
                timer.Stop();
                timer.Elapsed -= OnUpdate;
                timer.Dispose();
            }

            Members?.ForEach(i => i.Unsubscribe());
        }

        //...

        public static Type GetTemplateType(Type input)
        {
            if (input == null)
                return null;

            if (input.IsNullable())
                input = Enumerable.FirstOrDefault(input.GetGenericArguments());

            if (PropertyGrid.DefaultTypes.Contains(input))
                return input;

            if (input.IsArray)
                return typeof(Array);

            if (input.IsEnum)
                return typeof(Enum);

            if (input.Implements<ICommand>())
                return typeof(ICommand);

            //The order here is important!
            if (input.Implements<INotifyCollectionChanged>())
                return typeof(INotifyCollectionChanged);

            if (input.Implements<IList>())
                return typeof(IList);

            if (input.Implements<IEnumerable>())
                return typeof(IEnumerable);

            return typeof(object); //input.IsClass || input.IsValueType
        }

        #endregion
    }
}