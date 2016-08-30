using Imagin.Common;
using System;
using System.Timers;

namespace Imagin.Common
{
    [Serializable]
    ///<summary>
    /// An object with a date that periodically notifies.
    /// </summary>
    public class NamedEntry : Entry, INamable
    {
        #region Properties

        string name = string.Empty;
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        #endregion

        #region Methods
        
        protected override void OnInitialized()
        {
        }

        public override string ToString()
        {
            return this.Name;
        }

        #endregion

        #region NamedEntry

        public NamedEntry()
        {
        }

        public NamedEntry(string Name)
        {
            this.Name = Name;
        }

        public NamedEntry(string Name, int NotifyEvery) : base(NotifyEvery)
        {
            this.Name = Name;
        }

        #endregion
    }
}
