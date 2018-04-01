using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Imagin.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class FlagCheckView : System.Windows.Controls.UserControl
    {
        bool FlagsChangeHandled = false;

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FlagsProperty = DependencyProperty.Register("Flags", typeof(object), typeof(FlagCheckView), new FrameworkPropertyMetadata(default(object), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnFlagsChanged));
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
        public static DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(TCollection<CheckableObject<object>>), typeof(FlagCheckView), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// 
        /// </summary>
        public TCollection<CheckableObject<object>> Items
        {
            get
            {
                return (TCollection<CheckableObject<object>>)GetValue(ItemsProperty);
            }
            protected set
            {
                SetValue(ItemsProperty, value);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty ItemsPanelProperty = DependencyProperty.Register("ItemsPanel", typeof(ItemsPanelTemplate), typeof(FlagCheckView), new FrameworkPropertyMetadata(default(ItemsPanelTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
        public static DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(FlagCheckView), new FrameworkPropertyMetadata(default(DataTemplate), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
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
            DefaultStyleKey = typeof(FlagCheckView);
            Items = new TCollection<CheckableObject<object>>();
            Items.ItemAdded += OnItemAdded;
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
                    Items.Clear();

                    //If new type is enum, get new fields 
                    if (NewValue != null && NewValue.GetType().IsEnum)
                        NewValue.GetType().GetEnumValues().Where(i => !i.As<Enum>().HasAttribute<BrowsableAttribute>() || i.As<Enum>().GetAttribute<BrowsableAttribute>().Browsable).ForEach(i => Items.Add(new CheckableObject<object>(i, Flags.To<Enum>().Has(i as Enum))));
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
        protected virtual void OnItemAdded(object sender, EventArgs<CheckableObject<object>> e)
        {
            e.Value.Checked += OnItemChecked;
            e.Value.Unchecked += OnItemUnchecked;
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
