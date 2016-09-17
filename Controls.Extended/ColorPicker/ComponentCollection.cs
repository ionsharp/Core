using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Imagin.Controls.Extended
{
    public class ComponentCollection : ObservableCollection<ComponentModel>
    {
        public ComponentModel this[Type Type]
        {
            get
            {
                return this.Where(x => x.GetType() == Type).First();
            }
            set
            {
                for (int i = 0; i < this.Count; i++)
                {
                    if (this[i].GetType() == Type)
                    {
                        this[i] = value;
                        break;
                    }
                }
            }
        }

        public ComponentCollection() : base()
        {
        }
    }
}
