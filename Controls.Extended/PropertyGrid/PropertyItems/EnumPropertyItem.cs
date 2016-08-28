using System;
using System.Reflection;
using System.Collections.ObjectModel;
using Imagin.Common.Collections;

namespace Imagin.Controls.Extended
{
    public sealed class EnumPropertyItem : PropertyItem
    {
        private ObservableCollection<object> items = null;
        public ObservableCollection<object> Items
        {
            get
            {
                return items;
            } set
            {
                items = value;
                OnPropertyChanged("Itmes");
            }
        }
        
        private void GetItems(PropertyInfo Info)
        {
            this.Items = new ObservableCollection<object>();
            foreach (FieldInfo FieldInfo in Info.PropertyType.GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                if (FieldInfo == null)
                    continue;
                this.Items.Add(Enum.Parse(Info.PropertyType, FieldInfo.Name));
            }
        }

        #region Override Methods

        public override void SetValue(object NewValue)
        {
            if (this.Info != null)
                this.Info.SetValue(SelectedObject, NewValue, null);
        }

        #endregion

        public EnumPropertyItem(object SelectedObject, PropertyInfo Info, string Name, object Value, string Category, bool IsReadOnly, bool IsPrimary = false) : base(SelectedObject, Name, Value, Category, IsReadOnly, IsPrimary)
        {
            this.Type = PropertyType.Enum;
            this.GetItems(Info);
        }
    }
}
