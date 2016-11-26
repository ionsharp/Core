using Imagin.Common.Web;
using System;

namespace Imagin.Common.Data
{
    [Serializable]
    public abstract class Condition : AbstractObject
    {
        #region Members

        ServerObjectProperty property;
        public ServerObjectProperty Property
        {
            get
            {
                return property;
            }
            set
            {
                property = value;
                OnPropertyChanged("Property");
            }
        }

        protected object _operator = null;
        public virtual object Operator
        {
            get
            {
                return _operator;
            }
            set
            {
                _operator = value;
                OnPropertyChanged("Operator");
            }
        }

        object value = null;
        public object Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnPropertyChanged("Value");
            }
        }

        #endregion

        #region Virtual Methods

        public virtual bool Evaluate(string Value)
        {
            return default(bool);
        }

        public virtual bool Evaluate(long Value)
        {
            return default(bool);
        }

        public virtual bool Evaluate(DateTime Value)
        {
            return default(bool);
        }

        #endregion

        #region Condition

        public Condition()
        {
        }

        public Condition(ServerObjectProperty Property, object Operator, object Value)
        {
            this.Property = Property;
            this.Operator = Operator;
            this.Value = Value;
        }

        #endregion
    }
}
