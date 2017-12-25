using Imagin.Common;
using Imagin.Common.Linq;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public partial class FlagCheckView : ContentControl
    {
        bool FlagsChangeHandled = false;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FlagsProperty = DependencyProperty.Register("Flags", typeof(object), typeof(FlagCheckView), new PropertyMetadata(default(object), OnFlagsChanged));
        /// <summary>
        /// 
        /// </summary>
        public object Flags
        {
            get
            {
                return GetValue(FlagsProperty);
            }
            set
            {
                SetValue(FlagsProperty, value);
            }
        }
        static void OnFlagsChanged(DependencyObject Object, DependencyPropertyChangedEventArgs e)
        {
            Object.As<FlagCheckView>().OnFlagsChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(ObservableCollection<CheckableObject<object>>), typeof(FlagCheckView), new PropertyMetadata(null));
        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<CheckableObject<object>> Items
        {
            get
            {
                return (ObservableCollection<CheckableObject<object>>)GetValue(ItemsProperty);
            }
            private set
            {
                SetValue(ItemsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ItemsPanelProperty = DependencyProperty.Register("ItemsPanel", typeof(ItemsPanelTemplate), typeof(FlagCheckView), new PropertyMetadata(default(ItemsPanelTemplate)));
        /// <summary>
        /// 
        /// </summary>
        public ItemsPanelTemplate ItemsPanel
        {
            get
            {
                return (ItemsPanelTemplate)GetValue(ItemsPanelProperty);
            }
            set
            {
                SetValue(ItemsPanelProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(FlagCheckView), new PropertyMetadata(default(DataTemplate)));
        /// <summary>
        /// 
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get
            {
                return (DataTemplate)GetValue(ItemTemplateProperty);
            }
            set
            {
                SetValue(ItemTemplateProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public FlagCheckView() : base()
        {
            InitializeComponent();
            Items = new ObservableCollection<CheckableObject<object>>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OldValue"></param>
        /// <param name="NewValue"></param>
        protected virtual void OnFlagsChanged(object OldValue, object NewValue)
        {
            //If flags were changed externally
            if (!FlagsChangeHandled)
            {
                //If the old type != new type, clear references to old type
                if (OldValue?.GetType() != NewValue?.GetType())
                {
                    Items.ForEach(i =>
                    {
                        i.Checked -= OnItemChecked;
                        i.Unchecked -= OnItemUnchecked;
                    });

                    Items.Clear();

                    //If new type is enum, get new fields 
                    if (NewValue != null && NewValue.GetType().GetTypeInfo().IsEnum)
                    {
                        EnumExtensions.GetValues(NewValue.GetType()).Where(i =>
                        {
                            var fieldInfo = i.GetType().GetField(i.ToString());
                            return !fieldInfo.HasAttribute<BrowsableAttribute>() || fieldInfo.GetAttribute<BrowsableAttribute>().Browsable;
                        }).ForEach(i =>
                        {
                            var c = new CheckableObject<object>(i, Flags.To<Enum>().Has(i as Enum));
                            c.Checked += OnItemChecked;
                            c.Unchecked += OnItemUnchecked;

                            Items.Add(c);
                        });
                    }
                }
                //Else, if both old and new type is identical, set check states manually
                else Items.ForEach(i => i.IsChecked = Flags.To<Enum>().Has(i.Value as Enum));
            }
            //Internal changes are ignored here
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnItemChecked(object sender, EventArgs e)
        {
            var i = sender as CheckableObject<object>;

            FlagsChangeHandled = true;
            Flags = Flags.To<Enum>().Add(i.Value as Enum);
            FlagsChangeHandled = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void OnItemUnchecked(object sender, EventArgs e)
        {
            var i = sender as CheckableObject<object>;

            FlagsChangeHandled = true;
            Flags = Flags.To<Enum>().Remove(i.Value as Enum);
            FlagsChangeHandled = false;
        }
    }
}