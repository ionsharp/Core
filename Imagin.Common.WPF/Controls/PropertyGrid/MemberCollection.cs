using Imagin.Common.Collections.Concurrent;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Imagin.Common.Controls
{
    public class MemberCollection : ConcurrentCollection<MemberModel>, ISubscribe, IUnsubscribe
    {
        #region (class) Cache

        static readonly object cache = new();

        class Cache : Dictionary<Type, CacheData>
        {
            public static readonly Cache Current = new();

            Cache() : base() { }
        }

        class CacheData : Dictionary<MemberInfo, MemberAttributes>
        {
            public CacheData() : base() { }
        }

        #endregion

        #region (enum) LoadType

        internal enum LoadType
        {
            Recreate,
            Update
        }

        #endregion

        #region Fields

        internal readonly PropertyGrid ParentControl;

        internal readonly MemberModel ParentMember;

        #endregion

        #region Properties

        public MemberSource Source { get; private set; }

        //...

        bool loading = false;
        public bool Loading
        {
            get => loading;
            set => this.Change(ref loading, value);
        }

        #endregion

        #region MemberCollection

        public readonly int Depth = 0;

        internal MemberCollection(PropertyGrid propertyGrid) : base() => ParentControl = propertyGrid;

        internal MemberCollection(PropertyGrid propertyGrid, MemberModel parent, int depth) : this(propertyGrid)
        {
            ParentMember 
                = parent;
            Depth 
                = depth;
        }

        internal MemberModel this[string memberName] => this.FirstOrDefault(i => i.Name == memberName);

        #endregion

        #region Methods

        //... (new)

        new void Add(MemberModel i) => base.Add(i);

        new internal void Clear()
        {
            Unsubscribe();
            base.Clear();

            Source = null;
        }

        new void Insert(int index, MemberModel i) => base.Insert(index, i);

        new void Remove(MemberModel i) => base.Remove(i);

        new void RemoveAt(int i) => base.RemoveAt(i);

        //... (private)

        /// <summary>
        /// If <see cref="FieldInfo"/>, must be public. If <see cref="PropertyInfo"/>, must have <see langword="public"/> getter (with <see langword="internal"/>, <see langword="private"/>, <see langword="protected"/>, or <see langword="public"/> setter).
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        bool Supported(MemberInfo input)
        {
            if (input is FieldInfo a)
            {
                if (PropertyGrid.ForbiddenTypes.Contains(a.FieldType))
                    return false;

                return a.IsPublic;
            }

            if (input is PropertyInfo b)
            {
                if (PropertyGrid.ForbiddenTypes.Contains(b.PropertyType))
                    return false;

                return b.GetGetMethod(false) != null;
            }

            return false;
        }

        //...

        async Task Load(LoadType loadType, MemberFilter filter, Type filterAttribute, bool filterAttributeIgnore, Action<MemberModel> onAdded)
        {
            await Task.Run(() =>
            {
                switch (loadType)
                {
                    case LoadType.Recreate:

                        var visibility
                            = Source.SharedType.GetAttribute<MemberVisibilityAttribute>() ?? new();

                        lock (cache)
                        {
                            if (!Cache.Current.ContainsKey(Source.SharedType))
                            {
                                var data = new CacheData();
                                foreach (var i in GetMembers(Source.SharedType))
                                {
                                    if (!Supported(i))
                                        continue;

                                    bool isImplicit(MemberInfo j)
                                        => !j.HasAttribute<BrowsableAttribute>() && !j.HasAttribute<HiddenAttribute>() && !j.HasAttribute<VisibleAttribute>();

                                    switch (i.MemberType)
                                    {
                                        case MemberTypes.Field:
                                            if (visibility.Field == MemberVisibility.Explicit)
                                            {
                                                if (isImplicit(i))
                                                    continue;
                                            }
                                            break;

                                        case MemberTypes.Property:
                                            if (visibility.Property == MemberVisibility.Explicit)
                                            {
                                                if (isImplicit(i))
                                                    continue;
                                            }
                                            break;

                                        default: continue;
                                    }

                                    var attributes = new MemberAttributes(i);
                                    if (attributes.Hidden)
                                        continue;

                                    data.Add(i, new(i));
                                }
                                Cache.Current.Add(Source.SharedType, data);
                            }
                        }

                        foreach (var i in Cache.Current[Source.SharedType])
                        {
                            switch (i.Key.MemberType)
                            {
                                case MemberTypes.Field:
                                    if (!filter.HasFlag(MemberFilter.Field))
                                        continue;

                                    break;

                                case MemberTypes.Property:
                                    if (!filter.HasFlag(MemberFilter.Property))
                                        continue;

                                    break;

                                default: continue;
                            }

                            if (filterAttribute != null)
                            {
                                if (!filterAttributeIgnore)
                                {
                                    if (!i.Value.ContainsKey(filterAttribute))
                                        continue;
                                }
                                else
                                {
                                    if (i.Value.ContainsKey(filterAttribute))
                                        continue;
                                }
                            }

                            var data = new MemberData(this, Source, i.Key, i.Value);

                            MemberModel result = null;
                            if (i.Key is FieldInfo field)
                            {
                                var templateType = MemberModel.GetTemplateType(field.FieldType);

                                result = templateType == typeof(INotifyCollectionChanged)
                                ? new ListFieldModel(data)
                                : new FieldModel(data);
                            }

                            else if (i.Key is PropertyInfo property)
                            {
                                var templateType = MemberModel.GetTemplateType(property.PropertyType);

                                result = templateType == typeof(INotifyCollectionChanged)
                                ? new ListPropertyModel(data)
                                : new PropertyModel(data);
                            }

                            result.Depth = Depth;
                            result.UpdateValue();

                            Add(result);
                            Dispatch.Invoke(() => onAdded?.Invoke(result));
                        }
                        break;

                    case LoadType.Update:
                        this.ForEach(i => Dispatch.Invoke(() => i.UpdateSource(Source)));
                        break;
                }
            });
        }

        void Load(IDictionary input, Action<MemberModel> onAdded)
        {
            foreach (DictionaryEntry i in input)
            {
                if (i.Value != null)
                {
                    var memberData = new MemberData(this, Source, null, null);

                    var result = new EntryModel(memberData) { Name = i.Key.ToString() };
                    result.UpdateValue(i.Value);
                    
                    Add(result);
                    Dispatch.Invoke(() => onAdded?.Invoke(result));
                }
            }
        }

        //...

        void OnSourceChanged(object sender, PropertyChangedEventArgs e) 
            => this.FirstOrDefault(i => i.Name == e.PropertyName)?.If(i => i.Handle.SafeInvoke(() => i.UpdateValue()));

        void OnSourceLocked(object sender, LockedEventArgs e)
        {
            if (e.IsLocked)
            {
                foreach (var i in this)
                {
                    if (i.IsLockable)
                        i.IsLocked = true;
                }
            }
            else
            {
                var result = false;
                foreach (var i in Source)
                {
                    if (i is ILock j)
                    {
                        if (j.IsLocked)
                        {
                            result = true;
                            break;
                        }
                    }
                }
                if (!result)
                {
                    foreach (var i in this)
                    {
                        if (i.IsLockable)
                            i.IsLocked = false;
                    }
                }
            }
        }

        //... (internal)

        internal MemberSource GetSource(MemberRouteElement input)
        {
            var a = ParentControl.Route.LastOrDefault(0) as MemberRouteElement;
            var b = ParentControl.Route.LastOrDefault(1) as MemberRouteElement;
            return new MemberSource(input, input.Name == null || b == null ? null : new MemberSource.Ancestor(a.Name, b.Value));
        }

        internal async Task Reload(LoadType type, MemberSource source, MemberFilter filter, Type filterAttribute, bool filterAttributeIgnore, Action<MemberModel> onAdded)
        {
            Loading = true;

            if (type == LoadType.Recreate)
                Clear();

            if (type == LoadType.Update)
            {
                for (var i = Count - 1; i >= 0; i--)
                {
                    if (this[i] is EntryModel j)
                    {
                        j.Unsubscribe();
                        RemoveAt(i);
                    }
                }
            }

            Source = source;
            if (filter != MemberFilter.None)
            {
                if (filter.HasFlag(MemberFilter.Field) || filter.HasFlag(MemberFilter.Property))
                    await Load(type, filter, filterAttribute, filterAttributeIgnore, onAdded);

                if (Source.Count == 1)
                {
                    await Task.Run(() =>
                    {
                        if (filter.HasFlag(MemberFilter.Entry))
                        {
                            if (Source.First is IDictionary dictionary)
                                Load(dictionary, onAdded);
                        }
                    });
                }
            }

            Subscribe();
            Loading = false;
        }

        internal async Task Recreate(MemberFilter filter, Type filterAttribute, bool filterAttributeIgnore, Action<MemberModel> onAdded)
            => await Reload(LoadType.Recreate, Source, filter, filterAttribute, filterAttributeIgnore, onAdded);

        //... (public)

        public void Refresh() => this.ForEach(i => i.UpdateValue());

        public void Subscribe()
        {
            this.ForEach(i =>
            {
                i.Unsubscribe();
                i.Subscribe();
            });
            if (Source?.Count > 0)
            {
                foreach (var i in Source)
                {
                    i.If<ILock>(j =>
                    {
                        j.Locked -= OnSourceLocked;
                        j.Locked += OnSourceLocked;
                    });
                    i.If<INotifyPropertyChanged>(j =>
                    {
                        j.PropertyChanged -= OnSourceChanged;
                        j.PropertyChanged += OnSourceChanged;
                    });
                }
            }
        }

        public void Unsubscribe()
        {
            this.ForEach(i => i.Unsubscribe());
            if (Source?.Count > 0)
            {
                foreach (var i in Source)
                {
                    i.If<ILock>
                        (j => j.Locked -= OnSourceLocked);
                    i.If<INotifyPropertyChanged>
                        (j => j.PropertyChanged -= OnSourceChanged);
                }
            }
        }

        //...

        public static IEnumerable<MemberInfo> GetMembers(Type input) => input.GetMembers(BindingFlags.Instance | BindingFlags.Public).Select(i => i).Where(i =>
        {
            if (i is FieldInfo field && !field.IsStatic)
                return true;

            if (i is PropertyInfo property && !property.GetAccessors(true)[0].IsStatic)
                return true;

            return false;
        });

        #endregion
    }
}