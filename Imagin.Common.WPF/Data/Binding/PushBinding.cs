using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace Imagin.Common.Data
{
    #region (class) PushBindingManager

    /// <summary>
    /// Original source code: Fredrik Hedblad (https://meleak.wordpress.com/2011/08/28/onewaytosource-binding-for-readonly-dependency-property/)
    /// </summary>
    public class PushBindingManager
    {
        const string PushBindingsInternal 
            = "PushBindingsInternal";

        const string StylePushBindings 
            = "StylePushBindings";

        //...

        static readonly DependencyPropertyKey PushBindingsPropertyKey = DependencyProperty.RegisterAttachedReadOnly(PushBindingsInternal, typeof(PushBindingCollection), typeof(PushBindingManager), new UIPropertyMetadata(null));
        public static DependencyProperty PushBindingsProperty = PushBindingsPropertyKey.DependencyProperty;
        public static PushBindingCollection GetPushBindings(DependencyObject sender)
        {
            PushBindingCollection pushBindings = null;
            if (sender.GetValue(PushBindingsProperty) == null)
            {
                pushBindings = new PushBindingCollection(sender);
                SetPushBindings(sender, pushBindings);
            }
            return pushBindings;
        }
        static void SetPushBindings(DependencyObject sender, PushBindingCollection input)
        {
            sender.SetValue(PushBindingsPropertyKey, input);
            ((INotifyCollectionChanged)input).CollectionChanged += OnCollectionChanged;
        }

        public static DependencyProperty StylePushBindingsProperty = DependencyProperty.RegisterAttached(StylePushBindings, typeof(PushBindingCollection), typeof(PushBindingManager), new UIPropertyMetadata(null, OnStylePushBindingsChanged));
        public static PushBindingCollection GetStylePushBindings(DependencyObject sender) => (PushBindingCollection)sender.GetValue(StylePushBindingsProperty);
        public static void SetStylePushBindings(DependencyObject sender, PushBindingCollection input) => sender.SetValue(StylePushBindingsProperty, input);
        static void OnStylePushBindingsChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender != null)
            {
                if (e.OldValue != null)
                {
                    ((INotifyCollectionChanged)e.OldValue).CollectionChanged -= OnStyleCollectionChanged;
                    TryRemoveStylePushBindings(GetPushBindings(sender), e.OldValue as FreezableCollection<PushBinding>, false);
                }
                if (e.NewValue != null)
                {
                    ((INotifyCollectionChanged)e.NewValue).CollectionChanged += OnStyleCollectionChanged;
                    AddStylePushBindings(GetPushBindings(sender), e.NewValue as FreezableCollection<PushBinding>);
                }
            }
        }

        //...

        static void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {

            var pushBindings = (PushBindingCollection)sender;

            switch (e.Action)

            {

                case NotifyCollectionChangedAction.Add:

                    if (e.NewItems != null)

                        foreach (PushBinding item in e.NewItems)

                        {

                            item.TargetObject = pushBindings.TargetObject;

                            item.Id = GetId(pushBindings);

                            item.SetupTargetBinding();

                        }

                    break;

                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:

                    if (e.OldItems != null)

                        TryRemoveStylePushBindings(GetStylePushBindings(pushBindings.TargetObject), e.OldItems, true);

                    break;

                case NotifyCollectionChangedAction.Replace:

                    TryReplaceStylePushBindings(GetStylePushBindings(pushBindings.TargetObject), e.OldItems, e.NewItems, true);

                    break;

                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex == e.NewStartingIndex) break;
                    int difference = e.OldStartingIndex - e.NewStartingIndex;
                    int id;
                    FreezableCollection<PushBinding> styleBehaviors = GetStylePushBindings(pushBindings.TargetObject);
                    foreach (PushBinding item in e.OldItems)
                    {
                        id = item.Id;
                        item.Id -= difference;
                        foreach (PushBinding styleItem in styleBehaviors)
                            if (styleItem.Id == id)
                                styleItem.Id = item.Id;
                    }
                    void updateId(int startIndex, int length)
                    {
                        int count = length + startIndex;
                        for (int i = startIndex; i < count; i++)
                        {
                            id = pushBindings[startIndex].Id;
                            pushBindings[startIndex].Id += startIndex + 1;
                            foreach (PushBinding styleItem in styleBehaviors)
                                if (styleItem.Id == id)
                                    styleItem.Id = pushBindings[startIndex].Id;
                        }
                    }
                    int _startIndex;
                    if (e.NewStartingIndex < e.OldStartingIndex)
                    {
                        _startIndex = e.NewStartingIndex + e.OldItems.Count;
                        updateId(_startIndex, e.OldStartingIndex + e.OldItems.Count - _startIndex + 1);
                    }
                    else
                    {
                        _startIndex = e.OldStartingIndex;
                        updateId(_startIndex, e.NewStartingIndex - _startIndex + 1);
                    }
                    break;
                default:
                    break;
            }

        }

        static void OnStyleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var pushBindings = (PushBindingCollection)sender;
            switch (e.Action)
            {
                //when an item(s) is added we need to set the Owner property implicitly
                case NotifyCollectionChangedAction.Add:
                    if (e.NewItems != null)

                        AddStylePushBindings(GetPushBindings(pushBindings.TargetObject), e.NewItems);

                    break;

                //when an item(s) is removed we should Dispose the BehaviorBinding
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    if (e.OldItems != null)

                        TryRemoveStylePushBindings(GetPushBindings(pushBindings.TargetObject), e.OldItems, false);

                    break;

                //here we have to set the owner property to the new item and unregister the old item
                case NotifyCollectionChangedAction.Replace:

                    TryReplaceStylePushBindings(GetPushBindings(pushBindings.TargetObject), e.OldItems, e.NewItems, false);

                    break;

                case NotifyCollectionChangedAction.Move:

                    throw new InvalidOperationException("Move is not supported for style behaviors");

                default:
                    break;
            }
        }

        //...

        static void AddStylePushBindings(PushBindingCollection pushBindings, IList pushBindingsToAdd)

        {

            foreach (PushBinding pushBinding in pushBindingsToAdd)

            {

                var _pushBinding = pushBinding.Clone() as PushBinding;

                pushBindings.Add(_pushBinding as PushBinding);

                pushBinding.Id = _pushBinding.Id;

            }

        }

        static int GetId(PushBindingCollection sourceCollection) => sourceCollection.Count == 1 ? 1 : sourceCollection[sourceCollection.Count - 1].Id + 1;

        static void TryRemoveStylePushBindings(PushBindingCollection pushBindings, IList pushBindingsToRemove, bool disposePushBindings)

        {

            if (disposePushBindings)

                foreach (PushBinding pushBinding in pushBindingsToRemove)

                    pushBinding.Dispose();

            if (pushBindings != null)

                for (int i = 0; i < pushBindingsToRemove.Count; i++)

                    for (int j = 0; j < pushBindings.Count; j++)

                        if (((PushBinding)pushBindingsToRemove[i]).Id == pushBindings[j].Id)

                            pushBindings.RemoveAt(j);

        }

        static void TryReplaceStylePushBindings(PushBindingCollection pushBindings, IList oldPushBindings, IList newPushBindings, bool disposePushBindings)

        {

            for (int i = 0; i < oldPushBindings.Count; i++)

            {

                var oldItem = (PushBinding)oldPushBindings[i];
                var newItem = (PushBinding)newPushBindings[i];
                var clonedPushBinding = newItem.Clone() as PushBinding;

                newItem.TargetObject = clonedPushBinding.TargetObject = pushBindings.TargetObject;
                newItem.Id = oldItem.Id;

                if (disposePushBindings)

                    oldItem.Dispose();

                for (int j = 0; j < pushBindings.Count; j++)

                    if (pushBindings[j].Id == oldItem.Id)

                        pushBindings[j] = clonedPushBinding;

            }

        }
    }

    #endregion

    #region (class) PushBindingCollection

    /// <summary>
    /// Original source code: Fredrik Hedblad (https://meleak.wordpress.com/2011/08/28/onewaytosource-binding-for-readonly-dependency-property/)
    /// </summary>
    public class PushBindingCollection : FreezableCollection<PushBinding>
    {
        public PushBindingCollection() { }

        public PushBindingCollection(DependencyObject targetObject) => TargetObject = targetObject;

        public DependencyObject TargetObject { get; private set; }
    }

    #endregion

    #region (class) PushBinding

    /// <summary>
    /// Original source code: Fredrik Hedblad (https://meleak.wordpress.com/2011/08/28/onewaytosource-binding-for-readonly-dependency-property/)
    /// </summary>
    public class PushBinding : FreezableBinding, IDisposable
    {
        #region Properties

        public static DependencyProperty TargetPropertyMirrorProperty = DependencyProperty.Register(nameof(TargetPropertyMirror), typeof(object), typeof(PushBinding));
        public object TargetPropertyMirror
        {
            get => GetValue(TargetPropertyMirrorProperty);
            set => SetValue(TargetPropertyMirrorProperty, value);
        }

        public static DependencyProperty TargetPropertyListenerProperty = DependencyProperty.Register(nameof(TargetPropertyListener), typeof(object), typeof(PushBinding), new UIPropertyMetadata(null, OnTargetPropertyListenerChanged));
        public object TargetPropertyListener
        {
            get => GetValue(dp: TargetPropertyListenerProperty);
            set => SetValue(TargetPropertyListenerProperty, value);
        }
        static void OnTargetPropertyListenerChanged(object sender, DependencyPropertyChangedEventArgs e) => (sender as PushBinding).TargetPropertyValueChanged();


        [DefaultValue(null)]
        public string TargetProperty { get; set; }

        [DefaultValue(null)]
        public DependencyProperty TargetDependencyProperty { get; set; }

        #endregion

        #region PushBinding

        public PushBinding() => Mode = BindingMode.OneWayToSource;

        #endregion

        #region Methods

        void DependencyObject_Loaded(object sender, RoutedEventArgs e) => TargetPropertyValueChanged();

        void TargetPropertyValueChanged() => SetValue(TargetPropertyMirrorProperty, GetValue(TargetPropertyListenerProperty));

        public void Dispose()
        {
            BindingOperations.ClearBinding(this, TargetPropertyListenerProperty);
            BindingOperations.ClearBinding(this, TargetPropertyMirrorProperty);

            if (TargetObject is FrameworkElement _targetObject)
                _targetObject.Loaded -= DependencyObject_Loaded;

            else if (TargetObject is FrameworkContentElement __targetObject)
                __targetObject.Loaded -= DependencyObject_Loaded;
        }

        public void SetupTargetBinding()
        {
            // Prevent the designer from reporting exceptions since
            // changes will be made of a Binding in use if it is set

            if (TargetObject == null || DesignerProperties.GetIsInDesignMode(this))
                return;

            // Bind to the selected TargetProperty, e.g. ActualHeight and get
            // notified about changes in OnTargetPropertyListenerChanged
            var listenerBinding = new Binding
            {
                Source = TargetObject,
                Mode = BindingMode.OneWay
            };

            listenerBinding.Path = TargetDependencyProperty == null ? new PropertyPath(TargetProperty) : new PropertyPath(TargetDependencyProperty);

            _ = BindingOperations.SetBinding(this, TargetPropertyListenerProperty, listenerBinding);

            // Set up a OneWayToSource Binding with the Binding declared in Xaml from
            // the Mirror property of this class. The mirror property will be updated
            // everytime the Listener property gets updated
            _ = BindingOperations.SetBinding(this, TargetPropertyMirrorProperty, Binding);
            TargetPropertyValueChanged();

            if (TargetObject is FrameworkElement _targetObject)
                _targetObject.Loaded += DependencyObject_Loaded;

            else if (TargetObject is FrameworkContentElement __targetObject)
                __targetObject.Loaded += DependencyObject_Loaded;

        }

        #endregion

        #region Freezable

        protected override void CloneCore(Freezable sourceFreezable)
        {
            var pushBinding = sourceFreezable as PushBinding;
            TargetProperty = pushBinding.TargetProperty;
            TargetDependencyProperty = pushBinding.TargetDependencyProperty;
            base.CloneCore(sourceFreezable);
        }

        protected override Freezable CreateInstanceCore() => new PushBinding();

        #endregion
    }

    #endregion
}